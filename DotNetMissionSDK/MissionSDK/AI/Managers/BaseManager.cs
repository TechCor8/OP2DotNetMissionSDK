using DotNetMissionSDK.AI.Tasks.Base.Starship;
using DotNetMissionSDK.Utility;
using DotNetMissionSDK.AI.Tasks;
using DotNetMissionSDK.AI.Tasks.Base.Mining;
using DotNetMissionSDK.AI.Tasks.Base.Maintenance;
using DotNetMissionSDK.AI.Tasks.Base.Vehicle;
using System.Collections.Generic;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.AI.Combat.Groups;

namespace DotNetMissionSDK.AI.Managers
{
	public class BaseManager
	{
		public const int ExpandCommonMining_GoalID			= 0;
		public const int MaintainTruckRoutes_GoalID			= 1;
		public const int MaintainPower_GoalID				= 2;
		public const int UnloadSupplies_GoalID				= 3;
		public const int FixDisconnectedStructures_GoalID	= 4;
		public const int RepairStructures_GoalID			= 5;
		public const int MaintainFood_GoalID				= 6;
		public const int MaintainPopulation_GoalID			= 7;
		public const int MaintainDefenses_GoalID			= 8;
		public const int MaintainWalls_GoalID				= 9;
		public const int ExpandRareMining_GoalID			= 10;
		public const int BuildCombatVehicles_GoalID			= 11;
		public const int LaunchStarship_GoalID				= 12;

		private ResetVehicleTask m_ResetVehicleTask;

		private MiningBaseState m_MiningBaseState;

		public BotPlayer botPlayer	{ get; private set; }
		public PlayerInfo owner		{ get; private set; }

		public Goal[] goals			{ get; private set; }			// This manager's top-level goals.


		public BaseManager(BotPlayer botPlayer, PlayerInfo owner)
		{
			this.botPlayer = botPlayer;
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
				new Goal(new RepairStructuresTask(owner), 1),
				new Goal(new MaintainFoodTask(owner), 1),
				new Goal(new MaintainPopulationTask(owner), 1),
				new Goal(new MaintainDefenseTask(owner), 1),
				new Goal(new MaintainWallsTask(owner), 1),
				new Goal(new CreateRareMiningBaseTask(owner, m_MiningBaseState), 1),
				new Goal(new BuildVehicleGroupTask(owner), 1),
				new Goal(new DeployEvacModuleTask(owner), 1),
			};

			// TODO: Fill out goal weights
			switch (botPlayer.botType)
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

			m_ResetVehicleTask = new ResetVehicleTask(owner);
		}

		public void Update()
		{
			m_MiningBaseState.Update();
			
			UpdateGoal();

			// Fix stuck vehicles
			m_ResetVehicleTask.PerformTaskTree();
		}

		private void UpdateGoal()
		{
			// Top priority is establishing a base with common metals
			if (owner.units.commandCenters.Count == 0 || owner.units.commonOreMines.Count == 0 || owner.units.commonOreSmelters.Count == 0)
			{
				goals[ExpandCommonMining_GoalID].task.PerformTaskTree();
				return;
			}

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

			// Emergency repairs
			RepairStructuresTask repairStructureTask = (RepairStructuresTask)goals[RepairStructures_GoalID].task;
			repairStructureTask.repairCriticalOnly = true;

			if (!repairStructureTask.IsTaskComplete())
				repairStructureTask.PerformTaskTree();
			
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
			{
				maintainPopulationTask.PerformTaskTree();
				PerformFullRepairs();
				return;
			}

			// Build defenses
			if (!goals[MaintainDefenses_GoalID].task.IsTaskComplete())
				goals[MaintainDefenses_GoalID].task.PerformTaskTree();

			// Expand rare mining
			if (owner.units.rareOreMines.Count == 0 || owner.units.rareOreSmelters.Count == 0)
			{
				goals[ExpandRareMining_GoalID].task.PerformTaskTree();
				PerformFullRepairs();
			}
			else
			{
				// Expand common mining
				goals[ExpandCommonMining_GoalID].task.PerformTaskTree();
				PerformFullRepairs();
			}

			// Build combat units
			if (owner.player.Ore() > 2800)
			{
				BuildVehicleGroupTask combatGroupTask = (BuildVehicleGroupTask)goals[BuildCombatVehicles_GoalID].task;

				// Unassigned slots are returned in a prioritized order based on the ThreatZone.
				combatGroupTask.SetVehicleGroupSlots(botPlayer.combatManager.GetUnassignedSlots());

				combatGroupTask.PerformTaskTree();
			}

			//return goals[LaunchStarship_GoalID];
		}

		private bool PerformFullRepairs()
		{
			RepairStructuresTask repairStructureTask = (RepairStructuresTask)goals[RepairStructures_GoalID].task;
			repairStructureTask.repairCriticalOnly = false;

			if (!repairStructureTask.IsTaskComplete())
				return repairStructureTask.PerformTaskTree();

			return false;
		}
	}
}
