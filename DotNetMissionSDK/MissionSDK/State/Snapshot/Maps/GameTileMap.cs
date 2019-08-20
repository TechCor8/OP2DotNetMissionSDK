using DotNetMissionSDK.HFL;
using System;

namespace DotNetMissionSDK.State.Snapshot.Maps
{
	/// <summary>
	/// Represents a map of game tiles.
	/// NOTE: Recommended way to access this class is through StateSnapshot.
	/// </summary>
	public class GameTileMap
	{
		private int m_Width;
		private int m_Height;

		private int[] m_Grid;


		/// <summary>
		/// Initializes the tile map. Must be called after GameMap has been initialized.
		/// </summary>
		public GameTileMap()
		{
			m_Width = GameMap.bounds.width;
			m_Height = GameMap.bounds.height;

			m_Grid = new int[m_Width*m_Height];

			Initialize();
		}

		/// <summary>
		/// Initializes the map.
		/// NOTE: Should only be called from StateSnapshot.
		/// </summary>
		internal void Initialize()
		{
			GameMapEx.CopyTileMap(m_Grid);
		}

		private static LOCATION GetPointInGridSpace(LOCATION pt)
		{
			pt.x = pt.x - GameMap.bounds.xMin;
			pt.y = pt.y - GameMap.bounds.yMin;

			return pt;
		}

		/// <summary>
		/// Returns the cell type at the specified tile.
		/// </summary>
		public CellType GetCellType(LOCATION tilePosition)
		{
			tilePosition = GetPointInGridSpace(tilePosition);

			return (CellType)m_Grid[tilePosition.x + tilePosition.y * m_Width];
		}

		public bool IsTilePassable(int x, int y)
		{
			return IsTilePassable(new LOCATION(x,y));
		}

		public bool IsTilePassable(LOCATION tile)
		{
			// If tile is out of bounds, it is not passable
			if (!tile.IsInMapBounds())
				return false;

			// Check for passable tile types
			switch (GetCellType(tile))
			{
				case CellType.FastPassible1:
				case CellType.SlowPassible1:
				case CellType.SlowPassible2:
				case CellType.MediumPassible1:
				case CellType.MediumPassible2:
				case CellType.FastPassible2:
				case CellType.DozedArea:
				case CellType.Rubble:
				case CellType.Tube0:
					return true;
			}

			return false;
		}

		public int GetTileMovementCost(int x, int y)
		{
			return GetTileMovementCost(new LOCATION(x,y));
		}

		public int GetTileMovementCost(LOCATION tile)
		{
			if (!tile.IsInMapBounds())
				return 0;

			switch (GetCellType(tile))
			{
				case CellType.FastPassible1:		return 2;
				case CellType.SlowPassible1:		return 4;
				case CellType.SlowPassible2:		return 4;
				case CellType.MediumPassible1:		return 3;
				case CellType.MediumPassible2:		return 3;
				case CellType.FastPassible2:		return 2;
				case CellType.DozedArea:			return 1;
				case CellType.Rubble:				return 2;
				case CellType.Tube0:				return 1;
			}

			return 0;
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
