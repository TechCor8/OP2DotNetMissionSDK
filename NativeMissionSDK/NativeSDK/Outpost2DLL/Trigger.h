#pragma once
#ifndef OP2
#define OP2 __declspec(dllimport)
#endif

#include "ScStub.h"

// External type names
enum map_id;
enum compare_mode;
enum trig_res;
class Unit;
class ScGroup;

// Note: This is the trigger class returned by all the trigger creation
//		 functions. It can be used to control an active trigger.
// Note: The trigger class is only a stub that refers to an internal
//		 class that handles the trigger. It is seldom necessary to use
//		 this class to control a trigger and it is usually destroyed
//		 shortly after it is returned from the trigger creation function.

class OP2 Trigger : public ScStub
{
public:
	Trigger();
	~Trigger();	// {};
	Trigger& operator = (const Trigger& trigger);

	void Disable();
	void Enable();
	int HasFired(int playerNum);	// Note: Do not pass -1 = PlayerAll
};


// Trigger creation functions
// **************************

// Victory/Failure condition triggers
OP2 Trigger __fastcall CreateVictoryCondition(int bEnabled, int bOneShot /*not used, set to 0*/, Trigger& victoryTrigger, const char* missionObjective);
OP2 Trigger __fastcall CreateFailureCondition(int bEnabled, int bOneShot /*not used, set to 0*/, Trigger& failureTrigger, const char* failureCondition /*not used, set to ""*/);

// Typical Victory Triggers
OP2 Trigger __fastcall CreateOnePlayerLeftTrigger(int bEnabled, int bOneShot, const char* triggerFunction);			// Last One Standing (and later part of Land Rush)
OP2 Trigger __fastcall CreateEvacTrigger(int bEnabled, int bOneShot, int playerNum, const char* triggerFunction);	// Spacerace
OP2 Trigger __fastcall CreateMidasTrigger(int bEnabled, int bOneShot, int time, const char* triggerFunction);		// Midas
OP2 Trigger __fastcall CreateOperationalTrigger(int bEnabled, int bOneShot, int playerNum, map_id buildingType, int refValue, compare_mode compareType, const char* triggerFunction);	// Converting Land Rush to Last One Standing (when CC becomes active). Do not use PlayerAll.
// Research and Resource Count Triggers  [Note: Typically used to set what needs to be done by the end of a campaign mission]
OP2 Trigger __fastcall CreateResearchTrigger(int bEnabled, int bOneShot, int techID, int playerNum, const char* triggerFunction);
OP2 Trigger __fastcall CreateResourceTrigger(int bEnabled, int bOneShot, trig_res resourceType, int refAmount, int playerNum, compare_mode compareType, const char* triggerFunction);
OP2 Trigger __fastcall CreateKitTrigger(int bEnabled, int bOneShot, int playerNum, map_id, int refCount, const char* triggerFunction);
OP2 Trigger __fastcall CreateEscapeTrigger(int bEnabled, int bOneShot, int playerNum, int x, int y, int width, int height, int refValue, map_id unitType, int cargoType, int cargoAmount, const char* triggerFunction);
OP2 Trigger __fastcall CreateCountTrigger(int bEnabled, int bOneShot, int playerNum, map_id unitType, map_id cargoOrWeapon, int refCount, compare_mode compareType, const char* triggerFunction);
// Unit Count Triggers  [Note: See also CreateCountTrigger]
OP2 Trigger __fastcall CreateVehicleCountTrigger(int bEnabled, int bOneShot, int playerNum, int refCount, compare_mode compareType, const char* triggerFunction);
OP2 Trigger __fastcall CreateBuildingCountTrigger(int bEnabled, int bOneShot, int playerNum, int refCount, compare_mode compareType, const char* triggerFunction);
// Attack/Damage Triggers
OP2 Trigger __fastcall CreateAttackedTrigger(int bEnabled, int bOneShot, ScGroup& group, const char* triggerFunction);
OP2 Trigger __fastcall CreateDamagedTrigger(int bEnabled, int bOneShot, ScGroup& group, int damage, const char* triggerFunction);
// Time Triggers
OP2 Trigger __fastcall CreateTimeTrigger(int bEnabled, int bOneShot, int timeMin, int timeMax, const char* triggerFunction);
OP2 Trigger __fastcall CreateTimeTrigger(int bEnabled, int bOneShot, int time, const char* triggerFunction);
// Positional Triggers
OP2 Trigger __fastcall CreatePointTrigger(int bEnabled, int bOneShot, int playerNum, int x, int y, const char* triggerFunction);
OP2 Trigger __fastcall CreateRectTrigger(int bEnabled, int bOneShot, int playerNum, int x, int y, int width, int height, const char* triggerFunction);
// Special Target Trigger/Data
OP2 Trigger __fastcall CreateSpecialTarget(int bEnabled, int bOneShot, Unit& targetUnit /* Lab */, map_id sourceUnitType /* mapScout */, const char* triggerFunction);
OP2 void __fastcall GetSpecialTargetData(Trigger& specialTargetTrigger, Unit& sourceUnit /* Scout */);

// Set Trigger  [Note: Used to collect a number of other triggers into a single trigger output. Can be used for something like any 3 in a set of 5 objectives.]
OP2 Trigger __cdecl CreateSetTrigger(int bEnabled, int bOneShot, int totalTriggers, int neededTriggers, const char* triggerFunction, ...); // +list of triggers
