#include "HFL.h"

#pragma pack(push,1)
// All of these structs have room for only one unit ID as the functions operate on one unit only.
struct cmdAttackXY // supports waypoints, as well for attack unit
{
	char numUnits;
	short unitId;
	short unknown; // 0000
	short tileX;
	short tileY;
};

struct cmdGuard
{
	char numUnits;
	short unitId;
	short unknown; // 0000
	short unitIdToGuard;
	short unused; // FFFF
};

struct cmdDeploy
{
	char numUnits;
	short unitId;
	short numWayPoints;
	long wptLoc;		// Pixel-based X/Y location, X coordinate = low byte, Y coordinate = high byte
	short x1;
	short y1;
	short x2;
	short y2;
	short unknown;
};

struct cmdDoze
{
	char numUnits;
	short unitId;
	short x1;
	short y1;
	short x2;
	short y2;
};

struct wayPoint // todo: check into this
{
	unsigned :5;
	unsigned int x:15;
	unsigned int y:14;
};

union wayPoints {
	wayPoint points;
	unsigned int rawPoints;
};

struct cmdMove // same for patrol, dock, dockEG, cargo route
{
	char numUnits;
	short unitId;
	short numWayPoints;
	wayPoints pts;
	// ...
};

// todo: cargoroute

struct cmdBuildWall
{
	char numUnits;
	short unitId;
	short x1;
	short y1;
	short x2;
	short y2;
	short wallType; // map_id
	short unknown; // FFFF
};

struct cmdRemoveWall
{
	char numUnits;
	short unitId;
	short numWayPoints;
	//wayPoints pts;
	// ...
	short x1;
	short y1;
	short x2;
	short y2;
};

struct cmdProduce
{
	short unitIdFactory;
	short itemToProduce;
	short itemCargoWeapon;
	short unknown; // FFFF
};

struct cmdTransferCargo
{
	short unitIdBuilding;
	short bay;
	short unknown; // 0000
};

struct cmdLoadCargo // same for unload / dump / poof
{
	short unitId;
};

struct cmdResearch
{
	short unitIdLab;
	short techId;
	short numScis;
};

struct cmdTrainSci
{
	short unitId;
	short numScis;
};

struct cmdRepair // same for reprogram / dismantle. supports waypoints.. need to find out how
{
	char numUnits;
	short unitId;
	short unknown1; // 0000
	short unitToRepair;
	short unknown2; // FFFF
};

struct cmdSalvage
{
	short unitIdTruck;
	short x1;
	short y1;
	short x2;
	short y2;
	short unitIdGorf;
};

// todo: 'build' command with waypoints

// OP2Unit
// Can't use bit fields on the 'char' types since the compiler will still pad to word-size
struct OP2Unit
{
	void *vtbl;
	int isLive;
	void *prevUnit;
	void *nextUnit;
	int id;
	int pixelX;
	int pixelY;
	char unknown0;
	char ownerCreator; // lower 4 bits: owner ID, upper 4 bits: creator ID
	short damage;
	char boolNewAction;
	char curCmd;
	char curAction;
	char lastAction;
	short weaponCargo; // trucks: lowest 4 bits, cargo type; next 4 bits, cargo amount
	short unknown1;
	int unknown2;
	short instanceNum;
	short unknown3;
	int unknown4;
	void *destWaypoints; // missileDestX
	int missileDestY;
	int timer;
	int unknown5;
	unsigned int flags;
	char bayWeaponCargo[6];
	char unknown6;
	char unknownCargo;
	int unknown7;
	short timerStickyfoam;
	short timerEMP;
	short timerESG;
	short unknown8;
	int unknown9;
	char workersInTraining;
	char bayItem[6];
	char unknown11;
	int unknown12;
	short unknown13;
	int objectOnPad;
	int launchPadCargo; // Arklon says this should be an int32. unknown15 would be int16?
	short unknown15;
};

struct BeaconData
{
	int numTruckLoadsSoFar;
	int barYield;
	int variant;
	char oreType; // [0 = common, 1 = rare]
	char unknown1;
	char unknown2;
	char surveyedBy; // [player bit vector]
};

