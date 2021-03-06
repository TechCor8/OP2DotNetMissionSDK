﻿using DotNetMissionSDK.AI.Tasks;
using DotNetMissionSDK.AI.Tasks.Base.Mining;
using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.Async;
using System.Collections.Generic;
using DotNetMissionSDK.AI.Tasks.Base.Goals;
using System.Collections.ObjectModel;

namespace DotNetMissionSDK.AI.Managers
{
	/// <summary>
	/// Controls the Bot's base.
	/// </summary>
	public class BaseManager
	{
		/// <summary>
		/// Used for passing params from the async task to the async completion callback.
		/// </summary>
		private class AsyncParams
		{
			public BotCommands botCommands			{ get; private set; }
			public List<int> structureIDLaborOrder	{ get; private set; }
			public TaskResult goalResults			{ get; private set; }

			public AsyncParams(BotCommands botCommands, List<int> structureIDLaborOrder, TaskResult goalResults)
			{
				this.botCommands = botCommands;
				this.structureIDLaborOrder = structureIDLaborOrder;
				this.goalResults = goalResults;
			}
		}

		// This manager's top-level goals.
		private Goal[] m_Goals;

		private MaintainArmyGoal m_MaintainArmyGoal;
		private MaintainResearchGoal m_MaintainResearchGoal;

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
				m_MaintainResearchGoal = new MaintainResearchGoal(ownerID, 1),
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

			stateSnapshot.Retain();

			AsyncPump.Run(() =>
			{
				BotCommands botCommands = new BotCommands();
				List<int> structureIDLaborOrder = new List<int>();

				// Get goals and perform goal tasks
				List<Goal> goals = GetPrioritizedGoals(stateSnapshot);
				TaskResult goalResults = PerformGoalTasks(goals, stateSnapshot, botCommands);
				m_ResetVehicleTask.PerformTaskTree(stateSnapshot, goalResults.missingRequirements, botCommands); // Fix stuck vehicles

				m_DebugMessage = goals[0].GetType().Name; // DEBUG

				// Get activation and research priorities
				foreach (Goal goal in goals)
					goal.GetStructuresToActivate(stateSnapshot, structureIDLaborOrder);

				return new AsyncParams(botCommands, structureIDLaborOrder, goalResults);
			},
			(object returnState) =>
			{
				m_IsProcessing = false;

				// Execute all completed actions
				AsyncParams asyncParams = (AsyncParams)returnState;

				asyncParams.botCommands.Execute();

				// Store data
				m_StructureLaborOrder = asyncParams.structureIDLaborOrder.AsReadOnly();
				
				stateSnapshot.Release();

				// DEBUG:
				if (ownerID == TethysGame.LocalPlayer())
				{
					//TethysGame.AddMessage(ownerID, m_DebugMessage, ownerID, 0);
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

		private TaskResult PerformGoalTasks(IEnumerable<Goal> goals, StateSnapshot stateSnapshot, BotCommands botCommands)
		{
			TaskResult result = new TaskResult(TaskRequirements.None);

			bool didPerformResearchGoal = false;

			foreach (Goal goal in goals)
			{
				// Goals that approach zero importance should not be performed at all.
				if (goal.importance < 0.0001f)
					continue;

				// Perform the task. Combine the results.
				result += goal.PerformTask(stateSnapshot, result.missingRequirements, botCommands);

				// If this goal is waiting on research, trigger maintain research goal to get labs up and running.
				if (!didPerformResearchGoal && result.missingRequirements == TaskRequirements.Research)
				{
					result += m_MaintainResearchGoal.PerformTaskNoResearch(stateSnapshot, result.missingRequirements, botCommands);
					didPerformResearchGoal = true;
				}
				
				// If true, this goal always blocks lower priority goals, regardless of whether the goal's task was completed successfully.
				if (goal.blockLowerPriority)
					break;
			}

			return result;
		}
	}
}
