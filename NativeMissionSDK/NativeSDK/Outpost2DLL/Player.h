#pragma once
#ifndef OP2
#define OP2 __declspec(dllimport)
#endif

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


// External type names
enum map_id;
struct MAP_RECT;
class ScGroup;		// **


class OP2 _Player
{
public:
	// Object Construction/Assignment
	_Player(int playerNum);
	_Player& operator = (const _Player& player);

	// [Get] Game Settings
	int Difficulty() const;
	int IsEden();
	int IsHuman();
	// [Get] Population
	int Kids() const;
	int Workers() const;
	int Scientists() const;
	// [Get] Resources
	int Ore() const;
	int RareOre() const;
	int FoodStored() const;
	enum FoodStatus FoodSupply() const;
	// [Get] Misc
	enum MoraleLevels MoraleLevel() const;
	int GetRLVCount();
	// [Get] Indirect property lookups
	int HasTechnology(int techID) const;
	ScGroup GetDefaultGroup();
	// [Get] Player Strength  [Calculational]
	// Note: Unit Strengths are as follows:
	//	Spider/Scorpion	: 4
	//	Lynx			: Laser/Microwave: 5 Other: 6 ThorsHammer: 7
	//	Panther			: Laser/Microwave: 7 Other: 8 ThorsHammer: 9
	//	Tiger			: Laser/Microwave: 8 Other: 9 ThorsHammer: 10
	//	Guard Post		: Laser/Microwave: 4 Other: 5 ThorsHammer: 7
	//	Other			: 0  [Including Units in a Garage]
	int GetPlayerStrength(MAP_RECT& mapRect);						// Returns (strength / 8), where strength is the sum of all units owned by the player in the given map rectangle
	int GetTotalPlayerStrength();									// Returns (strength / 8), where strength is the sum of all units owned by the player
	// [Get] Checks  [Prerequisite searching]
	int canAccumulateOre();											// Checks for (CommonOreMine, or (hasVehicle(mapRoboMiner, mapAny), or canBuildVehicle(true))) + (hasVehicle(mapCargoTruck, mapAny), or canBuildVehicle(true)) + (CommonOreSmelter, or canBuildBuilding)
	int canAccumulateRareOre();										// Checks for (RareOreMine, or (hasVehicle(mapRoboMiner, mapAny), or canBuildVehicle(true))) + (hasVehicle(mapCargoTruck, mapAny), or canBuildVehicle(true)) + (RareOreSmelter, or canBuildBuilding)
	int canBuildSpace();											// Checks for Spaceport, or hasVehicle(mapConvec, mapSpaceport), or canBuildBuilding
	int canBuildBuilding();											// Checks for StructureFactory + (Convec, or (VehicleFactory, or (redundant) hasVehicle(mapConvec, mapVehicleFactory))), or hasVehicle(mapConvec, mapStructureFactory)
	int canBuildVehicle(int bCheckCanBuildBuilding);				// Checks for VehicleFactory, or hasVehicle(mapConvec, mapVehicleFactory), or [optional] canBuildBuilding  [Note: Uses last cached result if available, so optional parameter may not function as expected]
	int canDoResearch(int techID);									// Checks for <Tech.labType>Lab, or hasVehicle(mapConvec, map<Techc.labType>Lab), or canBuildBuilding
	int hasVehicle(map_id vehicleType, map_id cargoOrWeaponType);	// [cargoOrWeaponType: -1 = mapAny]  Checks for free units, or units in Garages
	int hasActiveCommand();											// Returns (numActiveCommandCenters > 0)
	// Reset cached check values
	void resetChecks();												// Clears checkValue array to -1  [Not Set]

	// [Set] Game Settings
	void SetColorNumber(int colorIndex);
	// [Set] Population
	void SetKids(int numKids);
	void SetWorkers(int numWorkers);
	void SetScientists(int numScientists);
	// [Set] Resources
	void SetOre(int newCommonOre);
	void SetRareOre(int newRareOre);
	void SetFoodStored(int newFoodStored);
	// [Set] Misc
	void SetSolarSat(int numSolarSatellites);
	// [Set] Indirect property setting
	void SetTechLevel(int techLevel);								// Gives all techs with techID <= (techLevel * 1000), and all free subsequent techs
	void MarkResearchComplete(int techID);							// Gives the tech with the given tech ID, and all free subsequent techs
	void SetDefaultGroup(ScGroup& newDefaultGroup);

	// [Method]
	void GoEden();
	void GoPlymouth();
	void GoAI();
	void GoHuman();
	void AllyWith(int playerNum);
	void CaptureRLV(int sourcePlayerNum);							// Steals an RLV from the source Player, provided they have one
	void CenterViewOn(int tileX, int tileY);						// Sets the view for this Player (does nothing if player is not the local player)

public:	// Why not? :)
	int playerNum;
	char checkValue[8];		// 0 = False, 1 = True, -1 = Not Set

	enum PlayerCheckIndex
	{
		PlayerCheckIndexStructureFactory	= 0,	// Has Structure, Kit loaded in Convec, or can build
		PlayerCheckIndexVehicleFactory		= 1,	// Has Structure, Kit loaded in Convec, or can build (partly optional)
		PlayerCheckIndexSpaceport			= 2,	// Has Structure, Kit loaded in Convec, or can build
		PlayerCheckIndexCommonOre			= 3,	// Has Mine+Smelter+CargoTruck, or can build
		PlayerCheckIndexRareOre				= 4,	// Has Mine+Smelter+CargoTruck, or can build
		PlayerCheckIndexBasicLab			= 5,	// Has Structure, Kit loaded in Convec, or can build
		PlayerCheckIndexStandardLab			= 6,	// Has Structure, Kit loaded in Convec, or can build
		PlayerCheckIndexAdvancedLab			= 7,	// Has Structure, Kit loaded in Convec, or can build
	};
};