struct LabData
{
	short nextResearchTime;		// Amount of time until next research increment(research progresses in chunks)
	short unknown1;
	int unknown2[5];
	int researchRemaining;		// Amount of research left to complete tech(intialized to costOfResearch * 256 when research starts)
	int unknown3[8];
	int techNum;
	char numScientists;			// numScientists researching at the lab
};
#pragma pack(pop)

// Unit flags
enum OP2UnitFlags
{
	flagHeadlights = 0x1,
	flagVehicle = 0x2,
	flagBuilding = 0x4,
	flagDoubleFireRate = 0x20,
	flagPower = 0x2000,
	flagWorkers = 0x4000,
	flagScientists = 0x8000,
	flagLive = 0x20000,
	flagStickyfoamed = 0x40000,
	flagInfected = 0x40000,
	flagEMPed = 0x80000,
	flagESGed = 0x100000,
	flagInvisible = 0x10000000,
	flagSpecialTarget = 0x20000000
};

OP2Unit **unitArray;

void UnitEx::DoAttack(LOCATION where)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdAttackXY *data = (cmdAttackXY*)packet.dataBuff;

	packet.type = ctMoAttackObj;
	packet.dataLength = sizeof(cmdAttackXY);
	data->numUnits = 1;
	data->unitId = unitID;
	data->tileX = where.x;
	data->tileY = where.y;
	data->unknown = 0;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoDeployMiner(LOCATION where)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdDeploy *data = (cmdDeploy*)packet.dataBuff;

	// Convert x/y location to pixel
	int pxLoc = 0;
	int x = where.x * 32;
	int y = where.y * 32;
	pxLoc = (x & 0x07FFF) | (y & 0x03FFF) << 15;

	// Build command packet
	packet.type = ctMoBuild;
	packet.dataLength = sizeof(cmdDeploy);
	data->numUnits = 1;
	data->unitId = unitID;
	data->numWayPoints = 1;
	data->wptLoc = pxLoc;
	data->x1 = where.x-1;
	data->y1 = where.y;
	data->x2 = where.x;
	data->y2 = where.y;
	data->unknown = -1;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoDoze(MAP_RECT area)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdDoze *data = (cmdDoze*)packet.dataBuff;

	packet.type = ctMoDoze;
	packet.dataLength = sizeof(cmdDoze);
	data->numUnits = 1;
	data->unitId = unitID;
	data->x1 = area.x1;
	data->x2 = area.x2;
	data->y1 = area.y1;
	data->y2 = area.y2;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoDock(LOCATION dockLocation)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdMove *data = (cmdMove*)packet.dataBuff;

	packet.type = ctMoDock;
	packet.dataLength = sizeof(cmdMove);
	data->numUnits = 1;
	data->unitId = unitID;
	data->numWayPoints = 1;
	long wpt = 0,
		x = dockLocation.x,
		y = dockLocation.y;
	wpt |= (x & 0x7ff) << 5;
	wpt |= (y & 0x3ff) << 20;
	data->pts.rawPoints = wpt;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoDockAtGarage(LOCATION dockLocation)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdMove *data = (cmdMove*)packet.dataBuff;

	packet.type = ctMoDockEG;
	packet.dataLength = sizeof(cmdMove);
	data->numUnits = 1;
	data->unitId = unitID;
	data->numWayPoints = 1;
	data->pts.points.x = dockLocation.x;
	data->pts.points.y = dockLocation.y;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoStandGround(LOCATION where)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdMove *data = (cmdMove*)packet.dataBuff;

	packet.type = ctMoStandGround;
	packet.dataLength = sizeof(cmdMove);
	data->numUnits = 1;
	data->unitId = unitID;
	data->numWayPoints = 1;
	data->pts.points.x = where.x;
	data->pts.points.y = where.y;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoBuildWall(map_id wallType, MAP_RECT area)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdBuildWall *data = (cmdBuildWall*)packet.dataBuff;

	packet.type = ctMoBuildWall;
	packet.dataLength = sizeof(cmdBuildWall);
	data->numUnits = 1;
	data->unitId = unitID;
	data->wallType = wallType;
	data->x1 = area.x1;
	data->x2 = area.x2;
	data->y1 = area.y1;
	data->y2 = area.y2;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoRemoveWall(MAP_RECT area)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdRemoveWall *data = (cmdRemoveWall*)packet.dataBuff;

	packet.type = ctMoRemoveWall;
	packet.dataLength = sizeof(cmdRemoveWall);
	data->numUnits = 1;
	data->unitId = unitID;
	data->numWayPoints = 0;
	data->x1 = area.x1;
	data->x2 = area.x2;
	data->y1 = area.y1;
	data->y2 = area.y2;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoProduce(map_id unitType, map_id cargoWeaponType)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdProduce *data = (cmdProduce*)packet.dataBuff;

	packet.type = ctMoProduce;
	packet.dataLength = sizeof(cmdProduce);
	data->unitIdFactory = unitID;
	data->itemToProduce = unitType;
	data->itemCargoWeapon = cargoWeaponType;
	data->unknown = -1;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoTransferCargo(int bay)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdTransferCargo *data = (cmdTransferCargo*)packet.dataBuff;

	packet.type = ctMoTransferCargo;
	packet.dataLength = sizeof(cmdTransferCargo);
	data->unitIdBuilding = unitID;
	data->bay = bay;
	data->unknown = 0;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoLoadCargo()
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdLoadCargo *data = (cmdLoadCargo*)packet.dataBuff;

	packet.type = ctMoLoadCargo;
	packet.dataLength = sizeof(cmdLoadCargo);
	data->unitId = unitID;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoUnloadCargo()
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdLoadCargo *data = (cmdLoadCargo*)packet.dataBuff;

	packet.type = ctMoUnloadCargo;
	packet.dataLength = sizeof(cmdLoadCargo);
	data->unitId = unitID;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoDumpCargo()
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdLoadCargo *data = (cmdLoadCargo*)packet.dataBuff;

	packet.type = ctMoDumpCargo;
	packet.dataLength = sizeof(cmdLoadCargo);
	data->unitId = unitID;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoResearch(int techID, int numScientists)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdResearch *data = (cmdResearch*)packet.dataBuff;

	packet.type = ctMoResearch;
	packet.dataLength = sizeof(cmdResearch);
	data->unitIdLab = unitID;
	data->techId = techID;
	data->numScis = numScientists;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoTrainScientists(int numToTrain)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdTrainSci *data = (cmdTrainSci*)packet.dataBuff;

	packet.type = ctMoTrainScientists;
	packet.dataLength = sizeof(cmdTrainSci);
	data->unitId = unitID;
	data->numScis = numToTrain;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoRepair(Unit what)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdRepair *data = (cmdRepair*)packet.dataBuff;

	packet.type = ctMoRepairObj;
	packet.dataLength = sizeof(cmdRepair);
	data->numUnits = 1;
	data->unitId = unitID;
	data->unitToRepair = what.unitID;
	data->unknown1 = 0;
	data->unknown2 = -1;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoReprogram(Unit what)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdRepair *data = (cmdRepair*)packet.dataBuff;

	packet.type = ctMoReprogram;
	packet.dataLength = sizeof(cmdRepair);
	data->numUnits = 1;
	data->unitId = unitID;
	data->unitToRepair = what.unitID;
	data->unknown1 = 0;
	data->unknown2 = -1;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoDismantle(Unit what)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdRepair *data = (cmdRepair*)packet.dataBuff;

	packet.type = ctMoDismantle;
	packet.dataLength = sizeof(cmdRepair);
	data->numUnits = 1;
	data->unitId = unitID;
	data->unitToRepair = what.unitID;
	data->unknown1 = 0;
	data->unknown2 = -1;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoSalvage(MAP_RECT area, Unit gorf)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdSalvage *data = (cmdSalvage*)packet.dataBuff;

	packet.type = ctMoSalvage;
	packet.dataLength = sizeof(cmdSalvage);
	data->unitIdTruck = unitID;
	data->unitIdGorf = gorf.unitID;
	data->x1 = area.x1;
	data->x2 = area.x2;
	data->y1 = area.y1;
	data->y2 = area.y2;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoGuard(Unit what)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdGuard *data = (cmdGuard*)packet.dataBuff;

	packet.type = ctMoGuard;
	packet.dataLength = sizeof(cmdGuard);
	data->numUnits = 1;
	data->unitId = unitID;
	data->unitIdToGuard = what.unitID;
	data->unknown = 0;
	data->unused = -1;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

