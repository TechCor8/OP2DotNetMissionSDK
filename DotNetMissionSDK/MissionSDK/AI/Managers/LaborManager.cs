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

			AsyncPump.Run(() =>
			{
				List<Action> buildingActions = new List<Action>();

				UpdatePriorityList(stateSnapshot);
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
			});
		}

		private void UpdatePriorityList(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			m_StructurePriority.Clear();
			m_LowPriorityStructures.Clear();
			m_UselessStructures.Clear();
			m_CrippledStructures.Clear();

			// When power is insufficient, we need to increase structure factory priority to correct it.
			// If there are no convecs, we need to increase vehicle factory priority to make one.
			bool addStructureFactoryEarly = owner.amountPowerAvailable < 25;
			bool addVehicleFactoryEarly = addStructureFactoryEarly && owner.units.convecs.Count == 0;

			// Power should always be on
			AddPriorityBuilding(stateSnapshot, owner.units.tokamaks, int.MaxValue);
			AddPriorityBuilding(stateSnapshot, owner.units.mhdGenerators, int.MaxValue);
			AddPriorityBuilding(stateSnapshot, owner.units.solarPowerArrays, int.MaxValue);
			AddPriorityBuilding(stateSnapshot, owner.units.geothermalPlants, int.MaxValue);

			// TODO: Detect if buildings connected to CC are active, if not, mark CC as useless. For now, just make them all high priority
			AddPriorityBuilding(stateSnapshot, owner.units.commandCenters, owner.units.commandCenters.Count);

			// Primary smelters should always be online
			AddPriorityBuilding(stateSnapshot, owner.units.commonOreSmelters, 1);

			if (addVehicleFactoryEarly) AddPriorityBuilding(stateSnapshot, owner.units.vehicleFactories, 1);
			if (addStructureFactoryEarly) AddPriorityBuilding(stateSnapshot, owner.units.structureFactories, 1);

			AddPriorityBuilding(stateSnapshot, owner.units.rareOreSmelters, 1);

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

			//if (m_StructurePriority.Count > 2 &&
			//	m_StructurePriority[m_StructurePriority.Count-1].GetUnitType() == map_id.University &&
			//	m_StructurePriority[m_StructurePriority.Count-2].GetUnitType() == map_id.Nursery)
			if (TethysGame.UsesMorale())
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

			// Primary factories
			if (!addStructureFactoryEarly) AddPriorityBuilding(stateSnapshot, owner.units.structureFactories, owner.units.convecs.Count > 0 ? 1 : 0);
			if (!addVehicleFactoryEarly) AddPriorityBuilding(stateSnapshot, owner.units.vehicleFactories, 1);

			// Low priority structures
			AddPriorityBuilding(stateSnapshot, owner.units.robotCommands, 1, true);
			AddPriorityBuilding(stateSnapshot, owner.units.guardPosts, int.MaxValue);
			AddPriorityBuilding(stateSnapshot, owner.units.arachnidFactories, 1);
			AddPriorityBuilding(stateSnapshot, owner.units.spaceports, 1);
			AddPriorityBuilding(stateSnapshot, owner.units.observatories, 1, true);
			AddPriorityBuilding(stateSnapshot, owner.units.meteorDefenses, 0);

			// Leave storages disabled as they will keep metals
			//AddPriorityBuilding(owner.units.commonStorages, 0);
			//AddPriorityBuilding(owner.units.rareStorages, 0);

			// Unneeded structures
			AddPriorityBuilding(stateSnapshot, owner.units.consumerFactories, 0, true);
			AddPriorityBuilding(stateSnapshot, owner.units.garages, 0, true);
			AddPriorityBuilding(stateSnapshot, owner.units.tradeCenters, 0, true);
			AddPriorityBuilding(stateSnapshot, owner.units.basicLabs, 0, true);
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
					m_CrippledStructures.Add(building);
				else
				{
					if (needed > 0)
					{
						m_StructurePriority.Add(building);
						--needed;
					}
					else if (!markExtrasAsUseless)
						m_LowPriorityStructures.Add(building);
					else
						m_UselessStructures.Add(building);
				}
			}
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
			return stateSnapshot.players[ownerID].commandMap.ConnectsTo(building.GetRect());
		}

		/// <summary>
		/// Attempts to activate all structures in priority order. Assigns scientists to research and workers to training.
		/// </summary>
		private void UpdateActivations(StateSnapshot stateSnapshot, List<Action> buildingActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Deactivate crippled structures
			foreach (StructureState building in m_CrippledStructures)
				buildingActions.Add(() => GameState.GetUnit(building.unitID)?.DoIdle());

			m_AvailableWorkers = owner.numAvailableWorkers;
			m_AvailableScientists = owner.numAvailableScientists;

			// Pull all scientists off of labs
			m_AvailableScientists += RemoveScientistsFromLab(owner.units.basicLabs, 9999, buildingActions);
			m_AvailableScientists += RemoveScientistsFromLab(owner.units.standardLabs, 9999, buildingActions);
			m_AvailableScientists += RemoveScientistsFromLab(owner.units.advancedLabs, 9999, buildingActions);

			List<StructureState> lowPriorityAndUseless = new List<StructureState>(m_LowPriorityStructures);
			lowPriorityAndUseless.AddRange(m_UselessStructures);

			// Activate high priority structures
			ActivateList(m_StructurePriority, lowPriorityAndUseless, true, buildingActions);

			// Assign all spare scientists to research
			AddScientistsToLabs(owner.units.advancedLabs, lowPriorityAndUseless, buildingActions);
			AddScientistsToLabs(owner.units.standardLabs, lowPriorityAndUseless, buildingActions);
			AddScientistsToLabs(owner.units.basicLabs, lowPriorityAndUseless, buildingActions);

			// Assign workers to training, but only if no scientists are assigned as workers
			if (owner.numScientistsAsWorkers == 0)
				AddWorkersToTraining(owner, buildingActions);

			// Activate low priority structures
			ActivateList(m_LowPriorityStructures, m_UselessStructures, false, buildingActions);
			ActivateList(m_UselessStructures, new List<StructureState>(), false, buildingActions);
		}

		private void ActivateList(List<StructureState> buildingsToActivate, List<StructureState> buildingsToIdle, bool canUseScientistsAsWorkers, List<Action> buildingActions)
		{
			// When freeing labor, we want to start with buildingsToIdle, and include not yet processed buildingsToActivate
			List<StructureState> allBuildings = new List<StructureState>(buildingsToActivate);
			allBuildings.AddRange(buildingsToIdle);

			for (int i=0; i < buildingsToActivate.Count; ++i)
			{
				StructureState building = buildingsToActivate[i];

				// Attempt to activate structure with currently available labor
				if (!ActivateStructure(building, canUseScientistsAsWorkers, buildingActions))
				{
					// Get needed workers and scientists for building
					StructureInfo info = building.structureInfo;
					int workersNeeded = info.workersRequired;
					int scientistsNeeded = info.scientistsRequired;

					workersNeeded -= Math.Max(m_AvailableWorkers, 0);
					scientistsNeeded -= Math.Max(m_AvailableScientists, 0);

					// Attempt to free labor
					if (FreeLabor(allBuildings, i, workersNeeded, scientistsNeeded, canUseScientistsAsWorkers, buildingActions))
					{
						// Labor freed, try again
						--i;
						continue;
					}
					else
					{
						// Couldn't free up labor, idle structure
						if (building.lastCommand != CommandType.ctMoIdle)
							buildingActions.Add(() => GameState.GetUnit(building.unitID)?.DoIdle());
					}
				}
			}
		}

		/// <summary>
		/// Activates a structure if it isn't already.
		/// </summary>
		/// <returns>False if there are not enough workers.</returns>
		private bool ActivateStructure(StructureState building, bool canUseScientistsAsWorkers, List<Action> buildingActions)
		{
			StructureInfo info = building.structureInfo;

			int workersReq = info.workersRequired;
			int scientistsReq = info.scientistsRequired;

			// Does building already meet labor requirements?
			bool hasWorkers = workersReq == 0 || building.hasWorkers;
			bool hasScientists = scientistsReq == 0 || building.hasScientists;

			// Don't need to do anything if the building has labor
			if (hasWorkers && hasScientists)
			{
				if (!building.isEnabled)
					buildingActions.Add(() => GameState.GetUnit(building.unitID)?.DoUnIdle());

				return true;
			}

			int scientistsAsWorkers = 0;

			// If there are not enough workers, pull from scientists
			if (m_AvailableWorkers < workersReq && canUseScientistsAsWorkers)
			{
				int workersNeeded = workersReq - m_AvailableWorkers;
				if (workersNeeded <= m_AvailableScientists)
					scientistsAsWorkers = workersNeeded;
			}

			// Not enough labor, find more labor or idle
			if (m_AvailableWorkers+scientistsAsWorkers < workersReq || m_AvailableScientists-scientistsAsWorkers < scientistsReq)
				return false;

			// Found labor, activate structure
			buildingActions.Add(() => GameState.GetUnit(building.unitID)?.DoUnIdle());

			m_AvailableWorkers -= (workersReq - scientistsAsWorkers);
			m_AvailableScientists -= (scientistsReq + scientistsAsWorkers);

			return true;
		}

		/// <summary>
		/// Attempts to add scientists to labs until maxed.
		/// Will use available scientists first and then buildingsToIdle to get the rest.
		/// </summary>
		private void AddScientistsToLabs(ReadOnlyCollection<LabState> labs, List<StructureState> buildingsToIdle, List<Action> buildingActions)
		{
			for (int i=0; i < labs.Count; ++i)
			{
				LabState lab = labs[i];

				if (!AddScientistsToLab(lab, buildingActions))
				{
					TechInfo info = Research.GetTechInfo(lab.labCurrentTopic);
					int maxScientists = info.GetMaxScientists();
					int assignedScientists = Math.Min(lab.labScientistCount, 1); // All but 1 scientist will be removed by this point

					if (FreeLabor(buildingsToIdle, -1, 0, maxScientists-assignedScientists, false, buildingActions))
					{
						// Labor freed, try again
						--i;
						continue;
					}
				}
			}
		}

		// Returns true if lab is at max scientists
		private bool AddScientistsToLab(LabState lab, List<Action> buildingActions)
		{
			if (lab.isEnabled && lab.isBusy)
			{
				// Get max assigned
				TechInfo info = Research.GetTechInfo(lab.labCurrentTopic);
				int maxScientists = info.GetMaxScientists();
				int scientistsAssigned = Math.Min(lab.labScientistCount, 1); // All but 1 scientist will be removed by this point

				int scientistsNeeded = maxScientists - scientistsAssigned;
				int scientistsToAdd = Math.Min(scientistsNeeded, m_AvailableScientists);

				buildingActions.Add(() => GameState.GetUnit(lab.unitID)?.DoResearch(lab.labCurrentTopic, scientistsAssigned + scientistsToAdd));//SetLabScientistCount(scientistsAssigned + scientistsToAdd));
				
				m_AvailableScientists -= scientistsToAdd;

				return scientistsAssigned + scientistsToAdd >= maxScientists;
			}

			return true;
		}

		private int AddWorkersToTraining(PlayerState owner, List<Action> buildingActions)
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

					buildingActions.Add(() => GameState.GetUnit(university.unitID)?.DoTrainScientists(numToAssign));
					
					assignedWorkers += numToAssign;
					workersToAssign -= numToAssign;
					if (workersToAssign == 0)
						break;
				}
			}

			return assignedWorkers;
		}

		// Returns true if labor has successfully be freed
		private bool FreeLabor(List<StructureState> buildingsForLabor, int cutoffIndex, int workersNeeded, int scientistsNeeded, bool canUseScientistsAsWorkers, List<Action> buildingActions)
		{
			int workersAdded = 0;
			int scientistsAdded = 0;

			List<StructureState> buildingsToIdle = new List<StructureState>();

			// Free labor by idling low priority structures
			for (int i=buildingsForLabor.Count-1; i > cutoffIndex; --i)
			{
				StructureState building = buildingsForLabor[i];
				StructureInfo info = building.structureInfo;

				int workersInBuilding = info.workersRequired;
				int scientistsInBuilding = info.scientistsRequired;

				bool doIdle = false;

				// Structure does not use labor
				if (workersInBuilding == 0 && scientistsInBuilding == 0)
					continue;

				// Can't idle busy structures
				if (building.isBusy)
					continue;

				// Must not be under construction or idle
				bool skip = false;
				switch (building.lastCommand)
				{
					case CommandType.ctMoDevelop:
					case CommandType.ctMoUnDevelop:
					case CommandType.ctMoIdle:
						skip = true;
						break;
				}

				if (skip)
					continue;

				if (!building.hasWorkers)
					workersInBuilding = 0;

				if (!building.hasScientists)
					scientistsInBuilding = 0;

				// Check if building has labor. If it does, idle it and collect the labor.
				if (workersInBuilding > 0 && workersNeeded > workersAdded)
				{
					workersAdded += workersInBuilding;
					doIdle = true;
				}

				if (scientistsInBuilding > 0 && (scientistsNeeded > scientistsAdded || canUseScientistsAsWorkers && workersNeeded+scientistsNeeded > workersAdded+scientistsAdded))
				{
					scientistsAdded += scientistsInBuilding;
					doIdle = true;
				}

				if (doIdle)
					buildingsToIdle.Add(building);

				// If labor needs met, return
				if (workersAdded+scientistsAdded >= workersNeeded+scientistsNeeded && scientistsAdded >= scientistsNeeded)
				{
					m_AvailableWorkers += workersAdded;
					m_AvailableScientists += scientistsAdded;

					foreach (StructureState b in buildingsToIdle)
						buildingActions.Add(() => GameState.GetUnit(b.unitID)?.DoIdle());

					return true;
				}
			}

			return false;
		}

		private int RemoveScientistsFromLab(IEnumerable<LabState> labs, int scientistsToRemove, List<Action> buildingActions)
		{
			if (scientistsToRemove <= 0)
				return 0;

			int scientistsRemoved = 0;

			foreach (LabState lab in labs)
			{
				if (!lab.isBusy)
					continue;

				int scientistsInLab = lab.labScientistCount;

				int amountToRemove = Math.Min(scientistsToRemove, scientistsInLab-1); // Leave 1 scientist in the lab
				if (amountToRemove <= 0)
					continue;

				buildingActions.Add(() => GameState.GetUnit(lab.unitID)?.DoResearch(lab.labCurrentTopic, scientistsInLab - amountToRemove));//SetLabScientistCount(scientistsInLab - amountToRemove));
				scientistsRemoved += amountToRemove;
			}

			return scientistsRemoved;
		}
	}
}
