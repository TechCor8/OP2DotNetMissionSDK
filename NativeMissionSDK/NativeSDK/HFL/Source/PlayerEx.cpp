// PlayerEx.cpp
#include "HFL.h"

#pragma pack(push,1)
struct OP2Player
{
	int playerId;
	unsigned int playerBitmask;
	struct {
		unsigned int rlv :4;
		unsigned int solarSat :4;
		unsigned int edwardSat :4;
		unsigned	:20;
	} satellites;
	int difficulty;
	int foodStored;
	int unk0;
	int maxCommonOre;
	int maxRareOre;
	int commonOre;
	int rareOre;
	int isHuman;
	int isEden;
	int colorNumber;
	int moraleLevel;
	int unk1[5];
	int foodSupply;
	int unk2[17];
	int workers;
	int scientists;
	int kids;
	int boolRecalcValues;
	int unk3[70];
	int numBuildingsBuilt;
	int unk4[662];
	Unit *buildingList;
	Unit *vehicleList;
	int unk5;
};
#pragma pack(pop)

OP2Player *playerArray;

PlayerEx *ExtPlayer = (PlayerEx*)&Player[0];

char* PlayerEx::GetPlayerName()
{
	if (!isInited)
		return (char*)HFLNOTINITED;

	char* (__fastcall *func)(OP2Player *classPtr) = (char* (__fastcall *)(OP2Player*))(imageBase + 0x90C80);
	return func(&playerArray[playerNum]);
}

void PlayerEx::SetPlayerName(char *newName)
{
	if (!isInited)
		return;

	void (__fastcall *func)(OP2Player *classPtr, int dummy, char *name) = (void (__fastcall *)(OP2Player*,int,char*))(imageBase + 0x90CB0);
	func(&playerArray[playerNum], 0, newName);
}

int PlayerEx::GetSatelliteCount(map_id objectType)
{
	if (!isInited)
		return HFLNOTINITED;

	switch (objectType)
	{
	case mapRLV:
		return playerArray[playerNum].satellites.rlv;
	case mapSolarSatellite:
		return playerArray[playerNum].satellites.solarSat;
	case mapEDWARDSatellite:
		return playerArray[playerNum].satellites.edwardSat;
	default:
		return 0;
	}
}

void PlayerEx::SetSatelliteCount(map_id objectType, int count)
{
	if (!isInited)
		return;

	switch (objectType)
	{
	case mapRLV:
		playerArray[playerNum].satellites.rlv = count;
	case mapSolarSatellite:
		playerArray[playerNum].satellites.solarSat = count;
	case mapEDWARDSatellite:
		playerArray[playerNum].satellites.edwardSat = count;
	}
}

int PlayerEx::GetColorNumber()
{
	if (!isInited)
		return HFLNOTINITED;

	return playerArray[playerNum].colorNumber;
}

int PlayerEx::IsAlliedTo(int playerId)
{
	if (!isInited)
		return HFLNOTINITED;

	if (playerId < 0 || playerId > 6)
		return -1;

	return (playerArray[playerNum].playerBitmask >> playerId) & 1;
}

int PlayerEx::GetNumBuildingsBuilt()
{
	if (!isInited)
		return HFLNOTINITED;

	return playerArray[playerNum].numBuildingsBuilt;
}

int PlayerEx::ProcessCommandPacket(CommandPacket *packet)
{
	if (!isInited)
		return HFLNOTINITED;

	int (__fastcall *func)(OP2Player *classPtr, int dummy, CommandPacket *packet) = (int (__fastcall *)(OP2Player*,int,CommandPacket*))(imageBase + 0x0E300);
	return func(&playerArray[playerNum], 0, packet);
}

void PlayerEx::Starve(int numToStarve, int boolSkipMoraleUpdate)
{
	if (!isInited)
		return;

	void (__fastcall *func)(OP2Player *classPtr, int dummy, int numStarve, int skipMoraleUpdate) = (void (__fastcall *)(OP2Player*,int,int,int))(imageBase + 0x71C70);
	func(&playerArray[playerNum], 0, numToStarve, boolSkipMoraleUpdate);
}

CommandPacket* PlayerEx::GetNextCommandPacketAddress()
{
	if (!isInited)
		return (CommandPacket*)HFLNOTINITED;

	CommandPacket* (__fastcall *func)(OP2Player *classPtr) = (CommandPacket* (__fastcall *)(OP2Player*))(imageBase + 0x90810);
	return func(&playerArray[playerNum]);
}

int PlayerEx::GetMaxOre()
{
	if (!isInited)
		return HFLNOTINITED;

	return playerArray[playerNum].maxCommonOre;
}

int PlayerEx::GetMaxRareOre()
{
	if (!isInited)
		return HFLNOTINITED;

	return playerArray[playerNum].maxRareOre;
}

void PlayerEx::RecalculateValues()
{
	if (!isInited)
		return;

	playerArray[playerNum].boolRecalcValues = 1;
}