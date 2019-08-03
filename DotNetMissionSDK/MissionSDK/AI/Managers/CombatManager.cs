using DotNetMissionSDK.AI.CombatGroups;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetMissionSDK.AI.Managers
{
	/// <summary>
	/// Organizes units and sends them into combat.
	/// </summary>
	public class CombatManager
	{
		private List<CombatGroup> m_CombatGroups = new List<CombatGroup>();

		public BotPlayer botPlayer							{ get; private set; }
		public PlayerInfo owner								{ get; private set; }

		
		public CombatManager(BotPlayer botPlayer, PlayerInfo owner)
		{
			this.botPlayer = botPlayer;
			this.owner = owner;
		}

		public void Update()
		{
			// Create Threat Zones

			// Calculate desired combat groups for zones
			Dictionary<CombatGroupType, int> desiredGroups = GetDesiredCombatGroups();

			// Remove expired groups
			for (int i=0; i < m_CombatGroups.Count; ++i)
			{
				CombatGroup group = m_CombatGroups[i];

				int count;
				desiredGroups.TryGetValue(group.groupType, out count);

				if (count == 0)
					m_CombatGroups.RemoveAt(i--);
				else
					--desiredGroups[group.groupType];
			}

			// Add new groups
			foreach (KeyValuePair<CombatGroupType, int> kv in desiredGroups)
			{
				for (int i=0; i < kv.Value; ++i)
					m_CombatGroups.Add(CreateGroup(kv.Key));
			}

			// Populate groups
			PopulateCombatGroups();

			// Update groups
			foreach (CombatGroup group in m_CombatGroups)
				group.Update();
		}

		// Returns the counts of each type of combat group the manager wants to use.
		private Dictionary<CombatGroupType, int> GetDesiredCombatGroups()
		{
			Dictionary<CombatGroupType, int> combatGroupTypes = new Dictionary<CombatGroupType, int>();

			combatGroupTypes.Add(CombatGroupType.Harass, 3);
			combatGroupTypes.Add(CombatGroupType.Bomber, 2);
			combatGroupTypes.Add(CombatGroupType.Assault, 2);

			return combatGroupTypes;
		}

		private CombatGroup CreateGroup(CombatGroupType type)
		{
			switch (type)
			{
				case CombatGroupType.Assault:		return new AssaultGroup(owner.player);
				case CombatGroupType.Harass:		return new HarassGroup(owner.player);
				case CombatGroupType.Bomber:		return new BomberGroup(owner.player);
				case CombatGroupType.Capture:		return new CaptureGroup(owner.player);
			}

			return null;
		}

		// Groups are populated with units starting at index 0.
		// Groups may be partially filled or empty as a result.
		private void PopulateCombatGroups()
		{
			List<UnitEx> unassignedUnits = new List<UnitEx>(owner.units.lynx);
			unassignedUnits.AddRange(owner.units.panthers);
			unassignedUnits.AddRange(owner.units.tigers);
			unassignedUnits.AddRange(owner.units.spiders);
			unassignedUnits.AddRange(owner.units.scorpions);

			foreach (CombatGroup group in m_CombatGroups)
				group.RetainUnits(unassignedUnits);

			foreach (CombatGroup group in m_CombatGroups)
				group.AssignUnits(unassignedUnits);
		}

		/// <summary>
		/// Gets all units desired by all combat groups.
		/// </summary>
		public List<CombatGroup.UnitWithWeapon> GetDesiredUnits()
		{
			List<CombatGroup.UnitWithWeapon> desiredUnits = new List<CombatGroup.UnitWithWeapon>();

			foreach (CombatGroup group in m_CombatGroups)
				desiredUnits.AddRange(group.GetDesiredUnits());

			return desiredUnits;
		}

		/*private void SetGroupTarget(CombatGroup group)
		{
			// If enemy attacking base, and group is closer to base than enemy base, defend

			// If weak enemy found, attack

			// Otherwise, patrol outside enemy base, same side as own base.
		}*/
	}
}
