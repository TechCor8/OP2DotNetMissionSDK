using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Combat.Groups
{
	/// <summary>
	/// A combat group for capturing enemy units.
	/// </summary>
	public class CaptureGroup : VehicleGroup
	{
		public override VehicleGroupType groupType		{ get { return VehicleGroupType.Capture;		}	}


		public CaptureGroup(PlayerInfo owner, ThreatZone zone) : base(owner, zone)
		{
		}

		/// <summary>
		/// Gets the units this group needs to reach capacity.
		/// </summary>
		protected override UnitSlot[] GetUnitSlots()
		{
			return new UnitSlot[]
			{
				new UnitSlot(new UnitWithWeaponType[] { new UnitWithWeaponType(map_id.Spider, map_id.None) }),
				new UnitSlot(new UnitWithWeaponType[] { new UnitWithWeaponType(map_id.Spider, map_id.None) }),
				new UnitSlot(new UnitWithWeaponType[] { new UnitWithWeaponType(map_id.Spider, map_id.None) }),
				new UnitSlot(new UnitWithWeaponType[] { new UnitWithWeaponType(map_id.Lynx, map_id.EMP) }),
				new UnitSlot(new UnitWithWeaponType[] { new UnitWithWeaponType(map_id.Lynx, map_id.EMP) }),
				new UnitSlot(new UnitWithWeaponType[] { new UnitWithWeaponType(map_id.Lynx, map_id.EMP) }),
				new UnitSlot(new UnitWithWeaponType[] { new UnitWithWeaponType(map_id.Lynx, map_id.EMP) })
			};
		}

		/// <summary>
		/// Called to update combat group.
		/// </summary>
		public override void Update()
		{
		}
	}
}
