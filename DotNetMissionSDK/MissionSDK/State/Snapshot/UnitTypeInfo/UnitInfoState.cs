using DotNetMissionSDK.HFL;

namespace DotNetMissionSDK.State.Snapshot.UnitTypeInfo
{
	/// <summary>
	/// Contains immutable unit info state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class UnitInfoState
	{
		public map_id unitType						{ get; private set; }

		/// <summary>
		/// The player ID this info has been pulled from.
		/// </summary>
		public int playerID							{ get; private set; }

		public int hitPoints						{ get; private set; }
		public int repairAmount						{ get; private set; }
		public int armor							{ get; private set; }
		public int oreCost							{ get; private set; }
		public int rareOreCost						{ get; private set; }
		public int buildTime						{ get; private set; }
		public int sightRange						{ get; private set; }


		/// <summary>
		/// Creates an immutable state for a unit type.
		/// </summary>
		/// <param name="unitTypeID">The unit type to pull data from.</param>
		/// <param name="playerID">The player to pull data from.</param>
		public UnitInfoState(map_id unitTypeID, int playerID)
		{
			UnitInfo info = new UnitInfo(unitTypeID);

			unitType		= unitTypeID;

			this.playerID	= playerID;

			hitPoints		= info.GetHitPoints(playerID);
			repairAmount	= info.GetRepairAmount(playerID);
			armor			= info.GetArmor(playerID);
			oreCost			= info.GetOreCost(playerID);
			rareOreCost		= info.GetRareOreCost(playerID);
			buildTime		= info.GetBuildTime(playerID);
			sightRange		= info.GetSightRange(playerID);
		}
	}
}
