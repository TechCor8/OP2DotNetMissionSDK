using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Managers
{
	public class MiningBaseState
	{
		public const int MaxMineDistanceToCC = 15;
		public const int MaxSmelterDistanceToCC = 15;
		public const int SmelterSaturationCount = 2;

		public const int MinTrucksPerSmelter = 2;
		public const int MaxTrucksPerSmelter = 5;
		public const int TilesPerTruck = 5;

		private PlayerInfo owner;

		private List<UnitEx> m_AllBeacons = new List<UnitEx>();
		private List<UnitEx> m_AllMines = new List<UnitEx>();
		private List<UnitEx> m_AllSmelters = new List<UnitEx>();

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
			m_AllMines.Clear();
			m_AllSmelters.Clear();

			miningBases.Clear();

			GetAllMiningBeacons();

			m_AllMines.AddRange(owner.units.commonOreMines);
			m_AllMines.AddRange(owner.units.rareOreMines);

			m_AllSmelters.AddRange(owner.units.commonOreSmelters);
			m_AllSmelters.AddRange(owner.units.rareOreSmelters);

			// Create mining base for each CC
			foreach (UnitEx cc in owner.units.commandCenters)
			{
				MiningBase miningBase = new MiningBase(cc, m_AllBeacons, m_AllMines, m_AllSmelters);
				
				miningBases.Add(miningBase);
			}
		}

		private void GetAllMiningBeacons()
		{
			m_AllBeacons.Clear();

			// Get all mining beacons
			foreach (UnitEx beacon in new PlayerUnitEnum(6))
			{
				map_id unitType = beacon.GetUnitType();

				if (unitType != map_id.MiningBeacon && unitType != map_id.MagmaVent)
					continue;

				m_AllBeacons.Add(beacon);
			}
		}
	}

	public class MiningSite
	{
		public UnitEx beacon;
		public UnitEx mine;

		public List<MiningSmelter> smelters = new List<MiningSmelter>();
		
		public MiningSite(UnitEx beacon, UnitEx mine)
		{
			this.beacon = beacon;
			this.mine = mine;
		}
	}

	public class MiningSmelter
	{
		public UnitEx smelter;

		public List<UnitEx> trucks = new List<UnitEx>();

		public int desiredTruckCount;


		public MiningSmelter(UnitEx smelter, int desiredTruckCount)
		{
			this.smelter = smelter;
			this.desiredTruckCount = desiredTruckCount;
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

		public List<MiningSite> miningSites;


		public MiningBase(UnitEx cc, List<UnitEx> beacons, List<UnitEx> mines, List<UnitEx> smelters)
		{
			commandCenter = cc;

			miningSites = new List<MiningSite>();

			// Move closest beacons to CC
			for (int i=0; i < beacons.Count; ++i)
			{
				if (beacons[i].GetPosition().GetDiagonalDistance(cc.GetPosition()) <= MiningBaseState.MaxMineDistanceToCC)
				{
					// Get mine on beacon
					UnitEx mine = mines.Find((UnitEx unit) => unit.GetPosition().Equals(beacons[i].GetPosition()));

					miningSites.Add(new MiningSite(beacons[i], mine));
					beacons.RemoveAt(i--);
				}
			}

			// Sort by distance to command center
			miningSites.Sort((a,b) => a.beacon.GetPosition().GetDiagonalDistance(cc.GetPosition()).CompareTo(b.beacon.GetPosition().GetDiagonalDistance(cc.GetPosition())));

			// Get smelters near command center
			List<UnitEx> ccSmelters = smelters.FindAll((UnitEx smelter) => smelter.GetPosition().GetDiagonalDistance(cc.GetPosition()) <= MiningBaseState.MaxSmelterDistanceToCC);

			foreach (MiningSite site in miningSites)
			{
				if (site.mine == null) continue;

				// Add closest smelters until mine site is saturated
				while (site.smelters.Count < MiningBaseState.SmelterSaturationCount && ccSmelters.Count > 0)
				{
					int distance;
					int closestIndex = GetClosestUnitIndex(site.mine.GetPosition(), ccSmelters, out distance);

					UnitEx smelter = ccSmelters[closestIndex];

					// Calculate desiredTruckCount
					int desiredTruckCount = distance / MiningBaseState.TilesPerTruck;

					if (desiredTruckCount < MiningBaseState.MinTrucksPerSmelter)
						desiredTruckCount = MiningBaseState.MinTrucksPerSmelter;
					if (desiredTruckCount > MiningBaseState.MaxTrucksPerSmelter)
						desiredTruckCount = MiningBaseState.MaxTrucksPerSmelter;

					// Add smelter
					site.smelters.Add(new MiningSmelter(smelter, desiredTruckCount));
					ccSmelters.RemoveAt(closestIndex);
				}
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
