using DotNetMissionSDK.AI.Tasks.Base.Starship;
using DotNetMissionSDK.State.Snapshot;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of launching the starship.
	/// </summary>
	public class LaunchStarshipGoal : Goal
	{
		public LaunchStarshipGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			m_Task = new DeployEvacModuleTask(ownerID);
			m_Task.GeneratePrerequisites();
		}

		/// <summary>
		/// Updates the importance of this goal.
		/// </summary>
		public override void UpdateImportance(StateSnapshot stateSnapshot, PlayerState owner)
		{
			float highestProgress = 0;

			foreach (PlayerState player in stateSnapshot.players)
			{
				if (player.playerID == owner.playerID)
					continue;

				if (player.starship.progress > highestProgress)
					highestProgress = player.starship.progress;
			}

			// Importance increases as opponent gets ahead
			float minRange = highestProgress - 0.1f;	// How far behind opponent before we take things seriously
			float maxRange = highestProgress + 0.25f;	// How far ahead of opponent before we stop caring
			importance = 1 - ((owner.starship.progress - minRange) / (maxRange - minRange));

			// Should always have some importance, but should never have overwhelming importance.
			if (importance < 0.2f)
				importance = 0.2f;
			else if (importance > 0.9f)
				importance = 0.9f;

			importance = Clamp(importance * weight);
		}
	}
}
