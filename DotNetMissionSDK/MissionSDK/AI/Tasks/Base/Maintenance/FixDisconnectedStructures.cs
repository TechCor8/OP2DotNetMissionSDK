using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class FixDisconnectedStructures : Task
	{
		private BuildEarthworkerTask m_BuildEarthworkerTask;


		public FixDisconnectedStructures() { }
		public FixDisconnectedStructures(PlayerInfo owner) : base(owner) { }

		public override bool IsTaskComplete()
		{
			// Task is complete if all structures are connected
			foreach (UnitEx building in new PlayerAllBuildingEnum(owner.player.playerID))
			{
				if (!BuildStructureTask.NeedsTube(building.GetUnitType()))
					continue;

				if (!owner.commandGrid.ConnectsTo(building.GetRect()))
					return false;
			}

			return true;
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

			// Fail Check: Not enough ore for tubes
			if (owner.player.Ore() < 50)
				return false;

			int structuresThatNeedTubes = 0;

			// Find disconnected structures
			foreach (UnitEx unitToFix in new PlayerAllBuildingEnum(owner.player.playerID))
			{
				if (!BuildStructureTask.NeedsTube(unitToFix.GetUnitType()))
					continue;

				if (owner.commandGrid.ConnectsTo(unitToFix.GetRect()))
					continue;

				++structuresThatNeedTubes;

				// Get earthworker
				UnitEx earthworker = GetClosestEarthworker(unitToFix.GetPosition());

				if (earthworker == null || earthworker.GetCurAction() != ActionType.moDone)
					continue;

				// Get path from tube network to structure
				LOCATION[] path = owner.commandGrid.GetPathToClosestConnectedTile(unitToFix.GetRect());
				if (path == null)
					continue;

				// Fix disconnected structure
				for (int i = 0; i < path.Length; ++i)
				{
					if (GameMap.GetCellType(path[i].x, path[i].y) == CellType.Tube0)
						continue;

					BuildStructureTask.ClearDeployArea(earthworker, map_id.LightTower, path[i], owner.player);

					earthworker.DoBuildWall(map_id.Tube, new MAP_RECT(path[i], new LOCATION(1, 1)));
					break;
				}
			}

			// If we are overwhelmed, build more earthworkers to meet demand
			if (structuresThatNeedTubes/4 + 1 > owner.units.earthWorkers.Count)
				m_BuildEarthworkerTask.targetCountToBuild = structuresThatNeedTubes/4 + 1;

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
	}
}
