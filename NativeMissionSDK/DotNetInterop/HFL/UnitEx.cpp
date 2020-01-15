// UnitEx.h
// Extra / useful unit stuff
#include "stdafx_unmanaged.h"

#include <HFL/Source/HFL.h>

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif


extern "C"
{
	extern EXPORT void __stdcall UnitEx_DoAttack(int unitID, int tileX, int tileY)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.DoAttack(LOCATION(tileX, tileY));
	}

	extern EXPORT void __stdcall UnitEx_DoDeployMiner(int unitID, int tileX, int tileY)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.DoDeployMiner(LOCATION(tileX, tileY));
	}

	extern EXPORT void __stdcall UnitEx_DoDoze(int unitID, int xMin, int yMin, int xMax, int yMax)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.DoDoze(MAP_RECT(xMin,yMin, xMax,yMax));
	}

	extern EXPORT void __stdcall UnitEx_DoDock(int unitID, int tileX, int tileY)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.DoDock(LOCATION(tileX, tileY));
	}
	extern EXPORT void __stdcall UnitEx_DoDockAtGarage(int unitID, int tileX, int tileY)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.DoDockAtGarage(LOCATION(tileX, tileY));
	}

	extern EXPORT void __stdcall UnitEx_DoStandGround(int unitID, int tileX, int tileY)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.DoStandGround(LOCATION(tileX, tileY));
	}

	extern EXPORT void __stdcall UnitEx_DoBuildWall(int unitID, map_id wallType, int xMin, int yMin, int xMax, int yMax)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.DoBuildWall(wallType, MAP_RECT(xMin, yMin, xMax, yMax));
	}

	extern EXPORT void __stdcall UnitEx_DoRemoveWall(int unitID, int xMin, int yMin, int xMax, int yMax)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.DoRemoveWall(MAP_RECT(xMin, yMin, xMax, yMax));
	}

	extern EXPORT void __stdcall UnitEx_DoProduce(int unitID, map_id unitType, map_id cargoWeaponType)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.DoProduce(unitType, cargoWeaponType);
	}

	extern EXPORT void __stdcall UnitEx_DoTransferCargo(int unitID, int bay)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.DoTransferCargo(bay);
	}

	extern EXPORT void __stdcall UnitEx_DoLoadCargo(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.DoLoadCargo();
	}
	extern EXPORT void __stdcall UnitEx_DoUnloadCargo(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.DoUnloadCargo();
	}
	extern EXPORT void __stdcall UnitEx_DoDumpCargo(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.DoDumpCargo();
	}

	extern EXPORT void __stdcall UnitEx_DoResearch(int unitID, int techID, int numScientists)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.DoResearch(techID, numScientists);
	}
	extern EXPORT void __stdcall UnitEx_DoTrainScientists(int unitID, int numScientists)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.DoTrainScientists(numScientists);
	}

	extern EXPORT void __stdcall UnitEx_DoRepair(int unitID, int targetUnitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		UnitEx targetUnit;
		targetUnit.unitID = targetUnitID;

		unit.DoRepair(targetUnit);
	}
	extern EXPORT void __stdcall UnitEx_DoReprogram(int unitID, int targetUnitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		UnitEx targetUnit;
		targetUnit.unitID = targetUnitID;

		unit.DoReprogram(targetUnit);
	}
	extern EXPORT void __stdcall UnitEx_DoDismantle(int unitID, int targetUnitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		UnitEx targetUnit;
		targetUnit.unitID = targetUnitID;

		unit.DoDismantle(targetUnit);
	}

	extern EXPORT void __stdcall UnitEx_DoSalvage(int unitID, int xMin, int yMin, int xMax, int yMax, int targetGorfID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		UnitEx targetUnit;
		targetUnit.unitID = targetGorfID;

		unit.DoSalvage(MAP_RECT(xMin, yMin, xMax, yMax), targetUnit);
	}

	extern EXPORT void __stdcall UnitEx_DoGuard(int unitID, int targetUnitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		UnitEx targetUnit;
		targetUnit.unitID = targetUnitID;

		unit.DoGuard(targetUnit);
	}

	extern EXPORT void __stdcall UnitEx_DoPoof(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.DoPoof();
	}

	extern EXPORT CommandType __stdcall UnitEx_GetLastCommand(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetLastCommand();
	}
	extern EXPORT ActionType __stdcall UnitEx_GetCurAction(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetCurAction();
	}
	extern EXPORT int __stdcall UnitEx_CreatorID(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.CreatorID();
	}
	extern EXPORT int __stdcall UnitEx_GetTimeEMPed(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetTimeEMPed();
	}
	extern EXPORT int __stdcall UnitEx_IsStickyfoamed(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.IsStickyfoamed();
	}
	extern EXPORT int __stdcall UnitEx_IsESGed(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.IsESGed();
	}
	extern EXPORT int __stdcall UnitEx_GetDamage(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetDamage();
	}
	extern EXPORT int __stdcall UnitEx_GetCargoAmount(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetCargoAmount();
	}
	extern EXPORT Truck_Cargo __stdcall UnitEx_GetCargoType(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetCargoType();
	}
	extern EXPORT int __stdcall UnitEx_GetWorkersInTraining(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetWorkersInTraining();
	}
	extern EXPORT map_id __stdcall UnitEx_GetFactoryCargo(int unitID, int bay)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetFactoryCargo(bay);
	}
	extern EXPORT map_id __stdcall UnitEx_GetFactoryCargoWeapon(int unitID, int bay)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetFactoryCargoWeapon(bay);
	}
	extern EXPORT map_id __stdcall UnitEx_GetLaunchPadCargo(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetLaunchPadCargo();
	}
	extern EXPORT void __stdcall UnitEx_SetLaunchPadCargo(int unitID, map_id moduleType)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.SetLaunchPadCargo(moduleType);
	}
	extern EXPORT int __stdcall UnitEx_GetLights(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetLights();
	}
	extern EXPORT int __stdcall UnitEx_GetDoubleFireRate(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetDoubleFireRate();
	}
	extern EXPORT int __stdcall UnitEx_GetInvisible(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetInvisible();
	}
	extern EXPORT int __stdcall UnitEx_HasPower(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.HasPower();
	}
	extern EXPORT int __stdcall UnitEx_HasWorkers(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.HasWorkers();
	}
	extern EXPORT int __stdcall UnitEx_HasScientists(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.HasScientists();
	}
	extern EXPORT int __stdcall UnitEx_IsInfected(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.IsInfected();
	}

	extern EXPORT void __stdcall UnitEx_SetDoubleFireRate(int unitID, int boolOn)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.SetDoubleFireRate(boolOn);
	}
	extern EXPORT void __stdcall UnitEx_SetInvisible(int unitID, int boolOn)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.SetInvisible(boolOn);
	}

	extern EXPORT INT64 __stdcall UnitEx_GetDockLocation(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		LOCATION loc = unit.GetDockLocation();

		return loc.x | ((INT64)loc.y << 32);
	}

	// No need to export this. In caller, do new UnitInfo(unitEx.GetType()).
	//extern EXPORT UnitInfo* __stdcall UnitEx_GetUnitInfo(int unitID)
	//{
	//	UnitEx unit;
	//	unit.unitID = unitID;
		
	//	unit.GetUnitInfo();
	//}

	extern EXPORT void __stdcall UnitEx_SetAnimation(int unitID, int animIdx, int animDelay, int animStartDelay, int boolInvisible, int boolSkipDoDeath)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.SetAnimation(animIdx, animDelay, animStartDelay, boolInvisible, boolSkipDoDeath);
	}

	extern EXPORT int __stdcall UnitEx_GetNumTruckLoadsSoFar(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetNumTruckLoadsSoFar();
	}
	extern EXPORT int __stdcall UnitEx_GetBarYield(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetBarYield();
	}
	extern EXPORT int __stdcall UnitEx_GetVariant(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetNumTruckLoadsSoFar();
	}
	extern EXPORT int __stdcall UnitEx_GetOreType(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetOreType();
	}
	extern EXPORT int __stdcall UnitEx_GetSurveyedBy(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetSurveyedBy();
	}

	extern EXPORT int __stdcall UnitEx_GetLabCurrentTopic(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetLabCurrentTopic();
	}
	extern EXPORT int __stdcall UnitEx_GetLabScientistCount(int unitID)
	{
		UnitEx unit;
		unit.unitID = unitID;

		return unit.GetLabScientistCount();
	}
	extern EXPORT void __stdcall UnitEx_SetLabScientistCount(int unitID, int numScientists)
	{
		UnitEx unit;
		unit.unitID = unitID;

		unit.SetLabScientistCount(numScientists);
	}
}