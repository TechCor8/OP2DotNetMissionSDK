#include "stdafx_unmanaged.h"

#include <Outpost2DLL/Outpost2DLL.h>	// Main Outpost 2 header to interface with the game

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif


extern "C"
{
	// Note: This file is used to define all the group related classes exported
	//		 from Outpost2.exe. It also contains the Pinwheel class (which is
	//		 like a group class in many ways) and the Trigger class (which
	//		 derives from the same base class as all the others).

	// Note: ScGroup is the main parent class of all other group classes.
	//		 Any functions defined here on this class are available to any
	//		 instance of the derived classes.
	// Note: Do not try to create an instance of this class. It was meant
	//		 simply as a base parent class from which other classes inherit
	//		 functions from. Creating an instance of this class serves little
	//		 (or no) purpose and may even crash the game.

	//extern EXPORT map_id __stdcall Unit_GetType(Unit* handle)
	extern EXPORT void __stdcall ScGroup_AddUnits(int stubIndex, UnitBlock* unitsToAdd)
	{
		ScGroup group;
		group.stubIndex = stubIndex;

		group.AddUnits(*unitsToAdd);
	}
	extern EXPORT void __stdcall ScGroup_ClearTargCount(int stubIndex)
	{
		ScGroup group;
		group.stubIndex = stubIndex;

		group.ClearTargCount();
	}
	extern EXPORT int __stdcall ScGroup_GetFirstOfType(int stubIndex, Unit* returnedUnit, UnitClassifactions unitType)	// ** Typo **
	{
		ScGroup group;
		group.stubIndex = stubIndex;

		return group.GetFirstOfType(*returnedUnit, unitType);
	}
	extern EXPORT int __stdcall ScGroup_GetFirstOfType2(int stubIndex, Unit* returnedUnit, map_id unitType, map_id cargoOrWeapon)
	{
		ScGroup group;
		group.stubIndex = stubIndex;

		return group.GetFirstOfType(*returnedUnit, unitType, cargoOrWeapon);
	}
	extern EXPORT int __stdcall ScGroup_HasBeenAttacked(int stubIndex)
	{
		ScGroup group;
		group.stubIndex = stubIndex;

		return group.HasBeenAttacked();
	}
	extern EXPORT void __stdcall ScGroup_RemoveUnit(int stubIndex, Unit* unitToRemove)
	{
		ScGroup group;
		group.stubIndex = stubIndex;

		group.RemoveUnit(*unitToRemove);
	}
	extern EXPORT void __stdcall ScGroup_SetDeleteWhenEmpty(int stubIndex, int bDelete)
	{
		ScGroup group;
		group.stubIndex = stubIndex;

		group.SetDeleteWhenEmpty(bDelete);
	}
	extern EXPORT void __stdcall ScGroup_SetLights(int stubIndex, int bOn)
	{
		ScGroup group;
		group.stubIndex = stubIndex;

		group.SetLights(bOn);
	}
	extern EXPORT void __stdcall ScGroup_SetTargCount(int stubIndex, UnitBlock* unitTypes)
	{
		ScGroup group;
		group.stubIndex = stubIndex;

		group.SetTargCount(*unitTypes);
	}
	extern EXPORT void __stdcall ScGroup_SetTargCount2(int stubIndex, map_id unitType, map_id weaponType, int targetCount)
	{
		ScGroup group;
		group.stubIndex = stubIndex;

		group.SetTargCount(unitType, weaponType, targetCount);
	}
	extern EXPORT void __stdcall ScGroup_TakeAllUnits(int stubIndex, int sourceGroup)
	{
		ScGroup group;
		group.stubIndex = stubIndex;

		ScGroup source;
		source.stubIndex = sourceGroup;

		group.TakeAllUnits(source);
	}
	extern EXPORT void __stdcall ScGroup_TakeUnit(int stubIndex, Unit* unitToAdd)
	{
		ScGroup group;
		group.stubIndex = stubIndex;

		group.TakeUnit(*unitToAdd);
	}
	extern EXPORT int __stdcall ScGroup_TotalUnitCount(int stubIndex)
	{
		ScGroup group;
		group.stubIndex = stubIndex;

		return group.TotalUnitCount();
	}
	extern EXPORT int __stdcall ScGroup_UnitCount(int stubIndex, UnitClassifactions unitType)	// ** Typo **
	{
		ScGroup group;
		group.stubIndex = stubIndex;

		return group.UnitCount(unitType);
	}



// Note: Do not try to create any of the following classes using only their
//		 constructors. Doing so will likely crash the game. Instead, create
//		 these classes by calling the group creation functions defined in
//		 Functions.h.


// Note: The BuildingGroup class is used to control Structure Factories,
//		 ConVecs, and Earthworkers to build/rebuild a base.

	extern EXPORT void __stdcall BuildingGroup_RecordBuilding(int stubIndex, int buildingX, int buildingY, map_id unitType, map_id cargoOrWeapon)
	{
		BuildingGroup group;
		group.stubIndex = stubIndex;

		group.RecordBuilding(LOCATION(buildingX, buildingY), unitType, cargoOrWeapon);
	}
	extern EXPORT void __stdcall BuildingGroup_RecordBuilding2(int stubIndex, int buildingX, int buildingY, map_id unitType, map_id cargoOrWeapon, int scGroup)
	{
		BuildingGroup group;
		group.stubIndex = stubIndex;

		ScGroup grp;
		grp.stubIndex = scGroup;

		group.RecordBuilding(LOCATION(buildingX, buildingY), unitType, cargoOrWeapon, grp);
	}
	extern EXPORT void __stdcall BuildingGroup_RecordTube(int stubIndex, int tubeX, int tubeY)
	{
		BuildingGroup group;
		group.stubIndex = stubIndex;

		group.RecordTube(LOCATION(tubeX, tubeY));
	}
	extern EXPORT void __stdcall BuildingGroup_RecordTubesTouching(int stubIndex, int startX, int startY)
	{
		BuildingGroup group;
		group.stubIndex = stubIndex;

		group.RecordTubesTouching(LOCATION(startX, startY));
	}
	extern EXPORT void __stdcall BuildingGroup_RecordUnitBlock(int stubIndex, UnitBlock* unitBlock)
	{
		BuildingGroup group;
		group.stubIndex = stubIndex;

		group.RecordUnitBlock(*unitBlock);
	}
	extern EXPORT void __stdcall BuildingGroup_RecordUnitBlock2(int stubIndex, UnitBlock* unitBlock, int scGroup)
	{
		BuildingGroup group;
		group.stubIndex = stubIndex;

		ScGroup grp;
		grp.stubIndex = scGroup;

		group.RecordUnitBlock(*unitBlock, grp);
	}
	extern EXPORT void __stdcall BuildingGroup_RecordVehReinforceGroup(int stubIndex, int targetGroup, int priority) // 0 = lowest priority, 0xFFFF = highest
	{
		BuildingGroup group;
		group.stubIndex = stubIndex;

		ScGroup grp;
		grp.stubIndex = targetGroup;

		group.RecordVehReinforceGroup(grp, priority);
	}
	extern EXPORT void __stdcall BuildingGroup_RecordWall(int stubIndex, int locationX, int locationY, map_id wallType)
	{
		BuildingGroup group;
		group.stubIndex = stubIndex;

		group.RecordWall(LOCATION(locationX, locationY), wallType);
	}
	extern EXPORT void __stdcall BuildingGroup_SetRect(int stubIndex, int minX, int minY, int maxX, int maxY)
	{
		BuildingGroup group;
		group.stubIndex = stubIndex;

		group.SetRect(MAP_RECT(minX, minY, maxX, maxY));
	}
	extern EXPORT void __stdcall BuildingGroup_UnRecordVehGroup(int stubIndex, int scGroup)
	{
		BuildingGroup group;
		group.stubIndex = stubIndex;

		ScGroup grp;
		grp.stubIndex = scGroup;

		group.UnRecordVehGroup(grp);
	}


// Note: The MiningGroup is a class used to setup mining routes with
//		 cargo trucks.

	extern EXPORT void __stdcall MiningGroup_Setup(int stubIndex, int mineX, int mineY, int smelterX, int smelterY, int smelterAreaMinX, int smelterAreaMinY, int smelterAreaMaxX, int smelterAreaMaxY)
	{
		MiningGroup group;
		group.stubIndex = stubIndex;

		group.Setup(LOCATION(mineX, mineY), LOCATION(smelterX, smelterY), MAP_RECT(smelterAreaMinX, smelterAreaMinY, smelterAreaMaxX, smelterAreaMaxY));
	}

	extern EXPORT void __stdcall MiningGroup_Setup2(int stubIndex, int mineX, int mineY, int smelterX, int smelterY, map_id mineType, map_id smelterType, int smelterAreaMinX, int smelterAreaMinY, int smelterAreaMaxX, int smelterAreaMaxY)
	{
		MiningGroup group;
		group.stubIndex = stubIndex;

		group.Setup(LOCATION(mineX, mineY), LOCATION(smelterX, smelterY), mineType, smelterType, MAP_RECT(smelterAreaMinX, smelterAreaMinY, smelterAreaMaxX, smelterAreaMaxY));
	}

	extern EXPORT void __stdcall MiningGroup_Setup3(int stubIndex, Unit* mine, Unit* smelter, int smelterAreaMinX, int smelterAreaMinY, int smelterAreaMaxX, int smelterAreaMaxY)
	{
		MiningGroup group;
		group.stubIndex = stubIndex;

		group.Setup(*mine, *smelter, MAP_RECT(smelterAreaMinX, smelterAreaMinY, smelterAreaMaxX, smelterAreaMaxY));
	}


// Note: The FightGroup class is used to control military units and is
//		 used to attack or defend a base. It can be used to cause an AI
//		 to produce units at a Vehicle Factory to attack with.

	extern EXPORT void __stdcall FightGroup_AddGuardedRect(int stubIndex, int guardedRectMinX, int guardedRectMinY, int guardedRectMaxX, int guardedRectMaxY)
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.AddGuardedRect(MAP_RECT(guardedRectMinX, guardedRectMinY, guardedRectMaxX, guardedRectMaxY));
	}
	extern EXPORT void __stdcall FightGroup_ClearCombineFire(int stubIndex)
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.ClearCombineFire();
	}
	extern EXPORT void __stdcall FightGroup_ClearGuarderdRects(int stubIndex)
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.ClearGuarderdRects();
	}
	extern EXPORT void __stdcall FightGroup_ClearPatrolMode(int stubIndex)
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.ClearPatrolMode();
	}
	extern EXPORT void __stdcall FightGroup_DoAttackEnemy(int stubIndex)
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.DoAttackEnemy();
	}
	extern EXPORT void __stdcall FightGroup_DoAttackUnit(int stubIndex)
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.DoAttackUnit();
	}
	extern EXPORT void __stdcall FightGroup_DoExitMap(int stubIndex)
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.DoExitMap();
	}
	extern EXPORT void __stdcall FightGroup_DoGuardGroup(int stubIndex)
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.DoGuardGroup();
	}
	extern EXPORT void __stdcall FightGroup_DoGuardRect(int stubIndex)
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.DoGuardRect();
	}
	extern EXPORT void __stdcall FightGroup_DoGuardUnit(int stubIndex)
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.DoGuardUnit();
	}
	extern EXPORT void __stdcall FightGroup_DoPatrolOnly(int stubIndex)                        //FightGroup units will no longer chase units that get close, but will still fire at enemies in sight during patrol
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.DoPatrolOnly();
	}
	extern EXPORT void __stdcall FightGroup_SetAttackType(int stubIndex, map_id attackType)		// ** Use in combination with DoAttackEnemy()
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.SetAttackType(attackType);
	}
	extern EXPORT void __stdcall FightGroup_SetCombineFire(int stubIndex)
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.SetCombineFire();
	}
	extern EXPORT void __stdcall FightGroup_SetFollowMode(int stubIndex, int followMode)
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.SetFollowMode(followMode);
	}
	extern EXPORT void __stdcall FightGroup_SetPatrolMode(int stubIndex, PatrolRoute* waypts)
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.SetPatrolMode(*waypts);
	}
	extern EXPORT void __stdcall FightGroup_SetRect(int stubIndex, int idleRectMinX, int idleRectMinY, int idleRectMaxX, int idleRectMaxY)
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.SetRect(MAP_RECT(idleRectMinX, idleRectMinY, idleRectMaxX, idleRectMaxY));
	}
	extern EXPORT void __stdcall FightGroup_SetTargetGroup(int stubIndex, int targetGroup)	// Use in combination with DoGuardGroup()
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		ScGroup target;
		target.stubIndex = targetGroup;

		group.SetTargetGroup(target);
	}
	extern EXPORT void __stdcall FightGroup_SetTargetUnit(int stubIndex, Unit* targetUnit)
	{
		FightGroup group;
		group.stubIndex = stubIndex;

		group.SetTargetUnit(*targetUnit);
	}


