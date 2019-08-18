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
		private Dictionary<int, List<TechInfo>> m_TechLevels = new Dictionary<int, List<TechInfo>>();

		private int m_CurrentTechLevel = 1;

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

				List<TechInfo> techList;

				if (!m_TechLevels.TryGetValue(info.GetTechLevel(), out techList))
				{
					techList = new List<TechInfo>();
					m_TechLevels.Add(info.GetTechLevel(), techList);
				}

				techList.Add(info);
			}
		}

		public void Update(StateSnapshot stateSnapshot)
		{
			ThreadAssert.MainThreadRequired();

			if (m_IsProcessing)
				return;

			m_IsProcessing = true;

			AsyncPump.Run(() =>
			{
				List<Action> buildingActions = new List<Action>();

				PlayerState owner = stateSnapshot.players[ownerID];

				// Check if tech level has been completed
				if (IsTechLevelResearched(owner, m_CurrentTechLevel))
					++m_CurrentTechLevel;

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
			});
		}

		private void SetTopicForLabs(StateSnapshot stateSnapshot, List<Action> buildingActions, ReadOnlyCollection<LabState> labs, LabType labType, ref int availableScientists)
		{
			foreach (LabState lab in labs)
			{
				if (availableScientists == 0)
					break;

				if (lab.isEnabled && !lab.isBusy)
				{
					int topic = GetNextResearchTopic(stateSnapshot, labType);
					if (topic <= 0)
						break;

					buildingActions.Add(() => GameState.GetUnit(lab.unitID)?.DoResearch(GetTechIndex(topic), 1));
					--availableScientists;
				}
			}
		}

		private int GetNextResearchTopic(StateSnapshot stateSnapshot, LabType labType)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			List<TechInfo> techList;
			if (!m_TechLevels.TryGetValue(m_CurrentTechLevel, out techList))
				return 0;

			// Find tech for this lab type that hasn't been research
			foreach (TechInfo info in techList)
			{
				int techID = info.GetTechID();

				// Skip tech that isn't available for this lab type
				if (info.GetLab() != labType)
					continue;

				// Skip tech if it is not available for this colony
				if (owner.isEden)
				{
					if (info.GetEdenCost() <= 0)
						continue;
				}
				else
				{
					if (info.GetPlymouthCost() <= 0)
						continue;
				}

				// Skip tech if this player has already researched this tech
				if (owner.HasTechnology(techID))
					continue;

				// Make sure another lab isn't researching this topic
				bool isBeingResearched = false;
				switch (labType)
				{
					case LabType.ltAdvanced:	isBeingResearched = IsLabResearchingTopic(owner.units.advancedLabs, techID);	break;
					case LabType.ltStandard:	isBeingResearched = IsLabResearchingTopic(owner.units.standardLabs, techID);	break;
					case LabType.ltBasic:		isBeingResearched = IsLabResearchingTopic(owner.units.basicLabs, techID);		break;
				}

				if (isBeingResearched)
					continue;

				// Found topic!
				return techID;
			}

			return 0;
		}

		private bool IsLabResearchingTopic(ReadOnlyCollection<LabState> labs, int techID)
		{
			foreach (LabState lab in labs)
			{
				if (lab.curAction != ActionType.moDoResearch)
					continue;

				if (Research.GetTechInfo(lab.labCurrentTopic).GetTechID() == techID)
					return true;
			}

			return false;
		}

		private bool IsTechLevelResearched(PlayerState owner, int techLevel)
		{
			List<TechInfo> techList;
			if (!m_TechLevels.TryGetValue(m_CurrentTechLevel, out techList))
				return true;

			foreach (TechInfo info in techList)
			{
				// Skip tech if it is not available for this colony
				if (owner.isEden)
				{
					if (info.GetEdenCost() <= 0)
						continue;
				}
				else
				{
					if (info.GetPlymouthCost() <= 0)
						continue;
				}

				if (!owner.HasTechnology(info.GetTechID()))
					return false;
			}

			return true;
		}

		private int GetTechIndex(int techID)
		{
			int techCount = Research.GetTechCount();
			for (int i=0; i < techCount; ++i)
			{
				TechInfo info = Research.GetTechInfo(i);
				if (info.GetTechID() == techID)
					return i;
			}

			return -1;
		}
	}
}
