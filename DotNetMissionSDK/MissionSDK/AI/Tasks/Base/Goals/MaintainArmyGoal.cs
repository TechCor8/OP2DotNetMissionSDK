using DotNetMissionSDK.AI.Combat.Groups;
using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.ResearchInfo;
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
		private ResearchTask m_ResearchTask;

		private List<int> m_TopicsToResearch = new List<int>();


		public MaintainArmyGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			m_Task = new BuildVehicleGroupTask(ownerID);
			m_Task.GeneratePrerequisites();

			m_MaintainVehicleFactoryTask = new MaintainVehicleFactoryTask(ownerID);
			m_MaintainVehicleFactoryTask.GeneratePrerequisites();

			m_ResearchTask = new ResearchTask(ownerID, -1);
			m_ResearchTask.GeneratePrerequisites();

			// Set research topics
			m_TopicsToResearch = new List<int>(new int[]
			{
				GetUnitResearchTopic(map_id.Lynx),
				GetUnitResearchTopic(map_id.Laser),
				GetUnitResearchTopic(map_id.Microwave),
				GetUnitResearchTopic(map_id.EMP),
				GetUnitResearchTopic(map_id.RailGun),
				GetUnitResearchTopic(map_id.RPG),
				GetUnitResearchTopic(map_id.Spider),
				GetUnitResearchTopic(map_id.Panther),
				GetUnitResearchTopic(map_id.Starflare),
				GetUnitResearchTopic(map_id.Stickyfoam),
				GetUnitResearchTopic(map_id.Scorpion),
				GetUnitResearchTopic(map_id.ESG),
				GetUnitResearchTopic(map_id.Tiger),
				GetUnitResearchTopic(map_id.Supernova),
				GetUnitResearchTopic(map_id.AcidCloud),
				GetUnitResearchTopic(map_id.ThorsHammer)
			});
			// TODO: Add upgrade topics
		}

		private int GetUnitResearchTopic(map_id unitToResearch)
		{
			return new UnitInfo(unitToResearch).GetResearchTopic();
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
		public override TaskResult PerformTask(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[m_OwnerID];

			// Research weapon systems
			if (m_ResearchTask.IsTaskComplete(stateSnapshot))
			{
				int newTopic = GetNewResearchTopic(stateSnapshot, owner);
				if (newTopic >= 0)
					m_ResearchTask.topicToResearch = newTopic;
			}
			else
			{
				// Perform research
				m_ResearchTask.PerformTaskTree(stateSnapshot, restrictedRequirements, unitActions);
			}

			// If all factories are occupied, increase number maintained
			int numBusy = owner.units.vehicleFactories.Count((factory) => factory.isEnabled && factory.isBusy);
			
			m_MaintainVehicleFactoryTask.targetCountToMaintain = numBusy + 1;

			// Build factory
			if (!m_MaintainVehicleFactoryTask.IsTaskComplete(stateSnapshot))
				return m_MaintainVehicleFactoryTask.PerformTaskTree(stateSnapshot, restrictedRequirements, unitActions);

			// Build army
			return m_Task.PerformTaskTree(stateSnapshot, restrictedRequirements, unitActions);
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

			m_ResearchTask.GetStructuresToActivate(stateSnapshot, structureIDs);
		}

		public void SetVehicleGroupSlots(List<VehicleGroup.UnitSlot> unassignedCombatSlots)
		{
			BuildVehicleGroupTask combatGroupTask = (BuildVehicleGroupTask)m_Task;

			// Unassigned slots are returned in a prioritized order based on the ThreatZone.
			combatGroupTask.SetVehicleGroupSlots(unassignedCombatSlots);
		}

		private int GetNewResearchTopic(StateSnapshot stateSnapshot, PlayerState owner)
		{
			int topic = -1;

			// Go through topics until we find one we can research
			while (m_TopicsToResearch.Count > 0 && !CanColonyResearchTopic(stateSnapshot, owner.isEden, topic))
			{
				topic = m_TopicsToResearch[0];
				m_TopicsToResearch.RemoveAt(0);
			}

			return topic;
		}

		private bool CanColonyResearchTopic(StateSnapshot stateSnapshot, bool isEden, int topic)
		{
			if (topic < 0)
				return false;

			GlobalTechInfo info = stateSnapshot.techInfo[topic];

			if (isEden)
				return info.edenCost > 0;
			else
				return info.plymouthCost > 0;
		}
	}
}
