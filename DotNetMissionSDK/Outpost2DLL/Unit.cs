// Note: This file is used to define the Unit class. Use this class to
//		 manipulate all the units in the game.
// Note: The Unit class is also used in conjunction with the enumerator
//		 classes used to find units and traverse lists of units, one unit
//		 at a time. See Enumerator.h for details.

// Note: This class controls Units and provides info on them. This class can
//		 be used to set cargo in ConVecs, Cargo Trucks, or factory bays.
//		 It can also be used to move units around the map and perform simple
//		 operations such as self destruct and headlight control.

using System;
using System.Runtime.InteropServices;


namespace DotNetMissionSDK
{
	public class Unit
	{
		private int m_StubIndex;
		
		/// <summary>
		/// Creates a wrapper that references a unit stub.
		/// </summary>
		public Unit(int stubIndex)
		{
			m_StubIndex = stubIndex;
		}

		/// <summary>
		/// Creates an empty Unit stub. Call SetStubIndex with a valid unit before using.
		/// </summary>
		public Unit() { }

		public int GetStubIndex()					{ return m_StubIndex;				}
		public void SetStubIndex(int stubIndex)		{ m_StubIndex = stubIndex;			}

		// Common
		// [Get]
		public map_id GetUnitType()			{ return Unit_GetType(m_StubIndex);			}
		public int GetOwnerID()				{ return Unit_OwnerID(m_StubIndex);			}
		public bool IsBuilding()			{ return Unit_IsBuilding(m_StubIndex) != 0;	}
		public bool IsVehicle()				{ return Unit_IsVehicle(m_StubIndex) != 0;	}
		public bool IsBusy()				{ return Unit_IsBusy(m_StubIndex) != 0;		}
		public bool IsLive()				{ return Unit_IsLive(m_StubIndex) != 0;		}
		public bool IsEMPed()				{ return Unit_IsEMPed(m_StubIndex) != 0;	}
		public int GetTileX()				{ return Unit_GetTileX(m_StubIndex);		}
		public int GetTileY()				{ return Unit_GetTileY(m_StubIndex);		}

		// [Set]
		public void SetDamage(int damage)					{ Unit_SetDamage(m_StubIndex, damage);						}
		public void SetCanAutoTarget(bool autoTarget)		{ Unit_SetOppFiredUpon(m_StubIndex, autoTarget ? 1 : 0);	}

		// [Method]
		public void DoDeath()								{ Unit_DoDeath(m_StubIndex);								}
		public void DoSelfDestruct()						{ Unit_DoSelfDestruct(m_StubIndex);							}		// Order Unit to SelfDestruct

		// Order Unit to Transfer to another Player (Vehicle or Building)
		public void DoTransfer(int destPlayerNum)			{ Unit_DoTransfer(m_StubIndex, destPlayerNum);				}
	
		// Combat Units
		public map_id GetWeapon()							{ return Unit_GetWeapon(m_StubIndex);						}
		public void SetWeapon(map_id weaponType)			{ Unit_SetWeapon(m_StubIndex, weaponType);					}
		public void DoAttack(Unit targetUnit)				{ Unit_DoAttack(m_StubIndex, targetUnit.m_StubIndex);		}		// Order Unit to Attack target Unit

		// Vehicles
		public void DoSetLights(bool isOn)					{ Unit_DoSetLights(m_StubIndex, isOn ? 1 : 0);				}		// Order Unit to SetLights
		public void DoMove(int tileX, int tileY)			{ Unit_DoMove(m_StubIndex, tileX, tileY);					}		// Order Unit to Move
		// Specific Vehicle
		public map_id GetCargo()										{ return Unit_GetCargo(m_StubIndex);						}	// [Convec]
		public void DoBuild(map_id buildingType, int tileX, int tileY)	{ Unit_DoBuild(m_StubIndex, buildingType, tileX, tileY);	}	// [Convec]
		public void SetCargo(map_id cargoType, map_id weaponType)		{ Unit_SetCargo(m_StubIndex, cargoType, weaponType);		}	// [Convec]
		public void SetTruckCargo(TruckCargo cargoType, int amount)	{ Unit_SetTruckCargo(m_StubIndex, cargoType, amount);		}	// [Cargo Truck]

		// Buildings
		public void DoIdle()								{ Unit_DoIdle(m_StubIndex);									}
		public void DoUnIdle()								{ Unit_DoUnIdle(m_StubIndex);								}
		public void DoStop()								{ Unit_DoStop(m_StubIndex);									}
		public void DoInfect()								{ Unit_DoInfect(m_StubIndex);								}
		// Specific Building
		public map_id GetObjectOnPad()						{ return Unit_GetObjectOnPad(m_StubIndex);					}										// [Spaceport]
		public void DoLaunch(int destPixelX, int destPixelY, int bForceEnable)	{ Unit_DoLaunch(m_StubIndex, destPixelX, destPixelY, bForceEnable);	}			// [Spaceport]
		public void PutInGarage(int bayIndex, int tileX, int tileY)				{ Unit_PutInGarage(m_StubIndex, bayIndex, tileX, tileY);			}			// [Garage]
		public bool HasOccupiedBay()											{ return Unit_HasOccupiedBay(m_StubIndex) != 0;						}			// [Garage, StructureFactory, Spaceport]
		// [StructureFactory, Spaceport]  [Note: If items is an SULV, RLV, or EMP Missile, it is placed on the launch pad instead of in the bay]
		public void SetFactoryCargo(int bay, map_id unitType, map_id cargoOrWeaponType)		{ Unit_SetFactoryCargo(m_StubIndex, bay, unitType, cargoOrWeaponType); }
		// [Factory]  [Note: Sets weapon/cargo to mapNone, can't build Lynx/Panther/Tiger/GuardPostKits]
		public void DoDevelop(map_id itemToProduce)											{ Unit_DoDevelop(m_StubIndex, itemToProduce);			}
		public void ClearSpecialTarget()													{ Unit_ClearSpecialTarget(m_StubIndex);					}			// [Lab]

