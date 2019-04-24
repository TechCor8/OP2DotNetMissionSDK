using DotNetMissionSDK.Utility;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks
{
	public abstract class Task
	{
		private Task m_Parent;
		private List<Task> m_Prerequisites = new List<Task>();
		
		protected PlayerInfo owner		{ get; private set; }


		// Constructor used by derived classes to avoid passing parameters that will be assigned by parent task
		protected Task() { }

		/// <summary>
		/// Creates a top-level task (goal).
		/// Derived classes will generate the necessary prerequisite tasks.
		/// </summary>
		/// <param name="owner">The player that performs this task.</param>
		public Task(PlayerInfo owner)
		{
			this.owner = owner;
		}

		/// <summary>
		/// Checks if this task is complete.
		/// </summary>
		/// <returns>True, if the task is complete.</returns>
		public abstract bool IsTaskComplete();

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
		public bool CanPerformTaskTree()
		{
			if (!CanPerformTask())
				return false;

			foreach (Task task in m_Prerequisites)
			{
				if (task.IsTaskComplete())
					continue;

				if (!task.CanPerformTaskTree())
					return false;
			}

			return true;
		}

		/// <summary>
		/// Checks if this task can be performed. Does not check if prerequisites are complete or can be performed.
		/// <para>WARNING: This method may not be implemented on all tasks!</para>
		/// </summary>
		/// <returns>True, if the task can be performed. Otherwise false.</returns>
		protected virtual bool CanPerformTask()
		{
			return true;
		}

		/// <summary>
		/// Performs this task and any underlying prerequisite tasks.
		/// You must call this method every update until the task is complete or no longer needed.
		/// </summary>
		/// <returns>True, if task is running. False, if task cannot be performed.</returns>
		public bool PerformTaskTree()
		{
			bool prerequisitesComplete = false;

			if (!PerformPrerequisites(out prerequisitesComplete))
				return false;

			if (prerequisitesComplete)
				return PerformTask();

			return true;
		}

		protected abstract bool PerformTask();

		/// <summary>
		/// Performs prerequisite tasks. If a task cannot be performed, returns false.
		/// </summary>
		/// <returns>False, if the task cannot be performed.</returns>
		private bool PerformPrerequisites(out bool prerequisitesComplete)
		{
			prerequisitesComplete = true;

			for (int i=0; i < m_Prerequisites.Count; ++i)
			{
				// Skip completed tasks
				if (m_Prerequisites[i].IsTaskComplete())
					continue;

				prerequisitesComplete = false;

				// Perform the task. Fail out if it can't be done.
				if (!m_Prerequisites[i].PerformTaskTree())
					return false;
			}

			return true;
		}

		/// <summary>
		/// Adds a prerequisite to this task.
		/// <para>
		/// If this task is an ancestor, the prerequisite will not be added.
		/// </para>
		/// </summary>
		/// <param name="prerequisite">The task to add as a prerequisite to this task. Should always be new().</param>
		protected void AddPrerequisite(Task prerequisite)
		{
			prerequisite.m_Parent = this;
			prerequisite.owner = owner;

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
	}
}
