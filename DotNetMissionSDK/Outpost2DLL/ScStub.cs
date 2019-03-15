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
	public class ScStub
	{
		public int stubIndex { get; private set; }


		public ScStub(int stubIndex)
		{
			this.stubIndex = stubIndex;
		}

		// Methods
		public void Destroy()			{ ScStub_Destroy(stubIndex);					}
		public void Disable()			{ ScStub_Disable(stubIndex);					}
		public void Enable()			{ ScStub_Enable(stubIndex);						}
		
		// [Get]
		public bool IsEnabled()			{ return ScStub_IsEnabled(stubIndex) != 0;		}
		public bool IsInitialized()		{ return ScStub_IsInitialized(stubIndex) != 0;	}
		// [Set]
		//void SetId(int stubIndex);

		// Methods
		[DllImport("DotNetInterop.dll")] private static extern void ScStub_Destroy(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void ScStub_Disable(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void ScStub_Enable(int stubIndex);
		
		// [Get]
		[DllImport("DotNetInterop.dll")] private static extern int ScStub_IsEnabled(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern int ScStub_IsInitialized(int stubIndex);
		// [Set]
		//void SetId(int stubIndex);
	}
}
