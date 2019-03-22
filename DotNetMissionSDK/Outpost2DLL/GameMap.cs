using System;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK
{
	/// <summary>
	/// This class is used to control the displayed tiles on the map as well as the movement/passability characteristics of the terrain.
	/// <para>
	/// Note: All members are static.
	/// </para>
	/// </summary>
	public class GameMap
	{
		public const int MaxMapSize = 512;

		public static bool doesWrap												{ get; private set; }
		public static int width													{ get; private set; }
		public static int height												{ get; private set; }
		public static MAP_RECT area												{ get; private set; } // In "map coordinates" Ex: (32,0,160,127)

		// [Get]
		public static CellType GetCellType(int x, int y)						{ return (CellType)GameMap_GetCellType(x, y);				}
		public static int GetTile(int x, int y)									{ return GameMap_GetTile(x, y);								}

		// [Set]
		public static void InitialSetTile(int x, int y, int tileIndex)			{ GameMap_InitialSetTile(x, y, tileIndex);					}
		public static void SetTile(int x, int y, int tileIndex)					{ GameMap_SetTile(x, y, tileIndex);							}
		public static void SetCellType(int x, int y, int cellIndex)				{ GameMap_SetCellType(x, y, cellIndex);						}
		public static void SetLavaPossible(int x, int y, bool bLavaPossible)	{ GameMap_SetLavaPossible(x, y, bLavaPossible ? 1 : 0);		}
		public static void SetVirusUL(int x, int y, bool bBlightPresent)		{ GameMap_SetVirusUL(x, y, bBlightPresent ? 1 : 0);			}
		public static void SetInitialLightLevel(int lightPosition)				{ GameMap_SetInitialLightLevel(lightPosition);				}

		// [Get]
		[DllImport("DotNetInterop.dll")] private static extern int GameMap_GetCellType(int x, int y);
		[DllImport("DotNetInterop.dll")] private static extern int GameMap_GetTile(int x, int y);

		// [Set]
		[DllImport("DotNetInterop.dll")] private static extern void GameMap_InitialSetTile(int x, int y, int tileIndex);
		[DllImport("DotNetInterop.dll")] private static extern void GameMap_SetTile(int x, int y, int tileIndex);
		[DllImport("DotNetInterop.dll")] private static extern void GameMap_SetCellType(int x, int y, int cellIndex);
		[DllImport("DotNetInterop.dll")] private static extern void GameMap_SetLavaPossible(int x, int y, int bLavaPossible);
		[DllImport("DotNetInterop.dll")] private static extern void GameMap_SetVirusUL(int x, int y, int bBlightPresent);
		[DllImport("DotNetInterop.dll")] private static extern void GameMap_SetInitialLightLevel(int lightPosition);

		public static bool IsTilePassable(int x, int y)
		{
			return IsTilePassable(new LOCATION(x,y));
		}

		public static bool IsTilePassable(LOCATION tile)
		{
			// If tile is out of bounds, it is not passable
			if (!tile.IsInMapBounds())
				return false;

			// Check for passable tile types
			switch (GetCellType(tile.x,tile.y))
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

		/// <summary>
		/// Calculates map constants.
		/// </summary>
		public static void Initialize()
		{
			// ************ WARNING *************
			// Width has been oversized on some maps (+1). The reason is unknown.
			// Recommended to verify whether a bug actually exists.
			// **********************************
			
			// Calculate map size and determine if map wraps
			// Clip to top left
			LOCATION mapSize = new LOCATION();
			mapSize.ClipToMap();

			// Initialize to top left corner
			area = new MAP_RECT(mapSize, mapSize);
			
			for (int i=0; i < MaxMapSize; ++i)
			{
				int x = mapSize.x;
				int y = mapSize.y;

				++mapSize.x;
				++mapSize.y;

				mapSize.ClipToMap();

				// Clamped map will pull mapSize.x back to x.
				// The max value is the current and previous value.
				if (mapSize.x == x)
				{
					area.maxX = x;
					doesWrap = false;
				}

				// Unclamped map will wrap mapSize.x back to 0.
				// The max value is the previous value.
				if (mapSize.x == 0)
				{
					area.maxX = x;
					doesWrap = true;
				}

				// Y is always clamped.
				// When mapSize.y is pulled back to y, and we have our clamped or wrapped values, end search.
				if ((mapSize.x == x || doesWrap) && mapSize.y == y)
				{
					area.maxY = y;
					break;
				}
			}

			width = area.width;
			height = area.height;

			Console.WriteLine("MapWidth: " + width);
			Console.WriteLine("MapHeight: " + height);
			Console.WriteLine("MapArea: " + area);
		}
	}
}