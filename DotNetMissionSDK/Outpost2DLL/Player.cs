// Note: This file contains the _Player class definition exported from
//		 Outpost2.exe. Use this class to modify anything associated
//		 with a player (human or AI).
// Note: The AI in Outpost 2 is sadly faked. Using _Player.GoAI() is
//		 essentially creating a computer controlled player that cheats.
//		 It's given essentially unlimited population resources and so
//		 it doesn't have to worry about food or idling buildings due
//		 to lack of workers. You can make a much more realistic AI
//		 if you leave the player set as human and just program all
//		 the actions yourself (which you'd have to do anyways since
//		 Outpost 2 has no real AI and all computer actions are
//		 hardcoded into each DLL).

using DotNetMissionSDK.Async;
using System;
using System.Runtime.InteropServices;


namespace DotNetMissionSDK
{
	public class Player
	{
		private IntPtr m_Handle;

		public int playerID { get; private set; }


		public Player(IntPtr handle, int playerID)
		{
			m_Handle = handle;
			this.playerID = playerID;
		}

		public IntPtr GetHandle()			{ return m_Handle;											}


		// [Get] Game Settings
		public int Difficulty()				{ ThreadAssert.MainThreadRequired();	return Player_Difficulty(m_Handle);							}
		public bool IsEden()				{ ThreadAssert.MainThreadRequired();	return Player_IsEden(m_Handle) != 0;						}
		public bool IsHuman()				{ ThreadAssert.MainThreadRequired();	return Player_IsHuman(m_Handle) != 0;						}
		// [Get] Population
		public int Kids()					{ ThreadAssert.MainThreadRequired();	return Player_Kids(m_Handle);								}
		public int Workers()				{ ThreadAssert.MainThreadRequired();	return Player_Workers(m_Handle);							}
		public int Scientists()				{ ThreadAssert.MainThreadRequired();	return Player_Scientists(m_Handle);							}
		public int TotalPopulation()		{ ThreadAssert.MainThreadRequired();	return Kids() + Workers() + Scientists();					}
		// [Get] Resources
		public int Ore()					{ ThreadAssert.MainThreadRequired();	return Player_Ore(m_Handle);								}
		public int RareOre()				{ ThreadAssert.MainThreadRequired();	return Player_RareOre(m_Handle);							}
		public int FoodStored()				{ ThreadAssert.MainThreadRequired();	return Player_FoodStored(m_Handle);							}
		public FoodStatus FoodSupply()		{ ThreadAssert.MainThreadRequired();	return Player_FoodSupply(m_Handle);							}
		// [Get] Misc
		public MoraleLevel MoraleLevel()	{ ThreadAssert.MainThreadRequired();	return Player_MoraleLevel(m_Handle);						}
		public int GetRLVCount()			{ ThreadAssert.MainThreadRequired();	return Player_GetRLVCount(m_Handle);						}
		// [Get] Indirect property lookups
		/// <summary>
		/// Returns true if the player has the specified technology.
		/// NOTE: techID is NOT the array index value returned by GetResearchTopic. Use GetTechInfo().GetTechID().
		/// </summary>
		/// <param name="techID">The techID of the technology as found in the techInfo.txt files.</param>
		public bool HasTechnology(int techID)	{ ThreadAssert.MainThreadRequired();	return Player_HasTechnology(m_Handle, techID) != 0;		}
		public ScGroup GetDefaultGroup()		{ ThreadAssert.MainThreadRequired();	return new ScGroup(Player_GetDefaultGroup(m_Handle));	}
		// [Get] Player Strength  [Calculational]
		// Note: Unit Strengths are as follows:
		//	Spider/Scorpion	: 4
		//	Lynx			: Laser/Microwave: 5 Other: 6 ThorsHammer: 7
		//	Panther			: Laser/Microwave: 7 Other: 8 ThorsHammer: 9
		//	Tiger			: Laser/Microwave: 8 Other: 9 ThorsHammer: 10
		//	Guard Post		: Laser/Microwave: 4 Other: 5 ThorsHammer: 7
		//	Other			: 0  [Including Units in a Garage]
	
