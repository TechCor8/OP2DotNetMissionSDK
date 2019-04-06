using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class MaintainFoodTask : Task
	{
		private BuildAgridomeTask m_BuildAgridomeTask;


		public MaintainFoodTask() { }
		public MaintainFoodTask(PlayerInfo owner) : base(owner) { }

		public override bool IsTaskComplete()
		{
			return owner.player.FoodSupply() == FoodStatus.Rising;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(m_BuildAgridomeTask = new BuildAgridomeTask());
		}

		protected override bool PerformTask()
		{
			// Keep building one more agridome until task complete
			m_BuildAgridomeTask.targetCountToBuild = owner.units.agridomes.Count+1;

			return true;
		}
	}
}
