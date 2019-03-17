// Note: This file stores all the enums exported from Outpost2.exe except for the
//		 map_id enum, which is important enough to get it's own file. =)

namespace DotNetMissionSDK
{
	// Used by various trigger creation functions
	public enum CompareMode
	{
		Equal = 0,					// 0
		LowerEqual,					// 1
		GreaterEqual,				// 2
		Lower,						// 3
		Greater,					// 4
	};

	// Used by CreateResourceTrigger
	public enum TriggerResource
	{
		Food = 0,					// 0
		CommonOre,					// 1
		RareOre,					// 2
		Kids,						// 3
		Workers,					// 4
		Scientists,					// 5
		Colonists,					// 6
	};

	// Used by Unit.SetCargo
	public enum TruckCargo {
		Empty = 0,					// 0
		Food,						// 1
		CommonOre,					// 2
		RareOre,					// 3
		CommonMetal,				// 4
		RareMetal,					// 5
		CommonRubble,				// 6
		RareRubble,					// 7
		Spaceport,					// 8
		Garbage,					// 9  Wreckage

		truckUnit = 0x03F8,			// 3F8
	};

	// Returned by _Player.FoodSupply()
	public enum FoodStatus
	{
		Rising = 0,					// 0
		NoChange,					// 1 - pretty hard, if not impossible to achieve with food
		Falling,					// 2
		Shortage,					// 3
	};

	// Returned by _Player.MoraleLevel()
	public enum MoraleLevel
	{
		Excellent = 0,				// 0
		Good,						// 1
		Fair,						// 2
		Poor,						// 3
		Terrible,					// 4
	};

	// Used by ScGroup.GetFirstOfType, ScGroup.UnitCount, and classes
	// derived from ScGroup which inherit these functions
	// (BuildingGroup, FightGroup, MiningGroup)
	public enum UnitClassification	// [Note: **Typo** in name]
	{
		Attack = 0,					// 0 (Lynx, Panther, Tiger, Scorpion)  [Not ESG, EMP, or Stickyfoam]
		ESG,						// 1 (Lynx, Panther, Tiger)
		EMP,						// 2 (Lynx, Panther, Tiger)
		Stickyfoam,					// 3 (Lynx, Panther, Tiger)
		Spider,						// 4
		Convec,						// 5
		RepairVehicle,				// 6
		CargoTruck,					// 7
		Earthworker,				// 8
		Colony,						// 9 (clsVehicle, not specified elsewhere) (RoboSurveyor, RoboMiner, GeoCon, Scout, RoboDozer, EvacuationTransport)
		VehicleFactory,				// A
		ArachnidFactory,			// B
		StructureFactory,			// C
		OreMine,					// D (CommonOreMine, RareOreMine)
		GuardPost,					// E
		Building,					// F (more like non vehicle, and non other specified class) (does not include Arachnid Factory, includes beacons, disasters, Blast, Tube, pretty much any non vehicle?)
		NotSet = 0x10,				// 10
		All = 0x11,					// 11 All vehicles and buildings
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
	public enum SongID
	{
		Eden11 = 0,		// 0x0
		Eden21,			// 0x1
		Eden22,			// 0x2
		Eden31,			// 0x3
		Eden32,			// 0x4
		Eden33,			// 0x5
		EP41,			// 0x6
		EP42,			// 0x7
		EP43,			// 0x8
		EP51,			// 0x9
		EP52,			// 0xA
		EP61,			// 0xB
		EP62,			// 0xC
		EP63,			// 0xD
		Plymouth11,		// 0xE
		Plymouth12,		// 0xF
		Plymouth21,		// 0x10
		Plymouth22,		// 0x11
		Plymouth31,		// 0x12
		Plymouth32,		// 0x13
		Plymouth33,		// 0x14
		Static01,		// 0x15
		Static02,		// 0x16
		Static03,		// 0x17
		Static04,		// 0x18
		Static05,		// 0x19
	};
}
