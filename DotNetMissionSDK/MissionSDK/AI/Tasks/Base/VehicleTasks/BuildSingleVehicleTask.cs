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

		protected BuildStructureTask m_BuildStructureTask;


		public BuildSingleVehicleTask(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			// This task never completes, as it is designed to build one unit and forget about it.
			return false;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildVehicleFactoryTask(ownerID));

			// This task is optional and not required. Used for constructing additional factories.
			m_BuildStructureTask = new BuildVehicleFactoryTask(ownerID);
			m_BuildStructureTask.GeneratePrerequisites();
		}

		protected override bool CanPerformTask(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			return owner.CanBuildUnit(stateSnapshot, m_VehicleToBuild, m_VehicleToBuildCargo);
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			if (!CanPerformTask(stateSnapshot))
				return false;

			PlayerState owner = stateSnapshot.players[ownerID];

			// Get factory to produce
			FactoryState factory = owner.units.vehicleFactories.FirstOrDefault((FactoryState unit) => unit.isEnabled && !unit.isBusy);
			
			if (factory == null)
				return false;

			ProduceUnit(unitActions, factory.unitID, m_VehicleToBuild, m_VehicleToBuildCargo);

			return true;
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

		/// <summary>
		/// Adds a factory to the base for producing more units.
		/// </summary>
		public virtual void AddFactory(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			m_BuildStructureTask.targetCountToBuild = owner.units.vehicleFactories.Count+1;
			m_BuildStructureTask.PerformTaskTree(stateSnapshot, unitActions);
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
			AddPrerequisite(new BuildArachnidFactoryTask(ownerID));

			// This task is optional and not required. Used for constructing additional factories.
			m_BuildStructureTask = new BuildArachnidFactoryTask(ownerID);
			m_BuildStructureTask.GeneratePrerequisites();
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			if (!CanPerformTask(stateSnapshot))
				return false;

			PlayerState owner = stateSnapshot.players[ownerID];

			// Get factory to produce
			FactoryState factory = owner.units.arachnidFactories.FirstOrDefault((FactoryState unit) => unit.isEnabled && !unit.isBusy);
			
			if (factory == null)
				return false;

			ProduceUnit(unitActions, factory.unitID, m_VehicleToBuild, m_VehicleToBuildCargo);

			return true;
		}

		private static void ProduceUnit(BotCommands unitActions, int factoryID, map_id vehicleToBuild, map_id vehicleToBuildCargo)
		{
			unitActions.AddUnitCommand(factoryID, 0, () => GameState.GetUnit(factoryID)?.DoProduce(vehicleToBuild, vehicleToBuildCargo));
		}

		/// <summary>
		/// Adds a factory to the base for producing more units.
		/// </summary>
		public override void AddFactory(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			m_BuildStructureTask.targetCountToBuild = owner.units.arachnidFactories.Count+1;
			m_BuildStructureTask.PerformTaskTree(stateSnapshot, unitActions);
		}
	}
}
