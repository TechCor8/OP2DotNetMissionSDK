// Note: This is the trigger class returned by all the trigger creation
//		 functions. It can be used to control an active trigger.
// Note: The trigger class is only a stub that refers to an internal
//		 class that handles the trigger. It is seldom necessary to use
//		 this class to control a trigger and it is usually destroyed
//		 shortly after it is returned from the trigger creation function.

using System;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK.Triggers
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi, Pack=1)]
	public class TriggerStubData
	{
		public bool isActive;       // If false, trigger has been released and its index is ready for reuse
		public bool hasFired;

		public int id;              // Developer specified ID for referencing this trigger
		public int stubIndex;		// The ID used by Outpost 2 to reference the trigger

		public bool enabled;
		public bool oneShot;
		public int playerID;
	}

	public class TriggerStub
	{
		private TriggerStubData m_StubData = new TriggerStubData();


		// [Properties]
		public int stubIndex
		{
			get { return m_StubData.stubIndex;				}
		}

		public int id
		{
			get { return m_StubData.id;						}
			set { m_StubData.id = value;					}
		}

		public bool isActive					{ get { return m_StubData.isActive;		} }

		internal TriggerStubData stubData		{ get { return m_StubData;				} }

		/// <summary>
		/// Only call this from TriggerManager for loading existing triggers.
		/// </summary>
		internal TriggerStub(int stubIndex, bool enabled, bool oneShot, int playerID)
		{
			m_StubData.isActive = true;

			m_StubData.stubIndex = stubIndex;

			m_StubData.enabled = enabled;
			m_StubData.oneShot = oneShot;
			m_StubData.hasFired = false;
			m_StubData.playerID = playerID;
		}

		public TriggerStub(TriggerStubData triggerStub)
		{
			m_StubData = triggerStub;
		}

		public void Disable()
		{
			m_StubData.enabled = false;

			Trigger_Disable(m_StubData.stubIndex);
		}

		public void Enable()
		{
			m_StubData.enabled = true;

			Trigger_Enable(m_StubData.stubIndex);
		}

		public bool HasFired(int playerID)  // Note: Do not pass -1 = PlayerAll
		{
			return Trigger_HasFired(m_StubData.stubIndex, playerID) != 0;
		}

		/// <summary>
		/// Checks the status of a trigger and returns true if it should execute.
		/// </summary>
		/// <param name="playerID">The player ID to check. If -1, uses the player ID passed at creation time.</param>
		/// <returns>True if the trigger should be executed.</returns>
		public bool CheckTrigger(int playerID=-1)
		{
			if (playerID < 0)
				playerID = m_StubData.playerID;

			if (playerID < 0)
			{
				playerID = 0;
				Console.WriteLine("CheckTrigger() - Warning: Bad playerNum!");
			}

			if (!m_StubData.hasFired && HasFired(playerID))
			{
				m_StubData.hasFired = true;

				// Release trigger
				m_StubData.isActive = false;
				
				return true;
			}

			return false;
		}


		[DllImport("NativeInterop.dll")] private static extern int Trigger_Disable(int stubIndex);
		[DllImport("NativeInterop.dll")] private static extern int Trigger_Enable(int stubIndex);
		[DllImport("NativeInterop.dll")] private static extern int Trigger_HasFired(int stubIndex, int playerNum);


		// Trigger creation functions
		// **************************

		// Victory/Failure condition triggers
		public static TriggerStub CreateVictoryCondition(bool enabled, TriggerStub victoryTriggerStub, string missionObjective)
		{
			int index = Trigger_CreateVictoryCondition(enabled ? 1 : 0, 0, victoryTriggerStub.m_StubData.stubIndex, missionObjective);
			return new TriggerStub(index, enabled, true, victoryTriggerStub.m_StubData.playerID);
		}

		public static TriggerStub CreateFailureCondition(bool enabled, TriggerStub failureTriggerStub)
		{
			int index = Trigger_CreateFailureCondition(enabled ? 1 : 0, 0, failureTriggerStub.m_StubData.stubIndex);
			return new TriggerStub(index, enabled, true, failureTriggerStub.m_StubData.playerID);
		}

		// Typical Victory Triggers
		public static TriggerStub CreateOnePlayerLeftTrigger(bool enabled, bool oneShot) // Last One Standing (and later part of Land Rush)
		{
			int index = Trigger_CreateOnePlayerLeftTrigger(enabled ? 1 : 0, oneShot ? 1 : 0);
			return new TriggerStub(index, enabled, oneShot, -1);
		}

		public static TriggerStub CreateEvacTrigger(bool enabled, bool oneShot, int playerID)  // Spacerace
		{
			int index = Trigger_CreateEvacTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerID);
			return new TriggerStub(index, enabled, oneShot, playerID);
		}

		public static TriggerStub CreateMidasTrigger(bool enabled, bool oneShot, int time)     // Midas
		{
			int index = Trigger_CreateMidasTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, time);
			return new TriggerStub(index, enabled, oneShot, -1);
		}

		public static TriggerStub CreateOperationalTrigger(bool enabled, bool oneShot, int playerID, map_id buildingType, int refValue, compare_mode compareType)  // Converting Land Rush to Last One Standing (when CC becomes active). Do not use PlayerAll.
		{
			int index = Trigger_CreateOperationalTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerID, buildingType, refValue, compareType);
			return new TriggerStub(index, enabled, oneShot, playerID);
		}

		// Research and Resource Count Triggers  [Note: Typically used to set what needs to be done by the end of a campaign mission]
		public static TriggerStub CreateResearchTrigger(bool enabled, bool oneShot, int techID, int playerID)
		{
			int index = Trigger_CreateResearchTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, techID, playerID);
			return new TriggerStub(index, enabled, oneShot, playerID);
		}
		public static TriggerStub CreateResourceTrigger(bool enabled, bool oneShot, trig_res resourceType, int refAmount, int playerID, compare_mode compareType)
		{
			int index = Trigger_CreateResourceTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, resourceType, refAmount, playerID, compareType);
			return new TriggerStub(index, enabled, oneShot, playerID);
		}
		public static TriggerStub CreateKitTrigger(bool enabled, bool oneShot, int playerID, map_id id, int refCount)
		{
			int index = Trigger_CreateKitTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerID, id, refCount);
			return new TriggerStub(index, enabled, oneShot, playerID);
		}
		public static TriggerStub CreateEscapeTrigger(bool enabled, bool oneShot, int playerID, int x, int y, int width, int height, int refValue, map_id unitType, Truck_Cargo cargoType, int cargoAmount)
		{
			int index = Trigger_CreateEscapeTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerID, x, y, width, height, refValue, unitType, cargoType, cargoAmount);
			return new TriggerStub(index, enabled, oneShot, playerID);
		}
		public static TriggerStub CreateCountTrigger(bool enabled, bool oneShot, int playerID, map_id unitType, map_id cargoOrWeapon, int refCount, compare_mode compareType)
		{
			int index = Trigger_CreateCountTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerID, unitType, cargoOrWeapon, refCount, compareType);
			return new TriggerStub(index, enabled, oneShot, playerID);
		}
		// Unit Count Triggers  [Note: See also CreateCountTrigger]
		public static TriggerStub CreateVehicleCountTrigger(bool enabled, bool oneShot, int playerID, int refCount, compare_mode compareType)
		{
			int index = Trigger_CreateVehicleCountTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerID, refCount, compareType);
			return new TriggerStub(index, enabled, oneShot, playerID);
		}
		public static TriggerStub CreateBuildingCountTrigger(bool enabled, bool oneShot, int playerID, int refCount, compare_mode compareType)
		{
			int index = Trigger_CreateBuildingCountTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerID, refCount, compareType);
			return new TriggerStub(index, enabled, oneShot, playerID);
		}
		// Attack/Damage Triggers
		//public static TriggerStub CreateAttackedTrigger(bool enabled, bool oneShot, ScGroup& group);
		//public static TriggerStub CreateDamagedTrigger(bool enabled, bool oneShot, ScGroup& group, int damage);
		// Time Triggers
		public static TriggerStub CreateTimeTrigger(bool enabled, bool oneShot, int timeMin, int timeMax)
		{
			int index = Trigger_CreateTimeTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, timeMin, timeMax);
			return new TriggerStub(index, enabled, oneShot, -1);
		}
		public static TriggerStub CreateTimeTrigger(bool enabled, bool oneShot, int time)
		{
			int index = Trigger_CreateTimeTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, time);
			return new TriggerStub(index, enabled, oneShot, -1);
		}
		// Positional Triggers
		public static TriggerStub CreatePointTrigger(bool enabled, bool oneShot, int playerID, int x, int y)
		{
			int index = Trigger_CreatePointTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerID, x, y);
			return new TriggerStub(index, enabled, oneShot, playerID);
		}
		public static TriggerStub CreateRectTrigger(bool enabled, bool oneShot, int playerID, int x, int y, int width, int height)
		{
			int index = Trigger_CreateRectTrigger(enabled ? 1 : 0, oneShot ? 1 : 0, playerID, x, y, width, height);
			return new TriggerStub(index, enabled, oneShot, playerID);
		}
		// Special Target Trigger/Data
		public static TriggerStub CreateSpecialTarget(bool enabled, bool oneShot, Unit targetUnit /* Lab */, map_id sourceUnitType /* mapScout */)
		{
			int index = Trigger_CreateSpecialTarget(enabled ? 1 : 0, oneShot ? 1 : 0, targetUnit.GetHandle(), sourceUnitType);
			return new TriggerStub(index, enabled, oneShot, -1);
		}
		public void GetSpecialTargetData(Unit sourceUnit /* Scout */)
		{
			Trigger_GetSpecialTargetData(m_StubData.stubIndex, sourceUnit.GetHandle());
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
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateVictoryCondition(int bEnabled, int bOneShot /*not used, set to 0*/, int victoryTrigger, string missionObjective);
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateFailureCondition(int bEnabled, int bOneShot /*not used, set to 0*/, int failureTrigger);

		// Typical Victory Triggers
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateOnePlayerLeftTrigger(int bEnabled, int bOneShot);			// Last One Standing (and later part of Land Rush)
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateEvacTrigger(int bEnabled, int bOneShot, int playerNum);	// Spacerace
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateMidasTrigger(int bEnabled, int bOneShot, int time);		// Midas
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateOperationalTrigger(int bEnabled, int bOneShot, int playerNum, map_id buildingType, int refValue, compare_mode compareType);	// Converting Land Rush to Last One Standing (when CC becomes active). Do not use PlayerAll.
		// Research and Resource Count Triggers  [Note: Typically used to set what needs to be done by the end of a campaign mission]
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateResearchTrigger(int bEnabled, int bOneShot, int techID, int playerNum);
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateResourceTrigger(int bEnabled, int bOneShot, trig_res resourceType, int refAmount, int playerNum, compare_mode compareType);
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateKitTrigger(int bEnabled, int bOneShot, int playerNum, map_id id, int refCount);
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateEscapeTrigger(int bEnabled, int bOneShot, int playerNum, int x, int y, int width, int height, int refValue, map_id unitType, Truck_Cargo cargoType, int cargoAmount);
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateCountTrigger(int bEnabled, int bOneShot, int playerNum, map_id unitType, map_id cargoOrWeapon, int refCount, compare_mode compareType);
		// Unit Count Triggers  [Note: See also CreateCountTrigger]
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateVehicleCountTrigger(int bEnabled, int bOneShot, int playerNum, int refCount, compare_mode compareType);
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateBuildingCountTrigger(int bEnabled, int bOneShot, int playerNum, int refCount, compare_mode compareType);
		// Attack/Damage Triggers
		//[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateAttackedTrigger(int bEnabled, int bOneShot, ScGroup& group);
		//[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateDamagedTrigger(int bEnabled, int bOneShot, ScGroup& group, int damage);
		// Time Triggers
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateTimeTrigger(int bEnabled, int bOneShot, int timeMin, int timeMax);
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateTimeTrigger(int bEnabled, int bOneShot, int time);
		// Positional Triggers
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreatePointTrigger(int bEnabled, int bOneShot, int playerNum, int x, int y);
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateRectTrigger(int bEnabled, int bOneShot, int playerNum, int x, int y, int width, int height);
		// Special Target Trigger/Data
		[DllImport("NativeInterop.dll")] private static extern int Trigger_CreateSpecialTarget(int bEnabled, int bOneShot, IntPtr targetUnit /* Lab */, map_id sourceUnitType /* mapScout */);
		[DllImport("NativeInterop.dll")] private static extern void Trigger_GetSpecialTargetData(int specialTargetTrigger, IntPtr sourceUnit /* Scout */);

		// Set Trigger  [Note: Used to collect a number of other triggers into a single trigger output. Can be used for something like any 3 in a set of 5 objectives.]
		//[DllImport("NativeInterop.dll")] private static extern IntPtr Trigger_CreateSetTrigger(int bEnabled, int bOneShot, int totalTriggers, int neededTriggers, string triggerFunction, IntPtr[] triggers); // +list of triggers
	}
}