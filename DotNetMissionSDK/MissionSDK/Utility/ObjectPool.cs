using System.Collections.Generic;

namespace DotNetMissionSDK.Utility
{
	/// <summary>
	/// Class for maintaining a pool of objects.
	/// Can be used in multithreaded applications.
	/// </summary>
	public class ObjectPool<T> where T : new()
	{
		private Queue<T> m_Pool = new Queue<T>();

		private readonly object m_SyncObject = new object();

		public T Create()
		{
			lock (m_SyncObject)
			{
				if (m_Pool.Count > 0)
					return m_Pool.Dequeue();

				return new T();
			}
		}

		public void Release(T obj)
		{
			lock (m_SyncObject)
			{
				m_Pool.Enqueue(obj);
			}
		}
	}
}