// Note: The Pinwheel class seems to be used for controlling waves
//		 of attacks.

/*class OP2 Pinwheel : public ScStub
{
public:
	Pinwheel();
	~Pinwheel();
	Pinwheel& operator = (const Pinwheel& pinwheel);

	void SendWaveNow(int);						// **
	void SetAttackComp(int, int, MrRec*);		// **
	void SetAttackFraction(int attackFraction);
	void SetContactDelay(int);					// **
	void SetGuardComp(int, int, MrRec*);		// **
	void SetNoRange(int, int);					// **
	void SetPoints(PWDef*);						// **
	void SetSapperComp(int, int, MrRec*);		// **
	void SetWavePeriod(int minTime, int maxTime);
};*/


	// Group creation functions
	// ************************

	extern EXPORT int __stdcall MiningGroup_Create(_Player* owner)
	{
		MiningGroup group = CreateMiningGroup(*owner);
		return group.stubIndex;
	}
	extern EXPORT int __stdcall BuildingGroup_Create(_Player* owner)
	{
		BuildingGroup group = CreateBuildingGroup(*owner);
		return group.stubIndex;
	}
	extern EXPORT int __stdcall FightGroup_Create(_Player* owner)
	{
		FightGroup group = CreateFightGroup(*owner);
		return group.stubIndex;
	}
	extern EXPORT int __stdcall Pinwheel_Create(_Player* owner)
	{
		Pinwheel group = CreatePinwheel(*owner);
		return group.stubIndex;
	}
}
