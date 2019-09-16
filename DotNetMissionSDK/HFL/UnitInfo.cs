using DotNetMissionSDK.Async;
using System;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK.HFL
{
	/// <summary>
	/// Contains info about unit types.
	/// </summary>
	public class UnitInfo
	{
		private map_id m_UnitType;
		

		public UnitInfo(map_id unitType)
		{
			m_UnitType = unitType;
		}

		public bool IsValid()										{ ThreadAssert.MainThreadRequired();	return UnitInfo_IsValid(m_UnitType) > 0;							}
	
		// *** Player unit type settings ***
		public int GetHitPoints(int playerID)						{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetHitPoints(m_UnitType, playerID);					}
		public void SetHitPoints(int playerID, int value)			{ ThreadAssert.MainThreadRequired();	UnitInfo_SetHitPoints(m_UnitType, playerID, value);					}
	
		public int GetRepairAmount(int playerID)					{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetRepairAmount(m_UnitType, playerID);				}
		public void SetRepairAmount(int playerID, int value)		{ ThreadAssert.MainThreadRequired();	UnitInfo_SetRepairAmount(m_UnitType, playerID, value);				}
	
		public int GetArmor(int playerID)							{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetArmor(m_UnitType, playerID);						}
		public void SetArmor(int playerID, int value)				{ ThreadAssert.MainThreadRequired();	UnitInfo_SetArmor(m_UnitType, playerID, value);						}
	
		public int GetOreCost(int playerID)							{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetOreCost(m_UnitType, playerID);					}
		public void SetOreCost(int playerID, int value)				{ ThreadAssert.MainThreadRequired();	UnitInfo_SetOreCost(m_UnitType, playerID, value);					}
	
		public int GetRareOreCost(int playerID)						{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetRareOreCost(m_UnitType, playerID);				}
		public void SetRareOreCost(int playerID, int value)			{ ThreadAssert.MainThreadRequired();	UnitInfo_SetRareOreCost(m_UnitType, playerID, value);				}
	
		public int GetBuildTime(int playerID)						{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetBuildTime(m_UnitType, playerID);					}
		public void SetBuildTime(int playerID, int value)			{ ThreadAssert.MainThreadRequired();	UnitInfo_SetBuildTime(m_UnitType, playerID, value);					}
	
		public int GetSightRange(int playerID)						{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetSightRange(m_UnitType, playerID);				}
		public void SetSightRange(int playerID, int value)			{ ThreadAssert.MainThreadRequired();	UnitInfo_SetSightRange(m_UnitType, playerID, value);				}
		
		public int GetWeaponRange(int playerID)						{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetWeaponRange(m_UnitType, playerID);				}
		public void SetWeaponRange(int playerID, int value)			{ ThreadAssert.MainThreadRequired();	UnitInfo_SetWeaponRange(m_UnitType, playerID, value);				}
		
		public int GetPowerRequired(int playerID)					{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetPowerRequired(m_UnitType, playerID);				}
		public void SetPowerRequired(int playerID, int value)		{ ThreadAssert.MainThreadRequired();	UnitInfo_SetPowerRequired(m_UnitType, playerID, value);				}
		
		public int GetMovePoints(int playerID)						{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetMovePoints(m_UnitType, playerID);				}
		public void SetMovePoints(int playerID, int value)			{ ThreadAssert.MainThreadRequired();	UnitInfo_SetMovePoints(m_UnitType, playerID, value);				}
		
		public int GetTurretTurnRate(int playerID)					{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetTurretTurnRate(m_UnitType, playerID);			}
		public void SetTurretTurnRate(int playerID, int value)		{ ThreadAssert.MainThreadRequired();	UnitInfo_SetTurretTurnRate(m_UnitType, playerID, value);			}
		
		public int GetConcussionDamage(int playerID)				{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetConcussionDamage(m_UnitType, playerID);			}
		public void SetConcussionDamage(int playerID, int value)	{ ThreadAssert.MainThreadRequired();	UnitInfo_SetConcussionDamage(m_UnitType, playerID, value);			}
		
		public int GetWorkersRequired(int playerID)					{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetWorkersRequired(m_UnitType, playerID);			}
		public void SetWorkersRequired(int playerID, int value)		{ ThreadAssert.MainThreadRequired();	UnitInfo_SetWorkersRequired(m_UnitType, playerID, value);			}
		
		public int GetTurnRate(int playerID)						{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetTurnRate(m_UnitType, playerID);					}
		public void SetTurnRate(int playerID, int value)			{ ThreadAssert.MainThreadRequired();	UnitInfo_SetTurnRate(m_UnitType, playerID, value);					}
		
		public int GetPenetrationDamage(int playerID)				{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetPenetrationDamage(m_UnitType, playerID);			}
		public void SetPenetrationDamage(int playerID, int value)	{ ThreadAssert.MainThreadRequired();	UnitInfo_SetPenetrationDamage(m_UnitType, playerID, value);			}
		
		public int GetScientistsRequired(int playerID)				{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetScientistsRequired(m_UnitType, playerID);		}
		public void SetScientistsRequired(int playerID, int value)	{ ThreadAssert.MainThreadRequired();	UnitInfo_SetScientistsRequired(m_UnitType, playerID, value);		}
		
		public int GetProductionRate(int playerID)					{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetProductionRate(m_UnitType, playerID);			}
		public void SetProductionRate(int playerID, int value)		{ ThreadAssert.MainThreadRequired();	UnitInfo_SetProductionRate(m_UnitType, playerID, value);			}
		
		public int GetReloadTime(int playerID)						{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetReloadTime(m_UnitType, playerID);				}
		public void SetReloadTime(int playerID, int value)			{ ThreadAssert.MainThreadRequired();	UnitInfo_SetReloadTime(m_UnitType, playerID, value);				}
	
		public int GetStorageCapacity(int playerID)					{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetStorageCapacity(m_UnitType, playerID);			}
		public void SetStorageCapacity(int playerID, int value)		{ ThreadAssert.MainThreadRequired();	UnitInfo_SetStorageCapacity(m_UnitType, playerID, value);			}
		
		public int GetWeaponSightRange(int playerID)				{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetWeaponSightRange(m_UnitType, playerID);			}
		public void SetWeaponSightRange(int playerID, int value)	{ ThreadAssert.MainThreadRequired();	UnitInfo_SetWeaponSightRange(m_UnitType, playerID, value);			}
		
		public int GetProductionCapacity(int playerID)				{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetProductionCapacity(m_UnitType, playerID);		}
		public void SetProductionCapacity(int playerID, int value)	{ ThreadAssert.MainThreadRequired();	UnitInfo_SetProductionCapacity(m_UnitType, playerID, value);		}
		
		public int GetNumStorageBays(int playerID)					{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetNumStorageBays(m_UnitType, playerID);			}
		public void SetNumStorageBays(int playerID, int value)		{ ThreadAssert.MainThreadRequired();	UnitInfo_SetNumStorageBays(m_UnitType, playerID, value);			}
		
		public int GetCargoCapacity(int playerID)					{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetCargoCapacity(m_UnitType, playerID);				}
		public void SetCargoCapacity(int playerID, int value)		{ ThreadAssert.MainThreadRequired();	UnitInfo_SetCargoCapacity(m_UnitType, playerID, value);				}
		
		// *** Global unit type settings ***
		/// <summary>
		/// Gets the unit's research topic.
		/// Returns the TechInfo array index NOT the techID.
		/// </summary>
		public int GetResearchTopic()
		{
			ThreadAssert.MainThreadRequired();

			// Unit types that don't have topics return -1.
			switch (m_UnitType)
			{
				case map_id.None:		return -1;
			}

			return UnitInfo_GetResearchTopic(m_UnitType);
		}
		public void SetResearchTopic(int techIndex)					{ ThreadAssert.MainThreadRequired();	UnitInfo_SetResearchTopic(m_UnitType, techIndex);					}
		
		public TrackType GetTrackType()								{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetTrackType(m_UnitType);							}
		public void SetTrackType(TrackType type)					{ ThreadAssert.MainThreadRequired();	UnitInfo_SetTrackType(m_UnitType, type);							}
		
		//public OwnerFlags GetOwnerFlags()							{ ThreadAssert.MainThreadRequired();	return (OwnerFlags)UnitInfo_GetOwnerFlags(m_UnitType);				}
		public void SetOwnerFlags(OwnerFlags flags)					{ ThreadAssert.MainThreadRequired();	UnitInfo_SetOwnerFlags(m_UnitType, (int)flags);						}
		
		public string GetUnitName()									{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetUnitName(m_UnitType);							}
		public void SetUnitName(string newName)						{ ThreadAssert.MainThreadRequired();	UnitInfo_SetUnitName(m_UnitType, newName);							}
		
		public string GetProduceListName()							{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetProduceListName(m_UnitType);						}
		public void SetProduceListName(string newName)				{ ThreadAssert.MainThreadRequired();	UnitInfo_SetProduceListName(m_UnitType, newName);					}
		
		public int GetXSize()										{ ThreadAssert.MainThreadRequired();	return IsStructure() ? UnitInfo_GetXSize(m_UnitType) : 1;			} // If not a structure, returns 1
		public void SetXSize(int value)								{ ThreadAssert.MainThreadRequired();	UnitInfo_SetXSize(m_UnitType, value);								}

		// Only valid for weapons
		public int GetDamageRadius()								{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetDamageRadius(m_UnitType);						}
		public void SetDamageRadius(int value)						{ ThreadAssert.MainThreadRequired();	UnitInfo_SetDamageRadius(m_UnitType, value);						}

		public int GetWeaponStrength()
		{
			switch (m_UnitType)
			{
				case map_id.AcidCloud:				return 4;
				case map_id.EMP:					return 3;
				case map_id.Laser:					return 2;
				case map_id.Microwave:				return 2;
				case map_id.RailGun:				return 4;
				case map_id.RPG:					return 4;
				case map_id.Starflare:				return 2;
				case map_id.Supernova:				return 3;
				case map_id.Starflare2:				return 1;
				case map_id.Supernova2:				return 2;
				case map_id.ESG:					return 5;
				case map_id.Stickyfoam:				return 3;
				case map_id.ThorsHammer:			return 6;
				case map_id.EnergyCannon:			return 1;
			}

			return 0;
		}
		
		// Only valid for vehicles
		public VehicleFlags GetVehicleFlags()						{ ThreadAssert.MainThreadRequired();	return (VehicleFlags)UnitInfo_GetVehicleFlags(m_UnitType);			}
		public void SetVehicleFlags(VehicleFlags flags)				{ ThreadAssert.MainThreadRequired();	UnitInfo_SetVehicleFlags(m_UnitType, (int)flags);					}
		
		public int GetYSize()										{ ThreadAssert.MainThreadRequired();	return IsStructure() ? UnitInfo_GetYSize(m_UnitType) : 1;			} // If not a structure, returns 1
		public void SetYSize(int value)								{ ThreadAssert.MainThreadRequired();	UnitInfo_SetYSize(m_UnitType, value);								}
		
		public int GetPixelsSkippedWhenFiring()						{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetPixelsSkippedWhenFiring(m_UnitType);				}
		public void SetPixelsSkippedWhenFiring(int value)			{ ThreadAssert.MainThreadRequired();	UnitInfo_SetPixelsSkippedWhenFiring(m_UnitType, value);				}
		
		public BuildingFlags GetBuildingFlags()						{ ThreadAssert.MainThreadRequired();	return (BuildingFlags)UnitInfo_GetBuildingFlags(m_UnitType);		}
		public void SetBuildingFlags(BuildingFlags flags)			{ ThreadAssert.MainThreadRequired();	UnitInfo_SetBuildingFlags(m_UnitType, (int)flags);					}
		
		public int GetExplosionSize()								{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetExplosionSize(m_UnitType);						}
		public void SetExplosionSize(int value)						{ ThreadAssert.MainThreadRequired();	UnitInfo_SetExplosionSize(m_UnitType, value);						}
		
		public int GetResourcePriority()							{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetResourcePriority(m_UnitType);					}
		public void SetResourcePriority(int value)					{ ThreadAssert.MainThreadRequired();	UnitInfo_SetResourcePriority(m_UnitType, value);					}
		
		public int GetRareRubble()									{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetRareRubble(m_UnitType);							}
		public void SetRareRubble(int value)						{ ThreadAssert.MainThreadRequired();	UnitInfo_SetRareRubble(m_UnitType, value);							}
		
		public int GetRubble()										{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetRubble(m_UnitType);								}
		public void SetRubble(int value)							{ ThreadAssert.MainThreadRequired();	UnitInfo_SetRubble(m_UnitType, value);								}
		
		public int GetEdenDockPos()									{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetEdenDockPos(m_UnitType);							}
		public void SetEdenDockPos(int value)						{ ThreadAssert.MainThreadRequired();	UnitInfo_SetEdenDockPos(m_UnitType, value);							}
		
		public int GetPlymDockPos()									{ ThreadAssert.MainThreadRequired();	return UnitInfo_GetPlymDockPos(m_UnitType);							}
		public void SetPlymDockPos(int value)						{ ThreadAssert.MainThreadRequired();	UnitInfo_SetPlymDockPos(m_UnitType, value);							}
		
		public string GetCodeName()									{ ThreadAssert.MainThreadRequired();	return Marshal.PtrToStringAnsi(UnitInfo_GetCodeName(m_UnitType));	}
		
		//public int CreateUnit(int tileX, int tileY, int unitID)	{ return UnitInfo_CreateUnit(m_UnitType, tileX, tileY, unitID);			}

		// Helper Functions
		private bool IsVehicle()									{ return (int)m_UnitType >= 1 && (int)m_UnitType <= 15;					}
		private bool IsStructure()									{ return (int)m_UnitType >= 21 && (int)m_UnitType <= 58;				}

		// includeBulldozedArea only applies to structures.
		public LOCATION GetSize(bool includeBulldozedArea=false)
		{
			ThreadAssert.MainThreadRequired();

			LOCATION result = new LOCATION(GetXSize(), GetYSize());

			if (includeBulldozedArea && IsStructure())
			{
				result.x += 2;
				result.y += 2;
			}

			return result;
		}

		/// <summary>
		/// Gets a rect representing the unit's size around a center point.
		/// </summary>
		/// <param name="position">The center point of the unit rect.</param>
		/// <param name="includeBulldozedArea">Whether or not to include the bulldozed area (structures only).</param>
		/// <returns>The unit rect.</returns>
		public MAP_RECT GetRect(LOCATION position, bool includeBulldozedArea=false)
		{
			ThreadAssert.MainThreadRequired();

			LOCATION size = GetSize(includeBulldozedArea);

			return new MAP_RECT(position - (size / 2), size);
		}

		public bool CanColonyUseUnit(bool isEden)
		{
			if (isEden)
				return (GetOwnerFlags() & OwnerFlags.Eden) != 0;
			else
				return (GetOwnerFlags() & OwnerFlags.Plymouth) != 0;
		}

		public OwnerFlags GetOwnerFlags()
		{
			switch (m_UnitType)
			{
				case map_id.CargoTruck:						return OwnerFlags.Both;
				case map_id.ConVec:							return OwnerFlags.Both;
				case map_id.Spider:							return OwnerFlags.Plymouth;
				case map_id.Scorpion:						return OwnerFlags.Plymouth;
				case map_id.Lynx:							return OwnerFlags.Both;
				case map_id.Panther:						return OwnerFlags.Both;
				case map_id.Tiger:							return OwnerFlags.Both;
				case map_id.RoboSurveyor:					return OwnerFlags.Both;
				case map_id.RoboMiner:						return OwnerFlags.Both;
				case map_id.GeoCon:							return OwnerFlags.Eden;
				case map_id.Scout:							return OwnerFlags.Both;
				case map_id.RoboDozer:						return OwnerFlags.Both;
				case map_id.EvacuationTransport:			return OwnerFlags.Both;
				case map_id.RepairVehicle:					return OwnerFlags.Eden;
				case map_id.Earthworker:					return OwnerFlags.Both;
				case map_id.SmallCapacityAirTransport:		return OwnerFlags.Gaia;

				case map_id.Tube:							return OwnerFlags.Both;
				case map_id.Wall:							return OwnerFlags.Both;
				case map_id.LavaWall:						return OwnerFlags.Plymouth;
				case map_id.MicrobeWall:					return OwnerFlags.Eden;

				case map_id.CommonOreMine:					return OwnerFlags.Both;
				case map_id.RareOreMine:					return OwnerFlags.Both;
				case map_id.GuardPost:						return OwnerFlags.Both;
				case map_id.LightTower:						return OwnerFlags.Both;
				case map_id.CommonStorage:					return OwnerFlags.Both;
				case map_id.RareStorage:					return OwnerFlags.Both;
				case map_id.Forum:							return OwnerFlags.Plymouth;
				case map_id.CommandCenter:					return OwnerFlags.Both;
				case map_id.MHDGenerator:					return OwnerFlags.Plymouth;
				case map_id.Residence:						return OwnerFlags.Both;
				case map_id.RobotCommand:					return OwnerFlags.Both;
				case map_id.TradeCenter:					return OwnerFlags.Both;
				case map_id.BasicLab:						return OwnerFlags.Both;
				case map_id.MedicalCenter:					return OwnerFlags.Both;
				case map_id.Nursery:						return OwnerFlags.Both;
				case map_id.SolarPowerArray:				return OwnerFlags.Both;
				case map_id.RecreationFacility:				return OwnerFlags.Both;
				case map_id.University:						return OwnerFlags.Both;
				case map_id.Agridome:						return OwnerFlags.Both;
				case map_id.DIRT:							return OwnerFlags.Both;
				case map_id.Garage:							return OwnerFlags.Both;
				case map_id.MagmaWell:						return OwnerFlags.Eden;
				case map_id.MeteorDefense:					return OwnerFlags.Eden;
				case map_id.GeothermalPlant:				return OwnerFlags.Eden;
				case map_id.ArachnidFactory:				return OwnerFlags.Plymouth;
				case map_id.ConsumerFactory:				return OwnerFlags.Eden;
				case map_id.StructureFactory:				return OwnerFlags.Both;
				case map_id.VehicleFactory:					return OwnerFlags.Both;
				case map_id.StandardLab:					return OwnerFlags.Both;
				case map_id.AdvancedLab:					return OwnerFlags.Both;
				case map_id.Observatory:					return OwnerFlags.Eden;
				case map_id.ReinforcedResidence:			return OwnerFlags.Plymouth;
				case map_id.AdvancedResidence:				return OwnerFlags.Eden;
				case map_id.CommonOreSmelter:				return OwnerFlags.Both;
				case map_id.Spaceport:						return OwnerFlags.Both;
				case map_id.RareOreSmelter:					return OwnerFlags.Both;
				case map_id.GORF:							return OwnerFlags.Both;
				case map_id.Tokamak:						return OwnerFlags.Both;

				case map_id.AcidCloud:						return OwnerFlags.Eden;
				case map_id.EMP:							return OwnerFlags.Both;
				case map_id.Laser:							return OwnerFlags.Eden;
				case map_id.Microwave:						return OwnerFlags.Plymouth;
				case map_id.RailGun:						return OwnerFlags.Eden;
				case map_id.RPG:							return OwnerFlags.Plymouth;
				case map_id.Starflare:						return OwnerFlags.Both;
				case map_id.Supernova:						return OwnerFlags.Plymouth;
				case map_id.Starflare2:						return OwnerFlags.Eden;
				case map_id.Supernova2:						return OwnerFlags.Plymouth;
				case map_id.ESG:							return OwnerFlags.Plymouth;
				case map_id.Stickyfoam:						return OwnerFlags.Plymouth;
				case map_id.ThorsHammer:					return OwnerFlags.Eden;
				case map_id.EnergyCannon:					return OwnerFlags.Plymouth;

				case map_id.EDWARDSatellite:				return OwnerFlags.Both;
				case map_id.SolarSatellite:					return OwnerFlags.Both;
				case map_id.IonDriveModule:					return OwnerFlags.Both;
				case map_id.FusionDriveModule:				return OwnerFlags.Both;
				case map_id.CommandModule:					return OwnerFlags.Both;
				case map_id.FuelingSystems:					return OwnerFlags.Both;
				case map_id.HabitatRing:					return OwnerFlags.Both;
				case map_id.SensorPackage:					return OwnerFlags.Both;
				case map_id.Skydock:						return OwnerFlags.Both;
				case map_id.StasisSystems:					return OwnerFlags.Both;
				case map_id.OrbitalPackage:					return OwnerFlags.Both;
				case map_id.PhoenixModule:					return OwnerFlags.Both;

				case map_id.RareMetalsCargo:				return OwnerFlags.Both;
				case map_id.CommonMetalsCargo:				return OwnerFlags.Both;
				case map_id.FoodCargo:						return OwnerFlags.Both;
				case map_id.EvacuationModule:				return OwnerFlags.Both;
				case map_id.ChildrenModule:					return OwnerFlags.Both;

				case map_id.SULV:							return OwnerFlags.Both;
				case map_id.RLV:							return OwnerFlags.Eden;
				case map_id.EMPMissile:						return OwnerFlags.Plymouth;

				case map_id.ImpulseItems:					return OwnerFlags.Eden;
				case map_id.Wares:							return OwnerFlags.Eden;
				case map_id.LuxuryWares:					return OwnerFlags.Eden;

				case map_id.Spider3Pack:					return OwnerFlags.Plymouth;
				case map_id.Scorpion3Pack:					return OwnerFlags.Plymouth;
			}

			return OwnerFlags.Gaia;
		}



		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_IsValid(map_id unitType);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetHitPoints(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetHitPoints(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetRepairAmount(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetRepairAmount(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetArmor(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetArmor(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetOreCost(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetOreCost(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetRareOreCost(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetRareOreCost(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetBuildTime(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetBuildTime(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetSightRange(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetSightRange(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetWeaponRange(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetWeaponRange(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetPowerRequired(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetPowerRequired(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetMovePoints(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetMovePoints(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetTurretTurnRate(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetTurretTurnRate(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetConcussionDamage(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetConcussionDamage(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetWorkersRequired(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetWorkersRequired(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetTurnRate(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetTurnRate(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetPenetrationDamage(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetPenetrationDamage(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetScientistsRequired(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetScientistsRequired(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetProductionRate(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetProductionRate(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetReloadTime(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetReloadTime(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetStorageCapacity(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetStorageCapacity(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetWeaponSightRange(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetWeaponSightRange(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetProductionCapacity(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetProductionCapacity(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetNumStorageBays(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetNumStorageBays(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetCargoCapacity(map_id unitType, int player);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetCargoCapacity(map_id unitType, int player, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetResearchTopic(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetResearchTopic(map_id unitType, int techID);
	
		[DllImport("DotNetInterop.dll")] private static extern TrackType UnitInfo_GetTrackType(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetTrackType(map_id unitType, TrackType type);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetOwnerFlags(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetOwnerFlags(map_id unitType, int flags);
	
		[DllImport("DotNetInterop.dll")] private static extern string UnitInfo_GetUnitName(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetUnitName(map_id unitType, string newName);
	
		[DllImport("DotNetInterop.dll")] private static extern string UnitInfo_GetProduceListName(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetProduceListName(map_id unitType, string newName);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetXSize(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetXSize(map_id unitType, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetDamageRadius(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetDamageRadius(map_id unitType, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetVehicleFlags(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetVehicleFlags(map_id unitType, int flags);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetYSize(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetYSize(map_id unitType, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetPixelsSkippedWhenFiring(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetPixelsSkippedWhenFiring(map_id unitType, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetBuildingFlags(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetBuildingFlags(map_id unitType, int flags);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetExplosionSize(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetExplosionSize(map_id unitType, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetResourcePriority(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetResourcePriority(map_id unitType, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetRareRubble(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetRareRubble(map_id unitType, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetRubble(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetRubble(map_id unitType, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetEdenDockPos(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetEdenDockPos(map_id unitType, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_GetPlymDockPos(map_id unitType);
		[DllImport("DotNetInterop.dll")] private static extern void UnitInfo_SetPlymDockPos(map_id unitType, int value);
	
		[DllImport("DotNetInterop.dll")] private static extern IntPtr UnitInfo_GetCodeName(map_id unitType);
		//[DllImport("DotNetInterop.dll")] private static extern map_id UnitInfo_GetMapID(map_id unitType); // No need to go to C++ for map_id when we have it right in this class!
	
		[DllImport("DotNetInterop.dll")] private static extern int UnitInfo_CreateUnit(map_id unitType, int tileX, int tileY, int unitID);
	}
}
