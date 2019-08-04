using DotNetMissionSDK.AI.Combat.Groups;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using DotNetMissionSDK.Utility.Maps;
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
	/// </summary>
	public class ThreatZone
	{
		private PlayerInfo m_Owner;

		public MAP_RECT bounds				{ get; private set; }
		public int strengthRequired			{ get; private set; }
		public int strengthDesired			{ get; private set; }
		public ThreatLevel threatLevel		{ get; private set; }

		public UnitEx[] priorityTargets		{ get; private set; }
		public VehicleGroup assignedGroup	{ get; private set; }


		/// <summary>
		/// Constructor for ThreatZone.
		/// </summary>
		/// <param name="owner">The bot player that owns this zone.</param>
		/// <param name="bounds">The map area of the zone.</param>
		/// <param name="priorityTargets">The primary targets to attack.</param>
		/// <param name="additionalStrengthDesired">Additional strength beyond what is required to control this zone.</param>
		/// <param name="groupType">The type of combat group to organize.</param>
		public ThreatZone(PlayerInfo owner, MAP_RECT bounds, UnitEx[] priorityTargets, int additionalStrengthDesired, VehicleGroupType groupType)
		{
			m_Owner = owner;
			this.bounds = bounds;

			// Get enemies in zone
			List<UnitEx> enemies = PlayerUnitMap.GetUnitsInArea(bounds);
			enemies.RemoveAll((UnitEx tileUnit) => owner.player.IsAlliedTo(tileUnit.GetOwner()));

			// Calculate enemy strength
			foreach (UnitEx unit in enemies)
				strengthRequired += unit.GetWeaponInfo().GetWeaponStrength();

			if (strengthRequired > 0)
				threatLevel = ThreatLevel.Armed;
			else if (enemies.Count > 0)
				threatLevel = ThreatLevel.Unarmed;
			else
				threatLevel = ThreatLevel.None;

			strengthDesired = strengthRequired + additionalStrengthDesired;
			this.priorityTargets = priorityTargets;

			switch (groupType)
			{
				case VehicleGroupType.Assault:		assignedGroup = new AssaultGroup(owner, this);		break;
				case VehicleGroupType.Harass:		assignedGroup = new HarassGroup(owner, this);		break;
				case VehicleGroupType.Bomber:		assignedGroup = new BomberGroup(owner, this);		break;
				case VehicleGroupType.Capture:		assignedGroup = new CaptureGroup(owner, this);		break;
			}
		}

		public void Update()
		{
			assignedGroup.Update();
		}

		/// <summary>
		/// Returns true if the zone contains the unit.
		/// </summary>
		public bool Contains(Unit unit)
		{
			return bounds.Contains(unit.GetPosition());
		}

		/// <summary>
		/// Returns true if the unit list can completely fill the group.
		/// </summary>
		public bool CanFillGroup(IEnumerable<UnitEx> units)
		{
			return assignedGroup.CanFillGroup(units);
		}

		/// <summary>
		/// Assigns the unit to the zone's combat group.
		/// </summary>
		/// <returns>Returns true, if unit is assigned</returns>
		public bool AssignUnit(UnitEx unit)
		{
			return assignedGroup.AssignUnit(unit);
		}
	}
}
