// PlayerEx.h
// Extra / useful player stuff
#ifndef _PLAYEREX_H_
#define _PLAYEREX_H_

class PlayerEx : public _Player
{
public:
	char* GetPlayerName();
	void SetPlayerName(char *newName);
	int GetSatelliteCount(map_id objectType);
	void SetSatelliteCount(map_id objectType, int count);
	int GetColorNumber();
	int IsAlliedTo(int playerId);
	int GetNumBuildingsBuilt();
	int ProcessCommandPacket(CommandPacket *packet);
	void Starve(int numToStarve, int boolSkipMoraleUpdate);
	CommandPacket* GetNextCommandPacketAddress();
	int GetMaxOre();
	int GetMaxRareOre();
	void RecalculateValues();

	int GetNumAvailableWorkers();
	int GetNumAvailableScientists();
	int GetAmountPowerGenerated();
	int GetInactivePowerCapacity();
	int GetAmountPowerConsumed();
	int GetAmountPowerAvailable();
	int GetNumIdleBuildings();
	int GetNumActiveBuildings();
	int GetNumBuildings();
	int GetNumUnpoweredStructures();
	int GetNumWorkersRequired();		// [assigned workers and scientists]
	int GetNumScientistsRequired();		// [assigned to buildings, or researching]
	int GetNumScientistsAsWorkers();
	int GetNumScientistsAssignedToResearch();
	int GetTotalFoodProduction();
	int GetTotalFoodConsumption();
	int GetFoodLacking();	// [how much food needed to stop people starving ? Note this can be 0 even if there is a food deficit]
	int GetNetFoodProduction();
	int GetNumSolarSatellites();	// [copy from bitfield at offset 8]

	int GetTotalRecreationFacilityCapacity();
	int GetTotalForumCapacity();
	int GetTotalMedCenterCapacity();
	int GetTotalResidenceCapacity();
	// todo: anything else?
};

// ExtPlayer array that references the Player array
extern PlayerEx *ExtPlayer;

struct OP2Player;
extern OP2Player *playerArray;

#endif // _PLAYEREX_H_
