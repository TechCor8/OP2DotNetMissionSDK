using DotNetMissionSDK.HFL;

namespace DotNetMissionSDK.State.Snapshot.UnitTypeInfo
{
	/// <summary>
	/// Contains immutable unit info state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class StructureInfo : UnitInfoState
	{
		public int powerRequired					{ get; private set; }
		public int workersRequired					{ get; private set; }
		public int scientistsRequired				{ get; private set; }
		public int productionRate					{ get; private set; }
		public int storageCapacity					{ get; private set; }
		public int productionCapacity				{ get; private set; }
		public int numStorageBays					{ get; private set; }
		public int cargoCapacity					{ get; private set; }


		/// <summary>
		/// Creates an immutable state for a unit type.
		/// </summary>
		/// <param name="unitTypeID">The unit type to pull data from.</param>
		/// <param name="playerID">The player to pull data from.</param>
		public StructureInfo(map_id unitTypeID, int playerID) : base(unitTypeID, playerID)
		{
			UnitInfo info = new UnitInfo(unitTypeID);

			powerRequired		= info.GetPowerRequired(playerID);
			workersRequired		= info.GetWorkersRequired(playerID);
			scientistsRequired	= info.GetScientistsRequired(playerID);
			productionRate		= info.GetProductionRate(playerID);
			storageCapacity		= info.GetStorageCapacity(playerID);
			productionCapacity	= info.GetProductionCapacity(playerID);
			numStorageBays		= info.GetNumStorageBays(playerID);
			cargoCapacity		= info.GetCargoCapacity(playerID);
		}
	}
}
