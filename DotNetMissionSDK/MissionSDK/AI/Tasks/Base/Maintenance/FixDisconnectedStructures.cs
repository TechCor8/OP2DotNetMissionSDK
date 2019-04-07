using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.AI.Tasks.Base.Vehicle;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class FixDisconnectedStructures : Task
	{
		public FixDisconnectedStructures() { }
		public FixDisconnectedStructures(PlayerInfo owner) : base(owner) { }

		public override bool IsTaskComplete()
		{
			// Task is complete if all structures are connected
			foreach (UnitEx building in new PlayerAllBuildingEnum(owner.player.playerID))
			{
				if (!owner.commandGrid.ConnectsTo(building.GetRect()))
					return false;
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildEarthworkerTask());
		}

		protected override bool PerformTask()
		{
			UnitEx unitToFix = null;

			// Find disconnected structure
			foreach (UnitEx building in new PlayerAllBuildingEnum(owner.player.playerID))
			{
				if (!BuildStructureTask.NeedsTube(building.GetUnitType()))
					continue;

				if (!owner.commandGrid.ConnectsTo(building.GetRect()))
				{
					unitToFix = building;
					break;
				}
			}

			// Fail Check: Not enough ore for tubes
			if (owner.player.Ore() < 50)
				return false;

			if (unitToFix == null)
				return false;

			// Get earthworker
			UnitEx earthworker = GetClosestEarthworker(unitToFix.GetPosition());

			if (earthworker.GetCurAction() != ActionType.moDone)
				return true;

			// Get path from tube network to structure
			LOCATION[] path = owner.commandGrid.GetPathToClosestConnectedTile(unitToFix.GetRect());
			if (path == null)
				return false;

			// Fix disconnected structure
			for (int i = 0; i < path.Length; ++i)
			{
				if (GameMap.GetCellType(path[i].x, path[i].y) == CellType.Tube0)
					continue;

				BuildStructureTask.ClearDeployArea(earthworker, map_id.LightTower, path[i]);

				earthworker.DoBuildWall(map_id.Tube, new MAP_RECT(path[i], new LOCATION(1, 1)));
				break;
			}

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