		// Returns (strength / 8), where strength is the sum of all units owned by the player in the given map rectangle
		public int GetPlayerStrength(MAP_RECT mapRect)	{ ThreadAssert.MainThreadRequired();	return Player_GetPlayerStrength(m_Handle, mapRect.xMin, mapRect.yMin, mapRect.xMax, mapRect.yMax);	}
		// Returns (strength / 8), where strength is the sum of all units owned by the player
		public int GetTotalPlayerStrength()				{ ThreadAssert.MainThreadRequired();	return Player_GetTotalPlayerStrength(m_Handle);								}
		// [Get] Checks  [Prerequisite searching]
		// Checks for (CommonOreMine, or (hasVehicle(mapRoboMiner, mapAny), or canBuildVehicle(true))) + (hasVehicle(mapCargoTruck, mapAny), or canBuildVehicle(true)) + (CommonOreSmelter, or canBuildBuilding)
		public bool CanAccumulateOre()					{ ThreadAssert.MainThreadRequired();	return Player_CanAccumulateOre(m_Handle) != 0;								}
		// Checks for (RareOreMine, or (hasVehicle(mapRoboMiner, mapAny), or canBuildVehicle(true))) + (hasVehicle(mapCargoTruck, mapAny), or canBuildVehicle(true)) + (RareOreSmelter, or canBuildBuilding)
		public bool CanAccumulateRareOre()				{ ThreadAssert.MainThreadRequired();	return Player_CanAccumulateRareOre(m_Handle) != 0;							}
		// Checks for Spaceport, or hasVehicle(mapConvec, mapSpaceport), or canBuildBuilding
		public bool CanBuildSpace()						{ ThreadAssert.MainThreadRequired();	return Player_CanBuildSpace(m_Handle) != 0;									}
		// Checks for StructureFactory + (Convec, or (VehicleFactory, or (redundant) hasVehicle(mapConvec, mapVehicleFactory))), or hasVehicle(mapConvec, mapStructureFactory)
		public bool CanBuildBuilding()					{ ThreadAssert.MainThreadRequired();	return Player_CanBuildBuilding(m_Handle) != 0;								}
		// Checks for VehicleFactory, or hasVehicle(mapConvec, mapVehicleFactory), or [optional] canBuildBuilding  [Note: Uses last cached result if available, so optional parameter may not function as expected]
		public bool CanBuildVehicle(bool bCheckCanBuildBuilding){ ThreadAssert.MainThreadRequired();	return Player_CanBuildVehicle(m_Handle, bCheckCanBuildBuilding ? 1 : 0) != 0;	}
		// Checks for <Tech.labType>Lab, or hasVehicle(mapConvec, map<Techc.labType>Lab), or canBuildBuilding
		public bool CanDoResearch(int techID)			{ ThreadAssert.MainThreadRequired();	return Player_CanDoResearch(m_Handle, techID) != 0;							}
		// [cargoOrWeaponType: -1 = mapAny]  Checks for free units, or units in Garages
		public bool HasVehicle(map_id vehicleType, map_id cargoOrWeaponType)	{ ThreadAssert.MainThreadRequired();	return Player_HasVehicle(m_Handle, vehicleType, cargoOrWeaponType) > 0;		}
		public bool HasActiveCommand()					{ ThreadAssert.MainThreadRequired();	return Player_HasActiveCommand(m_Handle) > 0;								}
		// Reset cached check values
		// Clears checkValue array to -1  [Not Set]
		public void ResetChecks()						{ ThreadAssert.MainThreadRequired();	Player_ResetChecks(m_Handle);												}

		// [Set] Game Settings
		public void SetColorNumber(PlayerColor color)	{ ThreadAssert.MainThreadRequired();	Player_SetColorNumber(m_Handle, color);										}
		// [Set] Population
		public void SetKids(int numKids)				{ ThreadAssert.MainThreadRequired();	Player_SetKids(m_Handle, numKids);											}
		public void SetWorkers(int numWorkers)			{ ThreadAssert.MainThreadRequired();	Player_SetWorkers(m_Handle, numWorkers);									}
		public void SetScientists(int numScientists)	{ ThreadAssert.MainThreadRequired();	Player_SetScientists(m_Handle, numScientists);								}
		// [Set] Resources
		public void SetOre(int newCommonOre)			{ ThreadAssert.MainThreadRequired();	Player_SetOre(m_Handle, newCommonOre);										}
		public void SetRareOre(int newRareOre)			{ ThreadAssert.MainThreadRequired();	Player_SetRareOre(m_Handle, newRareOre);									}
		public void SetFoodStored(int newFoodStored)	{ ThreadAssert.MainThreadRequired();	Player_SetFoodStored(m_Handle, newFoodStored);								}
		// [Set] Misc
		public void SetSolarSat(int numSolarSatellites)	{ ThreadAssert.MainThreadRequired();	Player_SetSolarSat(m_Handle, numSolarSatellites);							}
		// [Set] Indirect property setting
		// Gives all techs with techID <= (techLevel * 1000), and all free subsequent techs
		public void SetTechLevel(int techLevel)			{ ThreadAssert.MainThreadRequired();	Player_SetTechLevel(m_Handle, techLevel);									}
		// Gives the tech with the given tech ID, and all free subsequent techs
		public void MarkResearchComplete(int techID)	{ ThreadAssert.MainThreadRequired();	Player_MarkResearchComplete(m_Handle, techID);								}
		public void SetDefaultGroup(ScGroup newDefaultGroup)	{ ThreadAssert.MainThreadRequired();	Player_SetDefaultGroup(m_Handle, newDefaultGroup.stubIndex);		}

