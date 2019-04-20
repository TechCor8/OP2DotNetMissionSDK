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

		public PlayerInfo owner { get; private set; }


		public LaborManager(PlayerInfo owner)
		{
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

			if (m_StructurePriority.Count > 2 &&
				m_StructurePriority[m_StructurePriority.Count-1].GetUnitType() == map_id.University &&
				m_StructurePriority[m_StructurePriority.Count-2].GetUnitType() == map_id.Nursery)
			{
				// Can grow labor force, include morale structures

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
				// All morale structures are low priority
				AddPriorityBuilding(owner.units.advancedResidences, 0);
				AddPriorityBuilding(owner.units.reinforcedResidences, 0);
				AddPriorityBuilding(owner.units.residences, 0);

				AddPriorityBuilding(owner.units.medicalCenters, 0);
				AddPriorityBuilding(owner.units.gorfs, 0);
				AddPriorityBuilding(owner.units.forums, 0);
				AddPriorityBuilding(owner.units.recreationFacilities, 0);
				AddPriorityBuilding(owner.units.dirts, 0);
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

			// Compile priority list
			m_StructurePriority.AddRange(m_LowPriorityStructures);
			m_StructurePriority.AddRange(m_UselessStructures);
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
					if (i < needed)
						m_StructurePriority.Add(building);
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

			// Cannot have critical damage
			UnitInfo info = building.GetUnitInfo();
			float hitPerc = 1 - (building.GetDamage() / (float)info.GetHitPoints(owner.player.playerID));

			if (hitPerc < 0.25f)
				return false;

			// If doesn't need tube, it's ready to go
			if (!BuildStructureTask.NeedsTube(building.GetUnitType()))
				return true;

			// Cannot be disconnected from CC
			return owner.commandGrid.ConnectsTo(building.GetRect());
		}

		private void UpdateActivations()
		{
			// Deactivate crippled structures
			foreach (UnitEx building in m_CrippledStructures)
				building.DoIdle();

			int lowestPriorityIndex = m_StructurePriority.Count-1;

			int availableWorkers = owner.player.GetNumAvailableWorkers();
			int availableScientists = owner.player.GetNumAvailableScientists();

			for (int i=0; i < m_StructurePriority.Count; ++i)
			{
				UnitEx building = m_StructurePriority[i];
				UnitInfo info = building.GetUnitInfo();

				int workersReq = info.GetWorkersRequired(owner.player.playerID);
				int scientistsReq = info.GetScientistsRequired(owner.player.playerID);

				bool hasWorkers = workersReq == 0 || building.HasWorkers();
				bool hasScientists = scientistsReq == 0 || building.HasScientists();

				// Don't need to do anything if the building has labor
				if (hasWorkers && hasScientists)
				{
					if (!building.IsEnabled())
						building.DoUnIdle();

					continue;
				}

				int scientistsAsWorkers = 0;

				// If there are not enough workers, pull from scientists
				if (availableWorkers < workersReq)
				{
					int workersNeeded = workersReq - availableWorkers;
					if (workersNeeded <= availableScientists)
						scientistsAsWorkers = workersNeeded;
				}

				// Not enough labor, find more labor or idle
				if (availableWorkers+scientistsAsWorkers < workersReq || availableScientists-scientistsAsWorkers < scientistsReq)
				{
					int workersAdded;
					int scientistsAdded;

					bool didFreeLabor = FreeLabor(lowestPriorityIndex, i, workersReq - availableWorkers, scientistsReq - availableScientists, out lowestPriorityIndex, out workersAdded, out scientistsAdded);

					availableWorkers += workersAdded;
					availableScientists += scientistsAdded;

					if (didFreeLabor)
					{
						// Try to activate again
						--i;
						continue;
					}
					else
					{
						// Couldn't free up labor
						if (building.GetLastCommand() != CommandType.ctMoIdle)
							building.DoIdle();

						continue;
					}
				}

				// Found labor, activate structure
				building.DoUnIdle();

				availableWorkers -= (workersReq - scientistsAsWorkers);
				availableScientists -= (scientistsReq + scientistsAsWorkers);
			}

			// Assign all spare scientists to research
			foreach (UnitEx lab in owner.units.advancedLabs)
				availableScientists -= AddScientistsToLab(lab, availableScientists);

			foreach (UnitEx lab in owner.units.standardLabs)
				availableScientists -= AddScientistsToLab(lab, availableScientists);

			foreach (UnitEx lab in owner.units.basicLabs)
				availableScientists -= AddScientistsToLab(lab, availableScientists);

			// Assign to training
			availableWorkers -= AddWorkersToTraining(availableWorkers);
		}

		// Returns number of scientists added
		private int AddScientistsToLab(UnitEx lab, int availableScientists)
		{
			if (lab.IsEnabled() && lab.IsBusy())
			{
				// Get max assigned
				TechInfo info = Research.GetTechInfo(lab.GetLabCurrentTopic());
				int maxScientists = info.GetMaxScientists();
				int scientistsAssigned = lab.GetLabScientistCount();

				int scientistsNeeded = maxScientists - scientistsAssigned;
				int scientistsToAdd = Math.Min(scientistsNeeded, availableScientists);

				lab.SetLabScientistCount(scientistsAssigned + scientistsToAdd);

				return scientistsToAdd;
			}

			return 0;
		}

		private int AddWorkersToTraining(int availableWorkers)
		{
			// Assign workers to training (20% of population)
			int workersToAssign = 0;
			int neededScientists = (int)(owner.player.TotalPopulation() * 0.2f) - owner.player.Scientists();
			if (neededScientists > 0)
				workersToAssign = Math.Min(neededScientists, availableWorkers);

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

		private bool FreeLabor(int lowestPriorityIndex, int cutoffIndex, int workersNeeded, int scientistsNeeded, out int newLowestPriorityIndex, out int workersAdded, out int scientistsAdded)
		{
			newLowestPriorityIndex = lowestPriorityIndex;
			workersAdded = 0;
			scientistsAdded = 0;

			// Free labor by pulling off research
			scientistsAdded += RemoveScientistsFromLab(owner.units.basicLabs, scientistsNeeded+workersNeeded);
			scientistsAdded += RemoveScientistsFromLab(owner.units.standardLabs, scientistsNeeded+workersNeeded - scientistsAdded);
			scientistsAdded += RemoveScientistsFromLab(owner.units.advancedLabs, scientistsNeeded+workersNeeded - scientistsAdded);

			// If labor needs met, return
			if (workersAdded+scientistsAdded >= workersNeeded+scientistsNeeded && scientistsAdded >= scientistsNeeded)
				return true;

			// Free labor by idling low priority structures
			for (int i=lowestPriorityIndex; i > cutoffIndex; --i)
			{
				newLowestPriorityIndex = i;

				UnitEx building = m_StructurePriority[i];
				UnitInfo info = building.GetUnitInfo();

				int workersReq = info.GetWorkersRequired(owner.player.playerID);
				int scientistsReq = info.GetScientistsRequired(owner.player.playerID);

				bool doIdle = false;

				// Can't disable busy structures
				if (building.IsBusy())
					continue;

				// Check if building has labor. If it does, idle it and collect the labor.
				if (building.HasWorkers())
				{
					workersAdded += workersReq;
					doIdle = true;
				}

				if (building.HasScientists())
				{
					scientistsAdded += scientistsReq;
					doIdle = true;
				}

				if (doIdle)
					building.DoIdle();

				// If labor needs met, return
				if (workersAdded+scientistsAdded >= workersNeeded+scientistsNeeded && scientistsAdded >= scientistsNeeded)
					return true;
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

		// Don't take scientists off research for low priority and useless structures
	}
}
