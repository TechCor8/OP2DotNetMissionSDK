using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetMissionSDK.Utility
{
	/// <summary>
	/// Runs tasks asynchronously before synchronizing them to a target tick.
	/// Used to to keep asynchronous operations predictable on the game timeline.
	/// </summary>
	public class AsyncPump
	{
		private class AsyncOperation
		{
			public int targetTick					{ get; private set; }
			public CompletedCallback completedCB	{ get; private set; }
			public Task task;
			public object returnState;

			public AsyncOperation(int targetTick, CompletedCallback completedCB)
			{
				this.targetTick = targetTick;
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
		/// </summary>
		/// <param name="actionCB">The action to run asynchronously.</param>
		/// <param name="completedCB">The callback to execute when action is completed.</param>
		public static void Run(ActionCallback actionCB, CompletedCallback completedCB)
		{
			AsyncOperation operation = new AsyncOperation(TethysGame.Time() + 10, completedCB);

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
		/// </summary>
		public static void Update()
		{
			int tick = TethysGame.Time();

			// Wait for all pending operations that have reached the target tick
			foreach (AsyncOperation operation in m_PendingOperations)
			{
				if (operation.targetTick == tick)
					operation.task.Wait();
			}

			List<AsyncOperation> completedOperations = new List<AsyncOperation>();

			lock (m_SyncCompleted)
			{
				// Collect all completed operations that have reached the target tick
				for (int i=0; i < m_CompletedOperations.Count; ++i)
				{
					AsyncOperation operation = m_CompletedOperations[i];

					if (operation.targetTick == tick)
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
	}
}
