using DotNetMissionSDK.AI.Managers;
using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.Mining
{
	/// <summary>
	/// Finds beacons inside of a command center's control area and builds a new mine.
	/// All mines must be saturated before a new mine is created.
	/// </summary>
	public class CreateRareMineTask : Task
	{
		private MiningBaseState m_MiningBaseState;

		private SaturateRareMineTask m_SaturateMineTask;


		public CreateRareMineTask(int ownerID, MiningBaseState miningBaseState) : base(ownerID)	{ m_MiningBaseState = miningBaseState; }


		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			// Task is not complete until every CC beacon has been occupied and saturated
			if (!m_SaturateMineTask.IsTaskComplete(stateSnapshot))
				return false;
			
			// Task is complete if all controlled beacons have mines
			foreach (MiningBase miningBase in m_MiningBaseState.miningBases)
			{
				foreach (MiningSite site in miningBase.miningSites)
				{
					if (site.beacon.oreType != BeaconType.Rare)
						continue;
					
					if (site.mine == null)
						return false;
				}
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(m_SaturateMineTask = new SaturateRareMineTask(ownerID, m_MiningBaseState), true);
			AddPrerequisite(new BuildSurveyorTask(ownerID));
			AddPrerequisite(new BuildMinerTask(ownerID));
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Get miner
			if (owner.units.roboMiners.Count == 0) return false;
			VehicleState miner = owner.units.roboMiners[0];
			
			// Check if miner is being deployed (Both exist at the same time for a moment and triggers a bug)
			if (owner.units.rareOreMines.FirstOrDefault((StructureState mine) => mine.position.Equals(miner.position)) != null)
				return false;
			
			// Find closest unoccupied rare beacon
			MiningBeaconState beacon = GetClosestUnusedBeacon(miner);
			LOCATION beaconPosition = beacon.position;
			
			// Check if beacon is surveyed
			if (beacon.GetSurveyedBy(ownerID))
			{
				// Deploy miner
				BuildStructureTask.ClearDeployArea(miner, map_id.RareOreMine, beacon.position, stateSnapshot, ownerID, unitActions);

				if (!miner.position.Equals(beaconPosition) && miner.curAction == ActionType.moDone) // WARNING: If unit is EMP'd, it will get stuck
					unitActions.AddUnitCommand(miner.unitID, 1, () => GameState.GetUnit(miner.unitID)?.DoDeployMiner(beaconPosition.x, beaconPosition.y));

				return true;
			}
			
			// Move miner involved in expansion close to beacon for efficiency
			if (miner.curAction == ActionType.moDone)
				unitActions.AddUnitCommand(miner.unitID, 1, () => GameState.GetUnit(miner.unitID)?.DoMove(beaconPosition.x-1, beaconPosition.y+1));
			
			// Survey beacon
			if (owner.units.roboSurveyors.Count == 0)
				return false;
			
			VehicleState surveyor = owner.units.roboSurveyors[0];

			if (!surveyor.position.Equals(beaconPosition) && surveyor.curAction == ActionType.moDone) // WARNING: If unit is EMP'd, it will get stuck
				unitActions.AddUnitCommand(surveyor.unitID, 1, () => GameState.GetUnit(surveyor.unitID)?.DoMove(beaconPosition.x, beaconPosition.y));
			
			return true;
		}

		private MiningBeaconState GetClosestUnusedBeacon(VehicleState miner)
		{
			// Get closest beacon
			MiningBeaconState closestBeacon = null;
			int closestDistance = 900000;

			foreach (MiningBase miningBase in m_MiningBaseState.miningBases)
			{
				foreach (MiningSite site in miningBase.miningSites)
				{
					if (site.beacon.oreType != BeaconType.Rare) continue;
					if (site.mine != null) continue;

					int distance = miner.position.GetDiagonalDistance(site.beacon.position);
					if (distance < closestDistance)
					{
						closestBeacon = site.beacon;
						closestDistance = distance;
					}
				}
			}

			return closestBeacon;
		}
	}
}
