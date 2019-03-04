#pragma once
#ifndef OP2
#define OP2 __declspec(dllimport)
#endif

// Note: This file is used to define the Unit class. Use this class to
//		 manipulate all the units in the game.
// Note: The Unit class is also used in conjunction with the enumerator
//		 classes used to find units and traverse lists of units, one unit
//		 at a time. See Enumerator.h for details.

// External type names
enum map_id;
enum Truck_Cargo;
struct LOCATION;
struct CommandPacket;


// Note: This class controls Units and provides info on them. This class can
//		 be used to set cargo in ConVecs, Cargo Trucks, or factory bays.
//		 It can also be used to move units around the map and perform simple
//		 operations such as self destruct and headlight control.

class OP2 Unit
{
public:
	Unit();	// { unitID = 0; };
	Unit& operator = (const Unit& unit);
	int operator == (const Unit& unit) const;

	// Common
	// [Get]
	map_id GetType() const;
	int OwnerID() const;
	int IsBuilding() const;
	int IsVehicle() const;
	int IsBusy() const;
	int IsLive();
	int isEMPed() const;
	LOCATION Location() const;
	// [Set]
	void SetDamage(int damage);
	void SetId(int newUnitId);													// Change referenced unit of this Proxy/Stub
	void SetOppFiredUpon(int bTrue);											// Set if Unit is auto targetted
	// [Method]
	void DoDeath();
	void DoSelfDestruct();														// Order Unit to SelfDestruct
	void DoTransfer(int destPlayerNum);											// Order Unit to Transfer to another Player (Vehicle or Building)

	// Combat Units
	map_id GetWeapon() const;
	void SetWeapon(map_id weaponType);
	void DoAttack(Unit targetUnit);												// Order Unit to Attack target Unit

	// Vehicles
	void DoSetLights(int boolOn);												// Order Unit to SetLights
	void DoMove(LOCATION location);												// Order Unit to Move
	// Specific Vehicle
	map_id GetCargo() const;													// [Convec]
	void DoBuild(map_id buildingType, LOCATION location);						// [Convec]
	void SetCargo(map_id cargoType, map_id weaponType);							// [Convec]
	void SetTruckCargo(Truck_Cargo cargoType, int amount);						// [Cargo Truck]

	// Buildings
	void DoIdle();
	void DoUnIdle();
	void DoStop();
	void DoInfect();
	// Specific Building
	map_id GetObjectOnPad() const;												// [Spaceport]
	void DoLaunch(int destPixelX, int destPixelY, int bForceEnable);			// [Spaceport]
	void PutInGarage(int bayIndex, int tileX, int tileY);						// [Garage]
	int HasOccupiedBay() const;													// [Garage, StructureFactory, Spaceport]
	void SetFactoryCargo(int bay, map_id unitType, map_id cargoOrWeaponType);	// [StructureFactory, Spaceport]  [Note: If items is an SULV, RLV, or EMP Missile, it is placed on the launch pad instead of in the bay]
	void DoDevelop(map_id itemToProduce);										// [Factory]  [Note: Sets weapon/cargo to mapNone, can't build Lynx/Panther/Tiger/GuardPostKits]
	void ClearSpecialTarget();													// [Lab]

	// Wreckage
	int isDiscovered() const;													// Wreckage

protected:
	void DoSimpleCommand(int commandPacketType);
private:
	char* StoreSelf(CommandPacket& commandPacket) const;

public:	// Why not? :)
	int unitID;
};
