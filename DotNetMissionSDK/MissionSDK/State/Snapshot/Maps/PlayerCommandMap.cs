using DotNetMissionSDK.Pathfinding;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;

namespace DotNetMissionSDK.State.Snapshot.Maps
{
	/// <summary>
	/// Represents a map of players' command tube access.
	/// NOTE: Recommended way to access this class is through StateSnapshot.
	/// </summary>
	public class PlayerCommandMap
	{
		private struct Tile
		{
			/// <summary>
			/// Bit flags for each player. If flag is set, player has command access.
			/// </summary>
			private byte m_HasCommandAccess;

			public void SetCommandAccess(int playerID, int isActive)
			{
				m_HasCommandAccess &= (byte)~playerID;
				m_HasCommandAccess |=  (byte)(isActive << playerID);
			}

			public bool HasCommandAccess(int playerID)
			{
				return (m_HasCommandAccess & (1 << playerID)) != 0;
			}
		}

		private Tile[,] m_Grid;

		private StateSnapshot m_StateSnapshot;
		

		public PlayerCommandMap() { }

		/// <summary>
		/// Initializes the map. Must be called after GameMap has been initialized.
		/// </summary>
		public PlayerCommandMap(StateSnapshot stateSnapshot)
		{
			m_Grid = new Tile[GameMap.bounds.width, GameMap.bounds.height];

			Initialize(stateSnapshot);
		}

		/// <summary>
		/// Initializes the map.
		/// NOTE: Should only be called from StateSnapshot.
		/// </summary>
		internal void Initialize(StateSnapshot stateSnapshot)
		{
			m_StateSnapshot = stateSnapshot;

			// Update command grid with connection status
			for (int playerID=0; playerID < stateSnapshot.players.Count; ++playerID)
			{
				foreach (StructureState cc in stateSnapshot.players[playerID].units.commandCenters)
				{
					Pathfinder.GetClosestValidTile(cc.position, (x,y) => GetTileCost(playerID, x,y), IsValidTile, false);

					LOCATION gridPoint = GetPointInGridSpace(cc.position);

					m_Grid[gridPoint.x, gridPoint.y].SetCommandAccess(playerID, 1);
				}
			}
		}

		/// <summary>
		/// Checks if an area connects to a command center.
		/// </summary>
		/// <param name="area">The area to check.</param>
		/// <returns>True, if the area connects to a command center.</returns>
		public bool ConnectsTo(int playerID, MAP_RECT area)
		{
			for (int x=area.xMin; x < area.xMax; ++x)
			{
				for (int y=area.yMin; y < area.yMax; ++y)
				{
					if (ConnectsTo(playerID, new LOCATION(x,y)))
						return true;
				}
			}
			
			return false;
		}

		public bool ConnectsTo(int playerID, LOCATION point)
		{
			LOCATION gridPoint;

			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (m_Grid[gridPoint.x, gridPoint.y].HasCommandAccess(playerID)) return true;

			point.x -= 1;
			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (m_Grid[gridPoint.x, gridPoint.y].HasCommandAccess(playerID)) return true;

			point.x += 2;
			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (m_Grid[gridPoint.x, gridPoint.y].HasCommandAccess(playerID)) return true;

			point.x -= 1;
			point.y -= 1;
			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (m_Grid[gridPoint.x, gridPoint.y].HasCommandAccess(playerID)) return true;

			point.y += 2;
			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (m_Grid[gridPoint.x, gridPoint.y].HasCommandAccess(playerID)) return true;

			return false;
		}

		public bool GetClosestConnectedTile(int playerID, LOCATION pt, out LOCATION closestPt)
		{
			Pathfinder.ValidTileCallback validTileCB = (int x, int y) =>
			{
				LOCATION gridPoint = GetPointInGridSpace(new LOCATION(x,y));

				return m_Grid[gridPoint.x,gridPoint.y].HasCommandAccess(playerID);
			};

			return Pathfinder.GetClosestValidTile(pt, GetDefaultTileCost, validTileCB, out closestPt, false);
		}

		public LOCATION[] GetPathToClosestConnectedTile(int playerID, LOCATION pt)
		{
			LOCATION closestPt;

			if (!GetClosestConnectedTile(playerID, pt, out closestPt))
				return null;

			return Pathfinder.GetPath(pt, closestPt, false, GetDefaultTileCost);
		}

		public LOCATION[] GetPathToClosestConnectedTile(int playerID, MAP_RECT area)
		{
			LOCATION centerPt = new LOCATION((area.xMin+area.xMax)/2, (area.yMin+area.yMax)/2);
			LOCATION closestPt;

			if (!GetClosestConnectedTile(playerID, centerPt, out closestPt))
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
			if (!m_StateSnapshot.tileMap.IsTilePassable(x,y))
				return Pathfinder.Impassable;

			return 1;
		}

		// Callback for determining tile cost
		private int GetTileCost(int playerID, int x, int y)
		{
			// Out of map bounds is impassable
			if (!GameMap.bounds.Contains(x, y))
				return Pathfinder.Impassable;

			LOCATION tilePosition = new LOCATION(x,y);
			LOCATION gridPoint = GetPointInGridSpace(tilePosition);

			StructureState building = m_StateSnapshot.unitMap.GetUnitOnTile(tilePosition) as StructureState;

			// If the tile is not a tube or structure, mark it as impassable
			if (m_StateSnapshot.tileMap.GetCellType(tilePosition) != CellType.Tube0 && building == null)
				return Pathfinder.Impassable;

			// If tube or structure, mark grid as connected
			m_Grid[gridPoint.x,gridPoint.y].SetCommandAccess(playerID, 1);

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

		/// <summary>
		/// Gets all structures connected to this tile through the command map.
		/// If the tile does not have command access, no structures will be returned.
		/// </summary>
		public List<StructureState> GetConnectedStructures(int playerID, LOCATION tile)
		{
			Dictionary<int, StructureState> connectedStructures = new Dictionary<int, StructureState>();

			Pathfinder.TileCostCallback tileCostCB = (int x, int y) =>
			{
				// Out of map bounds is impassable
				if (!GameMap.bounds.Contains(x,y))
					return Pathfinder.Impassable;

				LOCATION tilePosition = new LOCATION(x,y);
				LOCATION gridPoint = GetPointInGridSpace(new LOCATION(x,y));

				StructureState building = m_StateSnapshot.unitMap.GetUnitOnTile(tilePosition) as StructureState;

				if (m_StateSnapshot.tileMap.GetCellType(tilePosition) != CellType.Tube0 && building == null)
					return Pathfinder.Impassable;

				if (building != null && building.ownerID == playerID)
				{
					int unitID = building.unitID;
					connectedStructures[unitID] = building;
				}

				return 1;
			};

			Pathfinder.GetClosestValidTile(tile, tileCostCB, IsValidTile, false);

			return new List<StructureState>(connectedStructures.Values);
		}

		/// <summary>
		/// Clears the map.
		/// NOTE: Should only be called from StateSnapshot.
		/// </summary>
		internal void Clear()
		{
			Array.Clear(m_Grid, 0, m_Grid.Length);
		}
	}
}
