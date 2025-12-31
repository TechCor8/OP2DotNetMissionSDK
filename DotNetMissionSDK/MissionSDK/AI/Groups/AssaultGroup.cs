using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Combat.Groups
{
	/// <summary>
	/// A combat group for crushing the enemy with overwhelming force.
	/// </summary>
	public class AssaultGroup : VehicleGroup
	{
		public override VehicleGroupType groupType		{ get { return VehicleGroupType.Assault;		}	}


		public AssaultGroup(int ownerID, CombatZone zone) : base(ownerID, zone)
		{
		}

		/// <summary>
		/// Gets the units this group needs to reach capacity.
		/// </summary>
		protected override UnitSlot[] GetUnitSlots(int combatStrength)
		{
			UnitWithWeaponType[] repairSupportedTypes = GetRepairSupportedTypes();
			UnitWithWeaponType[] empSupportedTypes = GetEMPSupportedTypes();

			List<UnitSlot> unitSlots = new List<UnitSlot>();

			// Fill group with any combat unit
			for (int i=0; i < combatStrength; ++i)
			{
				if (i % 10 == 9) // Every 10th slot, create a repair unit.
					unitSlots.Add(new UnitSlot(repairSupportedTypes));
				if (i % 5 == 4) // Every 5th slot, create an EMP unit.
					unitSlots.Add(new UnitSlot(empSupportedTypes));
				else
					unitSlots.Add(new UnitSlot(GetStandardCombatTypePriority()));
			}

			return unitSlots.ToArray();
		}

		private UnitWithWeaponType[] GetRepairSupportedTypes()
		{
			List<UnitWithWeaponType> supportedTypes = new List<UnitWithWeaponType>();
			supportedTypes.Add(new UnitWithWeaponType(map_id.Spider, map_id.None));
			supportedTypes.Add(new UnitWithWeaponType(map_id.RepairVehicle, map_id.None));
			supportedTypes.AddRange(GetStandardCombatTypePriority());

			return supportedTypes.ToArray();
		}

		private UnitWithWeaponType[] GetEMPSupportedTypes()
		{
			List<UnitWithWeaponType> supportedTypes = new List<UnitWithWeaponType>();
			supportedTypes.Add(new UnitWithWeaponType(map_id.Tiger, map_id.EMP));
			supportedTypes.Add(new UnitWithWeaponType(map_id.Panther, map_id.EMP));
			supportedTypes.Add(new UnitWithWeaponType(map_id.Lynx, map_id.EMP));
			supportedTypes.AddRange(GetStandardCombatTypePriority());

			return supportedTypes.ToArray();
		}
	}
}
