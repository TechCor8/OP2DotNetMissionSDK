using System;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK.HFL
{
	/// <summary>
	/// Extension class for Unit.
	/// </summary>
	public class UnitEx : Unit
	{
		public UnitEx(int stubIndex) : base(stubIndex)
		{
		}

		public void DoAttack(int tileX, int tileY)									{ UnitEx_DoAttack(m_StubIndex, tileX, tileY);													}
		public void DoDoze(MAP_RECT dozeArea)										{ UnitEx_DoDoze(m_StubIndex, dozeArea.xMin, dozeArea.yMin, dozeArea.xMax, dozeArea.yMax);		}

		public void DoDock(UnitEx targetDock)										{ LOCATION tile = targetDock.GetDockLocation(); UnitEx_DoDock(m_StubIndex, tile.x, tile.y);		}
		public void DoDock(int tileX, int tileY)									{ UnitEx_DoDock(m_StubIndex, tileX, tileY);														}
		public void DoDockAtGarage(int tileX, int tileY)							{ UnitEx_DoDockAtGarage(m_StubIndex, tileX, tileY);												}

		public void DoStandGround(int tileX, int tileY)								{ UnitEx_DoStandGround(m_StubIndex, tileX, tileY);												}
		public void DoRemoveWall(MAP_RECT area)										{ UnitEx_DoRemoveWall(m_StubIndex, area.xMin, area.yMin, area.xMax, area.yMax);					}

		public void DoProduce(map_id unitType, map_id cargoWeaponType)				{ UnitEx_DoProduce(m_StubIndex, unitType, cargoWeaponType);										}

		public void DoTransferCargo(int bay)										{ UnitEx_DoTransferCargo(m_StubIndex, bay);														}
		public void DoLoadCargo()													{ UnitEx_DoLoadCargo(m_StubIndex);																}
		public void DoUnloadCargo()													{ UnitEx_DoUnloadCargo(m_StubIndex);															}
		public void DoDumpCargo()													{ UnitEx_DoDumpCargo(m_StubIndex);																}

		public void DoResearch(int techID, int numScientists)						{ UnitEx_DoResearch(m_StubIndex, techID, numScientists);										}
		public void DoTrainScientists(int numScientists)							{ UnitEx_DoTrainScientists(m_StubIndex, numScientists);											}

		public void DoRepair(Unit targetUnit)										{ UnitEx_DoRepair(m_StubIndex, targetUnit.GetStubIndex());										}
		public void DoReprogram(Unit targetUnit)									{ UnitEx_DoReprogram(m_StubIndex, targetUnit.GetStubIndex());									}
		public void DoDismantle(Unit targetUnit)									{ UnitEx_DoDismantle(m_StubIndex, targetUnit.GetStubIndex());									}

		public void DoSalvage(MAP_RECT salvageArea, Unit targetGORF)				{ UnitEx_DoSalvage(m_StubIndex, salvageArea.xMin, salvageArea.yMin, salvageArea.xMax, salvageArea.yMax, targetGORF.GetStubIndex());		}
		public void DoGuard(Unit targetUnit)										{ UnitEx_DoGuard(m_StubIndex, targetUnit.GetStubIndex());										}
		public void DoPoof()														{ UnitEx_DoPoof(m_StubIndex);																	}

		public CommandType GetLastCommand()											{ return UnitEx_GetLastCommand(m_StubIndex);													}
		public ActionType GetCurAction()											{ return UnitEx_GetCurAction(m_StubIndex);														}

		public int CreatorID()														{ return UnitEx_CreatorID(m_StubIndex);															}

		public bool IsEMPedEx()														{ return UnitEx_IsEMPedEx(m_StubIndex) > 0;														}
		public bool IsStickyfoamed()												{ return UnitEx_IsStickyfoamed(m_StubIndex) > 0;												}
		public bool IsESGed()														{ return UnitEx_IsESGed(m_StubIndex) > 0;														}

		public int GetDamage()														{ return UnitEx_GetDamage(m_StubIndex);															}

		public int GetCargoAmount()													{ return UnitEx_GetCargoAmount(m_StubIndex);													}
		public TruckCargo GetCargoType()											{ return UnitEx_GetCargoType(m_StubIndex);														}
		public map_id GetFactoryCargo(int bay)										{ return UnitEx_GetFactoryCargo(m_StubIndex, bay);												}
		public map_id GetFactoryCargoWeapon(int bay)								{ return UnitEx_GetFactoryCargoWeapon(m_StubIndex, bay);										}

		public bool GetLights()														{ return UnitEx_GetLights(m_StubIndex) > 0;														}

		public bool GetDoubleFireRate()												{ return UnitEx_GetDoubleFireRate(m_StubIndex) > 0;												}
		public bool GetInvisible()													{ return UnitEx_GetInvisible(m_StubIndex) > 0;													}

		public void SetDoubleFireRate(bool active)									{ UnitEx_SetDoubleFireRate(m_StubIndex, active ? 1 : 0);										}
		public void SetInvisible(bool active)										{ UnitEx_SetInvisible(m_StubIndex, active ? 1 : 0);												}

		public LOCATION GetDockLocation()
		{
			LOCATION pos;
			long result = UnitEx_GetDockLocation(m_StubIndex);
			pos.x = (int)(result & uint.MaxValue);
			pos.y = (int)(result >> 32);
			return pos;
		}

		public UnitInfo GetUnitInfo()												{ return new UnitInfo(GetUnitType());															}

		public void SetAnimation(int animIdx, int animDelay, int animStartDelay, bool invisible, bool skipDoDeath)	{ UnitEx_SetAnimation(m_StubIndex, animIdx, animDelay, animStartDelay, invisible ? 1 : 0, skipDoDeath ? 1 : 0);	}



		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoAttack(int unitID, int tileX, int tileY);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoDoze(int unitID, int xMin, int yMin, int xMax, int yMax);

		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoDock(int unitID, int tileX, int tileY);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoDockAtGarage(int unitID, int tileX, int tileY);

		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoStandGround(int unitID, int tileX, int tileY);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoRemoveWall(int unitID, int xMin, int yMin, int xMax, int yMax);

		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoProduce(int unitID, map_id unitType, map_id cargoWeaponType);

		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoTransferCargo(int unitID, int bay);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoLoadCargo(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoUnloadCargo(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoDumpCargo(int unitID);

		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoResearch(int unitID, int techID, int numScientists);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoTrainScientists(int unitID, int numScientists);

		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoRepair(int unitID, int targetUnitID);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoReprogram(int unitID, int targetUnitID);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoDismantle(int unitID, int targetUnitID);

		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoSalvage(int unitID, int xMin, int yMin, int xMax, int yMax, int targetGorfID);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoGuard(int unitID, int targetUnitID);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoPoof(int unitID);

		[DllImport("DotNetInterop.dll")] private static extern CommandType UnitEx_GetLastCommand(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern ActionType UnitEx_GetCurAction(int unitID);

		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_CreatorID(int unitID);

		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_IsEMPedEx(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_IsStickyfoamed(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_IsESGed(int unitID);

		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetDamage(int unitID);

		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetCargoAmount(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern TruckCargo UnitEx_GetCargoType(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern map_id UnitEx_GetFactoryCargo(int unitID, int bay);
		[DllImport("DotNetInterop.dll")] private static extern map_id UnitEx_GetFactoryCargoWeapon(int unitID, int bay);

		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetLights(int unitID);

		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetDoubleFireRate(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetInvisible(int unitID);

		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_SetDoubleFireRate(int unitID, int boolOn);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_SetInvisible(int unitID, int boolOn);

		[DllImport("DotNetInterop.dll")] private static extern long UnitEx_GetDockLocation(int unitID);

		// No need to export this. In caller, do new UnitInfo(unitEx.GetType()).
		//extern EXPORT UnitInfo* __stdcall UnitEx_GetUnitInfo(int unitID);

		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_SetAnimation(int unitID, int animIdx, int animDelay, int animStartDelay, int boolInvisible, int boolSkipDoDeath);
	}
}
