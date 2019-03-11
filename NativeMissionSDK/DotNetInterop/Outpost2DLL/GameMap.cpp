#include "stdafx_unmanaged.h"

#include <Outpost2DLL/Outpost2DLL.h>	// Main Outpost 2 header to interface with the game

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif


extern "C"
{
	// Note: This class is used to control the displayed tiles on the map as well
	//		 as the movement/passability characteristics of the terrain.
	// Note: All members are static. Prefix class name and :: to access these functions.
	//		 Example: GameMap::SetInitialLightLevel(0);

	// [Get]
	extern EXPORT int __stdcall GameMap_GetCellType(int x, int y)
	{
		return GameMap::GetCellType(LOCATION(x,y));
	}
	extern EXPORT int __stdcall GameMap_GetTile(int x, int y)
	{
		return GameMap::GetTile(LOCATION(x, y));
	}
	// [Set]
	extern EXPORT void __stdcall GameMap_InitialSetTile(int x, int y, int tileIndex)
	{
		GameMap::InitialSetTile(LOCATION(x, y), tileIndex);
	}
	extern EXPORT void __stdcall GameMap_SetTile(int x, int y, int tileIndex)
	{
		GameMap::SetTile(LOCATION(x, y), tileIndex);
	}
	extern EXPORT void __stdcall GameMap_SetCellType(int x, int y, int cellIndex)
	{
		GameMap::SetCellType(LOCATION(x, y), cellIndex);
	}
	extern EXPORT void __stdcall GameMap_SetLavaPossible(int x, int y, int bLavaPossible)
	{
		GameMap::SetLavaPossible(LOCATION(x, y), bLavaPossible);
	}
	extern EXPORT void __stdcall GameMap_SetVirusUL(int x, int y, int bBlightPresent)
	{
		GameMap::SetVirusUL(LOCATION(x, y), bBlightPresent);
	}
	extern EXPORT void __stdcall GameMap_SetInitialLightLevel(int lightPosition)
	{
		GameMap::SetInitialLightLevel(lightPosition);
	}
}