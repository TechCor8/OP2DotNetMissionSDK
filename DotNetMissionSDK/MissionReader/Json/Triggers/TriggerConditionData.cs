using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	public enum TriggerConditionType
	{
		CurrentMark,					// [CurrentMark] [comparison] [quantity]
		CurrentTick,					// [CurrentTick] [comparison] [quantity]
		CurrentRegion,					// [CurrentRegion] [comparison] [Name]
		CurrentTopic,					// [CurrentTopic] [comparison] [TopicID]
		SwitchState,					// [Switch#] is [comparison] [value]

		PlayerEquals=1000,				// [CurrentPlayer] is [Player#]
		PlayerDifficulty,				// [CurrentPlayer/Player#] is [difficulty]
		PlayerColony,					// [CurrentPlayer/Player#] is [Eden/Plymouth]
		PlayerKids,						// [Any/All/CurrentPlayer/Player#] kids [comparison] [quantity]
		PlayerWorkers,					// [Any/All/CurrentPlayer/Player#] workers [comparison] [quantity]
		PlayerScientists,				// [Any/All/CurrentPlayer/Player#] scientists [comparison] [quantity]
		PlayerPopulation,				// [Any/All/CurrentPlayer/Player#] population [comparison] [quantity]
		PlayerCommonMetal,				// [Any/All/CurrentPlayer/Player#] common metal [comparison] [quantity]
		PlayerRareMetal,				// [Any/All/CurrentPlayer/Player#] rare metal [comparison] [quantity]
		PlayerMaxCommonMetal,			// [Any/All/CurrentPlayer/Player#] max common metal [comparison] [quantity]
		PlayerMaxRareMetal,				// [Any/All/CurrentPlayer/Player#] max rare metal [comparison] [quantity]
		PlayerFood,						// [Any/All/CurrentPlayer/Player#] food [comparison] [quantity]
		PlayerVehicleCount,				// [Any/All/CurrentPlayer/Player#] vehicle count [comparison] [quantity]
		PlayerUnitsInRegion,			// [Any/All/CurrentPlayer/Player#] has [comparison] [quantity] [Any/Category/UnitType] in region [Anywhere/name]
		PlayerConvecsWithKitInRegion,	// [Any/All/CurrentPlayer/Player#] has [comparison] [quantity] convecs with a [UnitType] kit in region [Anywhere/name]
		PlayerTrucksWithCargoInRegion,	// [Any/All/CurrentPlayer/Player#] has [comparison] [quantity] trucks with [TruckCargo] cargo in region [Anywhere/name]
		PlayerMorale,					// [Any/All/CurrentPlayer/Player#] morale is [comparison] [Terrible/Poor/Fair/Good/Excellent]
		PlayerResearch,					// [Any/All/CurrentPlayer/Player#] [has/does not have] technology [TopicID]
		PlayerAvailableWorkers,			// [Any/All/CurrentPlayer/Player#] available workers [comparison] [quantity]
		PlayerAvailableScientists,		// [Any/All/CurrentPlayer/Player#] available scientists [comparison] [quantity]
		PlayerPowerGenerated,			// [Any/All/CurrentPlayer/Player#] power generated [comparison] [quantity]
		PlayerInactivePowerCapacity,	// [Any/All/CurrentPlayer/Player#] inactive power capacity [comparison] [quantity]
		PlayerPowerConsumed,			// [Any/All/CurrentPlayer/Player#] power consumed [comparison] [quantity]
		PlayerPowerAvailable,			// [Any/All/CurrentPlayer/Player#] power available [comparison] [quantity]
		PlayerIdleStructureCount,		// [Any/All/CurrentPlayer/Player#] idle structure count [comparison] [quantity]
		PlayerActiveStructureCount,		// [Any/All/CurrentPlayer/Player#] active structure count [comparison] [quantity]
		PlayerStructureCount,			// [Any/All/CurrentPlayer/Player#] structure count [comparison] [quantity]
		PlayerUnpoweredStructureCount,	// [Any/All/CurrentPlayer/Player#] unpowered structure count [comparison] [quantity]
		PlayerWorkersRequired,			// [Any/All/CurrentPlayer/Player#] workers required [comparison] [quantity]
		PlayerScientistsRequired,		// [Any/All/CurrentPlayer/Player#] scientists required [comparison] [quantity]
		PlayerScientistsAsWorkers,		// [Any/All/CurrentPlayer/Player#] scientists as workers [comparison] [quantity]
		PlayerScientistsResearching,	// [Any/All/CurrentPlayer/Player#] scientists researching [comparison] [quantity]
		PlayerFoodProduced,				// [Any/All/CurrentPlayer/Player#] food produced [comparison] [quantity]
		PlayerFoodConsumed,				// [Any/All/CurrentPlayer/Player#] food consumed [comparison] [quantity]
		PlayerFoodLacking,				// [Any/All/CurrentPlayer/Player#] food lacking [comparison] [quantity]
		PlayerNetFoodProduction,		// [Any/All/CurrentPlayer/Player#] net food produced [comparison] [quantity]
		PlayerSolarSatelliteCount,		// [Any/All/CurrentPlayer/Player#] solar satellite count [comparison] [quantity]
		PlayerRecreationCapacity,		// [Any/All/CurrentPlayer/Player#] recreation capacity [comparison] [quantity]
		PlayerForumCapacity,			// [Any/All/CurrentPlayer/Player#] forum capacity [comparison] [quantity]
		PlayerMedicalCenterCapacity,	// [Any/All/CurrentPlayer/Player#] medical center capacity [comparison] [quantity]
		PlayerResidenceCapacity,		// [Any/All/CurrentPlayer/Player#] residence capacity [comparison] [quantity]
		PlayerCanAccumulateOre,			// [Any/All/CurrentPlayer/Player#] [can/can not] accumulate ore
		PlayerCanAccumulateRareOre,		// [Any/All/CurrentPlayer/Player#] [can/can not] accumulate rare ore
		PlayerCanBuildSpaceport,		// [Any/All/CurrentPlayer/Player#] [can/can not] build spaceport
		PlayerCanBuildStructure,		// [Any/All/CurrentPlayer/Player#] [can/can not] build structure
		PlayerCanBuildVehicle,			// [Any/All/CurrentPlayer/Player#] [can/can not] build vehicle
		PlayerCanResearchTopic,			// [Any/All/CurrentPlayer/Player#] [can/can not] research [TopicID]
		PlayerHasVehicle,				// [Any/All/CurrentPlayer/Player#] [has/does not have] vehicle [map_id] with cargo or weapon [map_id]
		PlayerHasActiveCommandCenter,	// [Any/All/CurrentPlayer/Player#] [has/does not have] active command center
		PlayerStarshipModuleDeployed,	// [Any/All/CurrentPlayer/Player#] [has/does not have] [starship.map_id]
		PlayerStarshipProgress,			// [Any/All/CurrentPlayer/Player#] starship progress [comparison] [quantity]
		
		UnitID=2000,					// [CurrentUnit] is [UnitID]
		UnitOwnerID,					// [CurrentUnit/UnitID] is owned by [Player#]
		UnitType,						// [CurrentUnit/UnitID] [is/is not] [UnitType]
		UnitBusy,						// [CurrentUnit/UnitID] [is/is not] busy
		UnitEMPed,						// [CurrentUnit/UnitID] [is/is not] EMPed
		StructureEnabled,				// [CurrentUnit/UnitID] [is/is not] enabled
		StructureDisabled,				// [CurrentUnit/UnitID] [is/is not] disabled
		UnitPositionX,					// [CurrentUnit/UnitID] position X [comparison] [value]
		UnitPositionY,					// [CurrentUnit/UnitID] position Y [comparison] [value]
		UnitHealth,						// [CurrentUnit/UnitID] health [comparison] [value]
		ConvecKitType,					// [CurrentUnit/UnitID] structure kit [comparison] [map_id]
		TruckCargoType,					// [CurrentUnit/UnitID] cargo [comparison] [TruckCargo]
		TruckCargoQuantity,				// [CurrentUnit/UnitID] cargo [comparison] [quantity]
		UnitWeapon,						// [CurrentUnit/UnitID] weapon [comparison] [map_id]
		UnitLastCommand,				// [CurrentUnit/UnitID] last command [comparison] [CommandType]
		UnitCurrentAction,				// [CurrentUnit/UnitID] current action [comparison] [ActionType]
		VehicleStickyfoamed,			// [CurrentUnit/UnitID] [comparison] stickyfoamed
		VehicleESGed,					// [CurrentUnit/UnitID] [comparison] ESGed
		UniversityWorkersInTraining,	// [CurrentUnit/UnitID] university workers in training [comparison] [quantity]
		SpaceportLaunchpadCargo,		// [CurrentUnit/UnitID] spaceport launchpad cargo [comparison] [map_id]
		UnitLights,						// [CurrentUnit/UnitID] lights [on/off]
		StructurePower,					// [CurrentUnit/UnitID] [has/does not have] power
		StructureWorkers,				// [CurrentUnit/UnitID] [has/does not have] workers
		StructureScientists,			// [CurrentUnit/UnitID] [has/does not have] scientists
		StructureInfected,				// [CurrentUnit/UnitID] [is/is not] infected
		LabResearchTopic,				// [CurrentUnit/UnitID] lab [is/is not] researching [TopicID]
		LabScientistCount,				// [CurrentUnit/UnitID] lab scientist count [comparison] [quantity]
		UnitHasWeapon,					// [CurrentUnit/UnitID] [has/does not have] weapon
		StructureHasEmptyBay,			// [CurrentUnit/UnitID] [has/does not have] empty bay
		StructureHasBayWithCargo,		// [CurrentUnit/UnitID] [has/does not have] bay with cargo [map_id]
		UnitInRegion,					// [CurrentUnit/UnitID] is in [region]

		BeaconTruckLoadsSoFar=3000,		// [UnitID] beacon truck loads so far [comparison] [quantity]
		BeaconSurveyedByPlayer,			// [UnitID] beacon surveyed by [CurrentPlayer/Player#]
	}

	[DataContract]
	public class TriggerConditionData
	{
		[DataMember(Name = "Type")]					private string m_Type				{ get; set; } = string.Empty;
		[DataMember(Name = "Subject")]				public string subject				{ get; set; } = string.Empty; // TriggerPlayerCategory, TriggerUnitCategory, Switch#
		[DataMember(Name = "Comparison")]			private string m_Comparison			{ get; set; } = string.Empty;

		[DataMember(Name = "Value")]				public string value					{ get; set; } = string.Empty; // Quantity, Value, TopicID, Colony, Difficulty, Morale, Region, etc.
		[DataMember(Name = "Value2")]				public string value2				{ get; set; } = string.Empty; // Secondary value used by some conditions
		[DataMember(Name = "Value3")]				public string value3				{ get; set; } = string.Empty; // Secondary value used by some conditions


		public TriggerConditionType type						{ get { return GetEnum<TriggerConditionType>(m_Type);		} set { m_Type = value.ToString();				} }
		public TriggerCompare comparison						{ get { return GetEnum<TriggerCompare>(m_Comparison);		} set { m_Comparison = value.ToString();		} }


		// Integer subject and values
		/*public int GetIntegerSubject()							{ int result; int.TryParse(subject, out result);	return result;		}
		public int GetIntegerValue()							{ int result; int.TryParse(value, out result);		return result;		}
		public int GetIntegerValue2()							{ int result; int.TryParse(value2, out result);		return result;		}
		public int GetIntegerValue3()							{ int result; int.TryParse(value3, out result);		return result;		}*/

		// Parse subject as enum and return as integer
		/*public int GetSubjectAsPlayerCategory()					{ return GetEnumOrInt<TriggerPlayerCategory>(subject);					}
		public int GetSubjectAsUnitCategory()					{ return GetEnumOrInt<TriggerUnitCategory>(subject);					}

		// Parse value as enum
		public TriggerPlayerCategory GetValueAsPlayerCategory()	{ return GetEnum<TriggerPlayerCategory>(value);							}
		public PlayerDifficulty GetValueAsPlayerDifficulty()	{ return GetEnum<PlayerDifficulty>(value);								}
		public TriggerColonyType GetValueAsColonyType()			{ return GetEnum<TriggerColonyType>(value);								}
		public MoraleLevel GetValueAsMorale()					{ return GetEnum<MoraleLevel>(value);									}
		public TruckCargo GetValueAsTruckCargo()				{ return GetEnum<TruckCargo>(value);									}
		public CommandType GetValueAsCommandType()				{ return GetEnum<CommandType>(value);									}
		public ActionType GetValueAsActionType()				{ return GetEnum<ActionType>(value);									}
		public map_id GetValueAsMapID()							{ return GetEnum<map_id>(value);										}
		public int GetValueAsRegion()							{ return GetEnumOrInt<TriggerRegion>(value);							}*/

		// Parse value2 as enum
		/*public map_id GetValue2AsMapID()						{ return GetEnum<map_id>(value2);										}

		// Parse value 3 as enum
		public int GetValue3AsRegion()							{ return GetEnumOrInt<TriggerRegion>(value3);							}*/


		public TriggerValueType GetSubjectType()
		{
			if ((int)type >= 1000 && (int)type < 2000) return TriggerValueType.PlayerCategory;
			if ((int)type >= 2000 && (int)type < 3000) return TriggerValueType.UnitCategory;
			return TriggerValueType.Integer;
		}

		/*public int GetSubjectAsInt()
		{
			switch (GetSubjectType())
			{
				case TriggerValueType.PlayerCategory:	return GetSubjectAsPlayerCategory();
				case TriggerValueType.UnitCategory:	return GetSubjectAsUnitCategory();
			}

			return GetIntegerSubject();
		}*/

		public TriggerValueType GetValueType()
		{
			switch (type)
			{
				case TriggerConditionType.CurrentRegion:				return TriggerValueType.Region;
				case TriggerConditionType.PlayerDifficulty:				return TriggerValueType.Difficulty;
				case TriggerConditionType.PlayerColony:					return TriggerValueType.ColonyType;
				case TriggerConditionType.PlayerMorale:					return TriggerValueType.MoraleLevel;
				case TriggerConditionType.PlayerHasVehicle:				return TriggerValueType.MapID;
				case TriggerConditionType.PlayerStarshipModuleDeployed:	return TriggerValueType.MapID;
				case TriggerConditionType.UnitType:						return TriggerValueType.MapID;
				case TriggerConditionType.ConvecKitType:				return TriggerValueType.MapID;
				case TriggerConditionType.TruckCargoType:				return TriggerValueType.TruckCargo;
				case TriggerConditionType.UnitWeapon:					return TriggerValueType.MapID;
				case TriggerConditionType.UnitLastCommand:				return TriggerValueType.Command;
				case TriggerConditionType.UnitCurrentAction:			return TriggerValueType.Action;
				case TriggerConditionType.SpaceportLaunchpadCargo:		return TriggerValueType.MapID;
				case TriggerConditionType.BeaconSurveyedByPlayer:		return TriggerValueType.PlayerCategory;
				case TriggerConditionType.StructureHasBayWithCargo:		return TriggerValueType.MapID;
				case TriggerConditionType.UnitInRegion:					return TriggerValueType.Region;
			}
			
			return TriggerValueType.Integer;
		}

		/*public int GetValueAsInt()
		{
			switch (GetValueType())
			{
				case TriggerValueType.Region:			return GetValueAsRegion();
				case TriggerValueType.Difficulty:		return (int)GetValueAsPlayerDifficulty();
				case TriggerValueType.ColonyType:		return (int)GetValueAsColonyType();
				case TriggerValueType.MoraleLevel:		return (int)GetValueAsMorale();
				case TriggerValueType.MapID:			return (int)GetValueAsMapID();
				case TriggerValueType.TruckCargo:		return (int)GetValueAsTruckCargo();
				case TriggerValueType.Command:			return (int)GetValueAsCommandType();
				case TriggerValueType.Action:			return (int)GetValueAsActionType();
				case TriggerValueType.PlayerCategory:	return (int)GetValueAsPlayerCategory();
			}

			return GetIntegerValue();
		}*/

		public TriggerValueType GetValue2Type()
		{
			switch (type)
			{
				case TriggerConditionType.PlayerHasVehicle:					return TriggerValueType.MapID;
				case TriggerConditionType.PlayerUnitsInRegion:				return TriggerValueType.MapID;
				case TriggerConditionType.PlayerConvecsWithKitInRegion:		return TriggerValueType.MapID;
				case TriggerConditionType.PlayerTrucksWithCargoInRegion:	return TriggerValueType.TruckCargo;
			}
			
			return TriggerValueType.Integer;
		}

		/*public int GetValue2AsInt()
		{
			switch (GetValueType())
			{
				case TriggerValueType.MapID:			return (int)GetValue2AsMapID();
			}

			return GetIntegerValue2();
		}*/

		public TriggerValueType GetValue3Type()
		{
			return TriggerValueType.Region;
		}

		/*public int GetValue3AsInt()
		{
			return GetValue3AsRegion();
		}*/


		public TriggerConditionData()
		{
		}

		public TriggerConditionData(TriggerConditionData clone)
		{
			m_Type = clone.m_Type;
			subject = clone.subject;
			m_Comparison = clone.m_Comparison;

			value = clone.value;
			value2 = clone.value2;
			value3 = clone.value3;
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
