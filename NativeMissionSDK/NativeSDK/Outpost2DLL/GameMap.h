#pragma once
#ifndef OP2
#define OP2 __declspec(dllimport)
#endif


// External type names
struct LOCATION;

// Note: This class is used to control the displayed tiles on the map as well
//		 as the movement/passability characteristics of the terrain.
// Note: All members are static. Prefix class name and :: to access these functions.
//		 Example: GameMap::SetInitialLightLevel(0);

class OP2 GameMap
{
public:
	GameMap& operator = (const GameMap& gameMap);
	// [Get]
	static int __fastcall GetCellType(LOCATION location);
	static int __fastcall GetTile(LOCATION location);
	// [Set]
	static void __fastcall InitialSetTile(LOCATION location, int tileIndex);
	static void __fastcall SetTile(LOCATION location, int tileIndex);
	static void __fastcall SetCellType(LOCATION location, int cellIndex);
	static void __fastcall SetLavaPossible(LOCATION location, int bLavaPossible);
	static void __fastcall SetVirusUL(LOCATION location, int bBlightPresent);
	static void __fastcall SetInitialLightLevel(int lightPosition);
};
