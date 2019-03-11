#include "stdafx_unmanaged.h"

#include <Outpost2DLL/Outpost2DLL.h>	// Main Outpost 2 header to interface with the game

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif


extern "C"
{
// Note: This file contains all the exported structures from Outpost2.exe.
// Note: Some of these structures are really more like full classes but
//		 since they called them struct's we'll let that one slide. =)



// Note: These first two structs have all member functions defined and
//		 exported by Outpost2.exe. (Essentially these structs are
//		 really classes in disguise.)

	extern EXPORT INT64 __stdcall LOCATION_Clip(int x, int y)
	{
		LOCATION loc = LOCATION(x, y);
		loc.Clip();

		INT64 result = loc.x | ((INT64)loc.y << 32);
		return result;
	}

	extern EXPORT int __stdcall MAP_RECT_Check(int minX, int minY, int maxX, int maxY, int x, int y)
	{
		MAP_RECT rect = MAP_RECT(minX, minY, maxX, maxY);
		LOCATION loc = LOCATION(x, y);

		return rect.Check(loc);
	}

	extern EXPORT INT64 __stdcall MAP_RECT_ClipToMapMin(int minX, int minY, int maxX, int maxY)
	{
		MAP_RECT rect = MAP_RECT(minX, minY, maxX, maxY);
		rect.ClipToMap();
		
		INT64 result = rect.x1 | ((INT64)rect.y1 << 32);
		return result;
	}

	extern EXPORT INT64 __stdcall MAP_RECT_ClipToMapMax(int minX, int minY, int maxX, int maxY)
	{
		MAP_RECT rect = MAP_RECT(minX, minY, maxX, maxY);
		rect.ClipToMap();

		INT64 result = rect.x2 | ((INT64)rect.y2 << 32);
		return result;
	}


// Note: These following structs have their names defined by the exported
//		 functions from Outpost2.exe but none of their fields are defined
//		 this way. (Only member functions are exported and these structs
//		 don't have any. Essentially these are the "true" structs.)



	/*struct OP2 PatrolRoute
	{
		int unknown1;
		LOCATION* waypoints;	// Max waypoints = 8, set Location.x = -1 for last waypoint in list if list is short
	};

	struct OP2 MrRec
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


	// Size: 0x20  [0x20 = 32, or 8 dwords]  [Note: last 2 fields are shorts]
	struct OP2 UnitRecord
	{
		map_id unitType;							// 0x0
		int x;										// 0x4
		int y;										// 0x8
		int unknown1;								// 0xC  ** [unused?]
		int rotation;								// 0x10 ** [Byte?]
		map_id weaponType;							// 0x14
		UnitClassifactions unitClassification;		// 0x18
		short cargoType;							// 0x1C
		short cargoAmount;							// 0x1E
	};*/
}
