using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;

namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	/// <summary>
	/// Orders repair units (convec, spider, repair vehicle) to repair damaged structures.
	/// </summary>
	public abstract class RepairStructureTask : Task
	{
		public const float CriticalDamagePercentage = 0.75f;

		protected map_id m_StructureToRepair = map_id.Agridome;

		private BuildRepairUnitTask m_BuildRepairUnitTask;


		public RepairStructureTask(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			UnitInfoState info = owner.GetUnitInfo(m_StructureToRepair);

			// Task is complete if all structures of type have adequate HP
			foreach (UnitState unit in owner.units.GetListForType(m_StructureToRepair))
			{
				StructureState building = (StructureState)unit;

				// Must not be under construction
				if (building.lastCommand == CommandType.ctMoDevelop || 
					building.lastCommand == CommandType.ctMoUnDevelop)
					continue;

				// Critical only repairs
				if (building.damage / (float)info.hitPoints > CriticalDamagePercentage)
					return false;
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			//AddPrerequisite(new BuildConvecTask(ownerID));
			AddPrerequisite(m_BuildRepairUnitTask = new BuildRepairUnitTask(ownerID));
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			//if (!m_BuildRepairUnitTask.IsTaskComplete(stateSnapshot))
			//	m_BuildRepairUnitTask.PerformTaskTree(stateSnapshot, unitActions);

			// Fail Check: Not enough ore for repairs
			if (owner.ore < 50)
				return false;

			// Find damaged structures
			foreach (UnitState unit in owner.units.GetListForType(m_StructureToRepair))
			{
				StructureState unitToFix = (StructureState)unit;

				// Must not be under construction
				if (unitToFix.lastCommand == CommandType.ctMoDevelop || 
					unitToFix.lastCommand == CommandType.ctMoUnDevelop)
					continue;

				// If damage not critical, skip unit
				UnitInfoState info = owner.GetUnitInfo(unitToFix.unitType);

				if (unitToFix.damage / (float)info.hitPoints < CriticalDamagePercentage)
					continue;

				// Get repair unit
				UnitState repairUnit = owner.units.GetClosestUnitOfType(m_BuildRepairUnitTask.repairUnitType, unitToFix.position);

				if (repairUnit == null)
					continue;

				unitActions.AddUnitCommand(repairUnit.unitID, 4, () =>
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

			return true;
		}
	}
}
