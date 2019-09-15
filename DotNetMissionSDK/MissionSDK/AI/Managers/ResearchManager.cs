using DotNetMissionSDK.Async;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetMissionSDK.AI.Managers
{
	/// <summary>
	/// Assigns research topics to labs.
	/// </summary>
	public class ResearchManager
	{
		private Dictionary<int, int> m_TechIDToTopicLookup = new Dictionary<int, int>();

		private List<int> m_PriorityResearchTopics = new List<int>();

		private bool m_IsProcessing;

		public BotPlayer botPlayer	{ get; private set; }
		public int ownerID			{ get; private set; }

		
		public ResearchManager(BotPlayer botPlayer, int ownerID)
		{
			this.botPlayer = botPlayer;
			this.ownerID = ownerID;

			// Add all tech to tech levels table
			int techCount = Research.GetTechCount();
			for (int i=0; i < techCount; ++i)
			{
				TechInfo info = Research.GetTechInfo(i);

				// Add tech ID and index to lookup table
				m_TechIDToTopicLookup.Add(info.GetTechID(), i);
			}
		}

		public void Update(StateSnapshot stateSnapshot)
		{
			ThreadAssert.MainThreadRequired();

			if (m_IsProcessing)
				return;

			m_IsProcessing = true;

			// Get data that requires main thread
			List<int> topPriorityResearchTopics = new List<int>(botPlayer.baseManager.GetResearchTopicPriority());

			stateSnapshot.Retain();

			AsyncPump.Run(() =>
			{
				List<Action> buildingActions = new List<Action>();

				PlayerState owner = stateSnapshot.players[ownerID];

				// Process top-level priority research topics into required topics
				m_PriorityResearchTopics.Clear();

				foreach (int topic in topPriorityResearchTopics)
					m_PriorityResearchTopics.AddRange(GetRequiredResearchTopics(owner, topic));


				int availableScientists = owner.numAvailableScientists;

				// Set topics for labs
				SetTopicForLabs(stateSnapshot, buildingActions, owner.units.advancedLabs, LabType.ltAdvanced, ref availableScientists);
				SetTopicForLabs(stateSnapshot, buildingActions, owner.units.standardLabs, LabType.ltStandard, ref availableScientists);
				SetTopicForLabs(stateSnapshot, buildingActions, owner.units.basicLabs, LabType.ltBasic, ref availableScientists);

				return buildingActions;
			},
			(object returnState) =>
			{
				m_IsProcessing = false;

				// Execute all completed actions
				List<Action> buildingActions = (List<Action>)returnState;

				foreach (Action action in buildingActions)
					action();

				stateSnapshot.Release();
			});
		}

		private List<int> GetRequiredResearchTopics(PlayerState owner, int desiredTopic)
		{
			if (owner.HasTechnologyByIndex(desiredTopic))
				return new List<int>();

			TechInfo info = Research.GetTechInfo(desiredTopic);

			List<int> requiredTopics = new List<int>();

			// Check all required topics
			int requiredCount = info.GetNumRequiredTechs();
			for (int i=0; i < requiredCount; ++i)
			{
				int topic = info.GetRequiredTechIndex(i);
				if (owner.HasTechnologyByIndex(topic))
					continue;

				// Get all required topics in unresearched topic
				requiredTopics.AddRange(GetRequiredResearchTopics(owner, topic));
			}

			// If there are topics to research before this one, return those...
			if (requiredTopics.Count > 0)
				return requiredTopics;

			// Otherwise, we return this topic.
			requiredTopics.Add(desiredTopic);
			return requiredTopics;
		}

		private void SetTopicForLabs(StateSnapshot stateSnapshot, List<Action> buildingActions, ReadOnlyCollection<LabState> labs, LabType labType, ref int availableScientists)
		{
			foreach (LabState lab in labs)
			{
				if (availableScientists == 0)
					break;

				// Find lab that isn't doing anything
				if (lab.isEnabled && !lab.isBusy)
				{
					int topic = GetNextResearchTopic(stateSnapshot, labType);
					if (topic < 0)
						break;

					buildingActions.Add(() => GameState.GetUnit(lab.unitID)?.DoResearch(topic, 1));
					--availableScientists;
				}
			}
		}

		private int GetNextResearchTopic(StateSnapshot stateSnapshot, LabType labType)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Get priority technology
			for (int i=0; i < m_PriorityResearchTopics.Count; ++i)
			{
				int topic = m_PriorityResearchTopics[i];

				TechInfo info = Research.GetTechInfo(topic);

				// Skip tech that isn't available for this lab type
				if (info.GetLab() != labType)
					continue;

				m_PriorityResearchTopics.RemoveAt(i--);

				// Skip tech if it is not available for this colony
				if (!CanColonyResearchTopic(owner.isEden, info))
					continue;

				// Make sure another lab isn't researching this topic
				if (IsLabResearchingTopic(owner, topic))
					continue;

				return topic;
			}
			
			return -1;
		}

		private bool CanColonyResearchTopic(bool isEden, TechInfo info)
		{
			if (isEden)
				return info.GetEdenCost() > 0;
			else
				return info.GetPlymouthCost() > 0;
		}

		private bool IsLabResearchingTopic(PlayerState owner, int techIndex)
		{
			TechInfo info = Research.GetTechInfo(techIndex);
			LabType labType = info.GetLab();
			
			switch (labType)
			{
				case LabType.ltAdvanced:	return IsLabResearchingTopic(owner.units.advancedLabs, techIndex);
				case LabType.ltStandard:	return IsLabResearchingTopic(owner.units.standardLabs, techIndex);
				case LabType.ltBasic:		return IsLabResearchingTopic(owner.units.basicLabs, techIndex);
			}

			return false;
		}

		private bool IsLabResearchingTopic(ReadOnlyCollection<LabState> labs, int techIndex)
		{
			foreach (LabState lab in labs)
			{
				if (lab.curAction != ActionType.moDoResearch)
					continue;

				if (lab.labCurrentTopic == techIndex)
					return true;
			}

			return false;
		}

		/*private int GetTechIndex(int techID)
		{
			int techCount = Research.GetTechCount();
			for (int i=0; i < techCount; ++i)
			{
				TechInfo info = Research.GetTechInfo(i);
				if (info.GetTechID() == techID)
					return i;
			}

			return -1;
		}*/
	}
}
