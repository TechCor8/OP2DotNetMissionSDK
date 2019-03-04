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
	public class Unit : IDisposable
	{
		private IntPtr m_Handle;

		/// <summary>
		/// Creates a Unit object.
		/// Unit must be disposed when no longer needed.
		/// </summary>
		public Unit()
		{
			m_Handle = Unit_Create();
		}

		public IntPtr GetHandle()			{ return m_Handle;						}

		// Common
		// [Get]
		public map_id GetUnitType()			{ return Unit_GetType(m_Handle);		}
		public int GetOwnerID()				{ return Unit_OwnerID(m_Handle);		}
		public bool IsBuilding()			{ return Unit_IsBuilding(m_Handle) != 0;}
		public bool IsVehicle()				{ return Unit_IsVehicle(m_Handle) != 0;	}
		public bool IsBusy()				{ return Unit_IsBusy(m_Handle) != 0;	}
		public bool IsLive()				{ return Unit_IsLive(m_Handle) != 0;	}
		public bool IsEMPed()				{ return Unit_IsEMPed(m_Handle) != 0;	}
		public int GetTileX()				{ return Unit_GetTileX(m_Handle);		}
		public int GetTileY()				{ return Unit_GetTileY(m_Handle);		}

		// [Set]
		public void SetDamage(int damage)					{ Unit_SetDamage(m_Handle, damage);							}
		public void SetCanAutoTarget(bool autoTarget)		{ Unit_SetOppFiredUpon(m_Handle, autoTarget ? 1 : 0);		}

		// [Method]
		public void DoDeath()								{ Unit_DoDeath(m_Handle);									}
		public void DoSelfDestruct()						{ Unit_DoSelfDestruct(m_Handle);							}		// Order Unit to SelfDestruct

		// Order Unit to Transfer to another Player (Vehicle or Building)
		public void DoTransfer(int destPlayerNum)			{ Unit_DoTransfer(m_Handle, destPlayerNum);					}
	
		// Combat Units
		public map_id GetWeapon()							{ return Unit_GetWeapon(m_Handle);							}
		public void SetWeapon(map_id weaponType)			{ Unit_SetWeapon(m_Handle, weaponType);						}
		public void DoAttack(Unit targetUnit)				{ Unit_DoAttack(m_Handle, targetUnit.m_Handle);				}		// Order Unit to Attack target Unit

		// Vehicles
		public void DoSetLights(bool isOn)					{ Unit_DoSetLights(m_Handle, isOn ? 1 : 0);					}		// Order Unit to SetLights
		public void DoMove(int tileX, int tileY)			{ Unit_DoMove(m_Handle, tileX, tileY);						}		// Order Unit to Move
		// Specific Vehicle
		public map_id GetCargo()							{ return Unit_GetCargo(m_Handle);							}			// [Convec]
		public void DoBuild(map_id buildingType, int tileX, int tileY)	{ Unit_DoBuild(m_Handle, buildingType, tileX, tileY);	}	// [Convec]
		public void SetCargo(map_id cargoType, map_id weaponType)		{ Unit_SetCargo(m_Handle, cargoType, weaponType);		}	// [Convec]
		public void SetTruckCargo(Truck_Cargo cargoType, int amount)	{ Unit_SetTruckCargo(m_Handle, cargoType, amount);		}	// [Cargo Truck]

		// Buildings
		public void DoIdle()								{ Unit_DoIdle(m_Handle);									}
		public void DoUnIdle()								{ Unit_DoUnIdle(m_Handle);									}
		public void DoStop()								{ Unit_DoStop(m_Handle);									}
		public void DoInfect()								{ Unit_DoInfect(m_Handle);									}
		// Specific Building
		public map_id GetObjectOnPad()						{ return Unit_GetObjectOnPad(m_Handle);						}										// [Spaceport]
		public void DoLaunch(int destPixelX, int destPixelY, int bForceEnable)	{ Unit_DoLaunch(m_Handle, destPixelX, destPixelY, bForceEnable);	}			// [Spaceport]
		public void PutInGarage(int bayIndex, int tileX, int tileY)				{ Unit_PutInGarage(m_Handle, bayIndex, tileX, tileY);				}			// [Garage]
		public bool HasOccupiedBay()											{ return Unit_HasOccupiedBay(m_Handle) != 0;						}			// [Garage, StructureFactory, Spaceport]
		// [StructureFactory, Spaceport]  [Note: If items is an SULV, RLV, or EMP Missile, it is placed on the launch pad instead of in the bay]
		public void SetFactoryCargo(int bay, map_id unitType, map_id cargoOrWeaponType)		{ Unit_SetFactoryCargo(m_Handle, bay, unitType, cargoOrWeaponType); }
		// [Factory]  [Note: Sets weapon/cargo to mapNone, can't build Lynx/Panther/Tiger/GuardPostKits]
		public void DoDevelop(map_id itemToProduce)											{ Unit_DoDevelop(m_Handle, itemToProduce);				}
		public void ClearSpecialTarget()													{ Unit_ClearSpecialTarget(m_Handle);					}			// [Lab]

		// Wreckage
		public bool IsDiscovered()															{ return Unit_IsDiscovered(m_Handle) != 0;				}			// Wreckage



		[DllImport("NativeInterop.dll")] private static extern IntPtr Unit_Create();
		[DllImport("NativeInterop.dll")] private static extern void Unit_Release(IntPtr handle);

		// Common
		// [Get]
		[DllImport("NativeInterop.dll")] private static extern map_id Unit_GetType(IntPtr handle);
		[DllImport("NativeInterop.dll")] private static extern int Unit_OwnerID(IntPtr handle);
		[DllImport("NativeInterop.dll")] private static extern int Unit_IsBuilding(IntPtr handle);
		[DllImport("NativeInterop.dll")] private static extern int Unit_IsVehicle(IntPtr handle);
		[DllImport("NativeInterop.dll")] private static extern int Unit_IsBusy(IntPtr handle);
		[DllImport("NativeInterop.dll")] private static extern int Unit_IsLive(IntPtr handle);
		[DllImport("NativeInterop.dll")] private static extern int Unit_IsEMPed(IntPtr handle);
		[DllImport("NativeInterop.dll")] private static extern int Unit_GetTileX(IntPtr handle);
		[DllImport("NativeInterop.dll")] private static extern int Unit_GetTileY(IntPtr handle);

		// [Set]
		[DllImport("NativeInterop.dll")] private static extern int Unit_SetDamage(IntPtr handle, int damage);
		//void SetId(int newUnitId);													// Change referenced unit of this Proxy/Stub
		[DllImport("NativeInterop.dll")] private static extern int Unit_SetOppFiredUpon(IntPtr handle, int bTrue);

		// [Method]
		[DllImport("NativeInterop.dll")] private static extern void Unit_DoDeath(IntPtr handle);
		[DllImport("NativeInterop.dll")] private static extern void Unit_DoSelfDestruct(IntPtr handle);					// Order Unit to SelfDestruct
		[DllImport("NativeInterop.dll")] private static extern void Unit_DoTransfer(IntPtr handle, int destPlayerNum);  // Order Unit to Transfer to another Player (Vehicle or Building)

		// Combat Units
		[DllImport("NativeInterop.dll")] private static extern map_id Unit_GetWeapon(IntPtr handle);
		[DllImport("NativeInterop.dll")] private static extern void Unit_SetWeapon(IntPtr handle, map_id weaponType);
		[DllImport("NativeInterop.dll")] private static extern void Unit_DoAttack(IntPtr handle, IntPtr targetUnit);		// Order Unit to Attack target Unit

		// Vehicles
		[DllImport("NativeInterop.dll")] private static extern void Unit_DoSetLights(IntPtr handle, int boolOn);												// Order Unit to SetLights
		[DllImport("NativeInterop.dll")] private static extern void Unit_DoMove(IntPtr handle, int tileX, int tileY);											// Order Unit to Move
		// Specific Vehicle
		[DllImport("NativeInterop.dll")] private static extern map_id Unit_GetCargo(IntPtr handle);																// [Convec]
		[DllImport("NativeInterop.dll")] private static extern void Unit_DoBuild(IntPtr handle, map_id buildingType, int tileX, int tileY);						// [Convec]
		[DllImport("NativeInterop.dll")] private static extern void Unit_SetCargo(IntPtr handle, map_id cargoType, map_id weaponType);							// [Convec]
		[DllImport("NativeInterop.dll")] private static extern void Unit_SetTruckCargo(IntPtr handle, Truck_Cargo cargoType, int amount);						// [Cargo Truck]

		// Buildings
		[DllImport("NativeInterop.dll")] private static extern void Unit_DoIdle(IntPtr handle);
		[DllImport("NativeInterop.dll")] private static extern void Unit_DoUnIdle(IntPtr handle);
		[DllImport("NativeInterop.dll")] private static extern void Unit_DoStop(IntPtr handle);
		[DllImport("NativeInterop.dll")] private static extern void Unit_DoInfect(IntPtr handle);
		// Specific Building
		[DllImport("NativeInterop.dll")] private static extern map_id Unit_GetObjectOnPad(IntPtr handle);														// [Spaceport]
		[DllImport("NativeInterop.dll")] private static extern void Unit_DoLaunch(IntPtr handle, int destPixelX, int destPixelY, int bForceEnable);				// [Spaceport]
		[DllImport("NativeInterop.dll")] private static extern void Unit_PutInGarage(IntPtr handle, int bayIndex, int tileX, int tileY);						// [Garage]
		[DllImport("NativeInterop.dll")] private static extern int Unit_HasOccupiedBay(IntPtr handle);															// [Garage, StructureFactory, Spaceport]
		[DllImport("NativeInterop.dll")] private static extern void Unit_SetFactoryCargo(IntPtr handle, int bay, map_id unitType, map_id cargoOrWeaponType);	// [StructureFactory, Spaceport]  [Note: If items is an SULV, RLV, or EMP Missile, it is placed on the launch pad instead of in the bay]
		[DllImport("NativeInterop.dll")] private static extern void Unit_DoDevelop(IntPtr handle, map_id itemToProduce);										// [Factory]  [Note: Sets weapon/cargo to mapNone, can't build Lynx/Panther/Tiger/GuardPostKits]
		[DllImport("NativeInterop.dll")] private static extern void Unit_ClearSpecialTarget(IntPtr handle);														// [Lab]

		// Wreckage
		[DllImport("NativeInterop.dll")] private static extern int Unit_IsDiscovered(IntPtr handle);															// Wreckage


		// --- Release ---
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{
				// Release managed objects
			}

			// Release unmanaged resources
			Unit_Release(m_Handle);
		}

		~Unit()
		{
			Dispose(false);
		}
	}
}
