using DotNetMissionSDK.AI.Managers;
using DotNetMissionSDK.AI.Tasks.Base.Mining;
using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of expanding rare mining operations.
	/// </summary>
	public class ExpandRareMiningGoal : Goal
	{
		private ResearchSetTask m_ResearchSetTask;


		public ExpandRareMiningGoal(int ownerID, MiningBaseState miningBaseState, float weight) : base(ownerID, weight)
		{
			m_Task = new ExpandRareMiningTask(ownerID, miningBaseState);
			m_Task.GeneratePrerequisites();

			// Set research topics
			ResearchSetTask.ResearchTopic[] topicsToResearch = new ResearchSetTask.ResearchTopic[]
			{
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(07201), false),	// Rare Ore Extraction
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(08103), true),		// Magma Refining
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(10301), false),	// Magma Purity Control
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(07203), false),	// Smelter Postprocessing
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
			// Importance increases as reserves dwindle
			importance = 1 - Clamp(owner.rareOre / 10000.0f);

			importance = Clamp(importance * weight);
		}

		/// <summary>
		/// Performs this goal's task.
		/// </summary>
		public override TaskResult PerformTask(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions)
		{
			TaskResult result = null;

			PlayerState owner = stateSnapshot.players[m_OwnerID];

			// Don't expand if smelters are disabled
			foreach (StructureState structure in owner.units.rareOreSmelters)
			{
				if (!structure.isEnabled && !structure.isCritical && stateSnapshot.commandMap.ConnectsTo(m_OwnerID, structure.position))
				{
					result = new TaskResult(TaskRequirements.None);
					break;
				}
			}

			// Expand mining if smelters are all enabled
			if (result == null)
				result = base.PerformTask(stateSnapshot, restrictedRequirements, unitActions);

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

			m_ResearchSetTask.GetStructuresToActivate(stateSnapshot, structureIDs);
		}
	}
}
