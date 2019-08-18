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

		public PlayerEx GetOwner()													{ return TethysGame.GetPlayer(GetOwnerID());													}

		public void DoAttack(int tileX, int tileY)									{ UnitEx_DoAttack(m_StubIndex, tileX, tileY);													}
		public void DoDeployMiner(int tileX, int tileY)								{ UnitEx_DoDeployMiner(m_StubIndex, tileX, tileY);												}
		public void DoDoze(MAP_RECT dozeArea)										{ UnitEx_DoDoze(m_StubIndex, dozeArea.xMin, dozeArea.yMin, dozeArea.xMax, dozeArea.yMax);		}

		public void DoDock(UnitEx targetDock)										{ LOCATION tile = targetDock.GetDockLocation(); DoDock(tile.x, tile.y);							}
		public void DoDock(int tileX, int tileY)									{ UnitEx_DoDock(m_StubIndex, tileX, tileY);														}
		public void DoDockAtGarage(int tileX, int tileY)							{ UnitEx_DoDockAtGarage(m_StubIndex, tileX, tileY);												}

		public void DoStandGround(int tileX, int tileY)								{ UnitEx_DoStandGround(m_StubIndex, tileX, tileY);												}
		public void DoBuildWall(map_id wallType, MAP_RECT area)						{ UnitEx_DoBuildWall(m_StubIndex, wallType, area.xMin, area.yMin, area.xMax, area.yMax);		}
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
		public int GetWorkersInTraining()											{ return UnitEx_GetWorkersInTraining(m_StubIndex);												}
		public map_id GetFactoryCargo(int bay)										{ return UnitEx_GetFactoryCargo(m_StubIndex, bay);												}
		public map_id GetFactoryCargoWeapon(int bay)								{ return UnitEx_GetFactoryCargoWeapon(m_StubIndex, bay);										}
		public map_id GetLaunchpadCargo()											{ return UnitEx_GetLaunchPadCargo(m_StubIndex);													}
		public void SetLaunchpadCargo(map_id moduleType)							{ UnitEx_SetLaunchPadCargo(m_StubIndex, moduleType);											}

		public bool GetLights()														{ return UnitEx_GetLights(m_StubIndex) > 0;														}
		public bool HasPower()														{ return UnitEx_HasPower(m_StubIndex) > 0;														}
		public bool HasWorkers()													{ return UnitEx_HasWorkers(m_StubIndex) > 0;													}
		public bool HasScientists()													{ return UnitEx_HasScientists(m_StubIndex) > 0;													}
		public bool IsInfected()													{ return UnitEx_IsInfected(m_StubIndex) > 0;													}

		public bool GetDoubleFireRate()												{ return UnitEx_GetDoubleFireRate(m_StubIndex) > 0;												}
		public bool GetInvisible()													{ return UnitEx_GetInvisible(m_StubIndex) > 0;													}

		public void SetDoubleFireRate(bool active)									{ UnitEx_SetDoubleFireRate(m_StubIndex, active ? 1 : 0);										}
		public void SetInvisible(bool active)										{ UnitEx_SetInvisible(m_StubIndex, active ? 1 : 0);												}

		public UnitInfo GetUnitInfo()												{ return new UnitInfo(GetUnitType());															}
		public UnitInfo GetWeaponInfo()												{ return new UnitInfo(GetWeapon());																}

		public void SetAnimation(int animIdx, int animDelay, int animStartDelay, bool invisible, bool skipDoDeath)	{ UnitEx_SetAnimation(m_StubIndex, animIdx, animDelay, animStartDelay, invisible ? 1 : 0, skipDoDeath ? 1 : 0);	}

		// Mining beacon
		public int GetNumTruckLoadsSoFar()											{ return UnitEx_GetNumTruckLoadsSoFar(m_StubIndex);												}
		public Yield GetBarYield()													{ return (Yield)UnitEx_GetBarYield(m_StubIndex);												}
		public Variant GetVariant()													{ return (Variant)UnitEx_GetVariant(m_StubIndex);												}
		public BeaconType GetOreType()												{ return (BeaconType)UnitEx_GetOreType(m_StubIndex);											}
		public bool GetSurveyedBy(int playerID)										{ return ((1 << playerID) & UnitEx_GetSurveyedBy(m_StubIndex)) != 0;							}
		public int GetSurveyedFlags()												{ return UnitEx_GetSurveyedBy(m_StubIndex);														}

		// Lab
		public int GetLabCurrentTopic()												{ return UnitEx_GetLabCurrentTopic(m_StubIndex);												}
		public int GetLabScientistCount()											{ return UnitEx_GetLabScientistCount(m_StubIndex);												}
		public void SetLabScientistCount(int numScientists)							{ UnitEx_SetLabScientistCount(m_StubIndex, numScientists);										}

		public bool HasWeapon()														{ return GetWeaponInfo().GetWeaponStrength() > 0;												}

		public bool HasEmptyBay()													{ return GetBayWithCargo(map_id.None) >= 0;														}
		public bool HasBayWithCargo(map_id cargoType)								{ return GetBayWithCargo(cargoType) >= 0;														}

		public MAP_RECT GetRect(bool includeBulldozedArea=false)					{ return GetUnitInfo().GetRect(GetPosition(), includeBulldozedArea);							}

		/// <summary>
		/// Returns the bay index that contains cargo type.
		/// Returns -1 if cargo type is not found.
		/// </summary>
		/// <param name="cargoType">The cargo type to search for.</param>
		public int GetBayWithCargo(map_id cargoType)
		{
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
			LOCATION pos;
			long result = UnitEx_GetDockLocation(m_StubIndex);
			pos.x = (int)(result & uint.MaxValue);
			pos.y = (int)(result >> 32);
			return pos;
		}

		public bool IsOnDock(UnitEx unitWithDock)
		{
			LOCATION dock = unitWithDock.GetDockLocation();
			return dock.Equals(GetPosition());
		}

		// True if structure is enabled.
		public bool IsEnabled()														{ return UnitEx_GetLights(m_StubIndex) == 1;													}

		// True if structure is disabled. A structure is not disabled if it is idle.
		public bool IsDisabled()													{ return UnitEx_GetLights(m_StubIndex) == 0 && UnitEx_GetLastCommand(m_StubIndex) != CommandType.ctMoIdle;	}

		// Swaps cargo between bay and launchpad
		public void DoTransferLaunchpadCargo(int bay)
		{
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

		[DllImport("DotNetInterop.dll")] private static extern int UnitEx_IsEMPedEx(int unitID);
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
