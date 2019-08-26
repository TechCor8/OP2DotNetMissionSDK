using DotNetMissionSDK.AI.Tasks.Base.Maintenance;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of keeping structures in good repair.
	/// </summary>
	public class RepairStructuresGoal : Goal
	{
		public RepairStructuresGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			RepairStructuresTask repairTask = new RepairStructuresTask(ownerID);
			m_Task = repairTask;
			m_Task.GeneratePrerequisites();

			repairTask.repairCriticalOnly = false;
		}

		/// <summary>
		/// Updates the importance of this goal.
		/// </summary>
		public override void UpdateImportance(StateSnapshot stateSnapshot, PlayerState owner)
		{
			float lowestPercentage = 1;

			// Importance increases as damage becomes severe.
			foreach (StructureState building in owner.units.GetStructures())
			{
				UnitInfoState info = owner.GetUnitInfo(building.unitType);

				float hpRemaining = 1.0f - (building.damage / (float)info.hitPoints);

				if (hpRemaining < lowestPercentage)
					lowestPercentage = hpRemaining;
			}

			importance = 1 - Clamp(lowestPercentage);

			importance = Clamp(importance * weight);
		}
	}
}
