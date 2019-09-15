using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetMissionSDK.AI.Tasks
{
	/// <summary>
	/// Represents materials that are required for a task to be completed.
	/// </summary>
	[Flags]
	public enum TaskRequirements
	{
		None		= 0,
		Common		= 1,
		Rare		= 2,
		Food		= 4,
		Research	= 8,
		All			= Common | Rare | Food | Research,
	}

	/// <summary>
	/// Contains info about the result of a performed task.
	/// </summary>
	public class TaskResult
	{
		private List<int> m_NeededResearchTopics = new List<int>();

		public bool didSucceed									{ get { return !HasAnyRequirement(missingRequirements, TaskRequirements.All); } }

		public TaskRequirements missingRequirements				{ get; private set; }
		public ReadOnlyCollection<int> neededResearchTopics		{ get; private set; }

		
		public TaskResult(TaskRequirements missingRequirements)
		{
			this.missingRequirements = missingRequirements;

			neededResearchTopics = m_NeededResearchTopics.AsReadOnly();
		}

		public TaskResult(TaskRequirements missingRequirements, int neededResearchTopic)
		{
			this.missingRequirements = missingRequirements;
			m_NeededResearchTopics.Add(neededResearchTopic);

			neededResearchTopics = m_NeededResearchTopics.AsReadOnly();
		}

		public static TaskResult operator+ (TaskResult a, TaskResult b)
		{
			TaskResult result = new TaskResult(a.missingRequirements | b.missingRequirements);
			
			result.m_NeededResearchTopics.AddRange(a.m_NeededResearchTopics);
			result.m_NeededResearchTopics.AddRange(b.m_NeededResearchTopics);

			result.neededResearchTopics = result.m_NeededResearchTopics.AsReadOnly();

			return result;
		}

		/// <summary>
		/// Checks if this player is the correct colony type, has completed the required research, and has the required resources to build a unit.
		/// </summary>
		public static bool CanBuildUnit(out TaskResult result, StateSnapshot stateSnapshot, PlayerState owner, TaskRequirements restrictedRequirements, map_id unitType, map_id cargoOrWeaponType=map_id.None)
		{
			result = new TaskResult(TaskRequirements.None);

			// Fail Check: Colony Type
			if (!owner.CanColonyUseUnit(stateSnapshot, unitType))
				return false;

			// Fail Check: Research
			if (!owner.HasTechnologyForUnit(stateSnapshot, unitType))
				result += new TaskResult(TaskRequirements.Research, stateSnapshot.GetGlobalUnitInfo(unitType).researchTopic);

			if (cargoOrWeaponType != map_id.None)
			{
				// Fail Check: Cargo Colony Type
				if (!owner.CanColonyUseUnit(stateSnapshot, cargoOrWeaponType))
					return false;

				// Fail Check: Cargo Research
				if (!owner.HasTechnologyForUnit(stateSnapshot, cargoOrWeaponType))
					result += new TaskResult(TaskRequirements.Research, stateSnapshot.GetGlobalUnitInfo(cargoOrWeaponType).researchTopic);

				UnitInfoState unitInfo = owner.GetUnitInfo(unitType);
				UnitInfoState cargoInfo = owner.GetUnitInfo(cargoOrWeaponType);

				TaskRequirements flags = TaskRequirements.None;

				// Fail Check: Unit Cost
				if (owner.ore < unitInfo.oreCost + cargoInfo.oreCost)				flags |= TaskRequirements.Common;
				if (owner.rareOre < unitInfo.rareOreCost + cargoInfo.rareOreCost)	flags |= TaskRequirements.Rare;

				// Fail Check: Restricted Requirements
				if (unitInfo.oreCost + cargoInfo.oreCost > 0 && HasAnyRequirement(restrictedRequirements, TaskRequirements.Common))
					flags |= TaskRequirements.Common;

				if (unitInfo.rareOreCost + cargoInfo.rareOreCost > 0 && HasAnyRequirement(restrictedRequirements, TaskRequirements.Rare))
					flags |= TaskRequirements.Rare;

				result += new TaskResult(flags);
			}
			else
			{
				UnitInfoState unitInfo = owner.GetUnitInfo(unitType);

				TaskRequirements flags = TaskRequirements.None;

				// Fail Check: Unit Cost
				if (owner.ore < unitInfo.oreCost)			flags |= TaskRequirements.Common;
				if (owner.rareOre < unitInfo.rareOreCost)	flags |= TaskRequirements.Rare;

				// Fail Check: Restricted Requirements
				if (unitInfo.oreCost > 0 && HasAnyRequirement(restrictedRequirements, TaskRequirements.Common))
					flags |= TaskRequirements.Common;

				if (unitInfo.rareOreCost > 0 && HasAnyRequirement(restrictedRequirements, TaskRequirements.Rare))
					flags |= TaskRequirements.Rare;

				result += new TaskResult(flags);
			}

			return result.didSucceed;
		}

		private static bool HasAnyRequirement(TaskRequirements requirements, TaskRequirements requirementsToCompare)
		{
			return (requirements & requirementsToCompare) != 0;
		}
	}
}
