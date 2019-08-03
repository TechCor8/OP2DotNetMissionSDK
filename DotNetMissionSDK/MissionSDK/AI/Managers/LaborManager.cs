using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System;
using System.Collections.Generic;

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
		private List<UnitEx> m_StructurePriority = new List<UnitEx>();
		private List<UnitEx> m_LowPriorityStructures = new List<UnitEx>();
		private List<UnitEx> m_UselessStructures = new List<UnitEx>();
		private List<UnitEx> m_CrippledStructures = new List<UnitEx>();

		private int m_AvailableWorkers;
		private int m_AvailableScientists;

		public BotPlayer botPlayer	{ get; private set; }
		public PlayerInfo owner		{ get; private set; }


		public LaborManager(BotPlayer botPlayer, PlayerInfo owner)
		{
			this.botPlayer = botPlayer;
			this.owner = owner;
		}

		public void Update()
		{
			UpdatePriorityList();
			UpdateActivations();
		}

		private void UpdatePriorityList()
		{
			m_StructurePriority.Clear();
			m_LowPriorityStructures.Clear();
			m_UselessStructures.Clear();
			m_CrippledStructures.Clear();

			// When power is insufficient, we need to increase structure factory priority to correct it.
			// If there are no convecs, we need to increase vehicle factory priority to make one.
			bool addStructureFactoryEarly = owner.player.GetAmountPowerAvailable() < 25;
			bool addVehicleFactoryEarly = addStructureFactoryEarly && owner.units.convecs.Count == 0;

			// Power should always be on
			AddPriorityBuilding(owner.units.tokamaks, int.MaxValue);
			AddPriorityBuilding(owner.units.mhdGenerators, int.MaxValue);
			AddPriorityBuilding(owner.units.solarPowerArrays, int.MaxValue);
			AddPriorityBuilding(owner.units.geothermalPlants, int.MaxValue);

			// TODO: Detect if buildings connected to CC are active, if not, mark CC as useless. For now, just make them all high priority
			AddPriorityBuilding(owner.units.commandCenters, owner.units.commandCenters.Count);

			// Primary smelters should always be online
			AddPriorityBuilding(owner.units.commonOreSmelters, 1);

			if (addVehicleFactoryEarly) AddPriorityBuilding(owner.units.vehicleFactories, 1);
			if (addStructureFactoryEarly) AddPriorityBuilding(owner.units.structureFactories, 1);

			AddPriorityBuilding(owner.units.rareOreSmelters, 1);

			// Get number of agridomes needed
			int neededAgridomes = 1;
			int numActiveAgridomes = owner.units.agridomes.FindAll((UnitEx unit) => unit.IsEnabled()).Count;
			if (numActiveAgridomes > 0)
			{
				int foodPerAgridome = owner.player.GetTotalFoodProduction() / numActiveAgridomes;
				if (foodPerAgridome == 0) foodPerAgridome = 1;
				neededAgridomes = owner.player.GetTotalFoodConsumption() / foodPerAgridome + 1;
			}

			AddPriorityBuilding(owner.units.agridomes, neededAgridomes, true);

			// Labor growth
			AddPriorityBuilding(owner.units.nurseries, 1, true);
			AddPriorityBuilding(owner.units.universities, 1, true);

			//if (m_StructurePriority.Count > 2 &&
			//	m_StructurePriority[m_StructurePriority.Count-1].GetUnitType() == map_id.University &&
			//	m_StructurePriority[m_StructurePriority.Count-2].GetUnitType() == map_id.Nursery)
			if (TethysGame.UsesMorale())
			{
				// Morale fluctuates, include morale structures

				// Residences
				int neededCapacity = owner.player.TotalPopulation();

				neededCapacity = AddToCapacity(neededCapacity, map_id.AdvancedResidence, owner.units.advancedResidences);
				if (neededCapacity > 0) AddToCapacity(neededCapacity, map_id.ReinforcedResidence, owner.units.reinforcedResidences);
				if (neededCapacity > 0) AddToCapacity(neededCapacity, map_id.Residence, owner.units.residences);

				// Medical centers
				neededCapacity = owner.player.TotalPopulation();

				AddToCapacity(neededCapacity, map_id.MedicalCenter, owner.units.medicalCenters);

				// GORF
				AddPriorityBuilding(owner.units.gorfs, 1, true);

				// Recreation
				neededCapacity = owner.player.TotalPopulation();

				neededCapacity = AddToCapacity(neededCapacity, map_id.Forum, owner.units.forums);
				if (neededCapacity > 0) AddToCapacity(neededCapacity, map_id.RecreationFacility, owner.units.recreationFacilities);

				// DIRT
				AddPriorityBuilding(owner.units.dirts, 1);
			}
			else
			{
				// All morale structures are useless
				AddPriorityBuilding(owner.units.advancedResidences, 0, true);
				AddPriorityBuilding(owner.units.reinforcedResidences, 0, true);
				AddPriorityBuilding(owner.units.residences, 0, true);

				AddPriorityBuilding(owner.units.medicalCenters, 0, true);
				AddPriorityBuilding(owner.units.gorfs, 0, true);
				AddPriorityBuilding(owner.units.forums, 0, true);
				AddPriorityBuilding(owner.units.recreationFacilities, 0, true);
				AddPriorityBuilding(owner.units.dirts, 0, true);
			}

			// Labs
			AddPriorityBuilding(owner.units.advancedLabs, 1, true);
			AddPriorityBuilding(owner.units.standardLabs, 1, true);

			// Primary factories
			if (!addStructureFactoryEarly) AddPriorityBuilding(owner.units.structureFactories, owner.units.convecs.Count > 0 ? 1 : 0);
			if (!addVehicleFactoryEarly) AddPriorityBuilding(owner.units.vehicleFactories, 1);

			// Low priority structures
			AddPriorityBuilding(owner.units.robotCommands, 1, true);
			AddPriorityBuilding(owner.units.guardPosts, int.MaxValue);
			AddPriorityBuilding(owner.units.arachnidFactories, 1);
			AddPriorityBuilding(owner.units.spaceports, 1);
			AddPriorityBuilding(owner.units.observatories, 1, true);
			AddPriorityBuilding(owner.units.meteorDefenses, 0);

			// Leave storages disabled as they will keep metals
			//AddPriorityBuilding(owner.units.commonStorages, 0);
			//AddPriorityBuilding(owner.units.rareStorages, 0);

			// Unneeded structures
			AddPriorityBuilding(owner.units.consumerFactories, 0, true);
			AddPriorityBuilding(owner.units.garages, 0, true);
			AddPriorityBuilding(owner.units.tradeCenters, 0, true);
			AddPriorityBuilding(owner.units.basicLabs, 0, true);
		}

		// Returns remaining capacity
		private int AddToCapacity(int neededCapacity, map_id buildingType, List<UnitEx> buildings)
		{
			UnitInfo info = new UnitInfo(buildingType);
			int capacityPerUnit = info.GetProductionCapacity(owner.player.playerID);
			int neededUnits = neededCapacity / capacityPerUnit;
			if (neededCapacity % capacityPerUnit > 0)
				++neededUnits;

			int oldCount = m_StructurePriority.Count;
			AddPriorityBuilding(buildings, neededUnits, true);
			int unitsAvailable = m_StructurePriority.Count - oldCount;

			neededUnits -= unitsAvailable;
			neededCapacity = neededUnits * capacityPerUnit;

			return neededCapacity;
		}

		private void AddPriorityBuilding(List<UnitEx> buildings, int needed, bool markExtrasAsUseless=false)
		{
			for (int i=0; i < buildings.Count; ++i)
			{
				UnitEx building = buildings[i];

				if (!CanBeActivated(building))
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
		private bool CanBeActivated(UnitEx building)
		{
			// Cannot be infected
			if (building.IsInfected())
				return false;

			// Must not be under construction
			switch (building.GetLastCommand())
			{
				case CommandType.ctMoDevelop:
				case CommandType.ctMoUnDevelop:
					return false;
			}

			// Cannot have critical damage
			UnitInfo info = building.GetUnitInfo();
			float hitPerc = 1 - (building.GetDamage() / (float)info.GetHitPoints(owner.player.playerID));

			if (hitPerc < 0.25f)
				return false;

			// If doesn't need tube, it's ready to go
			if (!BuildStructureTask.NeedsTube(building.GetUnitType()) || building.GetUnitType() == map_id.GuardPost)
				return true;

			// Cannot be disconnected from CC
			return owner.commandGrid.ConnectsTo(building.GetRect());
		}

		/// <summary>
		/// Attempts to activate all structures in priority order. Assigns scientists to research and workers to training.
		/// </summary>
		private void UpdateActivations()
		{
			// Deactivate crippled structures
			foreach (UnitEx building in m_CrippledStructures)
				building.DoIdle();

			m_AvailableWorkers = owner.player.GetNumAvailableWorkers();
			m_AvailableScientists = owner.player.GetNumAvailableScientists();

			// Pull all scientists off of labs
			m_AvailableScientists += RemoveScientistsFromLab(owner.units.basicLabs, 9999);
			m_AvailableScientists += RemoveScientistsFromLab(owner.units.standardLabs, 9999);
			m_AvailableScientists += RemoveScientistsFromLab(owner.units.advancedLabs, 9999);

			List<UnitEx> lowPriorityAndUseless = new List<UnitEx>(m_LowPriorityStructures);
			lowPriorityAndUseless.AddRange(m_UselessStructures);

			// Activate high priority structures
			ActivateList(m_StructurePriority, lowPriorityAndUseless, true);

			// Assign all spare scientists to research
			AddScientistsToLabs(owner.units.advancedLabs, lowPriorityAndUseless);
			AddScientistsToLabs(owner.units.standardLabs, lowPriorityAndUseless);
			AddScientistsToLabs(owner.units.basicLabs, lowPriorityAndUseless);

			// Assign workers to training, but only if no scientists are assigned as workers
			if (owner.player.GetNumScientistsAsWorkers() == 0)
				AddWorkersToTraining();

			// Activate low priority structures
			ActivateList(m_LowPriorityStructures, m_UselessStructures, false);
			ActivateList(m_UselessStructures, new List<UnitEx>(), false);
		}

		private void ActivateList(List<UnitEx> buildingsToActivate, List<UnitEx> buildingsToIdle, bool canUseScientistsAsWorkers)
		{
			// When freeing labor, we want to start with buildingsToIdle, and include not yet processed buildingsToActivate
			List<UnitEx> allBuildings = new List<UnitEx>(buildingsToActivate);
			allBuildings.AddRange(buildingsToIdle);

			for (int i=0; i < buildingsToActivate.Count; ++i)
			{
				UnitEx building = buildingsToActivate[i];

				// Attempt to activate structure with currently available labor
				if (!ActivateStructure(building, canUseScientistsAsWorkers))
				{
					// Get needed workers and scientists for building
					UnitInfo info = building.GetUnitInfo();
					int workersNeeded = info.GetWorkersRequired(owner.player.playerID);
					int scientistsNeeded = info.GetScientistsRequired(owner.player.playerID);

					workersNeeded -= m_AvailableWorkers;
					scientistsNeeded -= m_AvailableScientists;

					// Attempt to free labor
					if (FreeLabor(allBuildings, i, workersNeeded, scientistsNeeded, canUseScientistsAsWorkers))
					{
						// Labor freed, try again
						--i;
						continue;
					}
					else
					{
						// Couldn't free up labor, idle structure
						if (building.GetLastCommand() != CommandType.ctMoIdle)
							building.DoIdle();
					}
				}
			}
		}

		/// <summary>
		/// Activates a structure if it isn't already.
		/// </summary>
		/// <returns>False if there are not enough workers.</returns>
		private bool ActivateStructure(UnitEx building, bool canUseScientistsAsWorkers)
		{
			UnitInfo info = building.GetUnitInfo();

			int workersReq = info.GetWorkersRequired(owner.player.playerID);
			int scientistsReq = info.GetScientistsRequired(owner.player.playerID);

			// Does building already meet labor requirements?
			bool hasWorkers = workersReq == 0 || building.HasWorkers();
			bool hasScientists = scientistsReq == 0 || building.HasScientists();

			// Don't need to do anything if the building has labor
			if (hasWorkers && hasScientists)
			{
				if (!building.IsEnabled())
					building.DoUnIdle();

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
			building.DoUnIdle();

			m_AvailableWorkers -= (workersReq - scientistsAsWorkers);
			m_AvailableScientists -= (scientistsReq + scientistsAsWorkers);

			return true;
		}

		/// <summary>
		/// Attempts to add scientists to labs until maxed.
		/// Will use available scientists first and then buildingsToIdle to get the rest.
		/// </summary>
		private void AddScientistsToLabs(List<UnitEx> labs, List<UnitEx> buildingsToIdle)
		{
			for (int i=0; i < labs.Count; ++i)
			{
				UnitEx lab = labs[i];

				if (!AddScientistsToLab(lab))
				{
					TechInfo info = Research.GetTechInfo(lab.GetLabCurrentTopic());
					int maxScientists = info.GetMaxScientists();
					int assignedScientists = lab.GetLabScientistCount();

					if (FreeLabor(buildingsToIdle, -1, 0, maxScientists-assignedScientists, false))
					{
						// Labor freed, try again
						--i;
						continue;
					}
				}
			}
		}

		// Returns true if lab is at max scientists
		private bool AddScientistsToLab(UnitEx lab)
		{
			if (lab.IsEnabled() && lab.IsBusy())
			{
				// Get max assigned
				TechInfo info = Research.GetTechInfo(lab.GetLabCurrentTopic());
				int maxScientists = info.GetMaxScientists();
				int scientistsAssigned = lab.GetLabScientistCount();

				int scientistsNeeded = maxScientists - scientistsAssigned;
				int scientistsToAdd = Math.Min(scientistsNeeded, m_AvailableScientists);

				lab.SetLabScientistCount(scientistsAssigned + scientistsToAdd);

				m_AvailableScientists -= scientistsToAdd;

				return scientistsAssigned + scientistsToAdd >= maxScientists;
			}

			return true;
		}

		private int AddWorkersToTraining()
		{
			// Assign workers to training (20% of population)
			int workersToAssign = 0;
			int neededScientists = (int)(owner.player.TotalPopulation() * 0.2f) - owner.player.Scientists();
			if (neededScientists > 0)
				workersToAssign = Math.Min(neededScientists, m_AvailableWorkers);

			int assignedWorkers = 0;

			if (workersToAssign == 0)
				return 0;

			// Find university to assign to
			foreach (UnitEx university in owner.units.universities)
			{
				if (university.IsEnabled() && !university.IsBusy())
				{
					int numToAssign = Math.Min(workersToAssign, 10);

					university.DoTrainScientists(numToAssign);

					assignedWorkers += numToAssign;
					workersToAssign -= numToAssign;
					if (workersToAssign == 0)
						break;
				}
			}

			return assignedWorkers;
		}

		// Returns true if labor has successfully be freed
		private bool FreeLabor(List<UnitEx> buildingsForLabor, int cutoffIndex, int workersNeeded, int scientistsNeeded, bool canUseScientistsAsWorkers)
		{
			int workersAdded = 0;
			int scientistsAdded = 0;

			List<UnitEx> buildingsToIdle = new List<UnitEx>();

			// Free labor by idling low priority structures
			for (int i=buildingsForLabor.Count-1; i > cutoffIndex; --i)
			{
				UnitEx building = buildingsForLabor[i];
				UnitInfo info = building.GetUnitInfo();

				int workersInBuilding = info.GetWorkersRequired(owner.player.playerID);
				int scientistsInBuilding = info.GetScientistsRequired(owner.player.playerID);

				bool doIdle = false;

				// Structure does not use labor
				if (workersInBuilding == 0 && scientistsInBuilding == 0)
					continue;

				// Can't idle busy structures
				if (building.IsBusy())
					continue;

				// Must not be under construction or idle
				bool skip = false;
				switch (building.GetLastCommand())
				{
					case CommandType.ctMoDevelop:
					case CommandType.ctMoUnDevelop:
					case CommandType.ctMoIdle:
						skip = true;
						break;
				}

				if (skip)
					continue;

				if (!building.HasWorkers())
					workersInBuilding = 0;

				if (!building.HasScientists())
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

					foreach (UnitEx b in buildingsToIdle)
						b.DoIdle();

					return true;
				}
			}

			return false;
		}

		private int RemoveScientistsFromLab(IEnumerable<UnitEx> labs, int scientistsToRemove)
		{
			if (scientistsToRemove <= 0)
				return 0;

			int scientistsRemoved = 0;

			foreach (UnitEx lab in labs)
			{
				if (!lab.IsBusy())
					continue;

				int scientistsInLab = lab.GetLabScientistCount();

				int amountToRemove = Math.Min(scientistsToRemove, scientistsInLab-1); // Leave 1 scientist in the lab
				if (amountToRemove <= 0)
					continue;

				lab.SetLabScientistCount(scientistsInLab - amountToRemove);
				scientistsRemoved += amountToRemove;
			}

			return scientistsRemoved;
		}
	}
}
