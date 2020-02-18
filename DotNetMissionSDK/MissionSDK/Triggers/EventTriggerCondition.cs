using DotNetMissionSDK.Json;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System.Collections.Generic;

namespace DotNetMissionSDK.Triggers
{
	/// <summary>
	/// Caches values from TriggerConditionData to avoid string parsing.
	/// Checks game data against the condition and returns true/false.
	/// </summary>
	public class EventTriggerCondition
	{
		private EventSystemData m_EventData;
		private int m_OwnerID;

		private TriggerConditionType type					{ get; set; }
		private int subject									{ get; set; } // TriggerPlayerCategory, TriggerUnitCategory, Switch#
		private TriggerCompare comparison					{ get; set; }

		private int value									{ get; set; } // Quantity, Value, TopicID, Colony, Difficulty, Morale, Region, etc.
		private int value2									{ get; set; } // Secondary value used by some conditions
		private int value3									{ get; set; } // Secondary value used by some conditions

		private TriggerValueType subjectType				{ get; set; }

		//private TriggerValueType valueType					{ get; set; }
		//private TriggerValueType value2Type					{ get; set; }
		//private TriggerValueType value3Type					{ get; set; }


		public EventTriggerCondition(EventSystemData eventData, TriggerConditionData conditionData, int ownerID)
		{
			m_EventData = eventData;
			m_OwnerID = ownerID;

			type = conditionData.type;
			subject = TriggerValueTypeUtility.GetStringAsInt(conditionData.subject, conditionData.GetSubjectType());
			comparison = conditionData.comparison;

			value = TriggerValueTypeUtility.GetStringAsInt(conditionData.value, conditionData.GetValueType());
			value2 = TriggerValueTypeUtility.GetStringAsInt(conditionData.value2, conditionData.GetValue2Type());
			value3 = TriggerValueTypeUtility.GetStringAsInt(conditionData.value3, conditionData.GetValue3Type());

			subjectType = conditionData.GetSubjectType();

			//valueType = conditionData.GetValueType();
			//value2Type = conditionData.GetValue2Type();
			//value3Type = conditionData.GetValue3Type();
		}

		
		public bool DidMeetCondition(StateSnapshot stateSnapshot, int currentMark, int currentTick, PlayerState eventPlayer, UnitState eventUnit, int eventRegionIndex, int eventTopic)
		{
			// Default subject condition types
			switch (type)
			{
				case TriggerConditionType.CurrentMark:					return IsTrue(currentMark, comparison, value);
				case TriggerConditionType.CurrentTick:					return IsTrue(currentTick, comparison, value);
				case TriggerConditionType.CurrentRegion:				return IsTrue(eventRegionIndex, comparison, value);
				case TriggerConditionType.CurrentTopic:					return IsTrue(eventTopic, comparison, value);
				case TriggerConditionType.SwitchState:
					if (subject >= 0 && subject < m_EventData.switches.Length)
						return IsTrue(m_EventData.switches[subject], comparison, value);

					return false;
			}

			switch (subjectType)
			{
				case TriggerValueType.PlayerCategory:					return CheckPlayerSubject(stateSnapshot, eventPlayer);
				case TriggerValueType.UnitCategory:						return CheckUnitSubject(stateSnapshot, eventUnit);
			}

			return false;
		}

		private bool CheckPlayerSubject(StateSnapshot stateSnapshot, PlayerState eventPlayer)
		{
			switch ((TriggerPlayerCategory)subject)
			{
				case TriggerPlayerCategory.TriggerOwner:	return IsPlayerConditionTrue(stateSnapshot.players[m_OwnerID]);
				case TriggerPlayerCategory.EventPlayer:		return IsPlayerConditionTrue(eventPlayer);
				case TriggerPlayerCategory.Any:
					foreach (PlayerState player in stateSnapshot.players)
					{
						if (IsPlayerConditionTrue(player))
							return true;
					}
					return false;

				case TriggerPlayerCategory.All:
					foreach (PlayerState player in stateSnapshot.players)
					{
						if (!IsPlayerConditionTrue(player))
							return false;
					}
					return true;

				case TriggerPlayerCategory.OwnerAllies:
					foreach (PlayerState player in stateSnapshot.players)
					{
						if (!stateSnapshot.players[m_OwnerID].allyPlayerIDs.Contains(player.playerID))
							continue;

						if (!IsPlayerConditionTrue(player))
							return false;
					}
					return true;

				case TriggerPlayerCategory.OwnerEnemies:
					foreach (PlayerState player in stateSnapshot.players)
					{
						if (!stateSnapshot.players[m_OwnerID].enemyPlayerIDs.Contains(player.playerID))
							continue;

						if (!IsPlayerConditionTrue(player))
							return false;
					}
					return true;

				case TriggerPlayerCategory.EventAllies:
					foreach (PlayerState player in stateSnapshot.players)
					{
						if (!eventPlayer.allyPlayerIDs.Contains(player.playerID))
							continue;

						if (!IsPlayerConditionTrue(player))
							return false;
					}
					return true;

				case TriggerPlayerCategory.EventEnemies:
					foreach (PlayerState player in stateSnapshot.players)
					{
						if (!eventPlayer.enemyPlayerIDs.Contains(player.playerID))
							continue;

						if (!IsPlayerConditionTrue(player))
							return false;
					}
					return true;
			}

			return IsPlayerConditionTrue(stateSnapshot.players[subject]);
		}

