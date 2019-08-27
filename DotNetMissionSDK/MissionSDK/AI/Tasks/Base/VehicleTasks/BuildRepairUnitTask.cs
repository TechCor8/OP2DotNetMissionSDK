using DotNetMissionSDK.State.Snapshot;
using System;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.VehicleTasks
{
	/// <summary>
	/// Builds a repair vehicle, spider, or convec.
	/// </summary>
	public class BuildRepairUnitTask : Task
	{
		private BuildRepairVehicleTask m_BuildRepairVehicleTask;
		private BuildSpiderTask m_BuildSpiderTask;
		private BuildConvecTask m_BuildConvecTask;

		private BuildVehicleTask m_BuildVehicleTask;

		public bool mustRepairVehicles;

		public int targetCountToBuild = 1;


		public BuildRepairUnitTask(int ownerID) : base(ownerID)					{ Initialize(); }

		public map_id repairUnitType		{ get; private set; }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Default to most advanced repair unit for the colony type
			if (owner.isEden)
			{
				repairUnitType = map_id.RepairVehicle;
				m_BuildVehicleTask = m_BuildRepairVehicleTask;
			}
			else
			{
				repairUnitType = map_id.Spider;
				m_BuildVehicleTask = m_BuildSpiderTask;
			}

			SetTargetCountToBuild(targetCountToBuild);

			// Check if task is complete
			if (m_BuildVehicleTask.IsTaskComplete(stateSnapshot))
				return true;

			if (mustRepairVehicles)
				return false;
			
			// If advanced repair unit can be created, wait for it
			if (m_BuildVehicleTask.CanPerformTaskTree(stateSnapshot))
				return false;

			// ...Otherwise, use convecs
			repairUnitType = map_id.ConVec;
			m_BuildVehicleTask = m_BuildConvecTask;
			SetTargetCountToBuild(targetCountToBuild);

			// Check if there are enough convecs
			return m_BuildVehicleTask.IsTaskComplete(stateSnapshot);
		}

		private void Initialize()
		{
			repairUnitType = map_id.ConVec;
			m_BuildVehicleTask = m_BuildConvecTask;
		}

		private void SetTargetCountToBuild(int targetCount)
		{
			m_BuildRepairVehicleTask.targetCountToBuild = 0;
			m_BuildSpiderTask.targetCountToBuild = 0;
			m_BuildConvecTask.targetCountToBuild = 0;

			m_BuildVehicleTask.targetCountToBuild = targetCount;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(m_BuildRepairVehicleTask = new BuildRepairVehicleTask(ownerID));
			AddPrerequisite(m_BuildSpiderTask = new BuildSpiderTask(ownerID));
			AddPrerequisite(m_BuildConvecTask = new BuildConvecTask(ownerID));
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			return m_BuildVehicleTask.PerformTaskTree(stateSnapshot, unitActions);
		}
	}
}
