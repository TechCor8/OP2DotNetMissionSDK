using DotNetMissionSDK.AI.Managers;
using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Pathfinding;
using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Mining
{
	/// <summary>
	/// Finds common mines and deploys smelters until all of them are fully saturated.
	/// </summary>
	public class SaturateCommonMineTask : Task
	{
		private MiningBaseState m_MiningBaseState;


		public SaturateCommonMineTask(MiningBaseState miningBaseState)									{ m_MiningBaseState = miningBaseState; }
		public SaturateCommonMineTask(PlayerInfo owner, MiningBaseState miningBaseState) : base(owner)	{ m_MiningBaseState = miningBaseState; }


		public override bool IsTaskComplete()
		{
			// Task is complete when there are no unsaturated common mining sites left
			foreach (MiningBase miningBase in m_MiningBaseState.miningBases)
			{
				foreach (MiningSite site in miningBase.miningSites)
				{
					if (site.beacon.GetOreType() != BeaconType.Common) continue;
					if (site.mine == null) continue;

					if (site.smelters.Count < MiningBaseState.SmelterSaturationCount)
						return false;
				}
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildCommonSmelterKitTask());
		}

		protected override bool PerformTask()
		{
			// Get idle convec with smelter
			UnitEx convec = owner.units.convecs.Find((UnitEx unit) => unit.GetCargo() == map_id.CommonOreSmelter && unit.GetCurAction() == ActionType.moDone);
			if (convec == null)
				return true;

			// Find closest mining site that is not saturated
			MiningBase closestMiningBase = null;
			MiningSite closestMineSite = null;
			int closestDistance = 900000;

			foreach (MiningBase miningBase in m_MiningBaseState.miningBases)
			{
				foreach (MiningSite site in miningBase.miningSites)
				{
					if (site.beacon.GetOreType() != BeaconType.Common) continue;
					if (site.mine == null) continue;
					if (site.smelters.Count >= MiningBaseState.SmelterSaturationCount) continue;

					int distance = convec.GetPosition().GetDiagonalDistance(site.mine.GetPosition());
					if (distance < closestDistance)
					{
						closestMiningBase = miningBase;
						closestMineSite = site;
						closestDistance = distance;
					}
				}
			}

			if (closestMineSite == null)
				return false;

			if (!DeploySmelter(convec, closestMiningBase.commandCenter.GetPosition(), closestMineSite.mine.GetPosition()))
				return false;

			return true;
		}

		private bool DeploySmelter(UnitEx convec, LOCATION commandCenterPosition, LOCATION minePosition)
		{
			// Callback for determining tile cost
			Pathfinder.TileCostCallback getTileCostCB = (int x, int y) =>
			{
				if (!GameMap.IsTilePassable(x,y))
					return Pathfinder.Impassable;

				// Prevent search from exceeding command center control area
				if (commandCenterPosition.GetDiagonalDistance(new LOCATION(x,y)) > MiningBaseState.MaxSmelterDistanceToCC)
					return Pathfinder.Impassable;

				return 1;
			};

			// Callback for determining if tile is a valid place point
			Pathfinder.ValidTileCallback validTileCB = (int x, int y) =>
			{
				// Get area to deploy structure
				UnitInfo info = new UnitInfo(convec.GetCargo());
				LOCATION size = info.GetSize(true);
				MAP_RECT targetArea = new MAP_RECT(x-size.x+1, y-size.y+1, size.x,size.y);

				if (!BuildStructureTask.AreTilesPassable(targetArea, x, y))
					return false;

				// Check if area is blocked by structure or enemy
				if (BuildStructureTask.IsAreaBlocked(targetArea, owner.player.playerID))
					return false;

				// TODO: If can't build tubes, must be touching connected structure to be valid (Apply this to BuildStructureTask, too)
				//if (!owner.CanBuildTubes())
				//	return owner.commandGrid.ConnectsTo(targetArea);

				return true;
			};

			// Find open location near mine
			LOCATION foundPt;
			if (!Pathfinder.GetClosestValidTile(minePosition, getTileCostCB, validTileCB, out foundPt))
				return false;

			// Clear units out of deploy area
			BuildStructureTask.ClearDeployArea(convec, convec.GetCargo(), foundPt);

			// Build structure
			convec.DoBuild(convec.GetCargo(), foundPt.x, foundPt.y);

			return true;
		}
	}
}
