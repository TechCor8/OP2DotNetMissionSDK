using DotNetMissionSDK.Async;
using System;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK.HFL
{
	/// <summary>
	/// Extension class for Unit.
	/// </summary>
	public class UnitEx : Unit
	{
		/// <summary>
		/// Creates an empty UnitEx stub. Call SetStubIndex with a valid unit before using.
		/// </summary>
		public UnitEx() { }

		public UnitEx(int stubIndex) : base(stubIndex) { }

		public PlayerEx GetOwner()													{ ThreadAssert.MainThreadRequired();	return TethysGame.GetPlayer(GetOwnerID());													}

		public void DoAttack(int tileX, int tileY)									{ ThreadAssert.MainThreadRequired();	UnitEx_DoAttack(m_StubIndex, tileX, tileY);													}
		public void DoDeployMiner(int tileX, int tileY)								{ ThreadAssert.MainThreadRequired();	UnitEx_DoDeployMiner(m_StubIndex, tileX, tileY);											}
		public void DoDoze(MAP_RECT dozeArea)										{ ThreadAssert.MainThreadRequired();	UnitEx_DoDoze(m_StubIndex, dozeArea.xMin, dozeArea.yMin, dozeArea.xMax, dozeArea.yMax);		}

		public void DoDock(UnitEx targetDock)										{ ThreadAssert.MainThreadRequired();	LOCATION tile = targetDock.GetDockLocation(); DoDock(tile.x, tile.y);						}
		public void DoDock(int tileX, int tileY)									{ ThreadAssert.MainThreadRequired();	UnitEx_DoDock(m_StubIndex, tileX, tileY);													}
		public void DoDockAtGarage(int tileX, int tileY)							{ ThreadAssert.MainThreadRequired();	UnitEx_DoDockAtGarage(m_StubIndex, tileX, tileY);											}

		public void DoStandGround(int tileX, int tileY)								{ ThreadAssert.MainThreadRequired();	UnitEx_DoStandGround(m_StubIndex, tileX, tileY);											}
		public void DoBuildWall(map_id wallType, MAP_RECT area)						{ ThreadAssert.MainThreadRequired();	UnitEx_DoBuildWall(m_StubIndex, wallType, area.xMin, area.yMin, area.xMax, area.yMax);		}
		public void DoRemoveWall(MAP_RECT area)										{ ThreadAssert.MainThreadRequired();	UnitEx_DoRemoveWall(m_StubIndex, area.xMin, area.yMin, area.xMax, area.yMax);				}

		public void DoProduce(map_id unitType, map_id cargoWeaponType)				{ ThreadAssert.MainThreadRequired();	UnitEx_DoProduce(m_StubIndex, unitType, cargoWeaponType);									}

		public void DoTransferCargo(int bay)										{ ThreadAssert.MainThreadRequired();	UnitEx_DoTransferCargo(m_StubIndex, bay);													}
		public void DoLoadCargo()													{ ThreadAssert.MainThreadRequired();	UnitEx_DoLoadCargo(m_StubIndex);															}
		public void DoUnloadCargo()													{ ThreadAssert.MainThreadRequired();	UnitEx_DoUnloadCargo(m_StubIndex);															}
		public void DoDumpCargo()													{ ThreadAssert.MainThreadRequired();	UnitEx_DoDumpCargo(m_StubIndex);															}

		public void DoResearch(int techIndex, int numScientists)					{ ThreadAssert.MainThreadRequired();	UnitEx_DoResearch(m_StubIndex, techIndex, numScientists);										}
		public void DoTrainScientists(int numScientists)							{ ThreadAssert.MainThreadRequired();	UnitEx_DoTrainScientists(m_StubIndex, numScientists);										}

		public void DoRepair(Unit targetUnit)										{ ThreadAssert.MainThreadRequired();	UnitEx_DoRepair(m_StubIndex, targetUnit.GetStubIndex());									}
		public void DoReprogram(Unit targetUnit)									{ ThreadAssert.MainThreadRequired();	UnitEx_DoReprogram(m_StubIndex, targetUnit.GetStubIndex());									}
		public void DoDismantle(Unit targetUnit)									{ ThreadAssert.MainThreadRequired();	UnitEx_DoDismantle(m_StubIndex, targetUnit.GetStubIndex());									}

		public void DoSalvage(MAP_RECT salvageArea, Unit targetGORF)				{ ThreadAssert.MainThreadRequired();	UnitEx_DoSalvage(m_StubIndex, salvageArea.xMin, salvageArea.yMin, salvageArea.xMax, salvageArea.yMax, targetGORF.GetStubIndex());		}
		public void DoGuard(Unit targetUnit)										{ ThreadAssert.MainThreadRequired();	UnitEx_DoGuard(m_StubIndex, targetUnit.GetStubIndex());										}
		public void DoPoof()														{ ThreadAssert.MainThreadRequired();	UnitEx_DoPoof(m_StubIndex);																	}

		public CommandType GetLastCommand()											{ ThreadAssert.MainThreadRequired();	return UnitEx_GetLastCommand(m_StubIndex);													}
		public ActionType GetCurAction()											{ ThreadAssert.MainThreadRequired();	return UnitEx_GetCurAction(m_StubIndex);													}

		public int CreatorID()														{ ThreadAssert.MainThreadRequired();	return UnitEx_CreatorID(m_StubIndex);														}

		public int GetTimeEMPed()													{ ThreadAssert.MainThreadRequired();	return UnitEx_GetTimeEMPed(m_StubIndex);													}
		public bool IsStickyfoamed()												{ ThreadAssert.MainThreadRequired();	return UnitEx_IsStickyfoamed(m_StubIndex) > 0;												}
		public bool IsESGed()														{ ThreadAssert.MainThreadRequired();	return UnitEx_IsESGed(m_StubIndex) > 0;														}

		public int GetDamage()														{ ThreadAssert.MainThreadRequired();	return UnitEx_GetDamage(m_StubIndex);														}

		public int GetCargoAmount()													{ ThreadAssert.MainThreadRequired();	return UnitEx_GetCargoAmount(m_StubIndex);													}
		public TruckCargo GetCargoType()											{ ThreadAssert.MainThreadRequired();	return UnitEx_GetCargoType(m_StubIndex);													}
		public int GetWorkersInTraining()											{ ThreadAssert.MainThreadRequired();	return UnitEx_GetWorkersInTraining(m_StubIndex);											}
		public map_id GetFactoryCargo(int bay)										{ ThreadAssert.MainThreadRequired();	return UnitEx_GetFactoryCargo(m_StubIndex, bay);											}
		public map_id GetFactoryCargoWeapon(int bay)								{ ThreadAssert.MainThreadRequired();	return UnitEx_GetFactoryCargoWeapon(m_StubIndex, bay);										}
		public map_id GetLaunchpadCargo()											{ ThreadAssert.MainThreadRequired();	return UnitEx_GetLaunchPadCargo(m_StubIndex);												}
		public void SetLaunchpadCargo(map_id moduleType)							{ ThreadAssert.MainThreadRequired();	UnitEx_SetLaunchPadCargo(m_StubIndex, moduleType);											}

		public bool GetLights()														{ ThreadAssert.MainThreadRequired();	return UnitEx_GetLights(m_StubIndex) > 0;													}
		public bool HasPower()														{ ThreadAssert.MainThreadRequired();	return UnitEx_HasPower(m_StubIndex) > 0;													}
		public bool HasWorkers()													{ ThreadAssert.MainThreadRequired();	return UnitEx_HasWorkers(m_StubIndex) > 0;													}
		public bool HasScientists()													{ ThreadAssert.MainThreadRequired();	return UnitEx_HasScientists(m_StubIndex) > 0;												}
		public bool IsInfected()													{ ThreadAssert.MainThreadRequired();	return UnitEx_IsInfected(m_StubIndex) > 0;													}

		public bool GetDoubleFireRate()												{ ThreadAssert.MainThreadRequired();	return UnitEx_GetDoubleFireRate(m_StubIndex) > 0;											}
		public bool GetInvisible()													{ ThreadAssert.MainThreadRequired();	return UnitEx_GetInvisible(m_StubIndex) > 0;												}

		public void SetDoubleFireRate(bool active)									{ ThreadAssert.MainThreadRequired();	UnitEx_SetDoubleFireRate(m_StubIndex, active ? 1 : 0);										}
		public void SetInvisible(bool active)										{ ThreadAssert.MainThreadRequired();	UnitEx_SetInvisible(m_StubIndex, active ? 1 : 0);											}

		public UnitInfo GetUnitInfo()												{ ThreadAssert.MainThreadRequired();	return new UnitInfo(GetUnitType());															}
		public UnitInfo GetWeaponInfo()												{ ThreadAssert.MainThreadRequired();	return new UnitInfo(GetWeapon());															}

		public void SetAnimation(int animIdx, int animDelay, int animStartDelay, bool invisible, bool skipDoDeath)	{ ThreadAssert.MainThreadRequired();	UnitEx_SetAnimation(m_StubIndex, animIdx, animDelay, animStartDelay, invisible ? 1 : 0, skipDoDeath ? 1 : 0);	}

		// Mining beacon
		public int GetNumTruckLoadsSoFar()											{ ThreadAssert.MainThreadRequired();	return UnitEx_GetNumTruckLoadsSoFar(m_StubIndex);											}
		public Yield GetBarYield()													{ ThreadAssert.MainThreadRequired();	return (Yield)UnitEx_GetBarYield(m_StubIndex);												}
		public Variant GetVariant()													{ ThreadAssert.MainThreadRequired();	return (Variant)UnitEx_GetVariant(m_StubIndex);												}
		public BeaconType GetOreType()												{ ThreadAssert.MainThreadRequired();	return (BeaconType)UnitEx_GetOreType(m_StubIndex);											}
		public bool GetSurveyedBy(int playerID)										{ ThreadAssert.MainThreadRequired();	return ((1 << playerID) & UnitEx_GetSurveyedBy(m_StubIndex)) != 0;							}
		public int GetSurveyedFlags()												{ ThreadAssert.MainThreadRequired();	return UnitEx_GetSurveyedBy(m_StubIndex);													}

		// Lab
		public int GetLabCurrentTopic()												{ ThreadAssert.MainThreadRequired();	return UnitEx_GetLabCurrentTopic(m_StubIndex);												}
		public int GetLabScientistCount()											{ ThreadAssert.MainThreadRequired();	return UnitEx_GetLabScientistCount(m_StubIndex);											}
		public void SetLabScientistCount(int numScientists)							{ ThreadAssert.MainThreadRequired();	UnitEx_SetLabScientistCount(m_StubIndex, numScientists);									}

		public bool HasWeapon()														{ ThreadAssert.MainThreadRequired();	return GetWeaponInfo().GetWeaponStrength() > 0;												}

		public bool HasEmptyBay()													{ ThreadAssert.MainThreadRequired();	return GetBayWithCargo(map_id.None) >= 0;													}
		public bool HasBayWithCargo(map_id cargoType)								{ ThreadAssert.MainThreadRequired();	return GetBayWithCargo(cargoType) >= 0;														}

		public MAP_RECT GetRect(bool includeBulldozedArea=false)					{ ThreadAssert.MainThreadRequired();	return GetUnitInfo().GetRect(GetPosition(), includeBulldozedArea);							}

		/// <summary>
		/// Returns the bay index that contains cargo type.
		/// Returns -1 if cargo type is not found.
		/// </summary>
		/// <param name="cargoType">The cargo type to search for.</param>
		public int GetBayWithCargo(map_id cargoType)
		{
			ThreadAssert.MainThreadRequired();

			UnitInfo info = GetUnitInfo();
			int bayCount = info.GetNumStorageBays(GetOwnerID());
			for (int i=0; i < bayCount; ++i)
			{
				if (GetFactoryCargo(i) == cargoType)
					return i;
			}

			return -1;
		}

		public LOCATION GetDockLocation()
		{
			ThreadAssert.MainThreadRequired();

			LOCATION pos;
			long result = UnitEx_GetDockLocation(m_StubIndex);
			pos.x = (int)(result & uint.MaxValue);
			pos.y = (int)(result >> 32);
			return pos;
		}

		public bool IsOnDock(UnitEx unitWithDock)
		{
			ThreadAssert.MainThreadRequired();

			LOCATION dock = unitWithDock.GetDockLocation();
			return dock.Equals(GetPosition());
		}

		// True if structure is enabled.
		public bool IsEnabled()														{ ThreadAssert.MainThreadRequired();	return UnitEx_GetLights(m_StubIndex) == 1;													}

		// True if structure is disabled. A structure is not disabled if it is idle.
		public bool IsDisabled()													{ ThreadAssert.MainThreadRequired();	return UnitEx_GetLights(m_StubIndex) == 0 && UnitEx_GetLastCommand(m_StubIndex) != CommandType.ctMoIdle;	}

		// Swaps cargo between bay and launchpad
		public void DoTransferLaunchpadCargo(int bay)
		{
			ThreadAssert.MainThreadRequired();

			if (GetUnitType() != map_id.Spaceport)
				return;

			map_id objectOnPad = GetObjectOnPad();
			if (objectOnPad != map_id.SULV && objectOnPad != map_id.RLV)
				return;

			int currentCargo = UnitEx_GetUnknownValue(m_StubIndex, 46);
			int bayCargo = (int)UnitEx_GetFactoryCargo(m_StubIndex, bay);
			UnitEx_SetUnknownValue(m_StubIndex, 46, bayCargo);
			SetFactoryCargo(bay, (map_id)currentCargo, map_id.None);
		}

		public string VarDump()
		{
			ThreadAssert.MainThreadRequired();

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.AppendLine("** " + GetUnitType() + " Vars **");

			for (int i=0; i < 48; ++i)
				sb.AppendLine(UnitEx_GetUnknownValue(m_StubIndex, i).ToString());

			sb.AppendLine("** Dump Complete **");

			return sb.ToString();
		}

		/// <summary>
		/// Updates this vehicle.
		/// Call every frame.
		/// </summary>
		public virtual void Update()
		{
		}

		/// <summary>
		/// Called when unit ceases to exist.
		/// </summary>
		public virtual void OnDestroy()
		{
		}


		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoAttack(int unitID, int tileX, int tileY);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoDeployMiner(int unitID, int tileX, int tileY);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoDoze(int unitID, int xMin, int yMin, int xMax, int yMax);

		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoDock(int unitID, int tileX, int tileY);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoDockAtGarage(int unitID, int tileX, int tileY);

		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoStandGround(int unitID, int tileX, int tileY);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_DoBuildWall(int unitID, map_id wallType, int xMin, int yMin, int xMax, int yMax);
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

		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetTimeEMPed(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_IsStickyfoamed(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_IsESGed(int unitID);

		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetDamage(int unitID);

		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetCargoAmount(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern TruckCargo UnitEx_GetCargoType(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetWorkersInTraining(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern map_id UnitEx_GetFactoryCargo(int unitID, int bay);
		[DllImport("DotNetInterop.dll")] private static extern map_id UnitEx_GetFactoryCargoWeapon(int unitID, int bay);
		[DllImport("DotNetInterop.dll")] private static extern map_id UnitEx_GetLaunchPadCargo(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_SetLaunchPadCargo(int unitID, map_id moduleType);

		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetLights(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_HasPower(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_HasWorkers(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_HasScientists(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_IsInfected(int unitID);

		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetDoubleFireRate(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetInvisible(int unitID);

		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_SetDoubleFireRate(int unitID, int boolOn);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_SetInvisible(int unitID, int boolOn);

		[DllImport("DotNetInterop.dll")] private static extern long UnitEx_GetDockLocation(int unitID);

		// No need to export this. In caller, do new UnitInfo(unitEx.GetType()).
		//extern EXPORT UnitInfo* __stdcall UnitEx_GetUnitInfo(int unitID);

		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_SetAnimation(int unitID, int animIdx, int animDelay, int animStartDelay, int boolInvisible, int boolSkipDoDeath);

		// Mining beacon
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetNumTruckLoadsSoFar(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetBarYield(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetVariant(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetOreType(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetSurveyedBy(int unitID);

		// Lab
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetLabCurrentTopic(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetLabScientistCount(int unitID);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_SetLabScientistCount(int unitID, int numScientists);
		
		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_GetUnknownValue(int unitID, int index);
		[DllImport("DotNetInterop.dll")] private static extern void UnitEx_SetUnknownValue(int unitID, int index, int value);
	}
}
