// UnitInfo.h
// Classes for modifying UnitInfo structures
#ifndef _UNITINFO_H_
#define _UNITINFO_H_

class UnitInfo
{
public:
	UnitInfo(map_id unitType);
	UnitInfo(char *codeName);
	int IsValid();

	int GetHitPoints(int player);
	void SetHitPoints(int player, int value);
	int GetRepairAmt(int player);
	void SetRepairAmt(int player, int value);
	int GetArmor(int player);
	void SetArmor(int player, int value);
	int GetOreCost(int player);
	void SetOreCost(int player, int value);
	int GetRareOreCost(int player);
	void SetRareOreCost(int player, int value);

	int GetBuildTime(int player);
	void SetBuildTime(int player, int value);

	int GetSightRange(int player);
	void SetSightRange(int player, int value);
	int GetWeaponRange(int player);
	void SetWeaponRange(int player, int value);

	int GetPowerRequired(int player);
	void SetPowerRequired(int player, int value);
	int GetMovePoints(int player);
	void SetMovePoints(int player, int value);
	int GetTurretTurnRate(int player);
	void SetTurretTurnRate(int player, int value);

	int GetConcussionDamage(int player);
	void SetConcussionDamage(int player, int value);
	int GetWorkersRequired(int player);
	void SetWorkersRequired(int player, int value);
	int GetTurnRate(int player);
	void SetTurnRate(int player, int value);

	int GetPenetrationDamage(int player);
	void SetPenetrationDamage(int player, int value);
	int GetScientistsRequired(int player);
	void SetScientistsRequired(int player, int value);
	int GetProductionRate(int player);
	void SetProductionRate(int player, int value);

	int GetReloadTime(int player);
	void SetReloadTime(int player, int value);
	int GetStorageCapacity(int player);
	void SetStorageCapacity(int player, int value);

	int GetWeaponSightRange(int player);
	void SetWeaponSightRange(int player, int value);
	int GetProductionCapacity(int player);
	void SetProductionCapacity(int player, int value);

	int GetNumStorageBays(int player);
	void SetNumStorageBays(int player, int value);
	int GetCargoCapacity(int player);
	void SetCargoCapacity(int player, int value);

	int GetResearchTopic();
	void SetResearchTopic(int techId);

	TrackType GetTrackType();
	void SetTrackType(TrackType type);

	int GetOwnerFlags();
	void SetOwnerFlags(int flags);

	char* GetUnitName();
	void SetUnitName(char *newName);

	char* GetProduceListName();
	void SetProduceListName(char *newName);

	int GetXSize();
	void SetXSize(int value);
	int GetDamageRadius();
	void SetDamageRadius(int value);
	int GetVehicleFlags();
	void SetVehicleFlags(int flags);

	int GetYSize();
	void SetYSize(int value);

	int GetPixelsSkippedWhenFiring();
	void SetPixelsSkippedWhenFiring(int value);
	int GetBuildingFlags();
	void SetBuildingFlags(int flags);

	// todo: maybe use enum for this?
	int GetExplosionSize();
	void SetExplosionSize(int value);

	int GetResourcePriority();
	void SetResourcePriority(int value);

	int GetRareRubble();
	void SetRareRubble(int value);

	int GetRubble();
	void SetRubble(int value);

	int GetEdenDockPos();
	void SetEdenDockPos(int value);

	int GetPlymDockPos();
	void SetPlymDockPos(int value);

	char* GetCodeName();
	map_id GetMapID();
	Unit CreateUnit(LOCATION where, int unitId);
	// todo: perhaps load / save functions? (so people can save data into a stream)

	void *internalPtr;
};

struct OP2UnitInfo;
extern OP2UnitInfo **unitInfoArray;
extern void *unitInfoObj;

#endif // UNITINFO_H_