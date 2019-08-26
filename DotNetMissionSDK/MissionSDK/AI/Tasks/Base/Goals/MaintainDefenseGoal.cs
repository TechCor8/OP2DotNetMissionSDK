using DotNetMissionSDK.AI.Tasks.Base.Maintenance;
using DotNetMissionSDK.State.Snapshot;
using System;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of building guard posts.
	/// </summary>
	public class MaintainDefenseGoal : Goal
	{
		public MaintainDefenseGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			m_Task = new MaintainDefenseTask(ownerID);
			m_Task.GeneratePrerequisites();
		}

		/// <summary>
		/// Updates the importance of this goal.
		/// </summary>
		public override void UpdateImportance(StateSnapshot stateSnapshot, PlayerState owner)
		{
			// Importance increases with enemy strength
			int highestStrength = 0;
			foreach (PlayerState player in stateSnapshot.players)
			{
				int strength = player.units.lynx.Count + player.units.panthers.Count + player.units.tigers.Count;
				if (strength > highestStrength)
					highestStrength = strength;
			}

			highestStrength /= 5; // 5 guard posts ideal per unit

			float minRange = highestStrength - 2;		// How much fewer guard posts than ideal before we take things seriously
			float maxRange = highestStrength + 4;		// How many excess guard posts before we stop caring
			importance = 1 - Clamp((owner.units.guardPosts.Count - minRange) / (maxRange - minRange));

			// Cap importance
			importance = Math.Min(importance, 0.95f);

			importance = Clamp(importance * weight);
		}
	}
}
