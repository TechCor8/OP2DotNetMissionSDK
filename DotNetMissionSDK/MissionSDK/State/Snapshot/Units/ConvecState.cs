using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;

namespace DotNetMissionSDK.State.Snapshot.Units
{
	/// <summary>
	/// Contains immutable unit state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class ConvecState : VehicleState
	{
		public map_id cargoType									{ get; private set; }


		/// <summary>
		/// Creates an immutable state from UnitEx.
		/// </summary>
		/// <param name="vehicleInfo">The info that applies to this unit.</param>
		/// <param name="weaponInfo">The info that applies to this unit's weapon.</param>
		public ConvecState(UnitEx unit, VehicleInfo vehicleInfo, WeaponInfo weaponInfo) : base(unit, vehicleInfo, weaponInfo)
		{
			cargoType			= unit.GetCargo();
		}
	}
}
