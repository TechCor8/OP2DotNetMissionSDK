using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Combat
{
	public enum ThreatLevel
	{
		None,		// No enemies in the area
		Unarmed,	// Enemies in the area are unarmed
		Armed		// Enemies have weapons in the area
	}

	/// <summary>
	/// Represents an area on the map that needs military intervention.
	/// NOTE: CombatZone is immutable and safe for async operations.
	/// </summary>
	public class CombatZone
	{
		private PlayerState m_Owner;

		public MAP_RECT bounds				{ get; private set; }
		public int strengthRequired			{ get; private set; }
		public int strengthDesired			{ get; private set; }
		public ThreatLevel threatLevel		{ get; private set; }
		public UnitState[] priorityTargets	{ get; private set; }
		public float importance				{ get; private set; }


		/// <summary>
		/// Constructor for CombatZone.
		/// </summary>
		/// <param name="state">The snapshot to use to generate the zone.</param>
		/// <param name="ownerID">The bot player that owns this zone.</param>
		/// <param name="bounds">The map area of the zone.</param>
		/// <param name="priorityTargets">The primary targets to attack.</param>
		/// <param name="additionalStrengthDesired">Additional strength beyond what is required to control this zone.</param>
		/// <param name="groupType">The type of combat group to organize.</param>
		public CombatZone(StateSnapshot state, int ownerID, MAP_RECT bounds, UnitState[] priorityTargets, int additionalStrengthDesired)
		{
			m_Owner = state.players[ownerID];
			this.bounds = bounds;

			// Get enemies in zone
			List<UnitState> enemies = state.unitMap.GetUnitsInArea(bounds);
			enemies.RemoveAll((UnitState tileUnit) => m_Owner.allyPlayerIDs.Contains(tileUnit.ownerID));

			// Calculate enemy strength
			foreach (UnitState unit in enemies)
			{
				if (unit.hasWeapon)
					strengthRequired += state.weaponInfo[unit.weapon].weaponStrength;
			}

			if (strengthRequired > 0)
				threatLevel = ThreatLevel.Armed;
			else if (enemies.Count > 0)
				threatLevel = ThreatLevel.Unarmed;
			else
				threatLevel = ThreatLevel.None;

			strengthDesired = strengthRequired + additionalStrengthDesired;

			this.priorityTargets = priorityTargets;

			importance = CalculateImportance(state);
		}

		/// <summary>
		/// Returns true if the zone contains the position.
		/// </summary>
		public bool Contains(LOCATION position)
		{
			return bounds.Contains(position);
		}

		/// <summary>
		/// Returns the closest priority target to the specified position.
		/// </summary>
		public UnitState GetClosestPriorityTarget(LOCATION position)
		{
			UnitState closestTarget = null;
			int closestDistance = int.MaxValue;

			foreach (UnitState target in priorityTargets)
			{
				int distance = target.position.GetDiagonalDistance(position);
				if (distance < closestDistance)
				{
					closestTarget = target;
					closestDistance = distance;
				}
			}

			return closestTarget;
		}

		private float CalculateImportance(StateSnapshot state)
		{
			// Proximity to self structures
			int distanceToOurStructures = 99999;
			foreach (UnitState structure in m_Owner.units.GetStructures())
			{
				int distance = bounds.GetDiagonalDistance(structure.position);
				if (distance < distanceToOurStructures)
					distanceToOurStructures = distance;
			}

			// Proximity self civilian vehicles
			int distanceToOurVehicles = 99999;
			int distanceToOurMilitary = 99999;
			foreach (UnitState vehicle in m_Owner.units.GetVehicles())
			{
				int distance = bounds.GetDiagonalDistance(vehicle.position);
				if (vehicle.hasWeapon)
				{
					if (distance < distanceToOurMilitary)
						distanceToOurMilitary = distance;
				}
				else
				{
					if (distance < distanceToOurVehicles)
						distanceToOurVehicles = distance;
				}
			}

			// Proximity to allied structures
			// Proximity to allied civilian vehicles
			int distanceToAlliedStructures = 99999;
			int distanceToAlliedVehicles = 99999;

			foreach (int allyID in m_Owner.allyPlayerIDs)
			{
				PlayerState ally = state.players[allyID];

				foreach (UnitState unit in ally.units.GetUnits())
				{
					int distance = bounds.GetDiagonalDistance(unit.position);

					if (unit.isBuilding)
					{
						if (distance < distanceToAlliedStructures)
							distanceToAlliedStructures = distance;
					}
					else
					{
						if (distance < distanceToAlliedVehicles)
							distanceToAlliedVehicles = distance;
					}
				}
			}

			// Primary target value
			float targetValue = 0;
			foreach (UnitState target in priorityTargets)
			{
				targetValue += GetTargetUnitValue(target.unitType);
			}

			if (priorityTargets.Length > 0)
				targetValue /= priorityTargets.Length;

			float importance = 0.1f;

			float baseImportance = 1.0f - Clamp(distanceToOurStructures / 100.0f);
			float vehicleImportance = 1.0f - Clamp(distanceToOurVehicles / 100.0f);
			float militaryImportance = 1.0f - Clamp(distanceToOurMilitary / 30.0f);
			float allyBaseImportance = 1.0f - Clamp(distanceToAlliedStructures / 100.0f);
			float allyVehicleImportance = 1.0f - Clamp(distanceToAlliedVehicles / 100.0f);
			const bool areAlliesImportant = true;

			switch (threatLevel)
			{
				case ThreatLevel.Armed:
					// If the enemy is armed, importance is based on distance to our units and the enemies strength compared to our own.
					importance = System.Math.Max(baseImportance, vehicleImportance);
					if (areAlliesImportant)
					{
						float allyImportance = System.Math.Max(allyBaseImportance, allyVehicleImportance);
						importance = System.Math.Max(importance, allyImportance);
					}

					// Get the amount of our army that will be required to win the zone
					float armySizeMultiple = m_Owner.totalOffensiveStrength / strengthRequired;
					armySizeMultiple /= 5;
					armySizeMultiple = Clamp(armySizeMultiple);
					importance *= armySizeMultiple;

					importance *= targetValue;
					break;

				case ThreatLevel.Unarmed:
					// If enemy is unarmed, opportunity kills are more important when our military is close by.
					importance = militaryImportance * targetValue;
					break;

				case ThreatLevel.None:
					// If there is no enemy present, this is a defensive zone. Prioritize our units by distance to the enemy.
					importance = 0.01f;
					break;
			}

			return importance;
		}

		private float GetTargetUnitValue(map_id unitType)
		{
			switch (unitType)
			{
				case map_id.CargoTruck:					return 0.995f;
				case map_id.ConVec:						return 0.995f;
				case map_id.Spider:						return 1.0f;
				case map_id.Scorpion:					return 1.0f;
				case map_id.Lynx:						return 1.0f;
				case map_id.Panther:					return 1.0f;
				case map_id.Tiger:						return 1.0f;
				case map_id.RoboSurveyor:				return 0.25f;
				case map_id.RoboMiner:					return 0.5f;
				case map_id.GeoCon:						return 0.6f;
				case map_id.Scout:						return 0.15f;
				case map_id.RoboDozer:					return 0.1f;
				case map_id.EvacuationTransport:		return 0.05f;
				case map_id.RepairVehicle:				return 0.35f;
				case map_id.Earthworker:				return 0.45f;

				case map_id.CommonOreMine:				return 0.97f;
				case map_id.RareOreMine:				return 0.75f;
				case map_id.GuardPost:					return 0.99f;
				case map_id.LightTower:					return 0.15f;
				case map_id.CommonStorage:				return 0.93f;
				case map_id.RareStorage:				return 0.91f;
				case map_id.Forum:						return 0.05f;
				case map_id.CommandCenter:				return 0.95f;
				case map_id.MHDGenerator:				return 0.95f;
				case map_id.Residence:					return 0.05f;
				case map_id.RobotCommand:				return 0.05f;
				case map_id.TradeCenter:				return 0.05f;
				case map_id.BasicLab:					return 0.05f;
				case map_id.MedicalCenter:				return 0.05f;
				case map_id.Nursery:					return 0.07f;
				case map_id.SolarPowerArray:			return 0.95f;
				case map_id.RecreationFacility:			return 0.05f;
				case map_id.University:					return 0.07f;
				case map_id.Agridome:					return 0.1f;
				case map_id.DIRT:						return 0.05f;
				case map_id.Garage:						return 0.4f;
				case map_id.MagmaWell:					return 0.75f;
				case map_id.MeteorDefense:				return 0.7f;
				case map_id.GeothermalPlant:			return 0.95f;
				case map_id.ArachnidFactory:			return 0.725f;
				case map_id.ConsumerFactory:			return 0.05f;
				case map_id.StructureFactory:			return 0.7125f;
				case map_id.VehicleFactory:				return 0.745f;
				case map_id.StandardLab:				return 0.7f;
				case map_id.AdvancedLab:				return 0.71f;
				case map_id.Observatory:				return 0.7f;
				case map_id.ReinforcedResidence:		return 0.05f;
				case map_id.AdvancedResidence:			return 0.05f;
				case map_id.CommonOreSmelter:			return 0.98f;
				case map_id.Spaceport:					return 0.36f;
				case map_id.RareOreSmelter:				return 0.92f;
				case map_id.GORF:						return 0.09f;
				case map_id.Tokamak:					return 0.95f;
			}

			return 0;
		}

		private float Clamp(float val)
		{
			if (val < 0) return 0;
			if (val > 1) return 1;
			return val;
		}
	}
}
