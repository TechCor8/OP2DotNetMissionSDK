using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class MaintainFoodTask : Task
	{
		private BuildAgridomeTask m_BuildAgridomeTask;


		public MaintainFoodTask(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			return owner.foodSupply == FoodStatus.Rising;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(m_BuildAgridomeTask = new BuildAgridomeTask(ownerID));
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Don't build more agridomes if we aren't using all the ones we have
			foreach (StructureState agridome in owner.units.agridomes)
			{
				if (!agridome.isEnabled)
					return false;
			}

			// Keep building one more agridome until task complete
			m_BuildAgridomeTask.targetCountToBuild = owner.units.agridomes.Count+1;

			return true;
		}
	}
}
