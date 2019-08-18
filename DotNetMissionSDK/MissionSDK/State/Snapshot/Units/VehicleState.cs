using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;

namespace DotNetMissionSDK.State.Snapshot.Units
{
	/// <summary>
	/// Contains immutable unit state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class VehicleState : UnitState
	{
		public VehicleInfo vehicleInfo		{ get; private set; }
		public WeaponInfo weaponInfo		{ get; private set; }

		public bool isStickyfoamed			{ get; private set; }
		public bool isESGed					{ get; private set; }


		public bool IsOnDock(StructureState unitWithDock)
		{
			return unitWithDock.dockLocation.Equals(position);
		}


		/// <summary>
		/// Creates an immutable state from UnitEx.
		/// </summary>
		/// <param name="unit">The unit to pull data from.</param>
		/// <param name="vehicleInfo">The info that applies to this unit.</param>
		/// <param name="weaponInfo">The info that applies to this unit's weapon.</param>
		public VehicleState(UnitEx unit, VehicleInfo vehicleInfo, WeaponInfo weaponInfo) : base(unit)
		{
			this.vehicleInfo		= vehicleInfo;
			this.weaponInfo			= weaponInfo;

			isStickyfoamed			= unit.IsStickyfoamed();
			isESGed					= unit.IsESGed();
		}
	}
}
