// Note: This file stores all the enums exported from Outpost2.exe except for the
//		 map_id enum, which is important enough to get it's own file. =)

namespace DotNetMissionSDK
{
	// Used by various trigger creation functions
	public enum compare_mode
	{
		cmpEqual = 0,				// 0
		cmpLowerEqual,				// 1
		cmpGreaterEqual,			// 2
		cmpLower,					// 3
		cmpGreater,					// 4
	};

	// Used by CreateResourceTrigger
	public enum trig_res
	{
		resFood = 0,				// 0
		resCommonOre,				// 1
		resRareOre,					// 2
		resKids,					// 3
		resWorkers,					// 4
		resScientists,				// 5
		resColonists,				// 6
	};

	// Used by Unit.SetCargo
	public enum Truck_Cargo {
		truckEmpty = 0,				// 0
		truckFood,					// 1
		truckCommonOre,				// 2
		truckRareOre,				// 3
		truckCommonMetal,			// 4
		truckRareMetal,				// 5
		truckCommonRubble,			// 6
		truckRareRubble,			// 7
		truckSpaceport,				// 8
		truckGarbage,				// 9  Wreckage

		truckUnit = 0x03F8,			// 3F8
	};

	// Returned by _Player.FoodSupply()
	public enum FoodStatus
	{
		foodRising = 0,				// 0
		foodNoChange,				// 1 - pretty hard, if not impossible to achieve with food
		foodFalling,				// 2
		foodShortage,				// 3
	};

	// Returned by _Player.MoraleLevel()
	public enum MoraleLevels
	{
		moraleGreat = 0,			// 0
		moraleGood,					// 1
		moraleOK,					// 2
		moralePoor,					// 3
		moraleRotten,				// 4
	};

	// Used by ScGroup.GetFirstOfType, ScGroup.UnitCount, and classes
	// derived from ScGroup which inherit these functions
	// (BuildingGroup, FightGroup, MiningGroup)
	public enum UnitClassifications	// [Note: **Typo** in name]
	{
		clsAttack = 0,				// 0 (Lynx, Panther, Tiger, Scorpion)  [Not ESG, EMP, or Stickyfoam]
		clsESG,						// 1 (Lynx, Panther, Tiger)
		clsEMP,						// 2 (Lynx, Panther, Tiger)
		clsStickyfoam,				// 3 (Lynx, Panther, Tiger)
		clsSpider,					// 4
		clsConvec,					// 5
		clsRepairVehicle,			// 6
		clsCargoTruck,				// 7
		clsEarthworker,				// 8
		clsColony,					// 9 (clsVehicle, not specified elsewhere) (RoboSurveyor, RoboMiner, GeoCon, Scout, RoboDozer, EvacuationTransport)
		clsVehicleFactory,			// A
		clsArachnidFactory,			// B
		clsStructureFactory,		// C
		clsOreMine,					// D (CommonOreMine, RareOreMine)
		clsGuardPost,				// E
		clsBuilding,				// F (more like non vehicle, and non other specified class) (does not include Arachnid Factory, includes beacons, disasters, Blast, Tube, pretty much any non vehicle?)
		clsNotSet = 0x10,			// 10
		clsAll = 0x11,				// 11 All vehicles and buildings
	};

	// Note: clsAttack applies to all attack vehicles with the following weapons:
	//		Microwave
	//		Laser
	//		Rail Gun
	//		Starflare
	//		Supernova
	//		RPG
	//		Acid Cloud
	//		Thor's Hammer
	//		Energy Cannon (Scorpions)
	// Note: Scorpions always appear in this classification (clsAttack), even
	//		 if the weapon is changed to ESG, EMP, Stickyfoam



	// Used to define music playlists
	public enum SongIds
	{
		songEden11 = 0,		// 0x0
		songEden21,			// 0x1
		songEden22,			// 0x2
		songEden31,			// 0x3
		songEden32,			// 0x4
		songEden33,			// 0x5
		songEP41,			// 0x6
		songEP42,			// 0x7
		songEP43,			// 0x8
		songEP51,			// 0x9
		songEP52,			// 0xA
		songEP61,			// 0xB
		songEP62,			// 0xC
		songEP63,			// 0xD
		songPlymth11,		// 0xE
		songPlymth12,		// 0xF
		songPlymth21,		// 0x10
		songPlymth22,		// 0x11
		songPlymth31,		// 0x12
		songPlymth32,		// 0x13
		songPlymth33,		// 0x14
		songStatic01,		// 0x15
		songStatic02,		// 0x16
		songStatic03,		// 0x17
		songStatic04,		// 0x18
		songStatic05,		// 0x19
	};
}
