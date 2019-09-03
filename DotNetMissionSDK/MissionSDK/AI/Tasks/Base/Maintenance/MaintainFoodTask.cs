using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class MaintainFoodTask : Task
	{
		private MaintainAgridomeTask m_BuildAgridomeTask;


		public MaintainFoodTask(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			return owner.foodSupply == FoodStatus.Rising;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(m_BuildAgridomeTask = new MaintainAgridomeTask(ownerID));
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Don't build more agridomes if we aren't using all the ones we have
			foreach (StructureState agridome in owner.units.agridomes)
			{
				if (!agridome.isEnabled)
					return true;
			}

			// Calculate number of agridomes needed for net positive
			int neededAgridomes = 1;
			int numActiveAgridomes = owner.units.agridomes.Count((StructureState unit) => unit.isEnabled);
			if (numActiveAgridomes > 0)
			{
				int foodPerAgridome = owner.totalFoodProduction / numActiveAgridomes;
				if (foodPerAgridome == 0) foodPerAgridome = 1;
				neededAgridomes = owner.totalFoodConsumption / foodPerAgridome + 1;
			}

			m_BuildAgridomeTask.targetCountToMaintain = neededAgridomes;

			return true;
		}
	}
}
