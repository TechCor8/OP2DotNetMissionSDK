// Note: This file is used to define the enumerator classes exported
//		 from Outpost2.exe. These classes can be used to search for
//		 or traverse a list of units one unit at a time.

// ------------------------------------------------------------------------
// Note: All Enumerators implement a GetNext function, which returns
//	0 if no Unit was found, or non-zero if a Unit was found.
//	If a Unit was found, then it's index is returned in the Unit proxy/stub
//	passed as the first parameter.
// ------------------------------------------------------------------------

using DotNetMissionSDK.Async;
using DotNetMissionSDK.HFL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK
{
	/// <summary>
	/// Abstract base class for Outpost2 enumerators
	/// </summary>
	public abstract class OP2Enumerator : SDKDisposable, IEnumerable<Unit>
	{
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		public abstract bool GetNext(Unit unit);

		/// <summary>
		/// Returns the next unit in the enumerator.
		/// </summary>
		/// <returns>The next unit or null if not found.</returns>
		public Unit GetNext()
		{
			Unit unit = new UnitEx();
			if (!GetNext(unit))
				return null;

			return unit;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<Unit> GetEnumerator()
		{
			try
			{
				Unit unit;
				while ((unit = GetNext()) != null)
				{
					yield return unit;
				}
			}
			finally
			{
				Dispose();
			}
		}
	}

	// Group (enumerate all units in a group)
	public class GroupEnumerator : OP2Enumerator
	{
		private IntPtr m_Handle;


		public GroupEnumerator(ScGroup group)
		{
			ThreadAssert.MainThreadRequired();

			m_Handle = GroupEnumerator_Create(group.stubIndex);
		}

		/// <summary>
		/// Returns the next unit in the enumerator by overwriting the passed in unit.
		/// </summary>
		/// <param name="returnedUnit">The unit object to put the next unit into.</param>
		/// <returns>The true if the next unit is found, otherwise false.</returns>
		public override bool GetNext(Unit returnedUnit)
		{
			ThreadAssert.MainThreadRequired();

			int stubIndex = GroupEnumerator_GetNext(m_Handle);
			if (stubIndex < 0)
				return false;

			returnedUnit.SetStubIndex(stubIndex);
			return true;
		}

		[DllImport("DotNetInterop.dll")] private static extern IntPtr GroupEnumerator_Create(int groupStubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void GroupEnumerator_Release(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int GroupEnumerator_GetNext(IntPtr handle);

		// Dispose managed resources if "disposing" == true. Always dispose unmanaged resources.
		protected override void Dispose(bool disposing)
		{
			ThreadAssert.MainThreadRequired();

			base.Dispose(disposing);

			// Release unmanaged resources
			GroupEnumerator_Release(m_Handle);
		}
	}

	// Vehicles (enumerate all vehicles for a certain player)
	public class PlayerVehicleEnum : OP2Enumerator
	{
		private IntPtr m_Handle;


		public PlayerVehicleEnum(int playerID)
		{
			ThreadAssert.MainThreadRequired();

			m_Handle = PlayerVehicleEnum_Create(playerID);
		}

		/// <summary>
		/// Returns the next unit in the enumerator by overwriting the passed in unit.
		/// </summary>
		/// <param name="returnedUnit">The unit object to put the next unit into.</param>
		/// <returns>The true if the next unit is found, otherwise false.</returns>
		public override bool GetNext(Unit returnedUnit)
		{
			ThreadAssert.MainThreadRequired();

			int stubIndex = PlayerVehicleEnum_GetNext(m_Handle);
			if (stubIndex < 0)
				return false;

			returnedUnit.SetStubIndex(stubIndex);
			return true;
		}

		[DllImport("DotNetInterop.dll")] private static extern IntPtr PlayerVehicleEnum_Create(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern void PlayerVehicleEnum_Release(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerVehicleEnum_GetNext(IntPtr handle);

		// Dispose managed resources if "disposing" == true. Always dispose unmanaged resources.
		protected override void Dispose(bool disposing)
		{
			ThreadAssert.MainThreadRequired();

			base.Dispose(disposing);

			// Release unmanaged resources
			PlayerVehicleEnum_Release(m_Handle);
		}
	}

	// Buildings (enumerate all buildings of a certain type for a certain player)
	public class PlayerBuildingEnum : OP2Enumerator
	{
		private IntPtr m_Handle;


		public PlayerBuildingEnum(int playerID, map_id buildingType)
		{
			ThreadAssert.MainThreadRequired();

			m_Handle = PlayerBuildingEnum_Create(playerID, buildingType);
		}

		/// <summary>
		/// Returns the next unit in the enumerator by overwriting the passed in unit.
		/// </summary>
		/// <param name="returnedUnit">The unit object to put the next unit into.</param>
		/// <returns>The true if the next unit is found, otherwise false.</returns>
		public override bool GetNext(Unit returnedUnit)
		{
			ThreadAssert.MainThreadRequired();

			int stubIndex = PlayerBuildingEnum_GetNext(m_Handle);
			if (stubIndex < 0)
				return false;

			returnedUnit.SetStubIndex(stubIndex);
			return true;
		}

		[DllImport("DotNetInterop.dll")] private static extern IntPtr PlayerBuildingEnum_Create(int playerID, map_id buildingType);
		[DllImport("DotNetInterop.dll")] private static extern void PlayerBuildingEnum_Release(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerBuildingEnum_GetNext(IntPtr handle);

		// Dispose managed resources if "disposing" == true. Always dispose unmanaged resources.
		protected override void Dispose(bool disposing)
		{
			ThreadAssert.MainThreadRequired();

			base.Dispose(disposing);

			// Release unmanaged resources
			PlayerBuildingEnum_Release(m_Handle);
		}
	}

	public class PlayerAllBuildingEnum : OP2Enumerator
	{
		private int m_PlayerID;
		private int m_BuildingIndex;
		private PlayerBuildingEnum m_CurEnumerator;


		public PlayerAllBuildingEnum(int playerID)
		{
			ThreadAssert.MainThreadRequired();

			m_PlayerID = playerID;
			m_BuildingIndex = 21;
			m_CurEnumerator = new PlayerBuildingEnum(m_PlayerID, (map_id)m_BuildingIndex);
		}

		public override bool GetNext(Unit returnedUnit)
		{
			ThreadAssert.MainThreadRequired();

			do
			{
				if (m_CurEnumerator.GetNext(returnedUnit))
					return true;

				++m_BuildingIndex;
				if (m_BuildingIndex == 59)
					return false;

				m_CurEnumerator.Dispose();
				m_CurEnumerator = new PlayerBuildingEnum(m_PlayerID, (map_id)m_BuildingIndex);
			}
			while (true);
		}

		protected override void Dispose(bool disposing)
		{
			ThreadAssert.MainThreadRequired();

			base.Dispose(disposing);

			if (disposing)
				m_CurEnumerator.Dispose();
		}
	}

	// Units (enumerate all units of a certain player)
	public class PlayerUnitEnum : OP2Enumerator
	{
		private IntPtr m_Handle;


		public PlayerUnitEnum(int playerID)
		{
			ThreadAssert.MainThreadRequired();

			m_Handle = PlayerUnitEnum_Create(playerID);
		}

		/// <summary>
		/// Returns the next unit in the enumerator by overwriting the passed in unit.
		/// </summary>
		/// <param name="returnedUnit">The unit object to put the next unit into.</param>
		/// <returns>The true if the next unit is found, otherwise false.</returns>
		public override bool GetNext(Unit returnedUnit)
		{
			ThreadAssert.MainThreadRequired();

			int stubIndex = PlayerUnitEnum_GetNext(m_Handle);
			if (stubIndex < 0)
				return false;

			returnedUnit.SetStubIndex(stubIndex);
			return true;
		}

		[DllImport("DotNetInterop.dll")] private static extern IntPtr PlayerUnitEnum_Create(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern void PlayerUnitEnum_Release(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerUnitEnum_GetNext(IntPtr handle);

		// Dispose managed resources if "disposing" == true. Always dispose unmanaged resources.
		protected override void Dispose(bool disposing)
		{
			ThreadAssert.MainThreadRequired();

			base.Dispose(disposing);

			// Release unmanaged resources
			PlayerUnitEnum_Release(m_Handle);
		}
	}

	// InRange (enumerate all units within a given distance of a given location)
	public class InRangeEnumerator : OP2Enumerator
	{
		private IntPtr m_Handle;


		public InRangeEnumerator(LOCATION centerPoint, int maxTileDistance)
		{
			ThreadAssert.MainThreadRequired();

			m_Handle = InRangeEnumerator_Create(centerPoint.x, centerPoint.y, maxTileDistance);
		}

		/// <summary>
		/// Returns the next unit in the enumerator by overwriting the passed in unit.
		/// </summary>
		/// <param name="returnedUnit">The unit object to put the next unit into.</param>
		/// <returns>The true if the next unit is found, otherwise false.</returns>
		public override bool GetNext(Unit returnedUnit)
		{
			ThreadAssert.MainThreadRequired();

			int stubIndex = InRangeEnumerator_GetNext(m_Handle);
			if (stubIndex < 0)
				return false;

			returnedUnit.SetStubIndex(stubIndex);
			return true;
		}

		[DllImport("DotNetInterop.dll")] private static extern IntPtr InRangeEnumerator_Create(int centerPointX, int centerPointY, int maxTileDistance);
		[DllImport("DotNetInterop.dll")] private static extern void InRangeEnumerator_Release(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int InRangeEnumerator_GetNext(IntPtr handle);

		// Dispose managed resources if "disposing" == true. Always dispose unmanaged resources.
		protected override void Dispose(bool disposing)
		{
			ThreadAssert.MainThreadRequired();

			base.Dispose(disposing);

			// Release unmanaged resources
			InRangeEnumerator_Release(m_Handle);
		}
	}

	// InRect (enumerate all units within a given rectangle)
	public class InRectEnumerator : OP2Enumerator
	{
		private IntPtr m_Handle;


		public InRectEnumerator(MAP_RECT rect)
		{
			ThreadAssert.MainThreadRequired();

			m_Handle = InRectEnumerator_Create(rect.xMin, rect.yMin, rect.xMax, rect.yMax);
		}

		/// <summary>
		/// Returns the next unit in the enumerator by overwriting the passed in unit.
		/// </summary>
		/// <param name="returnedUnit">The unit object to put the next unit into.</param>
		/// <returns>The true if the next unit is found, otherwise false.</returns>
		public override bool GetNext(Unit returnedUnit)
		{
			ThreadAssert.MainThreadRequired();

			int stubIndex = InRectEnumerator_GetNext(m_Handle);
			if (stubIndex < 0)
				return false;

			returnedUnit.SetStubIndex(stubIndex);
			return true;
		}

		[DllImport("DotNetInterop.dll")] private static extern IntPtr InRectEnumerator_Create(int minX, int minY, int maxX, int maxY);
		[DllImport("DotNetInterop.dll")] private static extern void InRectEnumerator_Release(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int InRectEnumerator_GetNext(IntPtr handle);

		// Dispose managed resources if "disposing" == true. Always dispose unmanaged resources.
		protected override void Dispose(bool disposing)
		{
			ThreadAssert.MainThreadRequired();

			base.Dispose(disposing);

			// Release unmanaged resources
			InRectEnumerator_Release(m_Handle);
		}
	}

	// Location (enumerate all units at a given location)
	public class LocationEnumerator : OP2Enumerator
	{
		private IntPtr m_Handle;


		public LocationEnumerator(LOCATION location)
		{
			ThreadAssert.MainThreadRequired();

			m_Handle = LocationEnumerator_Create(location.x, location.y);
		}

		/// <summary>
		/// Returns the next unit in the enumerator by overwriting the passed in unit.
		/// </summary>
		/// <param name="returnedUnit">The unit object to put the next unit into.</param>
		/// <returns>The true if the next unit is found, otherwise false.</returns>
		public override bool GetNext(Unit returnedUnit)
		{
			ThreadAssert.MainThreadRequired();

			int stubIndex = LocationEnumerator_GetNext(m_Handle);
			if (stubIndex < 0)
				return false;

			returnedUnit.SetStubIndex(stubIndex);
			return true;
		}

		[DllImport("DotNetInterop.dll")] private static extern IntPtr LocationEnumerator_Create(int x, int y);
		[DllImport("DotNetInterop.dll")] private static extern void LocationEnumerator_Release(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int LocationEnumerator_GetNext(IntPtr handle);

		// Dispose managed resources if "disposing" == true. Always dispose unmanaged resources.
		protected override void Dispose(bool disposing)
		{
			ThreadAssert.MainThreadRequired();

			base.Dispose(disposing);

			// Release unmanaged resources
			LocationEnumerator_Release(m_Handle);
		}
	}

	// Closest (enumerate all units ordered by their distance to a given location)
	public class ClosestEnumerator : OP2Enumerator
	{
		private IntPtr m_Handle;


		public ClosestEnumerator(LOCATION location)
		{
			ThreadAssert.MainThreadRequired();

			m_Handle = ClosestEnumerator_Create(location.x, location.y);
		}

		/// <summary>
		/// Returns the next unit in the enumerator by overwriting the passed in unit.
		/// </summary>
		/// <param name="returnedUnit">The unit object to put the next unit into.</param>
		/// <returns>The true if the next unit is found, otherwise false.</returns>
		public override bool GetNext(Unit returnedUnit)
		{
			ThreadAssert.MainThreadRequired();

			int stubIndex = ClosestEnumerator_GetNext(m_Handle);
			if (stubIndex < 0)
				return false;

			returnedUnit.SetStubIndex(stubIndex);
			return true;
		}

		[DllImport("DotNetInterop.dll")] private static extern IntPtr ClosestEnumerator_Create(int x, int y);
		[DllImport("DotNetInterop.dll")] private static extern void ClosestEnumerator_Release(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int ClosestEnumerator_GetNext(IntPtr handle);
		//[DllImport("DotNetInterop.dll")] private static extern int ClosestEnumerator_GetCurrentPixelDistance(IntPtr handle);

		// Dispose managed resources if "disposing" == true. Always dispose unmanaged resources.
		protected override void Dispose(bool disposing)
		{
			ThreadAssert.MainThreadRequired();

			base.Dispose(disposing);

			// Release unmanaged resources
			ClosestEnumerator_Release(m_Handle);
		}
	}
}