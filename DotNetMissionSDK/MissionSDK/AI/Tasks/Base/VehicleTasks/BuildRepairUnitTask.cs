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

			m_BuildVehicleTask.targetCountToBuild = targetCountToBuild;

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
			m_BuildVehicleTask.targetCountToBuild = targetCountToBuild;

			// Check if there are enough convecs
			return m_BuildVehicleTask.IsTaskComplete(stateSnapshot);
		}

		private void Initialize()
		{
			repairUnitType = map_id.ConVec;
			m_BuildVehicleTask = m_BuildConvecTask;
		}

		public override void GeneratePrerequisites()
		{
			m_BuildRepairVehicleTask = new BuildRepairVehicleTask(ownerID);
			m_BuildSpiderTask = new BuildSpiderTask(ownerID);
			m_BuildConvecTask = new BuildConvecTask(ownerID);

			m_BuildRepairVehicleTask.GeneratePrerequisites();
			m_BuildSpiderTask.GeneratePrerequisites();
			m_BuildConvecTask.GeneratePrerequisites();
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			return m_BuildVehicleTask.PerformTaskTree(stateSnapshot, unitActions);
		}
	}
}
