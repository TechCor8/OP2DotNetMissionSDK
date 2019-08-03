using DotNetMissionSDK.HFL;
using System;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.CombatGroups
{
	public enum CombatGroupType { Assault, Harass, Bomber, Capture }

	/// <summary>
	/// Base class for AI combat groups used by CombatManager.
	/// </summary>
	public abstract class CombatGroup
	{
		public class UnitWithWeapon
		{
			public map_id unit		{ get; private set; }
			public map_id weapon	{ get; private set; }

			public UnitWithWeapon(map_id unit, map_id weapon)
			{
				this.unit = unit;
				this.weapon = weapon;
			}
		}

		private UnitWithWeapon[] m_DesiredUnits = new UnitWithWeapon[0];

		protected Player m_Owner;

		/// <summary>
		/// The units assigned to this group.
		/// The array is filled based on the types returned by GetDesiredUnits().
		/// Slots can be null, indicating that a unit has not been assigned.
		/// </summary>
		protected UnitEx[] m_Units = new UnitEx[0];

		/// <summary>
		/// The combat group's type represented as an enum.
		/// All derived classes must have a unique type.
		/// </summary>
		public abstract CombatGroupType groupType { get; }


		public CombatGroup(Player owner)
		{
			m_Owner = owner;

			// Initialize unit array
			GetDesiredUnits();
		}

		/// <summary>
		/// Gets the units this group needs to reach capacity.
		/// </summary>
		public UnitWithWeapon[] GetDesiredUnits()
		{
			m_DesiredUnits = GetDesiredUnitsInSubType();

			// Create unit array to fit desired units
			if (m_Units.Length != m_DesiredUnits.Length)
			{
				if (m_Units.Length > 0)
					Console.WriteLine("CombatGroup::GetDesiredUnits(): DesiredUnits.Length changed size. This will cause the group to lose retained units!");

				m_Units = new UnitEx[m_DesiredUnits.Length];
			}

			return m_DesiredUnits;
		}

		protected abstract UnitWithWeapon[] GetDesiredUnitsInSubType();

		/// <summary>
		/// Called by CombatManager to remove units from unassigned list that are already assigned.
		/// </summary>
		public void RetainUnits(List<UnitEx> unassignedUnits)
		{
			for (int i=0; i < m_Units.Length; ++i)
				m_Units[i] = GetRetainedUnit(unassignedUnits, m_Units[i]);
		}

		// Removes unitToRetain from the unassignedUnits list and returns it. If unitToRetain is not found in the list, returns null.
		// This function is used to prevent units from inside the group from being reassigned to a different group.
		private UnitEx GetRetainedUnit(List<UnitEx> unassignedUnits, UnitEx unitToRetain)
		{
			if (unitToRetain == null)
				return null;

			int index = unassignedUnits.FindIndex((UnitEx unit) => unit.GetStubIndex() == unitToRetain.GetStubIndex());

			// No unit found
			if (index < 0)
				return null;

			// Unit found. Remove from unassigned list.
			UnitEx result = unassignedUnits[index];
			unassignedUnits.RemoveAt(index);

			return result;
		}

		/// <summary>
		/// Called by CombatManager to populate group with units.
		/// </summary>
		public void AssignUnits(List<UnitEx> unassignedUnits)
		{
			for (int i=0; i < m_Units.Length; ++i)
				m_Units[i] = GetUnassignedUnit(unassignedUnits, m_DesiredUnits[i].unit, m_DesiredUnits[i].weapon);
		}

		// Assigns a unit to this group from the unassigned list.
		// This function is used to assign new units to the group.
		private UnitEx GetUnassignedUnit(List<UnitEx> unassignedUnits, map_id unitType, map_id weaponType)
		{
			int index = unassignedUnits.FindIndex((UnitEx unit) =>
			{
				 map_id thisUnitType = unit.GetUnitType();
				map_id thisWeapon = unit.GetWeapon();

				if (unitType == map_id.Any)
				{
					if (weaponType == map_id.Any)
						return true;
					else if (weaponType == thisWeapon)
						return true;
				}
				else if (unitType == thisUnitType)
				{
					if (weaponType == map_id.Any)
						return true;
					else if (weaponType == thisWeapon)
						return true;
				}

				return false;
			});

			// No unit found
			if (index < 0)
				return null;

			// Unit found. Remove from unassigned list.
			UnitEx result = unassignedUnits[index];
			unassignedUnits.RemoveAt(index);

			return result;
		}

		/// <summary>
		/// Called to update combat group.
		/// </summary>
		public abstract void Update();
	}
}
