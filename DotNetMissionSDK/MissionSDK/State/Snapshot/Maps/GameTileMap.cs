
namespace DotNetMissionSDK.State.Snapshot.Maps
{
	/// <summary>
	/// Represents a map of game tiles.
	/// NOTE: Recommended way to access this class is through StateSnapshot.
	/// </summary>
	public class GameTileMap
	{
		private struct Tile
		{
			public CellType cellType;
		}

		private Tile[,] m_Grid;


		/// <summary>
		/// Initializes the tile map. Must be called after GameMap has been initialized.
		/// </summary>
		public GameTileMap()
		{
			m_Grid = new Tile[GameMap.bounds.width, GameMap.bounds.height];
			//Array.Clear(m_Grid, 0, m_Grid.Length);

			for (int x=GameMap.bounds.xMin; x < GameMap.bounds.xMax; ++x)
			{
				for (int y=GameMap.bounds.yMin; y < GameMap.bounds.yMax; ++y)
				{
					LOCATION tile = GetPointInGridSpace(new LOCATION(x,y));
					m_Grid[tile.x,tile.y].cellType = GameMap.GetCellType(x,y);
				}
			}
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

			return m_Grid[tilePosition.x, tilePosition.y].cellType;
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
	}
}
