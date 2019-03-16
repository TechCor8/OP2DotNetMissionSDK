#include "stdafx_unmanaged.h"

#include <Outpost2DLL/Outpost2DLL.h>	// Main Outpost 2 header to interface with the game

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif


extern "C"
{
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


	extern EXPORT _Player* __stdcall Player_Create(int playerNum)
	{
		return new _Player(playerNum);
	}

	extern EXPORT void __stdcall Player_Release(_Player* handle)
	{
		delete handle;
	}

	// [Get] Game Settings
	extern EXPORT int __stdcall Player_Difficulty(_Player* handle)
	{
		return handle->Difficulty();
	}
	extern EXPORT int __stdcall Player_IsEden(_Player* handle)
	{
		return handle->IsEden();
	}
	extern EXPORT int __stdcall Player_IsHuman(_Player* handle)
	{
		return handle->IsHuman();
	}
	// [Get] Population
	extern EXPORT int __stdcall Player_Kids(_Player* handle)
	{
		return handle->Kids();
	}
	extern EXPORT int __stdcall Player_Workers(_Player* handle)
	{
		return handle->Workers();
	}
	extern EXPORT int __stdcall Player_Scientists(_Player* handle)
	{
		return handle->Scientists();
	}
	// [Get] Resources
	extern EXPORT int __stdcall Player_Ore(_Player* handle)
	{
		return handle->Ore();
	}
	extern EXPORT int __stdcall Player_RareOre(_Player* handle)
	{
		return handle->RareOre();
	}
	extern EXPORT int __stdcall Player_FoodStored(_Player* handle)
	{
		return handle->FoodStored();
	}
	extern EXPORT FoodStatus __stdcall Player_FoodSupply(_Player* handle)
	{
		return handle->FoodSupply();
	}
	// [Get] Misc
	extern EXPORT MoraleLevels __stdcall Player_MoraleLevel(_Player* handle)
	{
		return handle->MoraleLevel();
	}
	extern EXPORT int __stdcall Player_GetRLVCount(_Player* handle)
	{
		return handle->GetRLVCount();
	}
	// [Get] Indirect property lookups
	extern EXPORT int __stdcall Player_HasTechnology(_Player* handle, int techID)
	{
		return handle->HasTechnology(techID);
	}
	extern EXPORT int __stdcall Player_GetDefaultGroup(_Player* handle)
	{
		return handle->GetDefaultGroup().stubIndex;
	}
	// [Get] Player Strength  [Calculational]
	// Note: Unit Strengths are as follows:
	//	Spider/Scorpion	: 4
	//	Lynx			: Laser/Microwave: 5 Other: 6 ThorsHammer: 7
	//	Panther			: Laser/Microwave: 7 Other: 8 ThorsHammer: 9
	//	Tiger			: Laser/Microwave: 8 Other: 9 ThorsHammer: 10
	//	Guard Post		: Laser/Microwave: 4 Other: 5 ThorsHammer: 7
	//	Other			: 0  [Including Units in a Garage]
	extern EXPORT int __stdcall Player_GetPlayerStrength(_Player* handle, int x1, int y1, int x2, int y2)
	{
		MAP_RECT rect = MAP_RECT(x1, y1, x2, y2);
		return handle->GetPlayerStrength(rect);
	}
	extern EXPORT int __stdcall Player_GetTotalPlayerStrength(_Player* handle)
	{
		return handle->GetTotalPlayerStrength();
	}
	// [Get] Checks  [Prerequisite searching]
	extern EXPORT int __stdcall Player_CanAccumulateOre(_Player* handle)
	{
		return handle->canAccumulateOre();
	}
	extern EXPORT int __stdcall Player_CanAccumulateRareOre(_Player* handle)
	{
		return handle->canAccumulateRareOre();
	}
	extern EXPORT int __stdcall Player_CanBuildSpace(_Player* handle)
	{
		return handle->canBuildSpace();
	}
	extern EXPORT int __stdcall Player_CanBuildBuilding(_Player* handle)
	{
		return handle->canBuildBuilding();
	}
	extern EXPORT int __stdcall Player_CanBuildVehicle(_Player* handle, int bCheckCanBuildBuilding)
	{
		return handle->canBuildVehicle(bCheckCanBuildBuilding);
	}
	extern EXPORT int __stdcall Player_CanDoResearch(_Player* handle, int techID)
	{
		return handle->canDoResearch(techID);
	}
	extern EXPORT int __stdcall Player_HasVehicle(_Player* handle, map_id vehicleType, map_id cargoOrWeaponType)
	{
		return handle->hasVehicle(vehicleType, cargoOrWeaponType);
	}
	extern EXPORT int __stdcall Player_HasActiveCommand(_Player* handle)
	{
		return handle->hasActiveCommand();
	}
	// Reset cached check values
	extern EXPORT void __stdcall Player_ResetChecks(_Player* handle)
	{
		handle->resetChecks();
	}

	// [Set] Game Settings
	extern EXPORT void __stdcall Player_SetColorNumber(_Player* handle, int colorIndex)
	{
		handle->SetColorNumber(colorIndex);
	}
	// [Set] Population
	extern EXPORT void __stdcall Player_SetKids(_Player* handle, int numKids)
	{
		handle->SetKids(numKids);
	}
	extern EXPORT void __stdcall Player_SetWorkers(_Player* handle, int numWorkers)
	{
		handle->SetWorkers(numWorkers);
	}
	extern EXPORT void __stdcall Player_SetScientists(_Player* handle, int numScientists)
	{
		handle->SetScientists(numScientists);
	}
	// [Set] Resources
	extern EXPORT void __stdcall Player_SetOre(_Player* handle, int newCommonOre)
	{
		handle->SetOre(newCommonOre);
	}
	extern EXPORT void __stdcall Player_SetRareOre(_Player* handle, int newRareOre)
	{
		handle->SetRareOre(newRareOre);
	}
	extern EXPORT void __stdcall Player_SetFoodStored(_Player* handle, int newFoodStored)
	{
		handle->SetFoodStored(newFoodStored);
	}
	// [Set] Misc
	extern EXPORT void __stdcall Player_SetSolarSat(_Player* handle, int numSolarSatellites)
	{
		handle->SetSolarSat(numSolarSatellites);
	}
	// [Set] Indirect property setting
	extern EXPORT void __stdcall Player_SetTechLevel(_Player* handle, int techLevel)
	{
		handle->SetTechLevel(techLevel);
	}
	extern EXPORT void __stdcall Player_MarkResearchComplete(_Player* handle, int techID)
	{
		handle->MarkResearchComplete(techID);
	}
	extern EXPORT void __stdcall Player_SetDefaultGroup(_Player* handle, int newDefaultGroupIndex)
	{
		ScGroup newDefaultGroup;
		newDefaultGroup.stubIndex = newDefaultGroupIndex;

		handle->SetDefaultGroup(newDefaultGroup);
	}

	// [Method]
	extern EXPORT void __stdcall Player_GoEden(_Player* handle)
	{
	}
	extern EXPORT void __stdcall Player_GoPlymouth(_Player* handle)
	{
	}
	extern EXPORT void __stdcall Player_GoAI(_Player* handle)
	{
	}
	extern EXPORT void __stdcall Player_GoHuman(_Player* handle)
	{
	}
	extern EXPORT void __stdcall Player_AllyWith(_Player* handle, int playerNum)
	{
	}
	extern EXPORT void __stdcall Player_CaptureRLV(_Player* handle, int sourcePlayerNum)
	{
	}
	extern EXPORT void __stdcall Player_CenterViewOn(_Player* handle, int tileX, int tileY)
	{
	}

	//char checkValue[8];		// 0 = False, 1 = True, -1 = Not Set
};
