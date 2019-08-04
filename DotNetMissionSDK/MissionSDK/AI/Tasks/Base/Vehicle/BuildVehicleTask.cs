using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Vehicle
{
	/// <summary>
	/// Abstract class for building units from a vehicle factory.
	/// </summary>
	public abstract class BuildVehicleTask : Task
	{
		protected map_id m_VehicleToBuild;
		protected map_id m_VehicleToBuildCargo;
		public int targetCountToBuild = 1;


		public BuildVehicleTask() { }
		public BuildVehicleTask(PlayerInfo owner) : base(owner) { }

		public override bool IsTaskComplete()
		{
			List<UnitEx> units = owner.units.GetListForType(m_VehicleToBuild);
			return units.Count >= targetCountToBuild;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildVehicleFactoryTask());
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
				return true;

			factory.DoProduce(m_VehicleToBuild, m_VehicleToBuildCargo);

			return true;
		}
	}

	/// <summary>
	/// Abstract class for building units from an arachnid factory.
	/// </summary>
	public abstract class BuildArachnidTask : BuildVehicleTask
	{
		public BuildArachnidTask() { }
		public BuildArachnidTask(PlayerInfo owner) : base(owner) { }

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildArachnidFactoryTask());
		}

		protected override bool PerformTask()
		{
			if (!CanPerformTask())
				return false;

			// Get factory to produce
			UnitEx factory = owner.units.arachnidFactories.Find((UnitEx unit) => unit.IsEnabled() && !unit.IsBusy());
			
			if (factory == null)
				return true;

			factory.DoProduce(m_VehicleToBuild, m_VehicleToBuildCargo);

			return true;
		}
	}
}
