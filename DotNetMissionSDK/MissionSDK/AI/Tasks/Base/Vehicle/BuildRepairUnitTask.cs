using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Vehicle
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


		public BuildRepairUnitTask()											{ _GeneratePrerequisites(); }
		public BuildRepairUnitTask(PlayerInfo owner) : base(owner)				{ _GeneratePrerequisites(); }

		public map_id repairUnitType		{ get; private set; }

		public override bool IsTaskComplete()
		{
			// Default to most advanced repair unit for the colony type
			if (owner.player.IsEden())
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
			if (m_BuildVehicleTask.IsTaskComplete())
				return true;

			if (mustRepairVehicles)
				return false;
			
			// If advanced repair unit can be created, wait for it
			if (m_BuildVehicleTask.CanPerformTaskTree())
				return false;

			// ...Otherwise, use convecs
			repairUnitType = map_id.ConVec;
			m_BuildVehicleTask = m_BuildConvecTask;
			m_BuildVehicleTask.targetCountToBuild = targetCountToBuild;

			// Check if there are enough convecs
			return m_BuildVehicleTask.IsTaskComplete();
		}

		private void _GeneratePrerequisites()
		{
			m_BuildRepairVehicleTask = new BuildRepairVehicleTask(owner);
			m_BuildSpiderTask = new BuildSpiderTask(owner);
			m_BuildConvecTask = new BuildConvecTask(owner);

			repairUnitType = map_id.ConVec;
			m_BuildVehicleTask = m_BuildConvecTask;
		}

		public override void GeneratePrerequisites()
		{
		}

		protected override bool PerformTask()
		{
			return m_BuildVehicleTask.PerformTaskTree();
		}
	}
}
