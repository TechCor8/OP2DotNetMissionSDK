using DotNetMissionSDK.AI.Tasks.Base.Maintenance;
using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Snapshot;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of being able to research topics.
	/// </summary>
	public class MaintainResearchGoal : Goal
	{
		private ResearchSetTask m_ResearchSetTask;


		public MaintainResearchGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			m_Task = new MaintainResearchTask(ownerID);
			m_Task.GeneratePrerequisites();

			// Set research topics
			ResearchSetTask.ResearchTopic[] topicsToResearch = new ResearchSetTask.ResearchTopic[]
			{
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(05309), false),	// Hypnopaedia
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(05310), false),	// Hypnopaedia
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(03201), false),	// Seismology
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(03202), false),	// Vulcanology
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(05302), false),	// Meteorology
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(08316), false),	// Meteor Detection
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(05303), false),	// Severe Atmospheric Disturbances
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(05306), false),	// Recycler Postprocessing
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(07203), false),	// Smelter Postprocessing
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(08306), false),	// Enhanced Defensive Fortifications
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(07213), false),	// Advanced Robotic Manipulator Arm
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(07214), false),	// Advanced Robotic Manipulator Arm
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(07202), false),	// Hot-Cracking Column Efficiency
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(05107), false),	// "Magnetohydrodynamics
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(08304), false),	// Heat Mining
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(10204), false),	// Solar Power
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(10309), false),	// Precision Trajectory Projection Software
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(10101), false),	// Improved Launch Vehicle
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(08301), false),	// Efficiency Engineering
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(08302), false),	// Efficiency Engineering
				new ResearchSetTask.ResearchTopic(GetTopicFromTechID(05701), false),	// Lava Defenses
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
			if (owner.units.standardLabs.Count == 0)
				importance = 0.8f;
			else if (owner.units.advancedLabs.Count == 0)
				importance = 0.7f;
			else if (owner.units.advancedLabs.Count > 0 && !m_Task.IsTaskComplete(stateSnapshot))
				importance = 0.3f;

			importance = Clamp(importance * weight);
		}

		/// <summary>
		/// Performs this goal's task.
		/// </summary>
		public override TaskResult PerformTask(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions)
		{
			TaskResult result;

			result = base.PerformTask(stateSnapshot, restrictedRequirements, unitActions);

			// Perform research
			if (!m_ResearchSetTask.IsTaskComplete(stateSnapshot))
				m_ResearchSetTask.PerformTaskTree(stateSnapshot, restrictedRequirements, unitActions);

			return result;
		}

		/// <summary>
		/// Performs the lab maintenance task but skips research.
		/// </summary>
		public TaskResult PerformTaskNoResearch(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions)
		{
			return base.PerformTask(stateSnapshot, restrictedRequirements, unitActions);
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
