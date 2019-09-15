using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.VehicleTasks
{
	/// <summary>
	/// Task for building a single unit from a vehicle factory.
	/// </summary>
	public class BuildSingleVehicleTask : Task
	{
		protected map_id m_VehicleToBuild;
		protected map_id m_VehicleToBuildCargo;


		public BuildSingleVehicleTask(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			// This task never completes, as it is designed to build one unit and forget about it.
			return false;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new MaintainVehicleFactoryTask(ownerID));
		}

		protected override bool CanPerformTask(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			return owner.CanBuildUnit(stateSnapshot, m_VehicleToBuild, m_VehicleToBuildCargo);
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
			unitActions.AddUnitCommand(factoryID, 0, () => GameState.GetUnit(factoryID)?.DoProduce(vehicleToBuild, vehicleToBuildCargo));
		}

		/// <summary>
		/// Sets the vehicle to build.
		/// </summary>
		public void SetVehicle(map_id vehicleToBuild, map_id vehicleToBuildCargo)
		{
			m_VehicleToBuild = vehicleToBuild;
			m_VehicleToBuildCargo = vehicleToBuildCargo;
		}
	}

	/// <summary>
	/// Class for building a single unit from an arachnid factory.
	/// </summary>
	public class BuildSingleArachnidTask : BuildSingleVehicleTask
	{
		public BuildSingleArachnidTask(int ownerID) : base(ownerID) { }

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new MaintainArachnidFactoryTask(ownerID));
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
			unitActions.AddUnitCommand(factoryID, 0, () => GameState.GetUnit(factoryID)?.DoProduce(vehicleToBuild, vehicleToBuildCargo));
		}
	}
}
