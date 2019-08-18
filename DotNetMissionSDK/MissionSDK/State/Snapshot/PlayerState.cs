using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Snapshot.Maps;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetMissionSDK.State.Snapshot
{
	/// <summary>
	/// Contains immutable player state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class PlayerState
	{
		public int playerID								{ get; private set; }

		public ReadOnlyCollection<int> allyPlayerIDs	{ get; private set; }
		public ReadOnlyCollection<int> enemyPlayerIDs	{ get; private set; }

		// Player
		public int difficulty							{ get; private set; }
		public bool isEden								{ get; private set; }
		public bool isHuman								{ get; private set; }
		
		// [Get] Population
		public int kids									{ get; private set; }
		public int workers								{ get; private set; }
		public int scientists							{ get; private set; }
		public int totalPopulation						{ get; private set; }
		
		// [Get] Resources
		public int ore									{ get; private set; }
		public int rareOre								{ get; private set; }
		public int foodStored							{ get; private set; }
		public FoodStatus foodSupply					{ get; private set; }
		
		public MoraleLevel moraleLevel					{ get; private set; }
		public int rlvCount								{ get; private set; }

		// PlayerEx
		public int maxCommonOre							{ get; private set; }
		public int maxRareOre							{ get; private set; }

		public int numAvailableWorkers					{ get; private set; }
		public int numAvailableScientists				{ get; private set; }
		public int amountPowerGenerated					{ get; private set; }
		public int inactivePowerCapacity				{ get; private set; }
		public int amountPowerConsumed					{ get; private set; }
		public int amountPowerAvailable					{ get; private set; }
		public int numIdleBuildings						{ get; private set; }
		public int numActiveBuildings					{ get; private set; }
		public int numBuildings							{ get; private set; }
		public int numUnpoweredStructures				{ get; private set; }
		public int numWorkersRequired					{ get; private set; }
		public int numScientistsRequired				{ get; private set; }
		public int numScientistsAsWorkers				{ get; private set; }
		public int numScientistsAssignedToResearch		{ get; private set; }
		public int totalFoodProduction					{ get; private set; }
		public int totalFoodConsumption					{ get; private set; }
		public int foodLacking							{ get; private set; }
		public int netFoodProduction					{ get; private set; }
		//public int numSolarSatellites					{ get; private set; }

		public int totalRecreationFacilityCapacity		{ get; private set; }
		public int totalForumCapacity					{ get; private set; }
		public int totalMedCenterCapacity				{ get; private set; }
		public int totalResidenceCapacity				{ get; private set; }

		// Player Unit Info
		public ReadOnlyDictionary<map_id, VehicleInfo> vehicleInfo		{ get; private set; }
		public ReadOnlyDictionary<map_id, StructureInfo> structureInfo	{ get; private set; }
		public ReadOnlyDictionary<map_id, WeaponInfo> weaponInfo		{ get; private set; }

		public PlayerUnitState units					{ get; private set; }

		public PlayerCommandMap commandMap				{ get; private set; }


		private Dictionary<int, bool> m_HasTechnology;
		
		/// <summary>
		/// Returns true, if player has the technology.
		/// NOTE: techID is NOT the array index value returned by GetResearchTopic. Use GetTechInfo().GetTechID().
		/// </summary>
		/// <param name="techID">The techID of the technology as found in the techInfo.txt files.</param>
		public bool HasTechnology(int techID)			{ return m_HasTechnology[techID]; }

		/// <summary>
		/// Returns true, if the player has the technology.
		/// NOTE: techIndex is the array index value returned by GetResearchTopic.
		/// </summary>
		/// <param name="techIndex">The tech index of the technology based on the tech info array.</param>
		public bool HasTechnologyByIndex(int techIndex)
		{
			TechInfo techInfo = Research.GetTechInfo(techIndex);
			return m_HasTechnology[techInfo.GetTechID()];
		}

		/// <summary>
		/// Returns true, if the player has the technology required to build this unit.
		/// </summary>
		/// <param name="stateSnapshot">The state to pull unit info from.</param>
		/// <param name="unitTypeID">The unit type to check if technology requirements have been met.</param>
		public bool HasTechnologyForUnit(StateSnapshot stateSnapshot, map_id unitTypeID)
		{
			if (unitTypeID == map_id.None)
				return true;

			GlobalUnitInfo globalUnitInfo = stateSnapshot.GetGlobalUnitInfo(unitTypeID);
			return HasTechnologyByIndex(globalUnitInfo.researchTopic);
		}

		/// <summary>
		/// Returns true if the player's colony type can build this unit at some point regardless of technology or metal requirements.
		/// </summary>
		/// <param name="stateSnapshot">The state to pull unit info from.</param>
		/// <param name="unitTypeID">The unit type to check for usability.</param>
		/// <returns></returns>
		public bool CanColonyUseUnit(StateSnapshot stateSnapshot, map_id unitTypeID)
		{
			GlobalUnitInfo globalUnitInfo = stateSnapshot.GetGlobalUnitInfo(unitTypeID);
			return globalUnitInfo.CanColonyUseUnit(isEden);
		}

		/// <summary>
		/// Returns the info for the specified unit for this player.
		/// </summary>
		public UnitInfoState GetUnitInfo(map_id unitTypeID)
		{
			if ((int)unitTypeID >= 1 && (int)unitTypeID <= 15)		return vehicleInfo[unitTypeID];
			if ((int)unitTypeID >= 21 && (int)unitTypeID <= 58)		return structureInfo[unitTypeID];
			if ((int)unitTypeID >= 59 && (int)unitTypeID <= 73)		return weaponInfo[unitTypeID];

			throw new System.ArgumentOutOfRangeException("unitTypeID", "unitTypeID is invalid!");
		}

		/// <summary>
		/// Checks if this player is the correct colony type, has completed the required research, and has the required resources to build a unit.
		/// </summary>
		public bool CanBuildUnit(StateSnapshot stateSnapshot, map_id unitType, map_id cargoOrWeaponType=map_id.None)
		{
			// Fail Check: Colony Type
			if (!CanColonyUseUnit(stateSnapshot, unitType))
				return false;

			// Fail Check: Research
			if (!HasTechnologyForUnit(stateSnapshot, unitType))
				return false;

			if (cargoOrWeaponType != map_id.None)
			{
				// Fail Check: Cargo Colony Type
				if (!CanColonyUseUnit(stateSnapshot, cargoOrWeaponType))
					return false;

				// Fail Check: Cargo Research
				if (!HasTechnologyForUnit(stateSnapshot, cargoOrWeaponType))
					return false;

				UnitInfoState unitInfo = GetUnitInfo(unitType);
				UnitInfoState cargoInfo = GetUnitInfo(cargoOrWeaponType);
				
				// Fail Check: Unit cost
				if (ore < unitInfo.oreCost + cargoInfo.oreCost) return false;
				if (rareOre < unitInfo.rareOreCost + cargoInfo.rareOreCost) return false;
			}
			else
			{
				UnitInfoState unitInfo = GetUnitInfo(unitType);

				// Fail Check: Unit cost
				if (ore < unitInfo.oreCost) return false;
				if (rareOre < unitInfo.rareOreCost) return false;
			}

			return true;
		}

		/// <summary>
		/// Checks if this player can afford to build the unit.
		/// Does not check if player has the technology to build the unit.
		/// </summary>
		public bool CanAffordUnit(StateSnapshot stateSnapshot, map_id unitType, map_id cargoOrWeaponType=map_id.None)
		{
			if (cargoOrWeaponType != map_id.None)
			{
				UnitInfoState unitInfo = GetUnitInfo(unitType);
				UnitInfoState cargoInfo = GetUnitInfo(cargoOrWeaponType);
				
				// Fail Check: Unit cost
				if (ore < unitInfo.oreCost + cargoInfo.oreCost) return false;
				if (rareOre < unitInfo.rareOreCost + cargoInfo.rareOreCost) return false;
			}
			else
			{
				UnitInfoState unitInfo = GetUnitInfo(unitType);

				// Fail Check: Unit cost
				if (ore < unitInfo.oreCost) return false;
				if (rareOre < unitInfo.rareOreCost) return false;
			}

			return true;
		}


		/// <summary>
		/// Creates an immutable player state from PlayerInfo.
		/// </summary>
		/// <param name="player">The player to pull data from.</param>
		public PlayerState(PlayerEx player, PlayerState prevPlayerState)
		{
			playerID = player.playerID;

			// Set alliances
			List<int> allies = new List<int>();
			List<int> enemies = new List<int>();

			for (int i=0; i < GameState.players.Count; ++i)
			{
				PlayerEx otherPlayer = GameState.players[i];
				if (otherPlayer == null)
					continue;

				if (otherPlayer.IsAlliedTo(player))
					allies.Add(otherPlayer.playerID);
				else
					enemies.Add(otherPlayer.playerID);
			}

			allyPlayerIDs = allies.AsReadOnly();
			enemyPlayerIDs = enemies.AsReadOnly();

			// Player
			difficulty						= player.Difficulty();
			isEden							= player.IsEden();
			isHuman							= player.IsHuman();
		
			kids							= player.Kids();
			workers							= player.Workers();
			scientists						= player.Scientists();
			totalPopulation					= player.TotalPopulation();
		
			ore								= player.Ore();
			rareOre							= player.RareOre();
			foodStored						= player.FoodStored();
			foodSupply						= player.FoodSupply();
		
			moraleLevel						= player.MoraleLevel();
			rlvCount						= player.GetRLVCount();

			// PlayerEx
			maxCommonOre					= player.GetMaxCommonOre();
			maxRareOre						= player.GetMaxRareOre();

			numAvailableWorkers				= player.GetNumAvailableWorkers();
			numAvailableScientists			= player.GetNumAvailableScientists();
			amountPowerGenerated			= player.GetAmountPowerGenerated();
			inactivePowerCapacity			= player.GetInactivePowerCapacity();
			amountPowerConsumed				= player.GetAmountPowerConsumed();
			amountPowerAvailable			= player.GetAmountPowerAvailable();
			numIdleBuildings				= player.GetNumIdleBuildings();
			numActiveBuildings				= player.GetNumActiveBuildings();
			numBuildings					= player.GetNumBuildings();
			numUnpoweredStructures			= player.GetNumUnpoweredStructures();
			numWorkersRequired				= player.GetNumWorkersRequired();
			numScientistsRequired			= player.GetNumScientistsRequired();
			numScientistsAsWorkers			= player.GetNumScientistsAsWorkers();
			numScientistsAssignedToResearch	= player.GetNumScientistsAssignedToResearch();
			totalFoodProduction				= player.GetTotalFoodProduction();
			totalFoodConsumption			= player.GetTotalFoodConsumption();
			foodLacking						= player.GetFoodLacking();
			netFoodProduction				= player.GetNetFoodProduction();
			//numSolarSatellites			= player.GetNumSolarSatellites(); // Can be found in PlayerUnitState

			totalRecreationFacilityCapacity	= player.GetTotalRecreationFacilityCapacity();
			totalForumCapacity				= player.GetTotalForumCapacity();
			totalMedCenterCapacity			= player.GetTotalMedCenterCapacity();
			totalResidenceCapacity			= player.GetTotalResidenceCapacity();

			// Player Unit Info
			Dictionary<map_id, VehicleInfo> vehicleInfo = new Dictionary<map_id, VehicleInfo>();
			Dictionary<map_id, StructureInfo> structureInfo = new Dictionary<map_id, StructureInfo>();
			Dictionary<map_id, WeaponInfo> weaponInfo = new Dictionary<map_id, WeaponInfo>();

			for (int i=1; i <= 15; ++i)
				vehicleInfo.Add((map_id)i, new VehicleInfo((map_id)i, playerID));

			for (int i=21; i <= 58; ++i)
				structureInfo.Add((map_id)i, new StructureInfo((map_id)i, playerID));

			for (int i=59; i <= 73; ++i)
				weaponInfo.Add((map_id)i, new WeaponInfo((map_id)i, playerID));

			this.vehicleInfo = new ReadOnlyDictionary<map_id, VehicleInfo>(vehicleInfo);
			this.structureInfo = new ReadOnlyDictionary<map_id, StructureInfo>(structureInfo);
			this.weaponInfo = new ReadOnlyDictionary<map_id, WeaponInfo>(weaponInfo);

			// Parse units
			units = new PlayerUnitState(player.playerID, this.vehicleInfo, this.structureInfo, this.weaponInfo, prevPlayerState?.units);

			// Create maps
			commandMap = new PlayerCommandMap(units, playerID);

			// Parse technologies
			int techCount = Research.GetTechCount();
			Dictionary<int, bool> hasTech = new Dictionary<int, bool>(techCount);

			for (int i=0; i < techCount; ++i)
			{
				int techID = Research.GetTechInfo(i).GetTechID();
				hasTech.Add(techID, player.HasTechnology(techID));
			}

			m_HasTechnology = new Dictionary<int, bool>(hasTech);
		}
	}
}
