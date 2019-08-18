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

	// Common
	// [Get]
	extern EXPORT map_id __stdcall Unit_GetType(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		return unit.GetType();
	}

	extern EXPORT int __stdcall Unit_OwnerID(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		return  unit.OwnerID();
	}
	extern EXPORT int __stdcall Unit_IsBuilding(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		return unit.IsBuilding();
	}
	extern EXPORT int __stdcall Unit_IsVehicle(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		return unit.IsVehicle();
	}
	extern EXPORT int __stdcall Unit_IsBusy(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		return unit.IsBusy();
	}
	extern EXPORT int __stdcall Unit_IsLive(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		return unit.IsLive();
	}
	extern EXPORT int __stdcall Unit_IsEMPed(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		return unit.isEMPed();
	}
	extern EXPORT int __stdcall Unit_GetTileX(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		return unit.Location().x;
	}
	extern EXPORT int __stdcall Unit_GetTileY(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		return unit.Location().y;
	}

	// [Set]
	extern EXPORT void __stdcall Unit_SetDamage(int stubIndex, int damage)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.SetDamage(damage);
	}
	extern EXPORT void __stdcall Unit_SetId(int stubIndex, int newUnitId)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.SetId(newUnitId);
	}
	extern EXPORT void __stdcall Unit_SetOppFiredUpon(int stubIndex, int bTrue)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.SetOppFiredUpon(bTrue);
	}
	// [Method]
	extern EXPORT void __stdcall Unit_DoDeath(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.DoDeath();
	}
	extern EXPORT void __stdcall Unit_DoSelfDestruct(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.DoSelfDestruct();
	}
	extern EXPORT void __stdcall Unit_DoTransfer(int stubIndex, int destPlayerNum)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.DoTransfer(destPlayerNum);
	}

	// Combat Units
	extern EXPORT map_id __stdcall Unit_GetWeapon(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		return unit.GetWeapon();
	}
	extern EXPORT void __stdcall Unit_SetWeapon(int stubIndex, map_id weaponType)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.SetWeapon(weaponType);
	}
	extern EXPORT void __stdcall Unit_DoAttack(int stubIndex, int targetUnit)
	{
		Unit unit;
		unit.unitID = stubIndex;

		Unit tUnit;
		tUnit.unitID = targetUnit;

		unit.DoAttack(tUnit);
	}

	// Vehicles
	extern EXPORT void __stdcall Unit_DoSetLights(int stubIndex, int boolOn)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.DoSetLights(boolOn);
	}
	extern EXPORT void __stdcall Unit_DoMove(int stubIndex, int tileX, int tileY)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.DoMove(LOCATION(tileX, tileY));
	}
	// Specific Vehicle
	extern EXPORT map_id __stdcall Unit_GetCargo(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		return unit.GetCargo();
	}
	extern EXPORT void __stdcall Unit_DoBuild(int stubIndex, map_id buildingType, int tileX, int tileY)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.DoBuild(buildingType, LOCATION(tileX, tileY));
	}
	extern EXPORT void __stdcall Unit_SetCargo(int stubIndex, map_id cargoType, map_id weaponType)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.SetCargo(cargoType, weaponType);
	}
	extern EXPORT void __stdcall Unit_SetTruckCargo(int stubIndex, Truck_Cargo cargoType, int amount)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.SetTruckCargo(cargoType, amount);
	}

	// Buildings
	extern EXPORT void __stdcall Unit_DoIdle(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.DoIdle();
	}
	extern EXPORT void __stdcall Unit_DoUnIdle(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.DoUnIdle();
	}
	extern EXPORT void __stdcall Unit_DoStop(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.DoStop();
	}
	extern EXPORT void __stdcall Unit_DoInfect(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.DoInfect();
	}
	// Specific Building
	extern EXPORT map_id __stdcall Unit_GetObjectOnPad(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		return unit.GetObjectOnPad();
	}
	extern EXPORT void __stdcall Unit_DoLaunch(int stubIndex, int destPixelX, int destPixelY, int bForceEnable)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.DoLaunch(destPixelX, destPixelY, bForceEnable);
	}
	extern EXPORT void __stdcall Unit_PutInGarage(int stubIndex, int bayIndex, int tileX, int tileY)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.PutInGarage(bayIndex, tileX, tileY);
	}
	extern EXPORT int __stdcall Unit_HasOccupiedBay(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		return unit.HasOccupiedBay();
	}
	extern EXPORT void __stdcall Unit_SetFactoryCargo(int stubIndex, int bay, map_id unitType, map_id cargoOrWeaponType)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.SetFactoryCargo(bay, unitType, cargoOrWeaponType);
	}
	extern EXPORT void __stdcall Unit_DoDevelop(int stubIndex, map_id itemToProduce)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.DoDevelop(itemToProduce);
	}
	extern EXPORT void __stdcall Unit_ClearSpecialTarget(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		unit.ClearSpecialTarget();
	}

	// Wreckage
	extern EXPORT int __stdcall Unit_IsDiscovered(int stubIndex)
	{
		Unit unit;
		unit.unitID = stubIndex;

		return unit.isDiscovered();
	}

//protected: void DoSimpleCommand(int commandPacketType);
//private: char* StoreSelf(CommandPacket& commandPacket) const;

//public:	// Why not? :)
//	int unitID;
};
