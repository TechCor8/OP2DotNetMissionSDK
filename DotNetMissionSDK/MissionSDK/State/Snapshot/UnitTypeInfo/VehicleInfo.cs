using DotNetMissionSDK.HFL;

namespace DotNetMissionSDK.State.Snapshot.UnitTypeInfo
{
	/// <summary>
	/// Contains immutable unit info state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class VehicleInfo : UnitInfoState
	{
		public int movePoints						{ get; private set; }
		public int turnRate							{ get; private set; }


		/// <summary>
		/// Creates an immutable state for a unit type.
		/// </summary>
		/// <param name="unitTypeID">The unit type to pull data from.</param>
		/// <param name="playerID">The player to pull data from.</param>
		public VehicleInfo(map_id unitTypeID, int playerID) : base(unitTypeID, playerID)
		{
			UnitInfo info = new UnitInfo(unitTypeID);

			movePoints		= info.GetMovePoints(playerID);
			turnRate		= info.GetTurnRate(playerID);
		}
	}
}
