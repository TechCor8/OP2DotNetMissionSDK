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
	public class MaintainWallsTask : Task
	{
		private BuildEarthworkerTask m_BuildEarthworkerTask;


		public MaintainWallsTask(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			// No completion state specified
			return false;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildEarthworkerTask(ownerID));
			m_BuildEarthworkerTask = new BuildEarthworkerTask(ownerID);
		}

		protected override TaskResult PerformTask(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			TaskResult result = new TaskResult(TaskRequirements.None);

			if (!m_BuildEarthworkerTask.IsTaskComplete(stateSnapshot))
				result += m_BuildEarthworkerTask.PerformTaskTree(stateSnapshot, restrictedRequirements, unitActions);

			// Fail Check: Not enough ore for walls
			if (owner.ore < 50 || IsRequirementRestricted(restrictedRequirements, TaskRequirements.Common))
				return result + new TaskResult(TaskRequirements.Common);

			int structuresThatNeedWalls = 0;

			// Find structure that needs walls
			foreach (StructureState building in owner.units.GetStructures())
			{
				bool needsWall = false;

				switch (building.unitType)
				{
					case map_id.GuardPost:
					case map_id.AdvancedLab:
					case map_id.Spaceport:
						needsWall = true;
						break;
				}

				if (!needsWall)
					continue;

				// Structure must be connected, or we will accidentally cut it off
				if (!stateSnapshot.commandMap.ConnectsTo(ownerID, building.GetRect()))
					continue;

				// Get tile for wall
				LOCATION tile;
				if (!GetEmptyTileAroundRect(stateSnapshot, building.GetRect(true), out tile))
					continue;

				// Get earthworker
				UnitState earthworker = owner.units.GetClosestUnitOfType(map_id.Earthworker, tile);

				if (earthworker == null || earthworker.curAction != ActionType.moDone)
				{
					++structuresThatNeedWalls;
					continue;
				}

				BuildStructureTask.ClearDeployArea(earthworker, map_id.LightTower, tile, stateSnapshot, ownerID, unitActions);

				unitActions.AddUnitCommand(earthworker.unitID, 0, () => GameState.GetUnit(earthworker.unitID)?.DoBuildWall(map_id.Wall, new MAP_RECT(tile, new LOCATION(1, 1))));
				break;
			}

			// If we are overwhelmed, build more earthworkers to meet demand
			if (structuresThatNeedWalls/4 + 1 > owner.units.earthWorkers.Count)
				m_BuildEarthworkerTask.targetCountToBuild = structuresThatNeedWalls/4 + 1;

			return result;
		}

		private bool GetEmptyTileAroundRect(StateSnapshot stateSnapshot, MAP_RECT area, out LOCATION tile)
		{
			for (int tx=area.xMin-1; tx <= area.xMax; ++tx)
			{
				tile = new LOCATION(tx, area.yMin-1);
				if (IsEmptyTile(stateSnapshot, tx, area.yMin-1)) return true;
			}

			for (int ty=area.yMin-1; ty <= area.yMax; ++ty)
			{
				tile = new LOCATION(area.xMin-1, ty);
				if (IsEmptyTile(stateSnapshot, area.xMin-1, ty)) return true;
			}

			for (int tx=area.xMin-1; tx <= area.xMax; ++tx)
			{
				tile = new LOCATION(tx, area.yMax);
				if (IsEmptyTile(stateSnapshot, tx, area.yMax)) return true;
			}

			for (int ty=area.yMin-1; ty <= area.yMax; ++ty)
			{
				tile = new LOCATION(area.xMax, ty);
				if (IsEmptyTile(stateSnapshot, area.xMax, ty)) return true;
			}

			tile = new LOCATION(0, 0);

			return false;
		}

		private bool IsEmptyTile(StateSnapshot stateSnapshot, int tx, int ty)
		{
			if (!stateSnapshot.tileMap.IsTilePassable(tx, ty))
				return false;
			if (stateSnapshot.tileMap.GetCellType(new LOCATION(tx, ty)) == CellType.Tube0)
				return false;
			if (BuildStructureTask.IsAreaBlocked(stateSnapshot, new MAP_RECT(tx, ty, 1, 1), ownerID, true))
				return false;

			return true;
		}
	}
}