		// Wreckage
		public bool IsDiscovered()															{ return Unit_IsDiscovered(m_StubIndex) != 0;			}			// Wreckage



		// Common
		// [Get]
		[DllImport("DotNetInterop.dll")] private static extern map_id Unit_GetType(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern int Unit_OwnerID(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern int Unit_IsBuilding(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern int Unit_IsVehicle(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern int Unit_IsBusy(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern int Unit_IsLive(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern int Unit_IsEMPed(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern int Unit_GetTileX(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern int Unit_GetTileY(int stubIndex);

		// [Set]
		[DllImport("DotNetInterop.dll")] private static extern int Unit_SetDamage(int stubIndex, int damage);
		//void SetId(int newUnitId);													// Change referenced unit of this Proxy/Stub
		[DllImport("DotNetInterop.dll")] private static extern int Unit_SetOppFiredUpon(int stubIndex, int bTrue);

		// [Method]
		[DllImport("DotNetInterop.dll")] private static extern void Unit_DoDeath(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void Unit_DoSelfDestruct(int stubIndex);					// Order Unit to SelfDestruct
		[DllImport("DotNetInterop.dll")] private static extern void Unit_DoTransfer(int stubIndex, int destPlayerNum);  // Order Unit to Transfer to another Player (Vehicle or Building)

		// Combat Units
		[DllImport("DotNetInterop.dll")] private static extern map_id Unit_GetWeapon(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void Unit_SetWeapon(int stubIndex, map_id weaponType);
		[DllImport("DotNetInterop.dll")] private static extern void Unit_DoAttack(int stubIndex, int targetUnit);		// Order Unit to Attack target Unit

		// Vehicles
		[DllImport("DotNetInterop.dll")] private static extern void Unit_DoSetLights(int stubIndex, int boolOn);												// Order Unit to SetLights
		[DllImport("DotNetInterop.dll")] private static extern void Unit_DoMove(int stubIndex, int tileX, int tileY);											// Order Unit to Move
		// Specific Vehicle
		[DllImport("DotNetInterop.dll")] private static extern map_id Unit_GetCargo(int stubIndex);																// [Convec]
		[DllImport("DotNetInterop.dll")] private static extern void Unit_DoBuild(int stubIndex, map_id buildingType, int tileX, int tileY);						// [Convec]
		[DllImport("DotNetInterop.dll")] private static extern void Unit_SetCargo(int stubIndex, map_id cargoType, map_id weaponType);							// [Convec]
		[DllImport("DotNetInterop.dll")] private static extern void Unit_SetTruckCargo(int stubIndex, TruckCargo cargoType, int amount);						// [Cargo Truck]

		// Buildings
		[DllImport("DotNetInterop.dll")] private static extern void Unit_DoIdle(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void Unit_DoUnIdle(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void Unit_DoStop(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void Unit_DoInfect(int stubIndex);
		// Specific Building
		[DllImport("DotNetInterop.dll")] private static extern map_id Unit_GetObjectOnPad(int stubIndex);														// [Spaceport]
		[DllImport("DotNetInterop.dll")] private static extern void Unit_DoLaunch(int stubIndex, int destPixelX, int destPixelY, int bForceEnable);				// [Spaceport]
		[DllImport("DotNetInterop.dll")] private static extern void Unit_PutInGarage(int stubIndex, int bayIndex, int tileX, int tileY);						// [Garage]
		[DllImport("DotNetInterop.dll")] private static extern int Unit_HasOccupiedBay(int stubIndex);															// [Garage, StructureFactory, Spaceport]
		[DllImport("DotNetInterop.dll")] private static extern void Unit_SetFactoryCargo(int stubIndex, int bay, map_id unitType, map_id cargoOrWeaponType);	// [StructureFactory, Spaceport]  [Note: If items is an SULV, RLV, or EMP Missile, it is placed on the launch pad instead of in the bay]
		[DllImport("DotNetInterop.dll")] private static extern void Unit_DoDevelop(int stubIndex, map_id itemToProduce);										// [Factory]  [Note: Sets weapon/cargo to mapNone, can't build Lynx/Panther/Tiger/GuardPostKits]
		[DllImport("DotNetInterop.dll")] private static extern void Unit_ClearSpecialTarget(int stubIndex);														// [Lab]

		// Wreckage
		[DllImport("DotNetInterop.dll")] private static extern int Unit_IsDiscovered(int stubIndex);															// Wreckage
	}
}
