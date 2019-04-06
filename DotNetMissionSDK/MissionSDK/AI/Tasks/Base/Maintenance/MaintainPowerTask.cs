using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class MaintainPowerTask : Task
	{
		private BuildTokamakTask m_BuildTokamakTask;


		public MaintainPowerTask() { }
		public MaintainPowerTask(PlayerInfo owner) : base(owner) { }

		public override bool IsTaskComplete()
		{
			// TODO: Change this to MaxPower < CurrentPower+50
			if (owner.units.tokamaks.Count < 1)
				return false;

			return true;
		}

		public override void GeneratePrerequisites()
		{
			m_BuildTokamakTask = new BuildTokamakTask(owner);
		}

		protected override bool PerformTask()
		{
			// TODO: Support geocon, solar power, MHD Generator

			m_BuildTokamakTask.targetCountToBuild = 1; // TODO: target count = current count + power still needed / tokamak output
			return m_BuildTokamakTask.PerformTaskTree();
		}
	}
}
