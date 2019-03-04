// Note: This file contains the defintion of the TethysGame class
//		which controls the overall game environment

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
		public static Player GetPlayer(int playerNum)		{ return new Player(TethysGame_GetPlayer(playerNum), false);	}

		// Multiplayer game options  [Get Property]
		public static bool UsesDayNight()					{ return TethysGame_UsesDayNight() != 0;				}
		public static bool UsesMorale()						{ return TethysGame_UsesMorale() != 0;					}
		public static bool CanHaveDisasters()				{ return TethysGame_CanHaveDisasters() != 0;			}
		public static int InitialUnits()					{ return TethysGame_InitialUnits();						}	// Number of Laser/Microwave Lynx to start with
		public static bool CanAllowCheats()					{ return TethysGame_CanAllowCheats() != 0;				}	// Useless

		// Debug/Cheat flags
		public static void SetDaylightEverywhere(bool isOn)	{ TethysGame_SetDaylightEverywhere(isOn ? 1 : 0);		}
		public static void SetDaylightMoves(bool isOn)		{ TethysGame_SetDaylightMoves(isOn ? 1 : 0);			}

		// Unit Creation  [Returns: int numUnitsCreated]
		public static int CreateUnit(out Unit returnedUnit, map_id unitType, int tileX, int tileY, int playerNum, map_id weaponCargoType, int rotation)
		{
			returnedUnit = new Unit();

			return TethysGame_CreateUnit(returnedUnit.GetHandle(), unitType, tileX, tileY, playerNum, weaponCargoType, rotation);
		}
		// Note: techID must be >= 8000 but < (8000+4096) = 12096
		public static int CreateWreck(int tileX, int tileY, map_id techID, bool isInitiallyVisible)
		{
			return TethysGame_CreateWreck(tileX, tileY, techID, isInitiallyVisible ? 1 : 0);
		}
		public static int PlaceMarker(out Unit returnedUnit, int tileX, int tileY, int markerType)
		{
			returnedUnit = new Unit();

			return TethysGame_PlaceMarker(returnedUnit.GetHandle(), tileX, tileY, markerType);
		}


		// Player Number and Number of Players
		[DllImport("NativeInterop.dll")] public static extern int LocalPlayer();            // Returns the local player index
		[DllImport("NativeInterop.dll")] public static extern int NoPlayers();              // Returns number of players (including both Human and AI)
		[DllImport("NativeInterop.dll")] private static extern IntPtr TethysGame_GetPlayer(int playerNum);

		// Multiplayer game options  [Get Property]
		[DllImport("NativeInterop.dll")] private static extern int TethysGame_UsesDayNight();
		[DllImport("NativeInterop.dll")] private static extern int TethysGame_UsesMorale();
		[DllImport("NativeInterop.dll")] private static extern int TethysGame_CanHaveDisasters();
		[DllImport("NativeInterop.dll")] private static extern int TethysGame_InitialUnits();       // Number of Laser/Microwave Lynx to start with
		[DllImport("NativeInterop.dll")] private static extern int TethysGame_CanAllowCheats();     // Useless

		// Game Time
		[DllImport("NativeInterop.dll")] public static extern int Tick();   // Current time (tick is the smallest slice of game time)
		[DllImport("NativeInterop.dll")] public static extern int Time();   // Current tick / 4  (most processing is only done every 4 ticks)

		// Game Sounds and Voice warnings
		//  Note: SoundIndex = 94..227 [Inclusive] are recorded voice messages
		[DllImport("NativeInterop.dll")] public static extern void AddGameSound(int soundIndex, int toPlayerNum);           // Note: toPlayerNum: -1 = PlayerAll
		[DllImport("NativeInterop.dll")] public static extern void AddMapSound(int soundIndex, int tileX, int tileY);
		// Message log
		[DllImport("NativeInterop.dll")] public static extern void AddMessage(int pixelX, int pixelY, string message, int toPlayerNum, int soundIndex); // Note: toPlayerNum: -1 = PlayerAll
																																						//[DllImport("NativeInterop.dll")] public static extern void AddMessage(Unit owner, string message, int toPlayerNum, int soundIndex);				// Note: toPlayerNum: -1 = PlayerAll

		// Debug/Cheat flags
		[DllImport("NativeInterop.dll")] private static extern void TethysGame_SetDaylightEverywhere(int bOn);
		[DllImport("NativeInterop.dll")] private static extern void TethysGame_SetDaylightMoves(int bOn);
		// Cheat flags  [gutted and useless by official Sierra update]
		[DllImport("NativeInterop.dll")] public static extern void SetCheatFastProduction(int bOn);         // Useless
		[DllImport("NativeInterop.dll")] public static extern void SetCheatFastUnits(int bOn);              // Useless
		[DllImport("NativeInterop.dll")] public static extern void SetCheatProduceAll(int bOn);             // Useless
		[DllImport("NativeInterop.dll")] public static extern void SetCheatUnlimitedResources(int bOn);     // Useless

		// Unit Creation  [Returns: int numUnitsCreated]
		[DllImport("NativeInterop.dll")] private static extern int TethysGame_CreateUnit(IntPtr returnedUnit, map_id unitType, int tileX, int tileY, int playerNum, map_id weaponCargoType, int rotation);  // Note: see enum UnitDirection
		[DllImport("NativeInterop.dll")] public static extern int CreateBeacon(map_id beaconType, int tileX, int tileY, int commonRareType, int barYield, int barVariant);  // Note: see enums BeaconTypes, Yield, Variant
		[DllImport("NativeInterop.dll")] private static extern int TethysGame_CreateWreck(int tileX, int tileY, map_id techID, int bInitiallyVisible);      // Note: techID must be >= 8000 but < (8000+4096) = 12096
		[DllImport("NativeInterop.dll")] private static extern int TethysGame_PlaceMarker(IntPtr returnedUnit, int tileX, int tileY, int markerType);       // Note: See enum MarkerTypes
		[DllImport("NativeInterop.dll")] public static extern int CreateWallOrTube(int tileX, int tileY, int unused, map_id wallTubeType);      // Returns: 1 [true] always
																																				//[DllImport("NativeInterop.dll")] public static extern int CreateUnitBlock(_Player& ownerPlayer, string exportName, int bLightsOn);		// Returns: numUnitsCreated,  Note: see class UnitBlock

		// Morale Level
		//  Note: playerNum: -1 = PlayerAll
		//  Note: Calling ForceMoraleX functions after tick 0 will cause a "Cheated Game!" message to appear. FreeMoraleLevel can be called any time.
		//  Bug: ForceMoraleX is buggy if playerNum is not -1. You may need to call the function twice for the correct effect (see Forum post). FreeMoraleLevel is not affected by this bug.
		[DllImport("NativeInterop.dll")] public static extern void ForceMoraleGreat(int playerNum);
		[DllImport("NativeInterop.dll")] public static extern void ForceMoraleGood(int playerNum);
		[DllImport("NativeInterop.dll")] public static extern void ForceMoraleOK(int playerNum);
		[DllImport("NativeInterop.dll")] public static extern void ForceMoralePoor(int playerNum);
		[DllImport("NativeInterop.dll")] public static extern void ForceMoraleRotten(int playerNum);
		[DllImport("NativeInterop.dll")] public static extern void FreeMoraleLevel(int playerNum);      // Let Morale vary according to colony state and events

		// Random number generation
		[DllImport("NativeInterop.dll")] public static extern void SetSeed(uint randNumSeed);			// Set random number seed
		[DllImport("NativeInterop.dll")] public static extern int GetRand(int range);                   // Returns a number from 0..(range-1)

		// Disaster Creation
		[DllImport("NativeInterop.dll")] public static extern void SetMeteor(int tileX, int tileY, int size);
		[DllImport("NativeInterop.dll")] public static extern void SetEarthquake(int tileX, int tileY, int magnitude);
		[DllImport("NativeInterop.dll")] public static extern void SetEruption(int tileX, int tileY, int spreadSpeed);
		[DllImport("NativeInterop.dll")] public static extern void SetLightning(int startTileX, int startTileY, int duration, int endTileX, int endTileY);
		[DllImport("NativeInterop.dll")] public static extern void SetTornado(int startTileX, int startTileY, int duration, int endTileX, int endTileY, int bImmediate);
		// Disaster spread speed
		[DllImport("NativeInterop.dll")] public static extern void SetLavaSpeed(int spreadSpeed);
		[DllImport("NativeInterop.dll")] public static extern void SetMicrobeSpreadSpeed(int spreadSpeed);

		// EMP Missile
		//  Note: FindEMPMissileTarget searches aligned 8x8 blocks, for the block with the greatest weight
		//  Note: The target location is at the block center (+3, +3)
		//  Note: Targets first found block of heighest (non-negative) weight, or the first block if all blocks have negative weight
		//  Note: Target player military units weigh 64, non-target player military units weigh -32, and non-target player non-military units weigh 1.
		public static LOCATION FindEMPMissleTarget(int startTileX, int startTileY, int endTileX, int endTileY, int destPlayerNum)
		{
			long result = _FindEMPMissleTarget(startTileX, startTileY, endTileX, endTileY, destPlayerNum);
			LOCATION loc = new LOCATION();
			loc.x = (int)(result & uint.MaxValue);
			loc.y = (int)(result >> 32);

			return loc;
		}
		[DllImport("NativeInterop.dll")] private static extern long _FindEMPMissleTarget(int startTileX, int startTileY, int endTileX, int endTileY, int destPlayerNum);
		// Launches an EMP missile. May be launched from off screen (no spaceport required).
		// Will not launch an EMP missile if the selected sourcePlayer is not Plymouth.
		[DllImport("NativeInterop.dll")] public static extern void SetEMPMissile(int launchTileX, int launchTileY, int sourcePlayerNum, int destTileX, int destTileY);

		// Save/Load Games
		[DllImport("NativeInterop.dll")] public static extern void SaveGame(string savedGameName);		// Note: Unimplemented  [Useless]
		[DllImport("NativeInterop.dll")] public static extern void LoadGame(string savedGameName);		// Note: Saved game names default to: "SGAME?.OP2" file name format

		// Misc
		[DllImport("NativeInterop.dll")] public static extern void SetMusicPlayList(int numSongs, int repeatStartIndex, int[] songIdList);

		//[DllImport("NativeInterop.dll")] private static extern void Interop_sIssueOptPacket(int variableId, int newValue);

		/// <summary>
		/// Gets a random value between min and max.
		/// </summary>
		/// <param name="min">The minimum value. Inclusive</param>
		/// <param name="max">The maximum value. Exclusive</param>
		/// <returns>The random value between min and max.</returns>
		public static int GetRandomRange(int min, int max)
		{
			return GetRand(max-min) + min;
		}

		public static LOCATION GetRandomLocation(MAP_RECT rect)
		{
			return new LOCATION(
				GetRand(rect.width) + rect.minX+1,
				GetRand(rect.height) + rect.minY+1
				);
		}

		public static LOCATION GetMapCoordinates(LOCATION gameCoordinates)
		{
			return new LOCATION(gameCoordinates.x + 31, gameCoordinates.y - 1);
		}
	}
}
