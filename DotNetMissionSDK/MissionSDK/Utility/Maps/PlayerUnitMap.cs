using DotNetMissionSDK.HFL;
using System;
using System.Collections.Generic;

namespace DotNetMissionSDK.Utility.Maps
{
	/// <summary>
	/// Represents a map of all players' units. 
	/// </summary>
	public class PlayerUnitMap
	{
		private struct Tile
		{
			public UnitEx unitOnTile;
		}

		private static readonly object m_SyncObject = new object();

		private static Tile[,] m_Grid;


		/// <summary>
		/// Initializes the unit map. Must be called after GameMap has been initialized.
		/// </summary>
		public static void Initialize()
		{
			lock (m_SyncObject)
				m_Grid = new Tile[GameMap.bounds.width, GameMap.bounds.height];
		}

		/// <summary>
		/// Updates the unit map values.
		/// </summary>
		public static void Update(PlayerInfo[] playerInfos)
		{
			lock (m_SyncObject)
			{
				Array.Clear(m_Grid, 0, m_Grid.Length);

				// Loop through every unit in every player
				foreach (PlayerInfo info in playerInfos)
				{
					if (info == null)
						continue;

					foreach (UnitEx unit in info.units.GetUnits())
					{
						// Place unit on grid
						if (!unit.IsBuilding())
						{
							LOCATION gridPt = GetPointInGridSpace(unit.GetPosition());
							m_Grid[gridPt.x, gridPt.y].unitOnTile = unit;
						}
						else
						{
							// Place complete building area on grid
							MAP_RECT buildingArea = unit.GetUnitInfo().GetRect(unit.GetPosition());

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
				} // foreach playerInfo
			} // lock
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
		public static UnitEx GetUnitOnTile(LOCATION tilePosition)
		{
			tilePosition = GetPointInGridSpace(tilePosition);

			lock (m_SyncObject)
				return m_Grid[tilePosition.x, tilePosition.y].unitOnTile;
		}

		/// <summary>
		/// Returns all units in the specified area.
		/// </summary>
		public static List<UnitEx> GetUnitsInArea(MAP_RECT area)
		{
			List<UnitEx> units = new List<UnitEx>();

			for (int x=area.xMin; x < area.xMax; ++x)
			{
				for (int y=area.yMin; y < area.yMax; ++y)
				{
					LOCATION tile = new LOCATION(x,y);
					tile.ClipToMap();

					UnitEx unit = GetUnitOnTile(tile);
					if (unit != null)
						units.Add(unit);
				}
			}

			return units;
		}
	}
}
