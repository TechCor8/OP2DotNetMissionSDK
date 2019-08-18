using DotNetMissionSDK.HFL;

namespace DotNetMissionSDK.State.Snapshot.Units
{
	/// <summary>
	/// Contains immutable unit state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class WreckageState : GaiaUnitState
	{
		public bool isDiscovered								{ get; private set; }


		/// <summary>
		/// Creates an immutable state from UnitEx.
		/// </summary>
		/// <param name="unit">The unit to pull data from.</param>
		public WreckageState(UnitEx unit) : base(unit)
		{
			isDiscovered		= unit.IsDiscovered();
		}
	}
}
