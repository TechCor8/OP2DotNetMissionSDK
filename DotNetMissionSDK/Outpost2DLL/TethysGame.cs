// Note: This file contains the defintion of the TethysGame class
//		which controls the overall game environment

using DotNetMissionSDK.Async;
using DotNetMissionSDK.HFL;
using System;
using System.Runtime.InteropServices;

// Note: The TethysGame class controls creation of units and disasters,
//		 sending custom in game messages to the player, playing recorded
//		 sounds, morale controls, and day and night controls.
// Note: All members are static. Prefix class name and :: to access these functions.
//		 Example: numberOfPlayers = TethysGame::NoPlayers();

namespace DotNetMissionSDK
{
	public class TethysGame
	{
		public static int LocalPlayer()						{ ThreadAssert.MainThreadRequired();	return TethysGame_LocalPlayer();									}	// Returns the local player index
		public static int PlayerCount()						{ ThreadAssert.MainThreadRequired();	return TethysGame_NoPlayers();										}	// Returns number of players (including both Human and AI)
		public static PlayerEx GetPlayer(int playerID)		{ ThreadAssert.MainThreadRequired();	return new PlayerEx(TethysGame_GetPlayer(playerID), playerID);		}

		// Message log
		// Note: toPlayerNum: -1 = PlayerAll
		public static void AddMessage(Unit owner, string message, int toPlayerID, int soundIndex)
		{
			TethysGame_AddMessage2(owner.GetStubIndex(), message, toPlayerID, soundIndex);
		}

		// Multiplayer game options  [Get Property]
		public static bool UsesDayNight()					{ ThreadAssert.MainThreadRequired();	return TethysGame_UsesDayNight() != 0;			}
		public static bool UsesMorale()						{ ThreadAssert.MainThreadRequired();	return TethysGame_UsesMorale() != 0;			}
		public static bool CanHaveDisasters()				{ ThreadAssert.MainThreadRequired();	return TethysGame_CanHaveDisasters() != 0;		}
		public static int InitialUnits()					{ ThreadAssert.MainThreadRequired();	return TethysGame_InitialUnits();				}	// Number of Laser/Microwave Lynx to start with
		public static bool CanAllowCheats()					{ ThreadAssert.MainThreadRequired();	return TethysGame_CanAllowCheats() != 0;		}	// Useless

		// Game Time
		public static int Tick()							{ ThreadAssert.MainThreadRequired();	return TethysGame_Tick();						}   // Current time (tick is the smallest slice of game time)
		public static int Time()							{ ThreadAssert.MainThreadRequired();	return TethysGame_Time();						}   // Current tick / 4  (most processing is only done every 4 ticks)

		// Game Sounds and Voice warnings
		//  Note: SoundIndex = 94..227 [Inclusive] are recorded voice messages
		public static void AddGameSound(int soundIndex, int toPlayerID)			{ ThreadAssert.MainThreadRequired();	TethysGame_AddGameSound(soundIndex, toPlayerID);	}	// Note: toPlayerNum: -1 = PlayerAll
		public static void AddMapSound(int soundIndex, int tileX, int tileY)	{ ThreadAssert.MainThreadRequired();	TethysGame_AddMapSound(soundIndex, tileX, tileY);	}
		// Message log
		public static void AddMessage(int pixelX, int pixelY, string message, int toPlayerID, int soundIndex)	{ ThreadAssert.MainThreadRequired();	TethysGame_AddMessage(pixelX, pixelY, message, toPlayerID, soundIndex);	}	// Note: toPlayerNum: -1 = PlayerAll
		public static void AddMessage(int ownerIndex, string message, int toPlayerID, int soundIndex)			{ ThreadAssert.MainThreadRequired();	TethysGame_AddMessage2(ownerIndex, message, toPlayerID, soundIndex);	}	// Note: toPlayerNum: -1 = PlayerAll

		// Debug/Cheat flags
		public static void SetDaylightEverywhere(bool isOn)	{ ThreadAssert.MainThreadRequired();	TethysGame_SetDaylightEverywhere(isOn ? 1 : 0);	}
		public static void SetDaylightMoves(bool isOn)		{ ThreadAssert.MainThreadRequired();	TethysGame_SetDaylightMoves(isOn ? 1 : 0);		}

		// Unit Creation  [Returns: int numUnitsCreated]
		public static UnitEx CreateUnit(map_id unitType, int tileX, int tileY, int playerID, int weaponCargoType, UnitDirection direction)
		{
			ThreadAssert.MainThreadRequired();

			int index = TethysGame_CreateUnit(unitType, tileX, tileY, playerID, weaponCargoType, direction);
			if (index < 0)
				return null;

			return new UnitEx(index);
		}

