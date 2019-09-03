using DotNetMissionSDK.AI.Managers;
using DotNetMissionSDK.State.Snapshot;

namespace DotNetMissionSDK.AI.Tasks.Base.Mining
{
	/// <summary>
	/// Expands common mining operations.
	/// All bases must be fully saturated before a new base is created.
	/// </summary>
	public class ExpandCommonMiningTask : Task
	{
		private MiningBaseState m_MiningBaseState;

		public ExpandCommonMiningTask(int ownerID, MiningBaseState miningBaseState) : base(ownerID)	{ m_MiningBaseState = miningBaseState; }


		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			// Task is complete when there are no unoccupied beacons left and all mines have the required smelters and trucks.
			foreach (Task prerequisite in prerequisites)
			{
				if (!prerequisite.IsTaskComplete(stateSnapshot))
					return false;
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new SaturateCommonSmelterTask(ownerID, m_MiningBaseState));			// Put trucks on all smelters
			AddPrerequisite(new SaturateCommonMineTask(ownerID, m_MiningBaseState), true);		// Put smelters on all mines
			AddPrerequisite(new CreateCommonMineTask(ownerID, m_MiningBaseState), true);		// Put mines on all bases
			AddPrerequisite(new CreateCommonMiningBaseTask(ownerID, m_MiningBaseState), true);	// Put bases on all unoccupied beacons
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			return true;
		}
	}
}
