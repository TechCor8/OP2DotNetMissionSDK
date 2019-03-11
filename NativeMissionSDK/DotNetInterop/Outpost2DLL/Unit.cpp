#include "stdafx_unmanaged.h"

#include <Outpost2DLL/Outpost2DLL.h>	// Main Outpost 2 header to interface with the game

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif


extern "C"
{
// Note: This file is used to define the Unit class. Use this class to
//		 manipulate all the units in the game.
// Note: The Unit class is also used in conjunction with the enumerator
//		 classes used to find units and traverse lists of units, one unit
//		 at a time. See Enumerator.h for details.

// Note: This class controls Units and provides info on them. This class can
//		 be used to set cargo in ConVecs, Cargo Trucks, or factory bays.
//		 It can also be used to move units around the map and perform simple
//		 operations such as self destruct and headlight control.

	extern EXPORT Unit* __stdcall Unit_Create()
	{
		return new Unit();
	}

	extern EXPORT void __stdcall Unit_Release(Unit* handle)
	{
		delete handle;
	}

	// Common
	// [Get]
	extern EXPORT map_id __stdcall Unit_GetType(Unit* handle)
	{
		return handle->GetType();
	}

	extern EXPORT int __stdcall Unit_OwnerID(Unit* handle)
	{
		return  handle->OwnerID();
	}
	extern EXPORT int __stdcall Unit_IsBuilding(Unit* handle)
	{
		return handle->IsBuilding();
	}
	extern EXPORT int __stdcall Unit_IsVehicle(Unit* handle)
	{
		return handle->IsVehicle();
	}
	extern EXPORT int __stdcall Unit_IsBusy(Unit* handle)
	{
		return handle->IsBusy();
	}
	extern EXPORT int __stdcall Unit_IsLive(Unit* handle)
	{
		return handle->IsLive();
	}
	extern EXPORT int __stdcall Unit_IsEMPed(Unit* handle)
	{
		return handle->isEMPed();
	}
	extern EXPORT int __stdcall Unit_GetTileX(Unit* handle)
	{
		return handle->Location().x;
	}
	extern EXPORT int __stdcall Unit_GetTileY(Unit* handle)
	{
		return handle->Location().y;
	}

	// [Set]
	extern EXPORT void __stdcall Unit_SetDamage(Unit* handle, int damage)
	{
		handle->SetDamage(damage);
	}
	extern EXPORT void __stdcall Unit_SetId(Unit* handle, int newUnitId)
	{
		handle->SetId(newUnitId);
	}
	extern EXPORT void __stdcall Unit_SetOppFiredUpon(Unit* handle, int bTrue)
	{
		handle->SetOppFiredUpon(bTrue);
	}
	// [Method]
	extern EXPORT void __stdcall Unit_DoDeath(Unit* handle)
	{
		handle->DoDeath();
	}
	extern EXPORT void __stdcall Unit_DoSelfDestruct(Unit* handle)
	{
		handle->DoSelfDestruct();
	}
	extern EXPORT void __stdcall Unit_DoTransfer(Unit* handle, int destPlayerNum)
	{
		handle->DoTransfer(destPlayerNum);
	}

	// Combat Units
	extern EXPORT map_id __stdcall Unit_GetWeapon(Unit* handle)
	{
		return handle->GetWeapon();
	}
	extern EXPORT void __stdcall Unit_SetWeapon(Unit* handle, map_id weaponType)
	{
		handle->SetWeapon(weaponType);
	}
	extern EXPORT void __stdcall Unit_DoAttack(Unit* handle, Unit targetUnit)
	{
		handle->DoAttack(targetUnit);
	}

	// Vehicles
	extern EXPORT void __stdcall Unit_DoSetLights(Unit* handle, int boolOn)
	{
		handle->DoSetLights(boolOn);
	}
	extern EXPORT void __stdcall Unit_DoMove(Unit* handle, int tileX, int tileY)
	{
		handle->DoMove(LOCATION(tileX, tileY));
	}
	// Specific Vehicle
	extern EXPORT map_id __stdcall Unit_GetCargo(Unit* handle)
	{
		return handle->GetCargo();
	}
	extern EXPORT void __stdcall Unit_DoBuild(Unit* handle, map_id buildingType, int tileX, int tileY)
	{
		handle->DoBuild(buildingType, LOCATION(tileX, tileY));
	}
	extern EXPORT void __stdcall Unit_SetCargo(Unit* handle, map_id cargoType, map_id weaponType)
	{
		handle->SetCargo(cargoType, weaponType);
	}
	extern EXPORT void __stdcall Unit_SetTruckCargo(Unit* handle, Truck_Cargo cargoType, int amount)
	{
		handle->SetTruckCargo(cargoType, amount);
	}

	// Buildings
	extern EXPORT void __stdcall Unit_DoIdle(Unit* handle)
	{
		handle->DoIdle();
	}
	extern EXPORT void __stdcall Unit_DoUnIdle(Unit* handle)
	{
		handle->DoUnIdle();
	}
	extern EXPORT void __stdcall Unit_DoStop(Unit* handle)
	{
		handle->DoStop();
	}
	extern EXPORT void __stdcall Unit_DoInfect(Unit* handle)
	{
		handle->DoInfect();
	}
	// Specific Building
	extern EXPORT map_id __stdcall Unit_GetObjectOnPad(Unit* handle)
	{
		return handle->GetObjectOnPad();
	}
	extern EXPORT void __stdcall Unit_DoLaunch(Unit* handle, int destPixelX, int destPixelY, int bForceEnable)
	{
		handle->DoLaunch(destPixelX, destPixelY, bForceEnable);
	}
	extern EXPORT void __stdcall Unit_PutInGarage(Unit* handle, int bayIndex, int tileX, int tileY)
	{
		handle->PutInGarage(bayIndex, tileX, tileY);
	}
	extern EXPORT int __stdcall Unit_HasOccupiedBay(Unit* handle)
	{
		return handle->HasOccupiedBay();
	}
	extern EXPORT void __stdcall Unit_SetFactoryCargo(Unit* handle, int bay, map_id unitType, map_id cargoOrWeaponType)
	{
		handle->SetFactoryCargo(bay, unitType, cargoOrWeaponType);
	}
	extern EXPORT void __stdcall Unit_DoDevelop(Unit* handle, map_id itemToProduce)
	{
		handle->DoDevelop(itemToProduce);
	}
	extern EXPORT void __stdcall Unit_ClearSpecialTarget(Unit* handle)
	{
		handle->ClearSpecialTarget();
	}

	// Wreckage
	extern EXPORT int __stdcall Unit_isDiscovered(Unit* handle)
	{
		return handle->isDiscovered();
	}

//protected: void DoSimpleCommand(int commandPacketType);
//private: char* StoreSelf(CommandPacket& commandPacket) const;

//public:	// Why not? :)
//	int unitID;
};
