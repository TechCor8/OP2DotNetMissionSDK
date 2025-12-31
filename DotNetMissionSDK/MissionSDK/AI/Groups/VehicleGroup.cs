using DotNetMissionSDK.Async;
using DotNetMissionSDK.Pathfinding;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.Units;
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

			public int unitInSlot												{ get; set; }
			public ReadOnlyCollection<UnitWithWeaponType> supportedSlotTypes	{ get { return m_SupportedSlotTypes.AsReadOnly();	} }

			public UnitSlot(UnitWithWeaponType[] supportedSlotTypes)
			{
				m_SupportedSlotTypes = new List<UnitWithWeaponType>(supportedSlotTypes);

				unitInSlot = -1;
			}
		}

		protected int m_OwnerID;
		private UnitSlot[] m_UnitSlots = new UnitSlot[0];

		private MAP_RECT m_FormationArea;
		
		public CombatZone combatZone	{ get; set;			}

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
				if (m_UnitSlots[i].unitInSlot == -1)
					return true;
			}

			return false;
		}


		public VehicleGroup(int ownerID, CombatZone zone)
		{
			m_OwnerID = ownerID;
			combatZone = zone;

			m_UnitSlots = GetUnitSlots(zone.strengthDesired);
		}

		protected abstract UnitSlot[] GetUnitSlots(int combatStrength);

		/// <summary>
		/// Returns the unassigned slots in this group.
		/// </summary>
		public UnitSlot[] GetUnassignedSlots()
		{
			List<UnitSlot> unassignedSlots = new List<UnitSlot>();
			for (int i=0; i < m_UnitSlots.Length; ++i)
			{
				if (m_UnitSlots[i].unitInSlot == -1)
					unassignedSlots.Add(m_UnitSlots[i]);
			}

			return unassignedSlots.ToArray();
		}

		/// <summary>
		/// Returns true if the unit list can completely fill the group.
		/// </summary>
		public bool CanFillGroup(IEnumerable<VehicleState> units)
		{
			List<VehicleState> unassignedUnits = new List<VehicleState>(units);

			for (int i=0; i < m_UnitSlots.Length; ++i)
			{
				// Skip slots that have already been assigned
				if (m_UnitSlots[i].unitInSlot != -1)
					continue;

				bool didFillSlot = false;
				for (int j=0; j < unassignedUnits.Count; ++j)
				{
					VehicleState unit = unassignedUnits[j];

					if (FitsInSlot(i, unit.unitType, unit.weapon))
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
		public bool AssignUnit(VehicleState unit)
		{
			int index = GetEmptySlot(unit.unitType, unit.weapon);

			if (index >= 0)
			{
				m_UnitSlots[index].unitInSlot = unit.unitID;
				return true;
			}

			return false;
		}

		private int GetEmptySlot(map_id unitType, map_id weapon)
		{
			for (int i=0; i < m_UnitSlots.Length; ++i)
			{
				// Skip slots that have already been assigned
				if (m_UnitSlots[i].unitInSlot != -1)
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
		/// NOTE: Must be called from main thread.
		/// </summary>
		public virtual void Update(StateSnapshot stateSnapshot)
		{
			ThreadAssert.MainThreadRequired();

			UpdateMovement(stateSnapshot);
		}

		protected void UpdateMovement(StateSnapshot stateSnapshot)
		{
			ThreadAssert.MainThreadRequired();

			m_FormationArea = GetFormationArea();

			bool goToCombatZone = false;

			// If any unit is in threat zone, or all units are in formation, send everyone to combat zone
			// Otherwise, send all units to the formation area
			goToCombatZone = IsAnyUnitInCombatZone() || AreAllUnitsInFormation();

			// Move units
			foreach (UnitSlot slot in m_UnitSlots)
			{
				if (slot.unitInSlot == -1)
					continue;

				Vehicle unit = GameState.GetUnit(slot.unitInSlot) as Vehicle;

				if (unit == null)
				{
					// Unit probably dead.
					continue;
				}

				if (goToCombatZone)
				{
					// Attack closest priority target
					if (combatZone.priorityTargets.Length > 0)
					{
						UnitState target = combatZone.GetClosestPriorityTarget(unit.GetPosition());
						Unit targetUnit = GameState.GetUnit(target.unitID);
						if (targetUnit != null)
							unit.DoAttack(targetUnit);
					}
					else if (!unit.isSearchingOrHasPath || !combatZone.bounds.Contains(unit.destination))
					{
						// Otherwise, move to random location in zone
						LOCATION targetPosition = combatZone.bounds.GetRandomPointInRect();
						unit.DoMoveWithPathfinder(stateSnapshot, targetPosition);
					}
				}
				else if (!IsInFormation(unit.GetPosition()) && (!unit.isSearchingOrHasPath || !IsInFormation(unit.destination)))
				{
					// Head to formation area
					LOCATION targetPosition = m_FormationArea.GetRandomPointInRect();
					int unitStrength = unit.GetUnitInfo().GetWeaponStrength();
					stateSnapshot.Retain();
					unit.DoMoveWithPathfinder((x,y) => GetTileFormationCost(x,y, unitStrength, stateSnapshot), IsInFormation, (path) => stateSnapshot.Release());
					//unit.DoMoveWithPathfinder(stateSnapshot, targetPosition);
				}
			}
		}

		private bool IsAnyUnitInCombatZone()
		{
			foreach (UnitSlot slot in m_UnitSlots)
			{
				if (slot.unitInSlot == -1)
					continue;

				Vehicle unit = GameState.GetUnit(slot.unitInSlot) as Vehicle;

				if (unit == null)
					continue;

				if (combatZone.Contains(unit.GetPosition()))
					return true;
			}

			return false;
		}

		private bool AreAllUnitsInFormation()
		{
			foreach (UnitSlot slot in m_UnitSlots)
			{
				if (slot.unitInSlot == -1)
					continue;

				Vehicle unit = GameState.GetUnit(slot.unitInSlot) as Vehicle;

				if (unit == null)
					continue;

				if (!IsInFormation(unit.GetPosition()))
					return false;
			}

			return true;
		}

		private bool IsInFormation(LOCATION position)
		{
			return m_FormationArea.Contains(position);
		}

		private bool IsInFormation(int x, int y)
		{
			return m_FormationArea.Contains(x, y);
		}

		// The formation area is a moving area that represents where the units should group to be in formation.
		private MAP_RECT GetFormationArea()
		{
			ThreadAssert.MainThreadRequired();

			LOCATION avgLocation = new LOCATION();
			int unitCount = 0;

			foreach (UnitSlot slot in m_UnitSlots)
			{
				if (slot.unitInSlot == -1)
					continue;

				Vehicle unit = GameState.GetUnit(slot.unitInSlot) as Vehicle;

				if (unit == null)
					continue;

				avgLocation += unit.GetPosition();
				++unitCount;
			}

			if (unitCount == 0)
			{
				// No units. No formation area.
				return new MAP_RECT();
			}

			avgLocation /= unitCount;

			MAP_RECT area = new MAP_RECT(avgLocation, new LOCATION(0,0));
			area.Inflate(5);

			area.ClipToMap();

			return area;
		}

		private int GetTileFormationCost(int x, int y, int unitStrength, StateSnapshot state)
		{
			PlayerState owner = state.players[m_OwnerID];

			if (!state.tileMap.IsTilePassable(x,y))
				return Pathfinder.Impassable;

			// Buildings and units are impassable
			if (state.unitMap.GetUnitOnTile(new LOCATION(x,y)) != null)
				return Pathfinder.Impassable;

			// Get tile movement speed cost
			int moveCost = state.tileMap.GetTileMovementCost(x,y);

			// Get enemy strength at tile
			int enemyStrength = 0;
			foreach (int enemyID in owner.enemyPlayerIDs)
				enemyStrength += state.strengthMap.GetPlayerStrength(enemyID, x,y);

			// If tile has enemy strength greater than our own, avoid it.
			if (enemyStrength > unitStrength)
				return Pathfinder.Impassable;
			else if (enemyStrength > 0)
				moveCost += 4; // Try to navigate around enemy, but go ahead and engage if no choice.

			return moveCost;
		}


		protected UnitWithWeaponType[] GetStandardCombatTypePriority()
		{
			return m_StandardCombatUnitPriority;
		}

		private static UnitWithWeaponType[] m_StandardCombatUnitPriority = new UnitWithWeaponType[]
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
