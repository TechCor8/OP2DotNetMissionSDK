using DotNetMissionSDK.HFL;

namespace DotNetMissionSDK.AI.CombatGroups
{
	/// <summary>
	/// A combat group for bombing defenseless enemy structures.
	/// </summary>
	public class BomberGroup : CombatGroup
	{
		public override CombatGroupType groupType		{ get { return CombatGroupType.Bomber;		}	}


		public BomberGroup(Player owner) : base(owner)
		{
		}

		/// <summary>
		/// Gets the units this group needs to reach capacity.
		/// </summary>
		protected override UnitWithWeapon[] GetDesiredUnitsInSubType()
		{
			map_id bombType = map_id.Supernova;

			// Check if supernova is available
			UnitInfo unitInfo = new UnitInfo(bombType);
			TechInfo techInfo = Research.GetTechInfo(unitInfo.GetResearchTopic());

			if (!m_Owner.HasTechnology(techInfo.GetTechID()))
				bombType = map_id.Starflare;

			// Return units
			return new UnitWithWeapon[]
			{
				new UnitWithWeapon(map_id.Lynx, bombType),
				new UnitWithWeapon(map_id.Lynx, bombType),
				new UnitWithWeapon(map_id.Lynx, bombType),
				new UnitWithWeapon(map_id.Lynx, map_id.EMP),
				new UnitWithWeapon(map_id.Lynx, map_id.EMP),
				new UnitWithWeapon(map_id.Lynx, map_id.EMP),
				new UnitWithWeapon(map_id.Lynx, map_id.EMP)
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
