using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
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

		private PlayerInfo owner;

		private List<UnitEx> m_UnassignedCommandCenters = new List<UnitEx>();
		private List<UnitEx> m_UnassignedBeacons = new List<UnitEx>();
		private List<UnitEx> m_UnassignedMines = new List<UnitEx>();
		private List<UnitEx> m_UnassignedSmelters = new List<UnitEx>();
		private List<UnitEx> m_UnassignedTrucks = new List<UnitEx>();

		public List<MiningBase> miningBases = new List<MiningBase>();


		public MiningBaseState(PlayerInfo owner)
		{
			this.owner = owner;
		}

		public void Update()
		{
			UpdateMiningBases(owner);
		}

		private void UpdateMiningBases(PlayerInfo owner)
		{
			// Clear unassigned lists. We are going to recalculate them.
			m_UnassignedCommandCenters.Clear();
			m_UnassignedBeacons.Clear();
			m_UnassignedMines.Clear();
			m_UnassignedSmelters.Clear();
			m_UnassignedTrucks.Clear();

			GetAllMiningBeacons();

			// Add all units to the unassigned lists.
			m_UnassignedCommandCenters.AddRange(owner.units.commandCenters);

			m_UnassignedMines.AddRange(owner.units.commonOreMines);
			m_UnassignedMines.AddRange(owner.units.rareOreMines);

			m_UnassignedSmelters.AddRange(owner.units.commonOreSmelters);
			m_UnassignedSmelters.AddRange(owner.units.rareOreSmelters);

			m_UnassignedTrucks.AddRange(owner.units.cargoTrucks);

			// Don't assign trucks carrying things that don't involve mining
			m_UnassignedTrucks.RemoveAll((UnitEx truck) =>
			{
				switch (truck.GetCargoType())
				{
					case TruckCargo.CommonMetal:
					case TruckCargo.RareMetal:
					case TruckCargo.Food:
					case TruckCargo.Garbage:
						return true;
				}

				return false;
			});

			// Remove dead command centers from the current mining bases list
			miningBases.RemoveAll((MiningBase miningBase) => !miningBase.commandCenter.IsLive());

			// Remove existing command centers from unassigned list
			foreach (MiningBase miningBase in miningBases)
				miningBase.Cull(m_UnassignedCommandCenters, m_UnassignedBeacons, m_UnassignedMines, m_UnassignedSmelters, m_UnassignedTrucks);

			// Create mining base for new command centers
			foreach (UnitEx cc in m_UnassignedCommandCenters)
				miningBases.Add(new MiningBase(cc));

			// Update base assignments
			foreach (MiningBase miningBase in miningBases)
				miningBase.Update(m_UnassignedBeacons, m_UnassignedMines, m_UnassignedSmelters, m_UnassignedTrucks);
		}

		private void GetAllMiningBeacons()
		{
			// Get all mining beacons
			foreach (UnitEx beacon in new PlayerUnitEnum(6))
			{
				map_id unitType = beacon.GetUnitType();

				if (unitType != map_id.MiningBeacon && unitType != map_id.MagmaVent)
					continue;

				m_UnassignedBeacons.Add(beacon);
			}
		}
	}

	/// <summary>
	/// Represents the mining bases for a bot.
	/// Each command center (base) has a control area. Beacons inside the control area are assigned to that base.
	/// Smelters inside the control area are assigned to the closest beacon, until saturated.
	/// </summary>
	public class MiningBase
	{
		public UnitEx commandCenter			{ get; private set; }

		public List<MiningSite> miningSites = new List<MiningSite>();


		public MiningBase(UnitEx cc)
		{
			commandCenter = cc;
		}

		public void Cull(List<UnitEx> unassignedCCs, List<UnitEx> unassignedBeacons, List<UnitEx> unassignedMines, List<UnitEx> unassignedSmelters, List<UnitEx> unassignedTrucks)
		{
			// Remove existing command centers from unassigned list
			unassignedCCs.Remove(commandCenter);

			// Remove dead mining sites? Only if beacons ever disappear

			foreach (MiningSite miningSite in miningSites)
				miningSite.Cull(unassignedBeacons, unassignedMines, unassignedSmelters, unassignedTrucks);
		}

		public void Update(List<UnitEx> unassignedBeacons, List<UnitEx> unassignedMines, List<UnitEx> unassignedSmelters, List<UnitEx> unassignedTrucks)
		{
			// Create mining site for unassigned beacons near this base
			for (int i=0; i < unassignedBeacons.Count; ++i)
			{
				if (unassignedBeacons[i].GetPosition().GetDiagonalDistance(commandCenter.GetPosition()) <= MiningBaseState.MaxMineDistanceToCC)
				{
					miningSites.Add(new MiningSite(commandCenter, unassignedBeacons[i]));
					unassignedBeacons.RemoveAt(i--);
				}
			}

			// Sort by distance to command center
			miningSites.Sort((a,b) => a.beacon.GetPosition().GetDiagonalDistance(commandCenter.GetPosition()).CompareTo(b.beacon.GetPosition().GetDiagonalDistance(commandCenter.GetPosition())));

			// Update mining site assignments
			foreach (MiningSite site in miningSites)
				site.Update(unassignedMines, unassignedSmelters, unassignedTrucks);
		}
	}

	public class MiningSite
	{
		private UnitEx commandCenter;

		public UnitEx beacon			{ get; private set; }
		public UnitEx mine				{ get; private set; }

		public List<MiningSmelter> smelters = new List<MiningSmelter>();
		
		public MiningSite(UnitEx commandCenter, UnitEx beacon)
		{
			this.commandCenter = commandCenter;
			this.beacon = beacon;
		}

		public void Cull(List<UnitEx> unassignedBeacons, List<UnitEx> unassignedMines, List<UnitEx> unassignedSmelters, List<UnitEx> unassignedTrucks)
		{
			// Remove dead mine
			if (mine != null && !mine.IsLive())
				mine = null;

			// Remove existing beacons and mines from unassigned list
			unassignedBeacons.Remove(beacon);
			if (mine != null)
				unassignedMines.Remove(mine);
			else
			{
				// Smelters are no longer assigned when the mine is destroyed
				smelters.Clear();
				return;
			}

			// Remove dead smelters
			smelters.RemoveAll((MiningSmelter smelter) => !smelter.smelter.IsLive());

			foreach (MiningSmelter smelter in smelters)
				smelter.Cull(unassignedSmelters, unassignedTrucks);
		}

		public void Update(List<UnitEx> unassignedMines, List<UnitEx> unassignedSmelters, List<UnitEx> unassignedTrucks)
		{
			if (mine == null)
			{
				// Get mine on beacon
				int mineIndex = unassignedMines.FindIndex((UnitEx unit) => unit.GetPosition().Equals(beacon.GetPosition()));

				if (mineIndex >= 0)
				{
					mine = unassignedMines[mineIndex];
					unassignedMines.RemoveAt(mineIndex);
				}
			}

			if (mine == null)
				return;

			// Get smelters near command center
			List<UnitEx> ccSmelters = unassignedSmelters.FindAll((UnitEx smelter) => smelter.GetPosition().GetDiagonalDistance(commandCenter.GetPosition()) <= MiningBaseState.MaxSmelterDistanceToCC);

			map_id smelterType = beacon.GetOreType() == BeaconType.Common ? map_id.CommonOreSmelter : map_id.RareOreSmelter;

			// Add closest smelters until mine site is saturated
			while (smelters.Count < MiningBaseState.SmelterSaturationCount && ccSmelters.Count > 0)
			{
				int distance;
				int closestIndex = GetClosestUnitIndex(mine.GetPosition(), smelterType, ccSmelters, out distance);
				if (closestIndex < 0)
					break;

				UnitEx smelter = ccSmelters[closestIndex];

				// Calculate desiredTruckCount
				int desiredTruckCount = distance / MiningBaseState.TilesPerTruck + 1;

				if (desiredTruckCount < MiningBaseState.MinTrucksPerSmelter)
					desiredTruckCount = MiningBaseState.MinTrucksPerSmelter;
				if (desiredTruckCount > MiningBaseState.MaxTrucksPerSmelter)
					desiredTruckCount = MiningBaseState.MaxTrucksPerSmelter;

				// Add smelter
				smelters.Add(new MiningSmelter(smelter, desiredTruckCount));
				ccSmelters.RemoveAt(closestIndex);
				unassignedSmelters.Remove(smelter);
			}

			// Update smelter truck assignments
			foreach (MiningSmelter smelter in smelters)
				smelter.Update(unassignedTrucks);
		}

		private int GetClosestUnitIndex(LOCATION position, map_id unitType, List<UnitEx> units, out int closestDistance)
		{
			int closestUnitIndex = -1;
			closestDistance = 999999;

			for (int i=0; i < units.Count; ++i)
			{
				UnitEx unit = units[i];

				if (unit.GetUnitType() != unitType)
					continue;

				// Closest distance
				int distance = position.GetDiagonalDistance(unit.GetPosition());
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
		public UnitEx smelter			{ get; private set; }

		public List<UnitEx> trucks = new List<UnitEx>();

		public int desiredTruckCount;


		public MiningSmelter(UnitEx smelter, int desiredTruckCount)
		{
			this.smelter = smelter;
			this.desiredTruckCount = desiredTruckCount;
		}

		public void Cull(List<UnitEx> unassignedSmelters, List<UnitEx> unassignedTrucks)
		{
			// Remove existing smelters from unassigned list
			unassignedSmelters.Remove(smelter);

			// Trucks are removed from mining routes when dead
			trucks.RemoveAll((UnitEx truck) => !truck.IsLive());

			// Remove existing trucks from unassigned list
			foreach (UnitEx truck in trucks)
				unassignedTrucks.Remove(truck);
		}

		public void Update(List<UnitEx> unassignedTrucks)
		{
			// Add closest trucks to smelter route until saturated
			while (trucks.Count < desiredTruckCount && unassignedTrucks.Count > 0)
			{
				int index = GetClosestUnitIndex(smelter.GetPosition(), unassignedTrucks, out _);

				trucks.Add(unassignedTrucks[index]);
				unassignedTrucks.RemoveAt(index);
			}
		}

		private int GetClosestUnitIndex(LOCATION position, List<UnitEx> units, out int closestDistance)
		{
			int closestUnitIndex = -1;
			closestDistance = 999999;

			for (int i=0; i < units.Count; ++i)
			{
				UnitEx unit = units[i];

				// Closest distance
				int distance = position.GetDiagonalDistance(unit.GetPosition());
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
