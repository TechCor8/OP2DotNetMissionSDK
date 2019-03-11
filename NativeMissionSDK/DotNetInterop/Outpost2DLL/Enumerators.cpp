#include "stdafx_unmanaged.h"

#include <Outpost2DLL/Outpost2DLL.h>	// Main Outpost 2 header to interface with the game

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif


extern "C"
{
	// Note: This file is used to define the enumerator classes exported
	//		 from Outpost2.exe. These classes can be used to search for
	//		 or traverse a list of units one unit at a time.

	// ------------------------------------------------------------------------
	// Note: All Enumerators implement a GetNext function, which returns
	//	0 if no Unit was found, or non-zero if a Unit was found.
	//	If a Unit was found, then it's index is returned in the Unit proxy/stub
	//	passed as the first parameter.
	// ------------------------------------------------------------------------


	// Group (enumerate all units in a group)
	extern EXPORT GroupEnumerator* __stdcall GroupEnumerator_Create(int stubIndex)
	{
		ScGroup group;
		group.stubIndex = stubIndex;
		return new GroupEnumerator(group);
	}
	extern EXPORT void __stdcall GroupEnumerator_Release(GroupEnumerator* handle)
	{
		delete handle;
	}
	extern EXPORT int __stdcall GroupEnumerator_GetNext(GroupEnumerator* handle, Unit* returnedUnit)
	{
		return handle->GetNext(*returnedUnit);
	}

	// Vehicles (enumerate all vehicles for a certain player)
	extern EXPORT PlayerVehicleEnum* __stdcall PlayerVehicleEnum_Create(int playerID)
	{
		return new PlayerVehicleEnum(playerID);
	}
	extern EXPORT void __stdcall PlayerVehicleEnum_Release(PlayerVehicleEnum* handle)
	{
		delete handle;
	}
	extern EXPORT int __stdcall PlayerVehicleEnum_GetNext(PlayerVehicleEnum* handle, Unit* returnedUnit)
	{
		return handle->GetNext(*returnedUnit);
	}

	// Buildings (enumerate all buildings of a certain type for a certain player)
	extern EXPORT PlayerBuildingEnum* __stdcall PlayerBuildingEnum_Create(int playerID, map_id buildingType)
	{
		return new PlayerBuildingEnum(playerID, buildingType);
	}
	extern EXPORT void __stdcall PlayerBuildingEnum_Release(PlayerBuildingEnum* handle)
	{
		delete handle;
	}
	extern EXPORT int __stdcall PlayerBuildingEnum_GetNext(PlayerBuildingEnum* handle, Unit* returnedUnit)
	{
		return handle->GetNext(*returnedUnit);
	}

	// Units (enumerate all units of a certain player)
	extern EXPORT PlayerUnitEnum* __stdcall PlayerUnitEnum_Create(int playerID)
	{
		return new PlayerUnitEnum(playerID);
	}
	extern EXPORT void __stdcall PlayerUnitEnum_Release(PlayerUnitEnum* handle)
	{
		delete handle;
	}
	extern EXPORT int __stdcall PlayerUnitEnum_GetNext(PlayerUnitEnum* handle, Unit* returnedUnit)
	{
		return handle->GetNext(*returnedUnit);
	}

	// InRange (enumerate all units within a given distance of a given location)
	extern EXPORT InRangeEnumerator* __stdcall InRangeEnumerator_Create(int x, int y, int maxTileDistance)
	{
		return new InRangeEnumerator(LOCATION(x,y), maxTileDistance);
	}
	extern EXPORT void __stdcall InRangeEnumerator_Release(InRangeEnumerator* handle)
	{
		delete handle;
	}
	extern EXPORT int __stdcall InRangeEnumerator_GetNext(InRangeEnumerator* handle, Unit* returnedUnit)
	{
		return handle->GetNext(*returnedUnit);
	}

	// InRect (enumerate all units within a given rectangle)
	extern EXPORT InRectEnumerator* __stdcall InRectEnumerator_Create(int minX, int minY, int maxX, int maxY)
	{
		return new InRectEnumerator(MAP_RECT(minX, minY, maxX, maxY));
	}
	extern EXPORT void __stdcall InRectEnumerator_Release(InRectEnumerator* handle)
	{
		delete handle;
	}
	extern EXPORT int __stdcall InRectEnumerator_GetNext(InRectEnumerator* handle, Unit* returnedUnit)
	{
		return handle->GetNext(*returnedUnit);
	}

	// Location (enumerate all units at a given location)
	extern EXPORT LocationEnumerator* __stdcall LocationEnumerator_Create(int x, int y)
	{
		return new LocationEnumerator(LOCATION(x, y));
	}
	extern EXPORT void __stdcall LocationEnumerator_Release(LocationEnumerator* handle)
	{
		delete handle;
	}
	extern EXPORT int __stdcall LocationEnumerator_GetNext(LocationEnumerator* handle, Unit* returnedUnit)
	{
		return handle->GetNext(*returnedUnit);
	}

	// Closest (enumerate all units ordered by their distance to a given location)
	extern EXPORT ClosestEnumerator* __stdcall ClosestEnumerator_Create(int x, int y)
	{
		return new ClosestEnumerator(LOCATION(x, y));
	}
	extern EXPORT void __stdcall ClosestEnumerator_Release(ClosestEnumerator* handle)
	{
		delete handle;
	}
	extern EXPORT int __stdcall ClosestEnumerator_GetNext(ClosestEnumerator* handle, Unit* returnedUnit)
	{
		unsigned long pixelDistance;
		return handle->GetNext(*returnedUnit, pixelDistance);
	}
}
