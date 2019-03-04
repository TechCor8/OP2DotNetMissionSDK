// Note: This is the trigger class returned by all the trigger creation
//		 functions. It can be used to control an active trigger.
// Note: The trigger class is only a stub that refers to an internal
//		 class that handles the trigger. It is seldom necessary to use
//		 this class to control a trigger and it is usually destroyed
//		 shortly after it is returned from the trigger creation function.

using System;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK
{
	public class Trigger : ScStub
	{
		public Trigger(IntPtr handle) : base(handle)
		{
		}

		//void Disable();
		//void Enable();
		//int HasFired(int playerNum);	// Note: Do not pass -1 = PlayerAll


		public const string EmptyFunction = "NoResponseToTrigger";

		// Trigger creation functions
		// **************************
		// Victory/Failure condition triggers
		public static Trigger CreateVictoryCondition(bool enabled, Trigger victoryTrigger, string missionObjective)
		{
			return new Trigger(Trigger_CreateVictoryCondition(enabled ? 1 : 0, 1, victoryTrigger.GetHandle(), missionObjective));
		}
		public static Trigger CreateFailureCondition(bool enabled, Trigger failureTrigger)
		{
			return new Trigger(Trigger_CreateFailureCondition(enabled ? 1 : 0, 1, failureTrigger.GetHandle(), ""));
		}

		// Typical Victory Triggers
		public static Trigger CreateOnePlayerLeftTrigger(bool enabled, bool oneShot, string triggerFunction) // Last One Standing (and later part of Land Rush)
		{
			return new Trigger(Trigger_CreateOnePlayerLeftTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, triggerFunction));
		}
		public static Trigger CreateEvacTrigger(bool enabled, bool oneShot, int playerNum, string triggerFunction)  // Spacerace
		{
			return new Trigger(Trigger_CreateEvacTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerNum, triggerFunction));
		}
		public static Trigger CreateMidasTrigger(bool enabled, bool oneShot, int time, string triggerFunction)     // Midas
		{
			return new Trigger(Trigger_CreateMidasTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, time, triggerFunction));
		}
		public static Trigger CreateOperationalTrigger(bool enabled, bool oneShot, int playerNum, map_id buildingType, int refValue, compare_mode compareType, string triggerFunction)  // Converting Land Rush to Last One Standing (when CC becomes active). Do not use PlayerAll.
		{
			return new Trigger(Trigger_CreateOperationalTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerNum, buildingType, refValue, compareType, triggerFunction));
		}
		// Research and Resource Count Triggers  [Note: Typically used to set what needs to be done by the end of a campaign mission]
		public static Trigger CreateResearchTrigger(bool enabled, bool oneShot, int techID, int playerNum, string triggerFunction)
		{
			return new Trigger(Trigger_CreateResearchTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, techID, playerNum, triggerFunction));
		}
		public static Trigger CreateResourceTrigger(bool enabled, bool oneShot, trig_res resourceType, int refAmount, int playerNum, compare_mode compareType, string triggerFunction)
		{
			return new Trigger(Trigger_CreateResourceTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, resourceType, refAmount, playerNum, compareType, triggerFunction));
		}
		public static Trigger CreateKitTrigger(bool enabled, bool oneShot, int playerNum, map_id id, int refCount, string triggerFunction)
		{
			return new Trigger(Trigger_CreateKitTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerNum, id, refCount, triggerFunction));
		}
		public static Trigger CreateEscapeTrigger(bool enabled, bool oneShot, int playerNum, int x, int y, int width, int height, int refValue, map_id unitType, int cargoType, int cargoAmount, string triggerFunction)
		{
			return new Trigger(Trigger_CreateEscapeTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerNum, x, y, width, height, refValue, unitType, cargoType, cargoAmount, triggerFunction));
		}
		public static Trigger CreateCountTrigger(bool enabled, bool oneShot, int playerNum, map_id unitType, map_id cargoOrWeapon, int refCount, compare_mode compareType, string triggerFunction)
		{
			return new Trigger(Trigger_CreateCountTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerNum, unitType, cargoOrWeapon, refCount, compareType, triggerFunction));
		}
		// Unit Count Triggers  [Note: See also CreateCountTrigger]
		public static Trigger CreateVehicleCountTrigger(bool enabled, bool oneShot, int playerNum, int refCount, compare_mode compareType, string triggerFunction)
		{
			return new Trigger(Trigger_CreateVehicleCountTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerNum, refCount, compareType, triggerFunction));
		}
		public static Trigger CreateBuildingCountTrigger(bool enabled, bool oneShot, int playerNum, int refCount, compare_mode compareType, string triggerFunction)
		{
			return new Trigger(Trigger_CreateBuildingCountTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerNum, refCount, compareType, triggerFunction));
		}
		// Attack/Damage Triggers
		//public static Trigger CreateAttackedTrigger(bool enabled, bool oneShot, ScGroup& group, string triggerFunction);
		//public static Trigger CreateDamagedTrigger(bool enabled, bool oneShot, ScGroup& group, int damage, string triggerFunction);
		// Time Triggers
		public static Trigger CreateTimeTrigger(bool enabled, bool oneShot, int timeMin, int timeMax, string triggerFunction)
		{
			return new Trigger(Trigger_CreateTimeTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, timeMin, timeMax, triggerFunction));
		}
		public static Trigger CreateTimeTrigger(bool enabled, bool oneShot, int time, string triggerFunction)
		{
			return new Trigger(Trigger_CreateTimeTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, time, triggerFunction));
		}
		// Positional Triggers
		public static Trigger CreatePointTrigger(bool enabled, bool oneShot, int playerNum, int x, int y, string triggerFunction)
		{
			return new Trigger(Trigger_CreatePointTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerNum, x, y, triggerFunction));
		}
		public static Trigger CreateRectTrigger(bool enabled, bool oneShot, int playerNum, int x, int y, int width, int height, string triggerFunction)
		{
			return new Trigger(Trigger_CreateRectTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerNum, x, y, width, height, triggerFunction));
		}
		// Special Target Trigger/Data
		public static Trigger CreateSpecialTarget(bool enabled, bool oneShot, Unit targetUnit /* Lab */, map_id sourceUnitType /* mapScout */, string triggerFunction)
		{
			return new Trigger(Trigger_CreateSpecialTarget(enabled ? 1 : 0, oneShot ? 1 : 0, targetUnit.GetHandle(), sourceUnitType, triggerFunction));
		}
		public static void GetSpecialTargetData(Trigger specialTargetTrigger, Unit sourceUnit /* Scout */)
		{
			Trigger_GetSpecialTargetData(specialTargetTrigger.GetHandle(), sourceUnit.GetHandle());
		}

		// Set Trigger  [Note: Used to collect a number of other triggers into a single trigger output. Can be used for something like any 3 in a set of 5 objectives.]
		/*public static Trigger CreateSetTrigger(bool enabled, bool oneShot, int totalTriggers, int neededTriggers, string triggerFunction, Trigger[] triggers)
		{
			IntPtr[] pointers = new IntPtr[triggers.Length];
			for (int i=0; i < pointers.Length; ++i)
				pointers[i] = triggers[i].GetHandle();

			return new Trigger(Trigger_CreateSetTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, totalTriggers, neededTriggers, triggerFunction, pointers));
		}*/

		// Victory/Failure condition triggers
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateVictoryCondition(int bEnabled, int bOneShot /*not used, set to 0*/, IntPtr victoryTrigger, string missionObjective);
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateFailureCondition(int bEnabled, int bOneShot /*not used, set to 0*/, IntPtr failureTrigger, string failureCondition /*not used, set to ""*/);

		// Typical Victory Triggers
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateOnePlayerLeftTrigger(int bEnabled, int bOneShot, string triggerFunction);			// Last One Standing (and later part of Land Rush)
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateEvacTrigger(int bEnabled, int bOneShot, int playerNum, string triggerFunction);	// Spacerace
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateMidasTrigger(int bEnabled, int bOneShot, int time, string triggerFunction);		// Midas
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateOperationalTrigger(int bEnabled, int bOneShot, int playerNum, map_id buildingType, int refValue, compare_mode compareType, string triggerFunction);	// Converting Land Rush to Last One Standing (when CC becomes active). Do not use PlayerAll.
		// Research and Resource Count Triggers  [Note: Typically used to set what needs to be done by the end of a campaign mission]
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateResearchTrigger(int bEnabled, int bOneShot, int techID, int playerNum, string triggerFunction);
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateResourceTrigger(int bEnabled, int bOneShot, trig_res resourceType, int refAmount, int playerNum, compare_mode compareType, string triggerFunction);
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateKitTrigger(int bEnabled, int bOneShot, int playerNum, map_id id, int refCount, string triggerFunction);
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateEscapeTrigger(int bEnabled, int bOneShot, int playerNum, int x, int y, int width, int height, int refValue, map_id unitType, int cargoType, int cargoAmount, string triggerFunction);
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateCountTrigger(int bEnabled, int bOneShot, int playerNum, map_id unitType, map_id cargoOrWeapon, int refCount, compare_mode compareType, string triggerFunction);
		// Unit Count Triggers  [Note: See also CreateCountTrigger]
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateVehicleCountTrigger(int bEnabled, int bOneShot, int playerNum, int refCount, compare_mode compareType, string triggerFunction);
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateBuildingCountTrigger(int bEnabled, int bOneShot, int playerNum, int refCount, compare_mode compareType, string triggerFunction);
		// Attack/Damage Triggers
		//[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateAttackedTrigger(int bEnabled, int bOneShot, ScGroup& group, string triggerFunction);
		//[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateDamagedTrigger(int bEnabled, int bOneShot, ScGroup& group, int damage, string triggerFunction);
		// Time Triggers
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateTimeTrigger(int bEnabled, int bOneShot, int timeMin, int timeMax, string triggerFunction);
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateTimeTrigger(int bEnabled, int bOneShot, int time, string triggerFunction);
		// Positional Triggers
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreatePointTrigger(int bEnabled, int bOneShot, int playerNum, int x, int y, string triggerFunction);
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateRectTrigger(int bEnabled, int bOneShot, int playerNum, int x, int y, int width, int height, string triggerFunction);
		// Special Target Trigger/Data
		[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateSpecialTarget(int bEnabled, int bOneShot, IntPtr targetUnit /* Lab */, map_id sourceUnitType /* mapScout */, string triggerFunction);
		[DllImport("NativeInterop.dll")] private static extern void Trigger_GetSpecialTargetData(IntPtr specialTargetTrigger, IntPtr sourceUnit /* Scout */);

		// Set Trigger  [Note: Used to collect a number of other triggers into a single trigger output. Can be used for something like any 3 in a set of 5 objectives.]
		//[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateSetTrigger(int bEnabled, int bOneShot, int totalTriggers, int neededTriggers, string triggerFunction, IntPtr[] triggers); // +list of triggers
	}
}