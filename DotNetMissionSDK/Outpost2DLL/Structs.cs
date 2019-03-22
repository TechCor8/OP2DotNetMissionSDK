// Note: This file contains all the exported structures from Outpost2.exe.
// Note: Some of these structures are really more like full classes but
//		 since they called them struct's we'll let that one slide. =)

using System;
using System.Runtime.InteropServices;


namespace DotNetMissionSDK
{
	// Note: These following structs have their names defined by the exported
	//		 functions from Outpost2.exe but none of their fields are defined
	//		 this way. (Only member functions are exported and these structs
	//		 don't have any. Essentially these are the "true" structs.)

	public class PatrolRoute : SDKDisposable
	{
		private IntPtr m_Handle;

		public IntPtr GetHandle()			{ return m_Handle;						}


		public PatrolRoute(int waypointSize)
		{
			m_Handle = PatrolRoute_Create(waypointSize);
		}

		public void SetWaypoint(int index, LOCATION pt)
		{
			PatrolRoute_SetWaypoint(m_Handle, index, pt.x, pt.y);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			PatrolRoute_Release(m_Handle);
		}

		// Max waypoints = 8, set Location.x = -1 for last waypoint in list if list is short
		[DllImport("DotNetInterop.dll")] private static extern IntPtr PatrolRoute_Create(int waypointSize);
		[DllImport("DotNetInterop.dll")] private static extern void PatrolRoute_SetWaypoint(IntPtr handle, int index, int x, int y);
		[DllImport("DotNetInterop.dll")] private static extern void PatrolRoute_Release(IntPtr handle);
	}

	/*struct OP2 MrRec
	{
		map_id unitType;
		map_id weaponType;
		int unknown1; // -1 to terminate list
		int unknown2; // -1 to terminate list
	};

	struct OP2 PWDef
	{
		int x1; // -1 to terminate list
		int y1;
		int x2;
		int y2;
		int x3;
		int y3;
		int x4;
		int y4;
		int time1;
		int time2;
		int time3;
	};

	// CommandPackets seem to have a set maximum of 0x74 bytes
	// Note: The compiler must be told to pack this structure since the
	//		 short dataLength would otherwise have 2 padding bytes after
	//		 it which would mess up the rest of the structure.

	#pragma pack(push, 1)
	struct OP2 CommandPacket
	{
		int type;				// 0x00 Type of command - see secret list :)
		short dataLength;		// 0x04 Length of dataBuff
		int timeStamp;			// 0x06 Game Tick (only seems to be used for network traffic)
		int unknown;			// 0x0A **TODO** figure this out (only used for network traffic?)
		char dataBuff[0x66];	// 0x0E Dependent on message type
	};
	#pragma pack(pop)
	*/

	// Size: 0x20  [0x20 = 32, or 8 dwords]  [Note: last 2 fields are shorts]
	public class UnitRecord
	{
		public map_id unitType;								// 0x0
		public int x;										// 0x4
		public int y;										// 0x8
		public int unknown1;								// 0xC  ** [unused?]
		public int rotation;								// 0x10 ** [Byte?]
		public map_id weaponType;							// 0x14
		public UnitClassification unitClassification;		// 0x18
		public short cargoType;								// 0x1C
		public short cargoAmount;							// 0x1E
	};

	// TODO: Delete me if not needed
	/*public class UnitRecord : SDKDisposable
	{
		private IntPtr m_Handle;

		public IntPtr GetHandle()			{ return m_Handle;						}

		public UnitRecord(map_id unitType, int x, int y, int unknown1, int rotation, map_id weaponType,
						UnitClassifications unitClassification, short cargoType, short cargoAmount)
		{
			m_Handle = UnitRecord_Create(unitType, x, y, unknown1, rotation, weaponType, unitClassification, cargoType, cargoAmount);
		}


		[DllImport("DotNetInterop.dll")] private static extern IntPtr UnitRecord_Create(map_id unitType, int x, int y, int unknown1, int rotation, map_id weaponType,
																						UnitClassifications unitClassification, short cargoType, short cargoAmount);
		[DllImport("DotNetInterop.dll")] private static extern IntPtr UnitRecord_Release(IntPtr handle);

		// Dispose managed resources if "disposing" == true. Always dispose unmanaged resources.
		protected override void Dispose(bool disposing)
		{
			// Release unmanaged resources
			UnitRecord_Release(m_Handle);
		}


		// Create and manage UnitRecord array
		public static IntPtr CreateArray(int size)			{ return UnitRecord_CreateArray(size);		}
		public static void ReleaseArray(IntPtr handle)		{ UnitRecord_ReleaseArray(handle);			}
		public static void SetArray(IntPtr arr, int index, map_id unitType, int x, int y, int unknown1, int rotation, map_id weaponType,
												UnitClassifications unitClassification, short cargoType, short cargoAmount)
		{
			UnitRecord_SetArray(arr, index, unitType, x, y, unknown1, rotation, weaponType, unitClassification, cargoType, cargoAmount);
		}

		[DllImport("DotNetInterop.dll")] private static extern IntPtr UnitRecord_CreateArray(int size);
		[DllImport("DotNetInterop.dll")] private static extern void UnitRecord_SetArray(IntPtr arr, int index, map_id unitType, int x, int y, int unknown1, int rotation, map_id weaponType,
																						UnitClassifications unitClassification, short cargoType, short cargoAmount);
		[DllImport("DotNetInterop.dll")] private static extern void UnitRecord_ReleaseArray(IntPtr handle);
	};*/
}
