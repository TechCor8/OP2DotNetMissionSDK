// Note: This file is used to define all the group related classes exported
//		 from Outpost2.exe. It also contains the Pinwheel class (which is
//		 like a group class in many ways) and the Trigger class (which
//		 derives from the same base class as all the others).


// Note: ScGroup is the main parent class of all other group classes.
//		 Any functions defined here on this class are available to any
//		 instance of the derived classes.
// Note: Do not try to create an instance of this class. It was meant
//		 simply as a base parent class from which other classes inherit
//		 functions from. Creating an instance of this class serves little
//		 (or no) purpose and may even crash the game.
using DotNetMissionSDK.Async;
using DotNetMissionSDK.HFL;
using System;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK
{
	public class ScGroup : ScStub
	{
		public ScGroup(int stubIndex) : base(stubIndex)
		{
		}

		public void AddUnits(UnitBlock unitsToAdd)		{ ThreadAssert.MainThreadRequired();	ScGroup_AddUnits(stubIndex, unitsToAdd.GetHandle());		}
		public void ClearTargCount()					{ ThreadAssert.MainThreadRequired();	ScGroup_ClearTargCount(stubIndex);							}

		public Unit GetFirstOfType(UnitClassification unitType)
		{
			ThreadAssert.MainThreadRequired();

			int index = ScGroup_GetFirstOfType(stubIndex, unitType);
			if (index < 0)
				return null;

			return new UnitEx(index);
		}

		public Unit GetFirstOfType(map_id unitType, map_id cargoOrWeapon)
		{
			ThreadAssert.MainThreadRequired();

			int index = ScGroup_GetFirstOfType2(stubIndex, unitType, cargoOrWeapon);
			if (index < 0)
				return null;

			return new UnitEx(index);
		}

		public int HasBeenAttacked()					{ ThreadAssert.MainThreadRequired();	return ScGroup_HasBeenAttacked(stubIndex);					}
		public void RemoveUnit(Unit unitToRemove)		{ ThreadAssert.MainThreadRequired();	ScGroup_RemoveUnit(stubIndex, unitToRemove.GetStubIndex());	}
		public void SetDeleteWhenEmpty(int bDelete)		{ ThreadAssert.MainThreadRequired();	ScGroup_SetDeleteWhenEmpty(stubIndex, bDelete);				}
		public void SetLights(int bOn)					{ ThreadAssert.MainThreadRequired();	ScGroup_SetLights(stubIndex, bOn);							}
		public void SetTargCount(UnitBlock unitTypes)	{ ThreadAssert.MainThreadRequired();	ScGroup_SetTargCount(stubIndex, unitTypes.GetHandle());		}

		public void SetTargCount(map_id unitType, map_id weaponType, int targetCount)	{ ThreadAssert.MainThreadRequired();	ScGroup_SetTargCount2(stubIndex, unitType, weaponType, targetCount);	}

		public void TakeAllUnits(ScGroup sourceGroup)	{ ThreadAssert.MainThreadRequired();	ScGroup_TakeAllUnits(stubIndex, sourceGroup.stubIndex);		}
		public void TakeUnit(Unit unitToAdd)			{ ThreadAssert.MainThreadRequired();	ScGroup_TakeUnit(stubIndex, unitToAdd.GetStubIndex());		}
		public int TotalUnitCount()						{ ThreadAssert.MainThreadRequired();	return ScGroup_TotalUnitCount(stubIndex);					}
		public int UnitCount(UnitClassification unitType){ ThreadAssert.MainThreadRequired();	return ScGroup_UnitCount(stubIndex, unitType);				}

		[DllImport("DotNetInterop.dll")] private static extern void ScGroup_AddUnits(int stubIndex, IntPtr unitsToAdd);
		[DllImport("DotNetInterop.dll")] private static extern void ScGroup_ClearTargCount(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern int ScGroup_GetFirstOfType(int stubIndex, UnitClassification unitType);	// ** Typo **
		[DllImport("DotNetInterop.dll")] private static extern int ScGroup_GetFirstOfType2(int stubIndex, map_id unitType, map_id cargoOrWeapon);
		[DllImport("DotNetInterop.dll")] private static extern int ScGroup_HasBeenAttacked(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void ScGroup_RemoveUnit(int stubIndex, int unitToRemoveIndex);
		[DllImport("DotNetInterop.dll")] private static extern void ScGroup_SetDeleteWhenEmpty(int stubIndex, int bDelete);
		[DllImport("DotNetInterop.dll")] private static extern void ScGroup_SetLights(int stubIndex, int bOn);
		[DllImport("DotNetInterop.dll")] private static extern void ScGroup_SetTargCount(int stubIndex, IntPtr unitTypes);
		[DllImport("DotNetInterop.dll")] private static extern void ScGroup_SetTargCount2(int stubIndex, map_id unitType, map_id weaponType, int targetCount);
		[DllImport("DotNetInterop.dll")] private static extern void ScGroup_TakeAllUnits(int stubIndex, int sourceGroup);
		[DllImport("DotNetInterop.dll")] private static extern void ScGroup_TakeUnit(int stubIndex, int unitToAddIndex);
		[DllImport("DotNetInterop.dll")] private static extern int ScGroup_TotalUnitCount(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern int ScGroup_UnitCount(int stubIndex, UnitClassification unitType);	// ** Typo **
	}
	

	// Note: Do not try to create any of the following classes using only their
	//		 constructors. Doing so will likely crash the game. Instead, create
	//		 these classes by calling the group creation functions defined in
	//		 Functions.h.


	// Note: The BuildingGroup class is used to control Structure Factories,
	//		 ConVecs, and Earthworkers to build/rebuild a base.

	public class BuildingGroup : ScGroup
	{
		// Use this to reference an existing group
		public BuildingGroup(int stubIndex) : base(stubIndex)
		{
		}

		// Use this to create a new group
		public BuildingGroup(Player owner) : base(BuildingGroup_Create(owner.GetHandle()))
		{
			ThreadAssert.MainThreadRequired();
		}

		public void RecordBuilding(LOCATION buildingLocation, map_id unitType, map_id cargoOrWeapon)					{ ThreadAssert.MainThreadRequired();	BuildingGroup_RecordBuilding(stubIndex, buildingLocation.x, buildingLocation.y, unitType, cargoOrWeapon);					}
		public void RecordBuilding(LOCATION buildingLocation, map_id unitType, map_id cargoOrWeapon, ScGroup group)		{ ThreadAssert.MainThreadRequired();	BuildingGroup_RecordBuilding2(stubIndex, buildingLocation.x, buildingLocation.y, unitType, cargoOrWeapon, group.stubIndex);	}
		public void RecordTube(LOCATION tubeLocation)																	{ ThreadAssert.MainThreadRequired();	BuildingGroup_RecordTube(stubIndex, tubeLocation.x, tubeLocation.y);														}
		public void RecordTubesTouching(LOCATION startLocation)															{ ThreadAssert.MainThreadRequired();	BuildingGroup_RecordTubesTouching(stubIndex, startLocation.x, startLocation.y);												}
		public void RecordUnitBlock(UnitBlock unitBlock)																{ ThreadAssert.MainThreadRequired();	BuildingGroup_RecordUnitBlock(stubIndex, unitBlock.GetHandle());															}
		public void RecordUnitBlock(UnitBlock unitBlock, ScGroup group)													{ ThreadAssert.MainThreadRequired();	BuildingGroup_RecordUnitBlock2(stubIndex, unitBlock.GetHandle(), group.stubIndex);											}

		// 0 = lowest priority, 0xFFFF = highest
		public void RecordVehReinforceGroup(ScGroup targetGroup, int priority)											{ ThreadAssert.MainThreadRequired();	BuildingGroup_RecordVehReinforceGroup(stubIndex, targetGroup.stubIndex, priority);											}
		public void RecordWall(LOCATION location, map_id wallType)														{ ThreadAssert.MainThreadRequired();	BuildingGroup_RecordWall(stubIndex, location.x, location.y, wallType);														}
		public void SetRect(MAP_RECT defaultLocation)																	{ ThreadAssert.MainThreadRequired();	BuildingGroup_SetRect(stubIndex, defaultLocation.xMin, defaultLocation.yMin, defaultLocation.xMax, defaultLocation.yMax);	}
		public void UnRecordVehGroup(ScGroup group)																		{ ThreadAssert.MainThreadRequired();	BuildingGroup_UnRecordVehGroup(stubIndex, group.stubIndex);																	}

		[DllImport("DotNetInterop.dll")] private static extern int BuildingGroup_Create(IntPtr owner);
		[DllImport("DotNetInterop.dll")] private static extern void BuildingGroup_RecordBuilding(int stubIndex, int buildingX, int buildingY, map_id unitType, map_id cargoOrWeapon);
		[DllImport("DotNetInterop.dll")] private static extern void BuildingGroup_RecordBuilding2(int stubIndex, int buildingX, int buildingY, map_id unitType, map_id cargoOrWeapon, int scGroup);
		[DllImport("DotNetInterop.dll")] private static extern void BuildingGroup_RecordTube(int stubIndex, int tubeX, int tubeY);
		[DllImport("DotNetInterop.dll")] private static extern void BuildingGroup_RecordTubesTouching(int stubIndex, int startX, int startY);
		[DllImport("DotNetInterop.dll")] private static extern void BuildingGroup_RecordUnitBlock(int stubIndex, IntPtr unitBlock);
		[DllImport("DotNetInterop.dll")] private static extern void BuildingGroup_RecordUnitBlock2(int stubIndex, IntPtr unitBlock, int scGroup);
		[DllImport("DotNetInterop.dll")] private static extern void BuildingGroup_RecordVehReinforceGroup(int stubIndex, int targetGroup, int priority); // 0 = lowest priority, 0xFFFF = highest
		[DllImport("DotNetInterop.dll")] private static extern void BuildingGroup_RecordWall(int stubIndex, int locationX, int locationY, map_id wallType);
		[DllImport("DotNetInterop.dll")] private static extern void BuildingGroup_SetRect(int stubIndex, int minX, int minY, int maxX, int maxY);
		[DllImport("DotNetInterop.dll")] private static extern void BuildingGroup_UnRecordVehGroup(int stubIndex, int scGroup);
	}


	// Note: The MiningGroup is a class used to setup mining routes with
	//		 cargo trucks.
	
	public class MiningGroup : ScGroup
	{
		// Use this to reference an existing group
		public MiningGroup(int stubIndex) : base(stubIndex)
		{
		}

		// Use this to create a new group
		public MiningGroup(Player owner) : base(MiningGroup_Create(owner.GetHandle()))
		{
			ThreadAssert.MainThreadRequired();
		}

		public void Setup(LOCATION mine, LOCATION smelter, MAP_RECT smelterArea)										{ ThreadAssert.MainThreadRequired();	MiningGroup_Setup(stubIndex, mine.x, mine.y, smelter.x, smelter.y, smelterArea.xMin, smelterArea.yMin, smelterArea.xMax, smelterArea.yMax);							}
		public void Setup(LOCATION mine, LOCATION smelter, map_id mineType, map_id smelterType, MAP_RECT smelterArea)	{ ThreadAssert.MainThreadRequired();	MiningGroup_Setup2(stubIndex, mine.x, mine.y, smelter.x, smelter.y, mineType, smelterType, smelterArea.xMin, smelterArea.yMin, smelterArea.xMax, smelterArea.yMax);	}
		public void Setup(Unit mine, Unit smelter, MAP_RECT smelterArea)												{ ThreadAssert.MainThreadRequired();	MiningGroup_Setup3(stubIndex, mine.GetStubIndex(), smelter.GetStubIndex(), smelterArea.xMin, smelterArea.yMin, smelterArea.xMax, smelterArea.yMax);					}

		[DllImport("DotNetInterop.dll")] private static extern int MiningGroup_Create(IntPtr playerOwner);
		[DllImport("DotNetInterop.dll")] private static extern void MiningGroup_Setup(int stubIndex, int mineX, int mineY, int smelterX, int smelterY, int smelterAreaMinX, int smelterAreaMinY, int smelterAreaMaxX, int smelterAreaMaxY);
		[DllImport("DotNetInterop.dll")] private static extern void MiningGroup_Setup2(int stubIndex, int mineX, int mineY, int smelterX, int smelterY, map_id mineType, map_id smelterType, int smelterAreaMinX, int smelterAreaMinY, int smelterAreaMaxX, int smelterAreaMaxY);
		[DllImport("DotNetInterop.dll")] private static extern void MiningGroup_Setup3(int stubIndex, int mineIndex, int smelterIndex, int smelterAreaMinX, int smelterAreaMinY, int smelterAreaMaxX, int smelterAreaMaxY);
	}


	// Note: The FightGroup class is used to control military units and is
	//		 used to attack or defend a base. It can be used to cause an AI
	//		 to produce units at a Vehicle Factory to attack with.

	public class FightGroup : ScGroup
	{
		// Use this to reference an existing group
		public FightGroup(int stubIndex) : base(stubIndex)
		{
		}

		// Use this to create a new group
		public FightGroup(Player owner) : base(FightGroup_Create(owner.GetHandle()))
		{
			ThreadAssert.MainThreadRequired();
		}

		public void AddGuardedRect(MAP_RECT guardedRect){ ThreadAssert.MainThreadRequired();	FightGroup_AddGuardedRect(stubIndex, guardedRect.xMin, guardedRect.yMin, guardedRect.xMax, guardedRect.yMax);		}
		public void ClearCombineFire()					{ ThreadAssert.MainThreadRequired();	FightGroup_ClearCombineFire(stubIndex);		}
		public void ClearGuarderdRects()				{ ThreadAssert.MainThreadRequired();	FightGroup_ClearGuarderdRects(stubIndex);	}
		public void ClearPatrolMode()					{ ThreadAssert.MainThreadRequired();	FightGroup_ClearPatrolMode(stubIndex);		}
		public void DoAttackEnemy()						{ ThreadAssert.MainThreadRequired();	FightGroup_DoAttackEnemy(stubIndex);		}
		public void DoAttackUnit()						{ ThreadAssert.MainThreadRequired();	FightGroup_DoAttackUnit(stubIndex);			}
		public void DoExitMap()							{ ThreadAssert.MainThreadRequired();	FightGroup_DoExitMap(stubIndex);			}
		public void DoGuardGroup()						{ ThreadAssert.MainThreadRequired();	FightGroup_DoGuardGroup(stubIndex);			}
		public void DoGuardRect()						{ ThreadAssert.MainThreadRequired();	FightGroup_DoGuardRect(stubIndex);			}
		public void DoGuardUnit()						{ ThreadAssert.MainThreadRequired();	FightGroup_DoGuardUnit(stubIndex);			}
		
		//FightGroup units will no longer chase units that get close, but will still fire at enemies in sight during patrol
		public void DoPatrolOnly()						{ ThreadAssert.MainThreadRequired();	FightGroup_DoPatrolOnly(stubIndex);			}
		
		// ** Use in combination with DoAttackEnemy()
		public void SetAttackType(map_id attackType)	{ ThreadAssert.MainThreadRequired();	FightGroup_SetAttackType(stubIndex, attackType);												}
		public void SetCombineFire()					{ ThreadAssert.MainThreadRequired();	FightGroup_SetCombineFire(stubIndex);															}
		public void SetFollowMode(int followMode)		{ ThreadAssert.MainThreadRequired();	FightGroup_SetFollowMode(stubIndex, followMode);												}
		public void SetPatrolMode(PatrolRoute waypts)	{ ThreadAssert.MainThreadRequired();	FightGroup_SetPatrolMode(stubIndex, waypts.GetHandle());										}
		public void SetRect(MAP_RECT idleRect)			{ ThreadAssert.MainThreadRequired();	FightGroup_SetRect(stubIndex, idleRect.xMin, idleRect.yMin, idleRect.xMax, idleRect.yMax);		}

		// Use in combination with DoGuardGroup()
		public void SetTargetGroup(ScGroup targetGroup)	{ ThreadAssert.MainThreadRequired();	FightGroup_SetTargetGroup(stubIndex, targetGroup.stubIndex);									}
		public void SetTargetUnit(Unit targetUnit)		{ ThreadAssert.MainThreadRequired();	FightGroup_SetTargetUnit(stubIndex, targetUnit.GetStubIndex());									}

		[DllImport("DotNetInterop.dll")] private static extern int FightGroup_Create(IntPtr playerOwner);
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_AddGuardedRect(int stubIndex, int guardedRectMinX, int guardedRectMinY, int guardedRectMaxX, int guardedRectMaxY);
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_ClearCombineFire(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_ClearGuarderdRects(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_ClearPatrolMode(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_DoAttackEnemy(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_DoAttackUnit(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_DoExitMap(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_DoGuardGroup(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_DoGuardRect(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_DoGuardUnit(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_DoPatrolOnly(int stubIndex);                        //FightGroup units will no longer chase units that get close, but will still fire at enemies in sight during patrol
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_SetAttackType(int stubIndex, map_id attackType);     // ** Use in combination with DoAttackEnemy()
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_SetCombineFire(int stubIndex);
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_SetFollowMode(int stubIndex, int followMode);
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_SetPatrolMode(int stubIndex, IntPtr waypts);
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_SetRect(int stubIndex, int idleRectMinX, int idleRectMinY, int idleRectMaxX, int idleRectMaxY);
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_SetTargetGroup(int stubIndex, int targetGroup);  // Use in combination with DoGuardGroup()
		[DllImport("DotNetInterop.dll")] private static extern void FightGroup_SetTargetUnit(int stubIndex, int targetUnitIndex);
	}


	// Note: The Pinwheel class seems to be used for controlling waves
	//		 of attacks.

	/*class OP2 Pinwheel : public ScStub
	{
	public:
		Pinwheel();
		~Pinwheel();
		Pinwheel& operator = (const Pinwheel& pinwheel);

		void SendWaveNow(int);						// **
		void SetAttackComp(int, int, MrRec*);		// **
		void SetAttackFraction(int attackFraction);
		void SetContactDelay(int);					// **
		void SetGuardComp(int, int, MrRec*);		// **
		void SetNoRange(int, int);					// **
		void SetPoints(PWDef*);						// **
		void SetSapperComp(int, int, MrRec*);		// **
		void SetWavePeriod(int minTime, int maxTime);
	};
	*/
}