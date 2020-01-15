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
		private ResearchSetTask m_ResearchSetTask;

		
		public MaintainArmyGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			m_Task = new BuildVehicleGroupTask(ownerID);
			m_Task.GeneratePrerequisites();

			m_MaintainVehicleFactoryTask = new MaintainVehicleFactoryTask(ownerID);
			m_MaintainVehicleFactoryTask.GeneratePrerequisites();

			// Set research topics
			int[] topicsToResearch = new int[]
			{
				GetUnitResearchTopic(map_id.Lynx),
				GetUnitResearchTopic(map_id.Laser),
				GetUnitResearchTopic(map_id.Microwave),
				GetTopicFromTechID(05111),	// Independent Turret Power Systems
				GetTopicFromTechID(07206),	// Scout-class Drive Train Refit
				GetUnitResearchTopic(map_id.EMP),
				GetUnitResearchTopic(map_id.RailGun),
				GetTopicFromTechID(07403),	// Increased Capacitance Circuitry
				GetUnitResearchTopic(map_id.RPG),
				GetUnitResearchTopic(map_id.Spider),
				GetUnitResearchTopic(map_id.Panther),
				GetTopicFromTechID(08319),	// Spider Maintenance Software Revision
				GetTopicFromTechID(08309),	// Reinforced Panther Construction
				GetUnitResearchTopic(map_id.Starflare),
				GetUnitResearchTopic(map_id.Stickyfoam),
				GetTopicFromTechID(08320),	// Reduced Foam Evaporation
				GetUnitResearchTopic(map_id.Scorpion),
				GetTopicFromTechID(08321),	// Arachnid Durability
				GetTopicFromTechID(07405),	// Scorpion Power Systems
				GetUnitResearchTopic(map_id.ESG),
				GetUnitResearchTopic(map_id.Tiger),
				GetTopicFromTechID(10303),	// Advanced Armoring Systems
				GetUnitResearchTopic(map_id.Supernova),
				GetUnitResearchTopic(map_id.AcidCloud),
				GetUnitResearchTopic(map_id.ThorsHammer),
				GetTopicFromTechID(12201),	// Rocket Atmospheric Re-entry System
				GetTopicFromTechID(05601),	// Heat Dissipation Systems
				GetTopicFromTechID(12101),	// Heat Dissipation Systems
				GetTopicFromTechID(07211),	// Extended-Range Projectile Launcher
				GetTopicFromTechID(07212),	// Extended-Range Projectile Launcher
				GetTopicFromTechID(10306),	// Grenade Loading Mechanism
				GetTopicFromTechID(10305),	// Grenade Loading Mechanism
				GetTopicFromTechID(05318),	// Robotic Image Processing
			};

			m_ResearchSetTask = new ResearchSetTask(ownerID, topicsToResearch);
			m_ResearchSetTask.GeneratePrerequisites();
		}

		private int GetUnitResearchTopic(map_id unitToResearch)		{ return new UnitInfo(unitToResearch).GetResearchTopic();			}
		private int GetTopicFromTechID(int techID)					{ return Research.GetTechIndexByTechID(techID);						}

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

			float minRange = System.Math.Max(0, highestStrength - 2);	// How much less strength than our opponent before we take things seriously
			float maxRange = highestStrength + 20;						// How much extra strength than our opponent before we stop caring
			importance = 1 - Clamp((owner.totalOffensiveStrength - minRange) / (maxRange - minRange));

			importance = Clamp(importance * weight);
		}

		/// <summary>
		/// Performs this goal's task.
		/// </summary>
		public override TaskResult PerformTask(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions)
		{
			TaskResult result;

			PlayerState owner = stateSnapshot.players[m_OwnerID];

			// If all factories are occupied, increase number maintained
			int numBusy = owner.units.vehicleFactories.Count((factory) => factory.isEnabled && factory.isBusy);
			
			m_MaintainVehicleFactoryTask.targetCountToMaintain = numBusy + 1;

			// Build factory
			if (!m_MaintainVehicleFactoryTask.IsTaskComplete(stateSnapshot))
				result = m_MaintainVehicleFactoryTask.PerformTaskTree(stateSnapshot, restrictedRequirements, unitActions);
			else
			{
				// Build army
				result = m_Task.PerformTaskTree(stateSnapshot, restrictedRequirements, unitActions);
			}

			// Perform research
			if (!m_ResearchSetTask.IsTaskComplete(stateSnapshot))
				m_ResearchSetTask.PerformTaskTree(stateSnapshot, restrictedRequirements, unitActions);

			return result;
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

			m_ResearchSetTask.GetStructuresToActivate(stateSnapshot, structureIDs);
		}

		public void SetVehicleGroupSlots(List<VehicleGroup.UnitSlot> unassignedCombatSlots)
		{
			BuildVehicleGroupTask combatGroupTask = (BuildVehicleGroupTask)m_Task;

			// Unassigned slots are returned in a prioritized order based on the ThreatZone.
			combatGroupTask.SetVehicleGroupSlots(unassignedCombatSlots);
		}
	}
}
