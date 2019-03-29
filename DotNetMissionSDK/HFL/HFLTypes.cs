// Defines basic types such as command packet types, unit action types, etc

namespace DotNetMissionSDK.HFL
{
	// Command types, these are used in the command packet headers, they also indicate what
	// a unit has been ordered to do.
	public enum CommandType
	{
		ctNop				= 0x00, // Do nothing
		ctMoDoze			= 0x01,
		ctMoMove			= 0x02,
		ctMoDock			= 0x03,
		ctMoDockEG			= 0x04, // Dock at Garage
		ctMoStop			= 0x05,
		ctMoBuild			= 0x06,
		ctMoBuildWall		= 0x07,
		ctMoRemoveWall		= 0x08,
		ctMoProduce			= 0x09,
		ctMoTransferCargo	= 0x0A,
		ctMoLoadCargo		= 0x0B,
		ctMoUnloadCargo		= 0x0C,
		ctMoRecycle			= 0x0D,
		ctMoDumpCargo		= 0x0E,
		ctMoScavenge		= 0x0F,
		ctMoSpecialWait		= 0x10,
		ctMoSurvey			= 0x11,
		ctMoIdle			= 0x12,
		ctMoUnidle			= 0x13,
		ctMoSelfDestruct	= 0x14,
		ctMoScatter			= 0x15,
		ctMoResearch		= 0x16,
		ctMoTrainScientists	= 0x17,
		ctMoTransfer		= 0x18,
		ctMoLaunch			= 0x19,
		ctMoFlyInSpace		= 0x1A,
		ctMoRepair			= 0x1B,
		ctMoRepairObj		= 0x1C,
		ctMoReprogram		= 0x1D,
		ctMoDismantle		= 0x1E,
		ctMoSalvage			= 0x1F,
		ctMoCreate			= 0x20,
		ctMoDevelop			= 0x21,
		ctMoUnDevelop		= 0x22,
		ctMoLightToggle		= 0x23,
		ctMoAttackObj		= 0x24,
		ctMoGuard			= 0x25,
		ctMoStandGround		= 0x26,
		ctMoCargoRoute		= 0x27,
		ctMoPatrol			= 0x28,
		ctMapChange			= 0x29,
		ctMoPoof			= 0x2A,
		ctGameOpt			= 0x2B,
		ctGodWeapon			= 0x2C,
		ctChatText			= 0x2D,
		ctChatSFX			= 0x2E,
		ctMoDeath			= 0x2F,
		ctChat				= 0x30,
		ctQuit				= 0x31,
		ctAlly				= 0x32,
		ctGoAI				= 0x33,
		ctMachineSettings	= 0x34,
		ctInvalidCommand	= 0x35,
		WeaponFiring		= 0x36
	};

	// Action types, these indicate what a unit is currently doing
	public enum ActionType
	{
		moDone = 0,
		moMove,
		moBuild,
		moBuildMine,
		moRepair,
		moDevelop,
		moObjWait,
		moOperationalWait,
		moEMPWait,
		moLanding,
		moObjFroze,
		moDelSelf,
		moDoResearch,
		moObjDocking,
		moScavenge,
		weaponMove,
		weatherMove,
		empRecover,
		weaponAimCoarse,
		weaponAimFine,
		SantaWalking,
		moInvalid
	};

	// Vehicle track types
	public enum TrackType
	{
		trackLegged = 0,	// 'L'
		trackWheeled,		// 'W'
		trackMiner,			// miner / geo-con uses this type; 'M' in sheets (not sure if it has a real name)
		trackTracked,		// 'T'
		trackHover			// ?? - 'H' in sheets; only allows movement on bulldozed terrain.
	};

	// Report button ID's. Use for CommandPane::GetReportButton and PaneReport::GetLinkedButtonId
	public enum ReportButtonType
	{
		rbtnFactories = 1,
		rbtnLabs,
		rbtnResources,
		rbtnCommunications,
		rbtnSpace,
		rbtnGameSettings
	};

	// Armor classes. The names might only be accurate for structures
	public enum ArmorType
	{
		armorNone = 256,			// class 0 in sheets
		armorVeryLight = 180,		// class 1
		armorLight = 130,			// class 2
		armorMediumLight = 90,		// class 3
		armorMedium = 60,			// class 4
		armorHeavy = 40				// class 5
	};

	// Flags to specify who owns what
	[System.Flags]
	public enum OwnerFlags
	{
		ownerGaia = 0x0,			// 'G' in sheets; No player 'owns' (natural things like beacons, magmavents etc)
		ownerPlymouth = 0x800,		// 'P'
		ownerEden = 0x1000,			// 'E'
		ownerBoth = 0x1800			// 'B'
	};

	// Flags that apply to vehicles. They can be combined with the | (bitwise OR) operator.
	[System.Flags]
	public enum VehicleFlags
	{
		vflagVehicleFactory = 0x1,	// May be produced at VF
		vflagArachnidFactory = 0x2,	// May be produced at AF
		vflagWeaponEnabled = 0x4	// Can have a weapon turret
	};

