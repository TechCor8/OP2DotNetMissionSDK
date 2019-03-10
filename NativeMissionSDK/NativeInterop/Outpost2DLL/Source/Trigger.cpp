#include "stdafx.h"

#include <Outpost2DLL/Outpost2DLL.h>	// Main Outpost 2 header to interface with the game
#include <fstream>

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif

// Note: This is the trigger class returned by all the trigger creation
//		 functions. It can be used to control an active trigger.
// Note: The trigger class is only a stub that refers to an internal
//		 class that handles the trigger. It is seldom necessary to use
//		 this class to control a trigger and it is usually destroyed
//		 shortly after it is returned from the trigger creation function.

extern "C"
{
	extern EXPORT void __stdcall Trigger_Destroy(int stubIndex)
	{
		Trigger trigger;
		trigger.stubIndex = stubIndex;

		trigger.Destroy();
	}

	extern EXPORT void __stdcall Trigger_Disable(int stubIndex)
	{
		Trigger trigger;
		trigger.stubIndex = stubIndex;

		trigger.Disable();
	}

	extern EXPORT void __stdcall Trigger_Enable(int stubIndex)
	{
		Trigger trigger;
		trigger.stubIndex = stubIndex;

		trigger.Enable();
	}

	extern EXPORT int __stdcall Trigger_HasFired(int stubIndex, int playerNum)	// Note: Do not pass -1 = PlayerAll
	{
		Trigger trigger;
		trigger.stubIndex = stubIndex;

		return trigger.HasFired(playerNum);
	}


	// Trigger creation functions
	// **************************

	// Victory/Failure condition triggers
	extern EXPORT int __stdcall Trigger_CreateVictoryCondition(int bEnabled, int bOneShot /*not used, set to 0*/, int victoryTrigger, const char* missionObjective)
	{
		// CreateVictoryCondition does not make a copy, and missionObjective gets released. Make a copy now.
		size_t len = strlen(missionObjective)+1;
		char* copy = new char[len];
		strcpy_s(copy, len, missionObjective);

		Trigger t;
		t.stubIndex = victoryTrigger;

		return CreateVictoryCondition(bEnabled, bOneShot, t, copy).stubIndex;
	}
	extern EXPORT int __stdcall Trigger_CreateFailureCondition(int bEnabled, int bOneShot /*not used, set to 0*/, int failureTrigger)
	{
		Trigger t;
		t.stubIndex = failureTrigger;

		return CreateFailureCondition(bEnabled, bOneShot, t, "").stubIndex;
	}

	// Typical Victory Triggers
	extern EXPORT int __stdcall Trigger_CreateOnePlayerLeftTrigger(int bEnabled, int bOneShot)			// Last One Standing (and later part of Land Rush)
	{
		return CreateOnePlayerLeftTrigger(bEnabled, bOneShot, "NoResponseToTrigger").stubIndex;
	}
	extern EXPORT int __stdcall Trigger_CreateEvacTrigger(int bEnabled, int bOneShot, int playerNum)	// Spacerace
	{
		return CreateEvacTrigger(bEnabled, bOneShot, playerNum, "NoResponseToTrigger").stubIndex;
	}
	extern EXPORT int __stdcall Trigger_CreateMidasTrigger(int bEnabled, int bOneShot, int time)		// Midas
	{
		return CreateMidasTrigger(bEnabled, bOneShot, time, "NoResponseToTrigger").stubIndex;
	}
	// Converting Land Rush to Last One Standing (when CC becomes active). Do not use PlayerAll.
	extern EXPORT int __stdcall Trigger_CreateOperationalTrigger(int bEnabled, int bOneShot, int playerNum, map_id buildingType, int refValue, compare_mode compareType)
	{
		return CreateOperationalTrigger(bEnabled, bOneShot, playerNum, buildingType, refValue, compareType, "NoResponseToTrigger").stubIndex;
	}
	
	// Research and Resource Count Triggers  [Note: Typically used to set what needs to be done by the end of a campaign mission]
	extern EXPORT int __stdcall Trigger_CreateResearchTrigger(int bEnabled, int bOneShot, int techID, int playerNum)
	{
		return CreateResearchTrigger(bEnabled, bOneShot, techID, playerNum, "NoResponseToTrigger").stubIndex;
	}
	extern EXPORT int __stdcall Trigger_CreateResourceTrigger(int bEnabled, int bOneShot, trig_res resourceType, int refAmount, int playerNum, compare_mode compareType)
	{
		return CreateResourceTrigger(bEnabled, bOneShot, resourceType, refAmount, playerNum, compareType, "NoResponseToTrigger").stubIndex;
	}
	extern EXPORT int __stdcall Trigger_CreateKitTrigger(int bEnabled, int bOneShot, int playerNum, map_id id, int refCount)
	{
		return CreateKitTrigger(bEnabled, bOneShot, playerNum, id, refCount, "NoResponseToTrigger").stubIndex;
	}
	extern EXPORT int __stdcall Trigger_CreateEscapeTrigger(int bEnabled, int bOneShot, int playerNum, int x, int y, int width, int height, int refValue, map_id unitType, int cargoType, int cargoAmount)
	{
		return CreateEscapeTrigger(bEnabled, bOneShot, playerNum, x, y, width, height, refValue, unitType, cargoType, cargoAmount, "NoResponseToTrigger").stubIndex;
	}
	extern EXPORT int __stdcall Trigger_CreateCountTrigger(int bEnabled, int bOneShot, int playerNum, map_id unitType, map_id cargoOrWeapon, int refCount, compare_mode compareType)
	{
		return CreateCountTrigger(bEnabled, bOneShot, playerNum, unitType, cargoOrWeapon, refCount, compareType, "NoResponseToTrigger").stubIndex;
	}
	// Unit Count Triggers  [Note: See also CreateCountTrigger]
	extern EXPORT int __stdcall Trigger_CreateVehicleCountTrigger(int bEnabled, int bOneShot, int playerNum, int refCount, compare_mode compareType)
	{
		return CreateVehicleCountTrigger(bEnabled, bOneShot, playerNum, refCount, compareType, "NoResponseToTrigger").stubIndex;
	}
	extern EXPORT int __stdcall Trigger_CreateBuildingCountTrigger(int bEnabled, int bOneShot, int playerNum, int refCount, compare_mode compareType)
	{
		return CreateBuildingCountTrigger(bEnabled, bOneShot, playerNum, refCount, compareType, "NoResponseToTrigger").stubIndex;
	}
	// Attack/Damage Triggers
	extern EXPORT int __stdcall Trigger_CreateAttackedTrigger(int bEnabled, int bOneShot, ScGroup* group)
	{
		return CreateAttackedTrigger(bEnabled, bOneShot, *group, "NoResponseToTrigger").stubIndex;
	}
	extern EXPORT int __stdcall Trigger_CreateDamagedTrigger(int bEnabled, int bOneShot, ScGroup* group, int damage)
	{
		return CreateDamagedTrigger(bEnabled, bOneShot, *group, damage, "NoResponseToTrigger").stubIndex;
	}
	// Time Triggers
	extern EXPORT int __stdcall Trigger_CreateTimeTrigger(int bEnabled, int bOneShot, int timeMin, int timeMax)
	{
		return CreateTimeTrigger(bEnabled, bOneShot, timeMin, timeMax, "NoResponseToTrigger").stubIndex;
	}
	extern EXPORT int __stdcall Trigger_CreateTimeTrigger2(int bEnabled, int bOneShot, int time)
	{
		return CreateTimeTrigger(bEnabled, bOneShot, time, "NoResponseToTrigger").stubIndex;
	}
	// Positional Triggers
	extern EXPORT int __stdcall Trigger_CreatePointTrigger(int bEnabled, int bOneShot, int playerNum, int x, int y)
	{
		return CreatePointTrigger(bEnabled, bOneShot, playerNum, x, y, "NoResponseToTrigger").stubIndex;
	}
	extern EXPORT int __stdcall Trigger_CreateRectTrigger(int bEnabled, int bOneShot, int playerNum, int x, int y, int width, int height)
	{
		return CreateRectTrigger(bEnabled, bOneShot, playerNum, x, y, width, height, "NoResponseToTrigger").stubIndex;
	}
	// Special Target Trigger/Data
	extern EXPORT int __stdcall Trigger_CreateSpecialTarget(int bEnabled, int bOneShot, Unit* targetUnit /* Lab */, map_id sourceUnitType /* mapScout */)
	{
		return CreateSpecialTarget(bEnabled, bOneShot, *targetUnit, sourceUnitType, "NoResponseToTrigger").stubIndex;
	}
	extern EXPORT void __stdcall Trigger_GetSpecialTargetData(Trigger* specialTargetTrigger, Unit* sourceUnit /* Scout */)
	{
		GetSpecialTargetData(*specialTargetTrigger, *sourceUnit);
	}

	// Set Trigger  [Note: Used to collect a number of other triggers into a single trigger output. Can be used for something like any 3 in a set of 5 objectives.]
	/*extern EXPORT Trigger* __stdcall Trigger_CreateSetTrigger(int bEnabled, int bOneShot, int totalTriggers, int neededTriggers, const char* triggerFunction)
	{
		return new Trigger(CreateSetTrigger(bEnabled, bOneShot, totalTriggers, neededTriggers, triggerFunction));
	}*/
}
