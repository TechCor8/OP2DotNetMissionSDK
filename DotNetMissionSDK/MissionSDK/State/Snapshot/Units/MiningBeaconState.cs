using DotNetMissionSDK.HFL;

namespace DotNetMissionSDK.State.Snapshot.Units
{
	/// <summary>
	/// Contains immutable unit state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class MiningBeaconState : GaiaUnitState
	{
		private int m_PlayerSurveyedFlags;

		public int numTruckLoadsSoFar		{ get; private set; }
		public Yield barYield				{ get; private set; }
		public Variant variant				{ get; private set; }
		public BeaconType oreType			{ get; private set; }

		public bool GetSurveyedBy(int playerID)
		{
			return ((1 << playerID) & m_PlayerSurveyedFlags) != 0;
		}


		/// <summary>
		/// Creates an immutable state from UnitEx.
		/// </summary>
		/// <param name="unit">The unit to pull data from.</param>
		public MiningBeaconState(UnitEx unit) : base(unit)
		{
			numTruckLoadsSoFar		= unit.GetNumTruckLoadsSoFar();
			barYield				= unit.GetBarYield();
			variant					= unit.GetVariant();
			oreType					= unitType == map_id.MagmaVent ? BeaconType.Rare : unit.GetOreType();

			m_PlayerSurveyedFlags	= unit.GetSurveyedFlags();
		}
	}
}
