using DotNetMissionSDK.AI.Tasks.Base.Starship;
using DotNetMissionSDK.Utility;
using DotNetMissionSDK.AI.Tasks;
using DotNetMissionSDK.AI.Tasks.Base.Mining;
using DotNetMissionSDK.AI.Tasks.Base.Maintenance;

namespace DotNetMissionSDK.AI.Managers
{
	public class BaseManager
	{
		public const int ExpandCommonMining_GoalID			= 0;
		public const int MaintainTruckRoutes_GoalID			= 1;
		public const int MaintainPower_GoalID				= 2;
		public const int UnloadSupplies_GoalID				= 3;
		public const int FixDisconnectedStructures_GoalID	= 4;
		public const int MaintainFood_GoalID				= 5;
		public const int MaintainPopulation_GoalID			= 6;
		public const int MaintainDefenses_GoalID			= 7;
		public const int MaintainWalls_GoalID				= 8;
		public const int ExpandRareMining_GoalID			= 9;
		public const int LaunchStarship_GoalID				= 10;

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
				new Goal(new MaintainTruckRoutes(owner, m_MiningBaseState), 1),
				new Goal(new MaintainPowerTask(owner), 1),
				new Goal(new UnloadSuppliesTask(owner), 1),
				new Goal(new FixDisconnectedStructures(owner), 1),
				new Goal(new MaintainFoodTask(owner), 1),
				new Goal(new MaintainPopulationTask(owner), 1),
				new Goal(new MaintainDefenseTask(owner), 1),
				new Goal(new MaintainWallsTask(owner), 1),
				new Goal(new CreateRareMiningBaseTask(owner, m_MiningBaseState), 1),
				new Goal(new DeployEvacModuleTask(owner), 1),
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
			
			UpdateGoal();
		}

		private bool UpdateGoal()
		{
			// Top priority is establishing a base with common metals
			if (owner.units.commandCenters.Count == 0 || owner.units.commonOreMines.Count == 0 || owner.units.commonOreSmelters.Count == 0)
				return goals[ExpandCommonMining_GoalID].task.PerformTaskTree();

			// Power level is high priority
			if (!goals[MaintainPower_GoalID].task.IsTaskComplete())
				goals[MaintainPower_GoalID].task.PerformTaskTree();

			// Unload supplies
			if (!goals[UnloadSupplies_GoalID].task.IsTaskComplete())
				goals[UnloadSupplies_GoalID].task.PerformTaskTree();

			// Maintain truck routes
			MaintainTruckRoutes truckRoutes = (MaintainTruckRoutes)goals[MaintainTruckRoutes_GoalID].task;
			truckRoutes.UpdateNeededTrucks();

			if (!truckRoutes.IsTaskComplete())
				truckRoutes.PerformTaskTree();

			truckRoutes.PerformTruckRoutes();
			
			// Fix disconnected structures
			if (!goals[FixDisconnectedStructures_GoalID].task.IsTaskComplete())
				goals[FixDisconnectedStructures_GoalID].task.PerformTaskTree(); // Tubes
			else if (!goals[MaintainWalls_GoalID].task.IsTaskComplete())
				goals[MaintainWalls_GoalID].task.PerformTaskTree(); // Walls

			// Keep people fed
			if (!goals[MaintainFood_GoalID].task.IsTaskComplete())
				goals[MaintainFood_GoalID].task.PerformTaskTree();

			// Grow population
			MaintainPopulationTask maintainPopulationTask = (MaintainPopulationTask)goals[MaintainPopulation_GoalID].task;
			maintainPopulationTask.UpdateRequirements();

			if (!maintainPopulationTask.IsTaskComplete())
				return maintainPopulationTask.PerformTaskTree();

			// Build defenses
			if (!goals[MaintainDefenses_GoalID].task.IsTaskComplete())
				goals[MaintainDefenses_GoalID].task.PerformTaskTree();

			// Expand rare mining
			if (owner.units.rareOreMines.Count == 0 || owner.units.rareOreSmelters.Count == 0)
				return goals[ExpandRareMining_GoalID].task.PerformTaskTree();
			
			// Aspire to primary goal
			return goals[ExpandCommonMining_GoalID].task.PerformTaskTree();
			//return goals[LaunchStarship_GoalID];
		}
	}
}
