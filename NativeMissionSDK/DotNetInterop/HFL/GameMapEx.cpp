// GameMapEx.h
// Extensions to the GameMap class
#include "stdafx_unmanaged.h"

#include <HFL/Source/HFL.h>

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif


extern "C"
{
	// GetTileEx()
	extern EXPORT unsigned int __stdcall GameMapEx_GetTileMappingIndex(int x, int y)
	{
		MapTile tile = GameMapEx::GetTileEx(LOCATION(x, y));
		return tile.tileIndex;
	}
	extern EXPORT unsigned int __stdcall GameMapEx_GetTileUnitIndex(int x, int y)
	{
		MapTile tile = GameMapEx::GetTileEx(LOCATION(x, y));
		return tile.unitIndex;
	}
	extern EXPORT unsigned int __stdcall GameMapEx_GetTileFlags(int x, int y)
	{
		MapTile tile = GameMapEx::GetTileEx(LOCATION(x, y));
		
		unsigned int flags = tile.lava;
		
		flags |= (tile.lavaPossible << 1);
		flags |= (tile.expand << 2);
		flags |= (tile.microbe << 3);
		flags |= (tile.wallOrBuilding << 4);

		return tile.lava;
	}

	// SetTileEx
	extern EXPORT void __stdcall GameMapEx_SetTile(int x, int y, unsigned int mappingIndex, unsigned int unitIndex, unsigned int flags)
	{
		MapTile tile;

		tile.lava = (flags & 1);
		tile.lavaPossible = ((flags >> 1) & 1);
		tile.expand = ((flags >> 2) & 1);
		tile.microbe = ((flags >> 3) & 1);
		tile.wallOrBuilding = ((flags >> 4) & 1);

		GameMapEx::SetTileEx(LOCATION(x, y), tile);
	}

	extern EXPORT int __stdcall GameMapEx_GetMapWidth()
	{
		return GameMapEx::GetMapWidth();
	}
	extern EXPORT int __stdcall GameMapEx_GetMapHeight()
	{
		return GameMapEx::GetMapHeight();
	}
	extern EXPORT int __stdcall GameMapEx_GetNumUnits()
	{
		return GameMapEx::GetNumUnits();
	}

	extern EXPORT int __stdcall GameMapEx_LoadMap(char* fileName)
	{
		return GameMapEx::LoadMap(fileName);
	}

	extern EXPORT void __stdcall GameMapEx_CopyTileMap(MapTile* tileMap)
	{
		GameMapEx::CopyTileMap(tileMap);
	}
}