		// [Method]
		public void GoEden()							{ ThreadAssert.MainThreadRequired();	Player_GoEden(m_Handle);													}
		public void GoPlymouth()						{ ThreadAssert.MainThreadRequired();	Player_GoPlymouth(m_Handle);												}
		public void GoAI()								{ ThreadAssert.MainThreadRequired();	Player_GoAI(m_Handle);														}
		public void GoHuman()							{ ThreadAssert.MainThreadRequired();	Player_GoHuman(m_Handle);													}
		public void AllyWith(int playerNum)				{ ThreadAssert.MainThreadRequired();	Player_AllyWith(m_Handle, playerNum);										}
		// Steals an RLV from the source Player, provided they have one
		public void CaptureRLV(int sourcePlayerNum)		{ ThreadAssert.MainThreadRequired();	Player_CaptureRLV(m_Handle, sourcePlayerNum);								}
		// Sets the view for this Player (does nothing if player is not the local player)
		public void CenterViewOn(int tileX, int tileY)	{ ThreadAssert.MainThreadRequired();	Player_CenterViewOn(m_Handle, tileX, tileY);								}
	

		[DllImport("DotNetInterop.dll")] private static extern IntPtr Player_Create(int playerNum);
		[DllImport("DotNetInterop.dll")] private static extern void Player_Release(IntPtr handle);

