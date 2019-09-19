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
			private StructureState cc0,cc1,cc2,cc3,cc4,cc5,cc6,cc7;

			public void SetCommandAccess(int playerID, int isActive, StructureState cc)
			{
				m_HasCommandAccess &= (byte)~playerID;
				m_HasCommandAccess |=  (byte)(isActive << playerID);

				// Check if a CC has already claimed this tile
				// Replace CC only if assigned CC is inactive
				StructureState assignedCC = GetCommandCenter(playerID);
				if (assignedCC != null && assignedCC.isEnabled)
					return;

				switch (playerID)
				{
					case 0:		cc0 = cc;		break;
					case 1:		cc1 = cc;		break;
					case 2:		cc2 = cc;		break;
					case 3:		cc3 = cc;		break;
					case 4:		cc4 = cc;		break;
					case 5:		cc5 = cc;		break;
					case 6:		cc6 = cc;		break;
					case 7:		cc7 = cc;		break;
				}
			}

			public bool HasCommandAccess(int playerID)
			{
				return (m_HasCommandAccess & (1 << playerID)) != 0;
			}

			public StructureState GetCommandCenter(int playerID)
			{
				switch (playerID)
				{
					case 0:		return cc0;
					case 1:		return cc1;
					case 2:		return cc2;
					case 3:		return cc3;
					case 4:		return cc4;
					case 5:		return cc5;
					case 6:		return cc6;
					case 7:		return cc7;
				}
				return null;
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
					Pathfinder.GetClosestValidTile(cc.position, (x,y) => GetTileCost(playerID, cc, x,y), IsValidTile, false);

					LOCATION gridPoint = GetPointInGridSpace(cc.position);

					m_Grid[gridPoint.x, gridPoint.y].SetCommandAccess(playerID, 1, cc);
				}
			}
		}

		/// <summary>
		/// Checks if an area connects to a command center.
		/// </summary>
		/// <param name="area">The area to check.</param>
		/// <returns>True, if the area connects to a command center.</returns>
		public bool ConnectsTo(int playerID, MAP_RECT area, bool includeDisabledCCs=true)
		{
			for (int x=area.xMin; x < area.xMax; ++x)
			{
				for (int y=area.yMin; y < area.yMax; ++y)
				{
					if (ConnectsTo(playerID, new LOCATION(x,y), includeDisabledCCs))
						return true;
				}
			}
			
			return false;
		}

		public bool ConnectsTo(int playerID, LOCATION point, bool includeDisabledCCs=true)
		{
			LOCATION gridPoint;

			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (HasCommandAccess(gridPoint, playerID, includeDisabledCCs)) return true;

			point.x -= 1;
			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (HasCommandAccess(gridPoint, playerID, includeDisabledCCs)) return true;

			point.x += 2;
			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (HasCommandAccess(gridPoint, playerID, includeDisabledCCs)) return true;

			point.x -= 1;
			point.y -= 1;
			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (HasCommandAccess(gridPoint, playerID, includeDisabledCCs)) return true;

			point.y += 2;
			point.ClipToMap();
			gridPoint = GetPointInGridSpace(point);
			if (HasCommandAccess(gridPoint, playerID, includeDisabledCCs)) return true;

			return false;
		}

		private bool HasCommandAccess(LOCATION tile, int playerID, bool includeDisabledCCs)
		{
			if (includeDisabledCCs)
				return m_Grid[tile.x, tile.y].HasCommandAccess(playerID);
			else
			{
				// Only considered to have command access if command center is enabled
				StructureState cc = m_Grid[tile.x, tile.y].GetCommandCenter(playerID);
				if (cc == null)
					return false;

				return cc.isEnabled;
			}
		}

		public bool GetClosestConnectedTile(int playerID, LOCATION pt, out LOCATION closestPt)
		{
			return GetClosestConnectedTile(playerID, pt, out closestPt);
		}

		public bool GetClosestConnectedTile(int playerID, IEnumerable<LOCATION> points, out LOCATION closestPt)
		{
			Pathfinder.ValidTileCallback validTileCB = (int x, int y) =>
			{
				LOCATION gridPoint = GetPointInGridSpace(new LOCATION(x,y));

				return m_Grid[gridPoint.x,gridPoint.y].HasCommandAccess(playerID);
			};

			return Pathfinder.GetClosestValidTile(points, GetDefaultTileCost, validTileCB, out closestPt, false);
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
			LOCATION[] startPts = area.GetPoints();
			LOCATION closestPt;

			if (!GetClosestConnectedTile(playerID, startPts, out closestPt))
				return null;

			LOCATION[] path = Pathfinder.GetPath(startPts, closestPt, false, GetDefaultTileCost);
			if (path == null)
				return path;

			List<LOCATION> cullPath = new List<LOCATION>(path);

			// Remove the start and end points, which are already connected
			cullPath.RemoveAt(0);
			cullPath.RemoveAt(cullPath.Count-1);

			return cullPath.ToArray();
		}

		private int GetDefaultTileCost(int x, int y)
		{
			if (!m_StateSnapshot.tileMap.IsTilePassable(x,y))
				return Pathfinder.Impassable;

			return 1;
		}

		// Callback for determining tile cost
		private int GetTileCost(int playerID, StructureState cc, int x, int y)
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
			m_Grid[gridPoint.x,gridPoint.y].SetCommandAccess(playerID, 1, cc);

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
		/// Returns a command center connected to this tile, or null if tile is not connected.
		/// If more than one CC connects to this tile, the first one found that is enabled will be returned.
		/// </summary>
		public StructureState GetConnectedCommandCenter(int playerID, LOCATION tile)
		{
			LOCATION gridPoint;

			tile.ClipToMap();
			gridPoint = GetPointInGridSpace(tile);

			return m_Grid[gridPoint.x, gridPoint.y].GetCommandCenter(playerID);
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
