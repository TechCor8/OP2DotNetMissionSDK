using DotNetMissionSDK.HFL;
using System;

namespace DotNetMissionSDK.Utility.Maps
{
	/// <summary>
	/// Represents a map of all players' combat strength. 
	/// </summary>
	public class PlayerStrengthMap
	{
		private struct Tile
		{
			public int[] playerStrength;		// Each player's strength at this tile
		}

		private const int PlayerCount = 8;

		private static Tile[,] m_Grid;


		/// <summary>
		/// Initializes the strength map. Must be called after GameMap has been initialized.
		/// </summary>
		public static void Initialize()
		{
			m_Grid = new Tile[GameMap.bounds.width, GameMap.bounds.height];
		}

		/// <summary>
		/// Updates the strength map values.
		/// </summary>
		public static void Update(PlayerInfo[] playerInfos)
		{
			Array.Clear(m_Grid, 0, m_Grid.Length);

			foreach (PlayerInfo info in playerInfos)
				UpdatePlayerStrength(info);
		}

		private static void UpdatePlayerStrength(PlayerInfo info)
		{
			if (info == null)
				return;

			// Update guard posts
			foreach (UnitEx unit in info.units.guardPosts)
				UpdateUnitStrength(unit, info.player.playerID);

			// Update units
			foreach (UnitEx unit in info.units.lynx)
				UpdateUnitStrength(unit, info.player.playerID);

			foreach (UnitEx unit in info.units.panthers)
				UpdateUnitStrength(unit, info.player.playerID);

			foreach (UnitEx unit in info.units.tigers)
				UpdateUnitStrength(unit, info.player.playerID);

			foreach (UnitEx unit in info.units.scorpions)
				UpdateUnitStrength(unit, info.player.playerID);
		}

		private static void UpdateUnitStrength(UnitEx unit, int ownerID)
		{
			UnitInfo unitInfo = unit.GetUnitInfo();
			map_id weaponType = unit.GetWeapon();
			UnitInfo weaponInfo = new UnitInfo(weaponType);
			LOCATION position = unit.GetPosition();
			int weaponRange = weaponInfo.GetWeaponRange(ownerID);

			int weaponStrength = weaponInfo.GetWeaponStrength();

			// Apply strength to all tiles in range
			for (int x=position.x-weaponRange; x < position.x+weaponRange; ++x)
			{
				for (int y=position.y-weaponRange; y < position.y+weaponRange; ++y)
				{
					// Get tile position
					LOCATION pt = new LOCATION(x,y);
					pt.ClipToMap();

					LOCATION gridPt = GetPointInGridSpace(pt);

					// If player strength was not initialized for ths tile, initialize it
					if (m_Grid[gridPt.x,gridPt.y].playerStrength == null)
						m_Grid[gridPt.x,gridPt.y].playerStrength = new int[PlayerCount];

					// Add strength for tile for this player
					m_Grid[gridPt.x,gridPt.y].playerStrength[ownerID] += weaponStrength;
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
		/// Returns the specified player's strength at the specified tile coordinates.
		/// </summary>
		public static int GetPlayerStrength(int playerID, int gridX, int gridY)
		{
			if (m_Grid[gridX,gridY].playerStrength == null)
				return 0;

			return m_Grid[gridX,gridY].playerStrength[playerID];
		}

		/// <summary>
		/// Returns the specified players' strength at the specified tile coordinates.
		/// </summary>
		public static int GetPlayerStrength(int[] playerIDs, int gridX, int gridY)
		{
			if (m_Grid[gridX,gridY].playerStrength == null)
				return 0;

			int totalStrength = 0;
			for (int i=0; i < playerIDs.Length; ++i)
				totalStrength += m_Grid[gridX,gridY].playerStrength[playerIDs[i]];

			return totalStrength;
		}

		/// <summary>
		/// Returns the total strength of all players at the specified tile coordinates.
		/// </summary>
		public static int GetTotalStrength(int gridX, int gridY)
		{
			if (m_Grid[gridX,gridY].playerStrength == null)
				return 0;

			int totalStrength = 0;
			for (int i=0; i < m_Grid[gridX,gridY].playerStrength.Length; ++i)
				totalStrength += m_Grid[gridX,gridY].playerStrength[i];

			return totalStrength;
		}
	}
}
