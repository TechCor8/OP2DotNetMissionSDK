using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.ResearchInfo;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	/// <summary>
	/// This task completes a research topic including all prerequisite research topics.
	/// </summary>
	public class ResearchTask : Task
	{
		public int topicToResearch;

		private List<int> m_RequiredResearchTopics = new List<int>();

		//private MaintainBasicLabTask m_MaintainBasicLab;
		//private MaintainStandardLabTask m_MaintainStandardLab;
		//private MaintainAdvancedLabTask m_MaintainAdvancedLab;

		protected override int typeID						{ get { return topicToResearch;								}	}
		
		
		public ResearchTask(int ownerID, int researchTopic) : base(ownerID) { topicToResearch = researchTopic; }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			// Invalid topics are assumed to be researched. Invalid topics may come from unit types that don't have a required topic such as "map_id.None".
			if (topicToResearch < 0)
				return true;

			PlayerState owner = stateSnapshot.players[ownerID];

			// If player has the technology, task is complete.
			if (owner.HasTechnologyByIndex(topicToResearch))
				return true;

			// Get required topics to research that are currently available
			m_RequiredResearchTopics = GetRequiredResearchTopics(stateSnapshot, owner, topicToResearch);

			// Make sure we maintain any labs required for topics
			/*m_MaintainBasicLab.targetCountToMaintain = 0;
			m_MaintainStandardLab.targetCountToMaintain = 0;
			m_MaintainAdvancedLab.targetCountToMaintain = 0;

			for (int i=0; i < m_RequiredResearchTopics.Count; ++i)
			{
				int topic = m_RequiredResearchTopics[i];
				TechInfo info = Research.GetTechInfo(topic);

				switch (info.GetLab())
				{
					case LabType.ltBasic:		m_MaintainBasicLab.targetCountToMaintain = 1;		break;
					case LabType.ltStandard:	m_MaintainStandardLab.targetCountToMaintain = 1;	break;
					case LabType.ltAdvanced:	m_MaintainAdvancedLab.targetCountToMaintain = 1;	break;
				}
			}*/

			return false;
		}

		public override void GeneratePrerequisites()
		{
			//if (topicToResearch < 0)
			//	return;

			// Generating prerequisites for research multiplies memory usage by 20x (1000 MB).
			// Let's trigger the MaintainResearchGoal instead.
			//AddPrerequisite(m_MaintainBasicLab = new MaintainBasicLabTask(ownerID));
			//AddPrerequisite(m_MaintainStandardLab = new MaintainStandardLabTask(ownerID));
			//AddPrerequisite(m_MaintainAdvancedLab = new MaintainAdvancedLabTask(ownerID));
		}

		protected override TaskResult PerformTask(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			List<int> researchTopics = m_RequiredResearchTopics;

			// Assign required topics to labs
			for (int i=0; i < researchTopics.Count; ++i)
			{
				int topic = researchTopics[i];
				GlobalTechInfo info = stateSnapshot.techInfo[topic];
				
				// Get labs of type for topic
				ReadOnlyCollection<LabState> labsForTopic = GetLabsByType(owner, info.labType);

				if (labsForTopic.Count == 0)
					return new TaskResult(TaskRequirements.Research, topic);

				// Skip topic if currently being researched
				if (IsLabResearchingTopic(labsForTopic, topic))
					continue;

				// Get available lab
				LabState availableLab = labsForTopic.FirstOrDefault((lab) => lab.isEnabled && !lab.isBusy);
				if (availableLab == null)
					continue;

				// Start research for topic
				unitActions.AddUnitCommand(availableLab.unitID, 1, () => GameState.GetUnit(availableLab.unitID)?.DoResearch(topic, 1));
			}

			return new TaskResult(TaskRequirements.None);
		}

		private List<int> GetRequiredResearchTopics(StateSnapshot stateSnapshot, PlayerState owner, int desiredTopic)
		{
			// If topic has already been researched, there are no required topics
			if (owner.HasTechnologyByIndex(desiredTopic))
				return new List<int>();

			GlobalTechInfo info = stateSnapshot.techInfo[desiredTopic];

			// If topic can't be researched, don't bother with it
			if (!CanColonyResearchTopic(owner.isEden, info))
				return new List<int>();

			List<int> requiredTopics = new List<int>();

			// Check all required topics
			int requiredCount = info.requiredTopics.Length;
			for (int i=0; i < requiredCount; ++i)
			{
				int topic = info.requiredTopics[i];
				
				// Get all required topics in unresearched topic
				requiredTopics.AddRange(GetRequiredResearchTopics(stateSnapshot, owner, topic));
			}

			// If there are topics to research before this one, return those...
			if (requiredTopics.Count > 0)
				return requiredTopics;

			// Otherwise, we return this topic.
			requiredTopics.Add(desiredTopic);
			return requiredTopics;
		}

		private bool CanColonyResearchTopic(bool isEden, GlobalTechInfo info)
		{
			if (isEden)
				return info.edenCost > 0;
			else
				return info.plymouthCost > 0;
		}

		private ReadOnlyCollection<LabState> GetLabsByType(PlayerState owner, LabType labType)
		{
			switch (labType)
			{
				case LabType.ltAdvanced:	return owner.units.advancedLabs;
				case LabType.ltStandard:	return owner.units.standardLabs;
				case LabType.ltBasic:		return owner.units.basicLabs;
			}

			throw new ArgumentOutOfRangeException("labType", labType, "Invalid labType!");
		}

		private bool IsLabResearchingTopic(ReadOnlyCollection<LabState> labs, int techIndex)
		{
			return GetLabResearchingTopic(labs, techIndex) != null;
		}

		private LabState GetLabResearchingTopic(ReadOnlyCollection<LabState> labs, int techIndex)
		{
			foreach (LabState lab in labs)
			{
				if (lab.curAction != ActionType.moDoResearch)
					continue;

				if (lab.labCurrentTopic == techIndex)
					return lab;
			}

			return null;
		}

		/// <summary>
		/// Gets the list of structures to activate.
		/// </summary>
		/// <param name="stateSnapshot">The state snapshot to use for performing task calculations.</param>
		/// <param name="structureIDs">The list to add structures to.</param>
		public override void GetStructuresToActivate(StateSnapshot stateSnapshot, List<int> structureIDs)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			List<int> researchTopics = m_RequiredResearchTopics;

			bool isLabResearchingOurTopic = false;

			// Get labs that are researching our topics
			for (int i=0; i < researchTopics.Count; ++i)
			{
				int topic = researchTopics[i];
				GlobalTechInfo info = stateSnapshot.techInfo[topic];
				
				// Get labs of type for topic
				ReadOnlyCollection<LabState> labsForTopic = GetLabsByType(owner, info.labType);

				// Get lab with topic. If topic is not being researched, there is no lab to activate
				LabState labAssignedTopic = GetLabResearchingTopic(labsForTopic, topic);
				if (labAssignedTopic == null)
					continue;

				isLabResearchingOurTopic = true;

				// Only add the lab if it isn't already being activated
				if (!structureIDs.Contains(labAssignedTopic.unitID))
					structureIDs.Add(labAssignedTopic.unitID);
			}

			if (!isLabResearchingOurTopic)
			{
				// Find a busy lab that we need and prioritize that one
				for (int i=0; i < researchTopics.Count; ++i)
				{
					int topic = researchTopics[i];
					GlobalTechInfo info = stateSnapshot.techInfo[topic];
					
					// Get labs of type for topic
					ReadOnlyCollection<LabState> labsForTopic = GetLabsByType(owner, info.labType);

					if (labsForTopic.Count == 0)
						continue;

					// Get busy lab
					LabState busyLab = labsForTopic.FirstOrDefault((lab) => lab.isEnabled && lab.isBusy);
					if (busyLab == null)
						continue;

					// Only add the lab if it isn't already being activated
					if (!structureIDs.Contains(busyLab.unitID))
						structureIDs.Add(busyLab.unitID);
				}
			}

			// Parse prerequisites
			base.GetStructuresToActivate(stateSnapshot, structureIDs);
		}
	}
}