		public static int CreateBeacon(map_id beaconType, int tileX, int tileY, BeaconType commonRareType, Yield barYield, Variant barVariant)	{ ThreadAssert.MainThreadRequired();	return TethysGame_CreateBeacon(beaconType, tileX, tileY, commonRareType, barYield, barVariant);		}
		
		// Note: techID must be >= 8000 but < (8000+4096) = 12096
		public static int CreateWreck(int tileX, int tileY, map_id techID, bool isInitiallyVisible)	{ ThreadAssert.MainThreadRequired();	return TethysGame_CreateWreck(tileX, tileY, techID, isInitiallyVisible ? 1 : 0);	}

		public static Unit PlaceMarker(int tileX, int tileY, MarkerType markerType)
		{
			ThreadAssert.MainThreadRequired();

			int index = TethysGame_PlaceMarker(tileX, tileY, markerType);
			if (index < 0)
				return null;

			return new UnitEx(index);
		}

		public static void CreateWallOrTube(int tileX, int tileY, int unused, map_id wallTubeType)		{ ThreadAssert.MainThreadRequired();	TethysGame_CreateWallOrTube(tileX, tileY, unused, wallTubeType);		}

		// Morale Level
		//  Note: playerNum: -1 = PlayerAll
		//  Note: Calling ForceMoraleX functions after tick 0 will cause a "Cheated Game!" message to appear. FreeMoraleLevel can be called any time.
		//  Bug: ForceMoraleX is buggy if playerNum is not -1. You may need to call the function twice for the correct effect (see Forum post). FreeMoraleLevel is not affected by this bug.
		public static void ForceMoraleGreat(int playerID)	{ ThreadAssert.MainThreadRequired();	TethysGame_ForceMoraleGreat(playerID);		}
		public static void ForceMoraleGood(int playerID)	{ ThreadAssert.MainThreadRequired();	TethysGame_ForceMoraleGood(playerID);		}
		public static void ForceMoraleOK(int playerID)		{ ThreadAssert.MainThreadRequired();	TethysGame_ForceMoraleOK(playerID);			}
		public static void ForceMoralePoor(int playerID)	{ ThreadAssert.MainThreadRequired();	TethysGame_ForceMoralePoor(playerID);		}
		public static void ForceMoraleRotten(int playerID)	{ ThreadAssert.MainThreadRequired();	TethysGame_ForceMoraleRotten(playerID);		}
		public static void FreeMoraleLevel(int playerID)	{ ThreadAssert.MainThreadRequired();	TethysGame_FreeMoraleLevel(playerID);		}	// Let Morale vary according to colony state and events

		// Random number generation
		public static void SetSeed(uint randNumSeed)		{ ThreadAssert.MainThreadRequired();	TethysGame_SetSeed(randNumSeed);			}	// Set random number seed
		public static int GetRand(int range)				{ ThreadAssert.MainThreadRequired();	return TethysGame_GetRand(range);			}	// Returns a number from 0..(range-1)

		// Disaster Creation
		public static void SetMeteor(int tileX, int tileY, int size)															{ ThreadAssert.MainThreadRequired();	TethysGame_SetMeteor(tileX, tileY, size);													}
		public static void SetEarthquake(int tileX, int tileY, int magnitude)													{ ThreadAssert.MainThreadRequired();	TethysGame_SetEarthquake(tileX, tileY, magnitude);											}
		public static void SetEruption(int tileX, int tileY, int spreadSpeed)													{ ThreadAssert.MainThreadRequired();	TethysGame_SetEruption(tileX, tileY, spreadSpeed);											}
		public static void SetLightning(int startTileX, int startTileY, int duration, int endTileX, int endTileY)				{ ThreadAssert.MainThreadRequired();	TethysGame_SetLightning(startTileX, startTileY, duration, endTileX, endTileY);				}
		public static void SetTornado(int startTileX, int startTileY, int duration, int endTileX, int endTileY, int bImmediate)	{ ThreadAssert.MainThreadRequired();	TethysGame_SetTornado(startTileX, startTileY, duration, endTileX, endTileY, bImmediate);	}
		// Disaster spread speed
		public static void SetLavaSpeed(int spreadSpeed)																		{ ThreadAssert.MainThreadRequired();	TethysGame_SetLavaSpeed(spreadSpeed);														}
		public static void SetMicrobeSpreadSpeed(int spreadSpeed)																{ ThreadAssert.MainThreadRequired();	TethysGame_SetMicrobeSpreadSpeed(spreadSpeed);												}

