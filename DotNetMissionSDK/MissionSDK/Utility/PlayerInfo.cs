using DotNetMissionSDK.Triggers;
using DotNetMissionSDK.Utility.PlayerState;
using System;

namespace DotNetMissionSDK.Utility
{
	/// <summary>
	/// Maintains info about a player's state.
	/// </summary>
	public class PlayerInfo : IDisposable
	{
		private SaveData m_SaveData;		// The save data object to store persistent player state

		public Player player					{ get; private set; }

		/// <summary>
		/// Contains lists of player units by type.
		/// Tracks starship module counts.
		/// </summary>
		public PlayerUnitList units				{ get; private set; }

		public PlayerCommandGrid commandGrid	{ get; private set; }


		/// <summary>
		/// Creates a new player info object. There should only be one per player.
		/// </summary>
		/// <param name="triggerManager">The trigger manager for tracking player events.</param>
		/// <param name="player">The player that will have its state tracked.</param>
		/// <param name="saveData">The global save object for storing persistent state.</param>
		public PlayerInfo(TriggerManager triggerManager, Player player, SaveData saveData)
		{
			this.player = player;
			m_SaveData = saveData;

			commandGrid = new PlayerCommandGrid();
			units = new PlayerUnitList(triggerManager, player, saveData.playerInfo[player.playerID]);
		}

		/// <summary>
		/// Creates triggers for new mission. Only call when a new mission is started.
		/// </summary>
		public void InitializeNewMission()
		{
			PlayerInfoSaveData infoSaveData = new PlayerInfoSaveData();

			m_SaveData.playerInfo[player.playerID] = infoSaveData;

			units.InitializeNewMission(infoSaveData);
		}

		/// <summary>
		/// Updates the player state. Call this at the beginning of every frame.
		/// </summary>
		public void Update()
		{
			units.Update();
			commandGrid.Update(units, player.playerID);
		}
		
		public void Dispose()
		{
			units.Dispose();
		}
	}
}
