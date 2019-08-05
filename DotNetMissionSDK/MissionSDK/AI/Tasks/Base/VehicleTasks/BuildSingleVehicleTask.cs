using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;

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


		public BuildSingleVehicleTask() { }
		public BuildSingleVehicleTask(PlayerInfo owner) : base(owner) { }

		public override bool IsTaskComplete()
		{
			// This task never completes, as it is designed to build one unit and forget about it.
			return false;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildVehicleFactoryTask());

			// This task is optional and not required. Used for constructing additional factories.
			m_BuildStructureTask = new BuildVehicleFactoryTask(owner);
			m_BuildStructureTask.GeneratePrerequisites();
		}

		protected override bool CanPerformTask()
		{
			return owner.player.CanBuildUnit(m_VehicleToBuild, m_VehicleToBuildCargo);
		}

		protected override bool PerformTask()
		{
			if (!CanPerformTask())
				return false;

			// Get factory to produce
			UnitEx factory = owner.units.vehicleFactories.Find((UnitEx unit) => unit.IsEnabled() && !unit.IsBusy());
			
			if (factory == null)
				return false;

			factory.DoProduce(m_VehicleToBuild, m_VehicleToBuildCargo);

			return true;
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
		public virtual void AddFactory()
		{
			m_BuildStructureTask.targetCountToBuild = owner.units.vehicleFactories.Count+1;
			m_BuildStructureTask.PerformTaskTree();
		}
	}

	/// <summary>
	/// Class for building a single unit from an arachnid factory.
	/// </summary>
	public class BuildSingleArachnidTask : BuildSingleVehicleTask
	{
		public BuildSingleArachnidTask() { }
		public BuildSingleArachnidTask(PlayerInfo owner) : base(owner) { }

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildArachnidFactoryTask());

			// This task is optional and not required. Used for constructing additional factories.
			m_BuildStructureTask = new BuildArachnidFactoryTask(owner);
			m_BuildStructureTask.GeneratePrerequisites();
		}

		protected override bool PerformTask()
		{
			if (!CanPerformTask())
				return false;

			// Get factory to produce
			UnitEx factory = owner.units.arachnidFactories.Find((UnitEx unit) => unit.IsEnabled() && !unit.IsBusy());
			
			if (factory == null)
				return false;

			factory.DoProduce(m_VehicleToBuild, m_VehicleToBuildCargo);

			return true;
		}

		/// <summary>
		/// Adds a factory to the base for producing more units.
		/// </summary>
		public override void AddFactory()
		{
			m_BuildStructureTask.targetCountToBuild = owner.units.arachnidFactories.Count+1;
			m_BuildStructureTask.PerformTaskTree();
		}
	}
}
