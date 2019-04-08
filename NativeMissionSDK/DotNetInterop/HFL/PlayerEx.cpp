// PlayerEx.h
// Extra / useful player stuff
#include "stdafx_unmanaged.h"

#include <HFL/Source/HFL.h>

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif


extern "C"
{
	extern EXPORT char* __stdcall PlayerEx_GetPlayerName(int playerID)
	{
		PlayerEx* player = &ExtPlayer[playerID];

		return player->GetPlayerName();
	}
	extern EXPORT void __stdcall PlayerEx_SetPlayerName(int playerID, const char* newName)
	{
		// C# will delete string parameters, make a copy
		size_t len = strlen(newName) + 1;
		char* copy = new char[len];
		strcpy_s(copy, len, newName);

		PlayerEx* player = &ExtPlayer[playerID];

		return player->SetPlayerName(copy);
	}

	extern EXPORT int __stdcall PlayerEx_GetSatelliteCount(int playerID, map_id objectType)
	{
		PlayerEx* player = &ExtPlayer[playerID];

		return player->GetSatelliteCount(objectType);
	}
	extern EXPORT void __stdcall PlayerEx_SetSatelliteCount(int playerID, map_id objectType, int count)
	{
		PlayerEx* player = &ExtPlayer[playerID];

		return player->SetSatelliteCount(objectType, count);
	}

	extern EXPORT int __stdcall PlayerEx_GetColorNumber(int playerID)
	{
		PlayerEx* player = &ExtPlayer[playerID];

		return player->GetColorNumber();
	}
	extern EXPORT int __stdcall PlayerEx_IsAlliedTo(int playerID, int allyPlayerID)
	{
		PlayerEx* player = &ExtPlayer[playerID];

		return player->IsAlliedTo(allyPlayerID);
	}
	extern EXPORT int __stdcall PlayerEx_GetNumBuildingsBuilt(int playerID)
	{
		PlayerEx* player = &ExtPlayer[playerID];

		return player->GetNumBuildingsBuilt();
	}

	// NOTE: All commands should be separate exported functions. CommandPacket should not be passed at all.
	//extern EXPORT int __stdcall PlayerEx_ProcessCommandPacket(int playerID, CommandPacket *packet)
	//{
	//	PlayerEx* player = &ExtPlayer[playerID];
	//
	//	return player->ProcessCommandPacket(packet);
	//}

	extern EXPORT void __stdcall PlayerEx_Starve(int playerID, int numToStarve, int boolSkipMoraleUpdate)
	{
		PlayerEx* player = &ExtPlayer[playerID];

		player->Starve(numToStarve, boolSkipMoraleUpdate);
	}

	//extern EXPORT CommandPacket* __stdcall PlayerEx_GetNextCommandPacketAddress(int playerID)
	//{
	//	PlayerEx* player = &ExtPlayer[playerID];
	//
	//	return player->GetNextCommandPacketAddress();
	//}

	extern EXPORT int __stdcall PlayerEx_GetMaxOre(int playerID)
	{
		PlayerEx* player = &ExtPlayer[playerID];

		return player->GetMaxOre();
	}
	extern EXPORT int __stdcall PlayerEx_GetMaxRareOre(int playerID)
	{
		PlayerEx* player = &ExtPlayer[playerID];

		return player->GetMaxRareOre();
	}

	extern EXPORT void __stdcall PlayerEx_RecalculateValues(int playerID)
	{
		PlayerEx* player = &ExtPlayer[playerID];

		player->RecalculateValues();
	}

	extern EXPORT int __stdcall PlayerEx_GetNumAvailableWorkers(int playerID)					{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetNumAvailableWorkers();				}
	extern EXPORT int __stdcall PlayerEx_GetNumAvailableScientists(int playerID)				{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetNumAvailableScientists();				}
	extern EXPORT int __stdcall PlayerEx_GetAmountPowerGenerated(int playerID)					{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetAmountPowerGenerated();				}
	extern EXPORT int __stdcall PlayerEx_GetInactivePowerCapacity(int playerID)					{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetInactivePowerCapacity();				}
	extern EXPORT int __stdcall PlayerEx_GetAmountPowerConsumed(int playerID)					{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetAmountPowerConsumed();				}
	extern EXPORT int __stdcall PlayerEx_GetAmountPowerAvailable(int playerID)					{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetAmountPowerAvailable();				}
	extern EXPORT int __stdcall PlayerEx_GetNumIdleBuildings(int playerID)						{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetNumIdleBuildings();					}
	extern EXPORT int __stdcall PlayerEx_GetNumActiveBuildings(int playerID)					{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetNumActiveBuildings();					}
	extern EXPORT int __stdcall PlayerEx_GetNumBuildings(int playerID)							{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetNumBuildings();						}
	extern EXPORT int __stdcall PlayerEx_GetNumUnpoweredStructures(int playerID)				{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetNumUnpoweredStructures();				}
	extern EXPORT int __stdcall PlayerEx_GetNumWorkersRequired(int playerID)					{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetNumWorkersRequired();					}
	extern EXPORT int __stdcall PlayerEx_GetNumScientistsRequired(int playerID)					{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetNumScientistsRequired();				}
	extern EXPORT int __stdcall PlayerEx_GetNumScientistsAsWorkers(int playerID)				{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetNumScientistsAsWorkers();				}
	extern EXPORT int __stdcall PlayerEx_GetNumScientistsAssignedToResearch(int playerID)		{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetNumScientistsAssignedToResearch();	}
	extern EXPORT int __stdcall PlayerEx_GetTotalFoodProduction(int playerID)					{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetTotalFoodProduction();				}
	extern EXPORT int __stdcall PlayerEx_GetTotalFoodConsumption(int playerID)					{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetTotalFoodConsumption();				}
	extern EXPORT int __stdcall PlayerEx_GetFoodLacking(int playerID)							{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetFoodLacking();						}
	extern EXPORT int __stdcall PlayerEx_GetNetFoodProduction(int playerID)						{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetNetFoodProduction();					}
	extern EXPORT int __stdcall PlayerEx_GetNumSolarSatellites(int playerID)					{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetNumSolarSatellites();					}

	extern EXPORT int __stdcall PlayerEx_GetTotalRecreationFacilityCapacity(int playerID)		{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetTotalRecreationFacilityCapacity();	}
	extern EXPORT int __stdcall PlayerEx_GetTotalForumCapacity(int playerID)					{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetTotalForumCapacity();					}
	extern EXPORT int __stdcall PlayerEx_GetTotalMedCenterCapacity(int playerID)				{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetTotalMedCenterCapacity();				}
	extern EXPORT int __stdcall PlayerEx_GetTotalResidenceCapacity(int playerID)				{ PlayerEx* player = &ExtPlayer[playerID];	return player->GetTotalResidenceCapacity();				}
}