		private bool IsPlayerConditionTrue(PlayerState playerSubject)
		{
			switch (type)
			{
				case TriggerConditionType.PlayerEquals:						return IsTrue(playerSubject.playerID, comparison, value);
				case TriggerConditionType.PlayerDifficulty:					return IsTrue(playerSubject.difficulty, comparison, value);
				case TriggerConditionType.PlayerColony:						return IsTrue((int)(playerSubject.isEden ? TriggerColonyType.Eden : TriggerColonyType.Plymouth), comparison, value);
				case TriggerConditionType.PlayerKids:						return IsTrue(playerSubject.kids, comparison, value);
				case TriggerConditionType.PlayerWorkers:					return IsTrue(playerSubject.workers, comparison, value);
				case TriggerConditionType.PlayerScientists:					return IsTrue(playerSubject.scientists, comparison, value);
				case TriggerConditionType.PlayerPopulation:					return IsTrue(playerSubject.totalPopulation, comparison, value);
				case TriggerConditionType.PlayerCommonMetal:				return IsTrue(playerSubject.ore, comparison, value);
				case TriggerConditionType.PlayerRareMetal:					return IsTrue(playerSubject.rareOre, comparison, value);
				case TriggerConditionType.PlayerMaxCommonMetal:				return IsTrue(playerSubject.maxCommonOre, comparison, value);
				case TriggerConditionType.PlayerMaxRareMetal:				return IsTrue(playerSubject.maxRareOre, comparison, value);
				case TriggerConditionType.PlayerFood:						return IsTrue(playerSubject.foodStored, comparison, value);
				case TriggerConditionType.PlayerVehicleCount:				return IsTrue(new List<UnitState>(playerSubject.units.GetVehicles()).Count, comparison, value);
				case TriggerConditionType.PlayerUnitsInRegion:				return IsTrue(GetUnitsInRegion(GetUnitsInCategory(playerSubject, value2), value3).Count, comparison, value);
				case TriggerConditionType.PlayerConvecsWithKitInRegion:		return IsTrue(GetUnitsInRegion(GetConvecsWithKit(playerSubject, (map_id)value2), value3).Count, comparison, value);
				case TriggerConditionType.PlayerTrucksWithCargoInRegion:	return IsTrue(GetUnitsInRegion(GetTrucksWithCargo(playerSubject, (TruckCargo)value2), value3).Count, comparison, value);
				case TriggerConditionType.PlayerMorale:						return IsTrue((int)playerSubject.moraleLevel, comparison, value);
				case TriggerConditionType.PlayerResearch:					return IsTrue(playerSubject.HasTechnology(value), comparison, true);
				case TriggerConditionType.PlayerAvailableWorkers:			return IsTrue(playerSubject.numAvailableWorkers, comparison, value);
				case TriggerConditionType.PlayerAvailableScientists:		return IsTrue(playerSubject.numAvailableScientists, comparison, value);
				case TriggerConditionType.PlayerPowerGenerated:				return IsTrue(playerSubject.amountPowerGenerated, comparison, value);
				case TriggerConditionType.PlayerInactivePowerCapacity:		return IsTrue(playerSubject.inactivePowerCapacity, comparison, value);
				case TriggerConditionType.PlayerPowerConsumed:				return IsTrue(playerSubject.amountPowerConsumed, comparison, value);
				case TriggerConditionType.PlayerPowerAvailable:				return IsTrue(playerSubject.amountPowerAvailable, comparison, value);
				case TriggerConditionType.PlayerIdleStructureCount:			return IsTrue(playerSubject.numIdleBuildings, comparison, value);
				case TriggerConditionType.PlayerActiveStructureCount:		return IsTrue(playerSubject.numActiveBuildings, comparison, value);
				case TriggerConditionType.PlayerStructureCount:				return IsTrue(playerSubject.numBuildings, comparison, value);
				case TriggerConditionType.PlayerUnpoweredStructureCount:	return IsTrue(playerSubject.numUnpoweredStructures, comparison, value);
				case TriggerConditionType.PlayerWorkersRequired:			return IsTrue(playerSubject.numWorkersRequired, comparison, value);
				case TriggerConditionType.PlayerScientistsRequired:			return IsTrue(playerSubject.numScientistsRequired, comparison, value);
				case TriggerConditionType.PlayerScientistsAsWorkers:		return IsTrue(playerSubject.numScientistsAsWorkers, comparison, value);
				case TriggerConditionType.PlayerScientistsResearching:		return IsTrue(playerSubject.numScientistsAssignedToResearch, comparison, value);
				case TriggerConditionType.PlayerFoodProduced:				return IsTrue(playerSubject.totalFoodProduction, comparison, value);
				case TriggerConditionType.PlayerFoodConsumed:				return IsTrue(playerSubject.totalFoodConsumption, comparison, value);
				case TriggerConditionType.PlayerFoodLacking:				return IsTrue(playerSubject.foodLacking, comparison, value);
				case TriggerConditionType.PlayerNetFoodProduction:			return IsTrue(playerSubject.netFoodProduction, comparison, value);
				case TriggerConditionType.PlayerSolarSatelliteCount:		return IsTrue(playerSubject.starship.solarSatelliteCount, comparison, value);
				case TriggerConditionType.PlayerRecreationCapacity:			return IsTrue(playerSubject.totalRecreationFacilityCapacity, comparison, value);
				case TriggerConditionType.PlayerForumCapacity:				return IsTrue(playerSubject.totalForumCapacity, comparison, value);
				case TriggerConditionType.PlayerMedicalCenterCapacity:		return IsTrue(playerSubject.totalMedCenterCapacity, comparison, value);
				case TriggerConditionType.PlayerResidenceCapacity:			return IsTrue(playerSubject.totalResidenceCapacity, comparison, value);
				case TriggerConditionType.PlayerCanAccumulateOre:			return IsTrue(GameState.players[playerSubject.playerID].CanAccumulateOre(), comparison, true);
				case TriggerConditionType.PlayerCanAccumulateRareOre:		return IsTrue(GameState.players[playerSubject.playerID].CanAccumulateRareOre(), comparison, true);
				case TriggerConditionType.PlayerCanBuildSpaceport:			return IsTrue(GameState.players[playerSubject.playerID].CanBuildSpace(), comparison, true);
				case TriggerConditionType.PlayerCanBuildStructure:			return IsTrue(GameState.players[playerSubject.playerID].CanBuildBuilding(), comparison, true);
				case TriggerConditionType.PlayerCanBuildVehicle:			return IsTrue(GameState.players[playerSubject.playerID].CanBuildVehicle(true), comparison, true);
				case TriggerConditionType.PlayerCanResearchTopic:			return IsTrue(GameState.players[playerSubject.playerID].CanDoResearch(value), comparison, true);
				case TriggerConditionType.PlayerHasVehicle:					return IsTrue(GameState.players[playerSubject.playerID].HasVehicle((map_id)value, (map_id)value2), comparison, true);
				case TriggerConditionType.PlayerHasActiveCommandCenter:		return IsTrue(GameState.players[playerSubject.playerID].HasActiveCommand(), comparison, true);
				case TriggerConditionType.PlayerStarshipModuleDeployed:		return IsTrue(playerSubject.starship.GetCountByID((map_id)value) > 0, comparison, true);
				case TriggerConditionType.PlayerStarshipProgress:			return IsTrue(playerSubject.starship.progress, comparison, value);
			}

			return true;
		}

