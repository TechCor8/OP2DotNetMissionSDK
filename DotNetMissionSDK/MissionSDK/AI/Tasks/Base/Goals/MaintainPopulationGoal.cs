using DotNetMissionSDK.AI.Tasks.Base.Maintenance;
using DotNetMissionSDK.State.Snapshot;
using System;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of growing the population.
	/// </summary>
	public class MaintainPopulationGoal : Goal
	{
		public MaintainPopulationGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			m_Task = new MaintainMoraleTask(ownerID);
			m_Task.GeneratePrerequisites();
		}

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

			// Importance increases as spare labor dwindles
			importance = Math.Max(1 - Clamp(owner.numAvailableWorkers / 100.0f), importance);

			importance = Clamp(importance * weight);
		}
	}
}
