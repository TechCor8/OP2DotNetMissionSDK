// Note: This file contains the all important map_id enum.
//		 This enum is used for almost all aspects of DLL
//		 level programming. It provides the names used for
//		 all units (vehicles/buildings/disasters/resources)
//		 that can be created and placed on the map.
//		 It also names all weapon types and starship modules
//		 as well as a few other miscellaneous things.

public enum map_id {
	mapAny = -1,					// FF Use to specify 'all' or 'any'

	mapNone = 0,					// 00
	mapCargoTruck,					// 01
	mapConVec,						// 02
	mapSpider,						// 03
	mapScorpion,					// 04
	mapLynx,						// 05
	mapPanther,						// 06
	mapTiger,						// 07
	mapRoboSurveyor,				// 08
	mapRoboMiner,					// 09
	mapGeoCon,						// 0A
	mapScout,						// 0B
	mapRoboDozer,					// 0C
	mapEvacuationTransport,			// 0D
	mapRepairVehicle,				// 0E
	mapEarthworker,					// 0F
	mapSmallCapacityAirTransport,	// 10 Crashes game when it moves (looks like a scout)

	mapTube,						// 11
	mapWall,						// 12
	mapLavaWall,					// 13
	mapMicrobeWall,					// 14

	mapCommonOreMine,				// 15
	mapRareOreMine,					// 16
	mapGuardPost,					// 17
	mapLightTower,					// 18
	mapCommonStorage,				// 19
	mapRareStorage,					// 1A
	mapForum,						// 1B
	mapCommandCenter,				// 1C
	mapMHDGenerator,				// 1D
	mapResidence,					// 1E
	mapRobotCommand,				// 1F
	mapTradeCenter,					// 20
	mapBasicLab,					// 21
	mapMedicalCenter,				// 22
	mapNursery,						// 23
	mapSolarPowerArray,				// 24
	mapRecreationFacility,			// 25
	mapUniversity,					// 26
	mapAgridome,					// 27
	mapDIRT,						// 28
	mapGarage,						// 29
	mapMagmaWell,					// 2A
	mapMeteorDefense,				// 2B
	mapGeothermalPlant,				// 2C
	mapArachnidFactory,				// 2D
	mapConsumerFactory,				// 2E
	mapStructureFactory,			// 2F
	mapVehicleFactory,				// 30
	mapStandardLab,					// 31
	mapAdvancedLab,					// 32
	mapObservatory,					// 33
	mapReinforcedResidence,			// 34
	mapAdvancedResidence,			// 35
	mapCommonOreSmelter,			// 36
	mapSpaceport,					// 37
	mapRareOreSmelter,				// 38
	mapGORF,						// 39
	mapTokamak,						// 3A

	mapAcidCloud,					// 3B
	mapEMP,							// 3C
	mapLaser,						// 3D
	mapMicrowave,					// 3E
	mapRailGun,						// 3F
	mapRPG,							// 40
	mapStarflare,					// 41 Vehicle Starflare
	mapSupernova,					// 42 Vehicle Supernova
	mapStarflare2,					// 43 GuardPost Starflare
	mapSupernova2,					// 44 GuardPost Supernova
	mapNormalUnitExplosion,			// 45
	mapESG,							// 46
	mapStickyfoam,					// 47
	mapThorsHammer,					// 48
	mapEnergyCannon,				// 49

	mapBlast,						// 4A EMP/Sticky foam blast
	//mapUnknown4B,					// 4B Unknown what this is  "BFG"

	mapLightning = 0x4C,			// 4C
	mapVortex,						// 4D
	mapEarthquake,					// 4E
	mapEruption,					// 4F
	mapMeteor,						// 50

	mapMiningBeacon,				// 51
	mapMagmaVent,					// 52
	mapFumarole,					// 53

	mapWreckage,					// 54

	mapDisasterousBuildingExplosion,	// 55
	mapCatastrophicBuildingExplosion,	// 56
	mapAtheistBuildingExplosion,		// 57

	mapEDWARDSatellite,				// 58  Lynx (in Cargo Truck)
	mapSolarSatellite,				// 59  Wreckage (in Cargo Truck)
	mapIonDriveModule,				// 5A  Gene Bank 5 (in Cargo Truck)
	mapFusionDriveModule,			// 5B
	mapCommandModule,				// 5C
	mapFuelingSystems,				// 5D
	mapHabitatRing,					// 5E
	mapSensorPackage,				// 5F
	mapSkydock,						// 60
	mapStasisSystems,				// 61
	mapOrbitalPackage,				// 62
	mapPhoenixModule,				// 63

	mapRareMetalsCargo,				// 64
	mapCommonMetalsCargo,			// 65
	mapFoodCargo,					// 66
	mapEvacuationModule,			// 67
	mapChildrenModule,				// 68

	mapSULV,						// 69
	mapRLV,							// 6A
	mapEMPMissile,					// 6B

	mapImpulseItems,				// 6C
	mapWares,						// 6D
	mapLuxuryWares,					// 6E

	mapInterColonyShuttle,			// 6F
	mapSpider3Pack,					// 70
	mapScorpion3Pack,				// 71

	mapPrettyArt,					// 72 (Used for explosions)

	mapGeneralUnit,					// 73 Don't try to create this unless you're implementing a new unit class
};