void UnitEx::DoPoof()
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	CommandPacket packet;
	cmdLoadCargo *data = (cmdLoadCargo*)packet.dataBuff;

	packet.type = ctMoPoof;
	packet.dataLength = sizeof(cmdLoadCargo);
	data->unitId = unitID;

	ExtPlayer[OwnerID()].ProcessCommandPacket(&packet);
}

CommandType UnitEx::GetLastCommand()
{
	if (!isInited) {
		return (CommandType)HFLNOTINITED;
	}

	if (!IsLive()) {
		return (CommandType)-1;
	}

	return (CommandType)(*unitArray)[unitID].curCmd;
}

ActionType UnitEx::GetCurAction()
{
	if (!isInited) {
		return (ActionType)HFLNOTINITED;
	}

	if (!IsLive()) {
		return (ActionType)-1;
	}

	return (ActionType)(*unitArray)[unitID].curAction;
}

int UnitEx::CreatorID()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	if (!IsLive()) {
		return -1;
	}

	return ((*unitArray)[unitID].ownerCreator >> 4) & 0xF;
}

int UnitEx::IsEMPedEx()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	if (!IsLive()) {
		return -1;
	}

	if ((*unitArray)[unitID].ownerCreator & flagEMPed) {
		return (*unitArray)[unitID].timerEMP;
	}
	else {
		return 0;
	}
}

