using System;

namespace DotNetMissionSDK.Async
{
	/// <summary>
	/// Provides a thread-safe randomizer that does not cause a desync in multiplayer.
	/// </summary>
	public class AsyncRandom
	{
		private static Random m_Random;

		private static readonly object m_SyncObject = new object();


		/// <summary>
		/// Initializes the randomizer.
		/// Should be called with a value from TethysGame.GetRandom() to maintain synchronization in multiplayer.
		/// </summary>
		/// <param name="seed">A number used to calculate the starting value for the randomizer.</param>
		public static void Initialize(int seed)
		{
			ThreadAssert.MainThreadRequired();

			m_Random = new Random(seed);
		}

		/// <summary>
		/// Gets a random number from min inclusive to max exclusive.
		/// </summary>
		/// <param name="min">The minimum value (inclusive)</param>
		/// <param name="max">The maximum value (exclusive)</param>
		public static int GetRange(int min, int max)
		{
			lock (m_SyncObject)
				return m_Random.Next(min, max);
		}
	}
}
