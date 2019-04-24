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
		protected BuildStructureTask m_BuildStructureTask;

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
			UnitInfo vehicleInfo = new UnitInfo(m_VehicleToBuild);

			// Fail Check: Research
			TechInfo techInfo = Research.GetTechInfo(vehicleInfo.GetResearchTopic());

			if (!owner.player.HasTechnology(techInfo.GetTechID()))
				return false;

			if (m_VehicleToBuildCargo != map_id.None)
			{
				// Fail Check: Cargo Research
				UnitInfo cargoInfo = new UnitInfo(m_VehicleToBuildCargo);
				TechInfo cargoTechInfo = Research.GetTechInfo(vehicleInfo.GetResearchTopic());

				if (!owner.player.HasTechnology(cargoTechInfo.GetTechID()))
					return false;

				// Fail Check: Vehicle cost
				if (owner.player.Ore() < vehicleInfo.GetOreCost(owner.player.playerID) + cargoInfo.GetOreCost(owner.player.playerID)) return false;
				if (owner.player.RareOre() < vehicleInfo.GetRareOreCost(owner.player.playerID) + cargoInfo.GetRareOreCost(owner.player.playerID)) return false;
			}
			else
			{
				// Fail Check: Vehicle cost
				if (owner.player.Ore() < vehicleInfo.GetOreCost(owner.player.playerID)) return false;
				if (owner.player.RareOre() < vehicleInfo.GetRareOreCost(owner.player.playerID)) return false;
			}

			return true;
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