		private bool CheckUnitSubject(StateSnapshot stateSnapshot, UnitState eventUnit)
		{
			switch ((TriggerUnitCategory)subject)
			{
				case TriggerUnitCategory.EventUnit:		return IsUnitConditionTrue(eventUnit);
			}

			if (subject < 0 || subject >= m_EventData.unitIDs.Length)
				return false;

			// Get unit by ID
			int stubIndex = m_EventData.unitIDs[subject];
			UnitState unit = stateSnapshot.GetUnit(stubIndex);
			
			if (unit == null) return false;
			if (!unit.isLive) return false;

			return IsUnitConditionTrue(unit);
		}

		private bool IsUnitConditionTrue(UnitState unitSubject)
		{
			switch (type)
			{
				case TriggerConditionType.UnitID:						return IsTrue(unitSubject.unitID, comparison, value);
				case TriggerConditionType.UnitOwnerID:					return IsTrue(unitSubject.ownerID, comparison, value);
				case TriggerConditionType.UnitType:						return IsTrue((int)unitSubject.unitType, comparison, value);
				case TriggerConditionType.UnitBusy:						return IsTrue(unitSubject.isBusy, comparison, true);
				case TriggerConditionType.UnitEMPed:					return IsTrue(unitSubject.isEMPed, comparison, true);
				case TriggerConditionType.StructureEnabled:				if (unitSubject is StructureState) return IsTrue(((StructureState)unitSubject).isEnabled, comparison, true); return false;
				case TriggerConditionType.StructureDisabled:			if (unitSubject is StructureState) return IsTrue(((StructureState)unitSubject).isDisabled, comparison, true); return false;
				case TriggerConditionType.UnitPositionX:				return IsTrue(unitSubject.position.x, comparison, value);
				case TriggerConditionType.UnitPositionY:				return IsTrue(unitSubject.position.y, comparison, value);
				case TriggerConditionType.UnitHealth:					return IsTrue(1 - (unitSubject.damage / (float)GameState.GetUnit(unitSubject.unitID).GetUnitInfo().GetHitPoints(unitSubject.ownerID)), comparison, value);
				case TriggerConditionType.ConvecKitType:				if (unitSubject is ConvecState) return IsTrue((int)((ConvecState)unitSubject).cargoType, comparison, value); return false;
				case TriggerConditionType.TruckCargoType:				if (unitSubject is CargoTruckState) return IsTrue((int)((CargoTruckState)unitSubject).cargoType, comparison, value); return false;
				case TriggerConditionType.TruckCargoQuantity:			if (unitSubject is CargoTruckState) return IsTrue((int)((CargoTruckState)unitSubject).cargoAmount, comparison, value); return false;
				case TriggerConditionType.UnitWeapon:					return IsTrue((int)unitSubject.weapon, comparison, value);
				case TriggerConditionType.UnitLastCommand:				return IsTrue((int)unitSubject.lastCommand, comparison, value);
				case TriggerConditionType.UnitCurrentAction:			return IsTrue((int)unitSubject.curAction, comparison, value);
				case TriggerConditionType.VehicleStickyfoamed:			if (unitSubject is VehicleState) return IsTrue(((VehicleState)unitSubject).isStickyfoamed, comparison, true); return false;
				case TriggerConditionType.VehicleESGed:					if (unitSubject is VehicleState) return IsTrue(((VehicleState)unitSubject).isESGed, comparison, true); return false;
				case TriggerConditionType.UniversityWorkersInTraining:	if (unitSubject is UniversityState) return IsTrue(((UniversityState)unitSubject).workersInTraining, comparison, value); return false;
				case TriggerConditionType.SpaceportLaunchpadCargo:		if (unitSubject is SpaceportState) return IsTrue((int)((SpaceportState)unitSubject).launchpadCargo, comparison, value); return false;
				case TriggerConditionType.UnitLights:					return IsTrue(unitSubject.areLightsOn, comparison, true);
				case TriggerConditionType.StructurePower:				if (unitSubject is StructureState) return IsTrue(((StructureState)unitSubject).hasPower, comparison, true); return false;
				case TriggerConditionType.StructureWorkers:				if (unitSubject is StructureState) return IsTrue(((StructureState)unitSubject).hasWorkers, comparison, true); return false;
				case TriggerConditionType.StructureScientists:			if (unitSubject is StructureState) return IsTrue(((StructureState)unitSubject).hasScientists, comparison, true); return false;
				case TriggerConditionType.StructureInfected:			if (unitSubject is StructureState) return IsTrue(((StructureState)unitSubject).isInfected, comparison, true); return false;
				case TriggerConditionType.BeaconTruckLoadsSoFar:			// [UnitID] beacon truck loads so far [comparison] [quantity]
				case TriggerConditionType.BeaconSurveyedByPlayer:			// [UnitID] beacon surveyed by [CurrentPlayer/Player#]
				case TriggerConditionType.LabResearchTopic:				if (unitSubject is LabState) return IsTrue(((LabState)unitSubject).labCurrentTopic, comparison, value); return false;
				case TriggerConditionType.LabScientistCount:			if (unitSubject is LabState) return IsTrue(((LabState)unitSubject).labScientistCount, comparison, value); return false;
				case TriggerConditionType.UnitHasWeapon:				return IsTrue(unitSubject.hasWeapon, comparison, true);
				case TriggerConditionType.StructureHasEmptyBay:
					if (unitSubject is FactoryState)
						return IsTrue(((FactoryState)unitSubject).HasEmptyBay(), comparison, true);
					else if (unitSubject is GarageState)
						return IsTrue(((GarageState)unitSubject).hasOccupiedBay == false, comparison, true);
					return false;
				case TriggerConditionType.StructureHasBayWithCargo:     if (unitSubject is FactoryState) return IsTrue(((FactoryState)unitSubject).HasBayWithCargo((map_id)value), comparison, true); return false;
				case TriggerConditionType.UnitInRegion:					return IsTrue(IsUnitInRegion(unitSubject, value), comparison, true);
			}

			return true;
		}

