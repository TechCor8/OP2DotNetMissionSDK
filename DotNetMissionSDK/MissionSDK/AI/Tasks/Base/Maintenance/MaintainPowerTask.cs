using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.AI.Tasks.Base.Vehicle;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class MaintainPowerTask : Task
	{
		private const int MaxGeothermalPlantDistanceToCC = 55;

		private List<Task> m_Prerequisites = new List<Task>();

		private BuildTokamakTask m_BuildTokamakTask;
		private BuildMHDGeneratorTask m_BuildMHDGeneratorTask;
		private BuildSolarArrayTask m_BuildSolarArrayTask;
		private BuildGeoConTask m_BuildGeoConTask;


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
			m_Prerequisites.Add(m_BuildGeoConTask = new BuildGeoConTask(owner));

			m_BuildTokamakTask.targetCountToBuild = 0;
			m_BuildMHDGeneratorTask.targetCountToBuild = 0;
			m_BuildSolarArrayTask.targetCountToBuild = 0;
			m_BuildGeoConTask.targetCountToBuild = 0;

			foreach (Task task in m_Prerequisites)
				AddPrerequisite(task);
		}

		protected override bool PerformTask()
		{
			// Don't build more power plants if we aren't using all the ones we have
			List<UnitEx> powerPlants = new List<UnitEx>(owner.units.tokamaks);
			powerPlants.AddRange(owner.units.mhdGenerators);
			powerPlants.AddRange(owner.units.solarPowerArrays);
			powerPlants.AddRange(owner.units.geothermalPlants);

			foreach (UnitEx powerPlant in powerPlants)
			{
				if (!powerPlant.IsEnabled())
					return false;
			}

			// Build new power plant
			if (!BuildGeothermalPlant())
				BuildPowerPlant();

			return true;
		}

		private void BuildPowerPlant()
		{
			// Determine power plant type to build
			map_id powerPlantTypeToBuild = map_id.Tokamak;

			if (!owner.player.IsEden())
			{
				UnitInfo moduleInfo = new UnitInfo(map_id.MHDGenerator);

				// Fail Check: Kit cost
				bool canAfford = true;
				if (owner.player.Ore() < moduleInfo.GetOreCost(owner.player.playerID)) canAfford = false;
				if (owner.player.RareOre() < moduleInfo.GetRareOreCost(owner.player.playerID)) canAfford = false;

				UnitInfo unitInfo = new UnitInfo(map_id.MHDGenerator);
				TechInfo techInfo = Research.GetTechInfo(unitInfo.GetResearchTopic());

				if (canAfford && owner.player.HasTechnology(techInfo.GetTechID()))
					powerPlantTypeToBuild = map_id.MHDGenerator;
			}

			UnitInfo solarInfo = new UnitInfo(map_id.SolarPowerArray);
			TechInfo solarTechInfo = Research.GetTechInfo(solarInfo.GetResearchTopic());

			if (owner.player.HasTechnology(solarTechInfo.GetTechID()))
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

		private bool BuildGeothermalPlant()
		{
			if (!owner.player.IsEden())
				return false;

			// If geocon exists, use it
			if (owner.units.geoCons.Count > 0)
			{
				m_BuildGeoConTask.targetCountToBuild = 0;

				// Find fumarole to deploy on
				UnitEx geocon = owner.units.geoCons[0];
				UnitEx fumarole = GetClosestUnoccupiedFumarole(geocon.GetPosition());
				if (fumarole == null)
					return false;

				geocon.DoDeployMiner(fumarole.GetTileX(), fumarole.GetTileY());
				return true;
			}

			// Check if we can build a geo con
			if (!m_BuildGeoConTask.CanPerformTaskTree())
				return false;

			// Check if fumaroles are near a CC. If yes, build geocon
			foreach (UnitEx cc in owner.units.commandCenters)
			{
				UnitEx fumarole = GetClosestUnoccupiedFumarole(cc.GetPosition());

				if (fumarole == null)
				{
					m_BuildGeoConTask.targetCountToBuild = 0;
					return false;
				}

				if (fumarole.GetPosition().GetDiagonalDistance(cc.GetPosition()) < MaxGeothermalPlantDistanceToCC)
				{
					m_BuildGeoConTask.targetCountToBuild = 1;
					return true;
				}

				m_BuildGeoConTask.targetCountToBuild = 0;
			}

			return false;
		}

		private UnitEx GetClosestUnoccupiedFumarole(LOCATION position)
		{
			UnitEx closestUnit = null;
			int closestDistance = 999999;

			foreach (UnitEx unit in new PlayerUnitEnum(6))
			{
				if (unit.GetUnitType() != map_id.Fumarole)
					continue;

				LOCATION unitPos = unit.GetPosition();

				// Detect if occupied
				bool isOccupied = false;
				for (int i=0; i < TethysGame.NoPlayers(); ++i)
				{
					UnitEx building = GetClosestBuildingOfType(i, map_id.Any, unitPos);
					if (building != null && building.GetPosition().GetDiagonalDistance(unitPos) < 2)
					{
						isOccupied = true;
						break;
					}
				}

				if (isOccupied)
					continue;

				// Closest distance
				int distance = position.GetDiagonalDistance(unitPos);
				if (distance < closestDistance)
				{
					closestUnit = unit;
					closestDistance = distance;
				}
			}

			return closestUnit;
		}

		private UnitEx GetClosestBuildingOfType(int playerID, map_id type, LOCATION position)
		{
			UnitEx closestUnit = null;
			int closestDistance = 999999;

			OP2Enumerator enumerator;
			if (type == map_id.Any)
				enumerator = new PlayerAllBuildingEnum(playerID);
			else
				enumerator = new PlayerBuildingEnum(playerID, type);

			foreach (UnitEx unit in enumerator)
			{
				int distance = unit.GetPosition().GetDiagonalDistance(position);
				if (distance < closestDistance)
				{
					closestUnit = unit;
					closestDistance = distance;
				}
			}

			return closestUnit;
		}
	}
}
