using DotNetMissionSDK.State.Snapshot;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks
{
	public abstract class Task
	{
		private Task m_Parent;
		private List<Task> m_Prerequisites = new List<Task>();
		private bool waitForSiblingPrerequisites;
		
		protected int ownerID								{ get; private set; }
		protected IReadOnlyCollection<Task> prerequisites	{ get { return m_Prerequisites.AsReadOnly(); } }


		/// <summary>
		/// Creates a top-level task (goal).
		/// Derived classes will generate the necessary prerequisite tasks.
		/// </summary>
		/// <param name="ownerID">The player that performs this task.</param>
		public Task(int ownerID)
		{
			this.ownerID = ownerID;
		}

		/// <summary>
		/// Checks if this task is complete.
		/// </summary>
		/// <param name="stateSnapshot">The state snapshot to use for performing task calculations.</param>
		/// <returns>True, if the task is complete.</returns>
		public abstract bool IsTaskComplete(StateSnapshot stateSnapshot);

		/// <summary>
		/// Creates all prerequisite tasks for this task.
		/// </summary>
		public abstract void GeneratePrerequisites();

		/// <summary>
		/// Checks if this task and any incomplete prerequisites can be performed.
		/// If this task cannot be performed or any incomplete prerequisites cannot be performed, returns false.
		/// <para>WARNING: Relies on CanPerformTask() which may not be implemented on all tasks!</para>
		/// </summary>
		/// <returns>True, if the task tree can be performed.</returns>
		public bool CanPerformTaskTree(StateSnapshot stateSnapshot)
		{
			if (!CanPerformTask(stateSnapshot))
				return false;

			foreach (Task task in m_Prerequisites)
			{
				if (task.IsTaskComplete(stateSnapshot))
					continue;

				if (!task.CanPerformTaskTree(stateSnapshot))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Checks if this task can be performed. Does not check if prerequisites are complete or can be performed.
		/// <para>WARNING: This method may not be implemented on all tasks!</para>
		/// </summary>
		/// <returns>True, if the task can be performed. Otherwise false.</returns>
		protected virtual bool CanPerformTask(StateSnapshot stateSnapshot)
		{
			return true;
		}
		
		/// <summary>
		/// Performs this task and any underlying prerequisite tasks.
		/// You must call this method every update until the task is complete or no longer needed.
		/// </summary>
		/// <param name="stateSnapshot">The state snapshot to use for performing task calculations.</param>
		/// <param name="unitActions">Actions to be performed by units are added to this list rather than executed directly.</param>
		/// <returns>TaskResult that provides info about the task state.</returns>
		//public TaskResult PerformTaskTree(StateSnapshot stateSnapshot, BotCommands unitActions)
		//{
		//	return PerformTaskTree(stateSnapshot, TaskRequirements.None, unitActions);
		//}

		/// <summary>
		/// Performs this task and any underlying prerequisite tasks.
		/// You must call this method every update until the task is complete or no longer needed.
		/// </summary>
		/// <param name="stateSnapshot">The state snapshot to use for performing task calculations.</param>
		/// <param name="restrictedRequirements">Task materials that the task cannot use.</param>
		/// <param name="unitActions">Actions to be performed by units are added to this list rather than executed directly.</param>
		/// <returns>TaskResult that provides info about the task state.</returns>
		public TaskResult PerformTaskTree(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions)
		{
			bool prerequisitesComplete = false;

			TaskResult result = PerformPrerequisites(stateSnapshot, restrictedRequirements, unitActions, out prerequisitesComplete);
			
			if (prerequisitesComplete)
				return PerformTask(stateSnapshot, restrictedRequirements, unitActions);

			return result;
		}

		protected abstract TaskResult PerformTask(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions);

		/// <summary>
		/// Performs prerequisite tasks. If a task cannot be performed, returns false.
		/// </summary>
		/// <returns>TaskResult that provides info about the task state.</returns>
		private TaskResult PerformPrerequisites(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions, out bool prerequisitesComplete)
		{
			TaskResult result = new TaskResult(TaskRequirements.None);
			prerequisitesComplete = true;

			for (int i=0; i < m_Prerequisites.Count; ++i)
			{
				// Skip completed tasks
				if (m_Prerequisites[i].IsTaskComplete(stateSnapshot))
					continue;

				// If task has been set to wait for siblings, skip remaining prerequisites.
				if (m_Prerequisites[i].waitForSiblingPrerequisites && !prerequisitesComplete)
					break;

				prerequisitesComplete = false;

				// Perform the task. Combine the results.
				result += m_Prerequisites[i].PerformTaskTree(stateSnapshot, restrictedRequirements, unitActions);
			}

			return result;
		}

		/// <summary>
		/// Adds a prerequisite to this task.
		/// <para>
		/// If this task is an ancestor, the prerequisite will not be added.
		/// </para>
		/// </summary>
		/// <param name="prerequisite">The task to add as a prerequisite to this task. Should always be new().</param>
		/// <param name="waitForSiblingPrerequisites">If true, this task will wait for previously added prerequisites to complete before executing.</param>
		protected void AddPrerequisite(Task prerequisite, bool waitForSiblingPrerequisites=false)
		{
			prerequisite.m_Parent = this;
			prerequisite.ownerID = ownerID;
			prerequisite.waitForSiblingPrerequisites = waitForSiblingPrerequisites;

			if (prerequisite.IsAncestorTask(prerequisite))
				return;

			prerequisite.GeneratePrerequisites();

			m_Prerequisites.Add(prerequisite);
		}

		/// <summary>
		/// Checks if a task is an ancestor of this task.
		/// </summary>
		/// <param name="task">The task to check the heritage of.</param>
		/// <returns>True, if the passed in task is an ancestor of this task.</returns>
		private bool IsAncestorTask(Task task)
		{
			if (m_Parent == null)
				return false;

			if (task.GetType() == m_Parent.GetType())
				return true;

			return m_Parent.IsAncestorTask(task);
		}

		/// <summary>
		/// Gets the list of structures to activate.
		/// </summary>
		/// <param name="stateSnapshot">The state snapshot to use for performing task calculations.</param>
		/// <param name="structureIDs">The list to add structures to.</param>
		public virtual void GetStructuresToActivate(StateSnapshot stateSnapshot, List<int> structureIDs)
		{
			// Process all children
			for (int i=0; i < m_Prerequisites.Count; ++i)
				m_Prerequisites[i].GetStructuresToActivate(stateSnapshot, structureIDs);
		}

		protected bool IsRequirementRestricted(TaskRequirements restrictedRequirements, TaskRequirements neededRequirement)
		{
			return (restrictedRequirements & neededRequirement) != 0;
		}
	}
}