		// EMP Missile
		//  Note: FindEMPMissileTarget searches aligned 8x8 blocks, for the block with the greatest weight
		//  Note: The target location is at the block center (+3, +3)
		//  Note: Targets first found block of heighest (non-negative) weight, or the first block if all blocks have negative weight
		//  Note: Target player military units weigh 64, non-target player military units weigh -32, and non-target player non-military units weigh 1.
		public static LOCATION FindEMPMissleTarget(int startTileX, int startTileY, int endTileX, int endTileY, int destPlayerID)
		{
			ThreadAssert.MainThreadRequired();

			long result = TethysGame_FindEMPMissleTarget(startTileX, startTileY, endTileX, endTileY, destPlayerID);
			LOCATION loc = new LOCATION();
			loc.x = (int)(result & uint.MaxValue);
			loc.y = (int)(result >> 32);

			return loc;
		}
		
		// Launches an EMP missile. May be launched from off screen (no spaceport required).
		// Will not launch an EMP missile if the selected sourcePlayer is not Plymouth.
		public static void SetEMPMissile(int launchTileX, int launchTileY, int sourcePlayerID, int destTileX, int destTileY)	{ ThreadAssert.MainThreadRequired();	TethysGame_SetEMPMissile(launchTileX, launchTileY, sourcePlayerID, destTileX, destTileY);	}

		// Save/Load Games
		public static void SaveGame(string savedGameName)	{ ThreadAssert.MainThreadRequired();	TethysGame_SaveGame(savedGameName);			}		// Note: Unimplemented  [Useless]
		public static void LoadGame(string savedGameName)	{ ThreadAssert.MainThreadRequired();	TethysGame_LoadGame(savedGameName);			}		// Note: Saved game names default to: "SGAME?.OP2" file name format

		// Misc
		public static void SetMusicPlayList(int numSongs, int repeatStartIndex, int[] songIdList)	{ ThreadAssert.MainThreadRequired();	TethysGame_SetMusicPlayList(numSongs, repeatStartIndex, songIdList);			}

		/// <summary>
		/// Gets a random value between min and max.
		/// </summary>
		/// <param name="min">The minimum value. Inclusive</param>
		/// <param name="max">The maximum value. Exclusive</param>
		/// <returns>The random value between min and max.</returns>
		public static int GetRandomRange(int min, int max)
		{
			ThreadAssert.MainThreadRequired();

			return GetRand(max-min) + min;
		}

		/// <summary>
		/// Converts coordinates from game coordinates (HUD status bar) to map coordinates (internal rect of arbitrary min/max)
		/// </summary>
		/// <param name="gameCoordinates">The game coordinates to convert.</param>
		/// <returns>The equivalent map coordinates.</returns>
		public static LOCATION GetMapCoordinates(LOCATION gameCoordinates)
		{
			LOCATION mapCoordinates = new LOCATION(gameCoordinates.x + GameMap.bounds.xMin - 1, gameCoordinates.y + GameMap.bounds.yMin - 1);
			mapCoordinates.ClipToMap();
			return mapCoordinates;
		}

		public static LOCATION GetGameCoordinates(LOCATION mapCoordinates)
		{
			mapCoordinates.ClipToMap();
			return new LOCATION(mapCoordinates.x - GameMap.bounds.xMin + 1, mapCoordinates.y - GameMap.bounds.yMin + 1);
		}


		// Player Number and Number of Players
		[DllImport("DotNetInterop.dll")] private static extern int TethysGame_LocalPlayer();            // Returns the local player index
		[DllImport("DotNetInterop.dll")] private static extern int TethysGame_NoPlayers();              // Returns number of players (including both Human and AI)
		[DllImport("DotNetInterop.dll")] private static extern IntPtr TethysGame_GetPlayer(int playerNum);

		// Multiplayer game options  [Get Property]
		[DllImport("DotNetInterop.dll")] private static extern int TethysGame_UsesDayNight();
		[DllImport("DotNetInterop.dll")] private static extern int TethysGame_UsesMorale();
		[DllImport("DotNetInterop.dll")] private static extern int TethysGame_CanHaveDisasters();
		[DllImport("DotNetInterop.dll")] private static extern int TethysGame_InitialUnits();       // Number of Laser/Microwave Lynx to start with
		[DllImport("DotNetInterop.dll")] private static extern int TethysGame_CanAllowCheats();     // Useless

		// Game Time
		[DllImport("DotNetInterop.dll")] private static extern int TethysGame_Tick();   // Current time (tick is the smallest slice of game time)
		[DllImport("DotNetInterop.dll")] private static extern int TethysGame_Time();   // Current tick / 4  (most processing is only done every 4 ticks)

		// Game Sounds and Voice warnings
		//  Note: SoundIndex = 94..227 [Inclusive] are recorded voice messages
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_AddGameSound(int soundIndex, int toPlayerNum);           // Note: toPlayerNum: -1 = PlayerAll
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_AddMapSound(int soundIndex, int tileX, int tileY);
		// Message log
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_AddMessage(int pixelX, int pixelY, string message, int toPlayerNum, int soundIndex);		// Note: toPlayerNum: -1 = PlayerAll
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_AddMessage2(int ownerIndex, string message, int toPlayerNum, int soundIndex);			// Note: toPlayerNum: -1 = PlayerAll

