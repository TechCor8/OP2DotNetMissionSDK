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
		public static MAP_RECT bounds											{ get; private set; } // In "map coordinates" Ex: (32,0,160,127)

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

		public static int GetTileMovementCost(int x, int y)
		{
			return GetTileMovementCost(new LOCATION(x,y));
		}

		public static int GetTileMovementCost(LOCATION tile)
		{
			if (!tile.IsInMapBounds())
				return 0;

			switch (GetCellType(tile.x,tile.y))
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
		/// Calculates map constants.
		/// </summary>
		public static void Initialize()
		{
			// Calculate the map bounds
			LOCATION min = new LOCATION(-1,-1);
			LOCATION max;

			min = _ClipToMap(min);

			// Clamped maps set minimum to 32.
			doesWrap = min.x != 32;

			if (doesWrap)
			{
				min = new LOCATION(0,0);
				max = new LOCATION(512,256);
			}
			else
			{
				max = new LOCATION(5000,5000);
				max = _ClipToMap(max);

				// NOTE: OP2 clipping includes the max exclusive value for the x-axis.
				++max.y;
			}

			bounds = new MAP_RECT(min,max-min);

			Console.WriteLine("MapWrap: " + doesWrap);
			Console.WriteLine("MapWidth: " + bounds.width);
			Console.WriteLine("MapHeight: " + bounds.height);
			Console.WriteLine("MapBounds: " + bounds);
		}

		/// <summary>
		/// If the map wraps, the coordinates will roll over. Otherwise, the coordinates will clamp to the min/max of the map.
		/// <para>Pulled from OP2 clip rect. Do not use elsewhere in the SDK.</para>
		/// </summary>
		private static LOCATION _ClipToMap(LOCATION pos)
		{
			long result = LOCATION_Clip(pos.x,pos.y);
			pos.x = (int)(result & uint.MaxValue);
			pos.y = (int)(result >> 32);
			return pos;
		}

		[DllImport("DotNetInterop.dll")] private static extern long LOCATION_Clip(int x, int y);
	}
}