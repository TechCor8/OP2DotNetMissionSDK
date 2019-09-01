using DotNetMissionSDK.AI.Managers;
using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.Async;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Pathfinding;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.Mining
{
	/// <summary>
	/// Finds beacons outside of a command center's control area and builds a new command center.
	/// All bases must be fully saturated before a new base is created.
	/// </summary>
	public class CreateCommonMiningBaseTask : Task
	{
		private MiningBaseState m_MiningBaseState;

		private CreateCommonMineTask m_CreateMineTask;
		private MaintainCommandCenterTask m_MaintainCCTask;


		public CreateCommonMiningBaseTask(int ownerID, MiningBaseState miningBaseState) : base(ownerID)	{ m_MiningBaseState = miningBaseState; }


		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			// Task is not complete until every CC beacon has been occupied and saturated
			if (!m_CreateMineTask.IsTaskComplete(stateSnapshot))
				return false;

			PlayerState owner = stateSnapshot.players[ownerID];

			// Task is complete if there are no beacons outside of a command center's control area
			foreach (MiningBeaconState beacon in stateSnapshot.gaia.miningBeacons)
			{
				if (beacon.oreType != BeaconType.Common)
					continue;

				// Detect if occupied
				bool isOccupied = false;
				foreach (PlayerState player in stateSnapshot.players)
				{
					StructureState building = GetClosestBuildingOfType(player, map_id.Any, beacon.position);
					if (building != null && building.position.GetDiagonalDistance(beacon.position) < MiningBaseState.MaxMineDistanceToCC)
					{
						isOccupied = true;
						break;
					}
				}

				if (isOccupied)
					continue;

				// Build more mining bases
				m_MaintainCCTask.targetCountToMaintain = owner.units.commandCenters.Count + 1;
				SetBaseLocation(stateSnapshot);

				return false;
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(m_CreateMineTask = new CreateCommonMineTask(ownerID, m_MiningBaseState));
			AddPrerequisite(m_MaintainCCTask = new MaintainCommandCenterTask(ownerID), true);
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			return true;
		}

		private bool SetBaseLocation(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Get idle convec with CC
			ConvecState convec = owner.units.convecs.FirstOrDefault((unit) => unit.cargoType == map_id.CommandCenter && unit.curAction == ActionType.moDone);
			if (convec == null)
				return true;

			// Find unoccupied common beacon
			MiningBeaconState unoccupiedBeacon = GetClosestUnusedBeacon(stateSnapshot, convec.position);
			if (unoccupiedBeacon != null)
				return DeployCC(stateSnapshot, convec, unoccupiedBeacon.position);

			return true;
		}

		private bool DeployCC(StateSnapshot stateSnapshot,  ConvecState convec, LOCATION targetPosition)
		{
			// Callback for determining if tile is a valid place point
			Pathfinder.ValidTileCallback validTileCB = (int x, int y) =>
			{
				// Ignore tiles within X distance of beacon
				if (targetPosition.GetDiagonalDistance(new LOCATION(x,y)) < 6)
					return false;

				// Get area to deploy structure
				GlobalStructureInfo info = stateSnapshot.structureInfo[convec.cargoType];
				LOCATION size = info.GetSize(true);
				MAP_RECT targetArea = new MAP_RECT(x-size.x+1, y-size.y+1, size.x,size.y);

				if (!BuildStructureTask.AreTilesPassable(stateSnapshot, targetArea, x, y))
					return false;

				// Check if area is blocked by structure or enemy
				if (BuildStructureTask.IsAreaBlocked(stateSnapshot, targetArea, ownerID))
					return false;

				return true;
			};

			// Find open location near beacon
			LOCATION foundPt;
			if (!Pathfinder.GetClosestValidTile(targetPosition, (x,y) => BuildStructureTask.GetTileCost(stateSnapshot, x,y), validTileCB, out foundPt))
				return false;

			// Build structure
			m_MaintainCCTask.buildTask.SetLocation(foundPt);

			return true;
		}

		private MiningBeaconState GetClosestUnusedBeacon(StateSnapshot stateSnapshot, LOCATION position)
		{
			// Get closest beacon
			MiningBeaconState closestBeacon = null;
			int closestDistance = 900000;

			foreach (MiningBeaconState beacon in stateSnapshot.gaia.miningBeacons)
			{
				if (beacon.oreType != BeaconType.Common)
					continue;

				// Detect if occupied
				bool isOccupied = false;
				foreach (PlayerState player in stateSnapshot.players)
				{
					StructureState building = GetClosestBuildingOfType(player, map_id.Any, beacon.position);
					if (building != null && building.position.GetDiagonalDistance(beacon.position) < MiningBaseState.MaxMineDistanceToCC)
					{
						isOccupied = true;
						break;
					}
				}

				if (isOccupied)
					continue;
				
				// Closest distance
				int distance = position.GetDiagonalDistance(beacon.position);
				if (distance < closestDistance)
				{
					closestBeacon = beacon;
					closestDistance = distance;
				}
			}

			return closestBeacon;
		}

		private StructureState GetClosestBuildingOfType(PlayerState player, map_id type, LOCATION position)
		{
			StructureState closestUnit = null;
			int closestDistance = 999999;

			foreach (StructureState unit in player.units.GetStructures())
			{
				if (type != map_id.Any && type != unit.unitType)
					continue;

				int distance = unit.position.GetDiagonalDistance(position);
				if (distance < closestDistance)
				{
					closestUnit = unit;
					closestDistance = distance;
				}
			}

			return closestUnit;
		}
	}
}
