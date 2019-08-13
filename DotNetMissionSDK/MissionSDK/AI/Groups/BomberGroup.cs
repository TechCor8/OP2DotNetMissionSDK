using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Combat.Groups
{
	/// <summary>
	/// A combat group for bombing defenseless enemy structures.
	/// </summary>
	public class BomberGroup : VehicleGroup
	{
		public override VehicleGroupType groupType		{ get { return VehicleGroupType.Bomber;		}	}


		public BomberGroup(PlayerInfo owner, ThreatZone zone) : base(owner, zone)
		{
		}

		/// <summary>
		/// Gets the units this group needs to reach capacity.
		/// </summary>
		protected override UnitSlot[] GetUnitSlots(int combatStrength)
		{
			UnitWithWeaponType[] bombSupportedTypes = new UnitWithWeaponType[]
			{
				new UnitWithWeaponType(map_id.Lynx, map_id.Supernova),
				new UnitWithWeaponType(map_id.Lynx, map_id.Starflare),
			};

			return new UnitSlot[]
			{
				new UnitSlot(bombSupportedTypes)
			};
		}
	}
}
