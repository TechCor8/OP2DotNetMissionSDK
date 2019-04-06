using DotNetMissionSDK.AI.Tasks.Base.Starship;
using DotNetMissionSDK.Utility;
using DotNetMissionSDK.AI.Tasks;
using DotNetMissionSDK.AI.Tasks.Base.Mining;

namespace DotNetMissionSDK.AI.Managers
{
	public class BaseManager
	{
		public const int ExpandCommonMining_GoalID			= 0;

		private MiningBaseState m_MiningBaseState;

		public PlayerInfo owner { get; private set; }

		public Goal[] goals		{ get; private set; }			// This manager's top-level goals.


		public BaseManager(BotType botType, PlayerInfo owner)
		{
			this.owner = owner;

			m_MiningBaseState = new MiningBaseState(owner);

			// Initialize goals
			goals = new Goal[]
			{
				new Goal(new CreateCommonMiningBaseTask(owner, m_MiningBaseState), 1),
			};

			// TODO: Fill out goal weights
			switch (botType)
			{
				case BotType.PopulationGrowth:
				case BotType.LaunchStarship:
				case BotType.EconomicGrowth:
				case BotType.Passive:
				case BotType.Defender:
				case BotType.Balanced:
				case BotType.Aggressive:
				case BotType.Harassment:
				case BotType.Wreckless:
					break;
			}
		}

		public void Update()
		{
			m_MiningBaseState.Update();

			Goal currentGoal = SelectGoal();

			currentGoal.task.PerformTaskTree();
		}

		private Goal SelectGoal()
		{
			return goals[ExpandCommonMining_GoalID];
		}
	}
}
