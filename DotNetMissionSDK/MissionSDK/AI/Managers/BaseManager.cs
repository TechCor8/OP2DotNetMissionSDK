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
using DotNetMissionSDK.AI.Tasks.Base.Goals;

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

		public BotPlayer botPlayer	{ get; private set; }
		public int ownerID			{ get; private set; }

		
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
				//new FixDisconnectedStructuresGoal(ownerID, 1),
				new RepairStructuresGoal(ownerID, 1),
				new MaintainFoodGoal(ownerID, 0.99f),
				new MaintainPopulationGoal(ownerID, 1),
				new MaintainResearchGoal(ownerID, 1),
				new MaintainDefenseGoal(ownerID, 0.75f),
				new MaintainWallsGoal(ownerID, 0.1f),
				new ExpandRareMiningGoal(ownerID, m_MiningBaseState, 0.97f),
				m_MaintainArmyGoal = new MaintainArmyGoal(ownerID, 0.98f),
				new LaunchStarshipGoal(ownerID, 1),
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

				UpdateGoal(stateSnapshot, botCommands);
				m_ResetVehicleTask.PerformTaskTree(stateSnapshot, botCommands); // Fix stuck vehicles

				return botCommands;
			},
			(object returnState) =>
			{
				m_IsProcessing = false;

				// Execute all completed actions
				BotCommands botCommands = (BotCommands)returnState;

				botCommands.Execute();
				
				stateSnapshot.Release();

				// DEBUG:
				if (ownerID == TethysGame.LocalPlayer())
				{
					TethysGame.AddMessage(ownerID, m_DebugMessage, ownerID, 0);
				}
			});
		}

		private void UpdateGoal(StateSnapshot stateSnapshot, BotCommands botCommands)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			List<Goal> goals = new List<Goal>(m_Goals);

			// Update importance
			foreach (Goal goal in goals)
				goal.UpdateImportance(stateSnapshot, owner);

			// Sort priority descending
			goals.Sort((Goal a, Goal b) => b.importance.CompareTo(a.importance));

			foreach (Goal goal in goals)
			{
				if (goal.importance < 0.0001f)
					continue;

				if (!goal.PerformTask(stateSnapshot, botCommands))
					break;

				if (goal.blockLowerPriority)
					break;
			}

			m_DebugMessage = goals[0].GetType().Name;
		}

		private string m_DebugMessage = "None";
	}
}
