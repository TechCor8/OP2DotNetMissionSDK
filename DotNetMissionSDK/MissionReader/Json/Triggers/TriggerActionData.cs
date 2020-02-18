using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	public enum TriggerActionType
	{
		//WaitMarks,										// Wait [value] Game Marks
		WaitTicks,										// Wait [value] Game Ticks
		PreserveTrigger,								// Preserve Trigger
		MoveRegionToUnit,								// Move [region] to [CurrentUnit/UnitID]
		MoveRegionToPosition,							// Move [region] to [x],[y]
		MoveRegionToRegion,								// Move [region] to [region]
		MoveRegionToUnitTypeInRegion,					// Move [region] to [unit.map_id] with [weapon] owned by [Player] in [region]
		SetRegionSize,									// [Set] size of [region] to [x],[y]
		SetSwitch,										// [Set] [Switch#] to [value]
		
		SetPlayerName = 1000,							// Set [CurrentPlayer/Player#] name to [value]
		SetPlayerColor,									// Set [CurrentPlayer/Player#] color to [value]
		SetPlayerBotType,								// Set bot type for [CurrentPlayer/Player#] to [value]
		SetPlayerDefeated,								// [CurrentPlayer/Player#] defeated
		SetPlayerRLVs,									// [Add] [value] RLVs to [All/CurrentPlayer/Player#]
		SetPlayerSolarSatellites,						// [Add] [value] solar satellites to [All/CurrentPlayer/Player#]
		SetPlayerEDWARDSatellites,						// [Add] [value] EDWARD satellites to [All/CurrentPlayer/Player#]
		SetPlayerKids,									// [Add] [value] kids to [All/CurrentPlayer/Player#]
		SetPlayerWorkers,								// [Add] [value] workers to [All/CurrentPlayer/Player#]
		SetPlayerScientists,							// [Add] [value] scientists to [All/CurrentPlayer/Player#]
		SetPlayerCommonMetal,							// [Add] [value] common metal to [All/CurrentPlayer/Player#]
		SetPlayerRareMetal,								// [Add] [value] rare metal to [All/CurrentPlayer/Player#]
		SetPlayerFood,									// [Add] [value] food to [All/CurrentPlayer/Player#]
		SetPlayerTechLevel,								// Set [All/CurrentPlayer/Player#] tech level to [value]
		StarvePlayerPopulation,							// Starve [value] people for [All/CurrentPlayer/Player#] skip morale update [boolean]
		SetPlayerResearchCompleted,						// Mark research topic [TopicID] complete for [All/CurrentPlayer/Player#]
		SetPlayerAlliance,								// Ally [CurrentPlayer/Player#] with [Player#]
		CapturePlayerRLV,								// Capture RLV from [CurrentPlayer/Player#] and give to [CurrentPlayer/Player#]
		SetPlayerCameraToRegion,						// Center [All/CurrentPlayer/Player#] view on [region]

		ShowMessageToPlayerAtRegion,					// Display message [text] for player [All/CurrentPlayer/Player#] with sound [value] at [anywhere/region]
		ShowMessageToPlayerForUnit,						// Display message [text] for player [All/CurrentPlayer/Player#] with sound [value] for unit [CurrentUnit/UnitID]
		ShowMessageToPlayerFromPlayer,					// Display message [text] for player [All/CurrentPlayer/Player#] with sound [value] from player [CurrentPlayer/Player#]
		
		SetDaylightEverywhere,							// Set daylight everywhere [boolean]
		SetDaylightMoves,								// Set daylight moves [boolean]
		SetPlayerMorale,								// Set morale [Excellent/Good/Fair/Poor/Terrible] for [All/CurrentPlayer/Player#]
		FreePlayerMorale,								// Free morale for [All/CurrentPlayer/Player#]
		SetRandomSeed,									// Set random seed [value]

		CreateMeteor,									// Create meteor with size [value] in [region]
		CreateEarthquake,								// Create earthquake with magnitude [value] in [region]
		CreateStorm,									// Create storm with duration [value] in [region] to [region]
		CreateVortex,									// Create tornado with duration [value] in [region] to [region] immediately [boolean]
		CreateVolcano,									// Create volcano with spread speed [value] in [region]
		CreateMicrobe,									// Create microbe in [region] fill [boolean]
		SetMicrobeSpreadSpeed,							// Set microbe spread speed to [value]
		RemoveMicrobe,									// Remove microbe in [region] fill [boolean]

		FireMissileEMP,									// Fire EMP missile from [region] to [region] for [CurrentPlayer/Player#]

		SetMusic,										// Set music [ID CSV] repeat to [index]

		SetTileIndex,									// Set tile index to [value] in [region] fill [boolean]
		SetCellType,									// Set cell type to [value] in [region] fill [boolean]
		SetLavaPossible,								// Set lava possible to [value] in [region] fill [boolean]
		SetLightLevel,									// Set light level to [value]

		SetUnitsInRegionToAttackEnemyUnits=2000,		// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to attack enemy units in [region]
		SetUnitsInRegionToAttackGround,					// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to attack ground in [region]
		DeployMinesInRegion,							// Set [quantity] robominer for [All/CurrentPlayer/Player#] in [region] to deploy mine in [region]
		SetUnitsInRegionToBulldoze,						// Set [quantity] robodozer for [All/CurrentPlayer/Player#] in [region] to bulldoze [region]
		SetUnitsInRegionToDock,							// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to dock at structure in [region]
		//SetUnitsToDockInRegion,						// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to dock in [region]
		SetUnitsInRegionToStandGround,					// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to stand ground in [region]
		SetEarthworkersInRegionToBuildWallOrTube,		// Set [quantity] earthworker for [All/CurrentPlayer/Player#] in [region] to build [wall.map_id] at [region]
		SetEarthworkersInRegionToRemoveWallOrTube,		// Set [quantity] earthworker for [All/CurrentPlayer/Player#] in [region] to remove wall at [region]
		SetFactoriesInRegionToProduceUnit,				// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to produce unit [map_id] with cargo or weapon [map_id]
		SetStructuresInRegionToTransferBayCargo,		// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to transfer cargo in [bay]
		SetStructuresInRegionToLoadCargo,				// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to load cargo
		SetStructuresInRegionToUnloadCargo,				// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to unload cargo
		SetTrucksInRegionToDumpCargo,					// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to dump cargo
		SetLabsInRegionToResearch,						// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to research [techIndex] with [quantity] scientists
		SetUniversitiesInRegionToTrain,					// Set [quantity] universities for [All/CurrentPlayer/Player#] in [region] to train [quantity] scientists
		SetUnitsInRegionToRepair,						// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to repair unit in [region]
		SetSpidersInRegionToReprogram,					// Set [quantity] spider for [All/CurrentPlayer/Player#] in [region] to reprogram unit in [region]
		SetConvecsInRegionToDismantle,					// Set [quantity] convec for [All/CurrentPlayer/Player#] in [region] to dismantle unit in [region]
		SetTrucksInRegionToSalvage,						// Set [quantity] cargo truck for [All/CurrentPlayer/Player#] in [region] to salvage [region] and drop off at [region]
		SetUnitsInRegionToGuard,						// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to guard unit in [region]
		SetUnitsInRegionToPoof,							// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to poof
		SetSpaceportsInRegionToTransferLaunchpadCargo,	// Set [quantity] spaceport for [All/CurrentPlayer/Player#] in [region] to transfer launchpad cargo in [bay]
		SetHealthForUnitsInRegion,						// Set health of [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to [value] %
		SetAutotargetForUnitsInRegion,					// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to autotarget [boolean]
		KillUnitsInRegion,								// Kill [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region]
		SetUnitsInRegionToSelfDestruct,					// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to self destruct
		TransferUnitsInRegionToPlayer,					// Transfer [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to [CurrentPlayer/Player#]
		SetWeaponForUnitsInRegion,						// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] weapon to [weapon.map_id]
		SetLightsForUnitsInRegion,						// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] lights [boolean]
		SetUnitsInRegionToMove,							// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to move to [region]
		TeleportUnitsInRegion,							// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to teleport to [region]
		SetConvecsInRegionToBuild,						// Set [quantity] convec for [All/CurrentPlayer/Player#] in [region] to build structure at [region]
		SetCargoForConvecsInRegion,						// Set cargo of [quantity] convec for [All/CurrentPlayer/Player#] in [region] to [map_id] with [weapon.map_id]
		SetCargoForTrucksInRegion,						// Set cargo of [quantity] truck for [All/CurrentPlayer/Player#] in [region] to [quantity] [TruckCargo]

		SetStructuresInRegionToIdle,					// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to idle
		SetStructuresInRegionToActivate,				// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to activate
		SetStructuresInRegionToStop,					// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to stop
		SetStructuresInRegionToInfected,				// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to infected
		SetSpaceportsInRegionToLaunch,					// Set [quantity] spaceport for [All/CurrentPlayer/Player#] in [region] to launch and force [boolean]
		SetStructuresInRegionBayCargo,					// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] [bay] to [map_id] with cargo or weapon [map_id]
		SetFactoriesInRegionToDevelop,					// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to develop [map_id]
		SetUnitsInRegionToClearSpecialTarget,			// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to clear special target

		SetUnitToAttackEnemyUnits=3000,					// Set [CurrentUnit/UnitID] to attack unit in [region]
		SetUnitToAttackGround,							// Set [CurrentUnit/UnitID] to attack ground in [region]
		DeployMine,										// Set [CurrentUnit/UnitID] to deploy mine in [region]
		SetUnitToBulldoze,								// Set [CurrentUnit/UnitID] to bulldoze [region]
		SetUnitToDock,									// Set [CurrentUnit/UnitID] to dock at structure in [region]
		SetUnitToStandGround,							// Set [CurrentUnit/UnitID] to stand ground in [region]
		SetEarthworkerToBuildWallOrTube,				// Set [CurrentUnit/UnitID] to build [wall.map_id] at [region]
		SetEarthworkerToRemoveWallOrTube,				// Set [CurrentUnit/UnitID] to remove wall at [region]
		SetFactoryToProduceUnit,						// Set [CurrentUnit/UnitID] to produce unit [map_id] with cargo or weapon [map_id]
		SetStructureToTransferBayCargo,					// Set [CurrentUnit/UnitID] to transfer cargo in [bay]
		SetStructureToLoadCargo,						// Set [CurrentUnit/UnitID] to load cargo
		SetStructureToUnloadCargo,						// Set [CurrentUnit/UnitID] to unload cargo
		SetTruckToDumpCargo,							// Set [CurrentUnit/UnitID] to dump cargo
		SetLabToResearch,								// Set [CurrentUnit/UnitID] to research tech [index] with [quantity] scientists
		SetUniversityToTrain,							// Set [CurrentUnit/UnitID] to train [quantity] scientists
		SetUnitToRepair,								// Set [CurrentUnit/UnitID] to repair unit in [region]
		SetSpiderToReprogram,							// Set [CurrentUnit/UnitID] to reprogram unit in [region]
		SetConvecToDismantle,							// Set [CurrentUnit/UnitID] to dismantle unit in [region]
		SetTruckToSalvage,								// Set [CurrentUnit/UnitID] to salvage [region] and drop off at [region]
		SetUnitToGuard,									// Set [CurrentUnit/UnitID] to guard unit in [region]
		SetUnitToPoof,									// Set [CurrentUnit/UnitID] to poof (self destruct?)
		SetSpaceportToTransferLaunchpadCargo,			// Set [CurrentUnit/UnitID] to transfer launchpad cargo in [bay]
		SetHealthForUnit,								// Set [CurrentUnit/UnitID] health to [value] %
		SetAutotargetForUnit,							// Set [CurrentUnit/UnitID] to auto target [boolean]
		KillUnit,										// Kill [CurrentUnit/UnitID]
		SetUnitToSelfDestruct,							// Set [CurrentUnit/UnitID] to self destruct
		TransferUnitToPlayer,							// Transfer [CurrentUnit/UnitID] to player [CurrentPlayer/Player#]
		SetWeaponForUnit,								// Set [CurrentUnit/UnitID] weapon to [map_id]
		SetLightsForUnit,								// Set [CurrentUnit/UnitID] lights [boolean]
		SetUnitToMove,									// Set [CurrentUnit/UnitID] to move to [region]
		TeleportUnit,									// Set [CurrentUnit/UnitID] to teleport to [region]
		SetConvecToBuild,								// Set convec [CurrentUnit/UnitID] to build structure at [region]
		SetCargoForConvec,								// Set convec [CurrentUnit/UnitID] cargo to [map_id] with weapon [map_id]
		SetCargoForTruck,								// Set truck [CurrentUnit/UnitID] cargo to [quantity] [TruckCargo]
		
		SetStructureToIdle,								// Set structure [CurrentUnit/UnitID] to idle
		SetStructureToActivate,							// Set structure [CurrentUnit/UnitID] to activate
		SetStructureToStop,								// Set structure [CurrentUnit/UnitID] to stop
		SetStructureToInfected,							// Set structure [CurrentUnit/UnitID] to infected
		SetSpaceportToLaunch,							// Set spaceport [CurrentUnit/UnitID] to launch and force [boolean]
		// Put in garage?
		SetStructureBayCargo,							// Set factory [CurrentUnit/UnitID] [bay] to [map_id] with cargo or weapon [map_id]
		SetFactoryToDevelop,							// Set factory [CurrentUnit/UnitID] to develop [map_id]
		SetUnitToClearSpecialTarget,					// Set [CurrentUnit/UnitID] to clear special target

		CreateCargoTruck=4000,							// Create cargo truck with [quantity] [TruckCargo] in [region] facing [direction] with [health] [lights] and [UnitID]
		CreateConvec,									// Create convec with [cargo.map_id] [weapon.map_id] in [region] facing [direction] with [health] [lights] and [UnitID]
		CreateVehicle,									// Create vehicle [map_id] [weapon.map_id] in [region] facing [direction] with [health] [lights] and [UnitID]
		CreateStructure,								// Create structure [map_id] [weapon.map_id] in [region] with [health] and [UnitID]
		CreateMine,										// Create mine [map_id] in [region] with [barYield] [barVariant] [health] and [UnitID]
		CreateBeacon,									// Create beacon [map_id] in [region] with [oreType] [barYield] [barVariant]
		CreateMarker,									// Create marker [MarkerType] in [region] with [UnitID]
		CreateWreckage,									// Create [visible] wreckage [TechID] in [region]
		CreateWallOrTube,								// Create wall or tube [map_id] in [region]
	}

	[DataContract]
	public class TriggerActionData
	{
		[DataMember(Name = "Type")]					private string m_Type				{ get; set; }
		[DataMember(Name = "Modifier")]				private string m_Modifier			{ get; set; } // Set, add, subtract
		[DataMember(Name = "Subject")]				public string subject				{ get; set; } // TriggerPlayerCategory, TriggerUnitCategory, Switch#
		[DataMember(Name = "Subject2")]				public string subject2				{ get; set; } // map_id weapon
		[DataMember(Name = "SubjectPlayer")]		public string subjectPlayer			{ get; set; } // TriggerPlayerCategory
		[DataMember(Name = "SubjectRegion")]		public string subjectRegion			{ get; set; } // Region
		[DataMember(Name = "SubjectQuantity")]		public int subjectQuantity			{ get; set; }
		
		[DataMember(Name = "Value")]				public string value					{ get; set; }
		[DataMember(Name = "Value2")]				public string value2				{ get; set; }
		[DataMember(Name = "Value3")]				public string value3				{ get; set; }
		[DataMember(Name = "Value4")]				public string value4				{ get; set; }
		[DataMember(Name = "Value5")]				public string value5				{ get; set; }

		public TriggerActionType type							{ get { return GetEnum<TriggerActionType>(m_Type);			} set { m_Type = value.ToString();				} }
		public TriggerModifier modifier							{ get { return GetEnum<TriggerModifier>(m_Modifier);		} set { m_Modifier = value.ToString();			} }


		// Integer subject and values
		/*public int GetIntegerSubject()							{ int result; int.TryParse(subject, out result);	return result;		}
		public int GetIntegerSubjectPlayer()					{ int result; int.TryParse(subjectPlayer, out result);	return result;	}
		public int GetIntegerSubjectRegion()					{ int result; int.TryParse(subjectRegion, out result);	return result;	}
		public int GetIntegerValue()							{ int result; int.TryParse(value, out result);		return result;		}
		public int GetIntegerValue2()							{ int result; int.TryParse(value2, out result);		return result;		}
		public int GetIntegerValue3()							{ int result; int.TryParse(value3, out result);		return result;		}
		public int GetIntegerValue4()							{ int result; int.TryParse(value4, out result);		return result;		}
		public int GetIntegerValue5()							{ int result; int.TryParse(value5, out result);		return result;		}*/

		// Parse subject as enum and return as integer
		/*public int GetSubjectAsPlayerCategory()					{ return GetEnumOrInt<TriggerPlayerCategory>(subject);					}
		public int GetSubjectAsMapID()							{ return GetEnumOrInt<map_id>(subject);									}
		public int GetSubjectAsUnitCategory()					{ return GetEnumOrInt<TriggerUnitCategory>(subject);					}
		public int GetSubjectAsMarkerType()						{ return GetEnumOrInt<MarkerType>(subject);								}
		public int GetSubjectAsTruckCargo()						{ return GetEnumOrInt<TruckCargo>(subject);								}*/

		public int GetSubjectPlayer()							{ return GetEnumOrInt<TriggerPlayerCategory>(subjectPlayer);			}
		public int GetSubjectRegion()							{ return GetEnumOrInt<TriggerRegion>(subjectRegion);					}

		// Parse value as enum
		/*public TriggerPlayerCategory GetValueAsPlayerCategory()	{ return GetEnum<TriggerPlayerCategory>(value);							}
		public MoraleLevel GetValueAsMorale()					{ return GetEnum<MoraleLevel>(value);									}
		public TruckCargo GetValueAsTruckCargo()				{ return GetEnum<TruckCargo>(value);									}
		public map_id GetValueAsMapID()							{ return GetEnum<map_id>(value);										}
		public int GetValueAsRegion()							{ return GetEnumOrInt<TriggerRegion>(value);							}
		public int GetValueAsColor()							{ return GetEnumOrInt<PlayerColor>(value);								}
		public AI.BotType GetValueAsBotType()					{ return GetEnum<AI.BotType>(value);									}*/

		public TriggerValueType GetSubjectType()
		{
			if ((int)type >= 2000 && (int)type < 3000) return TriggerValueType.MapID;
			else if ((int)type >= 3000 && (int)type < 4000) return TriggerValueType.UnitCategory;
			else if ((int)type >= 4000 && (int)type < 5000) return TriggerValueType.MapID;
			else if (type == TriggerActionType.ShowMessageToPlayerForUnit) return TriggerValueType.UnitCategory;
			else if (type == TriggerActionType.CreateMarker) return TriggerValueType.MarkerType;
			else if (type == TriggerActionType.CreateCargoTruck) return TriggerValueType.TruckCargo;
			return TriggerValueType.PlayerCategory;
		}

		public TriggerValueType GetSubject2Type()
		{
			return TriggerValueType.MapID;
		}

		public TriggerValueType GetValueType()
		{
			switch (type)
			{
				case TriggerActionType.MoveRegionToUnit:							return TriggerValueType.UnitCategory;
				case TriggerActionType.MoveRegionToRegion:							return TriggerValueType.Region;
				case TriggerActionType.MoveRegionToUnitTypeInRegion:				return TriggerValueType.MapID;
				case TriggerActionType.SetPlayerName:								return TriggerValueType.String;
				case TriggerActionType.SetPlayerColor:								return TriggerValueType.Color;
				case TriggerActionType.SetPlayerBotType:							return TriggerValueType.BotType;
				case TriggerActionType.SetPlayerAlliance:							return TriggerValueType.PlayerCategory;
				case TriggerActionType.CapturePlayerRLV:							return TriggerValueType.PlayerCategory;
				case TriggerActionType.SetPlayerCameraToRegion:						return TriggerValueType.Region;
				case TriggerActionType.ShowMessageToPlayerAtRegion:					return TriggerValueType.String;
				case TriggerActionType.ShowMessageToPlayerForUnit:					return TriggerValueType.String;
				case TriggerActionType.ShowMessageToPlayerFromPlayer:				return TriggerValueType.String;
				case TriggerActionType.SetPlayerMorale:								return TriggerValueType.MoraleLevel;
				case TriggerActionType.FireMissileEMP:								return TriggerValueType.Region;
				case TriggerActionType.SetMusic:									return TriggerValueType.String;

				case TriggerActionType.SetUnitsInRegionToAttackEnemyUnits:			return TriggerValueType.Region;
				case TriggerActionType.SetUnitsInRegionToAttackGround:				return TriggerValueType.Region;
				case TriggerActionType.DeployMinesInRegion:							return TriggerValueType.Region;
				case TriggerActionType.SetUnitsInRegionToBulldoze:					return TriggerValueType.Region;
				case TriggerActionType.SetUnitsInRegionToDock:						return TriggerValueType.Region;
				case TriggerActionType.SetUnitsInRegionToStandGround:				return TriggerValueType.Region;
				case TriggerActionType.SetEarthworkersInRegionToBuildWallOrTube:	return TriggerValueType.MapID;
				case TriggerActionType.SetEarthworkersInRegionToRemoveWallOrTube:	return TriggerValueType.Region;
				case TriggerActionType.SetFactoriesInRegionToProduceUnit:			return TriggerValueType.MapID;
				case TriggerActionType.SetUnitsInRegionToRepair:					return TriggerValueType.Region;
				case TriggerActionType.SetSpidersInRegionToReprogram:				return TriggerValueType.Region;
				case TriggerActionType.SetConvecsInRegionToDismantle:				return TriggerValueType.Region;
				case TriggerActionType.SetTrucksInRegionToSalvage:					return TriggerValueType.Region;
				case TriggerActionType.SetUnitsInRegionToGuard:						return TriggerValueType.Region;
				case TriggerActionType.TransferUnitsInRegionToPlayer:				return TriggerValueType.PlayerCategory;
				case TriggerActionType.SetWeaponForUnitsInRegion:					return TriggerValueType.MapID;
				case TriggerActionType.SetUnitsInRegionToMove:						return TriggerValueType.Region;
				case TriggerActionType.TeleportUnitsInRegion:						return TriggerValueType.Region;
				case TriggerActionType.SetConvecsInRegionToBuild:					return TriggerValueType.Region;
				case TriggerActionType.SetCargoForConvecsInRegion:					return TriggerValueType.MapID;
				case TriggerActionType.SetFactoriesInRegionToDevelop:				return TriggerValueType.MapID;

				case TriggerActionType.SetUnitToAttackEnemyUnits:					return TriggerValueType.Region;
				case TriggerActionType.SetUnitToAttackGround:						return TriggerValueType.Region;
				case TriggerActionType.DeployMine:									return TriggerValueType.Region;
				case TriggerActionType.SetUnitToBulldoze:							return TriggerValueType.Region;
				case TriggerActionType.SetUnitToDock:								return TriggerValueType.Region;
				case TriggerActionType.SetUnitToStandGround:						return TriggerValueType.Region;
				case TriggerActionType.SetEarthworkerToBuildWallOrTube:				return TriggerValueType.MapID;
				case TriggerActionType.SetEarthworkerToRemoveWallOrTube:			return TriggerValueType.Region;
				case TriggerActionType.SetFactoryToProduceUnit:						return TriggerValueType.MapID;
				case TriggerActionType.SetUnitToRepair:								return TriggerValueType.Region;
				case TriggerActionType.SetSpiderToReprogram:						return TriggerValueType.Region;
				case TriggerActionType.SetConvecToDismantle:						return TriggerValueType.Region;
				case TriggerActionType.SetTruckToSalvage:							return TriggerValueType.Region;
				case TriggerActionType.SetUnitToGuard:								return TriggerValueType.Region;
				case TriggerActionType.TransferUnitToPlayer:						return TriggerValueType.PlayerCategory;
				case TriggerActionType.SetWeaponForUnit:							return TriggerValueType.MapID;
				case TriggerActionType.SetUnitToMove:								return TriggerValueType.Region;
				case TriggerActionType.TeleportUnit:								return TriggerValueType.Region;
				case TriggerActionType.SetConvecToBuild:							return TriggerValueType.Region;
				case TriggerActionType.SetCargoForConvec:							return TriggerValueType.MapID;
				case TriggerActionType.SetFactoryToDevelop:							return TriggerValueType.MapID;

				case TriggerActionType.CreateCargoTruck:                            return TriggerValueType.Direction;
				case TriggerActionType.CreateConvec:                                return TriggerValueType.Direction;
				case TriggerActionType.CreateVehicle:                               return TriggerValueType.Direction;
				case TriggerActionType.CreateMine:                                  return TriggerValueType.Yield;
				case TriggerActionType.CreateBeacon:                                return TriggerValueType.Yield;
			}
			
			return TriggerValueType.Integer;
		}

		public TriggerValueType GetValue2Type()
		{
			switch (type)
			{
				case TriggerActionType.MoveRegionToUnitTypeInRegion:				return TriggerValueType.MapID;
				case TriggerActionType.CreateStorm:									return TriggerValueType.Region;
				case TriggerActionType.CreateVortex:								return TriggerValueType.Region;
				case TriggerActionType.CreateVolcano:								return TriggerValueType.Region;

				case TriggerActionType.SetFactoriesInRegionToProduceUnit:			return TriggerValueType.MapID;
				case TriggerActionType.SetTrucksInRegionToSalvage:					return TriggerValueType.Region;
				case TriggerActionType.SetCargoForConvecsInRegion:					return TriggerValueType.MapID;
				case TriggerActionType.SetCargoForTrucksInRegion:					return TriggerValueType.TruckCargo;
				case TriggerActionType.SetStructuresInRegionBayCargo:				return TriggerValueType.MapID;

				case TriggerActionType.SetFactoryToProduceUnit:						return TriggerValueType.MapID;
				case TriggerActionType.SetTruckToSalvage:							return TriggerValueType.Region;
				case TriggerActionType.SetCargoForConvec:							return TriggerValueType.MapID;
				case TriggerActionType.SetCargoForTruck:							return TriggerValueType.TruckCargo;
				case TriggerActionType.SetStructureBayCargo:						return TriggerValueType.MapID;

				case TriggerActionType.CreateMine:                                  return TriggerValueType.Variant;
				case TriggerActionType.CreateBeacon:                                return TriggerValueType.Variant;
			}
			
			return TriggerValueType.Integer;
		}

		public TriggerValueType GetValue3Type()
		{
			switch (type)
			{
				case TriggerActionType.MoveRegionToUnitTypeInRegion:				return TriggerValueType.PlayerCategory;

				case TriggerActionType.ShowMessageToPlayerAtRegion:					return TriggerValueType.Region;
				case TriggerActionType.ShowMessageToPlayerForUnit:					return TriggerValueType.UnitCategory;
				case TriggerActionType.ShowMessageToPlayerFromPlayer:				return TriggerValueType.PlayerCategory;

				case TriggerActionType.SetStructuresInRegionBayCargo:				return TriggerValueType.MapID;

				case TriggerActionType.SetStructureBayCargo:						return TriggerValueType.MapID;

				case TriggerActionType.CreateBeacon:                                return TriggerValueType.OreType;
			}
			
			return TriggerValueType.Integer;
		}

		public TriggerValueType GetValue4Type()
		{
			switch (type)
			{
				case TriggerActionType.MoveRegionToUnitTypeInRegion:				return TriggerValueType.Region;
			}

			return TriggerValueType.Integer;
		}

		public TriggerValueType GetValue5Type()
		{
			switch (type)
			{
				case TriggerActionType.CreateConvec:                                return TriggerValueType.MapID;
				case TriggerActionType.CreateVehicle:                               return TriggerValueType.MapID;
				case TriggerActionType.CreateStructure:								return TriggerValueType.MapID;
			}

			return TriggerValueType.Integer;
		}


		public TriggerActionData()
		{
		}

		public TriggerActionData(TriggerActionData clone)
		{
			m_Type = clone.m_Type;
			m_Modifier = clone.m_Modifier;
			subject = clone.subject;
			subjectPlayer = clone.subjectPlayer;
			subjectRegion = clone.subjectRegion;
			subjectQuantity = clone.subjectQuantity;

			value = clone.value;
			value2 = clone.value2;
			value3 = clone.value3;
			value4 = clone.value4;
			value5 = clone.value5;
		}

		private int GetEnumOrInt<T>(string val) where T : struct
		{
			int result;
			if (int.TryParse(val, out result))
				return result;

			return System.Convert.ToInt32(GetEnum<T>(val));
		}

		private T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
