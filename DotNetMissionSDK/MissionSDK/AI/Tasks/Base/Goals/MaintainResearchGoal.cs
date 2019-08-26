using DotNetMissionSDK.AI.Tasks.Base.Maintenance;
using DotNetMissionSDK.State.Snapshot;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of being able to research topics.
	/// </summary>
	public class MaintainResearchGoal : Goal
	{
		public MaintainResearchGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			m_Task = new MaintainResearchTask(ownerID);
			m_Task.GeneratePrerequisites();
		}

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
	}
}
