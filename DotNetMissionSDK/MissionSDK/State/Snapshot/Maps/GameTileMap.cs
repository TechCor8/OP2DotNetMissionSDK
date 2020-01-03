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
		public class TileMask
		{
			public const uint CellType			= 0x1F;			// Cell type of this tile
			public const uint TileIndex			= 0xFFE0;		// Mapping index (tile graphics to use)
			public const uint UnitIndex			= 0x7FF0000;	// Index of unit on this tile
			public const uint Lava				= 0x8000000;	// true if lava is on the tile
			public const uint LavaPossible		= 0x10000000;	// true if lava can flow on the tile
			public const uint Expand			= 0x2000000;	// true if lava / microbe is expanding to this tile
			public const uint Microbe			= 0x4000000;	// true if microbe is on the tile
			public const uint WallOrBuilding	= 0x8000000;	// true if a wall or building is on the tile
		}

		private int m_Width;
		private int m_Height;

		private uint[] m_Grid;


		/// <summary>
		/// Initializes the tile map. Must be called after GameMap has been initialized.
		/// </summary>
		public GameTileMap()
		{
			m_Width = GameMapEx.GetMapWidth();
			m_Height = GameMapEx.GetMapHeight();

			m_Grid = new uint[m_Width*m_Height];

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

		/// <summary>
		/// Returns the cell type at the specified tile.
		/// </summary>
		public CellType GetCellType(LOCATION tilePosition)
		{
			int x = GameMap.doesWrap ? tilePosition.x % m_Width : tilePosition.x;

			const int columnWidth = 32;
			int column = x / columnWidth;
			int columnStartIndex = column * (m_Height * columnWidth);
			int tileIndex = columnStartIndex + (x % columnWidth) + (tilePosition.y * columnWidth);

			return (CellType)(m_Grid[tileIndex] & TileMask.CellType);
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
