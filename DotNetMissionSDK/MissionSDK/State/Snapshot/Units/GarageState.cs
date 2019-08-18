using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;

namespace DotNetMissionSDK.State.Snapshot.Units
{
	/// <summary>
	/// Contains immutable unit state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class GarageState : StructureState
	{
		public bool hasOccupiedBay		{ get; private set; }

		/// <summary>
		/// Creates an immutable state from UnitEx.
		/// </summary>
		/// <param name="structureInfo">The info that applies to this unit.</param>
		/// <param name="weaponInfo">The info that applies to this unit's weapon.</param>
		public GarageState(UnitEx unit, StructureInfo structureInfo, WeaponInfo weaponInfo) : base(unit, structureInfo, weaponInfo)
		{
			hasOccupiedBay		= unit.HasOccupiedBay();
		}
	}
}
