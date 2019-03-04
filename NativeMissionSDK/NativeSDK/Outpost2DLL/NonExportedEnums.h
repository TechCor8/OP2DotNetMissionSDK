#pragma once

// Note: This file is used for enums where the name of the enum is not
//		 specified by exports from Outpost2.exe. The names of the enums
//		 were chosen arbitrarily, however the values in the enums
//		 represent functions hardcoded into the exe and will not change.
//		 These values are used as function arguments where the parameter
//		 type is simply set to an int, but each int value has a specific
//		 and seperate meaning.




const int AllPlayers = -1;

enum PlayerNum
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

enum PlayerDifficulty
{
	DiffEasy = 0,
	DiffNormal = 1,
	DiffHard = 2,
};

// Mining beacon's
enum BeaconTypes
{
	OreTypeRandom = -1,
	OreTypeCommon = 0,
	OreTypeRare = 1,
};

// Yield Type's (Number of "Bars" a mine has)
enum Yield
{
	BarRandom = -1,
	Bar3 = 0,
	Bar2 = 1,
	Bar1 = 2,
};

// Used when creating Beacons to determine which variant of
// a certain "bar" yield to produce.
enum Variant
{
	VariantRandom = -1,		// Randomly choose one of the 3 variant values
	Variant1 = 0,
	Variant2 = 1,
	Variant3 = 2,
};


// Damage Types used by CreateDamagedTrigger
// Refers to % of FightGroup that has been destroyed
enum DamageType
{
	Damage100	= 1,		// 100% Damaged
	Damage75	= 2,		//  75% Damaged
	Damage50	= 3,		//  50% Damaged
};


// Marker Types
enum MarkerTypes
{
	Circle	= 0,			// Circular marker
	DNA		= 1,			// DNA strand
	Beaker	= 2,			// Beaker
};


// Unit direction constants for the last parameter to CreateUnit
enum UnitDirection
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
enum CellTypes
{
	cellFastPassible1 = 0,	// Rock vegetation
	cellImpassible2,		// Meteor craters, cracks/crevases
	cellSlowPassible1,		// Lava rock (dark)
	cellSlowPassible2,		// Rippled dirt/Lava rock bumps
	cellMediumPassible1,	// Dirt
	cellMediumPassible2,	// Lava rock
	cellImpassible1,		// Dirt/Rock/Lava rock mound/ice/volcano
	cellFastPassible2,		// Rock
	cellNorthCliffs,
	cellCliffsHighSide,
	cellCliffsLowSide,
	cellVentsAndFumaroles,	// Fumaroles (passable by GeoCons)
	cellzPad12,
	cellzPad13,
	cellzPad14,
	cellzPad15,
	cellzPad16,
	cellzPad17,
	cellzPad18,
	cellzPad19,
	cellzPad20,
	cellDozedArea,
	cellRubble,
	cellNormalWall,
	cellMicrobeWall,
	cellLavaWall,
	cellTube0,				// Used for tubes and areas under buildings
	cellTube1,				// Note: Tube values 1-5 don't appear to be used
	cellTube2,
	cellTube3,
	cellTube4,
	cellTube5,
};

// Color of structures and units belonging to a given player
enum PlayerColor
{
	PlayerBlue = 0, //Standard Eden color
	PlayerRed,      //Standard Plymouth color
	PlayerGreen,
	PlayerYellow,
	PlayerCyan,
	PlayerMagenta,
	PlayerBlack     //Not selectable as a player color from the multiplayer game initialization screen
};
