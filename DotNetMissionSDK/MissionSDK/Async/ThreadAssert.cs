using System.Diagnostics;
using System.Threading;

namespace DotNetMissionSDK.Async
{
	/// <summary>
	/// Class used for protecting the developer against incorrect use of multithreaded code.
	/// </summary>
	public class ThreadAssert
	{
		private static int m_MainThreadID;

		/// <summary>
		/// Initializes the ThreadAssert class.
		/// Must be called from the main thread at start.
		/// </summary>
		public static void Initialize()
		{
			m_MainThreadID = Thread.CurrentThread.ManagedThreadId;
		}

		/// <summary>
		/// When called, will throw an assert if not on the main thread.
		/// </summary>
		[Conditional("DEBUG")]
		public static void MainThreadRequired()
		{
			Debug.Assert(m_MainThreadID == Thread.CurrentThread.ManagedThreadId, "Main Thread Required", "This method was not called from the main thread.");
		}
	}
}
