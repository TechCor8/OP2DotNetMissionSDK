﻿using DotNetMissionSDK.Triggers;
using System;
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
		public List<Unit> cargoTrucks			= new List<Unit>();
		public List<Unit> convecs				= new List<Unit>();
		public List<Unit> spiders				= new List<Unit>();
		public List<Unit> scorpions				= new List<Unit>();
		public List<Unit> lynx					= new List<Unit>();
		public List<Unit> panthers				= new List<Unit>();
		public List<Unit> tigers				= new List<Unit>();
		public List<Unit> roboSurveyors			= new List<Unit>();
		public List<Unit> roboMiners			= new List<Unit>();
		public List<Unit> geoCons				= new List<Unit>();
		public List<Unit> scouts				= new List<Unit>();
		public List<Unit> roboDozers			= new List<Unit>();
		public List<Unit> evacTransports		= new List<Unit>();
		public List<Unit> repairVehicles		= new List<Unit>();
		public List<Unit> earthWorkers			= new List<Unit>();

		// Structures
		public List<Unit> commonOreMines		= new List<Unit>();
		public List<Unit> rareOreMines			= new List<Unit>();
		public List<Unit> guardPosts			= new List<Unit>();
		public List<Unit> lightTowers			= new List<Unit>();
		public List<Unit> commonStorages		= new List<Unit>();
		public List<Unit> rareStorages			= new List<Unit>();
		public List<Unit> forums				= new List<Unit>();
		public List<Unit> commandCenters		= new List<Unit>();
		public List<Unit> mhdGenerators			= new List<Unit>();
		public List<Unit> residences			= new List<Unit>();
		public List<Unit> robotCommands			= new List<Unit>();
		public List<Unit> tradeCenters			= new List<Unit>();
		public List<Unit> basicLabs				= new List<Unit>();
		public List<Unit> medicalCenters		= new List<Unit>();
		public List<Unit> nurseries				= new List<Unit>();
		public List<Unit> solarPowerArrays		= new List<Unit>();
		public List<Unit> recreationFacilities	= new List<Unit>();
		public List<Unit> universities			= new List<Unit>();
		public List<Unit> agridomes				= new List<Unit>();
		public List<Unit> dirts					= new List<Unit>();
		public List<Unit> garages				= new List<Unit>();
		public List<Unit> magmaWells			= new List<Unit>();
		public List<Unit> meteorDefenses		= new List<Unit>();
		public List<Unit> geothermalPlants		= new List<Unit>();
		public List<Unit> arachnidFactories		= new List<Unit>();
		public List<Unit> consumerFactories		= new List<Unit>();
		public List<Unit> structureFactories	= new List<Unit>();
		public List<Unit> vehicleFactories		= new List<Unit>();
		public List<Unit> standardLabs			= new List<Unit>();
		public List<Unit> advancedLabs			= new List<Unit>();
		public List<Unit> observatories			= new List<Unit>();
		public List<Unit> reinforcedResidences	= new List<Unit>();
		public List<Unit> advancedResidences	= new List<Unit>();
		public List<Unit> commonOreSmelters		= new List<Unit>();
		public List<Unit> spaceports			= new List<Unit>();
		public List<Unit> rareOreSmelters		= new List<Unit>();
		public List<Unit> gorfs					= new List<Unit>();
		public List<Unit> tokamaks				= new List<Unit>();

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
			foreach (Unit unit in new PlayerUnitEnum(m_Player.playerID))
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

		/// <summary>
		/// Clears all unit lists.
		/// </summary>
		public void Clear()
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

		/// <summary>
		/// Disposes events.
		/// </summary>
		public void Dispose()
		{
			m_TriggerManager.onTriggerFired -= OnTriggerFired;
		}
	}
}