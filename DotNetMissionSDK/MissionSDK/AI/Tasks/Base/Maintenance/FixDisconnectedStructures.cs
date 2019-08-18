using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class FixDisconnectedStructures : Task
	{
		private BuildEarthworkerTask m_BuildEarthworkerTask;


		public FixDisconnectedStructures(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Task is complete if all structures are connected
			foreach (StructureState building in owner.units.GetStructures())
			{
				if (!BuildStructureTask.NeedsTube(building.unitType))
					continue;

				if (!owner.commandMap.ConnectsTo(building.GetRect()))
					return false;
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildEarthworkerTask(ownerID));
			m_BuildEarthworkerTask = new BuildEarthworkerTask(ownerID);
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, List<Action> unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			if (!m_BuildEarthworkerTask.IsTaskComplete(stateSnapshot))
				m_BuildEarthworkerTask.PerformTaskTree(stateSnapshot, unitActions);

			// Fail Check: Not enough ore for tubes
			if (owner.ore < 50)
				return false;

			int structuresThatNeedTubes = 0;

			// Find disconnected structures
			foreach (UnitState unitToFix in owner.units.GetStructures())
			{
				if (!BuildStructureTask.NeedsTube(unitToFix.unitType))
					continue;

				MAP_RECT buildingRect = stateSnapshot.structureInfo[unitToFix.unitType].GetRect(unitToFix.position);

				if (owner.commandMap.ConnectsTo(buildingRect))
					continue;

				++structuresThatNeedTubes;

				// Get earthworker
				UnitState earthworker = owner.units.GetClosestUnitOfType(map_id.Earthworker, unitToFix.position);

				if (earthworker == null || earthworker.curAction != ActionType.moDone)
					continue;

				// Get path from tube network to structure
				LOCATION[] path = owner.commandMap.GetPathToClosestConnectedTile(buildingRect);
				if (path == null)
					continue;

				// Fix disconnected structure
				for (int i = 0; i < path.Length; ++i)
				{
					if (stateSnapshot.tileMap.GetCellType(path[i]) == CellType.Tube0)
						continue;

					BuildStructureTask.ClearDeployArea(earthworker, map_id.LightTower, path[i], stateSnapshot, ownerID, unitActions);

					unitActions.Add(() => GameState.GetUnit(earthworker.unitID)?.DoBuildWall(map_id.Tube, new MAP_RECT(path[i], new LOCATION(1, 1))));
					break;
				}
			}

			// If we are overwhelmed, build more earthworkers to meet demand
			if (structuresThatNeedTubes/4 + 1 > owner.units.earthWorkers.Count)
				m_BuildEarthworkerTask.targetCountToBuild = structuresThatNeedTubes/4 + 1;

			return true;
		}
	}
}
