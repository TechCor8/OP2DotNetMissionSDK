using System;

namespace DotNetMissionSDK
{
	/// <summary>
	/// Base class for cleaning up classes with unmanaged resources.
	/// </summary>
	public abstract class SDKDisposable : IDisposable
	{
		// --- Release ---
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{
				// Release managed objects
			}

			// Release unmanaged resources
		}

		~SDKDisposable()
		{
			Dispose(false);
		}
	}
}
