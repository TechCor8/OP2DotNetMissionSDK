// Note: This file is used to define the Unit class. Use this class to
//		 manipulate all the units in the game.
// Note: The Unit class is also used in conjunction with the enumerator
//		 classes used to find units and traverse lists of units, one unit
//		 at a time. See Enumerator.h for details.

// Note: This class controls Units and provides info on them. This class can
//		 be used to set cargo in ConVecs, Cargo Trucks, or factory bays.
//		 It can also be used to move units around the map and perform simple
//		 operations such as self destruct and headlight control.

using DotNetMissionSDK.Async;
using System;
using System.Runtime.InteropServices;


namespace DotNetMissionSDK
{
	public class Unit
	{
		protected int m_StubIndex;
		
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
		public map_id GetUnitType()			{ ThreadAssert.MainThreadRequired(); return Unit_GetType(m_StubIndex);				}
		public int GetOwnerID()				{ ThreadAssert.MainThreadRequired(); return Unit_OwnerID(m_StubIndex);				} // The player that owns this unit
		public bool IsBuilding()			{ ThreadAssert.MainThreadRequired(); return Unit_IsBuilding(m_StubIndex) != 0;		}
		public bool IsVehicle()				{ ThreadAssert.MainThreadRequired(); return Unit_IsVehicle(m_StubIndex) != 0;		}
		public bool IsBusy()				{ ThreadAssert.MainThreadRequired(); return Unit_IsBusy(m_StubIndex) != 0;			}
		public bool IsLive()				{ ThreadAssert.MainThreadRequired(); return Unit_IsLive(m_StubIndex) != 0;			}
		public bool IsEMPed()				{ ThreadAssert.MainThreadRequired(); return Unit_IsEMPed(m_StubIndex) != 0;			}
		public int GetTileX()				{ ThreadAssert.MainThreadRequired(); return Unit_GetTileX(m_StubIndex);				}
		public int GetTileY()				{ ThreadAssert.MainThreadRequired(); return Unit_GetTileY(m_StubIndex);				}

		public LOCATION GetPosition()		{ ThreadAssert.MainThreadRequired(); return new LOCATION(GetTileX(), GetTileY());	}

		// [Set]
		public void SetDamage(int damage)					{ ThreadAssert.MainThreadRequired(); Unit_SetDamage(m_StubIndex, damage);						}
		public void SetCanAutoTarget(bool autoTarget)		{ ThreadAssert.MainThreadRequired(); Unit_SetOppFiredUpon(m_StubIndex, autoTarget ? 1 : 0);		}

		// [Method]
		public void DoDeath()								{ ThreadAssert.MainThreadRequired(); Unit_DoDeath(m_StubIndex);									}
		public void DoSelfDestruct()						{ ThreadAssert.MainThreadRequired(); Unit_DoSelfDestruct(m_StubIndex);							}	// Order Unit to SelfDestruct

		// Order Unit to Transfer to another Player (Vehicle or Building)
		public void DoTransfer(int destPlayerNum)			{ ThreadAssert.MainThreadRequired(); Unit_DoTransfer(m_StubIndex, destPlayerNum);				}
	
		// Combat Units
		public map_id GetWeapon()							{ ThreadAssert.MainThreadRequired(); return Unit_GetWeapon(m_StubIndex);						}
		public void SetWeapon(map_id weaponType)			{ ThreadAssert.MainThreadRequired(); Unit_SetWeapon(m_StubIndex, weaponType);					}
		public virtual void DoAttack(Unit targetUnit)		{ ThreadAssert.MainThreadRequired(); Unit_DoAttack(m_StubIndex, targetUnit.m_StubIndex);		}	// Order Unit to Attack target Unit

