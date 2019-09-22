using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.VehicleTasks
{
	/// <summary>
	/// Abstract class for building units from a vehicle factory.
	/// </summary>
	public abstract class BuildVehicleTask : Task
	{
		protected map_id m_VehicleToBuild;
		protected map_id m_VehicleToBuildCargo;
		public int targetCountToBuild = 1;


		public BuildVehicleTask(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			IReadOnlyCollection<UnitState> units = owner.units.GetListForType(m_VehicleToBuild);
			int unitCount = units.Count((unit) => unit.weapon == m_VehicleToBuildCargo);

			return unitCount >= targetCountToBuild;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new MaintainVehicleFactoryTask(ownerID));
		}

		protected override bool CanPerformTask(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			return owner.CanBuildUnit(m_VehicleToBuild, m_VehicleToBuildCargo);
		}

		protected override TaskResult PerformTask(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Fail Check: Research and Cost
			TaskResult buildResult;
			if (!TaskResult.CanBuildUnit(out buildResult, stateSnapshot, owner, restrictedRequirements, m_VehicleToBuild, m_VehicleToBuildCargo))
				return buildResult;
			
			// Get factory to produce
			FactoryState factory = owner.units.vehicleFactories.FirstOrDefault((FactoryState unit) => unit.isEnabled && !unit.isBusy);
			
			if (factory == null)
				return new TaskResult(TaskRequirements.None);

			ProduceUnit(unitActions, factory.unitID, m_VehicleToBuild, m_VehicleToBuildCargo);

			return new TaskResult(TaskRequirements.None);
		}

		private static void ProduceUnit(BotCommands unitActions, int factoryID, map_id vehicleToBuild, map_id vehicleToBuildCargo)
		{
			unitActions.AddUnitCommand(factoryID, 1, () => GameState.GetUnit(factoryID)?.DoProduce(vehicleToBuild, vehicleToBuildCargo));
		}

		/// <summary>
		/// Gets the list of structures to activate.
		/// </summary>
		/// <param name="stateSnapshot">The state snapshot to use for performing task calculations.</param>
		/// <param name="structureIDs">The list to add structures to.</param>
		public override void GetStructuresToActivate(StateSnapshot stateSnapshot, List<int> structureIDs)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// If there are not enough structures of this type, parse priorities in children
			IReadOnlyCollection<UnitState> units = owner.units.GetListForType(m_VehicleToBuild);
			int unitCount = units.Count((unit) => unit.weapon == m_VehicleToBuildCargo);

			if (unitCount < targetCountToBuild)
				base.GetStructuresToActivate(stateSnapshot, structureIDs);
		}
	}

	/// <summary>
	/// Abstract class for building units from an arachnid factory.
	/// </summary>
	public abstract class BuildArachnidTask : BuildVehicleTask
	{
		public BuildArachnidTask(int ownerID) : base(ownerID) { }

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new MaintainArachnidFactoryTask(ownerID));
			AddPrerequisite(new ResearchTask(ownerID, new UnitInfo(m_VehicleToBuild).GetResearchTopic()));
			//AddPrerequisite(new ResearchTask(ownerID, new UnitInfo(m_VehicleToBuildCargo).GetResearchTopic()));
		}

		protected override TaskResult PerformTask(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Fail Check: Research and Cost
			TaskResult buildResult;
			if (!TaskResult.CanBuildUnit(out buildResult, stateSnapshot, owner, restrictedRequirements, m_VehicleToBuild, m_VehicleToBuildCargo))
				return buildResult;

			// Get factory to produce
			FactoryState factory = owner.units.arachnidFactories.FirstOrDefault((FactoryState unit) => unit.isEnabled && !unit.isBusy);
			
			if (factory == null)
				return new TaskResult(TaskRequirements.None);

			ProduceUnit(unitActions, factory.unitID, m_VehicleToBuild, m_VehicleToBuildCargo);

			return new TaskResult(TaskRequirements.None);
		}

		private static void ProduceUnit(BotCommands unitActions, int factoryID, map_id vehicleToBuild, map_id vehicleToBuildCargo)
		{
			unitActions.AddUnitCommand(factoryID, 1, () => GameState.GetUnit(factoryID)?.DoProduce(vehicleToBuild, vehicleToBuildCargo));
		}
	}
}
