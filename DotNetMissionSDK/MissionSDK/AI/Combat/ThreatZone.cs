using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Pathfinding;
using DotNetMissionSDK.Units;
using DotNetMissionSDK.Utility;
using DotNetMissionSDK.Utility.Maps;
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
	/// </summary>
	public class ThreatZone
	{
		private const int StagingAreaBorderWidth		= 3;

		private PlayerInfo m_Owner;

		public MAP_RECT bounds				{ get; private set; } // *See Sync Notes
		public int strengthRequired			{ get; private set; }
		public int strengthDesired			{ get; private set; }
		public ThreatLevel threatLevel		{ get; private set; }
		public UnitEx[] priorityTargets		{ get; private set; }
		public MAP_RECT[] stagingAreas		{ get; private set; }

		// Optimization variables
		private MAP_RECT m_StagingBounds; // *See Sync Notes

		// *Sync Notes:
		// Variables marked with this notice should not be modified outside of the constructor.
		// These variables are not locked and the pathfinder thread will read these at unknown times.
		// If these variables need to be changed, a new ThreatZone should be created.


		/// <summary>
		/// Constructor for ThreatZone.
		/// </summary>
		/// <param name="owner">The bot player that owns this zone.</param>
		/// <param name="bounds">The map area of the zone.</param>
		/// <param name="priorityTargets">The primary targets to attack.</param>
		/// <param name="additionalStrengthDesired">Additional strength beyond what is required to control this zone.</param>
		/// <param name="groupType">The type of combat group to organize.</param>
		public ThreatZone(PlayerInfo owner, MAP_RECT bounds, UnitEx[] priorityTargets, int additionalStrengthDesired)
		{
			m_Owner = owner;
			this.bounds = bounds;

			// Get enemies in zone
			List<UnitEx> enemies = PlayerUnitMap.GetUnitsInArea(bounds);
			enemies.RemoveAll((UnitEx tileUnit) => owner.player.IsAlliedTo(tileUnit.GetOwner()));

			// Calculate enemy strength
			foreach (UnitEx unit in enemies)
				strengthRequired += unit.GetWeaponInfo().GetWeaponStrength();

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
		/// Returns true if the zone contains the unit.
		/// </summary>
		public bool Contains(Unit unit)
		{
			return bounds.Contains(unit.GetPosition());
		}

		/// <summary>
		/// Is the unit in the staging area, but not in the threat zone?
		/// </summary>
		public bool IsInStagingArea(Unit unit)
		{
			return m_StagingBounds.Contains(unit.GetPosition()) && !Contains(unit);
		}

		public bool IsInStagingArea(LOCATION position)
		{
			return m_StagingBounds.Contains(position) && !bounds.Contains(position);
		}

		/// <summary>
		/// Returns the closest priority target to the specified position.
		/// </summary>
		public UnitEx GetClosestPriorityTarget(LOCATION position)
		{
			UnitEx closestTarget = null;
			int closestDistance = int.MaxValue;

			foreach (UnitEx target in priorityTargets)
			{
				int distance = target.GetPosition().GetDiagonalDistance(position);
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
		/// </summary>
		public void SendUnitToStagingArea(Vehicle unitForPath)
		{
			int unitStrength = unitForPath.GetUnitInfo().GetWeaponStrength();

			unitForPath.DoMoveWithPathfinder((x,y) => GetTileCost(x,y,unitStrength), IsTileInStagingArea);
		}

		private int GetTileCost(int x, int y, int unitStrength)
		{
			if (!GameMap.IsTilePassable(x,y))
				return Pathfinder.Impassable;

			// Buildings and units are impassable
			if (PlayerUnitMap.GetUnitOnTile(new LOCATION(x,y)) != null)
				return Pathfinder.Impassable;

			// Get tile movement speed cost
			int moveCost = GameMap.GetTileMovementCost(x,y);

			// Get enemy strength at tile
			int enemyStrength = 0;
			foreach (PlayerInfo info in m_Owner.enemies)
				enemyStrength += PlayerStrengthMap.GetPlayerStrength(info.player.playerID, x,y);

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
