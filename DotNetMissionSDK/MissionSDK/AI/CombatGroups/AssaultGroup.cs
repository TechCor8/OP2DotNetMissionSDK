﻿using DotNetMissionSDK.HFL;

namespace DotNetMissionSDK.AI.CombatGroups
{
	/// <summary>
	/// A combat group for crushing the enemy with overwhelming force.
	/// </summary>
	public class AssaultGroup : CombatGroup
	{
		public override CombatGroupType groupType		{ get { return CombatGroupType.Assault;		}	}


		public AssaultGroup(Player owner) : base(owner)
		{
		}

		/// <summary>
		/// Gets the units this group needs to reach capacity.
		/// </summary>
		protected override UnitWithWeapon[] GetDesiredUnitsInSubType()
		{
			map_id weaponType;

			if (m_Owner.IsEden())
				weaponType = map_id.Laser;
			else
				weaponType = map_id.Microwave;

			// TODO: Check for tech and metal. If adequate, prefer stronger units.

			// Return units
			return new UnitWithWeapon[]
			{
				new UnitWithWeapon(map_id.Lynx, weaponType),
				new UnitWithWeapon(map_id.Lynx, weaponType),
				new UnitWithWeapon(map_id.Lynx, weaponType),
				new UnitWithWeapon(map_id.Lynx, weaponType),
				new UnitWithWeapon(map_id.Lynx, weaponType),
				new UnitWithWeapon(map_id.Lynx, weaponType),
				new UnitWithWeapon(map_id.Lynx, weaponType),
				new UnitWithWeapon(map_id.Lynx, weaponType),
				new UnitWithWeapon(map_id.Lynx, weaponType),
				new UnitWithWeapon(map_id.Lynx, weaponType)
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