		private bool IsTrue(int value1, TriggerCompare compare, int value2)
		{
			switch (compare)
			{
				case TriggerCompare.Equals:					return value1 == value2;
				case TriggerCompare.NotEquals:				return value1 != value2;
				case TriggerCompare.GreaterThan:			return value1 > value2;
				case TriggerCompare.GreaterThanOrEqual:		return value1 >= value2;
				case TriggerCompare.LessThan:				return value1 < value2;
				case TriggerCompare.LessThanOrEqual:		return value1 <= value2;
			}

			return false;
		}

		private bool IsTrue(float value1, TriggerCompare compare, float value2)
		{
			switch (compare)
			{
				case TriggerCompare.Equals:					return value1 == value2;
				case TriggerCompare.NotEquals:				return value1 != value2;
				case TriggerCompare.GreaterThan:			return value1 > value2;
				case TriggerCompare.GreaterThanOrEqual:		return value1 >= value2;
				case TriggerCompare.LessThan:				return value1 < value2;
				case TriggerCompare.LessThanOrEqual:		return value1 <= value2;
			}

			return false;
		}

		private bool IsTrue(bool value1, TriggerCompare compare, bool value2)
		{
			switch (compare)
			{
				case TriggerCompare.Equals:					return value1 == value2;
				case TriggerCompare.NotEquals:				return value1 != value2;
			}

			return false;
		}

