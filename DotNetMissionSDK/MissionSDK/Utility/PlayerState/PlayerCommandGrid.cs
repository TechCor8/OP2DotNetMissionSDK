using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Pathfinding;
using System;

namespace DotNetMissionSDK.Utility.PlayerState
{
	public class PlayerCommandGrid
	{
		private byte[,] m_Grid;

		private int m_PlayerID;


		public PlayerCommandGrid()
		{
			m_Grid = new byte[GameMap.bounds.width, GameMap.bounds.height];
		}

		/// <summary>
		/// Checks if an area connects to a command center.
		/// </summary>
		/// <param name="area">The area to check.</param>
		/// <returns>True, if the area connects to a command center.</returns>
		public bool ConnectsTo(MAP_RECT area)
		{
			for (int x=area.xMin; x < area.xMax; ++x)
			{
				for (int y=area.yMin; y < area.yMax; ++y)
				{
					if (ConnectsTo(new LOCATION(x,y)))
						return true;
				}
			}
			
			return false;
		}

		public bool ConnectsTo(LOCATION point)
		{
			point.ClipToMap();
			if (m_Grid[point.x, point.y] > 0) return true;

			point.x -= 1;
			point.ClipToMap();
			if (m_Grid[point.x, point.y] > 0) return true;

			point.x += 2;
			point.ClipToMap();
			if (m_Grid[point.x, point.y] > 0) return true;

			point.x -= 1;
			point.y -= 1;
			point.ClipToMap();
			if (m_Grid[point.x, point.y] > 0) return true;

			point.y += 2;
			point.ClipToMap();
			if (m_Grid[point.x, point.y] > 0) return true;

			return false;
		}

		public void Update(PlayerUnitList units, int playerID)
		{
			m_PlayerID = playerID;

			Array.Clear(m_Grid, 0, m_Grid.Length);

			foreach (UnitEx cc in units.commandCenters)
			{
				LOCATION temp;
				LOCATION ccPos = cc.GetPosition();
				Pathfinder.GetClosestValidTile(ccPos, GetTileCost, IsValidTile, out temp);

				m_Grid[ccPos.x, ccPos.y] = 1;
			}
		}

		// Callback for determining tile cost
		private int GetTileCost(int x, int y)
		{
			// If the tile is not a tube or structure, mark it as impassable
			if (GameMap.GetCellType(x, y) != CellType.Tube0 && !IsBuildingOnTile(x, y))
				return Pathfinder.Impassable;

			// If tube or structure, mark grid as connected
			m_Grid[x,y] = 1;

			return 1;
		}

		// Callback for determining if tile is a valid place point
		private bool IsValidTile(int x, int y)
		{
			// We aren't looking for a valid tile.
			// We want to scan the whole area and get the tile connection state.
			return false;
		}

		private bool IsBuildingOnTile(int x, int y)
		{
			foreach (UnitEx building in new PlayerAllBuildingEnum(m_PlayerID))
			{
				MAP_RECT buildingArea = building.GetUnitInfo().GetRect(building.GetPosition());

				if (buildingArea.Contains(x,y))
					return true;
			}

			return false;
		}
	}
}
