using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Vehicle
{
	public abstract class BuildVehicleTask : Task
	{
		protected map_id m_VehicleToBuild;
		protected map_id m_VehicleToBuildCargo;
		public int targetCountToBuild;


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

		protected override bool PerformTask()
		{
			// Fail Check: Research
			//if (!owner.player.HasTechnology(TechInfo.GetTechID(m_VehicleToBuild)))
			//	return false;

			UnitInfo vehicleInfo = new UnitInfo(m_VehicleToBuild);

			// Fail Check: Vehicle cost
			if (owner.player.Ore() < vehicleInfo.GetOreCost(owner.player.playerID)) return false;
			if (owner.player.RareOre() < vehicleInfo.GetRareOreCost(owner.player.playerID)) return false;

			// Get factory to produce
			UnitEx factory = owner.units.vehicleFactories.Find((UnitEx unit) => unit.IsEnabled() && !unit.IsBusy());
			if (factory == null)
				return true;

			factory.DoProduce(m_VehicleToBuild, m_VehicleToBuildCargo);

			return true;
		}
	}
}