		private List<UnitState> GetUnitsInCategory(PlayerState playerSubject, int category)
		{
			if ((map_id)category == map_id.Any)
				return new List<UnitState>(playerSubject.units.GetUnits());

			return new List<UnitState>(playerSubject.units.GetListForType((map_id)category));
		}

		private List<UnitState> GetConvecsWithKit(PlayerState playerSubject, map_id kitType)
		{
			List<UnitState> unitsWithKit = new List<UnitState>();

			// Check convecs for kit
			foreach (ConvecState convec in playerSubject.units.convecs)
			{
				if (convec.cargoType == kitType)
					unitsWithKit.Add(convec);
				else if (kitType == map_id.Any && convec.cargoType != map_id.None)
					unitsWithKit.Add(convec);
			}

			return unitsWithKit;
		}

		private List<UnitState> GetFactoriesWithKit(PlayerState playerSubject, map_id kitType)
		{
			List<UnitState> unitsWithKit = new List<UnitState>();

			// Check factories for kit
			foreach (FactoryState factory in playerSubject.units.structureFactories)
			{
				if (factory.HasBayWithCargo(kitType))
					unitsWithKit.Add(factory);
				else if (kitType == map_id.Any && factory.hasOccupiedBay)
					unitsWithKit.Add(factory);
			}

			return unitsWithKit;
		}

		private List<UnitState> GetTrucksWithCargo(PlayerState playerSubject, TruckCargo cargoType)
		{
			List<UnitState> unitsWithKit = new List<UnitState>();

			// Check trucks for cargo
			foreach (CargoTruckState truck in playerSubject.units.cargoTrucks)
			{
				if (truck.cargoType == cargoType)
					unitsWithKit.Add(truck);
			}

			return unitsWithKit;
		}

		private List<UnitState> GetUnitsInRegion(List<UnitState> units, int regionID)
		{
			if (regionID == (int)TriggerRegion.Anywhere)
				return units;

			MAP_RECT area = GetMapRect(m_EventData.regions[regionID]);

			for (int i=0; i< units.Count; ++i)
			{
				UnitState unit = units[i];

				//unit.position.ClipToMap();

				if (!area.Contains(unit.position))
					units.RemoveAt(i--);
			}

			return units;
		}

		private bool IsUnitInRegion(UnitState unit, int regionID)
		{
			if (regionID == (int)TriggerRegion.Anywhere)
				return true;

			MAP_RECT area = GetMapRect(m_EventData.regions[regionID]);

			return area.Contains(unit.position);
		}

		private MAP_RECT GetMapRect(EventRegionData eventRegion)
		{
			return MAP_RECT.FromMinMax(eventRegion.xMin, eventRegion.yMin, eventRegion.xMax, eventRegion.yMax);
		}
	}
}
