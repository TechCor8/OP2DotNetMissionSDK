// Note: This file contains all the exported structures from Outpost2.exe.
// Note: Some of these structures are really more like full classes but
//		 since they called them struct's we'll let that one slide. =)

using DotNetMissionSDK.Json;
using System;
using System.Runtime.InteropServices;

// Note: These first two structs have all member functions defined and
//		 exported by Outpost2.exe. (Essentially these structs are
//		 really classes in disguise.)

namespace DotNetMissionSDK
{
	public class LOCATION
	{
		public int x;
		public int y;

		public LOCATION() { }

		public LOCATION(int tileX, int tileY)
		{
			this.x = tileX;
			this.y = tileY;
		}
		public LOCATION(DataLocation location)
		{
			x = location.x;
			y = location.y;
		}

		public void Add(LOCATION vector)
		{
			x += vector.x;
			y += vector.y;
		}

		public static LOCATION Difference(LOCATION a, LOCATION b)
		{
			return new LOCATION(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y));
		}

		// NOTE: Not 100% certain that "Clip" actually refers to map clipping.
		public void ClipToMap()
		{
			long result = LOCATION_Clip(x,y);
			x = (int)(result & uint.MaxValue);
			y = (int)(result >> 32);
		}

		[DllImport("DotNetInterop.dll")] private static extern long LOCATION_Clip(int x, int y);
		
		//int Norm(); WTF is Norm? The magnitude? The developer's name?
	};

	public class MAP_RECT
	{
		// Min = Top-Left, Max = Bottom-Right
		public int minX;
		public int minY;
		public int maxX;
		public int maxY;

		public int width		{ get { return maxX - minX; } }
		public int height		{ get { return maxY - minY; } }


		public MAP_RECT() { }
		public MAP_RECT(int minX, int minY, int maxX, int maxY)
		{
			this.minX = minX;
			this.minY = minY;
			this.maxX = maxX;
			this.maxY = maxY;
		}
		public MAP_RECT(LOCATION min, LOCATION max)
		{
			this.minX = min.x;
			this.minY = min.y;
			this.maxX = max.x;
			this.maxY = max.y;
		}
		public MAP_RECT(DataRect rect)
		{
			this.minX = rect.minX;
			this.minY = rect.minY;
			this.maxX = rect.maxX;
			this.maxY = rect.maxY;
		}

		public static MAP_RECT FromPointSize(int x, int y, int width, int height)
		{
			return new MAP_RECT(x - width/2, y - height/2, x + width/2, y + height/2);
		}

		public void Inflate(int unitsWide, int unitsHigh)
		{
			minX -= unitsWide;
			maxX += unitsWide;

			minY -= unitsHigh;
			maxY += unitsHigh;
		}

		public void Offset(int shiftRight, int shiftDown)
		{
			minX += shiftRight;
			maxX += shiftRight;

			minY += shiftDown;
			maxY += shiftDown;
		}

		public LOCATION GetRandomPointInRect()
		{
			return new LOCATION(minX + TethysGame.GetRand(width)+1, minY + TethysGame.GetRand(height)+1);
		}

		// int bInRect  [Checks if the point is in the rect  [handles x wrap around for rect coordinates]]
		public bool IsInRect(int x, int y)
		{
			// NOTE: Is this necessary? Can we check in managed code instead of native?
			return MAP_RECT_Check(minX, minY, maxX, maxY, x, y) != 0;
		}

		public void ClipToMap()
		{
			long min = MAP_RECT_ClipToMapMin(minX, minY, maxX, maxY);
			long max = MAP_RECT_ClipToMapMax(minX, minY, maxX, maxY);

			minX = (int)(min & uint.MaxValue);
			minY = (int)(min >> 32);

			maxX = (int)(max & uint.MaxValue);
			maxY = (int)(max >> 32);
		}

		[DllImport("DotNetInterop.dll")] private static extern int MAP_RECT_Check(int minX, int minY, int maxX, int maxY, int x, int y);		
		[DllImport("DotNetInterop.dll")] private static extern long MAP_RECT_ClipToMapMin(int minX, int minY, int maxX, int maxY);
		[DllImport("DotNetInterop.dll")] private static extern long MAP_RECT_ClipToMapMax(int minX, int minY, int maxX, int maxY);
	};


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
		public UnitClassifications unitClassification;		// 0x18
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
