using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Snapshot.Units;
using System;

namespace DotNetMissionSDK.State.Snapshot.Maps
{
	/// <summary>
	/// Represents a map of all players' combat strength.
	/// NOTE: Recommended way to access this class is through StateSnapshot.
	/// </summary>
	public class PlayerStrengthMap
	{
		private struct Tile
		{
			public int[] playerStrength;		// Each player's strength at this tile
		}

		private const int PlayerCount = 8;

		private Tile[,] m_Grid;


		/// <summary>
		/// Initializes the strength map. Must be called after GameMap has been initialized.
		/// </summary>
		public PlayerStrengthMap(PlayerState[] playerStates)
		{
			m_Grid = new Tile[GameMap.bounds.width, GameMap.bounds.height];

			Initialize(playerStates);
		}

		/// <summary>
		/// Initializes the map.
		/// NOTE: Should only be called from StateSnapshot.
		/// </summary>
		internal void Initialize(PlayerState[] playerStates)
		{
			foreach (PlayerState playerState in playerStates)
				UpdatePlayerStrength(playerState);
		}

		private void UpdatePlayerStrength(PlayerState playerState)
		{
			if (playerState == null)
				return;

			// Update guard posts
			foreach (UnitState unit in playerState.units.guardPosts)
				UpdateUnitStrength(GameState.units[unit.unitID], playerState.playerID);

			// Update units
			foreach (UnitState unit in playerState.units.lynx)
				UpdateUnitStrength(GameState.units[unit.unitID], playerState.playerID);

			foreach (UnitState unit in playerState.units.panthers)
				UpdateUnitStrength(GameState.units[unit.unitID], playerState.playerID);

			foreach (UnitState unit in playerState.units.tigers)
				UpdateUnitStrength(GameState.units[unit.unitID], playerState.playerID);

			foreach (UnitState unit in playerState.units.scorpions)
				UpdateUnitStrength(GameState.units[unit.unitID], playerState.playerID);
		}

		private void UpdateUnitStrength(UnitEx unit, int ownerID)
		{
			UnitInfo unitInfo = unit.GetUnitInfo();
			map_id weaponType = unit.GetWeapon();
			if (unit.GetUnitType() == map_id.Scorpion) weaponType = map_id.EnergyCannon; // Scorpion GetWeapon returns None
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
		public int GetPlayerStrength(int playerID, int gridX, int gridY)
		{
			LOCATION pt = GetPointInGridSpace(new LOCATION(gridX, gridY));

			if (m_Grid[pt.x,pt.y].playerStrength == null)
				return 0;

			return m_Grid[pt.x,pt.y].playerStrength[playerID];
		}

		/// <summary>
		/// Returns the specified players' strength at the specified tile coordinates.
		/// </summary>
		public int GetPlayerStrength(int[] playerIDs, int gridX, int gridY)
		{
			LOCATION pt = GetPointInGridSpace(new LOCATION(gridX, gridY));

			if (m_Grid[pt.x,pt.y].playerStrength == null)
				return 0;

			int totalStrength = 0;
			for (int i=0; i < playerIDs.Length; ++i)
				totalStrength += m_Grid[pt.x,pt.y].playerStrength[playerIDs[i]];

			return totalStrength;
		}

		/// <summary>
		/// Returns the total strength of all players at the specified tile coordinates.
		/// </summary>
		public int GetTotalStrength(int gridX, int gridY)
		{
			LOCATION pt = GetPointInGridSpace(new LOCATION(gridX, gridY));

			if (m_Grid[pt.x,pt.y].playerStrength == null)
				return 0;

			int totalStrength = 0;
			for (int i=0; i < m_Grid[pt.x,pt.y].playerStrength.Length; ++i)
				totalStrength += m_Grid[pt.x,pt.y].playerStrength[i];

			return totalStrength;
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
