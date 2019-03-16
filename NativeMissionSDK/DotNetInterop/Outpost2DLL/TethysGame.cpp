#include "stdafx_unmanaged.h"

#include <Outpost2DLL/Outpost2DLL.h>	// Main Outpost 2 header to interface with the game

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif


extern "C"
{
	// Player Number and Number of Players
	extern EXPORT int __stdcall LocalPlayer()
	{
		return TethysGame::LocalPlayer();
	}

	extern EXPORT int __stdcall NoPlayers()
	{
		return TethysGame::NoPlayers();
	}

	// Returns address of player in player array provided by Outpost2
	extern EXPORT _Player* __stdcall TethysGame_GetPlayer(int playerNum)
	{
		return &Player[playerNum];
	}

	// Multiplayer game options  [Get Property]
	extern EXPORT int __stdcall TethysGame_UsesDayNight()
	{
		return TethysGame::UsesDayNight();
	}

	extern EXPORT int __stdcall TethysGame_UsesMorale()
	{
		return TethysGame::UsesMorale();
	}

	extern EXPORT int __stdcall TethysGame_CanHaveDisasters()
	{
		return TethysGame::CanHaveDisasters();
	}

	extern EXPORT int __stdcall TethysGame_InitialUnits()
	{
		return TethysGame::InitialUnits();
	}

	extern EXPORT int __stdcall TethysGame_CanAllowCheats()
	{
		return TethysGame::CanAllowCheats();
	}

	// Game Time
	extern EXPORT int __stdcall Tick()
	{
		return TethysGame::Tick();
	}

	extern EXPORT int __stdcall Time()
	{
		return TethysGame::Time();
	}

	// Game Sounds and Voice warnings
	//  Note: SoundIndex = 94..227 [Inclusive] are recorded voice messages
	extern EXPORT void __stdcall AddGameSound(int soundIndex, int toPlayerNum)
	{
		TethysGame::AddGameSound(soundIndex, toPlayerNum);
	}

	extern EXPORT void __stdcall AddMapSound(int soundIndex, int tileX, int tileY)
	{
		TethysGame::AddMapSound(soundIndex, LOCATION(tileX,tileY));
	}

	// Message log
	extern EXPORT void __stdcall AddMessage(int pixelX, int pixelY, char *message, int toPlayerNum, int soundIndex)
	{
		TethysGame::AddMessage(pixelX, pixelY, message, toPlayerNum, soundIndex);
	}

	//extern EXPORT void __stdcall AddMessage(Unit owner, char *message, int toPlayerNum, int soundIndex);

	// Debug/Cheat flags
	extern EXPORT void __stdcall TethysGame_SetDaylightEverywhere(int bOn)
	{
		TethysGame::SetDaylightEverywhere(bOn);
	}
	extern EXPORT void __stdcall TethysGame_SetDaylightMoves(int bOn)
	{
		TethysGame::SetDaylightMoves(bOn);
	}
	// Cheat flags  [gutted and useless by official Sierra update]
	extern EXPORT void __stdcall SetCheatFastProduction(int bOn)
	{
		TethysGame::SetCheatFastProduction(bOn);
	}
	extern EXPORT void __stdcall SetCheatFastUnits(int bOn)
	{
		TethysGame::SetCheatFastUnits(bOn);
	}
	extern EXPORT void __stdcall SetCheatProduceAll(int bOn)
	{
		TethysGame::SetCheatProduceAll(bOn);
	}
	extern EXPORT void __stdcall SetCheatUnlimitedResources(int bOn)
	{
		TethysGame::SetCheatUnlimitedResources(bOn);
	}

	// Unit Creation  [Returns: int numUnitsCreated]
	extern EXPORT int __stdcall TethysGame_CreateUnit(map_id unitType, int tileX, int tileY, int playerNum, map_id weaponCargoType, int rotation)
	{
		Unit returnedUnit;
		returnedUnit.unitID = -1;
		TethysGame::CreateUnit(returnedUnit, unitType, LOCATION(tileX, tileY), playerNum, weaponCargoType, rotation);
		
		return returnedUnit.unitID;
	}
	extern EXPORT int __stdcall CreateBeacon(map_id beaconType, int tileX, int tileY, int commonRareType, int barYield, int barVariant)
	{
		return TethysGame::CreateBeacon(beaconType, tileX, tileY, commonRareType, barYield, barVariant);
	}
	extern EXPORT int __stdcall TethysGame_CreateWreck(int tileX, int tileY, map_id techID, int bInitiallyVisible)
	{
		return TethysGame::CreateWreck(tileX, tileY, techID, bInitiallyVisible);
	}
	extern EXPORT int __stdcall TethysGame_PlaceMarker(int tileX, int tileY, int markerType)
	{
		Unit returnedUnit;
		returnedUnit.unitID = -1;
		TethysGame::PlaceMarker(returnedUnit, tileX, tileY, markerType);
		
		return returnedUnit.unitID;
	}
	extern EXPORT int __stdcall CreateWallOrTube(int tileX, int tileY, int unused, map_id wallTubeType)
	{
		return TethysGame::CreateWallOrTube(tileX, tileY, unused, wallTubeType);
	}
	//extern EXPORT int __stdcall CreateUnitBlock(_Player& ownerPlayer, const char* exportName, int bLightsOn);	// Returns: numUnitsCreated,  Note: see class UnitBlock

	// Morale Level
	//  Note: playerNum: -1 = PlayerAll
	//  Note: Calling ForceMoraleX functions after tick 0 will cause a "Cheated Game!" message to appear. FreeMoraleLevel can be called any time.
	//  Bug: ForceMoraleX is buggy if playerNum is not -1. You may need to call the function twice for the correct effect (see Forum post). FreeMoraleLevel is not affected by this bug.
	extern EXPORT void __stdcall ForceMoraleGreat(int playerNum)
	{
		TethysGame::ForceMoraleGreat(playerNum);
	}
	extern EXPORT void __stdcall ForceMoraleGood(int playerNum)
	{
		TethysGame::ForceMoraleGood(playerNum);
	}
	extern EXPORT void __stdcall ForceMoraleOK(int playerNum)
	{
		TethysGame::ForceMoraleOK(playerNum);
	}
	extern EXPORT void __stdcall ForceMoralePoor(int playerNum)
	{
		TethysGame::ForceMoralePoor(playerNum);
	}
	extern EXPORT void __stdcall ForceMoraleRotten(int playerNum)
	{
		TethysGame::ForceMoraleRotten(playerNum);
	}

	extern EXPORT void __stdcall FreeMoraleLevel(int playerNum)
	{
		TethysGame::FreeMoraleLevel(playerNum);
	}

	// Random number generation
	extern EXPORT void __stdcall SetSeed(unsigned int randNumSeed)
	{
		TethysGame::SetSeed(randNumSeed);
	}
	extern EXPORT int __stdcall GetRand(int range)
	{
		return TethysGame::GetRand(range);
	}
	
	// Disaster Creation
	extern EXPORT void __stdcall SetMeteor(int tileX, int tileY, int size)
	{
		TethysGame::SetMeteor(tileX, tileY, size);
	}
	extern EXPORT void __stdcall SetEarthquake(int tileX, int tileY, int magnitude)
	{
		TethysGame::SetEarthquake(tileX, tileY, magnitude);
	}
	extern EXPORT void __stdcall SetEruption(int tileX, int tileY, int spreadSpeed)
	{
		TethysGame::SetEruption(tileX, tileY, spreadSpeed);
	}
	extern EXPORT void __stdcall SetLightning(int startTileX, int startTileY, int duration, int endTileX, int endTileY)
	{
		TethysGame::SetLightning(startTileX, startTileY, duration, endTileX, endTileY);
	}
	extern EXPORT void __stdcall SetTornado(int startTileX, int startTileY, int duration, int endTileX, int endTileY, int bImmediate)
	{
		TethysGame::SetTornado(startTileX, startTileY, duration, endTileX, endTileY, bImmediate);
	}
	// Disaster spread speed
	extern EXPORT void __stdcall SetLavaSpeed(int spreadSpeed)
	{
		TethysGame::SetLavaSpeed(spreadSpeed);
	}
	extern EXPORT void __stdcall SetMicrobeSpreadSpeed(int spreadSpeed)
	{
		TethysGame::SetMicrobeSpreadSpeed(spreadSpeed);
	}

	// EMP Missile
	//  Note: FindEMPMissileTarget searches aligned 8x8 blocks, for the block with the greatest weight
	//  Note: The target location is at the block center (+3, +3)
	//  Note: Targets first found block of heighest (non-negative) weight, or the first block if all blocks have negative weight
	//  Note: Target player military units weigh 64, non-target player military units weigh -32, and non-target player non-military units weigh 1.
	extern EXPORT INT64 __stdcall FindEMPMissleTarget(int startTileX, int startTileY, int endTileX, int endTileY, int destPlayerNum)
	{
		LOCATION loc = TethysGame::FindEMPMissleTarget(startTileX, startTileY, endTileX, endTileY, destPlayerNum);
		INT64 result = loc.x | ((INT64)loc.y << 32);
		return result;
	}
	// Launches an EMP missile. May be launched from off screen (no spaceport required).
	// Will not launch an EMP missile if the selected sourcePlayer is not Plymouth.
	extern EXPORT void __stdcall SetEMPMissile(int launchTileX, int launchTileY, int sourcePlayerNum, int destTileX, int destTileY)
	{
		TethysGame::SetEMPMissile(launchTileX, launchTileY, sourcePlayerNum, destTileX, destTileY);
	}

	// Save/Load Games
	extern EXPORT void __stdcall SaveGame(const char* savedGameName)
	{
		TethysGame::SaveGame(savedGameName);
	}
	extern EXPORT void __stdcall LoadGame(const char* savedGameName)
	{
		TethysGame::LoadGame(savedGameName);
	}

	// Misc
	extern EXPORT void __stdcall SetMusicPlayList(int numSongs, int repeatStartIndex, SongIds* songIdList)
	{
		TethysGame::SetMusicPlayList(numSongs, repeatStartIndex, songIdList);
	}

	//extern EXPORT void __stdcall sIssueOptPacket(int variableId, int newValue);
};



