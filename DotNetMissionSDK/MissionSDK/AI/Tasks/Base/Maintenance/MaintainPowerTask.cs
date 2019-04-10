using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class MaintainPowerTask : Task
	{
		private List<BuildStructureTask> m_Prerequisites = new List<BuildStructureTask>();

		private BuildTokamakTask m_BuildTokamakTask;
		private BuildMHDGeneratorTask m_BuildMHDGeneratorTask;
		private BuildSolarArrayTask m_BuildSolarArrayTask;


		public MaintainPowerTask() { }
		public MaintainPowerTask(PlayerInfo owner) : base(owner) { }

		public override bool IsTaskComplete()
		{
			if (owner.player.GetAmountPowerAvailable() < 100)
				return false;

			return true;
		}

		public override void GeneratePrerequisites()
		{
			m_Prerequisites.Add(m_BuildTokamakTask = new BuildTokamakTask(owner));
			m_Prerequisites.Add(m_BuildMHDGeneratorTask = new BuildMHDGeneratorTask(owner));
			m_Prerequisites.Add(m_BuildSolarArrayTask = new BuildSolarArrayTask(owner));

			foreach (BuildStructureTask task in m_Prerequisites)
			{
				task.targetCountToBuild = 0;
				AddPrerequisite(task);
			}
		}

		protected override bool PerformTask()
		{
			if (owner.player.IsEden())
			{
				if (owner.player.HasTechnology(TechInfoOld.GetTechID(map_id.GeothermalPlant)))
				{
					// TODO: Check if vents are near a CC. If yes, build geocon and return
				}
			}

			BuildPowerPlant();

			return true;
		}

		private void BuildPowerPlant()
		{
			// Don't build more power plants if we aren't using all the ones we have
			List<UnitEx> powerPlants = new List<UnitEx>(owner.units.tokamaks);
			powerPlants.AddRange(owner.units.mhdGenerators);
			powerPlants.AddRange(owner.units.solarPowerArrays);
			powerPlants.AddRange(owner.units.geothermalPlants);

			foreach (UnitEx powerPlant in powerPlants)
			{
				if (!powerPlant.IsEnabled())
					return;
			}

			// Determine power plant type to build
			map_id powerPlantTypeToBuild = map_id.Tokamak;

			if (!owner.player.IsEden())
			{
				UnitInfo moduleInfo = new UnitInfo(map_id.MHDGenerator);

				// Fail Check: Kit cost
				bool canAfford = true;
				if (owner.player.Ore() < moduleInfo.GetOreCost(owner.player.playerID)) canAfford = false;
				if (owner.player.RareOre() < moduleInfo.GetRareOreCost(owner.player.playerID)) canAfford = false;

				if (canAfford && owner.player.HasTechnology(TechInfoOld.GetTechID(map_id.MHDGenerator)))
					powerPlantTypeToBuild = map_id.MHDGenerator;
			}

			if (owner.player.HasTechnology(TechInfoOld.GetTechID(map_id.SolarPowerArray)))
			{
				UnitInfo moduleInfo = new UnitInfo(map_id.SolarPowerArray);

				// Fail Check: Kit cost
				bool canAfford = true;
				if (owner.player.Ore() < moduleInfo.GetOreCost(owner.player.playerID)) canAfford = false;
				if (owner.player.RareOre() < moduleInfo.GetRareOreCost(owner.player.playerID)) canAfford = false;

				if (canAfford && owner.units.solarPowerArrays.Count < owner.units.solarSatelliteCount)
					powerPlantTypeToBuild = map_id.SolarPowerArray;
			}

			// Convecs with loaded power kits get priority
			UnitEx convec = owner.units.convecs.Find((UnitEx unit) =>
			{
				switch (unit.GetCargo())
				{
					case map_id.Tokamak:	return true;
					case map_id.MHDGenerator: return true;
					case map_id.SolarPowerArray:
						if (owner.units.solarPowerArrays.Count < owner.units.solarSatelliteCount)
						{
							return true;
						}
						break;
				}

				return false;
			});

			if (convec != null)
				powerPlantTypeToBuild = convec.GetCargo();

			// Set the next power plant to build
			switch (powerPlantTypeToBuild)
			{
				case map_id.Tokamak:				m_BuildTokamakTask.targetCountToBuild = owner.units.tokamaks.Count+1;					break;
				case map_id.MHDGenerator:			m_BuildMHDGeneratorTask.targetCountToBuild = owner.units.mhdGenerators.Count+1;			break;
				case map_id.SolarPowerArray:		m_BuildSolarArrayTask.targetCountToBuild = owner.units.solarPowerArrays.Count+1;		break;
			}
		}
	}
}