		// Debug/Cheat flags
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SetDaylightEverywhere(int bOn);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SetDaylightMoves(int bOn);
		// Cheat flags  [gutted and useless by official Sierra update]
		//[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SetCheatFastProduction(int bOn);         // Useless
		//[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SetCheatFastUnits(int bOn);              // Useless
		//[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SetCheatProduceAll(int bOn);             // Useless
		//[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SetCheatUnlimitedResources(int bOn);     // Useless

		// Unit Creation  [Returns: int numUnitsCreated]
		[DllImport("DotNetInterop.dll")] private static extern int TethysGame_CreateUnit(map_id unitType, int tileX, int tileY, int playerNum, int weaponCargoType, UnitDirection direction);  // Note: see enum UnitDirection
		[DllImport("DotNetInterop.dll")] private static extern int TethysGame_CreateBeacon(map_id beaconType, int tileX, int tileY, BeaconType commonRareType, Yield barYield, Variant barVariant);  // Note: see enums BeaconTypes, Yield, Variant
		[DllImport("DotNetInterop.dll")] private static extern int TethysGame_CreateWreck(int tileX, int tileY, map_id techID, int bInitiallyVisible);      // Note: techID must be >= 8000 but < (8000+4096) = 12096
		[DllImport("DotNetInterop.dll")] private static extern int TethysGame_PlaceMarker(int tileX, int tileY, MarkerType markerType);       // Note: See enum MarkerTypes
		[DllImport("DotNetInterop.dll")] private static extern int TethysGame_CreateWallOrTube(int tileX, int tileY, int unused, map_id wallTubeType);      // Returns: 1 [true] always
		//[DllImport("DotNetInterop.dll")] public static extern int CreateUnitBlock(_Player& ownerPlayer, string exportName, int bLightsOn);		// Returns: numUnitsCreated,  Note: see class UnitBlock

		// Morale Level
		//  Note: playerNum: -1 = PlayerAll
		//  Note: Calling ForceMoraleX functions after tick 0 will cause a "Cheated Game!" message to appear. FreeMoraleLevel can be called any time.
		//  Bug: ForceMoraleX is buggy if playerNum is not -1. You may need to call the function twice for the correct effect (see Forum post). FreeMoraleLevel is not affected by this bug.
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_ForceMoraleGreat(int playerNum);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_ForceMoraleGood(int playerNum);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_ForceMoraleOK(int playerNum);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_ForceMoralePoor(int playerNum);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_ForceMoraleRotten(int playerNum);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_FreeMoraleLevel(int playerNum);			// Let Morale vary according to colony state and events

		// Random number generation
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SetSeed(uint randNumSeed);				// Set random number seed
		[DllImport("DotNetInterop.dll")] private static extern int TethysGame_GetRand(int range);						// Returns a number from 0..(range-1)

		// Disaster Creation
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SetMeteor(int tileX, int tileY, int size);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SetEarthquake(int tileX, int tileY, int magnitude);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SetEruption(int tileX, int tileY, int spreadSpeed);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SetLightning(int startTileX, int startTileY, int duration, int endTileX, int endTileY);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SetTornado(int startTileX, int startTileY, int duration, int endTileX, int endTileY, int bImmediate);
		// Disaster spread speed
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SetLavaSpeed(int spreadSpeed);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SetMicrobeSpreadSpeed(int spreadSpeed);

		// EMP Missile
		//  Note: FindEMPMissileTarget searches aligned 8x8 blocks, for the block with the greatest weight
		//  Note: The target location is at the block center (+3, +3)
		//  Note: Targets first found block of heighest (non-negative) weight, or the first block if all blocks have negative weight
		//  Note: Target player military units weigh 64, non-target player military units weigh -32, and non-target player non-military units weigh 1.
		[DllImport("DotNetInterop.dll")] private static extern long TethysGame_FindEMPMissleTarget(int startTileX, int startTileY, int endTileX, int endTileY, int destPlayerNum);
		
		// Launches an EMP missile. May be launched from off screen (no spaceport required).
		// Will not launch an EMP missile if the selected sourcePlayer is not Plymouth.
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SetEMPMissile(int launchTileX, int launchTileY, int sourcePlayerNum, int destTileX, int destTileY);

		// Save/Load Games
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SaveGame(string savedGameName);		// Note: Unimplemented  [Useless]
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_LoadGame(string savedGameName);		// Note: Saved game names default to: "SGAME?.OP2" file name format

		// Misc
		[DllImport("DotNetInterop.dll")] private static extern void TethysGame_SetMusicPlayList(int numSongs, int repeatStartIndex, int[] songIdList);

		//[DllImport("DotNetInterop.dll")] private static extern void Interop_sIssueOptPacket(int variableId, int newValue);
	}
}
