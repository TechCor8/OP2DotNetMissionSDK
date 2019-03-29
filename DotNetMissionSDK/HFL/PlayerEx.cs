using System;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK.HFL
{
	/// <summary>
	/// Extension class for Player.
	/// </summary>
	public class PlayerEx : Player
	{
		public PlayerEx(IntPtr handle, int playerID) : base(handle, playerID)
		{
		}

		public string GetPlayerName()									{ return PlayerEx_GetPlayerName(playerID);									}
		public void SetPlayerName(string newName)						{ PlayerEx_SetPlayerName(playerID, newName);								}

		//public int GetRLVCount()										{ return PlayerEx_GetSatelliteCount(playerID, map_id.RLV);					}
		public int GetSolarSatelliteCount()								{ return PlayerEx_GetSatelliteCount(playerID, map_id.SolarSatellite);		}
		public int GetEDWARDSatelliteCount()							{ return PlayerEx_GetSatelliteCount(playerID, map_id.EDWARDSatellite);		}

		public void SetRLVCount(int count)								{ PlayerEx_SetSatelliteCount(playerID, map_id.RLV, count);					}
		public void SetSolarSatelliteCount(int count)					{ PlayerEx_SetSatelliteCount(playerID, map_id.SolarSatellite, count);		}
		public void SetEDWARDSatelliteCount(int count)					{ PlayerEx_SetSatelliteCount(playerID, map_id.EDWARDSatellite, count);		}
		
		public int GetColorNumber()										{ return PlayerEx_GetColorNumber(playerID);									}
		public bool IsAlliedTo(Player ally)								{ return PlayerEx_IsAlliedTo(playerID, ally.playerID) > 0;					}
		public int GetNumBuildingsBuilt()								{ return PlayerEx_GetNumBuildingsBuilt(playerID);							}

		public void Starve(int numToStarve, bool skipMoraleUpdate)		{ PlayerEx_Starve(playerID, numToStarve, skipMoraleUpdate ? 1 : 0);			}

		public int GetMaxCommonOre()									{ return PlayerEx_GetMaxOre(playerID);										}
		public int GetMaxRareOre()										{ return PlayerEx_GetMaxRareOre(playerID);									}

		// Not sure what this does. If it's required for info to be updated, perhaps call in update loop?
		public void RecalculateValues()									{ PlayerEx_RecalculateValues(playerID);										}


		[DllImport("DotNetInterop.dll")] private static extern string PlayerEx_GetPlayerName(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern void PlayerEx_SetPlayerName(int playerID, string newName);

		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetSatelliteCount(int playerID, map_id objectType);
		[DllImport("DotNetInterop.dll")] private static extern void PlayerEx_SetSatelliteCount(int playerID, map_id objectType, int count);

		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetColorNumber(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_IsAlliedTo(int playerID, int allyPlayerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetNumBuildingsBuilt(int playerID);

		// NOTE: All commands should be separate exported functions. CommandPacket should not be passed at all.
		//extern EXPORT int __stdcall PlayerEx_ProcessCommandPacket(int playerID, CommandPacket *packet)
	
		[DllImport("DotNetInterop.dll")] private static extern void PlayerEx_Starve(int playerID, int numToStarve, int boolSkipMoraleUpdate);
	
		//extern EXPORT CommandPacket* __stdcall PlayerEx_GetNextCommandPacketAddress(int playerID)

		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetMaxOre(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetMaxRareOre(int playerID);

		[DllImport("DotNetInterop.dll")] private static extern void PlayerEx_RecalculateValues(int playerID);
	}
}
