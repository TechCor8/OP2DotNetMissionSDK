// Note: ScStub is the parent class of Triggers, Groups, and Pinwheel classes.
//		 All functions in this class are available to derived classes
// Note: Do not try to create an instance of this class. It was meant
//		 simply as a base parent class from which other classes inherit
//		 functions from. Creating an instance of this class serves little
//		 (or no) purpose and may even crash the game.

using System;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK
{
	public class ScStub : IDisposable
	{
		private IntPtr m_Handle;

		public IntPtr GetHandle() { return m_Handle; }

		public ScStub(IntPtr handle)
		{
			m_Handle = handle;
		}

		// Methods
		//void Destroy();
		//void Disable();
		//void Enable();
		// [Get]
		public int GetStubIndex()
		{
			return ScStub_GetIndex(m_Handle);
		}
		//int IsEnabled();
		//int IsInitialized();
		// [Set]
		//void SetId(int stubIndex);

		[DllImport("NativeInterop.dll")] private static extern int ScStub_GetIndex(IntPtr handle);

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
			ScStub_Release(m_Handle);
		}

		~ScStub()
		{
			Dispose(false);
		}

		[DllImport("NativeInterop.dll")] private static extern void ScStub_Release(IntPtr handle);
	}
}
