using DotNetMissionSDK.HFL;

namespace DotNetMissionSDK.State.Snapshot.UnitTypeInfo
{
	/// <summary>
	/// Contains immutable unit info state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class GlobalVehicleInfo : GlobalUnitInfo
	{
		public VehicleFlags vehicleFlags							{ get; private set; }

		public TrackType trackType									{ get; private set; }


		/// <summary>
		/// Creates an immutable state for a unit type.
		/// </summary>
		/// <param name="unitTypeID">The unit type to pull data from.</param>
		public GlobalVehicleInfo(map_id unitTypeID) : base(unitTypeID)
		{
			UnitInfo info = new UnitInfo(unitTypeID);

			vehicleFlags	= info.GetVehicleFlags();

			trackType		= info.GetTrackType();
		}
	}
}
