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
		//void Destroy();
		//void Disable();
		//void Enable();
		// [Get]
		
		//int IsEnabled();
		//int IsInitialized();
		// [Set]
		//void SetId(int stubIndex);

		//[DllImport("DotNetInterop.dll")] private static extern int ScStub_GetIndex(IntPtr handle);
	}
}
