using DotNetMissionSDK.AI.Combat.Groups;
using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.State.Snapshot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of meeting combat unit demand.
	/// </summary>
	public class MaintainArmyGoal : Goal
	{
		private MaintainVehicleFactoryTask m_MaintainVehicleFactoryTask;


		public MaintainArmyGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			m_Task = new BuildVehicleGroupTask(ownerID);
			m_Task.GeneratePrerequisites();

			m_MaintainVehicleFactoryTask = new MaintainVehicleFactoryTask(ownerID);
			m_MaintainVehicleFactoryTask.GeneratePrerequisites();
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
				if (player.playerID == m_OwnerID) continue;
				if (owner.allyPlayerIDs.Contains(player.playerID)) continue;

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
			PlayerState owner = stateSnapshot.players[m_OwnerID];

			// If all factories are occupied, increase number maintained
			int numBusy = owner.units.vehicleFactories.Count((factory) => factory.isEnabled && factory.isBusy);
			
			m_MaintainVehicleFactoryTask.targetCountToMaintain = numBusy + 1;

			// Build factory
			if (!m_MaintainVehicleFactoryTask.IsTaskComplete(stateSnapshot))
				m_MaintainVehicleFactoryTask.PerformTaskTree(stateSnapshot, unitActions);

			// Build army
			return m_Task.PerformTaskTree(stateSnapshot, unitActions);
		}

		/// <summary>
		/// Gets the list of structures to activate.
		/// </summary>
		/// <param name="stateSnapshot">The state snapshot to use for performing task calculations.</param>
		/// <param name="structureIDs">The list to add structures to.</param>
		public override void GetStructuresToActivate(StateSnapshot stateSnapshot, List<int> structureIDs)
		{
			base.GetStructuresToActivate(stateSnapshot, structureIDs);

			m_MaintainVehicleFactoryTask.GetStructuresToActivate(stateSnapshot, structureIDs);
		}

		public void SetVehicleGroupSlots(List<VehicleGroup.UnitSlot> unassignedCombatSlots)
		{
			BuildVehicleGroupTask combatGroupTask = (BuildVehicleGroupTask)m_Task;

			// Unassigned slots are returned in a prioritized order based on the ThreatZone.
			combatGroupTask.SetVehicleGroupSlots(unassignedCombatSlots);
		}
	}
}
