// Note: This class is used to control the displayed tiles on the map as well
//		 as the movement/passability characteristics of the terrain.
// Note: All members are static. Prefix class name and :: to access these functions.
//		 Example: GameMap::SetInitialLightLevel(0);

using System;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK
{
	public class GameMap
	{
		// [Get]
		public static int GetCellType(int x, int y)								{ return GameMap_GetCellType(x, y);							}
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
	}
}