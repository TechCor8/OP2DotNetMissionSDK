using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Pathfinding;
using System;
using System.Collections.Generic;

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
			LOCATION gridPoint;

			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (m_Grid[gridPoint.x, gridPoint.y] > 0) return true;

			point.x -= 1;
			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (m_Grid[gridPoint.x, gridPoint.y] > 0) return true;

			point.x += 2;
			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (m_Grid[gridPoint.x, gridPoint.y] > 0) return true;

			point.x -= 1;
			point.y -= 1;
			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (m_Grid[gridPoint.x, gridPoint.y] > 0) return true;

			point.y += 2;
			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (m_Grid[gridPoint.x, gridPoint.y] > 0) return true;

			return false;
		}

		public bool GetClosestConnectedTile(LOCATION pt, out LOCATION closestPt)
		{
			Pathfinder.ValidTileCallback validTileCB = (int x, int y) =>
			{
				LOCATION gridPoint = GetPointInGridSpace(new LOCATION(x,y));

				return m_Grid[gridPoint.x,gridPoint.y] > 0;
			};

			return Pathfinder.GetClosestValidTile(pt, GetDefaultTileCost, validTileCB, out closestPt, false);
		}

		public LOCATION[] GetPathToClosestConnectedTile(LOCATION pt)
		{
			LOCATION closestPt;

			if (!GetClosestConnectedTile(pt, out closestPt))
				return null;

			return Pathfinder.GetPath(pt, closestPt, false, GetDefaultTileCost);
		}

		public LOCATION[] GetPathToClosestConnectedTile(MAP_RECT area)
		{
			LOCATION centerPt = new LOCATION((area.xMin+area.xMax)/2, (area.yMin+area.yMax)/2);
			LOCATION closestPt;

			if (!GetClosestConnectedTile(centerPt, out closestPt))
				return null;

			LOCATION[] path = Pathfinder.GetPath(closestPt, centerPt, false, GetDefaultTileCost);
			if (path == null)
				return path;

			// Remove excess tubing. If path is adjacent to area, we don't need anything after it.
			List<LOCATION> cullPath = new List<LOCATION>(path);

			area.Inflate(1,1);

			for (int i=0; i < cullPath.Count; ++i)
			{
				LOCATION pt = cullPath[i];

				if (area.Contains(pt))
				{
					if (pt.x != area.xMin && pt.y != area.yMin &&	// Top left
						pt.x != area.xMax && pt.y != area.yMin &&	// Top right
						pt.x != area.xMax && pt.y != area.yMax &&	// Bottom right
						pt.x != area.xMin && pt.y != area.yMax)		// Bottom left
					{
						++i;
						cullPath.RemoveRange(i, cullPath.Count-i);
					}
				}
			}

			return cullPath.ToArray();
		}

		private int GetDefaultTileCost(int x, int y)
		{
			if (!GameMap.IsTilePassable(x,y))
				return Pathfinder.Impassable;

			return 1;
		}

		public void Update(PlayerUnitList units, int playerID)
		{
			m_PlayerID = playerID;

			Array.Clear(m_Grid, 0, m_Grid.Length);

			foreach (UnitEx cc in units.commandCenters)
			{
				LOCATION temp;
				LOCATION ccPos = cc.GetPosition();
				Pathfinder.GetClosestValidTile(ccPos, GetTileCost, IsValidTile, out temp, false);

				LOCATION gridPoint = GetPointInGridSpace(ccPos);

				m_Grid[gridPoint.x, gridPoint.y] = 1;
			}
		}

		// Callback for determining tile cost
		private int GetTileCost(int x, int y)
		{
			// If the tile is not a tube or structure, mark it as impassable
			if (GameMap.GetCellType(x, y) != CellType.Tube0 && !IsBuildingOnTile(x, y))
				return Pathfinder.Impassable;

			// If tube or structure, mark grid as connected
			LOCATION gridPoint = GetPointInGridSpace(new LOCATION(x,y));

			m_Grid[gridPoint.x,gridPoint.y] = 1;

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

		private UnitEx GetBuildingOnTile(int x, int y)
		{
			foreach (UnitEx building in new PlayerAllBuildingEnum(m_PlayerID))
			{
				MAP_RECT buildingArea = building.GetUnitInfo().GetRect(building.GetPosition());

				if (buildingArea.Contains(x,y))
					return building;
			}

			return null;
		}

		private LOCATION GetPointInGridSpace(LOCATION pt)
		{
			pt.x = pt.x - GameMap.bounds.xMin;
			pt.y = pt.y - GameMap.bounds.yMin;

			return pt;
		}

		public List<UnitEx> GetConnectedStructures(LOCATION tile)
		{
			List<UnitEx> connectedStructures = new List<UnitEx>();

			Pathfinder.TileCostCallback tileCostCB = (int x, int y) =>
			{
				UnitEx building = GetBuildingOnTile(x, y);

				if (GameMap.GetCellType(x, y) != CellType.Tube0 && building == null)
					return Pathfinder.Impassable;
				
				if (building != null)
					connectedStructures.Add(building);

				return 1;
			};

			Pathfinder.GetClosestValidTile(tile, tileCostCB, IsValidTile, out _, false);

			return connectedStructures;
		}
	}
}
