using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;

namespace DotNetMissionSDK.State.Snapshot.Units
{
	/// <summary>
	/// Contains immutable unit state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class FactoryState : StructureState
	{
		private map_id[] m_BayCargo;
		private map_id[] m_BayCargoWeapon;

		public int storageBayCount													{ get; private set; }

		public map_id GetFactoryCargo(int bay)										{ return m_BayCargo[bay];											}
		public map_id GetFactoryCargoWeapon(int bay)								{ return m_BayCargoWeapon[bay];										}

		public bool HasEmptyBay()													{ return GetBayWithCargo(map_id.None) >= 0;							}
		public bool HasBayWithCargo(map_id cargoType)								{ return GetBayWithCargo(cargoType) >= 0;							}

		public bool hasOccupiedBay													{ get; private set; }

		/// <summary>
		/// Returns the bay index that contains cargo type.
		/// Returns -1 if cargo type is not found.
		/// </summary>
		/// <param name="cargoType">The cargo type to search for.</param>
		public int GetBayWithCargo(map_id cargoType)
		{
			for (int i=0; i < storageBayCount; ++i)
			{
				if (GetFactoryCargo(i) == cargoType)
					return i;
			}

			return -1;
		}

		/// <summary>
		/// Creates an immutable state from UnitEx.
		/// </summary>
		/// <param name="structureInfo">The info that applies to this unit.</param>
		/// <param name="weaponInfo">The info that applies to this unit's weapon.</param>
		public FactoryState(UnitEx unit, StructureInfo structureInfo, WeaponInfo weaponInfo) : base(unit, structureInfo, weaponInfo)
		{
			storageBayCount = unit.GetUnitInfo().GetNumStorageBays(ownerID);

			m_BayCargo			= new map_id[storageBayCount];
			m_BayCargoWeapon	= new map_id[storageBayCount];

			for (int i=0; i < m_BayCargo.Length; ++i)
			{
				m_BayCargo[i]		= unit.GetFactoryCargo(i);
				m_BayCargoWeapon[i] = unit.GetFactoryCargoWeapon(i);
			}

			hasOccupiedBay		= unit.HasOccupiedBay();
		}
	}
}
