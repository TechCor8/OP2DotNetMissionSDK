using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Triggers;
using DotNetMissionSDK.Utility.PlayerState;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetMissionSDK.Utility
{
	/// <summary>
	/// Maintains info about a player's state.
	/// </summary>
	public class PlayerInfo : IDisposable
	{
		private SaveData m_SaveData;        // The save data object to store persistent player state

		private readonly object m_SyncObject = new object();

		private ReadOnlyCollection<PlayerInfo> m_Allies		= new ReadOnlyCollection<PlayerInfo>(new List<PlayerInfo>());
		private ReadOnlyCollection<PlayerInfo> m_Enemies	= new ReadOnlyCollection<PlayerInfo>(new List<PlayerInfo>());

		public PlayerEx player							{ get; private set; }
		public ReadOnlyCollection<PlayerInfo> allies	{ get { lock (m_SyncObject) return m_Allies;	}	}
		public ReadOnlyCollection<PlayerInfo> enemies	{ get { lock (m_SyncObject) return m_Enemies;	}	}

		/// <summary>
		/// Contains lists of player units by type.
		/// Tracks starship module counts.
		/// </summary>
		public PlayerUnitList units						{ get; private set; } // PlayerUnitList IS NOT THREAD SAFE

		public PlayerCommandGrid commandGrid			{ get; private set; } // PlayerCommandGrid IS NOT THREAD SAFE


		/// <summary>
		/// Creates a new player info object. There should only be one per player.
		/// </summary>
		/// <param name="triggerManager">The trigger manager for tracking player events.</param>
		/// <param name="player">The player that will have its state tracked.</param>
		/// <param name="saveData">The global save object for storing persistent state.</param>
		public PlayerInfo(TriggerManager triggerManager, PlayerEx player, SaveData saveData)
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
		public void Update(PlayerInfo[] allPlayerInfos)
		{
			units.Update();
			commandGrid.Update(units, player.playerID);

			UpdateAlliances(allPlayerInfos);
		}

		private void UpdateAlliances(PlayerInfo[] allPlayerInfos)
		{
			List<PlayerInfo> allies = new List<PlayerInfo>();
			List<PlayerInfo> enemies = new List<PlayerInfo>();

			for (int i=0; i < allPlayerInfos.Length; ++i)
			{
				PlayerInfo info = allPlayerInfos[i];
				if (info == null)
					continue;

				PlayerEx otherPlayer = info.player;

				if (otherPlayer.IsAlliedTo(player))
					allies.Add(info);
				else
					enemies.Add(info);
			}

			lock (m_SyncObject)
			{
				m_Allies = allies.AsReadOnly();
				m_Enemies = enemies.AsReadOnly();
			}
		}
		
		public void Dispose()
		{
			units.Dispose();
		}
	}
}