		// Vehicles
		public void DoSetLights(bool isOn)					{ ThreadAssert.MainThreadRequired(); Unit_DoSetLights(m_StubIndex, isOn ? 1 : 0);						}		// Order Unit to SetLights
		public void DoMove(int tileX, int tileY)			{ ThreadAssert.MainThreadRequired(); Unit_DoMove(m_StubIndex, tileX, tileY);							}		// Order Unit to Move
		// Specific Vehicle
		public map_id GetCargo()										{ ThreadAssert.MainThreadRequired(); return Unit_GetCargo(m_StubIndex);						}	// [Convec]
		public void DoBuild(map_id buildingType, int tileX, int tileY)	{ ThreadAssert.MainThreadRequired(); Unit_DoBuild(m_StubIndex, buildingType, tileX, tileY);	}	// [Convec]
		public void SetCargo(map_id cargoType, map_id weaponType)		{ ThreadAssert.MainThreadRequired(); Unit_SetCargo(m_StubIndex, cargoType, weaponType);		}	// [Convec]
		public void SetTruckCargo(TruckCargo cargoType, int amount)		{ ThreadAssert.MainThreadRequired(); Unit_SetTruckCargo(m_StubIndex, cargoType, amount);	}	// [Cargo Truck]

		// Buildings
		public void DoIdle()								{ ThreadAssert.MainThreadRequired(); Unit_DoIdle(m_StubIndex);											}
		public void DoUnIdle()								{ ThreadAssert.MainThreadRequired(); Unit_DoUnIdle(m_StubIndex);										}
		public void DoStop()								{ ThreadAssert.MainThreadRequired(); Unit_DoStop(m_StubIndex);											}
		public void DoInfect()								{ ThreadAssert.MainThreadRequired(); Unit_DoInfect(m_StubIndex);										}
		// Specific Building
		public map_id GetObjectOnPad()						{ ThreadAssert.MainThreadRequired(); return Unit_GetObjectOnPad(m_StubIndex);							}	// [Spaceport]
		public void DoLaunch(int destPixelX, int destPixelY, bool bForceEnable)	{ ThreadAssert.MainThreadRequired(); Unit_DoLaunch(m_StubIndex, destPixelX, destPixelY, bForceEnable ? 1 : 0);	}	// [Spaceport]
		public void PutInGarage(int bayIndex, int tileX, int tileY)				{ ThreadAssert.MainThreadRequired(); Unit_PutInGarage(m_StubIndex, bayIndex, tileX, tileY);						}	// [Garage]
		public bool HasOccupiedBay()											{ ThreadAssert.MainThreadRequired(); return Unit_HasOccupiedBay(m_StubIndex) != 0;								}	// [Garage, StructureFactory, Spaceport]
		// [StructureFactory, Spaceport]  [Note: If items is an SULV, RLV, or EMP Missile, it is placed on the launch pad instead of in the bay]
		public void SetFactoryCargo(int bay, map_id unitType, map_id cargoOrWeaponType)		{ ThreadAssert.MainThreadRequired(); Unit_SetFactoryCargo(m_StubIndex, bay, unitType, cargoOrWeaponType); }	// [Factory]  [Note: Sets weapon/cargo to mapNone, can't build Lynx/Panther/Tiger/GuardPostKits]
		public void DoDevelop(map_id itemToProduce)											{ ThreadAssert.MainThreadRequired(); Unit_DoDevelop(m_StubIndex, itemToProduce);			}
		public void ClearSpecialTarget()													{ ThreadAssert.MainThreadRequired(); Unit_ClearSpecialTarget(m_StubIndex);					}	// [Lab]

		// Wreckage
		public bool IsDiscovered()															{ ThreadAssert.MainThreadRequired(); return Unit_IsDiscovered(m_StubIndex) != 0;			}	// Wreckage



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
		[DllImport("DotNetInterop.dll")] private static extern int Unit_IsDiscovered(int stubIndex);                                                            // Wreckage

		public override bool Equals(object obj)
		{
			Unit unit = obj as Unit;
			if (unit == null)
				return false;

			return m_StubIndex == unit.m_StubIndex;
		}

		public override int GetHashCode()
		{
			return m_StubIndex;
		}
	}
}
