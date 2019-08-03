using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Managers
{
	/// <summary>
	/// Assigns research topics to labs.
	/// </summary>
	public class ResearchManager
	{
		private Dictionary<int, List<TechInfo>> m_TechLevels = new Dictionary<int, List<TechInfo>>();

		private int m_CurrentTechLevel = 1;

		public BotPlayer botPlayer	{ get; private set; }
		public PlayerInfo owner		{ get; private set; }

		
		public ResearchManager(BotPlayer botPlayer, PlayerInfo owner)
		{
			this.botPlayer = botPlayer;
			this.owner = owner;

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

		public void Update()
		{
			// Check if tech level has been completed
			if (IsTechLevelResearched(m_CurrentTechLevel))
				++m_CurrentTechLevel;

			int availableScientists = owner.player.GetNumAvailableScientists();

			// Set topics for labs
			SetTopicForLabs(owner.units.advancedLabs, LabType.ltAdvanced, ref availableScientists);
			SetTopicForLabs(owner.units.standardLabs, LabType.ltStandard, ref availableScientists);
			SetTopicForLabs(owner.units.basicLabs, LabType.ltBasic, ref availableScientists);
		}

		private void SetTopicForLabs(List<UnitEx> labs, LabType labType, ref int availableScientists)
		{
			foreach (UnitEx lab in labs)
			{
				if (availableScientists == 0)
					break;

				if (lab.IsEnabled() && !lab.IsBusy())
				{
					int topic = GetNextResearchTopic(labType);
					if (topic <= 0)
						break;

					lab.DoResearch(GetTechIndex(topic), 1);
					--availableScientists;
				}
			}
		}

		private int GetNextResearchTopic(LabType labType)
		{
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
				if (owner.player.IsEden())
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
				if (owner.player.HasTechnology(techID))
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

		private bool IsLabResearchingTopic(List<UnitEx> labs, int techID)
		{
			foreach (UnitEx lab in labs)
			{
				if (lab.GetCurAction() != ActionType.moDoResearch)
					continue;

				if (Research.GetTechInfo(lab.GetLabCurrentTopic()).GetTechID() == techID)
					return true;
			}

			return false;
		}

		private bool IsTechLevelResearched(int techLevel)
		{
			List<TechInfo> techList;
			if (!m_TechLevels.TryGetValue(m_CurrentTechLevel, out techList))
				return true;

			foreach (TechInfo info in techList)
			{
				// Skip tech if it is not available for this colony
				if (owner.player.IsEden())
				{
					if (info.GetEdenCost() <= 0)
						continue;
				}
				else
				{
					if (info.GetPlymouthCost() <= 0)
						continue;
				}

				if (!owner.player.HasTechnology(info.GetTechID()))
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
