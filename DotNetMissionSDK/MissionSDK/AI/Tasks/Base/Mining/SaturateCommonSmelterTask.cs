using DotNetMissionSDK.AI.Managers;
using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.State.Snapshot;

namespace DotNetMissionSDK.AI.Tasks.Base.Mining
{
	/// <summary>
	/// Finds common smelters and creates trucks until they are full.
	/// </summary>
	public class SaturateCommonSmelterTask : Task
	{
		private MiningBaseState m_MiningBaseState;

		private BuildCargoTruckTask m_BuildCargoTruckTask;


		public SaturateCommonSmelterTask(int ownerID, MiningBaseState miningBaseState) : base(ownerID)	{ m_MiningBaseState = miningBaseState; }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			UpdateNeededTrucks(stateSnapshot);

			// Task is complete when all smelters are saturated
			foreach (MiningBase miningBase in m_MiningBaseState.miningBases)
			{
				foreach (MiningSite site in miningBase.miningSites)
				{
					if (site.mine == null)
						continue;

					if (site.beacon.oreType != BeaconType.Common)
						continue;

					foreach (MiningSmelter smelter in site.smelters)
					{
						if (!smelter.smelter.isEnabled)
							continue;

						if (smelter.trucks.Count != smelter.desiredTruckCount)
							return false;
					}
				}
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(m_BuildCargoTruckTask = new BuildCargoTruckTask(ownerID));
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			return true;
		}

		private void UpdateNeededTrucks(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Determine total assigned trucks and total desired trucks
			int assignedTrucks = 0;
			int desiredTrucks = 0;

			foreach (MiningBase miningBase in m_MiningBaseState.miningBases)
			{
				foreach (MiningSite site in miningBase.miningSites)
				{
					if (site.mine == null)
						continue;

					if (site.beacon.oreType != BeaconType.Common)
						continue;

					foreach (MiningSmelter smelter in site.smelters)
					{
						if (!smelter.smelter.isEnabled)
							continue;

						assignedTrucks += smelter.trucks.Count;
						desiredTrucks += smelter.desiredTruckCount;
					}
				}
			}

			int trucksDoingSomethingElse = owner.units.cargoTrucks.Count - assignedTrucks;

			m_BuildCargoTruckTask.targetCountToBuild = desiredTrucks + trucksDoingSomethingElse;
		}
	}
}
