using DotNetMissionSDK.State.Snapshot.Units;
using System.Collections.Generic;

namespace DotNetMissionSDK.State.Snapshot.Maps
{
	/// <summary>
	/// Represents a map of all players' units.
	/// NOTE: Recommended way to access this class is through StateSnapshot.
	/// </summary>
	public class PlayerUnitMap
	{
		private struct Tile
		{
			public UnitState unitOnTile;
		}

		private Tile[,] m_Grid;


		/// <summary>
		/// Initializes the unit map. Must be called after GameMap has been initialized.
		/// </summary>
		public PlayerUnitMap(PlayerState[] playerStates)
		{
			m_Grid = new Tile[GameMap.bounds.width, GameMap.bounds.height];
			//Array.Clear(m_Grid, 0, m_Grid.Length);

			// Loop through every unit in every player
			foreach (PlayerState state in playerStates)
			{
				if (state == null)
					continue;

				foreach (UnitState unit in state.units.GetUnits())
				{
					// Place unit on grid
					if (!unit.isBuilding)
					{
						LOCATION gridPt = GetPointInGridSpace(unit.position);
						m_Grid[gridPt.x, gridPt.y].unitOnTile = unit;
					}
					else
					{
						// Place complete building area on grid
						StructureState buildingState = (StructureState)unit;
						MAP_RECT buildingArea = buildingState.GetRect();

						for (int x=buildingArea.xMin; x < buildingArea.xMax; ++x)
						{
							for (int y=buildingArea.yMin; y < buildingArea.yMax; ++y)
							{
								LOCATION gridPoint = new LOCATION(x, y);
								gridPoint.ClipToMap();

								gridPoint = GetPointInGridSpace(gridPoint);

								m_Grid[gridPoint.x,gridPoint.y].unitOnTile = unit;
							}
						}
					}
				}
			} // foreach playerStates
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
		public UnitState GetUnitOnTile(LOCATION tilePosition)
		{
			tilePosition = GetPointInGridSpace(tilePosition);

			return m_Grid[tilePosition.x, tilePosition.y].unitOnTile;
		}

		/// <summary>
		/// Returns all units in the specified area.
		/// </summary>
		public List<UnitState> GetUnitsInArea(MAP_RECT area)
		{
			List<UnitState> units = new List<UnitState>();

			for (int x=area.xMin; x < area.xMax; ++x)
			{
				for (int y=area.yMin; y < area.yMax; ++y)
				{
					LOCATION tile = new LOCATION(x,y);
					tile.ClipToMap();

					UnitState unit = GetUnitOnTile(tile);
					if (unit != null)
						units.Add(unit);
				}
			}

			return units;
		}
	}
}
