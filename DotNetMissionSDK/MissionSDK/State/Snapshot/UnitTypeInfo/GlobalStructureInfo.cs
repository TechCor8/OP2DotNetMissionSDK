using DotNetMissionSDK.HFL;

namespace DotNetMissionSDK.State.Snapshot.UnitTypeInfo
{
	/// <summary>
	/// Contains immutable unit info state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class GlobalStructureInfo : GlobalUnitInfo
	{
		public int xSize											{ get; private set; }
		public int ySize											{ get; private set; }
		public BuildingFlags buildingFlags							{ get; private set; }
		public int resourcePriority									{ get; private set; }
		public int rareRubble										{ get; private set; }
		public int rubble											{ get; private set; }
		public int edenDockPos										{ get; private set; }
		public int plymDockPos										{ get; private set; }
		
		public LOCATION GetSize(bool includeBulldozedArea=false)
		{
			LOCATION result = new LOCATION(xSize, ySize);

			if (includeBulldozedArea)
			{
				result.x += 2;
				result.y += 2;
			}

			return result;
		}

		/// <summary>
		/// Gets a rect representing the unit's size around a center point.
		/// </summary>
		/// <param name="position">The center point of the unit rect.</param>
		/// <param name="includeBulldozedArea">Whether or not to include the bulldozed area.</param>
		/// <returns>The unit rect.</returns>
		public MAP_RECT GetRect(LOCATION position, bool includeBulldozedArea=false)
		{
			LOCATION size = GetSize(includeBulldozedArea);

			return new MAP_RECT(position - (size / 2), size);
		}

		/// <summary>
		/// Creates an immutable state for a unit type.
		/// </summary>
		/// <param name="unitTypeID">The unit type to pull data from.</param>
		public GlobalStructureInfo(map_id unitTypeID) : base(unitTypeID)
		{
			UnitInfo info = new UnitInfo(unitTypeID);

			xSize				= info.GetXSize();
			ySize				= info.GetYSize();
			buildingFlags		= info.GetBuildingFlags();
			resourcePriority	= info.GetResourcePriority();
			rareRubble			= info.GetRareRubble();
			rubble				= info.GetRubble();
			edenDockPos			= info.GetEdenDockPos();
			plymDockPos			= info.GetPlymDockPos();
		}
	}
}
