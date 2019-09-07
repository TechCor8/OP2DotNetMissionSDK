using DotNetMissionSDK.AI.Tasks;
using DotNetMissionSDK.AI.Tasks.Base.Mining;
using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.Async;
using System.Collections.Generic;
using DotNetMissionSDK.AI.Tasks.Base.Goals;
using DotNetMissionSDK.State.Snapshot.Units;
using System.Collections.ObjectModel;

namespace DotNetMissionSDK.AI.Managers
{
	/// <summary>
	/// Controls the Bot's base.
	/// </summary>
	public class BaseManager
	{
		// This manager's top-level goals.
		private Goal[] m_Goals;

		private MaintainArmyGoal m_MaintainArmyGoal;

		private MiningBaseState m_MiningBaseState;

		// Low-level tasks that are executed directly
		private MaintainTruckRoutes m_MaintainTruckRoutes;
		private ResetVehicleTask m_ResetVehicleTask;

		private bool m_IsProcessing;
		private string m_DebugMessage = "None";

		public BotPlayer botPlayer								{ get; private set; }
		public int ownerID										{ get; private set; }

		private ReadOnlyCollection<int> m_StructureLaborOrder;
		public ReadOnlyCollection<int> GetStructureLaborOrderByID()
		{
			ThreadAssert.MainThreadRequired();

			return m_StructureLaborOrder;
		}

		
		public BaseManager(BotPlayer botPlayer, int ownerID)
		{
			this.botPlayer = botPlayer;
			this.ownerID = ownerID;

			m_MiningBaseState = new MiningBaseState(ownerID);

			// Initialize goals
			m_Goals = new Goal[]
			{
				new ExpandCommonMiningGoal(ownerID, m_MiningBaseState, 1),
				new MaintainPowerGoal(ownerID, 0.99f),
				new UnloadCommonMetalGoal(ownerID, 1),
				new UnloadRareMetalGoal(ownerID, 1),
				new UnloadFoodGoal(ownerID, 1),
				new RepairStructuresGoal(ownerID, 1),
				new MaintainFoodGoal(ownerID, 0.99f),
				new MaintainPopulationGoal(ownerID, 1),
				new MaintainResearchGoal(ownerID, 1),
				new MaintainDefenseGoal(ownerID, 0.75f),
				new MaintainWallsGoal(ownerID, 0.1f),
				new ExpandRareMiningGoal(ownerID, m_MiningBaseState, 0.97f),
				m_MaintainArmyGoal = new MaintainArmyGoal(ownerID, 0.98f),
				new LaunchStarshipGoal(ownerID, 1),
				new MaintainStructureFactoryGoal(ownerID, 1),
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

			m_MaintainTruckRoutes = new MaintainTruckRoutes(ownerID, m_MiningBaseState);
			m_ResetVehicleTask = new ResetVehicleTask(ownerID);

			m_StructureLaborOrder = new ReadOnlyCollection<int>(new int[0]);
		}

		public void Update(StateSnapshot stateSnapshot)
		{
			ThreadAssert.MainThreadRequired();

			// ** Update every frame **
			BotCommands frameCommands = new BotCommands();

			m_MiningBaseState.Update(stateSnapshot);

			// Perform truck routes
			m_MaintainTruckRoutes.PerformTruckRoutes(frameCommands);

			frameCommands.Execute();

			// ** Intermittent/async processing below this point **
			if (m_IsProcessing)
				return;

			m_IsProcessing = true;

			// Get data that requires main thread
			m_MaintainArmyGoal.SetVehicleGroupSlots(botPlayer.combatManager.GetUnassignedSlots());

			// Variables that should only be used in async callbacks
			List<int> structureIDLaborOrder = new List<int>();

			stateSnapshot.Retain();

			AsyncPump.Run(() =>
			{
				BotCommands botCommands = new BotCommands();

				// Get goals and perform goal tasks
				List<Goal> goals = GetPrioritizedGoals(stateSnapshot);
				PerformGoalTasks(goals, stateSnapshot, botCommands);
				m_ResetVehicleTask.PerformTaskTree(stateSnapshot, botCommands); // Fix stuck vehicles

				m_DebugMessage = goals[0].GetType().Name; // DEBUG

				// Get activation and research priorities
				foreach (Goal goal in goals)
					goal.GetStructuresToActivate(stateSnapshot, structureIDLaborOrder);

				return botCommands;
			},
			(object returnState) =>
			{
				m_IsProcessing = false;

				// Execute all completed actions
				BotCommands botCommands = (BotCommands)returnState;

				botCommands.Execute();

				// Store data
				m_StructureLaborOrder = structureIDLaborOrder.AsReadOnly();
				
				stateSnapshot.Release();

				// DEBUG:
				if (ownerID == TethysGame.LocalPlayer())
				{
					TethysGame.AddMessage(ownerID, m_DebugMessage, ownerID, 0);
				}
			});
		}

		private List<Goal> GetPrioritizedGoals(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			List<Goal> goals = new List<Goal>(m_Goals);

			// Update importance
			foreach (Goal goal in goals)
				goal.UpdateImportance(stateSnapshot, owner);

			// Sort priority descending
			goals.Sort((Goal a, Goal b) => b.importance.CompareTo(a.importance));

			return goals;
		}

		private void PerformGoalTasks(IEnumerable<Goal> goals, StateSnapshot stateSnapshot, BotCommands botCommands)
		{
			foreach (Goal goal in goals)
			{
				// Goals that approach zero importance should not be performed at all.
				if (goal.importance < 0.0001f)
					continue;

				// If task failed, do not perform lower priority tasks, as they may prevent acquisition of resources to complete this task.
				if (!goal.PerformTask(stateSnapshot, botCommands))
					break;

				// If true, this goal always blocks lower priority goals, regardless of whether the goal's task was completed successfully.
				if (goal.blockLowerPriority)
					break;
			}
		}
	}
}
