using DotNetMissionSDK.HFL;

namespace DotNetMissionSDK.AI.CombatGroups
{
	/// <summary>
	/// A combat group for capturing enemy units.
	/// </summary>
	public class CaptureGroup : CombatGroup
	{
		public override CombatGroupType groupType		{ get { return CombatGroupType.Capture;		}	}


		public CaptureGroup(Player owner) : base(owner)
		{
		}

		/// <summary>
		/// Gets the units this group needs to reach capacity.
		/// </summary>
		protected override UnitWithWeapon[] GetDesiredUnitsInSubType()
		{
			if (m_Owner.IsEden())
				return new UnitWithWeapon[0];

			// Check if spider and EMP is available
			UnitInfo unitInfo = new UnitInfo(map_id.Spider);
			TechInfo techInfo = Research.GetTechInfo(unitInfo.GetResearchTopic());

			if (!m_Owner.HasTechnology(techInfo.GetTechID()))
				return new UnitWithWeapon[0];

			unitInfo = new UnitInfo(map_id.EMP);
			techInfo = Research.GetTechInfo(unitInfo.GetResearchTopic());

			if (!m_Owner.HasTechnology(techInfo.GetTechID()))
				return new UnitWithWeapon[0];

			// Return units
			return new UnitWithWeapon[]
			{
				new UnitWithWeapon(map_id.Spider, map_id.None),
				new UnitWithWeapon(map_id.Spider, map_id.None),
				new UnitWithWeapon(map_id.Spider, map_id.None),
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
