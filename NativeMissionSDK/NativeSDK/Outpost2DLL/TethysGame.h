#pragma once
#ifndef OP2
#define OP2 __declspec(dllimport)
#endif

// Note: This file contains the defintion of the TethysGame class
//		which controls the overall game environment

// External type names
enum map_id;
enum SongIds;
struct LOCATION;
class Unit;
class _Player;

// Note: The TethysGame class controls creation of units and disasters,
//		 sending custom in game messages to the player, playing recorded
//		 sounds, morale controls, and day and night controls.
// Note: All members are static. Prefix class name and :: to access these functions.
//		 Example: numberOfPlayers = TethysGame::NoPlayers();

class OP2 TethysGame
{
public:
	TethysGame& operator = (const TethysGame& tethysGame);

	// Player Number and Number of Players
	static int __fastcall LocalPlayer();			// Returns the local player index
	static int __fastcall NoPlayers();				// Returns number of players (including both Human and AI)

	// Multiplayer game options  [Get Property]
	static int __fastcall UsesDayNight();
	static int __fastcall UsesMorale();
	static int __fastcall CanHaveDisasters();
	static int __fastcall InitialUnits();		// Number of Laser/Microwave Lynx to start with
	static int __fastcall CanAllowCheats();		// Useless

	// Game Time
	static int __fastcall Tick();	// Current time (tick is the smallest slice of game time)
	static int __fastcall Time();	// Current tick / 4  (most processing is only done every 4 ticks)

	// Game Sounds and Voice warnings
	//  Note: SoundIndex = 94..227 [Inclusive] are recorded voice messages
	static void __fastcall AddGameSound(int soundIndex, int toPlayerNum);			// Note: toPlayerNum: -1 = PlayerAll
	static void __fastcall AddMapSound(int soundIndex, LOCATION location);
	// Message log
	static void __fastcall AddMessage(int pixelX, int pixelY, char *message, int toPlayerNum, int soundIndex);	// Note: toPlayerNum: -1 = PlayerAll
	static void __fastcall AddMessage(Unit owner, char *message, int toPlayerNum, int soundIndex);				// Note: toPlayerNum: -1 = PlayerAll

	// Debug/Cheat flags
	static void __fastcall SetDaylightEverywhere(int bOn);
	static void __fastcall SetDaylightMoves(int bOn);
	// Cheat flags  [gutted and useless by official Sierra update]
	static void __fastcall SetCheatFastProduction(int bOn);			// Useless
	static void __fastcall SetCheatFastUnits(int bOn);				// Useless
	static void __fastcall SetCheatProduceAll(int bOn);				// Useless
	static void __fastcall SetCheatUnlimitedResources(int bOn);		// Useless

	// Unit Creation  [Returns: int numUnitsCreated]
	static int __fastcall CreateUnit(Unit& returnedUnit, map_id unitType, LOCATION location, int playerNum, map_id weaponCargoType, int rotation);	// Note: see enum UnitDirection
	static int __fastcall CreateBeacon(map_id beaconType, int tileX, int tileY, int commonRareType, int barYield, int barVariant);	// Note: see enums BeaconTypes, Yield, Variant
	static int __fastcall CreateWreck(int tileX, int tileY, map_id techID, int bInitiallyVisible);		// Note: techID must be >= 8000 but < (8000+4096) = 12096
	static int __fastcall PlaceMarker(Unit& returnedUnit, int tileX, int tileY, int markerType);		// Note: See enum MarkerTypes
	static int __fastcall CreateWallOrTube(int tileX, int tileY, int unused, map_id wallTubeType);		// Returns: 1 [true] always
	static int __fastcall CreateUnitBlock(_Player& ownerPlayer, const char* exportName, int bLightsOn);	// Returns: numUnitsCreated,  Note: see class UnitBlock

	// Morale Level
	//  Note: playerNum: -1 = PlayerAll
	//  Note: Calling ForceMoraleX functions after tick 0 will cause a "Cheated Game!" message to appear. FreeMoraleLevel can be called any time.
	//  Bug: ForceMoraleX is buggy if playerNum is not -1. You may need to call the function twice for the correct effect (see Forum post). FreeMoraleLevel is not affected by this bug.
	static void __fastcall ForceMoraleGreat(int playerNum);
	static void __fastcall ForceMoraleGood(int playerNum);
	static void __fastcall ForceMoraleOK(int playerNum);
	static void __fastcall ForceMoralePoor(int playerNum);
	static void __fastcall ForceMoraleRotten(int playerNum);
	static void __fastcall FreeMoraleLevel(int playerNum);		// Let Morale vary according to colony state and events

	// Random number generation
	static void __fastcall SetSeed(unsigned int randNumSeed);	// Set random number seed
	static int __fastcall GetRand(int range);					// Returns a number from 0..(range-1)

	// Disaster Creation
	static void __fastcall SetMeteor(int tileX, int tileY, int size);
	static void __fastcall SetEarthquake(int tileX, int tileY, int magnitude);
	static void __fastcall SetEruption(int tileX, int tileY, int spreadSpeed);
	static void __fastcall SetLightning(int startTileX, int startTileY, int duration, int endTileX, int endTileY);
	static void __fastcall SetTornado(int startTileX, int startTileY, int duration, int endTileX, int endTileY, int bImmediate);
	// Disaster spread speed
	static void __fastcall SetLavaSpeed(int spreadSpeed);
	static void __fastcall SetMicrobeSpreadSpeed(int spreadSpeed);

	// EMP Missile
	//  Note: FindEMPMissileTarget searches aligned 8x8 blocks, for the block with the greatest weight
	//  Note: The target location is at the block center (+3, +3)
	//  Note: Targets first found block of heighest (non-negative) weight, or the first block if all blocks have negative weight
	//  Note: Target player military units weigh 64, non-target player military units weigh -32, and non-target player non-military units weigh 1.
	static LOCATION __fastcall FindEMPMissleTarget(int startTileX, int startTileY, int endTileX, int endTileY, int destPlayerNum);
	// Launches an EMP missile. May be launched from off screen (no spaceport required).
	// Will not launch an EMP missile if the selected sourcePlayer is not Plymouth.
	static void __fastcall SetEMPMissile(int launchTileX, int launchTileY, int sourcePlayerNum, int destTileX, int destTileY);

	// Save/Load Games
	static void __fastcall SaveGame(const char* savedGameName);		// Note: Unimplemented  [Useless]
	static void __fastcall LoadGame(const char* savedGameName);		// Note: Saved game names default to: "SGAME?.OP2" file name format

	// Misc
	static void __fastcall SetMusicPlayList(int numSongs, int repeatStartIndex, SongIds* songIdList);

private:
	static void __fastcall sIssueOptPacket(int variableId, int newValue);
};