int UnitEx::IsStickyfoamed()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	if (!IsLive()) {
		return -1;
	}

	if ((*unitArray)[unitID].ownerCreator & flagStickyfoamed) {
		return (*unitArray)[unitID].timerStickyfoam;
	}
	else {
		return 0;
	}
}

int UnitEx::IsESGed()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	if (!IsLive()) {
		return -1;
	}

	if ((*unitArray)[unitID].ownerCreator & flagESGed) {
		return (*unitArray)[unitID].timerESG;
	}
	else {
		return 0;
	}
}

int UnitEx::GetDamage()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	if (!IsLive()) {
		return -1;
	}

	return (*unitArray)[unitID].damage;
}

int UnitEx::GetCargoAmount()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	if (!IsLive()) {
		return -1;
	}

	int cargoAmount = (((*unitArray)[unitID].weaponCargo) >> 4) & 0xFFF;
	if (GetCargoType() == truckGarbage) {
		cargoAmount += 8000;
	}

	return cargoAmount;
}

Truck_Cargo UnitEx::GetCargoType()
{
	if (!isInited) {
		return (Truck_Cargo)HFLNOTINITED;
	}

	if (!IsLive()) {
		return (Truck_Cargo)-1;
	}

	return (Truck_Cargo)(((*unitArray)[unitID].weaponCargo) & 0xF);
}

int UnitEx::GetWorkersInTraining()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	if (!IsLive()) {
		return -1;
	}

	return (int)((*unitArray)[unitID].workersInTraining);
}

map_id UnitEx::GetFactoryCargo(int bay)
{
	if (!isInited) {
		return (map_id)HFLNOTINITED;
	}

	if (!IsLive()) {
		return (map_id)-1;
	}

	if (bay < 0 || bay > 5) {
		return (map_id)-1;
	}

	return (map_id)(*unitArray)[unitID].bayItem[bay];
}

map_id UnitEx::GetFactoryCargoWeapon(int bay)
{
	if (!isInited) {
		return (map_id)HFLNOTINITED;
	}

	if (!IsLive()) {
		return (map_id)-1;
	}

	if (bay < 0 || bay > 5) {
		return (map_id)-1;
	}

	return (map_id)(*unitArray)[unitID].bayWeaponCargo[bay];
}

map_id UnitEx::GetLaunchPadCargo()
{
	if (!isInited) {
		return (map_id)HFLNOTINITED;
	}

	if (!IsLive()) {
		return (map_id)-1;
	}

	return (map_id)(*unitArray)[unitID].launchPadCargo;
}

void UnitEx::SetLaunchPadCargo(map_id moduleType)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	(*unitArray)[unitID].launchPadCargo = moduleType;
}

int UnitEx::GetLights()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	if (!IsLive()) {
		return -1;
	}

	return (*unitArray)[unitID].flags & flagHeadlights;
}

int UnitEx::GetDoubleFireRate()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	if (!IsLive()) {
		return -1;
	}

	return (*unitArray)[unitID].flags & flagDoubleFireRate;
}

