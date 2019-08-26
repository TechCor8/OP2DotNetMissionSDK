using DotNetMissionSDK.AI.Tasks.Base.Maintenance;
using DotNetMissionSDK.State.Snapshot;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of meeting power demand.
	/// </summary>
	public class MaintainPowerGoal : Goal
	{
		public MaintainPowerGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			m_Task = new MaintainPowerTask(ownerID);
			m_Task.GeneratePrerequisites();
		}

		/// <summary>
		/// Updates the importance of this goal.
		/// </summary>
		public override void UpdateImportance(StateSnapshot stateSnapshot, PlayerState owner)
		{
			// Importance increases as reserves dwindle
			importance = 1 - Clamp(owner.amountPowerAvailable / 1000.0f);

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
