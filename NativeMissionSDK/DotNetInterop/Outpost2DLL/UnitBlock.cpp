#include "stdafx_unmanaged.h"

#include <Outpost2DLL/Outpost2DLL.h>	// Main Outpost 2 header to interface with the game

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif

// Note: The UnitBlock class can be used for creating blocks of units
//		 of certain predefined types.

extern "C"
{
	extern EXPORT UnitBlock* __stdcall UnitBlock_Create(UnitRecord* unitRecordTable)
	{
		return new UnitBlock(unitRecordTable);
	}
	extern EXPORT int __stdcall UnitBlock_CreateUnits(UnitBlock* handle, int playerNum, int bLightsOn)
	{
		return handle->CreateUnits(playerNum, bLightsOn);
	}
}