int UnitEx::GetInvisible()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	if (!IsLive()) {
		return -1;
	}

	return (*unitArray)[unitID].flags & flagInvisible;
}

int UnitEx::HasPower()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	if (!IsLive()) {
		return -1;
	}

	return (*unitArray)[unitID].flags & flagPower;
}

int UnitEx::HasWorkers()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	if (!IsLive()) {
		return -1;
	}

	return (*unitArray)[unitID].flags & flagWorkers;
}

int UnitEx::HasScientists()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	if (!IsLive()) {
		return -1;
	}

	return (*unitArray)[unitID].flags & flagScientists;
}

int UnitEx::IsInfected()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	if (!IsLive()) {
		return -1;
	}

	return (*unitArray)[unitID].flags & flagInfected;
}

void UnitEx::SetDoubleFireRate(int boolOn)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	if (boolOn) {
		(*unitArray)[unitID].flags |= flagDoubleFireRate;
	}
	else {
		(*unitArray)[unitID].flags &= ~flagDoubleFireRate;
	}
}

void UnitEx::SetInvisible(int boolOn)
{
	if (!isInited) {
		return;
	}

	if (!IsLive()) {
		return;
	}

	if (boolOn) {
		(*unitArray)[unitID].flags |= flagInvisible;
	}
	else {
		(*unitArray)[unitID].flags &= ~flagInvisible;
	}
}

LOCATION UnitEx::GetDockLocation()
{
	if (!isInited) {
		return LOCATION(HFLNOTINITED, HFLNOTINITED);
	}

	if (!IsLive()) {
		return LOCATION(-1, -1);
	}

	LOCATION loc;

	void (__fastcall *func)(OP2Unit *classPtr, int dummy, LOCATION *loc) = (void (__fastcall *)(OP2Unit*,int,LOCATION*))(imageBase + 0x82F40);
	func(&(*unitArray)[unitID], 0, &loc);

	return loc;
}

UnitInfo UnitEx::GetUnitInfo()
{
	if (!isInited) {
		return UnitInfo(mapNone);
	}

	if (!IsLive()) {
		return UnitInfo(mapAny);
	}

	return UnitInfo(GetType());
}

void UnitEx::SetAnimation(int animIdx, int animDelay, int animStartDelay, int boolInvisible, int boolSkipDoDeath)
{
	if (!isInited) {
		return;
	}

	void (__fastcall *func)(OP2Unit *classPtr, int dummy, int animIdx, int animDelay, int animStartDelay, int boolInvisible, int boolSkipDoDeath) = (void (__fastcall *)(OP2Unit*,int,int,int,int,int,int))(imageBase + 0x5110);

	func(&(*unitArray)[unitID], 0, animIdx, animDelay, animStartDelay, boolInvisible, boolSkipDoDeath);
}

int UnitEx::GetNumTruckLoadsSoFar()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	BeaconData* p = (BeaconData*)((int)(*unitArray) + (unitID*120) + 0x58);
	return p->numTruckLoadsSoFar;
}
int UnitEx::GetBarYield()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	BeaconData* p = (BeaconData*)((int)(*unitArray) + (unitID * 120) + 0x58);
	return p->barYield;
}
int UnitEx::GetVariant()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	BeaconData* p = (BeaconData*)((int)(*unitArray) + (unitID * 120) + 0x58);
	return p->variant;
}
int UnitEx::GetOreType()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	BeaconData* p = (BeaconData*)((int)(*unitArray) + (unitID * 120) + 0x58);
	return p->oreType;
}
int UnitEx::GetSurveyedBy()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	BeaconData* p = (BeaconData*)((int)(*unitArray) + (unitID * 120) + 0x58);
	return p->surveyedBy;
}

int UnitEx::GetLabCurrentTopic()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	LabData* p = (LabData*)((int)(*unitArray) + (unitID * 120) + 0x24);
	return p->techNum;
}

int UnitEx::GetLabScientistCount()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	LabData* p = (LabData*)((int)(*unitArray) + (unitID * 120) + 0x24);
	return p->numScientists;
}

void UnitEx::SetLabScientistCount(int numScientists)
{
	if (!isInited) {
		return;
	}

	LabData* p = (LabData*)((int)(*unitArray) + (unitID * 120) + 0x24);
	p->numScientists = numScientists;
}

