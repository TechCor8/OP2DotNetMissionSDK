using DotNetMissionSDK.Async;
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

		public string GetPlayerName()									{ ThreadAssert.MainThreadRequired();	return Marshal.PtrToStringAnsi(PlayerEx_GetPlayerName(playerID));		}
		public void SetPlayerName(string newName)						{ ThreadAssert.MainThreadRequired();	PlayerEx_SetPlayerName(playerID, newName);								}

		//public int GetRLVCount()										{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetSatelliteCount(playerID, map_id.RLV);				}
		public int GetSolarSatelliteCount()								{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetSatelliteCount(playerID, map_id.SolarSatellite);		}
		public int GetEDWARDSatelliteCount()							{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetSatelliteCount(playerID, map_id.EDWARDSatellite);	}

		public void SetRLVCount(int count)								{ ThreadAssert.MainThreadRequired();	PlayerEx_SetSatelliteCount(playerID, map_id.RLV, count);				}
		public void SetSolarSatelliteCount(int count)					{ ThreadAssert.MainThreadRequired();	PlayerEx_SetSatelliteCount(playerID, map_id.SolarSatellite, count);		}
		public void SetEDWARDSatelliteCount(int count)					{ ThreadAssert.MainThreadRequired();	PlayerEx_SetSatelliteCount(playerID, map_id.EDWARDSatellite, count);	}
		
		public int GetColorNumber()										{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetColorNumber(playerID);								}
		public bool IsAlliedTo(Player ally)
		{
			ThreadAssert.MainThreadRequired();

			if (ally.playerID == playerID)
				return true;

			return PlayerEx_IsAlliedTo(playerID, ally.playerID) > 0;
		}
		public int GetNumBuildingsBuilt()								{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetNumBuildingsBuilt(playerID);							}

		public void Starve(int numToStarve, bool skipMoraleUpdate)		{ ThreadAssert.MainThreadRequired();	PlayerEx_Starve(playerID, numToStarve, skipMoraleUpdate ? 1 : 0);		}

		public int GetMaxCommonOre()									{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetMaxOre(playerID);									}
		public int GetMaxRareOre()										{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetMaxRareOre(playerID);								}

		// Not sure what this does. If it's required for info to be updated, perhaps call in update loop?
		public void RecalculateValues()									{ ThreadAssert.MainThreadRequired();	PlayerEx_RecalculateValues(playerID);									}

		public int GetNumAvailableWorkers()								{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetNumAvailableWorkers(playerID);						}
		public int GetNumAvailableScientists()							{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetNumAvailableScientists(playerID);					}
		public int GetAmountPowerGenerated()							{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetAmountPowerGenerated(playerID);						}
		public int GetInactivePowerCapacity()							{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetInactivePowerCapacity(playerID);						}
		public int GetAmountPowerConsumed()								{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetAmountPowerConsumed(playerID);						}
		public int GetAmountPowerAvailable()							{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetAmountPowerAvailable(playerID);						}
		public int GetNumIdleBuildings()								{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetNumIdleBuildings(playerID);							}
		public int GetNumActiveBuildings()								{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetNumActiveBuildings(playerID);						}
		public int GetNumBuildings()									{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetNumBuildings(playerID);								}
		public int GetNumUnpoweredStructures()							{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetNumUnpoweredStructures(playerID);					}
		public int GetNumWorkersRequired()								{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetNumWorkersRequired(playerID);						}
		public int GetNumScientistsRequired()							{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetNumScientistsRequired(playerID);						}
		public int GetNumScientistsAsWorkers()							{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetNumScientistsAsWorkers(playerID);					}
		public int GetNumScientistsAssignedToResearch()					{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetNumScientistsAssignedToResearch(playerID);			}
		public int GetTotalFoodProduction()								{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetTotalFoodProduction(playerID);						}
		public int GetTotalFoodConsumption()							{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetTotalFoodConsumption(playerID);						}
		public int GetFoodLacking()										{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetFoodLacking(playerID);								}
		public int GetNetFoodProduction()								{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetNetFoodProduction(playerID);							}
		public int GetNumSolarSatellites()								{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetNumSolarSatellites(playerID);						}

		public int GetTotalRecreationFacilityCapacity()					{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetTotalRecreationFacilityCapacity(playerID);			}
		public int GetTotalForumCapacity()								{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetTotalForumCapacity(playerID);						}
		public int GetTotalMedCenterCapacity()							{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetTotalMedCenterCapacity(playerID);					}
		public int GetTotalResidenceCapacity()							{ ThreadAssert.MainThreadRequired();	return PlayerEx_GetTotalResidenceCapacity(playerID);					}

		/// <summary>
		/// Checks if this player is the correct colony type, has completed the required research, and has the required resources to build a unit.
		/// </summary>
		public bool CanBuildUnit(map_id unitType, map_id cargoOrWeaponType=map_id.None)
		{
			ThreadAssert.MainThreadRequired();

			bool isEden = IsEden();

			UnitInfo vehicleInfo = new UnitInfo(unitType);

			// Fail Check: Colony Type
			if (!vehicleInfo.CanColonyUseUnit(isEden))
				return false;

			// Fail Check: Research
			TechInfo techInfo = Research.GetTechInfo(vehicleInfo.GetResearchTopic());
			if (!HasTechnology(techInfo.GetTechID()))
				return false;

			if (cargoOrWeaponType != map_id.None)
			{
				UnitInfo cargoInfo = new UnitInfo(cargoOrWeaponType);

				// Fail Check: Cargo Colony Type
				if (!cargoInfo.CanColonyUseUnit(isEden))
					return false;

				// Fail Check: Cargo Research
				TechInfo cargoTechInfo = Research.GetTechInfo(cargoInfo.GetResearchTopic());
				if (!HasTechnology(cargoTechInfo.GetTechID()))
					return false;

				// Fail Check: Vehicle cost
				if (Ore() < vehicleInfo.GetOreCost(playerID) + cargoInfo.GetOreCost(playerID)) return false;
				if (RareOre() < vehicleInfo.GetRareOreCost(playerID) + cargoInfo.GetRareOreCost(playerID)) return false;
			}
			else
			{
				// Fail Check: Vehicle cost
				if (Ore() < vehicleInfo.GetOreCost(playerID)) return false;
				if (RareOre() < vehicleInfo.GetRareOreCost(playerID)) return false;
			}

			return true;
		}


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
