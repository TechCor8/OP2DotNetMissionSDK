// UnitInfo.h
// Classes for modifying UnitInfo structures
#include "stdafx_unmanaged.h"

#include <HFL/Source/HFL.h>

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif


extern "C"
{
	extern EXPORT int __stdcall UnitInfo_IsValid(map_id unitType)
	{
		return UnitInfo(unitType).IsValid();
	}

	extern EXPORT int __stdcall UnitInfo_GetHitPoints(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetHitPoints(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetHitPoints(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetHitPoints(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetRepairAmount(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetRepairAmt(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetRepairAmount(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetRepairAmt(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetArmor(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetArmor(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetArmor(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetArmor(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetOreCost(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetOreCost(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetOreCost(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetOreCost(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetRareOreCost(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetRareOreCost(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetRareOreCost(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetRareOreCost(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetBuildTime(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetBuildTime(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetBuildTime(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetBuildTime(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetSightRange(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetSightRange(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetSightRange(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetSightRange(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetWeaponRange(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetWeaponRange(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetWeaponRange(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetWeaponRange(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetPowerRequired(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetPowerRequired(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetPowerRequired(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetPowerRequired(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetMovePoints(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetMovePoints(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetMovePoints(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetMovePoints(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetTurretTurnRate(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetTurretTurnRate(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetTurretTurnRate(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetTurretTurnRate(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetConcussionDamage(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetConcussionDamage(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetConcussionDamage(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetConcussionDamage(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetWorkersRequired(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetWorkersRequired(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetWorkersRequired(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetWorkersRequired(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetTurnRate(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetTurnRate(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetTurnRate(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetTurnRate(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetPenetrationDamage(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetPenetrationDamage(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetPenetrationDamage(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetPenetrationDamage(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetScientistsRequired(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetScientistsRequired(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetScientistsRequired(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetScientistsRequired(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetProductionRate(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetProductionRate(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetProductionRate(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetProductionRate(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetReloadTime(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetReloadTime(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetReloadTime(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetReloadTime(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetStorageCapacity(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetStorageCapacity(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetStorageCapacity(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetStorageCapacity(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetWeaponSightRange(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetWeaponSightRange(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetWeaponSightRange(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetWeaponSightRange(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetProductionCapacity(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetProductionCapacity(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetProductionCapacity(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetProductionCapacity(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetNumStorageBays(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetNumStorageBays(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetNumStorageBays(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetNumStorageBays(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetCargoCapacity(map_id unitType, int player)
	{
		return UnitInfo(unitType).GetCargoCapacity(player);
	}
	extern EXPORT void __stdcall UnitInfo_SetCargoCapacity(map_id unitType, int player, int value)
	{
		UnitInfo(unitType).SetCargoCapacity(player, value);
	}

	extern EXPORT int __stdcall UnitInfo_GetResearchTopic(map_id unitType)
	{
		return UnitInfo(unitType).GetResearchTopic();
	}
	extern EXPORT void __stdcall UnitInfo_SetResearchTopic(map_id unitType, int techID)
	{
		UnitInfo(unitType).SetResearchTopic(techID);
	}

	extern EXPORT TrackType __stdcall UnitInfo_GetTrackType(map_id unitType)
	{
		return UnitInfo(unitType).GetTrackType();
	}
	extern EXPORT void __stdcall UnitInfo_SetTrackType(map_id unitType, TrackType type)
	{
		UnitInfo(unitType).SetTrackType(type);
	}

	extern EXPORT int __stdcall UnitInfo_GetOwnerFlags(map_id unitType)
	{
		return UnitInfo(unitType).GetOwnerFlags();
	}
	extern EXPORT void __stdcall UnitInfo_SetOwnerFlags(map_id unitType, int flags)
	{
		UnitInfo(unitType).SetOwnerFlags(flags);
	}

	extern EXPORT char* __stdcall UnitInfo_GetUnitName(map_id unitType)
	{
		return UnitInfo(unitType).GetUnitName();
	}
	extern EXPORT void __stdcall UnitInfo_SetUnitName(map_id unitType, const char* newName)
	{
		// C# will delete string parameters, make a copy
		size_t len = strlen(newName) + 1;
		char* copy = new char[len];
		strcpy_s(copy, len, newName);

		return UnitInfo(unitType).SetUnitName(copy);
	}

	extern EXPORT char* __stdcall UnitInfo_GetProduceListName(map_id unitType)
	{
		return UnitInfo(unitType).GetProduceListName();
	}
	extern EXPORT void __stdcall UnitInfo_SetProduceListName(map_id unitType, const char* newName)
	{
		// C# will delete string parameters, make a copy
		size_t len = strlen(newName) + 1;
		char* copy = new char[len];
		strcpy_s(copy, len, newName);

		return UnitInfo(unitType).SetProduceListName(copy);
	}

	extern EXPORT int __stdcall UnitInfo_GetXSize(map_id unitType)
	{
		return UnitInfo(unitType).GetXSize();
	}
	extern EXPORT void __stdcall UnitInfo_SetXSize(map_id unitType, int value)
	{
		UnitInfo(unitType).SetXSize(value);
	}

	extern EXPORT int __stdcall UnitInfo_GetDamageRadius(map_id unitType)
	{
		return UnitInfo(unitType).GetDamageRadius();
	}
	extern EXPORT void __stdcall UnitInfo_SetDamageRadius(map_id unitType, int value)
	{
		UnitInfo(unitType).SetDamageRadius(value);
	}

	extern EXPORT int __stdcall UnitInfo_GetVehicleFlags(map_id unitType)
	{
		return UnitInfo(unitType).GetVehicleFlags();
	}
	extern EXPORT void __stdcall UnitInfo_SetVehicleFlags(map_id unitType, int flags)
	{
		UnitInfo(unitType).SetVehicleFlags(flags);
	}

	extern EXPORT int __stdcall UnitInfo_GetYSize(map_id unitType)
	{
		return UnitInfo(unitType).GetYSize();
	}
	extern EXPORT void __stdcall UnitInfo_SetYSize(map_id unitType, int value)
	{
		UnitInfo(unitType).SetYSize(value);
	}

	extern EXPORT int __stdcall UnitInfo_GetPixelsSkippedWhenFiring(map_id unitType)
	{
		return UnitInfo(unitType).GetPixelsSkippedWhenFiring();
	}
	extern EXPORT void __stdcall UnitInfo_SetPixelsSkippedWhenFiring(map_id unitType, int value)
	{
		UnitInfo(unitType).SetPixelsSkippedWhenFiring(value);
	}

	extern EXPORT int __stdcall UnitInfo_GetBuildingFlags(map_id unitType)
	{
		return UnitInfo(unitType).GetBuildingFlags();
	}
	extern EXPORT void __stdcall UnitInfo_SetBuildingFlags(map_id unitType, int flags)
	{
		UnitInfo(unitType).SetBuildingFlags(flags);
	}

	extern EXPORT int __stdcall UnitInfo_GetExplosionSize(map_id unitType)
	{
		return UnitInfo(unitType).GetExplosionSize();
	}
	extern EXPORT void __stdcall UnitInfo_SetExplosionSize(map_id unitType, int value)
	{
		UnitInfo(unitType).SetExplosionSize(value);
	}

	extern EXPORT int __stdcall UnitInfo_GetResourcePriority(map_id unitType)
	{
		return UnitInfo(unitType).GetResourcePriority();
	}
	extern EXPORT void __stdcall UnitInfo_SetResourcePriority(map_id unitType, int value)
	{
		UnitInfo(unitType).SetResourcePriority(value);
	}

	extern EXPORT int __stdcall UnitInfo_GetRareRubble(map_id unitType)
	{
		return UnitInfo(unitType).GetRareRubble();
	}
	extern EXPORT void __stdcall UnitInfo_SetRareRubble(map_id unitType, int value)
	{
		UnitInfo(unitType).SetRareRubble(value);
	}

	extern EXPORT int __stdcall UnitInfo_GetRubble(map_id unitType)
	{
		return UnitInfo(unitType).GetRubble();
	}
	extern EXPORT void __stdcall UnitInfo_SetRubble(map_id unitType, int value)
	{
		UnitInfo(unitType).SetRubble(value);
	}

	extern EXPORT int __stdcall UnitInfo_GetEdenDockPos(map_id unitType)
	{
		return UnitInfo(unitType).GetEdenDockPos();
	}
	extern EXPORT void __stdcall UnitInfo_SetEdenDockPos(map_id unitType, int value)
	{
		UnitInfo(unitType).SetEdenDockPos(value);
	}

	extern EXPORT int __stdcall UnitInfo_GetPlymDockPos(map_id unitType)
	{
		return UnitInfo(unitType).GetPlymDockPos();
	}
	extern EXPORT void __stdcall UnitInfo_SetPlymDockPos(map_id unitType, int value)
	{
		UnitInfo(unitType).SetPlymDockPos(value);
	}

	extern EXPORT char* __stdcall UnitInfo_GetCodeName(map_id unitType)
	{
		return UnitInfo(unitType).GetCodeName();
	}
	extern EXPORT map_id __stdcall UnitInfo_GetMapID(map_id unitType)
	{
		return UnitInfo(unitType).GetMapID();
	}

	extern EXPORT int __stdcall UnitInfo_CreateUnit(map_id unitType, int tileX, int tileY, int unitID)
	{
		UnitInfo info(unitType);
		
		Unit unit = info.CreateUnit(LOCATION(tileX, tileY), unitID);

		return unit.unitID;
	}
}
