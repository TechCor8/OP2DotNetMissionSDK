// Note: The UnitBlock class can be used for creating blocks of units
//		 of certain predefined types.

using DotNetMissionSDK.Async;
using System;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK
{
	public class UnitBlock : SDKDisposable
	{
		private IntPtr m_Handle;

		public IntPtr GetHandle()			{ return m_Handle;						}


		public UnitBlock(UnitRecord[] unitRecordTable)
		{
			ThreadAssert.MainThreadRequired();

			// Write records into array
			IntPtr arr = UnitRecord_CreateArray(unitRecordTable.Length);

			for (int i=0; i < unitRecordTable.Length; ++i)
			{
				UnitRecord_SetArray(arr, i, unitRecordTable[i].unitType, unitRecordTable[i].x, unitRecordTable[i].y, unitRecordTable[i].unknown1, unitRecordTable[i].rotation,
									unitRecordTable[i].weaponType, unitRecordTable[i].unitClassification, unitRecordTable[i].cargoType, unitRecordTable[i].cargoAmount);
			}

			// Create UnitBlock
			m_Handle = UnitBlock_Create(arr);

			UnitRecord_ReleaseArray(arr);
		}

		// Returns numUnitsCreated
		public int CreateUnits(int playerID, int bLightsOn)
		{
			ThreadAssert.MainThreadRequired();

			return UnitBlock_CreateUnits(m_Handle, playerID, bLightsOn);
		}

		[DllImport("DotNetInterop.dll")] private static extern IntPtr UnitBlock_Create(IntPtr unitRecordTable);
		[DllImport("DotNetInterop.dll")] private static extern int UnitBlock_CreateUnits(IntPtr handle, int playerNum, int bLightsOn);

		// Create and manage UnitRecord array
		[DllImport("DotNetInterop.dll")] private static extern IntPtr UnitRecord_CreateArray(int size);
		[DllImport("DotNetInterop.dll")] private static extern void UnitRecord_SetArray(IntPtr arr, int index, map_id unitType, int x, int y, int unknown1, int rotation, map_id weaponType,
																						UnitClassification unitClassification, short cargoType, short cargoAmount);
		[DllImport("DotNetInterop.dll")] private static extern void UnitRecord_ReleaseArray(IntPtr handle);
	}
}
