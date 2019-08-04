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

		public bool IsValid()										{ return UnitInfo_IsValid(m_UnitType) > 0;								}
	
		// *** Player unit type settings ***
		public int GetHitPoints(int playerID)						{ return UnitInfo_GetHitPoints(m_UnitType, playerID);					}
		public void SetHitPoints(int playerID, int value)			{ UnitInfo_SetHitPoints(m_UnitType, playerID, value);					}
	
		public int GetRepairAmount(int playerID)					{ return UnitInfo_GetRepairAmount(m_UnitType, playerID);				}
		public void SetRepairAmount(int playerID, int value)		{ UnitInfo_SetRepairAmount(m_UnitType, playerID, value);				}
	
		public int GetArmor(int playerID)							{ return UnitInfo_GetArmor(m_UnitType, playerID);						}
		public void SetArmor(int playerID, int value)				{ UnitInfo_SetArmor(m_UnitType, playerID, value);						}
	
		public int GetOreCost(int playerID)							{ return UnitInfo_GetOreCost(m_UnitType, playerID);						}
		public void SetOreCost(int playerID, int value)				{ UnitInfo_SetOreCost(m_UnitType, playerID, value);						}
	
		public int GetRareOreCost(int playerID)						{ return UnitInfo_GetRareOreCost(m_UnitType, playerID);					}
		public void SetRareOreCost(int playerID, int value)			{ UnitInfo_SetRareOreCost(m_UnitType, playerID, value);					}
	
		public int GetBuildTime(int playerID)						{ return UnitInfo_GetBuildTime(m_UnitType, playerID);					}
		public void SetBuildTime(int playerID, int value)			{ UnitInfo_SetBuildTime(m_UnitType, playerID, value);					}
	
		public int GetSightRange(int playerID)						{ return UnitInfo_GetSightRange(m_UnitType, playerID);					}
		public void SetSightRange(int playerID, int value)			{ UnitInfo_SetSightRange(m_UnitType, playerID, value);					}
		
		public int GetWeaponRange(int playerID)						{ return UnitInfo_GetWeaponRange(m_UnitType, playerID);					}
		public void SetWeaponRange(int playerID, int value)			{ UnitInfo_SetWeaponRange(m_UnitType, playerID, value);					}
		
		public int GetPowerRequired(int playerID)					{ return UnitInfo_GetPowerRequired(m_UnitType, playerID);				}
		public void SetPowerRequired(int playerID, int value)		{ UnitInfo_SetPowerRequired(m_UnitType, playerID, value);				}
		
		public int GetMovePoints(int playerID)						{ return UnitInfo_GetMovePoints(m_UnitType, playerID);					}
		public void SetMovePoints(int playerID, int value)			{ UnitInfo_SetMovePoints(m_UnitType, playerID, value);					}
		
		public int GetTurretTurnRate(int playerID)					{ return UnitInfo_GetTurretTurnRate(m_UnitType, playerID);				}
		public void SetTurretTurnRate(int playerID, int value)		{ UnitInfo_SetTurretTurnRate(m_UnitType, playerID, value);				}
		
		public int GetConcussionDamage(int playerID)				{ return UnitInfo_GetConcussionDamage(m_UnitType, playerID);			}
		public void SetConcussionDamage(int playerID, int value)	{ UnitInfo_SetConcussionDamage(m_UnitType, playerID, value);			}
		
		public int GetWorkersRequired(int playerID)					{ return UnitInfo_GetWorkersRequired(m_UnitType, playerID);				}
		public void SetWorkersRequired(int playerID, int value)		{ UnitInfo_SetWorkersRequired(m_UnitType, playerID, value);				}
		
		public int GetTurnRate(int playerID)						{ return UnitInfo_GetTurnRate(m_UnitType, playerID);					}
		public void SetTurnRate(int playerID, int value)			{ UnitInfo_SetTurnRate(m_UnitType, playerID, value);					}
		
		public int GetPenetrationDamage(int playerID)				{ return UnitInfo_GetPenetrationDamage(m_UnitType, playerID);			}
		public void SetPenetrationDamage(int playerID, int value)	{ UnitInfo_SetPenetrationDamage(m_UnitType, playerID, value);			}
		
		public int GetScientistsRequired(int playerID)				{ return UnitInfo_GetScientistsRequired(m_UnitType, playerID);			}
		public void SetScientistsRequired(int playerID, int value)	{ UnitInfo_SetScientistsRequired(m_UnitType, playerID, value);			}
		
		public int GetProductionRate(int playerID)					{ return UnitInfo_GetProductionRate(m_UnitType, playerID);				}
		public void SetProductionRate(int playerID, int value)		{ UnitInfo_SetProductionRate(m_UnitType, playerID, value);				}
		
		public int GetReloadTime(int playerID)						{ return UnitInfo_GetReloadTime(m_UnitType, playerID);					}
		public void SetReloadTime(int playerID, int value)			{ UnitInfo_SetReloadTime(m_UnitType, playerID, value);					}
	
		public int GetStorageCapacity(int playerID)					{ return UnitInfo_GetStorageCapacity(m_UnitType, playerID);				}
		public void SetStorageCapacity(int playerID, int value)		{ UnitInfo_SetStorageCapacity(m_UnitType, playerID, value);				}
		
		public int GetWeaponSightRange(int playerID)				{ return UnitInfo_GetWeaponSightRange(m_UnitType, playerID);			}
		public void SetWeaponSightRange(int playerID, int value)	{ UnitInfo_SetWeaponSightRange(m_UnitType, playerID, value);			}
		
		public int GetProductionCapacity(int playerID)				{ return UnitInfo_GetProductionCapacity(m_UnitType, playerID);			}
		public void SetProductionCapacity(int playerID, int value)	{ UnitInfo_SetProductionCapacity(m_UnitType, playerID, value);			}
		
		public int GetNumStorageBays(int playerID)					{ return UnitInfo_GetNumStorageBays(m_UnitType, playerID);				}
		public void SetNumStorageBays(int playerID, int value)		{ UnitInfo_SetNumStorageBays(m_UnitType, playerID, value);				}
		
		public int GetCargoCapacity(int playerID)					{ return UnitInfo_GetCargoCapacity(m_UnitType, playerID);				}
		public void SetCargoCapacity(int playerID, int value)		{ UnitInfo_SetCargoCapacity(m_UnitType, playerID, value);				}
		
		// *** Global unit type settings ***
		public int GetResearchTopic()								{ return UnitInfo_GetResearchTopic(m_UnitType);							}
		public void SetResearchTopic(int techID)					{ UnitInfo_SetResearchTopic(m_UnitType, techID);						}
		
		public TrackType GetTrackType()								{ return UnitInfo_GetTrackType(m_UnitType);								}
		public void SetTrackType(TrackType type)					{ UnitInfo_SetTrackType(m_UnitType, type);								}
		
		public OwnerFlags GetOwnerFlags()							{ return (OwnerFlags)UnitInfo_GetOwnerFlags(m_UnitType);				}
		public void SetOwnerFlags(OwnerFlags flags)					{ UnitInfo_SetOwnerFlags(m_UnitType, (int)flags);						}
		
		public string GetUnitName()									{ return UnitInfo_GetUnitName(m_UnitType);								}
		public void SetUnitName(string newName)						{ UnitInfo_SetUnitName(m_UnitType, newName);							}
		
		public string GetProduceListName()							{ return UnitInfo_GetProduceListName(m_UnitType);						}
		public void SetProduceListName(string newName)				{ UnitInfo_SetProduceListName(m_UnitType, newName);						}
		
		public int GetXSize()										{ return IsStructure() ? UnitInfo_GetXSize(m_UnitType) : 1;				} // If not a structure, returns 1
		public void SetXSize(int value)								{ UnitInfo_SetXSize(m_UnitType, value);									}
		
		// Only valid for weapons
		public int GetDamageRadius()								{ return UnitInfo_GetDamageRadius(m_UnitType);							}
		public void SetDamageRadius(int value)						{ UnitInfo_SetDamageRadius(m_UnitType, value);							}

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
		public VehicleFlags GetVehicleFlags()						{ return (VehicleFlags)UnitInfo_GetVehicleFlags(m_UnitType);			}
		public void SetVehicleFlags(VehicleFlags flags)				{ UnitInfo_SetVehicleFlags(m_UnitType, (int)flags);						}
		
		public int GetYSize()										{ return IsStructure() ? UnitInfo_GetYSize(m_UnitType) : 1;				} // If not a structure, returns 1
		public void SetYSize(int value)								{ UnitInfo_SetYSize(m_UnitType, value);									}
		
		public int GetPixelsSkippedWhenFiring()						{ return UnitInfo_GetPixelsSkippedWhenFiring(m_UnitType);				}
		public void SetPixelsSkippedWhenFiring(int value)			{ UnitInfo_SetPixelsSkippedWhenFiring(m_UnitType, value);				}
		
		public BuildingFlags GetBuildingFlags()						{ return (BuildingFlags)UnitInfo_GetBuildingFlags(m_UnitType);			}
		public void SetBuildingFlags(BuildingFlags flags)			{ UnitInfo_SetBuildingFlags(m_UnitType, (int)flags);					}
		
		public int GetExplosionSize()								{ return UnitInfo_GetExplosionSize(m_UnitType);							}
		public void SetExplosionSize(int value)						{ UnitInfo_SetExplosionSize(m_UnitType, value);							}
		
		public int GetResourcePriority()							{ return UnitInfo_GetResourcePriority(m_UnitType);						}
		public void SetResourcePriority(int value)					{ UnitInfo_SetResourcePriority(m_UnitType, value);						}
		
		public int GetRareRubble()									{ return UnitInfo_GetRareRubble(m_UnitType);							}
		public void SetRareRubble(int value)						{ UnitInfo_SetRareRubble(m_UnitType, value);							}
		
		public int GetRubble()										{ return UnitInfo_GetRubble(m_UnitType);								}
		public void SetRubble(int value)							{ UnitInfo_SetRubble(m_UnitType, value);								}
		
		public int GetEdenDockPos()									{ return UnitInfo_GetEdenDockPos(m_UnitType);							}
		public void SetEdenDockPos(int value)						{ UnitInfo_SetEdenDockPos(m_UnitType, value);							}
		
		public int GetPlymDockPos()									{ return UnitInfo_GetPlymDockPos(m_UnitType);							}
		public void SetPlymDockPos(int value)						{ UnitInfo_SetPlymDockPos(m_UnitType, value);							}
		
		public string GetCodeName()									{ return Marshal.PtrToStringAnsi(UnitInfo_GetCodeName(m_UnitType));		}
		
		//public int CreateUnit(int tileX, int tileY, int unitID)	{ return UnitInfo_CreateUnit(m_UnitType, tileX, tileY, unitID);			}

		// Helper Functions
		private bool IsVehicle()									{ return (int)m_UnitType >= 1 && (int)m_UnitType <= 15;					}
		private bool IsStructure()									{ return (int)m_UnitType >= 21 && (int)m_UnitType <= 58;				}

		// includeBulldozedArea only applies to structures.
		public LOCATION GetSize(bool includeBulldozedArea=false)
		{
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
			LOCATION size = GetSize(includeBulldozedArea);

			return new MAP_RECT(position - (size / 2), size);
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
