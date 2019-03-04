#include "stdafx.h"

#include <Outpost2DLL/Outpost2DLL.h>	// Main Outpost 2 header to interface with the game

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif

// Note: This is the trigger class returned by all the trigger creation
//		 functions. It can be used to control an active trigger.
// Note: The trigger class is only a stub that refers to an internal
//		 class that handles the trigger. It is seldom necessary to use
//		 this class to control a trigger and it is usually destroyed
//		 shortly after it is returned from the trigger creation function.

/*class OP2 Trigger : public ScStub
{
public:
	Trigger();
	~Trigger();	// {};
	Trigger& operator = (const Trigger& trigger);

	void Disable();
	void Enable();
	int HasFired(int playerNum);	// Note: Do not pass -1 = PlayerAll
};*/

extern "C"
{
	// Trigger creation functions
	// **************************

	// Victory/Failure condition triggers
	extern EXPORT Trigger* __stdcall Trigger_CreateVictoryCondition(int bEnabled, int bOneShot /*not used, set to 0*/, Trigger* victoryTrigger, const char* missionObjective)
	{
		return new Trigger(CreateVictoryCondition(bEnabled, bOneShot, *victoryTrigger, missionObjective));
	}
	extern EXPORT Trigger* __stdcall Trigger_CreateFailureCondition(int bEnabled, int bOneShot /*not used, set to 0*/, Trigger* failureTrigger, const char* failureCondition /*not used, set to ""*/)
	{
		return new Trigger(CreateFailureCondition(bEnabled, bOneShot, *failureTrigger, failureCondition));
	}

	// Typical Victory Triggers
	extern EXPORT Trigger* __stdcall Trigger_CreateOnePlayerLeftTrigger(int bEnabled, int bOneShot, const char* triggerFunction)			// Last One Standing (and later part of Land Rush)
	{
		return new Trigger(CreateOnePlayerLeftTrigger(bEnabled, bOneShot, triggerFunction));
	}
	extern EXPORT Trigger* __stdcall Trigger_CreateEvacTrigger(int bEnabled, int bOneShot, int playerNum, const char* triggerFunction)	// Spacerace
	{
		return new Trigger(CreateEvacTrigger(bEnabled, bOneShot, playerNum, triggerFunction));
	}
	extern EXPORT Trigger* __stdcall Trigger_CreateMidasTrigger(int bEnabled, int bOneShot, int time, const char* triggerFunction)		// Midas
	{
		return new Trigger(CreateMidasTrigger(bEnabled, bOneShot, time, triggerFunction));
	}
	// Converting Land Rush to Last One Standing (when CC becomes active). Do not use PlayerAll.
	extern EXPORT Trigger* __stdcall Trigger_CreateOperationalTrigger(int bEnabled, int bOneShot, int playerNum, map_id buildingType, int refValue, compare_mode compareType, const char* triggerFunction)
	{
		return new Trigger(CreateOperationalTrigger(bEnabled, bOneShot, playerNum, buildingType, refValue, compareType, triggerFunction));
	}
	
	// Research and Resource Count Triggers  [Note: Typically used to set what needs to be done by the end of a campaign mission]
	extern EXPORT Trigger* __stdcall Trigger_CreateResearchTrigger(int bEnabled, int bOneShot, int techID, int playerNum, const char* triggerFunction)
	{
		return new Trigger(CreateResearchTrigger(bEnabled, bOneShot, techID, playerNum, triggerFunction));
	}
	extern EXPORT Trigger* __stdcall Trigger_CreateResourceTrigger(int bEnabled, int bOneShot, trig_res resourceType, int refAmount, int playerNum, compare_mode compareType, const char* triggerFunction)
	{
		return new Trigger(CreateResourceTrigger(bEnabled, bOneShot, resourceType, refAmount, playerNum, compareType, triggerFunction));
	}
	extern EXPORT Trigger* __stdcall Trigger_CreateKitTrigger(int bEnabled, int bOneShot, int playerNum, map_id id, int refCount, const char* triggerFunction)
	{
		return new Trigger(CreateKitTrigger(bEnabled, bOneShot, playerNum, id, refCount, triggerFunction));
	}
	extern EXPORT Trigger* __stdcall Trigger_CreateEscapeTrigger(int bEnabled, int bOneShot, int playerNum, int x, int y, int width, int height, int refValue, map_id unitType, int cargoType, int cargoAmount, const char* triggerFunction)
	{
		return new Trigger(CreateEscapeTrigger(bEnabled, bOneShot, playerNum, x, y, width, height, refValue, unitType, cargoType, cargoAmount, triggerFunction));
	}
	extern EXPORT Trigger* __stdcall Trigger_CreateCountTrigger(int bEnabled, int bOneShot, int playerNum, map_id unitType, map_id cargoOrWeapon, int refCount, compare_mode compareType, const char* triggerFunction)
	{
		return new Trigger(CreateCountTrigger(bEnabled, bOneShot, playerNum, unitType, cargoOrWeapon, refCount, compareType, triggerFunction));
	}
	// Unit Count Triggers  [Note: See also CreateCountTrigger]
	extern EXPORT Trigger* __stdcall Trigger_CreateVehicleCountTrigger(int bEnabled, int bOneShot, int playerNum, int refCount, compare_mode compareType, const char* triggerFunction)
	{
		return new Trigger(CreateVehicleCountTrigger(bEnabled, bOneShot, playerNum, refCount, compareType, triggerFunction));
	}
	extern EXPORT Trigger* __stdcall Trigger_CreateBuildingCountTrigger(int bEnabled, int bOneShot, int playerNum, int refCount, compare_mode compareType, const char* triggerFunction)
	{
		return new Trigger(CreateBuildingCountTrigger(bEnabled, bOneShot, playerNum, refCount, compareType, triggerFunction));
	}
	// Attack/Damage Triggers
	extern EXPORT Trigger* __stdcall Trigger_CreateAttackedTrigger(int bEnabled, int bOneShot, ScGroup* group, const char* triggerFunction)
	{
		return new Trigger(CreateAttackedTrigger(bEnabled, bOneShot, *group, triggerFunction));
	}
	extern EXPORT Trigger* __stdcall Trigger_CreateDamagedTrigger(int bEnabled, int bOneShot, ScGroup* group, int damage, const char* triggerFunction)
	{
		return new Trigger(CreateDamagedTrigger(bEnabled, bOneShot, *group, damage, triggerFunction));
	}
	// Time Triggers
	extern EXPORT Trigger* __stdcall Trigger_CreateTimeTrigger(int bEnabled, int bOneShot, int timeMin, int timeMax, const char* triggerFunction)
	{
		return new Trigger(CreateTimeTrigger(bEnabled, bOneShot, timeMin, timeMax, triggerFunction));
	}
	extern EXPORT Trigger* __stdcall Trigger_CreateTimeTrigger2(int bEnabled, int bOneShot, int time, const char* triggerFunction)
	{
		return new Trigger(CreateTimeTrigger(bEnabled, bOneShot, time, triggerFunction));
	}
	// Positional Triggers
	extern EXPORT Trigger* __stdcall Trigger_CreatePointTrigger(int bEnabled, int bOneShot, int playerNum, int x, int y, const char* triggerFunction)
	{
		return new Trigger(CreatePointTrigger(bEnabled, bOneShot, playerNum, x, y, triggerFunction));
	}
	extern EXPORT Trigger* __stdcall Trigger_CreateRectTrigger(int bEnabled, int bOneShot, int playerNum, int x, int y, int width, int height, const char* triggerFunction)
	{
		return new Trigger(CreateRectTrigger(bEnabled, bOneShot, playerNum, x, y, width, height, triggerFunction));
	}
	// Special Target Trigger/Data
	extern EXPORT Trigger* __stdcall Trigger_CreateSpecialTarget(int bEnabled, int bOneShot, Unit* targetUnit /* Lab */, map_id sourceUnitType /* mapScout */, const char* triggerFunction)
	{
		return new Trigger(CreateSpecialTarget(bEnabled, bOneShot, *targetUnit, sourceUnitType, triggerFunction));
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
