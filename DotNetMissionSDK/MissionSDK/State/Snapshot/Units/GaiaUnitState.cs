using DotNetMissionSDK.HFL;

namespace DotNetMissionSDK.State.Snapshot.Units
{
	/// <summary>
	/// Contains immutable gaia (unowned) unit state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// NOTE: GaiaUnitStates that have the same unitID are considered equal.
	/// </summary>
	public class GaiaUnitState
	{
		public int unitID					{ get; private set; }

		public map_id unitType				{ get; private set; }
		public int ownerID					{ get; private set; }

		public bool isLive					{ get; private set; }

		public LOCATION position			{ get; private set; }

		/// <summary>
		/// Creates an immutable state from UnitEx.
		/// </summary>
		/// <param name="unit">The unit to pull data from.</param>
		public GaiaUnitState(UnitEx unit)
		{
			unitID			= unit.GetStubIndex();

			unitType		= unit.GetUnitType();
			ownerID			= unit.GetOwnerID();

			isLive			= unit.IsLive();

			position		= unit.GetPosition();
		}

		public override bool Equals(object obj)
		{
			GaiaUnitState t = obj as GaiaUnitState;
			if (t == null)
				return false;

			return unitID == t.unitID;
		}

		public override int GetHashCode()
		{
			return unitID;
		}
	}
}
