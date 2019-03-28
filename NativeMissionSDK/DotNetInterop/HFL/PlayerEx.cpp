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
}
