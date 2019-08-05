using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class MaintainWallsTask : Task
	{
		private BuildEarthworkerTask m_BuildEarthworkerTask;


		public MaintainWallsTask() { }
		public MaintainWallsTask(PlayerInfo owner) : base(owner) { }

		public override bool IsTaskComplete()
		{
			// No completion state specified
			return false;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildEarthworkerTask());
			m_BuildEarthworkerTask = new BuildEarthworkerTask(owner);
		}

		protected override bool PerformTask()
		{
			if (!m_BuildEarthworkerTask.IsTaskComplete())
				m_BuildEarthworkerTask.PerformTaskTree();

			// Fail Check: Not enough ore for walls
			if (owner.player.Ore() < 50)
				return false;

			int structuresThatNeedWalls = 0;

			// Find structure that needs walls
			foreach (UnitEx building in new PlayerAllBuildingEnum(owner.player.playerID))
			{
				bool needsWall = false;

				switch (building.GetUnitType())
				{
					case map_id.GuardPost:
					case map_id.AdvancedLab:
						needsWall = true;
						break;
				}

				if (!needsWall)
					continue;

				// Get tile for wall
				LOCATION tile;
				if (!GetEmptyTileAroundRect(building.GetRect(true), out tile))
					continue;

				// Get earthworker
				UnitEx earthworker = GetClosestEarthworker(tile);

				if (earthworker == null || earthworker.GetCurAction() != ActionType.moDone)
				{
					++structuresThatNeedWalls;
					continue;
				}

				BuildStructureTask.ClearDeployArea(earthworker, map_id.LightTower, tile, owner.player);

				earthworker.DoBuildWall(map_id.Wall, new MAP_RECT(tile, new LOCATION(1, 1)));
				break;
			}

			// If we are overwhelmed, build more earthworkers to meet demand
			if (structuresThatNeedWalls/4 + 1 > owner.units.earthWorkers.Count)
				m_BuildEarthworkerTask.targetCountToBuild = structuresThatNeedWalls/4 + 1;

			return true;
		}

		private UnitEx GetClosestEarthworker(LOCATION position)
		{
			UnitEx closestUnit = null;
			int closestDistance = 999999;

			foreach (UnitEx unit in owner.units.earthWorkers)
			{
				// Closest distance
				int distance = position.GetDiagonalDistance(unit.GetPosition());
				if (distance < closestDistance)
				{
					closestUnit = unit;
					closestDistance = distance;
				}
			}

			return closestUnit;
		}

		private bool GetEmptyTileAroundRect(MAP_RECT area, out LOCATION tile)
		{
			for (int tx=area.xMin-1; tx <= area.xMax; ++tx)
			{
				tile = new LOCATION(tx, area.yMin-1);
				if (IsEmptyTile(tx, area.yMin-1)) return true;
			}

			for (int ty=area.yMin-1; ty <= area.yMax; ++ty)
			{
				tile = new LOCATION(area.xMin-1, ty);
				if (IsEmptyTile(area.xMin-1, ty)) return true;
			}

			for (int tx=area.xMin-1; tx <= area.xMax; ++tx)
			{
				tile = new LOCATION(tx, area.yMax);
				if (IsEmptyTile(tx, area.yMax)) return true;
			}

			for (int ty=area.yMin-1; ty <= area.yMax; ++ty)
			{
				tile = new LOCATION(area.xMax, ty);
				if (IsEmptyTile(area.xMax, ty)) return true;
			}

			tile = new LOCATION(0, 0);

			return false;
		}

		private bool IsEmptyTile(int tx, int ty)
		{
			if (!GameMap.IsTilePassable(tx, ty))
				return false;
			if (GameMap.GetCellType(tx, ty) == CellType.Tube0)
				return false;
			if (BuildStructureTask.IsAreaBlocked(new MAP_RECT(tx, ty, 1, 1), owner.player.playerID, true))
				return false;

			return true;
		}
	}
}
