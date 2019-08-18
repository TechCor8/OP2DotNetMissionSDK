using DotNetMissionSDK.State.Snapshot.Units;
using System.Collections.Generic;

namespace DotNetMissionSDK.State.Snapshot.Maps
{
	/// <summary>
	/// Represents a map of global, unowned units. This includes mining beacons, magma vents, fumaroles, and wreckage.
	/// NOTE: Recommended way to access this class is through StateSnapshot.
	/// </summary>
	public class GaiaMap
	{
		private struct Tile
		{
			public GaiaUnitState unitOnTile;
		}

		private Tile[,] m_Grid;


		/// <summary>
		/// Initializes the map. Must be called after GameMap has been initialized.
		/// </summary>
		public GaiaMap(GaiaState gaia)
		{
			m_Grid = new Tile[GameMap.bounds.width, GameMap.bounds.height];
			//Array.Clear(m_Grid, 0, m_Grid.Length);

			// Loop through every gaia unit
			foreach (GaiaUnitState unit in gaia)
			{
				// Place unit on grid
				LOCATION gridPt = GetPointInGridSpace(unit.position);
				m_Grid[gridPt.x, gridPt.y].unitOnTile = unit;
			}
		}

		private static LOCATION GetPointInGridSpace(LOCATION pt)
		{
			pt.x = pt.x - GameMap.bounds.xMin;
			pt.y = pt.y - GameMap.bounds.yMin;

			return pt;
		}

		/// <summary>
		/// Returns the unit at the specified tile.
		/// </summary>
		public GaiaUnitState GetUnitOnTile(LOCATION tilePosition)
		{
			tilePosition = GetPointInGridSpace(tilePosition);

			return m_Grid[tilePosition.x, tilePosition.y].unitOnTile;
		}

		/// <summary>
		/// Returns all units in the specified area.
		/// </summary>
		public List<GaiaUnitState> GetUnitsInArea(MAP_RECT area)
		{
			List<GaiaUnitState> units = new List<GaiaUnitState>();

			for (int x=area.xMin; x < area.xMax; ++x)
			{
				for (int y=area.yMin; y < area.yMax; ++y)
				{
					LOCATION tile = new LOCATION(x,y);
					tile.ClipToMap();

					GaiaUnitState unit = GetUnitOnTile(tile);
					if (unit != null)
						units.Add(unit);
				}
			}

			return units;
		}
	}
}
