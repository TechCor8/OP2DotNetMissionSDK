using DotNetMissionSDK.AI.Combat;
using DotNetMissionSDK.AI.Combat.Groups;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Units;
using DotNetMissionSDK.Utility;
using DotNetMissionSDK.Utility.Maps;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Managers
{
	/// <summary>
	/// Organizes units and sends them into combat.
	/// </summary>
	public class CombatManager
	{
		private List<VehicleGroup> m_CombatGroups = new List<VehicleGroup>();

		// Debugging
		private List<Unit> m_DebugMarkers = new List<Unit>();

		public BotPlayer botPlayer							{ get; private set; }
		public PlayerInfo owner								{ get; private set; }

		
		public CombatManager(BotPlayer botPlayer, PlayerInfo owner)
		{
			this.botPlayer = botPlayer;
			this.owner = owner;
		}

		public void Update()
		{
			m_CombatGroups.Clear();

			// Create threat zones
			CreateProximityZones();
			CreateVulnerableStructureZones();
			CreateVulnerableVehicleZones();
			CreateEnemyBaseZones();
			CreateDefenseZones();
			CreateMiningZones();

			// Update debug markers
			if (owner.player.playerID == TethysGame.LocalPlayer())
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

			PopulateCombatGroups();

			// Update vehicle groups
			foreach (VehicleGroup group in m_CombatGroups)
				group.Update();
		}

		private void PopulateCombatGroups()
		{
			List<Vehicle> unassignedUnits = new List<Vehicle>(owner.units.lynx);
			unassignedUnits.AddRange(owner.units.panthers);
			unassignedUnits.AddRange(owner.units.tigers);
			unassignedUnits.AddRange(owner.units.spiders);
			unassignedUnits.AddRange(owner.units.scorpions);

			// Units already in an active threat zone must be assigned to that zone
			for (int i=0; i < unassignedUnits.Count; ++i)
			{
				Vehicle unit = unassignedUnits[i];

				foreach (VehicleGroup group in m_CombatGroups)
				{
					if (group.threatZone.threatLevel != ThreatLevel.None && group.threatZone.Contains(unit))
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
			foreach (VehicleGroup group in m_CombatGroups)
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
			foreach (VehicleGroup group in m_CombatGroups)
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
		/// </summary>
		public List<VehicleGroup.UnitSlot> GetUnassignedSlots()
		{
			List<VehicleGroup.UnitSlot> unassignedSlots = new List<VehicleGroup.UnitSlot>();

			// Get full list of unassigned slots
			foreach (VehicleGroup group in m_CombatGroups)
				unassignedSlots.AddRange(group.GetUnassignedSlots());

			return unassignedSlots;
		}

		// TODO:
		// PopulateCombatGroup - Units to populate must be able to reach zone without engaging in combat
		// Unit pathfinding

		/// <summary>
		/// Creates threat zones for enemy combat units in close proximity to allied civilian units.
		/// </summary>
		public void CreateProximityZones()
		{
			// Check all self structures for nearby enemies
			foreach (UnitEx unit in owner.units.GetStructures())
				CreateProximityZone(unit);

			// Check all self civilian units for nearby enemies
			foreach (UnitEx unit in owner.units.GetVehicles())
			{
				if (unit.HasWeapon())
					continue;

				CreateProximityZone(unit);
			}

			foreach (PlayerInfo player in owner.allies)
			{
				// Skip self
				if (player == owner)
					continue;

				// Check all allied structures for nearby enemies
				foreach (UnitEx unit in player.units.GetStructures())
					CreateProximityZone(unit);

				// Check all allied civilian units for nearby enemies
				foreach (UnitEx unit in player.units.GetVehicles())
				{
					if (unit.HasWeapon())
						continue;

					CreateProximityZone(unit);
				}
			}
		}

		private void CreateProximityZone(UnitEx unit)
		{
			// Create zone around unit
			MAP_RECT area = new MAP_RECT(unit.GetPosition(), new LOCATION(0,0));
			area.Inflate(8);

			if (DoesOverlapZone(area))
				return;

			List<UnitEx> enemiesInArea = PlayerUnitMap.GetUnitsInArea(area);
			enemiesInArea.RemoveAll((UnitEx tileUnit) => owner.player.IsAlliedTo(tileUnit.GetOwner()));

			if (enemiesInArea.Count > 0)
				CreateZone(area, enemiesInArea.ToArray(), 5, VehicleGroupType.Assault);
		}

		/// <summary>
		/// Creates threat zones for vulnerable enemy structures.
		/// </summary>
		public void CreateVulnerableStructureZones()
		{
			foreach (PlayerInfo enemy in owner.enemies)
			{
				foreach (UnitEx unit in enemy.units.GetStructures())
				{
					// Skip combat units
					if (unit.HasWeapon())
						continue;

					MAP_RECT area = new MAP_RECT(unit.GetPosition(), new LOCATION(0,0));
					area.Inflate(8);

					if (DoesOverlapZone(area))
						continue;

					List<UnitEx> enemiesInArea = PlayerUnitMap.GetUnitsInArea(area);
					enemiesInArea.RemoveAll((UnitEx tileUnit) => owner.player.IsAlliedTo(tileUnit.GetOwner()));

					// Skip areas that are defended
					if (enemiesInArea.Find((UnitEx areaEnemy) => areaEnemy.HasWeapon()) != null)
						continue;

					// Get all enemy units of this unit's type
					enemiesInArea.RemoveAll((UnitEx tileUnit) => tileUnit.GetUnitType() != unit.GetUnitType());

					UnitInfo starflareInfo = new UnitInfo(map_id.Starflare);
					TechInfo techInfo = Research.GetTechInfo(starflareInfo.GetResearchTopic());

					// Create zone, prioritizing bombers if they are available
					if (owner.player.HasTechnology(techInfo.GetTechID()))
						CreateZone(area, enemiesInArea.ToArray(), 1, VehicleGroupType.Bomber);
					else
						CreateZone(area, enemiesInArea.ToArray(), 4, VehicleGroupType.Harass);
				}
			}
		}

		/// <summary>
		/// Creates threat zones for vulnerable enemy vehicles.
		/// </summary>
		public void CreateVulnerableVehicleZones()
		{
			foreach (PlayerInfo enemy in owner.enemies)
			{
				foreach (UnitEx unit in enemy.units.GetVehicles())
				{
					// Skip combat units
					if (unit.HasWeapon())
						continue;

					MAP_RECT area = new MAP_RECT(unit.GetPosition(), new LOCATION(0,0));
					area.Inflate(8);

					if (DoesOverlapZone(area))
						continue;

					List<UnitEx> enemiesInArea = PlayerUnitMap.GetUnitsInArea(area);
					enemiesInArea.RemoveAll((UnitEx tileUnit) => owner.player.IsAlliedTo(tileUnit.GetOwner()));

					// Skip areas that are defended
					if (enemiesInArea.Find((UnitEx areaEnemy) => areaEnemy.HasWeapon()) != null)
						continue;

					// Get all enemy units of this unit's type
					enemiesInArea.RemoveAll((UnitEx tileUnit) => tileUnit.GetUnitType() != unit.GetUnitType());

					UnitInfo spiderInfo = new UnitInfo(map_id.Spider);
					TechInfo techInfo = Research.GetTechInfo(spiderInfo.GetResearchTopic());

					// Create zone, prioritizing capture groups if they are available
					if (!owner.player.IsEden() && owner.player.HasTechnology(techInfo.GetTechID()))
						CreateZone(area, enemiesInArea.ToArray(), 1, VehicleGroupType.Capture);
					else
						CreateZone(area, enemiesInArea.ToArray(), 4, VehicleGroupType.Harass);
				}
			}
		}

		/// <summary>
		/// Create threat zones for enemy bases.
		/// </summary>
		public void CreateEnemyBaseZones()
		{
			foreach (PlayerInfo enemy in owner.enemies)
			{
				foreach (UnitEx unit in enemy.units.GetStructures())
				{
					MAP_RECT area = new MAP_RECT(unit.GetPosition(), new LOCATION(0,0));
					area.Inflate(8);

					if (DoesOverlapZone(area))
						continue;

					List<UnitEx> enemiesInArea = PlayerUnitMap.GetUnitsInArea(area);
					enemiesInArea.RemoveAll((UnitEx tileUnit) => owner.player.IsAlliedTo(tileUnit.GetOwner()));

					CreateZone(area, enemiesInArea.ToArray(), 5, VehicleGroupType.Assault);
				}
			}
		}

		/// <summary>
		/// Create threat zones for own base units.
		/// </summary>
		public void CreateDefenseZones()
		{
			foreach (UnitEx unit in owner.units.GetStructures())
			{
				MAP_RECT area = new MAP_RECT(unit.GetPosition(), new LOCATION(0,0));
				area.Inflate(8);

				if (DoesOverlapZone(area))
					continue;

				CreateZone(area, new UnitEx[0], 3, VehicleGroupType.Assault);
			}

			foreach (UnitEx unit in owner.units.GetVehicles())
			{
				// Don't create defense zones for our own combat units. It will cause confusion.
				if (unit.HasWeapon())
					continue;

				MAP_RECT area = new MAP_RECT(unit.GetPosition(), new LOCATION(0,0));
				area.Inflate(8);

				if (DoesOverlapZone(area))
					continue;

				CreateZone(area, new UnitEx[0], 4, VehicleGroupType.Assault);
			}
		}

		/// <summary>
		/// Create threat zones for empty mining sites.
		/// </summary>
		public void CreateMiningZones()
		{
			// TODO: Implement
		}

		private bool DoesOverlapZone(MAP_RECT area)
		{
			foreach (VehicleGroup group in m_CombatGroups)
			{
				if (group.threatZone.bounds.DoesRectIntersect(area))
					return true;
			}

			return false;
		}

		private void CreateZone(MAP_RECT area, UnitEx[] enemiesInArea, int additionalStrengthDesired, VehicleGroupType groupType)
		{
			ThreatZone zone = new ThreatZone(owner, area, enemiesInArea, additionalStrengthDesired);
			
			VehicleGroup zoneGroup = null;

			// Create group for zone and assign zone to it.
			switch (groupType)
			{
				case VehicleGroupType.Assault:		zoneGroup = new AssaultGroup(owner, zone);		break;
				case VehicleGroupType.Harass:		zoneGroup = new HarassGroup(owner, zone);		break;
				case VehicleGroupType.Bomber:		zoneGroup = new BomberGroup(owner, zone);		break;
				case VehicleGroupType.Capture:		zoneGroup = new CaptureGroup(owner, zone);		break;
			}

			if (zoneGroup != null)
				m_CombatGroups.Add(zoneGroup);
		}
	}
}
