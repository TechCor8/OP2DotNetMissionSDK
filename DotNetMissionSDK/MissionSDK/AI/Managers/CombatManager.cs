using DotNetMissionSDK.AI.Combat;
using DotNetMissionSDK.AI.Combat.Groups;
using DotNetMissionSDK.Async;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;
using DotNetMissionSDK.Units;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Managers
{
	/// <summary>
	/// Organizes units and sends them into combat.
	/// </summary>
	public class CombatManager
	{
		private List<ThreatZone> m_DefenseZones = new List<ThreatZone>();		// Proximity, Defense, Mining
		private List<ThreatZone> m_EnemyBaseZones = new List<ThreatZone>();		// Enemy bases
		private List<ThreatZone> m_VulnerableZones = new List<ThreatZone>();	// Vulnerable Structure, Vulnerable Vehicle

		private List<VehicleGroup> m_CombatGroups = new List<VehicleGroup>();

		private bool m_IsProcessing;

		// Debugging
		private List<Unit> m_DebugMarkers = new List<Unit>();

		public BotPlayer botPlayer							{ get; private set; }
		public int ownerID									{ get; private set; }

		
		public CombatManager(BotPlayer botPlayer, int ownerID)
		{
			this.botPlayer = botPlayer;
			this.ownerID = ownerID;
		}

		public void Update(StateSnapshot stateSnapshot)
		{
			ThreadAssert.MainThreadRequired();

			if (m_IsProcessing)
				return;

			m_IsProcessing = true;

			PlayerState owner = stateSnapshot.players[ownerID];
			int combatGroupCapacity = m_CombatGroups.Capacity;

			stateSnapshot.Retain();

			AsyncPump.Run(() =>
			{
				m_DefenseZones.Clear();
				m_EnemyBaseZones.Clear();
				m_VulnerableZones.Clear();

				List<VehicleGroup> combatGroups = new List<VehicleGroup>(combatGroupCapacity);

				// Create threat zones
				CreateProximityZones(stateSnapshot, combatGroups);
				CreateVulnerableStructureZones(stateSnapshot, combatGroups);
				CreateVulnerableVehicleZones(stateSnapshot, combatGroups);
				CreateEnemyBaseZones(stateSnapshot, combatGroups);
				CreateDefenseZones(stateSnapshot, combatGroups);
				CreateMiningZones(stateSnapshot, combatGroups);

				PopulateCombatGroups(owner, combatGroups);

				return combatGroups;
			},
			(object returnState) =>
			{
				m_IsProcessing = false;

				m_CombatGroups = (List<VehicleGroup>)returnState;

				// Update debug markers
				if (ownerID == TethysGame.LocalPlayer())
				{
					foreach (Unit unit in m_DebugMarkers)
						unit.DoDeath();

					m_DebugMarkers.Clear();

					for (int i=0; i < m_CombatGroups.Count; ++i)
					{
						LOCATION position = m_CombatGroups[i].threatZone.bounds.position;
						position += m_CombatGroups[i].threatZone.bounds.size / 2;
						m_DebugMarkers.Add(TethysGame.PlaceMarker(position.x, position.y, MarkerType.Circle));
					}
				}

				// Update vehicle groups
				foreach (VehicleGroup group in m_CombatGroups)
					group.Update(stateSnapshot);

				stateSnapshot.Release();
			});
		}

		private void PopulateCombatGroups(PlayerState owner, List<VehicleGroup> combatGroups)
		{
			List<VehicleState> unassignedUnits = new List<VehicleState>(owner.units.lynx);
			unassignedUnits.AddRange(owner.units.panthers);
			unassignedUnits.AddRange(owner.units.tigers);
			unassignedUnits.AddRange(owner.units.spiders);
			unassignedUnits.AddRange(owner.units.scorpions);

			// Units already in an active threat zone must be assigned to that zone
			for (int i=0; i < unassignedUnits.Count; ++i)
			{
				VehicleState unit = unassignedUnits[i];

				foreach (VehicleGroup group in combatGroups)
				{
					if (group.threatZone.threatLevel != ThreatLevel.None && group.threatZone.Contains(unit.position))
					{
						if (group.AssignUnit(unit))
						{
							unassignedUnits.RemoveAt(i--);
							break;
						}
					}
				}
			}

			// Units are assigned to groups based on the prioritized order, as long as they can fill the group
			foreach (VehicleGroup group in combatGroups)
			{
				// If the zone's group can't be filled, skip this zone
				if (!group.CanFillGroup(unassignedUnits))
					continue;

				// Assign units to zone
				for (int i=0; i < unassignedUnits.Count; ++i)
				{
					if (group.AssignUnit(unassignedUnits[i]))
						unassignedUnits.RemoveAt(i--);
				}
			}

			// Remaining units are assigned where ever they can be placed
			foreach (VehicleGroup group in combatGroups)
			{
				// Assign units to zone
				for (int i=0; i < unassignedUnits.Count; ++i)
				{
					if (group.AssignUnit(unassignedUnits[i]))
						unassignedUnits.RemoveAt(i--);
				}
			}
		}

		/// <summary>
		/// Gets all unassigned slots in all combat groups.
		/// NOTE: Must be called from main thread.
		/// </summary>
		public List<VehicleGroup.UnitSlot> GetUnassignedSlots()
		{
			ThreadAssert.MainThreadRequired();

			List<VehicleGroup.UnitSlot> unassignedSlots = new List<VehicleGroup.UnitSlot>();

			// Get full list of unassigned slots
			foreach (VehicleGroup group in m_CombatGroups)
				unassignedSlots.AddRange(group.GetUnassignedSlots());

			return unassignedSlots;
		}

		// TODO:
		// PopulateCombatGroup - Units to populate must be able to reach zone without engaging in combat

		/// <summary>
		/// Creates threat zones for enemy combat units in close proximity to allied civilian units.
		/// </summary>
		private void CreateProximityZones(StateSnapshot stateSnapshot, List<VehicleGroup> combatGroups)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Check all self structures for nearby enemies
			foreach (UnitState unit in owner.units.GetStructures())
				CreateProximityZone(stateSnapshot, combatGroups, unit);

			// Check all self civilian units for nearby enemies
			foreach (UnitState unit in owner.units.GetVehicles())
			{
				if (unit.hasWeapon)
					continue;

				CreateProximityZone(stateSnapshot, combatGroups, unit);
			}

			foreach (int allyID in owner.allyPlayerIDs)
			{
				// Skip self
				if (allyID == ownerID)
					continue;

				PlayerState ally = stateSnapshot.players[allyID];

				// Check all allied structures for nearby enemies
				foreach (UnitState unit in ally.units.GetStructures())
					CreateProximityZone(stateSnapshot, combatGroups, unit);

				// Check all allied civilian units for nearby enemies
				foreach (UnitState unit in ally.units.GetVehicles())
				{
					if (unit.hasWeapon)
						continue;

					CreateProximityZone(stateSnapshot, combatGroups, unit);
				}
			}
		}

		private void CreateProximityZone(StateSnapshot stateSnapshot, List<VehicleGroup> combatGroups, UnitState unit)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Create zone around unit
			MAP_RECT area = new MAP_RECT(unit.position, new LOCATION(0,0));
			area.Inflate(8);

			if (DoesOverlapZone(area, m_DefenseZones))
				return;

			List<UnitState> enemiesInArea = stateSnapshot.unitMap.GetUnitsInArea(area);
			enemiesInArea.RemoveAll((UnitState tileUnit) => owner.allyPlayerIDs.Contains(tileUnit.ownerID));

			if (enemiesInArea.Count > 0)
				m_DefenseZones.Add(CreateZone(stateSnapshot, combatGroups, area, enemiesInArea.ToArray(), 5, VehicleGroupType.Assault));
		}

		/// <summary>
		/// Creates threat zones for vulnerable enemy structures.
		/// </summary>
		private void CreateVulnerableStructureZones(StateSnapshot stateSnapshot, List<VehicleGroup> combatGroups)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			foreach (int enemyID in owner.enemyPlayerIDs)
			{
				PlayerState enemy = stateSnapshot.players[enemyID];

				foreach (UnitState unit in enemy.units.GetStructures())
				{
					// Skip combat units
					if (unit.hasWeapon)
						continue;

					MAP_RECT area = new MAP_RECT(unit.position, new LOCATION(0,0));
					area.Inflate(8);

					if (DoesOverlapZone(area, m_VulnerableZones))
						continue;

					List<UnitState> enemiesInArea = stateSnapshot.unitMap.GetUnitsInArea(area);
					enemiesInArea.RemoveAll((UnitState tileUnit) => owner.allyPlayerIDs.Contains(tileUnit.ownerID));

					// Skip areas that are defended
					if (enemiesInArea.Find((UnitState areaEnemy) => areaEnemy.hasWeapon) != null)
						continue;

					// Get all enemy units of this unit's type
					enemiesInArea.RemoveAll((UnitState tileUnit) => tileUnit.unitType != unit.unitType);

					GlobalUnitInfo starflareInfo = stateSnapshot.weaponInfo[map_id.Starflare];
					
					// Create zone, prioritizing bombers if they are available
					if (owner.HasTechnologyByIndex(starflareInfo.researchTopic))
						m_VulnerableZones.Add(CreateZone(stateSnapshot, combatGroups, area, enemiesInArea.ToArray(), 1, VehicleGroupType.Bomber));
					else
						m_VulnerableZones.Add(CreateZone(stateSnapshot, combatGroups, area, enemiesInArea.ToArray(), 4, VehicleGroupType.Harass));
				}
			}
		}

		/// <summary>
		/// Creates threat zones for vulnerable enemy vehicles.
		/// </summary>
		private void CreateVulnerableVehicleZones(StateSnapshot stateSnapshot, List<VehicleGroup> combatGroups)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			foreach (int enemyID in owner.enemyPlayerIDs)
			{
				PlayerState enemy = stateSnapshot.players[enemyID];

				foreach (VehicleState unit in enemy.units.GetVehicles())
				{
					// Skip combat units
					if (unit.hasWeapon)
						continue;

					MAP_RECT area = new MAP_RECT(unit.position, new LOCATION(0,0));
					area.Inflate(8);

					if (DoesOverlapZone(area, m_VulnerableZones))
						continue;

					List<UnitState> enemiesInArea = stateSnapshot.unitMap.GetUnitsInArea(area);
					enemiesInArea.RemoveAll((UnitState tileUnit) => owner.allyPlayerIDs.Contains(tileUnit.ownerID));

					// Skip areas that are defended
					if (enemiesInArea.Find((UnitState areaEnemy) => areaEnemy.hasWeapon) != null)
						continue;

					// Get all enemy units of this unit's type
					enemiesInArea.RemoveAll((UnitState tileUnit) => tileUnit.unitType != unit.unitType);

					GlobalUnitInfo spiderInfo = stateSnapshot.vehicleInfo[map_id.Spider];
					
					// Create zone, prioritizing capture groups if they are available
					if (!owner.isEden && owner.HasTechnologyByIndex(spiderInfo.researchTopic))
						m_VulnerableZones.Add(CreateZone(stateSnapshot, combatGroups, area, enemiesInArea.ToArray(), 1, VehicleGroupType.Capture));
					else
						m_VulnerableZones.Add(CreateZone(stateSnapshot, combatGroups, area, enemiesInArea.ToArray(), 4, VehicleGroupType.Harass));
				}
			}
		}

		/// <summary>
		/// Create threat zones for enemy bases.
		/// </summary>
		private void CreateEnemyBaseZones(StateSnapshot stateSnapshot, List<VehicleGroup> combatGroups)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			foreach (int enemyID in owner.enemyPlayerIDs)
			{
				PlayerState enemy = stateSnapshot.players[enemyID];

				foreach (StructureState unit in enemy.units.GetStructures())
				{
					MAP_RECT area = new MAP_RECT(unit.position, new LOCATION(0,0));
					area.Inflate(8);

					if (DoesOverlapZone(area, m_EnemyBaseZones))
						continue;

					List<UnitState> enemiesInArea = stateSnapshot.unitMap.GetUnitsInArea(area);
					enemiesInArea.RemoveAll((UnitState tileUnit) => owner.allyPlayerIDs.Contains(tileUnit.ownerID));

					m_EnemyBaseZones.Add(CreateZone(stateSnapshot, combatGroups, area, enemiesInArea.ToArray(), 5, VehicleGroupType.Assault));
				}
			}
		}

		/// <summary>
		/// Create threat zones for own base units.
		/// </summary>
		private void CreateDefenseZones(StateSnapshot stateSnapshot, List<VehicleGroup> combatGroups)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			foreach (UnitState unit in owner.units.GetStructures())
			{
				MAP_RECT area = new MAP_RECT(unit.position, new LOCATION(0,0));
				area.Inflate(8);

				if (DoesOverlapZone(area, m_DefenseZones))
					continue;

				m_DefenseZones.Add(CreateZone(stateSnapshot, combatGroups, area, new UnitState[0], 3, VehicleGroupType.Assault));
			}

			foreach (UnitState unit in owner.units.GetVehicles())
			{
				// Don't create defense zones for our own combat units. It will cause confusion.
				if (unit.hasWeapon)
					continue;

				MAP_RECT area = new MAP_RECT(unit.position, new LOCATION(0,0));
				area.Inflate(8);

				if (DoesOverlapZone(area, m_DefenseZones))
					continue;

				m_DefenseZones.Add(CreateZone(stateSnapshot, combatGroups, area, new UnitState[0], 4, VehicleGroupType.Assault));
			}
		}

		/// <summary>
		/// Create threat zones for empty mining sites.
		/// </summary>
		private void CreateMiningZones(StateSnapshot stateSnapshot, List<VehicleGroup> combatGroups)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// TODO: Implement
		}

		private bool DoesOverlapZone(MAP_RECT area, IEnumerable<ThreatZone> zones)
		{
			foreach (ThreatZone zone in zones)
			{
				if (zone.bounds.DoesRectIntersect(area))
					return true;
			}

			return false;
		}

		private ThreatZone CreateZone(StateSnapshot stateSnapshot, List<VehicleGroup> combatGroups, MAP_RECT area, UnitState[] enemiesInArea, int additionalStrengthDesired, VehicleGroupType groupType)
		{
			ThreatZone zone = new ThreatZone(stateSnapshot, ownerID, area, enemiesInArea, additionalStrengthDesired);
			
			VehicleGroup zoneGroup = null;

			// Create group for zone and assign zone to it.
			switch (groupType)
			{
				case VehicleGroupType.Assault:		zoneGroup = new AssaultGroup(ownerID, zone);	break;
				case VehicleGroupType.Harass:		zoneGroup = new HarassGroup(ownerID, zone);		break;
				case VehicleGroupType.Bomber:		zoneGroup = new BomberGroup(ownerID, zone);		break;
				case VehicleGroupType.Capture:		zoneGroup = new CaptureGroup(ownerID, zone);	break;
			}

			if (zoneGroup != null)
			{
				combatGroups.Add(zoneGroup);
				return zone;
			}

			return null;
		}
	}
}
