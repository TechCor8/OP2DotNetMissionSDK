// Note: This file contains the all important map_id enum.
//		 This enum is used for almost all aspects of DLL
//		 level programming. It provides the names used for
//		 all units (vehicles/buildings/disasters/resources)
//		 that can be created and placed on the map.
//		 It also names all weapon types and starship modules
//		 as well as a few other miscellaneous things.

namespace DotNetMissionSDK
{
	public enum map_id
	{
		Any = -1,						// FF Use to specify 'all' or 'any'

		None = 0,						// 00
		CargoTruck,						// 01
		ConVec,							// 02
		Spider,							// 03
		Scorpion,						// 04
		Lynx,							// 05
		Panther,						// 06
		Tiger,							// 07
		RoboSurveyor,					// 08
		RoboMiner,						// 09
		GeoCon,							// 0A
		Scout,							// 0B
		RoboDozer,						// 0C
		EvacuationTransport,			// 0D
		RepairVehicle,					// 0E
		Earthworker,					// 0F
		SmallCapacityAirTransport,		// 10 Crashes game when it moves (looks like a scout)

		Tube,							// 11
		Wall,							// 12
		LavaWall,						// 13
		MicrobeWall,					// 14

		CommonOreMine,					// 15
		RareOreMine,					// 16
		GuardPost,						// 17
		LightTower,						// 18
		CommonStorage,					// 19
		RareStorage,					// 1A
		Forum,							// 1B
		CommandCenter,					// 1C
		MHDGenerator,					// 1D
		Residence,						// 1E
		RobotCommand,					// 1F
		TradeCenter,					// 20
		BasicLab,						// 21
		MedicalCenter,					// 22
		Nursery,						// 23
		SolarPowerArray,				// 24
		RecreationFacility,				// 25
		University,						// 26
		Agridome,						// 27
		DIRT,							// 28
		Garage,							// 29
		MagmaWell,						// 2A
		MeteorDefense,					// 2B
		GeothermalPlant,				// 2C
		ArachnidFactory,				// 2D
		ConsumerFactory,				// 2E
		StructureFactory,				// 2F
		VehicleFactory,					// 30
		StandardLab,					// 31
		AdvancedLab,					// 32
		Observatory,					// 33
		ReinforcedResidence,			// 34
		AdvancedResidence,				// 35
		CommonOreSmelter,				// 36
		Spaceport,						// 37
		RareOreSmelter,					// 38
		GORF,							// 39
		Tokamak,						// 3A

		AcidCloud,						// 3B
		EMP,							// 3C
		Laser,							// 3D
		Microwave,						// 3E
		RailGun,						// 3F
		RPG,							// 40
		Starflare,						// 41 Vehicle Starflare
		Supernova,						// 42 Vehicle Supernova
		Starflare2,						// 43 GuardPost Starflare
		Supernova2,						// 44 GuardPost Supernova
		NormalUnitExplosion,			// 45
		ESG,							// 46
		Stickyfoam,						// 47
		ThorsHammer,					// 48
		EnergyCannon,					// 49

		Blast,							// 4A EMP/Sticky foam blast
		//Unknown4B,					// 4B Unknown what this is  "BFG"

		Lightning = 0x4C,				// 4C
		Vortex,							// 4D
		Earthquake,						// 4E
		Eruption,						// 4F
		Meteor,							// 50

		MiningBeacon,					// 51
		MagmaVent,						// 52
		Fumarole,						// 53

		mapWreckage,					// 54

		DisasterousBuildingExplosion,	// 55
		CatastrophicBuildingExplosion,	// 56
		AtheistBuildingExplosion,		// 57

		EDWARDSatellite,				// 58  Lynx (in Cargo Truck)
		SolarSatellite,					// 59  Wreckage (in Cargo Truck)
		IonDriveModule,					// 5A  Gene Bank 5 (in Cargo Truck)
		FusionDriveModule,				// 5B
		CommandModule,					// 5C
		FuelingSystems,					// 5D
		HabitatRing,					// 5E
		SensorPackage,					// 5F
		Skydock,						// 60
		StasisSystems,					// 61
		OrbitalPackage,					// 62
		PhoenixModule,					// 63

		RareMetalsCargo,				// 64
		CommonMetalsCargo,				// 65
		FoodCargo,						// 66
		EvacuationModule,				// 67
		ChildrenModule,					// 68

		SULV,							// 69
		RLV,							// 6A
		EMPMissile,						// 6B

		ImpulseItems,					// 6C
		Wares,							// 6D
		LuxuryWares,					// 6E

		InterColonyShuttle,				// 6F
		Spider3Pack,					// 70
		Scorpion3Pack,					// 71

		PrettyArt,						// 72 (Used for explosions)

		GeneralUnit,					// 73 Don't try to create this unless you're implementing a new unit class
	}
}
