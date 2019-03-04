// Note: This file is used to define the enumerator classes exported
//		 from Outpost2.exe. These classes can be used to search for
//		 or traverse a list of units one unit at a time.

/*
// Private UnitNode structure that should have no external access
struct UnitNode
{
	UnitNode* prev;		// ** Maybe? (Guessed)
	UnitNode* next;
	Unit* unit;
};


// ------------------------------------------------------------------------
// Note: All Enumerators implement a GetNext function, which returns
//	0 if no Unit was found, or non-zero if a Unit was found.
//	If a Unit was found, then it's index is returned in the Unit proxy/stub
//	passed as the first parameter.
// ------------------------------------------------------------------------


// Group (enumerate all units in a group)
class OP2 GroupEnumerator
{
public:
	GroupEnumerator(ScGroup& group);
	GroupEnumerator& operator = (const GroupEnumerator& groupEnum);
	int GetNext(Unit& currentUnit);
public:	// Why not? =)
	UnitNode* currentUnitNode;
};

// Vehicles (enumerate all vehicles for a certain player)
class OP2 PlayerVehicleEnum
{
public:
	PlayerVehicleEnum(int playerNum);
	PlayerVehicleEnum& operator = (const PlayerVehicleEnum& playerVehicleEnum);
	int GetNext(Unit& currentUnit);
public:	// Why not? =)
	void* currentUnit;
};

// Buildings (enumerate all buildings of a certain type for a certain player)
class OP2 PlayerBuildingEnum
{
public:
	PlayerBuildingEnum(int playerNum, map_id buildingType);
	PlayerBuildingEnum& operator = (const PlayerBuildingEnum& playerBuildingEnum);
	int GetNext(class Unit &currentUnit);
public:	// Why not? =)
	void* currentUnit;
	map_id buildingType;
};

// Units (enumerate all units of a certain player)
class OP2 PlayerUnitEnum
{
public:
	PlayerUnitEnum(int playerNum);
	PlayerUnitEnum& operator = (const PlayerUnitEnum& playerUnitEnum);
	int GetNext(Unit& currentUnit);
public:	// Why not? =)
	void* currentUnit;
	int playerNum;
};

// InRange (enumerate all units within a given distance of a given location)
class OP2 InRangeEnumerator
{
public:
	InRangeEnumerator(const LOCATION& centerPoint, int maxTileDistance);
	InRangeEnumerator& operator = (const InRangeEnumerator& inRangeEnum);
	int GetNext(class Unit &currentUnit);
private:
	int unknown[13];	// **TODO** Fill in details
};

// InRect (enumerate all units within a given rectangle)
class OP2 InRectEnumerator
{
public:
	InRectEnumerator(const MAP_RECT& rect);
	InRectEnumerator& operator = (const InRectEnumerator& inRectEnum);
	int GetNext(Unit& currentUnit);
private:
	int unknown[13];	// **TODO** Fill in details
};

// Location (enumerate all units at a given location)
class OP2 LocationEnumerator
{
public:
	LocationEnumerator(const LOCATION& location);
	LocationEnumerator& operator = (const LocationEnumerator& locationEnum);
	int GetNext(Unit& currentUnit);
private:
	int unknown[13];	// **TODO** Fill in details
};

// Closest (enumerate all units ordered by their distance to a given location)
class OP2 ClosestEnumerator
{
public:
	ClosestEnumerator(const LOCATION& location);
	ClosestEnumerator& operator = (const ClosestEnumerator& closestEnum);
	int GetNext(Unit& currentUnit, unsigned long &pixelDistance);
private:
	int unknown[13];	// **TODO** Fill in details
};
*/