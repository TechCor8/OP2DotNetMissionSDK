using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Snapshot.Units;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetMissionSDK.State.Snapshot
{
	/// <summary>
	/// Contains immutable gaia (unowned unit) state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class GaiaState : IEnumerable<GaiaUnitState>
	{
		public ReadOnlyCollection<MiningBeaconState> miningBeacons	{ get; private set; }
		public ReadOnlyCollection<MiningBeaconState> magmaVents		{ get; private set; }
		public ReadOnlyCollection<GaiaUnitState> fumaroles			{ get; private set; }
		public ReadOnlyCollection<GaiaUnitState> wreckages			{ get; private set; }


		/// <summary>
		/// Creates an immutable gaia (unowned unit) state.
		/// </summary>
		public GaiaState()
		{
			List<MiningBeaconState> beacons = new List<MiningBeaconState>();
			List<MiningBeaconState> magmaVents = new List<MiningBeaconState>();
			List<GaiaUnitState> fumaroles = new List<GaiaUnitState>();
			List<GaiaUnitState> wreckages = new List<GaiaUnitState>();

			foreach (UnitEx unit in new PlayerUnitEnum(6))
			{
				switch (unit.GetUnitType())
				{
					case map_id.MiningBeacon:		beacons.Add(new MiningBeaconState(unit));		break;
					case map_id.MagmaVent:			magmaVents.Add(new MiningBeaconState(unit));	break;
					case map_id.Fumarole:			fumaroles.Add(new GaiaUnitState(unit));			break;

					case map_id.mapWreckage:		wreckages.Add(new WreckageState(unit));			break;
				}
			}

			this.miningBeacons	= beacons.AsReadOnly();
			this.magmaVents		= magmaVents.AsReadOnly();
			this.fumaroles		= fumaroles.AsReadOnly();
			this.wreckages		= wreckages.AsReadOnly();
		}

		private IReadOnlyCollection<GaiaUnitState> GetListForType(map_id type)
		{
			switch (type)
			{
				case map_id.MiningBeacon:			return miningBeacons;
				case map_id.MagmaVent:				return magmaVents;
				case map_id.Fumarole:				return fumaroles;
				case map_id.mapWreckage:			return wreckages;
			}

			return null;
		}

		public GaiaUnitState GetClosestUnitOfType(map_id unitType, LOCATION position)
		{
			GaiaUnitState closestUnit = null;
			int closestDistance = 999999;

			foreach (GaiaUnitState unit in GetListForType(unitType))
			{
				// Closest distance
				int distance = position.GetDiagonalDistance(unit.position);
				if (distance < closestDistance)
				{
					closestUnit = unit;
					closestDistance = distance;
				}
			}

			return closestUnit;
		}

		public MiningBeaconState GetClosestMineOfType(BeaconType oreType, LOCATION position, bool includeMagmaVents)
		{
			MiningBeaconState closestUnit = null;
			int closestDistance = 999999;

			foreach (MiningBeaconState beacon in miningBeacons)
			{
				if (beacon.oreType != oreType)
					continue;

				// Closest distance
				int distance = position.GetDiagonalDistance(beacon.position);
				if (distance < closestDistance)
				{
					closestUnit = beacon;
					closestDistance = distance;
				}
			}

			if (includeMagmaVents)
			{
				MiningBeaconState closestMagmaVent = GetClosestUnitOfType(map_id.MagmaVent, position) as MiningBeaconState;
				if (closestMagmaVent != null)
				{
					int distance = position.GetDiagonalDistance(closestMagmaVent.position);
					if (distance < closestDistance)
					{
						closestUnit = closestMagmaVent;
						closestDistance = distance;
					}
				}
			}

			return closestUnit;
		}

		public IEnumerator<GaiaUnitState> GetEnumerator()
		{
			foreach (GaiaUnitState unit in miningBeacons)
				yield return unit;

			foreach (GaiaUnitState unit in magmaVents)
				yield return unit;

			foreach (GaiaUnitState unit in fumaroles)
				yield return unit;

			foreach (GaiaUnitState unit in wreckages)
				yield return unit;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
