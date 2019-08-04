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

		public string GetPlayerName()									{ return Marshal.PtrToStringAnsi(PlayerEx_GetPlayerName(playerID));			}
		public void SetPlayerName(string newName)						{ PlayerEx_SetPlayerName(playerID, newName);								}

		//public int GetRLVCount()										{ return PlayerEx_GetSatelliteCount(playerID, map_id.RLV);					}
		public int GetSolarSatelliteCount()								{ return PlayerEx_GetSatelliteCount(playerID, map_id.SolarSatellite);		}
		public int GetEDWARDSatelliteCount()							{ return PlayerEx_GetSatelliteCount(playerID, map_id.EDWARDSatellite);		}

		public void SetRLVCount(int count)								{ PlayerEx_SetSatelliteCount(playerID, map_id.RLV, count);					}
		public void SetSolarSatelliteCount(int count)					{ PlayerEx_SetSatelliteCount(playerID, map_id.SolarSatellite, count);		}
		public void SetEDWARDSatelliteCount(int count)					{ PlayerEx_SetSatelliteCount(playerID, map_id.EDWARDSatellite, count);		}
		
		public int GetColorNumber()										{ return PlayerEx_GetColorNumber(playerID);									}
		public bool IsAlliedTo(Player ally)
		{
			if (ally.playerID == playerID)
				return true;

			return PlayerEx_IsAlliedTo(playerID, ally.playerID) > 0;
		}
		public int GetNumBuildingsBuilt()								{ return PlayerEx_GetNumBuildingsBuilt(playerID);							}

		public void Starve(int numToStarve, bool skipMoraleUpdate)		{ PlayerEx_Starve(playerID, numToStarve, skipMoraleUpdate ? 1 : 0);			}

		public int GetMaxCommonOre()									{ return PlayerEx_GetMaxOre(playerID);										}
		public int GetMaxRareOre()										{ return PlayerEx_GetMaxRareOre(playerID);									}

		// Not sure what this does. If it's required for info to be updated, perhaps call in update loop?
		public void RecalculateValues()									{ PlayerEx_RecalculateValues(playerID);										}

		public int GetNumAvailableWorkers()								{ return PlayerEx_GetNumAvailableWorkers(playerID);							}
		public int GetNumAvailableScientists()							{ return PlayerEx_GetNumAvailableScientists(playerID);						}
		public int GetAmountPowerGenerated()							{ return PlayerEx_GetAmountPowerGenerated(playerID);						}
		public int GetInactivePowerCapacity()							{ return PlayerEx_GetInactivePowerCapacity(playerID);						}
		public int GetAmountPowerConsumed()								{ return PlayerEx_GetAmountPowerConsumed(playerID);							}
		public int GetAmountPowerAvailable()							{ return PlayerEx_GetAmountPowerAvailable(playerID);						}
		public int GetNumIdleBuildings()								{ return PlayerEx_GetNumIdleBuildings(playerID);							}
		public int GetNumActiveBuildings()								{ return PlayerEx_GetNumActiveBuildings(playerID);							}
		public int GetNumBuildings()									{ return PlayerEx_GetNumBuildings(playerID);								}
		public int GetNumUnpoweredStructures()							{ return PlayerEx_GetNumUnpoweredStructures(playerID);						}
		public int GetNumWorkersRequired()								{ return PlayerEx_GetNumWorkersRequired(playerID);							}
		public int GetNumScientistsRequired()							{ return PlayerEx_GetNumScientistsRequired(playerID);						}
		public int GetNumScientistsAsWorkers()							{ return PlayerEx_GetNumScientistsAsWorkers(playerID);						}
		public int GetNumScientistsAssignedToResearch()					{ return PlayerEx_GetNumScientistsAssignedToResearch(playerID);				}
		public int GetTotalFoodProduction()								{ return PlayerEx_GetTotalFoodProduction(playerID);							}
		public int GetTotalFoodConsumption()							{ return PlayerEx_GetTotalFoodConsumption(playerID);						}
		public int GetFoodLacking()										{ return PlayerEx_GetFoodLacking(playerID);									}
		public int GetNetFoodProduction()								{ return PlayerEx_GetNetFoodProduction(playerID);							}
		public int GetNumSolarSatellites()								{ return PlayerEx_GetNumSolarSatellites(playerID);							}

		public int GetTotalRecreationFacilityCapacity()					{ return PlayerEx_GetTotalRecreationFacilityCapacity(playerID);				}
		public int GetTotalForumCapacity()								{ return PlayerEx_GetTotalForumCapacity(playerID);							}
		public int GetTotalMedCenterCapacity()							{ return PlayerEx_GetTotalMedCenterCapacity(playerID);						}
		public int GetTotalResidenceCapacity()							{ return PlayerEx_GetTotalResidenceCapacity(playerID);						}


		[DllImport("DotNetInterop.dll")] private static extern IntPtr PlayerEx_GetPlayerName(int playerID);
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

		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetNumAvailableWorkers(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetNumAvailableScientists(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetAmountPowerGenerated(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetInactivePowerCapacity(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetAmountPowerConsumed(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetAmountPowerAvailable(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetNumIdleBuildings(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetNumActiveBuildings(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetNumBuildings(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetNumUnpoweredStructures(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetNumWorkersRequired(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetNumScientistsRequired(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetNumScientistsAsWorkers(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetNumScientistsAssignedToResearch(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetTotalFoodProduction(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetTotalFoodConsumption(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetFoodLacking(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetNetFoodProduction(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetNumSolarSatellites(int playerID);

		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetTotalRecreationFacilityCapacity(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetTotalForumCapacity(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetTotalMedCenterCapacity(int playerID);
		[DllImport("DotNetInterop.dll")] private static extern int PlayerEx_GetTotalResidenceCapacity(int playerID);
	}
}
