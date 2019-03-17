// Note: This file is used for enums where the name of the enum is not
//		 specified by exports from Outpost2.exe. The names of the enums
//		 were chosen arbitrarily, however the values in the enums
//		 represent functions hardcoded into the exe and will not change.
//		 These values are used as function arguments where the parameter
//		 type is simply set to an int, but each int value has a specific
//		 and seperate meaning.

namespace DotNetMissionSDK
{
	public enum PlayerID
	{
		PlayerAll = -1,
		Player0 = 0,
		Player1 = 1,
		Player2 = 2,
		Player3 = 3,
		Player4 = 4,
		Player5 = 5,
		Player6 = 6,
	};

	public enum PlayerDifficulty
	{
		Easy = 0,
		Normal = 1,
		Hard = 2,
	};

	// Mining beacon's
	public enum BeaconType
	{
		Random = -1,
		Common = 0,
		Rare = 1,
	};

	// Yield Type's (Number of "Bars" a mine has)
	public enum Yield
	{
		Random = -1,
		Bar3 = 0,
		Bar2 = 1,
		Bar1 = 2,
	};

	// Used when creating Beacons to determine which variant of
	// a certain "bar" yield to produce.
	public enum Variant
	{
		Random = -1,		// Randomly choose one of the 3 variant values
		Variant1 = 0,
		Variant2 = 1,
		Variant3 = 2,
	};


	// Damage Types used by CreateDamagedTrigger
	// Refers to % of FightGroup that has been destroyed
	public enum DamageType
	{
		Damage100	= 1,		// 100% Damaged
		Damage75	= 2,		//  75% Damaged
		Damage50	= 3,		//  50% Damaged
	};


	// Marker Types
	public enum MarkerType
	{
		Circle	= 0,			// Circular marker
		DNA		= 1,			// DNA strand
		Beaker	= 2,			// Beaker
	};


	// Unit direction constants for the last parameter to CreateUnit
	public enum UnitDirection
	{
		East		= 0,
		SouthEast	= 1,
		South		= 2,
		SouthWest	= 3,
		West		= 4,
		NorthWest	= 5,
		North		= 6,
		NorthEast	= 7,
	};


	// CellTypes returned and set by the GameMap class
	public enum CellType
	{
		FastPassible1 = 0,	// Rock vegetation
		Impassible2,		// Meteor craters, cracks/crevases
		SlowPassible1,		// Lava rock (dark)
		SlowPassible2,		// Rippled dirt/Lava rock bumps
		MediumPassible1,	// Dirt
		MediumPassible2,	// Lava rock
		Impassible1,		// Dirt/Rock/Lava rock mound/ice/volcano
		FastPassible2,		// Rock
		NorthCliffs,
		CliffsHighSide,
		CliffsLowSide,
		VentsAndFumaroles,	// Fumaroles (passable by GeoCons)
		zPad12,
		zPad13,
		zPad14,
		zPad15,
		zPad16,
		zPad17,
		zPad18,
		zPad19,
		zPad20,
		DozedArea,
		Rubble,
		NormalWall,
		MicrobeWall,
		LavaWall,
		Tube0,				// Used for tubes and areas under buildings
		Tube1,				// Note: Tube values 1-5 don't appear to be used
		Tube2,
		Tube3,
		Tube4,
		Tube5,
	};

	// Color of structures and units belonging to a given player
	public enum PlayerColor
	{
		Blue = 0, //Standard Eden color
		Red,      //Standard Plymouth color
		Green,
		Yellow,
		Cyan,
		Magenta,
		Black     //Not selectable as a player color from the multiplayer game initialization screen
	};

	public enum MissionType
	{
		Colony					= -1, //0xFF	// c
		AutoDemo				= -2, //0xFF	// a
		Tutorial				= -3, //0xFD	// t

		MultiLandRush			= -4, //0xFC	// mu<x>
		MultiSpaceRace			= -5, //0xFB	// mf<x>
		MultiResourceRace		= -6, //0xFA	// mr<x>
		MultiMidas				= -7, //0xF9	// mm<x>
		MultiLastOneStanding	= -8, //0xF8	// ml<x>
	};
}