	// Flags that apply to buildings.
	[System.Flags]
	public enum BuildingFlags
	{
		bflagTubes = 0x1,				// "Border" in sheets
		bflagStructureKit = 0x2,		// Can be produced in the SF? (not set for geo con, miners)
		bflagDockingAll = 0x4,
		bflagDockingTruck = 0x8,		// allows cargo truck to dock
		bflagDockingConvec = 0x10,		// allows convec to dock
		bflagDockingEvac = 0x20,		// ? - allow evac. transport to dock maybe?
		bflagCanBeAutoTargeted = 0x40	// can be automatically targeted. CC doesn't have this set by default (may affect whether something can be EMP'ed as well)
	};

	// text rendering structures
	/*struct RenderHeader
	{
		int numChunks;		// number of render chunks available
		int numLines;		// number of lines used
		int numChunksUsed;	// number of render chunks used
	};

	struct RenderChunk
	{
		int xOffset;		// offset from the left side of the screen
		int stringStart;	// starting char in the input string where this chunk begins
		int stringLen;		// length of this chunk
		int isEOL;			// nonzero if this is the last chunk in the current line of text
		COLORREF color;		// color to use for this chunk
	};

	struct RenderData
	{
		RenderHeader header;
		RenderChunk chunk[1];
	};*/

	// common animation IDs
	public enum AnimId
	{
		animGameSettingsBtn = 141,
		animFactoriesBtn,
		animCommunicationsBtn,
		animResourcesBtn,
		animLabsBtn,
		animSpaceBtn,
		animToggleLightsBtn,		// vehicle light toggle button in Command Pane
		animActiveLight,
		animDisabledLight,
		animIdleLight,
		animBigBtn,					// big blank button
		animSmallBtn,				// small blank button
		animEdenBig,				// eden background logo in Command Pane
		animGlobeBtn,				// minimap globe / flat switch
		animUnitPicBorder,			// green border around unit pic in Command Pane
		animMinusBtn,				// minimap zoom out
		animPlusBtn,				// minimap zoom in
		animTacticalViewBtn,		// minimap tactical / normal view switch
		animPageDownBtn = 453,		// list page down button
		animPageUpBtn,				// list page up button
		animAttackBtn = 1150,
		animBuildBtn,				// normal build button
		animScatterBtn,
		animGuardBtn,
		animDemolishBtn,
		animBulldozeBtn,
		animDumpBtn,
		animBuildMineBtn,			// not used in full version. build mine button
		animPatrolBtn,				// as well as cargo route
		animReprogramBtn,
		animRepairBtn,
		animDestructBtn,			// self destruct button
		animSpecialWallBtn,			// build special wall button
		animStandGroundBtn,
		animStopBtn,
		animTransferBtn,
		animTubeBtn,				// build tube button
		animMoveBtn,
		animNormalWallBtn,			// build normal wall button
		animPower = 1201,			// atom storage bay / research icon (power plants)
		animEmergency,				// red flashing light icon (med center, DIRT)
		animDisaster,				// icon used with disaster research
		animSpace,					// space icon (not pertaining to starship)
		animWeapon,					// weapon research / storage bay
		animFood,					// Agridome / food related research
		animVehicle,				// VF / RCC / vehicles related research
		animColony,					// Morale related icon (residences, forum, rec facility)
		animStructure,				// SF / durability related icon
		animMetal,					// Metals processing related icon
		animLab,					// Lab (storage bay)
		animEden,					// storage bay Eden icon
		animPlymouth,				// storage bay Plymouth icon
		animPlymouthBig = 1344,		// plymouth background logo in Command Pane
		animSalvageBtn = 1560,
		animEmptyBay = 1798,		// empty storage bay icon
		animStarship,				// starship related research / bay icon
		animCommonBeacon1 = 1850,	// 1 bar common
		animCommonBeacon2,
		animCommonBeacon3,
		animRareBeacon1,			// 1 bar rare
		animRareBeacon2,
		animRareBeacon3,
		animUnknownBeacon,			// un-surveyed beacon (white)
		animBlueCircle = 1858,		// blue circle, looks somewhat like 'sonar'. not used in game
		animCheckbox,				// checkbox icon used for toggle buttons
		animArachnid = 1941			// arachnid bay / research icon
	};

	// Standard frame IDs - use unless the button has multiple frames of animation or something else
	public enum FrameId
	{
		frameNormal = 0,
		frameActive,
		frameDisabled
	};

	// Extended map tile used in GetTileEx / SetTileEx
	/*struct MapTile
	{
		unsigned int cellType		:5;		// Cell type of this tile
		unsigned int tileIndex		:11;	// Mapping index (tile graphics to use)
		unsigned int unitIndex		:11;	// Index of unit on this tile
		unsigned int lava			:1;		// true if lava is on the tile
		unsigned int lavaPossible	:1;		// true if lava can flow on the tile
		unsigned int expand			:1;		// true if lava / microbe is expanding to this tile
		unsigned int microbe		:1;		// true if microbe is on the tile
		unsigned int wallOrBuilding	:1;		// true if a wall or building is on the tile
	};*/

	// RCC effect state in SetRCCEffect
	public enum RCCEffectState
	{
		rccNormal = 0,	// No special effect
		rccDisable,		// Disable RCC effect
		rccForce		// Force enable RCC effect
	};

	// Colors for lower status bar
	//#define COLORWHITE (RGB(255,255,255))
	//#define COLORRED (RGB(0,255,255))
	//#define COLORGREEN (RGB(0,255,0))
	//#define COLORBLUE (RGB(0,0,255))
	//#define COLORBLACK (RGB(0,0,0))
}
