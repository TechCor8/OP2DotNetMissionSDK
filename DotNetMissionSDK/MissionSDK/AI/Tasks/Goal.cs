using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks
{
	/// <summary>
	/// A top-level task that gets assigned a priority based on importance.
	/// </summary>
	public abstract class Goal
	{
		protected int m_OwnerID;
		protected Task m_Task;
		
		public float weight				{ get; private set; }
		public float importance			{ get; protected set; }
		public bool blockLowerPriority	{ get; protected set; }


		public Goal(int ownerID, float weight)
		{
			m_OwnerID = ownerID;
			this.weight = weight;
		}

		/// <summary>
		/// Updates the importance of this goal.
		/// </summary>
		public abstract void UpdateImportance(StateSnapshot stateSnapshot, PlayerState owner);

		/// <summary>
		/// Performs this goal's task.
		/// </summary>
		/// <returns>Returns true if the task was performed and the next goal should be executed.</returns>
		public virtual bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			if (!m_Task.IsTaskComplete(stateSnapshot))
				return m_Task.PerformTaskTree(stateSnapshot, unitActions);

			return true;
		}

		/// <summary>
		/// Gets the list of structures to activate.
		/// </summary>
		/// <param name="stateSnapshot">The state snapshot to use for performing task calculations.</param>
		/// <param name="structureIDs">The list to add structures to.</param>
		public void GetStructuresToActivate(StateSnapshot stateSnapshot, List<int> structureIDs)
		{
			m_Task.GetStructuresToActivate(stateSnapshot, structureIDs);
		}

		protected float Clamp(float val)
		{
			if (val < 0) return 0;
			if (val > 1) return 1;
			return val;
		}
	}
}
