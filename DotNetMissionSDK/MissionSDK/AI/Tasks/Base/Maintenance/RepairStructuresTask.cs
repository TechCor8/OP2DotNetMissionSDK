﻿using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;
using System;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	/// <summary>
	/// Orders repair units (convec, spider, repair vehicle) to repair damaged structures.
	/// </summary>
	public class RepairStructuresTask : Task
	{
		private BuildRepairUnitTask m_BuildRepairUnitTask;

		public bool repairCriticalOnly;


		public RepairStructuresTask(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Task is complete if all structures have adequate HP
			foreach (StructureState building in owner.units.GetStructures())
			{
				// Critical only repairs
				if (repairCriticalOnly)
				{
					UnitInfoState info = owner.GetUnitInfo(building.unitType);

					if (building.damage / (float)info.hitPoints > 0.75f)
						return false;
				}

				// Full repairs
				if (building.unitType == map_id.Tokamak)
				{
					if (building.damage > 40)
						return false;
				}
				else if (building.damage > 0)
					return false;
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildConvecTask(ownerID));
			m_BuildRepairUnitTask = new BuildRepairUnitTask(ownerID);
			m_BuildRepairUnitTask.GeneratePrerequisites();
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			if (!m_BuildRepairUnitTask.IsTaskComplete(stateSnapshot))
				m_BuildRepairUnitTask.PerformTaskTree(stateSnapshot, unitActions);

			// Fail Check: Not enough ore for repairs
			if (owner.ore < 50)
				return false;

			int damagedStructureCount = 0;

			// Find damaged structures
			foreach (UnitState unitToFix in owner.units.GetStructures())
			{
				// Must not be under construction
				if (unitToFix.lastCommand == CommandType.ctMoDevelop || 
					unitToFix.lastCommand == CommandType.ctMoUnDevelop)
					continue;

				if (repairCriticalOnly)
				{
					// If damage not critical, skip unit
					UnitInfoState info = owner.GetUnitInfo(unitToFix.unitType);

					if (unitToFix.damage / (float)info.hitPoints < 0.75f)
						continue;
				}
				else
				{
					// If not damaged, skip unit
					if (unitToFix.unitType == map_id.Tokamak)
					{
						if (unitToFix.damage <= 40)
							continue;
					}
					else if (unitToFix.damage <= 0)
						continue;
				}

				++damagedStructureCount;

				// Get repair unit
				UnitState repairUnit = owner.units.GetClosestUnitOfType(m_BuildRepairUnitTask.repairUnitType, unitToFix.position);

				if (repairUnit == null)
					continue;

				int priority = repairCriticalOnly ? 4 : 2;

				unitActions.AddUnitCommand(repairUnit.unitID, priority, () =>
				{
					UnitEx liveRepairUnit = GameState.GetUnit(repairUnit.unitID);
					UnitEx unitToRepair = GameState.GetUnit(unitToFix.unitID);
					if (liveRepairUnit != null && unitToRepair != null)
					{
						if (repairUnit.curAction == ActionType.moDone)
							GameState.GetUnit(repairUnit.unitID)?.DoRepair(unitToRepair);
					}
				});
			}

			// If we are overwhelmed, build more repair units to meet demand
			if (damagedStructureCount/4 + 1 > owner.units.GetListForType(m_BuildRepairUnitTask.repairUnitType).Count)
				m_BuildRepairUnitTask.targetCountToBuild = damagedStructureCount/4 + 1;

			return true;
		}
	}
}
