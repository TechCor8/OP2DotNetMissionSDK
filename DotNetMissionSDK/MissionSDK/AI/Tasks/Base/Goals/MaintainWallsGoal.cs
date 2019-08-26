using DotNetMissionSDK.AI.Tasks.Base.Maintenance;
using DotNetMissionSDK.State.Snapshot;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of building walls.
	/// </summary>
	public class MaintainWallsGoal : Goal
	{
		public MaintainWallsGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			m_Task = new MaintainWallsTask(ownerID);
			m_Task.GeneratePrerequisites();
		}

		/// <summary>
		/// Updates the importance of this goal.
		/// </summary>
		public override void UpdateImportance(StateSnapshot stateSnapshot, PlayerState owner)
		{
			importance = 1.0f;

			importance = Clamp(importance * weight);
		}
	}
}
