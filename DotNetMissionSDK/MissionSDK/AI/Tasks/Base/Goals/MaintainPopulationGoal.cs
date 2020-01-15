using DotNetMissionSDK.AI.Tasks.Base.Maintenance;
using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;
using System;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of growing the population.
	/// </summary>
	public class MaintainPopulationGoal : Goal
	{
		private ResearchSetTask m_ResearchSetTask;


		public MaintainPopulationGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			m_Task = new MaintainMoraleTask(ownerID);
			m_Task.GeneratePrerequisites();

			// Set research topics
			ResearchSetTask.ResearchTopic[] topicsToResearch = new ResearchSetTask.ResearchTopic[]
			{
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
			((MaintainMoraleTask)m_Task).UpdateRequirements(stateSnapshot);

			// Importance increases as morale declines
			switch (owner.moraleLevel)
			{
				case MoraleLevel.Excellent:		importance = 0.2f;		break;
				case MoraleLevel.Good:			importance = 0.4f;		break;
				case MoraleLevel.Fair:			importance = 0.6f;		break;
				case MoraleLevel.Poor:			importance = 0.8f;		break;
				case MoraleLevel.Terrible:		importance = 0.95f;		break;
			}

			// Importance skyrockets when we don't have baby and slave factories
			if (owner.units.nurseries.Count == 0 || owner.units.universities.Count == 0)
				importance = 1.0f;

			// Importance increases as labor shortage increases
			int workersNeeded = 0;
			int scientistsNeeded = 0;

			// Get labor needed for structures
			foreach (StructureState structure in owner.units.GetStructures())
			{
				StructureInfo info = owner.structureInfo[structure.unitType];

				if (!structure.hasWorkers) workersNeeded += info.workersRequired;
				if (!structure.hasScientists) scientistsNeeded += info.scientistsRequired;

				LabState lab = structure as LabState;
				if (lab != null)// && lab.isEnabled && lab.isBusy)
					scientistsNeeded += 10; // Use static amount to prevent priority "bounce"
			}

			workersNeeded -= owner.numAvailableWorkers;
			scientistsNeeded -= owner.numAvailableScientists;

			const int desiredUnemployment = 6;
			const float criticalShortage = 16;

			importance = Math.Max((workersNeeded + scientistsNeeded + desiredUnemployment) / criticalShortage, importance);

			importance = Clamp(importance * weight);
		}

		/// <summary>
		/// Performs this goal's task.
		/// </summary>
		public override TaskResult PerformTask(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions)
		{
			TaskResult result = base.PerformTask(stateSnapshot, restrictedRequirements, unitActions);

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
