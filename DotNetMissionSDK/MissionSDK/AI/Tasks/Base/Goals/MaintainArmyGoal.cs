using DotNetMissionSDK.AI.Combat.Groups;
using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.State.Snapshot;
using System;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of meeting combat unit demand.
	/// </summary>
	public class MaintainArmyGoal : Goal
	{
		public MaintainArmyGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			m_Task = new BuildVehicleGroupTask(ownerID);
			m_Task.GeneratePrerequisites();
		}

		/// <summary>
		/// Updates the importance of this goal.
		/// </summary>
		public override void UpdateImportance(StateSnapshot stateSnapshot, PlayerState owner)
		{
			// Importance increases with enemy strength
			int highestStrength = 0;
			foreach (PlayerState player in stateSnapshot.players)
			{
				if (player.playerID == m_OwnerID)
					continue;

				if (player.totalOffensiveStrength > highestStrength)
					highestStrength = player.totalOffensiveStrength;
			}

			float minRange = highestStrength - 2;		// How much less strength than our opponent before we take things seriously
			float maxRange = highestStrength + 20;		// How much extra strength than our opponent before we stop caring
			importance = 1 - Clamp((owner.totalOffensiveStrength - minRange) / (maxRange - minRange));

			importance = Clamp(importance * weight);
		}

		/// <summary>
		/// Performs this goal's task.
		/// </summary>
		public override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			return m_Task.PerformTaskTree(stateSnapshot, unitActions);
		}

		public void SetVehicleGroupSlots(List<VehicleGroup.UnitSlot> unassignedCombatSlots)
		{
			BuildVehicleGroupTask combatGroupTask = (BuildVehicleGroupTask)m_Task;

			// Unassigned slots are returned in a prioritized order based on the ThreatZone.
			combatGroupTask.SetVehicleGroupSlots(unassignedCombatSlots);
		}
	}
}
