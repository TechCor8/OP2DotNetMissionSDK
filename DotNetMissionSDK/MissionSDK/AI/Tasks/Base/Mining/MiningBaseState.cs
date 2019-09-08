using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Managers
{
	/// <summary>
	/// Contains metadata about the mining bases a bot uses.
	/// </summary>
	public class MiningBaseState
	{
		public const int MaxMineDistanceToCC = 15;
		public const int MaxSmelterDistanceToCC = 15;
		public const int SmelterSaturationCount = 2;

		public const int MinTrucksPerSmelter = 2;
		public const int MaxTrucksPerSmelter = 5;
		public const int TilesPerTruck = 2;

		private int ownerID;

		private List<StructureState> m_UnassignedCommandCenters = new List<StructureState>();
		private List<MiningBeaconState> m_UnassignedBeacons = new List<MiningBeaconState>();
		private List<StructureState> m_UnassignedMines = new List<StructureState>();
		private List<StructureState> m_UnassignedSmelters = new List<StructureState>();
		private List<CargoTruckState> m_UnassignedTrucks = new List<CargoTruckState>();

		public List<MiningBase> miningBases = new List<MiningBase>();


		public MiningBaseState(int ownerID)
		{
			this.ownerID = ownerID;
		}

		public void Update(StateSnapshot stateSnapshot)
		{
			UpdateMiningBases(stateSnapshot);
		}

		private void UpdateMiningBases(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Clear unassigned lists. We are going to recalculate them.
			m_UnassignedCommandCenters.Clear();
			m_UnassignedBeacons.Clear();
			m_UnassignedMines.Clear();
			m_UnassignedSmelters.Clear();
			m_UnassignedTrucks.Clear();

			GetAllMiningBeacons(stateSnapshot);

			// Add all units to the unassigned lists.
			m_UnassignedCommandCenters.AddRange(owner.units.commandCenters);

			m_UnassignedMines.AddRange(owner.units.commonOreMines);
			m_UnassignedMines.AddRange(owner.units.rareOreMines);
			m_UnassignedMines.AddRange(owner.units.magmaWells);

			m_UnassignedSmelters.AddRange(owner.units.commonOreSmelters);
			m_UnassignedSmelters.AddRange(owner.units.rareOreSmelters);

			m_UnassignedTrucks.AddRange(owner.units.cargoTrucks);

			// Don't assign trucks carrying things that don't involve mining
			m_UnassignedTrucks.RemoveAll((CargoTruckState truck) =>
			{
				switch (truck.cargoType)
				{
					case TruckCargo.CommonMetal:
					case TruckCargo.RareMetal:
					case TruckCargo.Food:
					case TruckCargo.Garbage:
						return true;
				}

				return false;
			});

			// Determine if we need to prioritize truck assignments
			StructureInfo vehicleFactoryInfo = owner.structureInfo[map_id.VehicleFactory];
			VehicleInfo truckInfo = owner.vehicleInfo[map_id.CargoTruck];

			int vehicleFactoryCost = owner.units.vehicleFactories.Count == 0 ? vehicleFactoryInfo.oreCost : 0;

			bool needCommonFirst = owner.ore < truckInfo.oreCost + vehicleFactoryCost;
			bool needRareFirst = owner.rareOre < truckInfo.rareOreCost && owner.units.vehicleFactories.Count > 0;

			// If we need resources for trucks, we need to reset truck assignments
			if (needCommonFirst || needRareFirst)
				ResetTruckAssignments();

			// Remove dead command centers from the current mining bases list
			miningBases.RemoveAll((MiningBase miningBase) => !m_UnassignedCommandCenters.Contains(miningBase.commandCenter));

			// Remove existing command centers from unassigned list
			foreach (MiningBase miningBase in miningBases)
				miningBase.Cull(m_UnassignedCommandCenters, m_UnassignedBeacons, m_UnassignedMines, m_UnassignedSmelters, m_UnassignedTrucks);

			// Create mining base for new command centers
			foreach (StructureState cc in m_UnassignedCommandCenters)
				miningBases.Add(new MiningBase(cc));

			// Update mine assignments
			foreach (MiningBase miningBase in miningBases)
				miningBase.UpdateMines(m_UnassignedBeacons, m_UnassignedMines);

			// Update smelter assignments
			foreach (StructureState smelter in m_UnassignedSmelters)
			{
				BeaconType oreType = smelter.unitType == map_id.CommonOreSmelter ? BeaconType.Common : BeaconType.Rare;

				// Get closest unsaturated mine to smelter
				MiningSite closestSite = GetClosestMiningSite(smelter.position, oreType);
				if (closestSite == null)
					continue;

				closestSite.AddSmelter(smelter);
			}

			// Assign trucks to mines based on ability to create more trucks.
			// If we can't build more trucks, mining the ore we need is more important.
			if (needCommonFirst || needRareFirst)
			{
				// Prioritize truck assignments
				foreach (MiningBase miningBase in miningBases)
				{
					foreach (MiningSite miningSite in miningBase.miningSites)
					{
						if (!needCommonFirst && miningSite.beacon.oreType == BeaconType.Common) continue;
						if (!needRareFirst && miningSite.beacon.oreType == BeaconType.Rare) continue;

						miningSite.AddTrucks(m_UnassignedTrucks);

						if (miningSite.beacon.oreType == BeaconType.Common) needCommonFirst = false;
						if (miningSite.beacon.oreType == BeaconType.Rare) needRareFirst = false;
					}
				}
			}

			// Update truck assignments
			foreach (MiningBase miningBase in miningBases)
			{
				foreach (MiningSite miningSite in miningBase.miningSites)
					miningSite.AddTrucks(m_UnassignedTrucks);
			}
		}

		private void ResetTruckAssignments()
		{
			foreach (MiningBase miningBase in miningBases)
			{
				foreach (MiningSite miningSite in miningBase.miningSites)
				{
					foreach (MiningSmelter smelter in miningSite.smelters)
						smelter.trucks.Clear();
				}
			}
		}

		private void GetAllMiningBeacons(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Get all mining beacons
			foreach (GaiaUnitState gaiaUnit in stateSnapshot.gaia)
			{
				if (gaiaUnit.unitType != map_id.MiningBeacon && gaiaUnit.unitType != map_id.MagmaVent)
					continue;

				if (gaiaUnit.unitType == map_id.MagmaVent && (!owner.CanColonyUseUnit(stateSnapshot, map_id.MagmaWell) || !owner.HasTechnologyForUnit(stateSnapshot, map_id.MagmaWell)))
					continue;

				m_UnassignedBeacons.Add((MiningBeaconState)gaiaUnit);
			}
		}

		private MiningSite GetClosestMiningSite(LOCATION position, BeaconType oreType)
		{
			MiningSite closestUnit = null;
			int closestDistance = 999999;

			foreach (MiningBase miningBase in miningBases)
			{
				foreach (MiningSite miningSite in miningBase.miningSites)
				{
					if (miningSite.mine == null) continue;
					if (miningSite.beacon.oreType != oreType) continue;
					if (miningSite.smelters.Count >= MiningBaseState.SmelterSaturationCount) continue;

					// Closest distance
					int distance = position.GetDiagonalDistance(miningSite.mine.position);
					if (distance < closestDistance)
					{
						closestUnit = miningSite;
						closestDistance = distance;
					}
				}
			}

			return closestUnit;
		}
	}

	/// <summary>
	/// Represents the mining bases for a bot.
	/// Each command center (base) has a control area. Beacons inside the control area are assigned to that base.
	/// Smelters inside the control area are assigned to the closest beacon, until saturated.
	/// </summary>
	public class MiningBase
	{
		public StructureState commandCenter		{ get; private set; }

		public List<MiningSite> miningSites = new List<MiningSite>();


		public MiningBase(StructureState cc)
		{
			commandCenter = cc;
		}

		public void Cull(List<StructureState> unassignedCCs, List<MiningBeaconState> unassignedBeacons, List<StructureState> unassignedMines, List<StructureState> unassignedSmelters, List<CargoTruckState> unassignedTrucks)
		{
			// Remove existing command centers from unassigned list
			int ccIndex = unassignedCCs.IndexOf(commandCenter);
			if (ccIndex >= 0)
			{
				commandCenter = unassignedCCs[ccIndex];
				unassignedCCs.RemoveAt(ccIndex);
			}

			// Remove dead mining sites? Only if beacons ever disappear

			foreach (MiningSite miningSite in miningSites)
				miningSite.Cull(unassignedBeacons, unassignedMines, unassignedSmelters, unassignedTrucks);
		}

		public void UpdateMines(List<MiningBeaconState> unassignedBeacons, List<StructureState> unassignedMines)
		{
			// Create mining site for unassigned beacons near this base
			for (int i=0; i < unassignedBeacons.Count; ++i)
			{
				if (unassignedBeacons[i].position.GetDiagonalDistance(commandCenter.position) <= MiningBaseState.MaxMineDistanceToCC)
				{
					miningSites.Add(new MiningSite(commandCenter, unassignedBeacons[i]));
					unassignedBeacons.RemoveAt(i--);
				}
			}

			// Sort by distance to command center
			miningSites.Sort((a,b) => a.beacon.position.GetDiagonalDistance(commandCenter.position).CompareTo(b.beacon.position.GetDiagonalDistance(commandCenter.position)));

			// Update mining site assignments
			foreach (MiningSite site in miningSites)
				site.UpdateMine(unassignedMines);
		}
	}

	public class MiningSite
	{
		private StructureState commandCenter;

		public MiningBeaconState beacon	{ get; private set; }
		public StructureState mine		{ get; private set; }

		public List<MiningSmelter> smelters = new List<MiningSmelter>();
		
		public MiningSite(StructureState commandCenter, MiningBeaconState beacon)
		{
			this.commandCenter = commandCenter;
			this.beacon = beacon;
		}

		public void Cull(List<MiningBeaconState> unassignedBeacons, List<StructureState> unassignedMines, List<StructureState> unassignedSmelters, List<CargoTruckState> unassignedTrucks)
		{
			// Remove dead mine
			if (mine != null && !unassignedMines.Contains(mine))
				mine = null;

			// Remove existing beacons and mines from unassigned list
			int beaconIndex = unassignedBeacons.IndexOf(beacon);
			if (beaconIndex >= 0)
			{
				beacon = unassignedBeacons[beaconIndex];
				unassignedBeacons.RemoveAt(beaconIndex);
			}
			if (mine != null)
			{
				int mineIndex = unassignedMines.IndexOf(mine);
				if (mineIndex >= 0)
				{
					mine = unassignedMines[mineIndex];
					unassignedMines.RemoveAt(mineIndex);
				}
			}
			else
			{
				// Smelters are no longer assigned when the mine is destroyed
				smelters.Clear();
				return;
			}

			// Remove dead smelters
			smelters.RemoveAll((MiningSmelter smelter) => !unassignedSmelters.Contains(smelter.smelter));

			foreach (MiningSmelter smelter in smelters)
				smelter.Cull(unassignedSmelters, unassignedTrucks);
		}

		public void UpdateMine(List<StructureState> unassignedMines)
		{
			if (mine == null)
			{
				// Get mine on beacon
				int mineIndex = unassignedMines.FindIndex((StructureState unit) => unit.position.Equals(beacon.position));

				if (mineIndex >= 0)
				{
					mine = unassignedMines[mineIndex];
					unassignedMines.RemoveAt(mineIndex);
				}
			}
		}

		public void AddSmelter(StructureState smelter)
		{
			if (mine == null || !mine.isEnabled)
				return;

			int distance = smelter.position.GetDiagonalDistance(mine.position);

			// Calculate desiredTruckCount
			int desiredTruckCount = distance / MiningBaseState.TilesPerTruck + 1;

			if (desiredTruckCount < MiningBaseState.MinTrucksPerSmelter)
				desiredTruckCount = MiningBaseState.MinTrucksPerSmelter;
			if (desiredTruckCount > MiningBaseState.MaxTrucksPerSmelter)
				desiredTruckCount = MiningBaseState.MaxTrucksPerSmelter;

			// Add smelter
			smelters.Add(new MiningSmelter(smelter, desiredTruckCount));
		}

		public void AddTrucks(List<CargoTruckState> unassignedTrucks)
		{
			// Don't add trucks to a site that can't provide ore
			if (mine == null || !mine.isEnabled)
				return;

			// Update smelter truck assignments
			foreach (MiningSmelter smelter in smelters)
				smelter.AddTrucks(unassignedTrucks);
		}

		private int GetClosestUnitIndex(LOCATION position, map_id unitType, List<StructureState> units, out int closestDistance)
		{
			int closestUnitIndex = -1;
			closestDistance = 999999;

			for (int i=0; i < units.Count; ++i)
			{
				StructureState unit = units[i];

				if (unit.unitType != unitType)
					continue;

				// Closest distance
				int distance = position.GetDiagonalDistance(unit.position);
				if (distance < closestDistance)
				{
					closestUnitIndex = i;
					closestDistance = distance;
				}
			}

			return closestUnitIndex;
		}
	}

	public class MiningSmelter
	{
		public StructureState smelter		{ get; private set; }

		public List<CargoTruckState> trucks = new List<CargoTruckState>();

		public int desiredTruckCount;


		public MiningSmelter(StructureState smelter, int desiredTruckCount)
		{
			this.smelter = smelter;
			this.desiredTruckCount = desiredTruckCount;
		}

		public void Cull(List<StructureState> unassignedSmelters, List<CargoTruckState> unassignedTrucks)
		{
			// Remove existing smelters from unassigned list
			int smelterIndex = unassignedSmelters.IndexOf(smelter);
			if (smelterIndex >= 0)
			{
				smelter = unassignedSmelters[smelterIndex];
				unassignedSmelters.RemoveAt(smelterIndex);
			}

			// Inactive smelter cannot have trucks
			if (!smelter.isEnabled)
				trucks.Clear();

			// Trucks are removed from mining routes when dead
			trucks.RemoveAll((CargoTruckState truck) => !unassignedTrucks.Contains(truck));

			// Remove existing trucks from unassigned list
			for (int i=0; i < trucks.Count; ++i)
			{
				CargoTruckState truck = trucks[i];

				int truckIndex = unassignedTrucks.IndexOf(truck);
				if (truckIndex >= 0)
				{
					trucks[i] = unassignedTrucks[truckIndex];
					unassignedTrucks.RemoveAt(truckIndex);
				}
			}
		}

		public void AddTrucks(List<CargoTruckState> unassignedTrucks)
		{
			// Inactive smelter cannot have trucks
			if (!smelter.isEnabled)
				return;

			// Add closest trucks to smelter route until saturated
			while (trucks.Count < desiredTruckCount && unassignedTrucks.Count > 0)
			{
				int index = GetClosestUnitIndex(smelter.position, unassignedTrucks, out _);

				trucks.Add(unassignedTrucks[index]);
				unassignedTrucks.RemoveAt(index);
			}
		}

		private int GetClosestUnitIndex(LOCATION position, List<CargoTruckState> units, out int closestDistance)
		{
			int closestUnitIndex = -1;
			closestDistance = 999999;

			for (int i=0; i < units.Count; ++i)
			{
				CargoTruckState unit = units[i];

				// Closest distance
				int distance = position.GetDiagonalDistance(unit.position);
				if (distance < closestDistance)
				{
					closestUnitIndex = i;
					closestDistance = distance;
				}
			}

			return closestUnitIndex;
		}
	}
}
