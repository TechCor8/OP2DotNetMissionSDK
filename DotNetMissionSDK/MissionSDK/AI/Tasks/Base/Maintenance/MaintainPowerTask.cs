using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class MaintainPowerTask : Task
	{
		private const int MaxGeothermalPlantDistanceToCC = 55;

		private List<Task> m_Prerequisites = new List<Task>();

		private MaintainTokamakTask m_MaintainTokamakTask;
		private MaintainMHDGeneratorTask m_MaintainMHDGeneratorTask;
		private MaintainSolarArrayTask m_MaintainSolarArrayTask;
		private BuildGeoConTask m_BuildGeoConTask;


		public MaintainPowerTask(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			if (owner.amountPowerAvailable < 100)
				return false;

			return true;
		}

		public override void GeneratePrerequisites()
		{
			m_Prerequisites.Add(m_MaintainTokamakTask = new MaintainTokamakTask(ownerID));
			m_Prerequisites.Add(m_MaintainMHDGeneratorTask = new MaintainMHDGeneratorTask(ownerID));
			m_Prerequisites.Add(m_MaintainSolarArrayTask = new MaintainSolarArrayTask(ownerID));
			m_Prerequisites.Add(m_BuildGeoConTask = new BuildGeoConTask(ownerID));

			m_MaintainTokamakTask.targetCountToMaintain = 0;
			m_MaintainMHDGeneratorTask.targetCountToMaintain = 0;
			m_MaintainSolarArrayTask.targetCountToMaintain = 0;
			m_BuildGeoConTask.targetCountToBuild = 0;

			foreach (Task task in m_Prerequisites)
				AddPrerequisite(task);
		}

		protected override TaskResult PerformTask(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Don't build more power plants if we aren't using all the ones we have
			List<StructureState> powerPlants = new List<StructureState>(owner.units.tokamaks);
			powerPlants.AddRange(owner.units.mhdGenerators);
			powerPlants.AddRange(owner.units.solarPowerArrays);
			powerPlants.AddRange(owner.units.geothermalPlants);

			foreach (StructureState powerPlant in powerPlants)
			{
				if (!powerPlant.isEnabled)
					return new TaskResult(TaskRequirements.None);
			}

			// Build new power plant
			if (!BuildGeothermalPlant(stateSnapshot, owner, unitActions))
				BuildPowerPlant(stateSnapshot, owner);

			return new TaskResult(TaskRequirements.None);
		}

		private void BuildPowerPlant(StateSnapshot stateSnapshot, PlayerState owner)
		{
			// Determine power plant type to build. Default to Tokamak.
			map_id powerPlantTypeToBuild = map_id.Tokamak;

			// Build MHD if available
			if (owner.CanBuildUnit(stateSnapshot, map_id.MHDGenerator))
				powerPlantTypeToBuild = map_id.MHDGenerator;

			// Build Solar Power Array if available and satellite in orbit
			if (owner.CanBuildUnit(stateSnapshot, map_id.SolarPowerArray) && owner.units.solarPowerArrays.Count < owner.units.solarSatelliteCount)
				powerPlantTypeToBuild = map_id.SolarPowerArray;
			
			// Convecs with loaded power kits get priority
			ConvecState convec = owner.units.convecs.FirstOrDefault((ConvecState unit) =>
			{
				switch (unit.cargoType)
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
				powerPlantTypeToBuild = convec.cargoType;

			// Set the next power plant to build
			switch (powerPlantTypeToBuild)
			{
				case map_id.Tokamak:				m_MaintainTokamakTask.targetCountToMaintain = owner.units.tokamaks.Count+1;				break;
				case map_id.MHDGenerator:			m_MaintainMHDGeneratorTask.targetCountToMaintain = owner.units.mhdGenerators.Count+1;	break;
				case map_id.SolarPowerArray:		m_MaintainSolarArrayTask.targetCountToMaintain = owner.units.solarPowerArrays.Count+1;	break;
			}
		}

		private bool BuildGeothermalPlant(StateSnapshot stateSnapshot, PlayerState owner, BotCommands unitActions)
		{
			if (!owner.isEden)
				return false;

			// If geocon exists, use it
			if (owner.units.geoCons.Count > 0)
			{
				m_BuildGeoConTask.targetCountToBuild = 0;

				// Find fumarole to deploy on
				VehicleState geocon = owner.units.geoCons[0];
				GaiaUnitState fumarole = GetClosestUnoccupiedFumarole(stateSnapshot, geocon.position);
				if (fumarole == null)
					return false;

				unitActions.AddUnitCommand(geocon.unitID, 1, () => GameState.GetUnit(geocon.unitID)?.DoDeployMiner(fumarole.position.x, fumarole.position.y));
				return true;
			}

			// Check if we can build a geo con
			if (!m_BuildGeoConTask.CanPerformTaskTree(stateSnapshot))
				return false;

			// Check if fumaroles are near a CC. If yes, build geocon
			foreach (StructureState cc in owner.units.commandCenters)
			{
				GaiaUnitState fumarole = GetClosestUnoccupiedFumarole(stateSnapshot, cc.position);

				if (fumarole == null)
				{
					m_BuildGeoConTask.targetCountToBuild = 0;
					return false;
				}

				if (fumarole.position.GetDiagonalDistance(cc.position) < MaxGeothermalPlantDistanceToCC)
				{
					m_BuildGeoConTask.targetCountToBuild = 1;
					return true;
				}

				m_BuildGeoConTask.targetCountToBuild = 0;
			}

			return false;
		}

		private GaiaUnitState GetClosestUnoccupiedFumarole(StateSnapshot stateSnapshot, LOCATION position)
		{
			GaiaUnitState closestUnit = null;
			int closestDistance = 999999;

			foreach (GaiaUnitState unit in stateSnapshot.gaia.fumaroles)
			{
				// Detect if occupied
				bool isOccupied = false;
				for (int i=0; i < stateSnapshot.players.Count; ++i)
				{
					StructureState building = GetClosestBuildingOfType(stateSnapshot.players[i], map_id.Any, unit.position);
					if (building != null && building.position.GetDiagonalDistance(unit.position) < 2)
					{
						isOccupied = true;
						break;
					}
				}

				if (isOccupied)
					continue;

				// Closest distance
				int distance = position.GetDiagonalDistance(unit.position);
				if (distance < closestDistance)
				{
					closestUnit = unit;
					closestDistance = distance;
				}
			}

			return closestUnit;
		}

		private StructureState GetClosestBuildingOfType(PlayerState player, map_id type, LOCATION position)
		{
			StructureState closestUnit = null;
			int closestDistance = 999999;

			IEnumerable<UnitState> enumerator;

			if (type == map_id.Any)
				enumerator = player.units.GetStructures();
			else
				enumerator = player.units.GetListForType(type);

			foreach (StructureState unit in enumerator)
			{
				int distance = unit.position.GetDiagonalDistance(position);
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
