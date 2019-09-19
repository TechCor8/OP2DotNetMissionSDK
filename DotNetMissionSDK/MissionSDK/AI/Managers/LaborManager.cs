using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.Async;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DotNetMissionSDK.AI.Managers
{
	/// <summary>
	/// Manages labor division by activating and idling structures.
	/// <para>
	/// If a build is disabled due to damage, idle it until it is repaired.
	/// Command centers can only be idled if every connected structure is idle.
	/// 
	/// Labor priorities:
	/// 1. Main common/rare smelter
	/// 2. Agridomes
	/// 3. Labor growth (nursery/university)
	/// 4. Morale structures (This is skipped if there isn't a nursery or university)
	/// 5. Researching labs (Worker only. Researchers have lowest priority.)
	/// 6. Structure Factory
	/// 7. Vehicle Factory
	/// 8. Robot Command
	/// 9. Extra smelters (if less than 10k ore)
	/// 10. Extra vehicle factories
	/// 11. Extra structure factories
	/// </para>
	/// </summary>
	public class LaborManager
	{
		private List<StructureState> m_StructurePriority = new List<StructureState>();
		private List<StructureState> m_LowPriorityStructures = new List<StructureState>();
		private List<StructureState> m_UselessStructures = new List<StructureState>();
		private List<StructureState> m_CrippledStructures = new List<StructureState>();

		private int m_AvailableWorkers;
		private int m_AvailableScientists;
		private int m_AvailablePower;

		private bool m_IsProcessing;

		public BotPlayer botPlayer	{ get; private set; }
		public int ownerID			{ get; private set; }


		public LaborManager(BotPlayer botPlayer, int ownerID)
		{
			this.botPlayer = botPlayer;
			this.ownerID = ownerID;
		}

		public void Update(StateSnapshot stateSnapshot)
		{
			ThreadAssert.MainThreadRequired();

			if (m_IsProcessing)
				return;

			m_IsProcessing = true;

			// Get data that requires main thread
			ReadOnlyCollection<int> structureLaborOrderByID = botPlayer.baseManager.GetStructureLaborOrderByID();

			stateSnapshot.Retain();

			AsyncPump.Run(() =>
			{
				List<Action> buildingActions = new List<Action>();

				// Convert order ID to structure state
				List<StructureState> priorityStructures = new List<StructureState>(structureLaborOrderByID.Count);
				foreach (int id in structureLaborOrderByID)
				{
					StructureState unit = stateSnapshot.GetUnit(id) as StructureState;
					if (unit == null) continue;
					priorityStructures.Add(unit);
				}

				UpdatePriorityList(stateSnapshot, priorityStructures.ToArray());
				UpdateActivations(stateSnapshot, buildingActions);

				return buildingActions;
			},
			(object returnState) =>
			{
				m_IsProcessing = false;

				// Execute all completed actions
				List<Action> buildingActions = (List<Action>)returnState;

				foreach (Action action in buildingActions)
					action();

				stateSnapshot.Release();
			});
		}

		private int GetStorageUnitsRequired(int metalToStore, int unitCapacity, int unitsAvailable)
		{
			return Math.Min((int)Math.Ceiling(metalToStore / (float)unitCapacity), unitsAvailable);
		}

		private void UpdatePriorityList(StateSnapshot stateSnapshot, StructureState[] priorityStructures)
		{
			// We control command center priority internally, so remove any references to them
			List<StructureState> externalStructurePriority = new List<StructureState>(priorityStructures);
			externalStructurePriority.RemoveAll((structure) => structure.unitType == map_id.CommandCenter);

			PlayerState owner = stateSnapshot.players[ownerID];

			m_StructurePriority.Clear();
			m_LowPriorityStructures.Clear();
			m_UselessStructures.Clear();
			m_CrippledStructures.Clear();

			// Power should always be on
			AddPriorityBuilding(stateSnapshot, owner.units.tokamaks, int.MaxValue);
			AddPriorityBuilding(stateSnapshot, owner.units.mhdGenerators, int.MaxValue);
			AddPriorityBuilding(stateSnapshot, owner.units.solarPowerArrays, int.MaxValue);
			AddPriorityBuilding(stateSnapshot, owner.units.geothermalPlants, int.MaxValue);

			// Calculate number of storages required to avoid losing metal
			int commonMetalLost = owner.ore;
			int rareMetalLost = owner.rareOre;

			int commonSmeltersRequired = GetStorageUnitsRequired(commonMetalLost, owner.structureInfo[map_id.CommonOreSmelter].storageCapacity, owner.units.commonOreSmelters.Count);
			int rareSmeltersRequired = GetStorageUnitsRequired(rareMetalLost, owner.structureInfo[map_id.RareOreSmelter].storageCapacity, owner.units.rareOreSmelters.Count);

			commonMetalLost -= commonSmeltersRequired * owner.structureInfo[map_id.CommonOreSmelter].storageCapacity;
			rareMetalLost -= rareSmeltersRequired * owner.structureInfo[map_id.RareOreSmelter].storageCapacity;

			int commonStoragesRequired = GetStorageUnitsRequired(commonMetalLost, owner.structureInfo[map_id.CommonStorage].storageCapacity, owner.units.commonStorages.Count);
			int rareStoragesRequired = GetStorageUnitsRequired(rareMetalLost, owner.structureInfo[map_id.RareStorage].storageCapacity, owner.units.rareStorages.Count);

			// Set smelter/storage needs
			AddPriorityBuilding(stateSnapshot, owner.units.commonOreSmelters, commonSmeltersRequired);
			AddPriorityBuilding(stateSnapshot, owner.units.rareOreSmelters, rareSmeltersRequired);

			AddPriorityBuilding(stateSnapshot, owner.units.commonStorages, commonStoragesRequired);
			AddPriorityBuilding(stateSnapshot, owner.units.rareStorages, rareStoragesRequired);


			// Base manager chosen structures are very high priority
			AddPriorityBuilding(stateSnapshot, externalStructurePriority, int.MaxValue);

			// Get number of agridomes needed
			int neededAgridomes = 1;
			int numActiveAgridomes = owner.units.agridomes.Count((StructureState unit) => unit.isEnabled);
			if (numActiveAgridomes > 0)
			{
				int foodPerAgridome = owner.totalFoodProduction / numActiveAgridomes;
				if (foodPerAgridome == 0) foodPerAgridome = 1;
				neededAgridomes = owner.totalFoodConsumption / foodPerAgridome + 1;
			}

			AddPriorityBuilding(stateSnapshot, owner.units.agridomes, neededAgridomes, true);

			// Labor growth
			AddPriorityBuilding(stateSnapshot, owner.units.nurseries, 1, true);
			AddPriorityBuilding(stateSnapshot, owner.units.universities, 1, true);

			if (stateSnapshot.usesMorale)
			{
				// Morale fluctuates, include morale structures

				// Residences
				int neededCapacity = owner.totalPopulation;

				neededCapacity = AddToCapacity(stateSnapshot, neededCapacity, map_id.AdvancedResidence, owner.units.advancedResidences);
				if (neededCapacity > 0) AddToCapacity(stateSnapshot, neededCapacity, map_id.ReinforcedResidence, owner.units.reinforcedResidences);
				if (neededCapacity > 0) AddToCapacity(stateSnapshot, neededCapacity, map_id.Residence, owner.units.residences);

				// Medical centers
				neededCapacity = owner.totalPopulation;

				AddToCapacity(stateSnapshot, neededCapacity, map_id.MedicalCenter, owner.units.medicalCenters);

				// GORF
				AddPriorityBuilding(stateSnapshot, owner.units.gorfs, 1, true);

				// Recreation
				neededCapacity = owner.totalPopulation;

				neededCapacity = AddToCapacity(stateSnapshot, neededCapacity, map_id.Forum, owner.units.forums);
				if (neededCapacity > 0) AddToCapacity(stateSnapshot, neededCapacity, map_id.RecreationFacility, owner.units.recreationFacilities);

				// DIRT
				AddPriorityBuilding(stateSnapshot, owner.units.dirts, 1);
			}
			else
			{
				// All morale structures are useless
				AddPriorityBuilding(stateSnapshot, owner.units.advancedResidences, 0, true);
				AddPriorityBuilding(stateSnapshot, owner.units.reinforcedResidences, 0, true);
				AddPriorityBuilding(stateSnapshot, owner.units.residences, 0, true);

				AddPriorityBuilding(stateSnapshot, owner.units.medicalCenters, 0, true);
				AddPriorityBuilding(stateSnapshot, owner.units.gorfs, 0, true);
				AddPriorityBuilding(stateSnapshot, owner.units.forums, 0, true);
				AddPriorityBuilding(stateSnapshot, owner.units.recreationFacilities, 0, true);
				AddPriorityBuilding(stateSnapshot, owner.units.dirts, 0, true);
			}

			// Labs
			AddPriorityBuilding(stateSnapshot, owner.units.advancedLabs, 1, true);
			AddPriorityBuilding(stateSnapshot, owner.units.standardLabs, 1, true);

			// Low priority structures
			AddPriorityBuilding(stateSnapshot, owner.units.robotCommands, 1, true);
			AddPriorityBuilding(stateSnapshot, owner.units.guardPosts, int.MaxValue);
			AddPriorityBuilding(stateSnapshot, owner.units.observatories, 1, true);
			AddPriorityBuilding(stateSnapshot, owner.units.meteorDefenses, 0);

			// Factories
			AddPriorityBuilding(stateSnapshot, owner.units.structureFactories, 0);
			AddPriorityBuilding(stateSnapshot, owner.units.vehicleFactories, 0);
			AddPriorityBuilding(stateSnapshot, owner.units.arachnidFactories, 0);
			AddPriorityBuilding(stateSnapshot, owner.units.spaceports, 0);

			// Unneeded structures
			AddPriorityBuilding(stateSnapshot, owner.units.consumerFactories, 0, true);
			AddPriorityBuilding(stateSnapshot, owner.units.garages, 0, true);
			AddPriorityBuilding(stateSnapshot, owner.units.tradeCenters, 0, true);
			AddPriorityBuilding(stateSnapshot, owner.units.basicLabs, 0, true);

			// Determine command center priority based on structure priority
			List<StructureState> unassignedCCs = new List<StructureState>(owner.units.commandCenters);

			// Crippled CCs are always put in the crippled priority
			for (int i=0; i < unassignedCCs.Count; ++i)
			{
				if (!CanBeActivated(stateSnapshot, unassignedCCs[i]))
				{
					AddPriorityUnique(m_CrippledStructures, unassignedCCs[i], 3);
					unassignedCCs.RemoveAt(i--);
				}
			}

			AssignCommandCenterPriority(stateSnapshot, unassignedCCs, m_StructurePriority);
			AssignCommandCenterPriority(stateSnapshot, unassignedCCs, m_LowPriorityStructures);
			AssignCommandCenterPriority(stateSnapshot, unassignedCCs, m_UselessStructures);
			m_UselessStructures.AddRange(unassignedCCs); // Remaining CCs are extra useless
		}

		private void AssignCommandCenterPriority(StateSnapshot stateSnapshot, List<StructureState> unassignedCCs, List<StructureState> priorityList)
		{
			for (int i=0; i < priorityList.Count; ++i)
			{
				StructureState structure = priorityList[i];

				// Structures that don't need a command center are ignored
				if (!BuildStructureTask.NeedsTube(structure.unitType))
					continue;

				StructureState cc = stateSnapshot.commandMap.GetConnectedCommandCenter(ownerID, structure.position);
				if (cc == null) continue; // Structure not connected

				// Assign CC priority above this structure
				if (unassignedCCs.Remove(cc))
				{
					priorityList.Insert(i--, cc);

					if (unassignedCCs.Count == 0)
						break;

					continue;
				}
			}
		}

		// Returns remaining capacity
		private int AddToCapacity(StateSnapshot stateSnapshot, int neededCapacity, map_id buildingType, IEnumerable<StructureState> buildings)
		{
			StructureInfo info = stateSnapshot.players[ownerID].structureInfo[buildingType];
			int capacityPerUnit = info.productionCapacity;
			int neededUnits = neededCapacity / capacityPerUnit;
			if (neededCapacity % capacityPerUnit > 0)
				++neededUnits;

			int oldCount = m_StructurePriority.Count;
			AddPriorityBuilding(stateSnapshot, buildings, neededUnits, true);
			int unitsAvailable = m_StructurePriority.Count - oldCount;

			neededUnits -= unitsAvailable;
			neededCapacity = neededUnits * capacityPerUnit;

			return neededCapacity;
		}

		private void AddPriorityBuilding(StateSnapshot stateSnapshot, IEnumerable<StructureState> buildings, int needed, bool markExtrasAsUseless=false)
		{
			foreach (StructureState building in buildings)
			{
				if (!CanBeActivated(stateSnapshot, building))
					AddPriorityUnique(m_CrippledStructures, building, 3);
				else
				{
					if (needed > 0)
					{
						AddPriorityUnique(m_StructurePriority, building, 0);
						--needed;
					}
					else if (!markExtrasAsUseless)
						AddPriorityUnique(m_LowPriorityStructures, building, 1);
					else
						AddPriorityUnique(m_UselessStructures, building, 2);
				}
			}
		}

		private void AddPriorityUnique(List<StructureState> list, StructureState building, int priority)
		{
			// Current priority is higher than requested, skip
			if (priority >= 0 && m_StructurePriority.Contains(building))		return;
			if (priority >= 1 && m_LowPriorityStructures.Contains(building))	return;
			if (priority >= 2 && m_UselessStructures.Contains(building))		return;
			if (priority >= 3 && m_CrippledStructures.Contains(building))		return;

			// Priority has increased, remove from previous list
			if (priority < 1) m_LowPriorityStructures.Remove(building);
			if (priority < 2) m_UselessStructures.Remove(building);
			if (priority < 3) m_CrippledStructures.Remove(building);

			list.Add(building);
		}

		/// <summary>
		/// For the purposes of priority, a building can be activated if it is not infected and damage is not critical.
		/// </summary>
		private bool CanBeActivated(StateSnapshot stateSnapshot, StructureState building)
		{
			// Cannot be infected
			if (building.isInfected)
				return false;

			// Must not be under construction
			switch (building.lastCommand)
			{
				case CommandType.ctMoDevelop:
				case CommandType.ctMoUnDevelop:
					return false;
			}

			// Cannot have critical damage
			StructureInfo info = building.structureInfo;
			float hitPerc = 1 - (building.damage / (float)info.hitPoints);

			if (hitPerc < 0.25f)
				return false;

			// If doesn't need tube, it's ready to go
			if (!BuildStructureTask.NeedsTube(building.unitType) || building.unitType == map_id.GuardPost)
				return true;

			// Cannot be disconnected from CC
			return stateSnapshot.commandMap.ConnectsTo(ownerID, building.GetRect());
		}

		/// <summary>
		/// Attempts to activate all structures in priority order. Assigns scientists to research and workers to training.
		/// </summary>
		private void UpdateActivations(StateSnapshot stateSnapshot, List<Action> buildingActions)
		{
			List<Action> idleActions = new List<Action>();
			List<Action> enableActions = new List<Action>();

			PlayerState owner = stateSnapshot.players[ownerID];

			m_AvailableWorkers = owner.workers;
			m_AvailableScientists = owner.scientists;
			m_AvailablePower = owner.amountPowerGenerated;

			// Remove workers in training
			foreach (UniversityState university in owner.units.universities)
				m_AvailableWorkers -= university.workersInTraining;

			// Since we cannot remove workers from busy structures, substract from available labor pool.
			foreach (UnitState unit in owner.units.GetStructures())
			{
				StructureState structure = (StructureState)unit;
				StructureInfo info = owner.structureInfo[unit.unitType];

				if (!unit.isBusy)			continue;
				if (!structure.isEnabled)	continue;

				if (structure.hasWorkers)
				{
					int scientistsAsWorkers = GetScientistsAsWorkers(info.workersRequired);

					m_AvailableWorkers -= (info.workersRequired - scientistsAsWorkers);
					m_AvailableScientists -= scientistsAsWorkers;
				}
				if (structure.hasScientists) m_AvailableScientists -= info.scientistsRequired;
				if (structure.hasPower) m_AvailablePower -= info.powerRequired;

				LabState lab = structure as LabState;
				if (lab != null)
				{
					// Scientists should take refuge from the collapsing structure!
					if (lab.isCritical)
						idleActions.Add(() => GameState.GetUnit(lab.unitID)?.DoResearch(lab.labCurrentTopic, 0));
				}
				else
				{
					// Non-lab structures won't have labor assignments changed.
					m_StructurePriority.Remove(structure);
					m_LowPriorityStructures.Remove(structure);
					m_UselessStructures.Remove(structure);
				}

				// All busy crippled structures won't have labor assignments changed.
				m_CrippledStructures.Remove(structure);
			}

			// Activate high priority structures
			ActivateList(stateSnapshot, enableActions, idleActions, m_StructurePriority, true);

			// Assign workers to training, but only if no scientists are assigned as workers
			if (owner.numScientistsAsWorkers == 0)
				AddWorkersToTraining(owner, enableActions);

			// Activate low priority structures
			ActivateList(stateSnapshot,enableActions, idleActions, m_LowPriorityStructures, false);
			ActivateList(stateSnapshot,enableActions, idleActions, m_UselessStructures, false);

			// Deactivate crippled structures
			foreach (StructureState building in m_CrippledStructures)
			{
				// Skip metal storage if it will destroy metals
				if (building.unitType == map_id.CommonOreSmelter || building.unitType == map_id.CommonStorage)
				{
					if (owner.ore > owner.maxCommonOre-building.structureInfo.storageCapacity)
						continue;
				}
				if (building.unitType == map_id.RareOreSmelter || building.unitType == map_id.RareStorage)
				{
					if (owner.rareOre > owner.maxRareOre-building.structureInfo.storageCapacity)
						continue;
				}

				// Deactivate structure
				if (building.lastCommand != CommandType.ctMoIdle)
					idleActions.Add(() => GameState.GetUnit(building.unitID)?.DoIdle());
			}

			// Add results to action list in order
			buildingActions.AddRange(idleActions);
			buildingActions.AddRange(enableActions);
		}

		private void ActivateList(StateSnapshot stateSnapshot, List<Action> enableActions, List<Action> idleActions, List<StructureState> buildingsToActivate, bool canUseScientistsAsWorkers)
		{
			// Attempt to activate all buildings in priority order
			for (int i=0; i < buildingsToActivate.Count; ++i)
			{
				StructureState building = buildingsToActivate[i];
				StructureInfo info = building.structureInfo;

				// If there are not enough workers, pull from scientists
				int scientistsAsWorkers = canUseScientistsAsWorkers ? GetScientistsAsWorkers(info.workersRequired) : 0;

				if (building is LabState && building.isBusy && building.isEnabled)
				{
					// Add scientists to busy lab, worker already added during "busy" assignment phase
					AddScientistsToLab(enableActions, idleActions, (LabState)building);
				}
				else if (m_AvailableWorkers+scientistsAsWorkers >= info.workersRequired && 
					m_AvailableScientists-scientistsAsWorkers >= info.scientistsRequired &&
					m_AvailablePower >= info.powerRequired &&
					(!BuildStructureTask.NeedsTube(building.unitType) || stateSnapshot.commandMap.ConnectsTo(ownerID, building.GetRect(), false)))
				{
					// Found labor, power and cc access. Activate building.
					m_AvailableWorkers -= (info.workersRequired - scientistsAsWorkers);
					m_AvailableScientists -= (info.scientistsRequired + scientistsAsWorkers);
					m_AvailablePower -= info.powerRequired;

					if (!building.isEnabled)
						enableActions.Add(() => GameState.GetUnit(building.unitID)?.DoUnIdle());
				}
				else
				{
					// Not enough labor, power or cc access. Idle building.
					if (building.lastCommand != CommandType.ctMoIdle)
						idleActions.Add(() => GameState.GetUnit(building.unitID)?.DoIdle());
				}
			}
		}

		private int GetScientistsAsWorkers(int workersRequired)
		{
			// If there are not enough workers, pull from scientists
			if (m_AvailableWorkers < workersRequired)
			{
				int workersNeeded = workersRequired - m_AvailableWorkers;
				if (workersNeeded <= m_AvailableScientists)
					return workersNeeded;
			}

			return 0;
		}

		/// <summary>
		/// Adds scientists until lab is maxed or there are not enough scientists available.
		/// </summary>
		private void AddScientistsToLab(List<Action> addActions, List<Action> removeActions, LabState lab)
		{
			// Get max assigned
			TechInfo info = Research.GetTechInfo(lab.labCurrentTopic);
			int maxScientists = info.GetMaxScientists();

			int scientistsAssigned = Math.Min(maxScientists, m_AvailableScientists);

			m_AvailableScientists -= scientistsAssigned;

			if (scientistsAssigned < lab.labScientistCount)
			{
				// Removing scientists
				removeActions.Add(() => GameState.GetUnit(lab.unitID)?.DoResearch(lab.labCurrentTopic, scientistsAssigned));//SetLabScientistCount(scientistsAssigned));
			}
			else if (scientistsAssigned > lab.labScientistCount)
			{
				// Adding scientists
				if (lab.isEnabled && lab.isBusy)
				{
					addActions.Add(() => GameState.GetUnit(lab.unitID)?.DoResearch(lab.labCurrentTopic, scientistsAssigned));//SetLabScientistCount(scientistsAssigned));
				}
			}
		}

		private int AddWorkersToTraining(PlayerState owner, List<Action> enableActions)
		{
			// Assign workers to training (20% of population)
			int workersToAssign = 0;
			int neededScientists = (int)(owner.totalPopulation * 0.2f) - owner.scientists;
			if (neededScientists > 0)
				workersToAssign = Math.Min(neededScientists, m_AvailableWorkers);

			int assignedWorkers = 0;

			if (workersToAssign == 0)
				return 0;

			// Find university to assign to
			foreach (StructureState university in owner.units.universities)
			{
				if (university.isEnabled && !university.isBusy)
				{
					int numToAssign = Math.Min(workersToAssign, 10);

					m_AvailableWorkers -= numToAssign;

					enableActions.Add(() => GameState.GetUnit(university.unitID)?.DoTrainScientists(numToAssign));
					
					assignedWorkers += numToAssign;
					workersToAssign -= numToAssign;
					if (workersToAssign == 0)
						break;
				}
			}

			return assignedWorkers;
		}
	}
}
