using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
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
			return units.Count >= targetCountToBuild;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildVehicleFactoryTask(ownerID));
		}

		protected override bool CanPerformTask(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			return owner.CanBuildUnit(stateSnapshot, m_VehicleToBuild, m_VehicleToBuildCargo);
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, List<Action> unitActions)
		{
			if (!CanPerformTask(stateSnapshot))
				return false;

			PlayerState owner = stateSnapshot.players[ownerID];

			// Get factory to produce
			FactoryState factory = owner.units.vehicleFactories.FirstOrDefault((FactoryState unit) => unit.isEnabled && !unit.isBusy);
			
			if (factory == null)
				return true;

			ProduceUnit(unitActions, factory.unitID, m_VehicleToBuild, m_VehicleToBuildCargo);

			return true;
		}

		private static void ProduceUnit(List<Action> unitActions, int factoryID, map_id vehicleToBuild, map_id vehicleToBuildCargo)
		{
			unitActions.Add(() => GameState.GetUnit(factoryID)?.DoProduce(vehicleToBuild, vehicleToBuildCargo));
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
			AddPrerequisite(new BuildArachnidFactoryTask(ownerID));
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, List<Action> unitActions)
		{
			if (!CanPerformTask(stateSnapshot))
				return false;

			PlayerState owner = stateSnapshot.players[ownerID];

			// Get factory to produce
			FactoryState factory = owner.units.arachnidFactories.FirstOrDefault((FactoryState unit) => unit.isEnabled && !unit.isBusy);
			
			if (factory == null)
				return true;

			ProduceUnit(unitActions, factory.unitID, m_VehicleToBuild, m_VehicleToBuildCargo);

			return true;
		}

		private static void ProduceUnit(List<Action> unitActions, int factoryID, map_id vehicleToBuild, map_id vehicleToBuildCargo)
		{
			unitActions.Add(() => GameState.GetUnit(factoryID)?.DoProduce(vehicleToBuild, vehicleToBuildCargo));
		}
	}
}
