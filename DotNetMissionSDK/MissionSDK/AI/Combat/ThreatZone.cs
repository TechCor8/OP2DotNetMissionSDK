using DotNetMissionSDK.Async;
using DotNetMissionSDK.Pathfinding;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.Units;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Combat
{
	public enum ThreatLevel
	{
		None,		// No enemies in the area
		Unarmed,	// Enemies in the area are unarmed
		Armed		// Enemies have weapons in the area
	}

	/// <summary>
	/// Represents an area on the map that needs military intervention.
	/// NOTE: ThreatZone is immutable and safe for async operations.
	/// </summary>
	public class ThreatZone
	{
		private const int StagingAreaBorderWidth		= 3;

		private PlayerState m_Owner;

		public MAP_RECT bounds				{ get; private set; }
		public int strengthRequired			{ get; private set; }
		public int strengthDesired			{ get; private set; }
		public ThreatLevel threatLevel		{ get; private set; }
		public UnitState[] priorityTargets	{ get; private set; }
		public MAP_RECT[] stagingAreas		{ get; private set; }

		// Optimization variables
		private MAP_RECT m_StagingBounds;


		/// <summary>
		/// Constructor for ThreatZone.
		/// </summary>
		/// <param name="state">The snapshot to use to generate the zone.</param>
		/// <param name="ownerID">The bot player that owns this zone.</param>
		/// <param name="bounds">The map area of the zone.</param>
		/// <param name="priorityTargets">The primary targets to attack.</param>
		/// <param name="additionalStrengthDesired">Additional strength beyond what is required to control this zone.</param>
		/// <param name="groupType">The type of combat group to organize.</param>
		public ThreatZone(StateSnapshot state, int ownerID, MAP_RECT bounds, UnitState[] priorityTargets, int additionalStrengthDesired)
		{
			m_Owner = state.players[ownerID];
			this.bounds = bounds;

			// Get enemies in zone
			List<UnitState> enemies = state.unitMap.GetUnitsInArea(bounds);
			enemies.RemoveAll((UnitState tileUnit) => m_Owner.allyPlayerIDs.Contains(tileUnit.ownerID));

			// Calculate enemy strength
			foreach (UnitState unit in enemies)
			{
				if (unit.hasWeapon)
					strengthRequired += state.weaponInfo[unit.weapon].weaponStrength;
			}

			if (strengthRequired > 0)
				threatLevel = ThreatLevel.Armed;
			else if (enemies.Count > 0)
				threatLevel = ThreatLevel.Unarmed;
			else
				threatLevel = ThreatLevel.None;

			strengthDesired = strengthRequired + additionalStrengthDesired;

			this.priorityTargets = priorityTargets;

			// Calculate staging area
			stagingAreas = bounds.GetBorder(StagingAreaBorderWidth);

			m_StagingBounds = bounds;
			m_StagingBounds.Inflate(StagingAreaBorderWidth);
			m_StagingBounds.ClipToMap();
		}

		/// <summary>
		/// Returns true if the zone contains the position.
		/// </summary>
		public bool Contains(LOCATION position)
		{
			return bounds.Contains(position);
		}

		/// <summary>
		/// Is the position in the staging area, but not in the threat zone?
		/// </summary>
		public bool IsInStagingArea(LOCATION position)
		{
			return m_StagingBounds.Contains(position) && !bounds.Contains(position);
		}

		/// <summary>
		/// Returns the closest priority target to the specified position.
		/// </summary>
		public UnitState GetClosestPriorityTarget(LOCATION position)
		{
			UnitState closestTarget = null;
			int closestDistance = int.MaxValue;

			foreach (UnitState target in priorityTargets)
			{
				int distance = target.position.GetDiagonalDistance(position);
				if (distance < closestDistance)
				{
					closestTarget = target;
					closestDistance = distance;
				}
			}

			return closestTarget;
		}

		/// <summary>
		/// Sends the unit to the zone's staging area.
		/// NOTE: Requires main thread.
		/// </summary>
		public void SendUnitToStagingArea(StateSnapshot stateSnapshot, Vehicle unitForPath)
		{
			ThreadAssert.MainThreadRequired();

			int unitStrength = unitForPath.GetUnitInfo().GetWeaponStrength();

			unitForPath.DoMoveWithPathfinder((x,y) => GetTileCost(x,y, unitStrength, stateSnapshot), IsTileInStagingArea);
		}

		private int GetTileCost(int x, int y, int unitStrength, StateSnapshot state)
		{
			if (!state.tileMap.IsTilePassable(x,y))
				return Pathfinder.Impassable;

			// Buildings and units are impassable
			if (state.unitMap.GetUnitOnTile(new LOCATION(x,y)) != null)
				return Pathfinder.Impassable;

			// Get tile movement speed cost
			int moveCost = state.tileMap.GetTileMovementCost(x,y);

			// Get enemy strength at tile
			int enemyStrength = 0;
			foreach (int enemyID in m_Owner.enemyPlayerIDs)
				enemyStrength += state.strengthMap.GetPlayerStrength(enemyID, x,y);

			// If tile has enemy strength greater than our own, avoid it.
			if (enemyStrength > unitStrength)
				return Pathfinder.Impassable;
			else if (enemyStrength > 0)
				moveCost += 4; // Try to navigate around enemy, but go ahead and engage if no choice.

			// Perform distance heuristic for performance.
			// Do not use heuristic if inside the threat zone, as this will increase tile cost when trying to move away from the center point.
			if (!bounds.Contains(x,y))
			{
				LOCATION centerPt = bounds.position + (bounds.size / 2);
				return Pathfinder.Heuristic_Diagonal(new LOCATION(x,y), centerPt, moveCost);
			}

			return moveCost;
		}

		private bool IsTileInStagingArea(int x, int y)
		{
			MAP_RECT stagingBounds = m_StagingBounds;
			stagingBounds.ClipToMap();

			return stagingBounds.Contains(x, y);
		}
	}
}
