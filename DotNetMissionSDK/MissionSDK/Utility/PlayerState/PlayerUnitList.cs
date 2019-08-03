using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DotNetMissionSDK.Utility.PlayerState
{
	/// <summary>
	/// Maintains a list of player units and starship module counts.
	/// </summary>
	public class PlayerUnitList : IDisposable
	{
		private Player m_Player;
		private PlayerInfoSaveData m_SaveData;
		private TriggerManager m_TriggerManager;

		public const int TriggerID_EDWARDSatellite		= 30000;
		public const int TriggerID_SolarSatellite		= 30001;
		public const int TriggerID_IonDriveModule		= 30002;
		public const int TriggerID_FusionDriveModule	= 30003;
		public const int TriggerID_CommandModule		= 30004;
		public const int TriggerID_FuelingSystems		= 30005;
		public const int TriggerID_HabitatRing			= 30006;
		public const int TriggerID_SensorPackage		= 30007;
		public const int TriggerID_Skydock				= 30008;
		public const int TriggerID_StasisSystems		= 30009;
		public const int TriggerID_OrbitalPackage		= 30010;
		public const int TriggerID_PhoenixModule		= 30011;
		
		public const int TriggerID_RareMetalsCargo		= 30012;
		public const int TriggerID_CommonMetalsCargo	= 30013;
		public const int TriggerID_FoodCargo			= 30014;
		public const int TriggerID_EvacuationModule		= 30015;
		public const int TriggerID_ChildrenModule		= 30016;

		// Units
		public List<UnitEx> cargoTrucks			= new List<UnitEx>();
		public List<UnitEx> convecs				= new List<UnitEx>();
		public List<UnitEx> spiders				= new List<UnitEx>();
		public List<UnitEx> scorpions			= new List<UnitEx>();
		public List<UnitEx> lynx				= new List<UnitEx>();
		public List<UnitEx> panthers			= new List<UnitEx>();
		public List<UnitEx> tigers				= new List<UnitEx>();
		public List<UnitEx> roboSurveyors		= new List<UnitEx>();
		public List<UnitEx> roboMiners			= new List<UnitEx>();
		public List<UnitEx> geoCons				= new List<UnitEx>();
		public List<UnitEx> scouts				= new List<UnitEx>();
		public List<UnitEx> roboDozers			= new List<UnitEx>();
		public List<UnitEx> evacTransports		= new List<UnitEx>();
		public List<UnitEx> repairVehicles		= new List<UnitEx>();
		public List<UnitEx> earthWorkers		= new List<UnitEx>();

		// Structures
		public List<UnitEx> commonOreMines			= new List<UnitEx>();
		public List<UnitEx> rareOreMines			= new List<UnitEx>();
		public List<UnitEx> guardPosts				= new List<UnitEx>();
		public List<UnitEx> lightTowers				= new List<UnitEx>();
		public List<UnitEx> commonStorages			= new List<UnitEx>();
		public List<UnitEx> rareStorages			= new List<UnitEx>();
		public List<UnitEx> forums					= new List<UnitEx>();
		public List<UnitEx> commandCenters			= new List<UnitEx>();
		public List<UnitEx> mhdGenerators			= new List<UnitEx>();
		public List<UnitEx> residences				= new List<UnitEx>();
		public List<UnitEx> robotCommands			= new List<UnitEx>();
		public List<UnitEx> tradeCenters			= new List<UnitEx>();
		public List<UnitEx> basicLabs				= new List<UnitEx>();
		public List<UnitEx> medicalCenters			= new List<UnitEx>();
		public List<UnitEx> nurseries				= new List<UnitEx>();
		public List<UnitEx> solarPowerArrays		= new List<UnitEx>();
		public List<UnitEx> recreationFacilities	= new List<UnitEx>();
		public List<UnitEx> universities			= new List<UnitEx>();
		public List<UnitEx> agridomes				= new List<UnitEx>();
		public List<UnitEx> dirts					= new List<UnitEx>();
		public List<UnitEx> garages					= new List<UnitEx>();
		public List<UnitEx> magmaWells				= new List<UnitEx>();
		public List<UnitEx> meteorDefenses			= new List<UnitEx>();
		public List<UnitEx> geothermalPlants		= new List<UnitEx>();
		public List<UnitEx> arachnidFactories		= new List<UnitEx>();
		public List<UnitEx> consumerFactories		= new List<UnitEx>();
		public List<UnitEx> structureFactories		= new List<UnitEx>();
		public List<UnitEx> vehicleFactories		= new List<UnitEx>();
		public List<UnitEx> standardLabs			= new List<UnitEx>();
		public List<UnitEx> advancedLabs			= new List<UnitEx>();
		public List<UnitEx> observatories			= new List<UnitEx>();
		public List<UnitEx> reinforcedResidences	= new List<UnitEx>();
		public List<UnitEx> advancedResidences		= new List<UnitEx>();
		public List<UnitEx> commonOreSmelters		= new List<UnitEx>();
		public List<UnitEx> spaceports				= new List<UnitEx>();
		public List<UnitEx> rareOreSmelters			= new List<UnitEx>();
		public List<UnitEx> gorfs					= new List<UnitEx>();
		public List<UnitEx> tokamaks				= new List<UnitEx>();

		// Starship module counts
		public byte EDWARDSatelliteCount	{ get { return m_SaveData.EDWARDSatelliteCount;		} }
		public byte solarSatelliteCount		{ get { return m_SaveData.solarSatelliteCount;		} }
		public byte ionDriveModuleCount		{ get { return m_SaveData.ionDriveModuleCount;		} }
		public byte fusionDriveModuleCount	{ get { return m_SaveData.fusionDriveModuleCount;	} }
		public byte commandModuleCount		{ get { return m_SaveData.commandModuleCount;		} }
		public byte fuelingSystemsCount		{ get { return m_SaveData.fuelingSystemsCount;		} }
		public byte habitatRingCount		{ get { return m_SaveData.habitatRingCount;			} }
		public byte sensorPackageCount		{ get { return m_SaveData.sensorPackageCount;		} }
		public byte skydockCount			{ get { return m_SaveData.skydockCount;				} }
		public byte stasisSystemsCount		{ get { return m_SaveData.stasisSystemsCount;		} }
		public byte orbitalPackageCount		{ get { return m_SaveData.orbitalPackageCount;		} }
		public byte phoenixModuleCount		{ get { return m_SaveData.phoenixModuleCount;		} }

		public byte rareMetalsCargoCount	{ get { return m_SaveData.rareMetalsCargoCount;		} }
		public byte commonMetalsCargoCount	{ get { return m_SaveData.commonMetalsCargoCount;	} }
		public byte foodCargoCount			{ get { return m_SaveData.foodCargoCount;			} }
		public byte evacuationModuleCount	{ get { return m_SaveData.evacuationModuleCount;	} }
		public byte childrenModuleCount		{ get { return m_SaveData.childrenModuleCount;		} }


		public PlayerUnitList(TriggerManager triggerManager, Player player, PlayerInfoSaveData playerSaveData)
		{
			m_TriggerManager = triggerManager;
			m_Player = player;
			m_SaveData = playerSaveData;

			triggerManager.onTriggerFired += OnTriggerFired;

			Update();
		}

		/// <summary>
		/// Creates triggers for new mission. Only call when a new mission is started.
		/// </summary>
		public void InitializeNewMission(PlayerInfoSaveData playerSaveData)
		{
			m_SaveData = playerSaveData;

			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_EDWARDSatellite,	true, false, m_Player.playerID, map_id.EDWARDSatellite,	map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_SolarSatellite,	true, false, m_Player.playerID, map_id.SolarSatellite,	map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_IonDriveModule,	true, false, m_Player.playerID, map_id.IonDriveModule,	map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_FusionDriveModule, true, false, m_Player.playerID, map_id.FusionDriveModule,map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_CommandModule,		true, false, m_Player.playerID, map_id.CommandModule,	map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_FuelingSystems,	true, false, m_Player.playerID, map_id.FuelingSystems,	map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_HabitatRing,		true, false, m_Player.playerID, map_id.HabitatRing,		map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_SensorPackage,		true, false, m_Player.playerID, map_id.SensorPackage,	map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_Skydock,			true, false, m_Player.playerID, map_id.Skydock,			map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_StasisSystems,		true, false, m_Player.playerID, map_id.StasisSystems,	map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_OrbitalPackage,	true, false, m_Player.playerID, map_id.OrbitalPackage,	map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_PhoenixModule,		true, false, m_Player.playerID, map_id.PhoenixModule,	map_id.None, 1, CompareMode.GreaterEqual));

			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_RareMetalsCargo,	true, false, m_Player.playerID, map_id.RareMetalsCargo,	map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_CommonMetalsCargo, true, false, m_Player.playerID, map_id.CommonMetalsCargo,map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_FoodCargo,			true, false, m_Player.playerID, map_id.FoodCargo,		map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_EvacuationModule,	true, false, m_Player.playerID, map_id.EvacuationModule,map_id.None, 1, CompareMode.GreaterEqual));
			m_TriggerManager.AddTrigger(TriggerStub.CreateCountTrigger(TriggerID_ChildrenModule,	true, false, m_Player.playerID, map_id.ChildrenModule,	map_id.None, 1, CompareMode.GreaterEqual));
		}

		/// <summary>
		/// Updates unit lists.
		/// </summary>
		public void Update()
		{
			Clear();

			for (int i=21; i < 59; ++i)
			{
				foreach (UnitEx unit in new PlayerBuildingEnum(m_Player.playerID, (map_id)i))
				{
					switch ((map_id)i)
					{
						case map_id.CommonOreMine:			commonOreMines.Add(unit);		break;
						case map_id.RareOreMine:			rareOreMines.Add(unit);			break;
						case map_id.GuardPost:				guardPosts.Add(unit);			break;
						case map_id.LightTower:				lightTowers.Add(unit);			break;
						case map_id.CommonStorage:			commonStorages.Add(unit);		break;
						case map_id.RareStorage:			rareStorages.Add(unit);			break;
						case map_id.Forum:					forums.Add(unit);				break;
						case map_id.CommandCenter:			commandCenters.Add(unit);		break;
						case map_id.MHDGenerator:			mhdGenerators.Add(unit);		break;
						case map_id.Residence:				residences.Add(unit);			break;
						case map_id.RobotCommand:			robotCommands.Add(unit);		break;
						case map_id.TradeCenter:			tradeCenters.Add(unit);			break;
						case map_id.BasicLab:				basicLabs.Add(unit);			break;
						case map_id.MedicalCenter:			medicalCenters.Add(unit);		break;
						case map_id.Nursery:				nurseries.Add(unit);			break;
						case map_id.SolarPowerArray:		solarPowerArrays.Add(unit);		break;
						case map_id.RecreationFacility:		recreationFacilities.Add(unit);	break;
						case map_id.University:				universities.Add(unit);			break;
						case map_id.Agridome:				agridomes.Add(unit);			break;
						case map_id.DIRT:					dirts.Add(unit);				break;
						case map_id.Garage:					garages.Add(unit);				break;
						case map_id.MagmaWell:				magmaWells.Add(unit);			break;
						case map_id.MeteorDefense:			meteorDefenses.Add(unit);		break;
						case map_id.GeothermalPlant:		geothermalPlants.Add(unit);		break;
						case map_id.ArachnidFactory:		arachnidFactories.Add(unit);	break;
						case map_id.ConsumerFactory:		consumerFactories.Add(unit);	break;
						case map_id.StructureFactory:		structureFactories.Add(unit);	break;
						case map_id.VehicleFactory:			vehicleFactories.Add(unit);		break;
						case map_id.StandardLab:			standardLabs.Add(unit);			break;
						case map_id.AdvancedLab:			advancedLabs.Add(unit);			break;
						case map_id.Observatory:			observatories.Add(unit);		break;
						case map_id.ReinforcedResidence:	reinforcedResidences.Add(unit);	break;
						case map_id.AdvancedResidence:		advancedResidences.Add(unit);	break;
						case map_id.CommonOreSmelter:		commonOreSmelters.Add(unit);	break;
						case map_id.Spaceport:				spaceports.Add(unit);			break;
						case map_id.RareOreSmelter:			rareOreSmelters.Add(unit);		break;
						case map_id.GORF:					gorfs.Add(unit);				break;
						case map_id.Tokamak:				tokamaks.Add(unit);				break;
					}
				}
			}

			foreach (UnitEx unit in new PlayerUnitEnum(m_Player.playerID))
			{
				switch (unit.GetUnitType())
				{
					case map_id.CargoTruck:				cargoTrucks.Add(unit);			break;
					case map_id.ConVec:					convecs.Add(unit);				break;
					case map_id.Spider:					spiders.Add(unit);				break;
					case map_id.Scorpion:				scorpions.Add(unit);			break;
					case map_id.Lynx:					lynx.Add(unit);					break;
					case map_id.Panther:				panthers.Add(unit);				break;
					case map_id.Tiger:					tigers.Add(unit);				break;
					case map_id.RoboSurveyor:			roboSurveyors.Add(unit);		break;
					case map_id.RoboMiner:				roboMiners.Add(unit);			break;
					case map_id.GeoCon:					geoCons.Add(unit);				break;
					case map_id.Scout:					scouts.Add(unit);				break;
					case map_id.RoboDozer:				roboDozers.Add(unit);			break;
					case map_id.EvacuationTransport:	evacTransports.Add(unit);		break;
					case map_id.RepairVehicle:			repairVehicles.Add(unit);		break;
					case map_id.Earthworker:			earthWorkers.Add(unit);			break;
				}
			}
		}

		/// <summary>
		/// Clears all unit lists.
		/// </summary>
		private void Clear()
		{
			// Units
			cargoTrucks.Clear();
			convecs.Clear();
			spiders.Clear();
			scorpions.Clear();
			lynx.Clear();
			panthers.Clear();
			tigers.Clear();
			roboSurveyors.Clear();
			roboMiners.Clear();
			geoCons.Clear();
			scouts.Clear();
			roboDozers.Clear();
			evacTransports.Clear();
			repairVehicles.Clear();
			earthWorkers.Clear();

			// Structures
			commonOreMines.Clear();
			rareOreMines.Clear();
			guardPosts.Clear();
			lightTowers.Clear();
			commonStorages.Clear();
			rareStorages.Clear();
			forums.Clear();
			commandCenters.Clear();
			mhdGenerators.Clear();
			residences.Clear();
			robotCommands.Clear();
			tradeCenters.Clear();
			basicLabs.Clear();
			medicalCenters.Clear();
			nurseries.Clear();
			solarPowerArrays.Clear();
			recreationFacilities.Clear();
			universities.Clear();
			agridomes.Clear();
			dirts.Clear();
			garages.Clear();
			magmaWells.Clear();
			meteorDefenses.Clear();
			geothermalPlants.Clear();
			arachnidFactories.Clear();
			consumerFactories.Clear();
			structureFactories.Clear();
			vehicleFactories.Clear();
			standardLabs.Clear();
			advancedLabs.Clear();
			observatories.Clear();
			reinforcedResidences.Clear();
			advancedResidences.Clear();
			commonOreSmelters.Clear();
			spaceports.Clear();
			rareOreSmelters.Clear();
			gorfs.Clear();
			tokamaks.Clear();
		}

		private void OnTriggerFired(TriggerStub trigger)
		{
			// Only handle triggers for this player
			if (trigger.stubData.playerID != m_Player.playerID)
				return;

			// Increment starship counters
			switch (trigger.id)
			{
				case TriggerID_EDWARDSatellite:		++m_SaveData.EDWARDSatelliteCount; 		break;
				case TriggerID_SolarSatellite:		++m_SaveData.solarSatelliteCount;		break;
				case TriggerID_IonDriveModule:		++m_SaveData.ionDriveModuleCount;		break;
				case TriggerID_FusionDriveModule:	++m_SaveData.fusionDriveModuleCount;	break;
				case TriggerID_CommandModule:		++m_SaveData.commandModuleCount;		break;
				case TriggerID_FuelingSystems:		++m_SaveData.fuelingSystemsCount;		break;
				case TriggerID_HabitatRing:			++m_SaveData.habitatRingCount;			break;
				case TriggerID_SensorPackage:		++m_SaveData.sensorPackageCount;		break;
				case TriggerID_Skydock:				++m_SaveData.skydockCount;				break;
				case TriggerID_StasisSystems:		++m_SaveData.stasisSystemsCount;		break;
				case TriggerID_OrbitalPackage:		++m_SaveData.orbitalPackageCount;		break;
				case TriggerID_PhoenixModule:		++m_SaveData.phoenixModuleCount;		break;

				case TriggerID_RareMetalsCargo:		++m_SaveData.rareMetalsCargoCount;		break;
				case TriggerID_CommonMetalsCargo:	++m_SaveData.commonMetalsCargoCount;	break;
				case TriggerID_FoodCargo:			++m_SaveData.foodCargoCount;			break;
				case TriggerID_EvacuationModule:	++m_SaveData.evacuationModuleCount;		break;
				case TriggerID_ChildrenModule:		++m_SaveData.childrenModuleCount;		break;
			}
		}

		public List<UnitEx> GetListForType(map_id type)
		{
			switch (type)
			{
				case map_id.CommonOreMine:			return commonOreMines;
				case map_id.RareOreMine:			return rareOreMines;
				case map_id.GuardPost:				return guardPosts;
				case map_id.LightTower:				return lightTowers;
				case map_id.CommonStorage:			return commonStorages;
				case map_id.RareStorage:			return rareStorages;
				case map_id.Forum:					return forums;
				case map_id.CommandCenter:			return commandCenters;
				case map_id.MHDGenerator:			return mhdGenerators;
				case map_id.Residence:				return residences;
				case map_id.RobotCommand:			return robotCommands;
				case map_id.TradeCenter:			return tradeCenters;
				case map_id.BasicLab:				return basicLabs;
				case map_id.MedicalCenter:			return medicalCenters;
				case map_id.Nursery:				return nurseries;
				case map_id.SolarPowerArray:		return solarPowerArrays;
				case map_id.RecreationFacility:		return recreationFacilities;
				case map_id.University:				return universities;
				case map_id.Agridome:				return agridomes;
				case map_id.DIRT:					return dirts;
				case map_id.Garage:					return garages;
				case map_id.MagmaWell:				return magmaWells;
				case map_id.MeteorDefense:			return meteorDefenses;
				case map_id.GeothermalPlant:		return geothermalPlants;
				case map_id.ArachnidFactory:		return arachnidFactories;
				case map_id.ConsumerFactory:		return consumerFactories;
				case map_id.StructureFactory:		return structureFactories;
				case map_id.VehicleFactory:			return vehicleFactories;
				case map_id.StandardLab:			return standardLabs;
				case map_id.AdvancedLab:			return advancedLabs;
				case map_id.Observatory:			return observatories;
				case map_id.ReinforcedResidence:	return reinforcedResidences;
				case map_id.AdvancedResidence:		return advancedResidences;
				case map_id.CommonOreSmelter:		return commonOreSmelters;
				case map_id.Spaceport:				return spaceports;
				case map_id.RareOreSmelter:			return rareOreSmelters;
				case map_id.GORF:					return gorfs;
				case map_id.Tokamak:				return tokamaks;

				case map_id.CargoTruck:				return cargoTrucks;
				case map_id.ConVec:					return convecs;
				case map_id.Spider:					return spiders;
				case map_id.Scorpion:				return scorpions;
				case map_id.Lynx:					return lynx;
				case map_id.Panther:				return panthers;
				case map_id.Tiger:					return tigers;
				case map_id.RoboSurveyor:			return roboSurveyors;
				case map_id.RoboMiner:				return roboMiners;
				case map_id.GeoCon:					return geoCons;
				case map_id.Scout:					return scouts;
				case map_id.RoboDozer:				return roboDozers;
				case map_id.EvacuationTransport:	return evacTransports;
				case map_id.RepairVehicle:			return repairVehicles;
				case map_id.Earthworker:			return earthWorkers;
			}

			return null;
		}

		/// <summary>
		/// Disposes events.
		/// </summary>
		public void Dispose()
		{
			m_TriggerManager.onTriggerFired -= OnTriggerFired;
		}

		public IEnumerable<UnitEx> GetUnits()		{ return new UnitEnumerable(this, UnitEnumerable.EnumType.Units);			}
		public IEnumerable<UnitEx> GetStructures()	{ return new UnitEnumerable(this, UnitEnumerable.EnumType.Structures);		}
		public IEnumerable<UnitEx> GetVehicles()	{ return new UnitEnumerable(this, UnitEnumerable.EnumType.Vehicles);		}

		public class UnitEnumerable : IEnumerable<UnitEx>
		{
			public enum EnumType { Units, Vehicles, Structures }

			private PlayerUnitList m_List;
			private EnumType m_EnumType;

			public UnitEnumerable(PlayerUnitList list, EnumType enumType)
			{
				m_List = list;
				m_EnumType = enumType;
			}

			public IEnumerator<UnitEx> GetEnumerator()
			{
				if (m_EnumType == EnumType.Units || m_EnumType == EnumType.Structures)
				{
					// Enumerate all structures
					for (int i=21; i < 59; ++i)
					{
						List<UnitEx> structures = m_List.GetListForType((map_id)i);
						foreach (UnitEx unit in structures)
							yield return unit;
					}
				}
				if (m_EnumType == EnumType.Units || m_EnumType == EnumType.Vehicles)
				{
					// Enumerate all vehicles
					for (int i=1; i < 16; ++i)
					{
						List<UnitEx> vehicles = m_List.GetListForType((map_id)i);
						foreach (UnitEx unit in vehicles)
							yield return unit;
					}
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
	}
}
