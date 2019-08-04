using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetMissionSDK.AI.Combat.Groups
{
	public enum VehicleGroupType { Assault, Harass, Bomber, Capture }

	/// <summary>
	/// Base class for AI vehicle groups.
	/// </summary>
	public abstract class VehicleGroup
	{
		/// <summary>
		/// Represents a combined unit and weapon type.
		/// </summary>
		public class UnitWithWeaponType
		{
			public map_id unit		{ get; private set; }
			public map_id weapon	{ get; private set; }

			public UnitWithWeaponType(map_id unit, map_id weapon)
			{
				this.unit = unit;
				this.weapon = weapon;
			}
		}

		/// <summary>
		/// Represents a unit slot in the vehicle group.
		/// </summary>
		public class UnitSlot
		{
			private List<UnitWithWeaponType> m_SupportedSlotTypes = new List<UnitWithWeaponType>();

			public UnitEx unitInSlot											{ get; set; }
			public ReadOnlyCollection<UnitWithWeaponType> supportedSlotTypes	{ get { return m_SupportedSlotTypes.AsReadOnly();	} }

			public UnitSlot(UnitWithWeaponType[] supportedSlotTypes)
			{
				m_SupportedSlotTypes = new List<UnitWithWeaponType>(supportedSlotTypes);
			}
		}

		protected PlayerInfo m_Owner;
		protected ThreatZone m_ThreatZone;

		private UnitSlot[] m_UnitSlots = new UnitSlot[0];

		/// <summary>
		/// The vehicle group's type represented as an enum.
		/// All derived classes must have a unique type.
		/// </summary>
		public abstract VehicleGroupType groupType { get; }

		/// <summary>
		/// Returns true if the group has an empty slot.
		/// </summary>
		public bool HasEmptySlot()
		{
			for (int i=0; i < m_UnitSlots.Length; ++i)
			{
				if (m_UnitSlots[i].unitInSlot == null)
					return true;
			}

			return false;
		}


		public VehicleGroup(PlayerInfo owner, ThreatZone zone)
		{
			m_Owner = owner;
			m_ThreatZone = zone;

			m_UnitSlots = GetUnitSlots();
		}

		protected abstract UnitSlot[] GetUnitSlots();

		/// <summary>
		/// Returns the unassigned slots in this group.
		/// </summary>
		public UnitSlot[] GetUnassignedSlots()
		{
			List<UnitSlot> unassignedSlots = new List<UnitSlot>();
			for (int i=0; i < m_UnitSlots.Length; ++i)
			{
				if (m_UnitSlots[i].unitInSlot == null)
					unassignedSlots.Add(m_UnitSlots[i]);
			}

			return unassignedSlots.ToArray();
		}

		/// <summary>
		/// Returns true if the unit list can completely fill the group.
		/// </summary>
		public bool CanFillGroup(IEnumerable<UnitEx> units)
		{
			List<UnitEx> unassignedUnits = new List<UnitEx>(units);

			for (int i=0; i < m_UnitSlots.Length; ++i)
			{
				// Skip slots that have already been assigned
				if (m_UnitSlots[i].unitInSlot != null)
					continue;

				bool didFillSlot = false;
				for (int j=0; j < unassignedUnits.Count; ++j)
				{
					UnitEx unit = unassignedUnits[j];

					if (FitsInSlot(i, unit.GetUnitType(), unit.GetWeapon()))
					{
						didFillSlot = true;
						unassignedUnits.RemoveAt(j);
						break;
					}
				}

				if (!didFillSlot)
					return false;
			}

			return true;
		}
		
		/// <summary>
		/// Assigns a unit to the group.
		/// </summary>
		/// <param name="unit">The unit to assign.</param>
		/// <returns>True, if the unit was assigned. False, if the unit is the wrong type.</returns>
		public bool AssignUnit(UnitEx unit)
		{
			int index = GetEmptySlot(unit.GetUnitType(), unit.GetWeapon());

			if (index >= 0)
			{
				m_UnitSlots[index].unitInSlot = unit;
				return true;
			}

			return false;
		}

		private int GetEmptySlot(map_id unitType, map_id weapon)
		{
			for (int i=0; i < m_UnitSlots.Length; ++i)
			{
				// Skip slots that have already been assigned
				if (m_UnitSlots[i].unitInSlot != null)
					continue;

				if (FitsInSlot(i, unitType, weapon))
					return i;
			}

			return -1;
		}

		private bool FitsInSlot(int index, map_id unitType, map_id weapon)
		{
			List<UnitWithWeaponType> slotTypes = new List<UnitWithWeaponType>(m_UnitSlots[index].supportedSlotTypes);

			return slotTypes.Find((UnitWithWeaponType type) => type.unit == unitType && type.weapon == weapon) != null;
		}

		/// <summary>
		/// Called to update vehicle group.
		/// </summary>
		public abstract void Update();

		protected UnitWithWeaponType[] GetStandardCombatTypePriority()
		{
			return m_StandardCombatUnitPriority;
		}

		private UnitWithWeaponType[] m_StandardCombatUnitPriority = new UnitWithWeaponType[]
		{
			new UnitWithWeaponType(map_id.Tiger, map_id.ThorsHammer),
			new UnitWithWeaponType(map_id.Tiger, map_id.ESG),
			new UnitWithWeaponType(map_id.Tiger, map_id.RPG),
			new UnitWithWeaponType(map_id.Tiger, map_id.RailGun),
			new UnitWithWeaponType(map_id.Tiger, map_id.Microwave),
			new UnitWithWeaponType(map_id.Tiger, map_id.Laser),

			new UnitWithWeaponType(map_id.Panther, map_id.ThorsHammer),
			new UnitWithWeaponType(map_id.Panther, map_id.ESG),
			new UnitWithWeaponType(map_id.Panther, map_id.RPG),
			new UnitWithWeaponType(map_id.Panther, map_id.RailGun),
			new UnitWithWeaponType(map_id.Panther, map_id.Microwave),
			new UnitWithWeaponType(map_id.Panther, map_id.Laser),

			new UnitWithWeaponType(map_id.Lynx, map_id.ThorsHammer),
			new UnitWithWeaponType(map_id.Lynx, map_id.ESG),
			new UnitWithWeaponType(map_id.Lynx, map_id.RPG),
			new UnitWithWeaponType(map_id.Lynx, map_id.RailGun),
			new UnitWithWeaponType(map_id.Lynx, map_id.Microwave),
			new UnitWithWeaponType(map_id.Lynx, map_id.Laser),

			new UnitWithWeaponType(map_id.Scorpion, map_id.None),
		};
	}
}
