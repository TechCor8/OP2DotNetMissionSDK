using System.Runtime.InteropServices;

namespace DotNetMissionSDK.HFL
{
	/// <summary>
	/// Extension class for GameMap.
	/// </summary>
	public class GameMapEx : GameMap
	{
		public static uint GetTileMappingIndex(int tileX, int tileY)										{ return GameMapEx_GetTileMappingIndex(tileX, tileY);						}
		public static uint GetTileUnitIndex(int tileX, int tileY)											{ return GameMapEx_GetTileUnitIndex(tileX, tileY);							}
		public static uint GetTileFlags(int tileX, int tileY)												{ return GameMapEx_GetTileFlags(tileX, tileY);								}
		public static void SetTile(int tileX, int tileY, uint mappingIndex, uint unitIndex, uint flags)		{ GameMapEx_SetTile(tileX, tileY, mappingIndex, unitIndex, flags);			}

		public static int GetMapWidth(int tileX, int tileY)													{ return GameMapEx_GetMapWidth();											}
		public static int GetMapHeight(int tileX, int tileY)												{ return GameMapEx_GetMapHeight();											}

		public static int GetNumUnits(int tileX, int tileY)													{ return GameMapEx_GetNumUnits();											}
		public static int LoadMap(int tileX, int tileY, string fileName)									{ return GameMapEx_LoadMap(fileName);										}

		public static void CopyTileMap(int[] tileMapBuffer)
		{
			GameMapEx_CopyTileMap(tileMapBuffer, bounds.xMin, bounds.xMax, bounds.yMin, bounds.yMax);
		}

		[DllImport("DotNetInterop.dll")] private static extern uint GameMapEx_GetTileMappingIndex(int x, int y);
		[DllImport("DotNetInterop.dll")] private static extern uint GameMapEx_GetTileUnitIndex(int x, int y);
		[DllImport("DotNetInterop.dll")] private static extern uint GameMapEx_GetTileFlags(int x, int y);
		[DllImport("DotNetInterop.dll")] private static extern void GameMapEx_SetTile(int x, int y, uint mappingIndex, uint unitIndex, uint flags);

		[DllImport("DotNetInterop.dll")] private static extern int GameMapEx_GetMapWidth();
		[DllImport("DotNetInterop.dll")] private static extern int GameMapEx_GetMapHeight();

		[DllImport("DotNetInterop.dll")] private static extern int GameMapEx_GetNumUnits();
		[DllImport("DotNetInterop.dll")] private static extern int GameMapEx_LoadMap(string fileName);

		[DllImport("DotNetInterop.dll")] private static extern void GameMapEx_CopyTileMap(int[] tileMap, int xMin, int xMax, int yMin, int yMax);
	}
}
