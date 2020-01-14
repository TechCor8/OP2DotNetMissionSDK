using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetMissionSDK.Async
{
	/// <summary>
	/// Runs tasks asynchronously before synchronizing them to a target tick.
	/// Used to to keep asynchronous operations predictable on the game timeline.
	/// </summary>
	public class AsyncPump
	{
		private class AsyncOperation
		{
			public int targetTime					{ get; private set; }
			public CompletedCallback completedCB	{ get; private set; }
			public Task task;
			public object returnState;

			public AsyncOperation(int targetTime, CompletedCallback completedCB)
			{
				this.targetTime = targetTime;
				this.completedCB = completedCB;
			}
		}

		private static readonly object m_SyncCompleted = new object();
		private static List<AsyncOperation> m_PendingOperations = new List<AsyncOperation>();
		private static List<AsyncOperation> m_CompletedOperations = new List<AsyncOperation>();

		public delegate object ActionCallback();
		public delegate void CompletedCallback(object returnState);


		/// <summary>
		/// Runs the specified action asynchronously.
		/// Executes completedCB upon completion of the action.
		/// NOTE: Must be run from main thread.
		/// </summary>
		/// <param name="actionCB">The action to run asynchronously.</param>
		/// <param name="completedCB">The callback to execute when action is completed.</param>
		/// <param name="timeToAdd">How much TethysGame.Time() until the task must complete. Must be > 0</param>
		public static void Run(ActionCallback actionCB, CompletedCallback completedCB, int timeToAdd=6)
		{
			ThreadAssert.MainThreadRequired();

			if (timeToAdd <= 0)
				throw new ArgumentOutOfRangeException("timeToAdd", timeToAdd, "timeToAdd must be greater than 0.");

			AsyncOperation operation = new AsyncOperation(TethysGame.Time() + timeToAdd, completedCB);

			lock (m_SyncCompleted)
				m_PendingOperations.Add(operation);
			
			// Run the operation. When it is complete, add it to the completed operations list
			operation.task = Task.Run(() =>
			{
				operation.returnState = actionCB();

				lock (m_SyncCompleted)
				{
					m_PendingOperations.Remove(operation);
					m_CompletedOperations.Add(operation);
				}
			});
		}

		/// <summary>
		/// Updates the async pump.
		/// NOTE: Must be run from main thread.
		/// </summary>
		public static void Update()
		{
			ThreadAssert.MainThreadRequired();

			int time = TethysGame.Time();

			List<AsyncOperation> pendingOperations;

			lock (m_SyncCompleted)
				pendingOperations = new List<AsyncOperation>(m_PendingOperations);

			// Wait for all pending operations that have reached the target time
			foreach (AsyncOperation operation in pendingOperations)
			{
				if (operation.targetTime == time)
					operation.task.Wait();
			}

			List<AsyncOperation> completedOperations = new List<AsyncOperation>();

			lock (m_SyncCompleted)
			{
				// Collect all completed operations that have reached the target time
				for (int i=0; i < m_CompletedOperations.Count; ++i)
				{
					AsyncOperation operation = m_CompletedOperations[i];

					if (operation.targetTime == time)
					{
						completedOperations.Add(operation);
						m_CompletedOperations.RemoveAt(i--);
					}
				}
			}

			// Execute the completed operation callbacks
			foreach (AsyncOperation operation in completedOperations)
				operation.completedCB?.Invoke(operation.returnState);
		}

		/// <summary>
		/// Releases all pending and completed operations.
		/// Should be called when the mission is terminated.
		/// </summary>
		public static void Release()
		{
			lock (m_SyncCompleted)
			{
				m_PendingOperations.Clear();
				m_CompletedOperations.Clear();
			}
		}
	}
}
