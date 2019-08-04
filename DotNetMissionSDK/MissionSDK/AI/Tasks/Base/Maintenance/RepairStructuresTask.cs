using DotNetMissionSDK.AI.Tasks.Base.Vehicle;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	/// <summary>
	/// Orders repair units (convec, spider, repair vehicle) to repair damaged structures.
	/// </summary>
	public class RepairStructuresTask : Task
	{
		private BuildRepairUnitTask m_BuildRepairUnitTask;

		public bool repairCriticalOnly;


		public RepairStructuresTask() { }
		public RepairStructuresTask(PlayerInfo owner) : base(owner) { }

		public override bool IsTaskComplete()
		{
			// Task is complete if all structures have adequate HP
			foreach (UnitEx building in new PlayerAllBuildingEnum(owner.player.playerID))
			{
				// Critical only repairs
				if (repairCriticalOnly)
				{
					int maxHP = building.GetUnitInfo().GetHitPoints(owner.player.playerID);
					if (building.GetDamage() / (float)maxHP > 0.75f)
						return false;
				}

				// Full repairs
				if (building.GetUnitType() == map_id.Tokamak)
				{
					if (building.GetDamage() > 40)
						return false;
				}
				else if (building.GetDamage() > 0)
					return false;
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildConvecTask());
			m_BuildRepairUnitTask = new BuildRepairUnitTask(owner);
			m_BuildRepairUnitTask.GeneratePrerequisites();
		}

		protected override bool PerformTask()
		{
			if (!m_BuildRepairUnitTask.IsTaskComplete())
				m_BuildRepairUnitTask.PerformTaskTree();

			// Fail Check: Not enough ore for repairs
			if (owner.player.Ore() < 50)
				return false;

			int damagedStructureCount = 0;

			// Find damaged structures
			foreach (UnitEx unitToFix in new PlayerAllBuildingEnum(owner.player.playerID))
			{
				// Must not be under construction
				if (unitToFix.GetLastCommand() == CommandType.ctMoDevelop || 
					unitToFix.GetLastCommand() == CommandType.ctMoUnDevelop)
					continue;

				if (repairCriticalOnly)
				{
					// If damage not critical, skip unit
					int maxHP = unitToFix.GetUnitInfo().GetHitPoints(owner.player.playerID);
					if (unitToFix.GetDamage() / (float)maxHP < 0.75f)
						continue;
				}
				else
				{
					// If not damaged, skip unit
					if (unitToFix.GetUnitType() == map_id.Tokamak)
					{
						if (unitToFix.GetDamage() <= 40)
							continue;
					}
					else if (unitToFix.GetDamage() <= 0)
						continue;
				}

				++damagedStructureCount;

				// Get repair unit
				UnitEx repairUnit = GetClosestRepairUnit(unitToFix.GetPosition());

				if (repairUnit == null || repairUnit.GetCurAction() != ActionType.moDone)
					continue;

				repairUnit.DoRepair(unitToFix);
			}

			// If we are overwhelmed, build more repair units to meet demand
			if (damagedStructureCount/4 + 1 > owner.units.GetListForType(m_BuildRepairUnitTask.repairUnitType).Count)
				m_BuildRepairUnitTask.targetCountToBuild = damagedStructureCount/4 + 1;

			return true;
		}

		private UnitEx GetClosestRepairUnit(LOCATION position)
		{
			UnitEx closestUnit = null;
			int closestDistance = 999999;

			foreach (UnitEx unit in owner.units.GetListForType(m_BuildRepairUnitTask.repairUnitType))
			{
				// Closest distance
				int distance = position.GetDiagonalDistance(unit.GetPosition());
				if (distance < closestDistance)
				{
					closestUnit = unit;
					closestDistance = distance;
				}
			}

			return closestUnit;
		}
	}
}
