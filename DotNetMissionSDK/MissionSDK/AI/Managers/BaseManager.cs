using DotNetMissionSDK.AI.Tasks.Base.Starship;
using DotNetMissionSDK.AI.Tasks;
using DotNetMissionSDK.AI.Tasks.Base.Mining;
using DotNetMissionSDK.AI.Tasks.Base.Maintenance;
using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.State.Snapshot;
using System.Linq;
using DotNetMissionSDK.Async;
using System.Collections.Generic;
using System;
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
		private bool m_IsProcessing;

		public BotPlayer botPlayer	{ get; private set; }
		public int ownerID			{ get; private set; }

		// This manager's top-level goals.
		private Goal[] m_Goals;


		public BaseManager(BotPlayer botPlayer, int ownerID)
		{
			this.botPlayer = botPlayer;
			this.ownerID = ownerID;

			m_MiningBaseState = new MiningBaseState(ownerID);

			// Initialize goals
			m_Goals = new Goal[]
			{
				new Goal(new CreateCommonMiningBaseTask(ownerID, m_MiningBaseState), 1),
				new Goal(new MaintainTruckRoutes(ownerID, m_MiningBaseState), 1),
				new Goal(new MaintainPowerTask(ownerID), 1),
				new Goal(new UnloadSuppliesTask(ownerID), 1),
				new Goal(new FixDisconnectedStructures(ownerID), 1),
				new Goal(new RepairStructuresTask(ownerID), 1),
				new Goal(new MaintainFoodTask(ownerID), 1),
				new Goal(new MaintainPopulationTask(ownerID), 1),
				new Goal(new MaintainDefenseTask(ownerID), 1),
				new Goal(new MaintainWallsTask(ownerID), 1),
				new Goal(new CreateRareMiningBaseTask(ownerID, m_MiningBaseState), 1),
				new Goal(new BuildVehicleGroupTask(ownerID), 1),
				new Goal(new DeployEvacModuleTask(ownerID), 1),
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

			m_ResetVehicleTask = new ResetVehicleTask(ownerID);
		}

		public void Update(StateSnapshot stateSnapshot)
		{
			ThreadAssert.MainThreadRequired();

			if (m_IsProcessing)
				return;

			m_IsProcessing = true;

			// Get data that requires main thread
			List<VehicleGroup.UnitSlot> unassignedCombatSlots = botPlayer.combatManager.GetUnassignedSlots();

			//AsyncPump.Run(() =>
			//{
				List<Action> unitActions = new List<Action>();

				m_MiningBaseState.Update(stateSnapshot);
				UpdateGoal(stateSnapshot, unitActions, unassignedCombatSlots);
				m_ResetVehicleTask.PerformTaskTree(stateSnapshot, unitActions); // Fix stuck vehicles

			//	return unitActions;
			//},
			//(object returnState) =>
			//{
				m_IsProcessing = false;

				// Execute all completed actions
			//	List<Action> unitActions = (List<Action>)returnState;

				foreach (Action action in unitActions)
					action();
			//});
		}

		private void UpdateGoal(StateSnapshot stateSnapshot, List<Action> unitActions, List<VehicleGroup.UnitSlot> unassignedCombatSlots)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Top priority is establishing a base with common metals
			if (owner.units.commandCenters.Count == 0 || owner.units.commonOreMines.Count == 0 || owner.units.commonOreSmelters.Count == 0)
			{
				m_Goals[ExpandCommonMining_GoalID].task.PerformTaskTree(stateSnapshot, unitActions);
				return;
			}

			// Power level is high priority
			if (!m_Goals[MaintainPower_GoalID].task.IsTaskComplete(stateSnapshot))
				m_Goals[MaintainPower_GoalID].task.PerformTaskTree(stateSnapshot, unitActions);

			// Unload supplies
			if (!m_Goals[UnloadSupplies_GoalID].task.IsTaskComplete(stateSnapshot))
				m_Goals[UnloadSupplies_GoalID].task.PerformTaskTree(stateSnapshot, unitActions);

			// Maintain truck routes
			MaintainTruckRoutes truckRoutes = (MaintainTruckRoutes)m_Goals[MaintainTruckRoutes_GoalID].task;
			truckRoutes.UpdateNeededTrucks(stateSnapshot);

			if (!truckRoutes.IsTaskComplete(stateSnapshot))
				truckRoutes.PerformTaskTree(stateSnapshot, unitActions);

			truckRoutes.PerformTruckRoutes(unitActions);

			// Emergency repairs
			RepairStructuresTask repairStructureTask = (RepairStructuresTask)m_Goals[RepairStructures_GoalID].task;
			repairStructureTask.repairCriticalOnly = true;

			if (!repairStructureTask.IsTaskComplete(stateSnapshot))
				repairStructureTask.PerformTaskTree(stateSnapshot, unitActions);
			
			// Fix disconnected structures
			if (!m_Goals[FixDisconnectedStructures_GoalID].task.IsTaskComplete(stateSnapshot))
				m_Goals[FixDisconnectedStructures_GoalID].task.PerformTaskTree(stateSnapshot, unitActions); // Tubes
			else if (!m_Goals[MaintainWalls_GoalID].task.IsTaskComplete(stateSnapshot))
				m_Goals[MaintainWalls_GoalID].task.PerformTaskTree(stateSnapshot, unitActions); // Walls

			// Keep people fed
			if (!m_Goals[MaintainFood_GoalID].task.IsTaskComplete(stateSnapshot))
				m_Goals[MaintainFood_GoalID].task.PerformTaskTree(stateSnapshot, unitActions);

			// Grow population
			MaintainPopulationTask maintainPopulationTask = (MaintainPopulationTask)m_Goals[MaintainPopulation_GoalID].task;
			maintainPopulationTask.UpdateRequirements(stateSnapshot);

			if (!maintainPopulationTask.IsTaskComplete(stateSnapshot))
			{
				maintainPopulationTask.PerformTaskTree(stateSnapshot, unitActions);
				PerformFullRepairs(stateSnapshot, unitActions);
				//return;
			}

			// Build defenses
			if (!m_Goals[MaintainDefenses_GoalID].task.IsTaskComplete(stateSnapshot))
				m_Goals[MaintainDefenses_GoalID].task.PerformTaskTree(stateSnapshot, unitActions);

			// Expand rare mining
			if (owner.units.rareOreMines.Count == 0 || owner.units.rareOreSmelters.Count == 0)
			{
				if (owner.units.rareOreSmelters.FirstOrDefault((smelter) => !smelter.isEnabled) == null)
					m_Goals[ExpandRareMining_GoalID].task.PerformTaskTree(stateSnapshot, unitActions);
			}
			else
			{
				// Expand common mining
				if (owner.units.commonOreSmelters.FirstOrDefault((smelter) => !smelter.isEnabled) == null)
					m_Goals[ExpandCommonMining_GoalID].task.PerformTaskTree(stateSnapshot, unitActions);
			}

			PerformFullRepairs(stateSnapshot, unitActions);

			// Build combat units
			if (owner.ore > 2800)
			{
				BuildVehicleGroupTask combatGroupTask = (BuildVehicleGroupTask)m_Goals[BuildCombatVehicles_GoalID].task;

				// Unassigned slots are returned in a prioritized order based on the ThreatZone.
				combatGroupTask.SetVehicleGroupSlots(unassignedCombatSlots);

				combatGroupTask.PerformTaskTree(stateSnapshot, unitActions);
			}

			//return goals[LaunchStarship_GoalID];
		}

		private bool PerformFullRepairs(StateSnapshot stateSnapshot, List<Action> unitActions)
		{
			RepairStructuresTask repairStructureTask = (RepairStructuresTask)m_Goals[RepairStructures_GoalID].task;
			repairStructureTask.repairCriticalOnly = false;

			if (!repairStructureTask.IsTaskComplete(stateSnapshot))
				return repairStructureTask.PerformTaskTree(stateSnapshot, unitActions);

			return false;
		}
	}
}
