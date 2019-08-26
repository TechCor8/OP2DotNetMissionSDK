using DotNetMissionSDK.AI.Tasks.Base.Maintenance;
using DotNetMissionSDK.State.Snapshot;
using System;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of meeting food demand.
	/// </summary>
	public class MaintainFoodGoal : Goal
	{
		public MaintainFoodGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			m_Task = new MaintainFoodTask(ownerID);
			m_Task.GeneratePrerequisites();
		}

		/// <summary>
		/// Updates the importance of this goal.
		/// </summary>
		public override void UpdateImportance(StateSnapshot stateSnapshot, PlayerState owner)
		{
			const int minimumTimeUnconcerned = 100; // If time until starvation is >= this value, bot is completely unconcerned.

			// Importance increases as reserves dwindle
			int timeUntilStarvation = minimumTimeUnconcerned;

			if (owner.netFoodProduction != 0)
				timeUntilStarvation = owner.foodStored / Math.Abs(owner.netFoodProduction);

			importance = 1 - Clamp(timeUntilStarvation / minimumTimeUnconcerned);

			// If food supply in surplus, take starvation importance and combine it with how critical our last agridome is.
			// If we have at least 1 agridome worth of net surplus, this is not important.
			// If we are running the net surplus close to 0, we are at increased risk if an agridome goes down, especially if we are low on food.
			if (owner.foodSupply == FoodStatus.Rising)
				importance = (1 - Clamp((float)owner.netFoodProduction / owner.structureInfo[map_id.Agridome].productionRate)) * importance;

			importance = Clamp(importance * weight);
		}

		/// <summary>
		/// Performs this goal's task.
		/// </summary>
		public override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			return m_Task.PerformTaskTree(stateSnapshot, unitActions);
		}
	}
}
