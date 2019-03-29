using System.Runtime.InteropServices;

namespace DotNetMissionSDK.HFL
{
	/// <summary>
	/// Must be initialized before using HFL.
	/// </summary>
	public class HFLCore
	{
		public static bool Init()										{ return HFL_Init() != HFLCANTINIT;		}
		public static void Cleanup()									{ HFL_Cleanup();						}
		


		// Defines for global / basic functions
		private const int HFLNOTINITED = -999;			// error: HFL not inited
		private const int HFLALREADYINITED = -998;		// error: HFL already inited
		private const int HFLLOADED = -997;				// HFL inited OK
		private const int HFLCANTINIT = -996;			// error: HFLInit() failed

		[DllImport("DotNetInterop.dll")] private static extern int HFL_Init();
		[DllImport("DotNetInterop.dll")] private static extern int HFL_Cleanup();
	}
}
