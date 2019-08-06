using DotNetMissionSDK.Utility;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Combat.Groups
{
	/// <summary>
	/// A combat group for harassing defenseless enemy structures.
	/// </summary>
	public class HarassGroup : VehicleGroup
	{
		public override VehicleGroupType groupType		{ get { return VehicleGroupType.Harass;		}	}


		public HarassGroup(PlayerInfo owner, ThreatZone zone) : base(owner, zone)
		{
		}

		/// <summary>
		/// Gets the units this group needs to reach capacity.
		/// </summary>
		protected override UnitSlot[] GetUnitSlots(int combatStrength)
		{
			List<UnitSlot> unitSlots = new List<UnitSlot>();

			// Fill group with any combat unit
			for (int i=0; i < combatStrength; ++i)
				unitSlots.Add(new UnitSlot(GetStandardCombatTypePriority()));

			return unitSlots.ToArray();
		}
	}
}
