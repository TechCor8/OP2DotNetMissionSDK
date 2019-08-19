using DotNetMissionSDK.Pathfinding;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;

namespace DotNetMissionSDK.State.Snapshot.Maps
{
	/// <summary>
	/// Represents a map of a player's command tube access.
	/// NOTE: Recommended way to access this class is through StateSnapshot.
	/// </summary>
	public class PlayerCommandMap
	{
		private struct Tile
		{
			public byte hasCommandAccess;			// Command grid. 0 - No command access, 1 - Has command access
			public StructureState buildingOnTile;
		}

		private Tile[,] m_Grid;

		private int m_PlayerID;


		public PlayerCommandMap(PlayerUnitState units, int playerID)
		{
			m_Grid = new Tile[GameMap.bounds.width, GameMap.bounds.height];

			m_PlayerID = playerID;

			// Update command grid with connection status
			foreach (StructureState cc in units.commandCenters)
			{
				Pathfinder.GetClosestValidTile(cc.position, GetTileCost, IsValidTile, false);

				LOCATION gridPoint = GetPointInGridSpace(cc.position);

				m_Grid[gridPoint.x, gridPoint.y].hasCommandAccess = 1;
			}

			// Update buildings on command grid
			foreach (StructureState building in units.GetStructures())
			{
				MAP_RECT buildingArea = building.GetRect();

				for (int x=buildingArea.xMin; x < buildingArea.xMax; ++x)
				{
					for (int y=buildingArea.yMin; y < buildingArea.yMax; ++y)
					{
						LOCATION gridPoint = new LOCATION(x, y);
						gridPoint.ClipToMap();

						gridPoint = GetPointInGridSpace(gridPoint);

						m_Grid[gridPoint.x,gridPoint.y].buildingOnTile = building;
					}
				}
			}
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
			if (m_Grid[gridPoint.x, gridPoint.y].hasCommandAccess > 0) return true;

			point.x -= 1;
			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (m_Grid[gridPoint.x, gridPoint.y].hasCommandAccess > 0) return true;

			point.x += 2;
			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (m_Grid[gridPoint.x, gridPoint.y].hasCommandAccess > 0) return true;

			point.x -= 1;
			point.y -= 1;
			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (m_Grid[gridPoint.x, gridPoint.y].hasCommandAccess > 0) return true;

			point.y += 2;
			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (m_Grid[gridPoint.x, gridPoint.y].hasCommandAccess > 0) return true;

			return false;
		}

		public bool GetClosestConnectedTile(LOCATION pt, out LOCATION closestPt)
		{
			Pathfinder.ValidTileCallback validTileCB = (int x, int y) =>
			{
				LOCATION gridPoint = GetPointInGridSpace(new LOCATION(x,y));

				return m_Grid[gridPoint.x,gridPoint.y].hasCommandAccess > 0;
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
					if (pt.x != area.xMin && pt.y != area.yMin &&		// Top left
						pt.x != area.xMax-1 && pt.y != area.yMin &&		// Top right
						pt.x != area.xMax-1 && pt.y != area.yMax-1 &&	// Bottom right
						pt.x != area.xMin && pt.y != area.yMax-1)		// Bottom left
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

		// Callback for determining tile cost
		private int GetTileCost(int x, int y)
		{
			// Out of map bounds is impassable
			if (!GameMap.bounds.Contains(x, y))
				return Pathfinder.Impassable;

			LOCATION gridPoint = GetPointInGridSpace(new LOCATION(x,y));

			// If the tile is not a tube or structure, mark it as impassable
			if (GameMap.GetCellType(x, y) != CellType.Tube0 && m_Grid[gridPoint.x,gridPoint.y].buildingOnTile == null)
				return Pathfinder.Impassable;

			// If tube or structure, mark grid as connected
			m_Grid[gridPoint.x,gridPoint.y].hasCommandAccess = 1;

			return 1;
		}

		// Callback for determining if tile is a valid place point
		private bool IsValidTile(int x, int y)
		{
			// We aren't looking for a valid tile.
			// We want to scan the whole area and get the tile connection state.
			return false;
		}

		private LOCATION GetPointInGridSpace(LOCATION pt)
		{
			pt.x = pt.x - GameMap.bounds.xMin;
			pt.y = pt.y - GameMap.bounds.yMin;

			return pt;
		}

		public List<StructureState> GetConnectedStructures(LOCATION tile)
		{
			Dictionary<int, StructureState> connectedStructures = new Dictionary<int, StructureState>();

			Pathfinder.TileCostCallback tileCostCB = (int x, int y) =>
			{
				// Out of map bounds is impassable
				if (!GameMap.bounds.Contains(x,y))
					return Pathfinder.Impassable;

				LOCATION gridPoint = GetPointInGridSpace(new LOCATION(x,y));

				StructureState building = m_Grid[gridPoint.x,gridPoint.y].buildingOnTile;

				if (GameMap.GetCellType(x, y) != CellType.Tube0 && building == null)
					return Pathfinder.Impassable;

				if (building != null)
				{
					int unitID = building.unitID;
					connectedStructures[unitID] = building;
				}

				return 1;
			};

			Pathfinder.GetClosestValidTile(tile, tileCostCB, IsValidTile, false);

			return new List<StructureState>(connectedStructures.Values);
		}
	}
}