		// [Get] Game Settings
		[DllImport("DotNetInterop.dll")] private static extern int Player_Difficulty(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int Player_IsEden(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int Player_IsHuman(IntPtr handle);
		// [Get] Population
		[DllImport("DotNetInterop.dll")] private static extern int Player_Kids(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int Player_Workers(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int Player_Scientists(IntPtr handle);
		// [Get] Resources
		[DllImport("DotNetInterop.dll")] private static extern int Player_Ore(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int Player_RareOre(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int Player_FoodStored(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern FoodStatus Player_FoodSupply(IntPtr handle);
		// [Get] Misc
		[DllImport("DotNetInterop.dll")] private static extern MoraleLevel Player_MoraleLevel(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int Player_GetRLVCount(IntPtr handle);
		// [Get] Indirect property lookups
		[DllImport("DotNetInterop.dll")] private static extern int Player_HasTechnology(IntPtr handle, int techID);
		[DllImport("DotNetInterop.dll")] private static extern int Player_GetDefaultGroup(IntPtr handle);
		// [Get] Player Strength  [Calculational]
		// Note: Unit Strengths are as follows:
		//	Spider/Scorpion	: 4
		//	Lynx			: Laser/Microwave: 5 Other: 6 ThorsHammer: 7
		//	Panther			: Laser/Microwave: 7 Other: 8 ThorsHammer: 9
		//	Tiger			: Laser/Microwave: 8 Other: 9 ThorsHammer: 10
		//	Guard Post		: Laser/Microwave: 4 Other: 5 ThorsHammer: 7
		//	Other			: 0  [Including Units in a Garage]
		[DllImport("DotNetInterop.dll")] private static extern int Player_GetPlayerStrength(IntPtr handle, int x1,int y1,int x2,int y2);			// Returns (strength / 8), where strength is the sum of all units owned by the player in the given map rectangle
		[DllImport("DotNetInterop.dll")] private static extern int Player_GetTotalPlayerStrength(IntPtr handle);									// Returns (strength / 8), where strength is the sum of all units owned by the player
		// [Get] Checks  [Prerequisite searching]
		[DllImport("DotNetInterop.dll")] private static extern int Player_CanAccumulateOre(IntPtr handle);											// Checks for (CommonOreMine, or (hasVehicle(mapRoboMiner, mapAny), or canBuildVehicle(true))) + (hasVehicle(mapCargoTruck, mapAny), or canBuildVehicle(true)) + (CommonOreSmelter, or canBuildBuilding)
		[DllImport("DotNetInterop.dll")] private static extern int Player_CanAccumulateRareOre(IntPtr handle);										// Checks for (RareOreMine, or (hasVehicle(mapRoboMiner, mapAny), or canBuildVehicle(true))) + (hasVehicle(mapCargoTruck, mapAny), or canBuildVehicle(true)) + (RareOreSmelter, or canBuildBuilding)
		[DllImport("DotNetInterop.dll")] private static extern int Player_CanBuildSpace(IntPtr handle);												// Checks for Spaceport, or hasVehicle(mapConvec, mapSpaceport), or canBuildBuilding
		[DllImport("DotNetInterop.dll")] private static extern int Player_CanBuildBuilding(IntPtr handle);											// Checks for StructureFactory + (Convec, or (VehicleFactory, or (redundant) hasVehicle(mapConvec, mapVehicleFactory))), or hasVehicle(mapConvec, mapStructureFactory)
		[DllImport("DotNetInterop.dll")] private static extern int Player_CanBuildVehicle(IntPtr handle, int bCheckCanBuildBuilding);				// Checks for VehicleFactory, or hasVehicle(mapConvec, mapVehicleFactory), or [optional] canBuildBuilding  [Note: Uses last cached result if available, so optional parameter may not function as expected]
		[DllImport("DotNetInterop.dll")] private static extern int Player_CanDoResearch(IntPtr handle, int techID);									// Checks for <Tech.labType>Lab, or hasVehicle(mapConvec, map<Techc.labType>Lab), or canBuildBuilding
		[DllImport("DotNetInterop.dll")] private static extern int Player_HasVehicle(IntPtr handle, map_id vehicleType, map_id cargoOrWeaponType);	// [cargoOrWeaponType: -1 = mapAny]  Checks for free units, or units in Garages
		[DllImport("DotNetInterop.dll")] private static extern int Player_HasActiveCommand(IntPtr handle);											// Returns (numActiveCommandCenters > 0)
		// Reset cached check values
		[DllImport("DotNetInterop.dll")] private static extern void Player_ResetChecks(IntPtr handle);												// Clears checkValue array to -1  [Not Set]

		// [Set] Game Settings
		[DllImport("DotNetInterop.dll")] private static extern void Player_SetColorNumber(IntPtr handle, PlayerColor colorIndex);
		// [Set] Population
		[DllImport("DotNetInterop.dll")] private static extern void Player_SetKids(IntPtr handle, int numKids);
		[DllImport("DotNetInterop.dll")] private static extern void Player_SetWorkers(IntPtr handle, int numWorkers);
		[DllImport("DotNetInterop.dll")] private static extern void Player_SetScientists(IntPtr handle, int numScientists);
		// [Set] Resources
		[DllImport("DotNetInterop.dll")] private static extern void Player_SetOre(IntPtr handle, int newCommonOre);
		[DllImport("DotNetInterop.dll")] private static extern void Player_SetRareOre(IntPtr handle, int newRareOre);
		[DllImport("DotNetInterop.dll")] private static extern void Player_SetFoodStored(IntPtr handle, int newFoodStored);
		// [Set] Misc
		[DllImport("DotNetInterop.dll")] private static extern void Player_SetSolarSat(IntPtr handle, int numSolarSatellites);
		// [Set] Indirect property setting
		[DllImport("DotNetInterop.dll")] private static extern void Player_SetTechLevel(IntPtr handle, int techLevel);								// Gives all techs with techID <= (techLevel * 1000), and all free subsequent techs
		[DllImport("DotNetInterop.dll")] private static extern void Player_MarkResearchComplete(IntPtr handle, int techID);							// Gives the tech with the given tech ID, and all free subsequent techs
		[DllImport("DotNetInterop.dll")] private static extern void Player_SetDefaultGroup(IntPtr handle, int newDefaultGroupIndex);

		// [Method]
		[DllImport("DotNetInterop.dll")] private static extern void Player_GoEden(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern void Player_GoPlymouth(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern void Player_GoAI(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern void Player_GoHuman(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern void Player_AllyWith(IntPtr handle, int playerNum);
		[DllImport("DotNetInterop.dll")] private static extern void Player_CaptureRLV(IntPtr handle, int sourcePlayerNum);							// Steals an RLV from the source Player, provided they have one
		[DllImport("DotNetInterop.dll")] private static extern void Player_CenterViewOn(IntPtr handle, int tileX, int tileY);						// Sets the view for this Player (does nothing if player is not the local player)

	//public:	// Why not? :)
	//	char checkValue[8];		// 0 = False, 1 = True, -1 = Not Set

		/*enum PlayerCheckIndex
		{
			PlayerCheckIndexStructureFactory	= 0,	// Has Structure, Kit loaded in Convec, or can build
			PlayerCheckIndexVehicleFactory		= 1,	// Has Structure, Kit loaded in Convec, or can build (partly optional)
			PlayerCheckIndexSpaceport			= 2,	// Has Structure, Kit loaded in Convec, or can build
			PlayerCheckIndexCommonOre			= 3,	// Has Mine+Smelter+CargoTruck, or can build
			PlayerCheckIndexRareOre				= 4,	// Has Mine+Smelter+CargoTruck, or can build
			PlayerCheckIndexBasicLab			= 5,	// Has Structure, Kit loaded in Convec, or can build
			PlayerCheckIndexStandardLab			= 6,	// Has Structure, Kit loaded in Convec, or can build
			PlayerCheckIndexAdvancedLab			= 7,	// Has Structure, Kit loaded in Convec, or can build
		};*/
	}
}
