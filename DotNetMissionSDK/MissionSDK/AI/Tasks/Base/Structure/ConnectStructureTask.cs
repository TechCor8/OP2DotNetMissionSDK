using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	public class ConnectStructureTask : Task
	{
		protected map_id m_StructureToConnect = map_id.Agridome;

		public ConnectStructureTask(map_id structureToConnect, int ownerID) : base(ownerID) { m_StructureToConnect = structureToConnect; }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			// Skip task if structure does not need tubes
			if (!BuildStructureTask.NeedsTube(m_StructureToConnect))
				return true;

			PlayerState owner = stateSnapshot.players[ownerID];

			// Task is complete if all structures are connected
			foreach (StructureState building in owner.units.GetListForType(m_StructureToConnect))
			{
				if (!stateSnapshot.commandMap.ConnectsTo(ownerID, building.GetRect()))
					return false;
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildEarthworkerTask(ownerID));
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			// Skip task if structure does not need tubes
			if (!BuildStructureTask.NeedsTube(m_StructureToConnect))
				return true;

			PlayerState owner = stateSnapshot.players[ownerID];

			// Fail Check: Not enough ore for tubes
			if (owner.ore < 50)
				return false;

			List<UnitState> disconnectedStructures = new List<UnitState>();

			// Create list of disconnected structures
			foreach (UnitState unitToFix in owner.units.GetListForType(m_StructureToConnect))
			{
				MAP_RECT buildingRect = stateSnapshot.structureInfo[unitToFix.unitType].GetRect(unitToFix.position);

				if (stateSnapshot.commandMap.ConnectsTo(ownerID, buildingRect))
					continue;

				disconnectedStructures.Add(unitToFix);
			}


			// Find idle earthworkers
			foreach (UnitState earthworker in owner.units.earthWorkers)
			{
				//if (earthworker.curAction != ActionType.moDone)
				//	continue;

				// Find closest disconnected structure
				UnitState unitToFix = GetClosestUnit(disconnectedStructures, earthworker.position);
				if (unitToFix == null)
					break;

				disconnectedStructures.Remove(unitToFix);

				MAP_RECT buildingRect = stateSnapshot.structureInfo[unitToFix.unitType].GetRect(unitToFix.position);

				// Get path from tube network to structure
				LOCATION[] path = stateSnapshot.commandMap.GetPathToClosestConnectedTile(ownerID, buildingRect);
				if (path == null)
					continue;

				// Fix disconnected structure
				for (int i = 0; i < path.Length; ++i)
				{
					if (stateSnapshot.tileMap.GetCellType(path[i]) == CellType.Tube0)
						continue;

					BuildStructureTask.ClearDeployArea(earthworker, map_id.LightTower, path[i], stateSnapshot, ownerID, unitActions);

					unitActions.AddUnitCommand(earthworker.unitID, 1, () => GameState.GetUnit(earthworker.unitID)?.DoBuildWall(map_id.Tube, new MAP_RECT(path[i], new LOCATION(1, 1))));
					break;
				}
			}

			return true;
		}

		private UnitState GetClosestUnit(IEnumerable<UnitState> list, LOCATION position)
		{
			UnitState closestUnit = null;
			int closestDistance = 999999;

			foreach (UnitState unit in list)
			{
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
	}
}
