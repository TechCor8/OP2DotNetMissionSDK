using DotNetMissionSDK.AI.Managers;
using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Pathfinding;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.Mining
{
	/// <summary>
	/// Finds rare mines and deploys smelters until all of them are fully saturated.
	/// </summary>
	public class SaturateRareMineTask : Task
	{
		private MiningBaseState m_MiningBaseState;


		public SaturateRareMineTask(int ownerID, MiningBaseState miningBaseState) : base(ownerID)	{ m_MiningBaseState = miningBaseState; }


		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			// Task is complete when there are no unsaturated rare mining sites left
			foreach (MiningBase miningBase in m_MiningBaseState.miningBases)
			{
				foreach (MiningSite site in miningBase.miningSites)
				{
					if (site.beacon.oreType != BeaconType.Rare) continue;
					if (site.mine == null) continue;
					
					if (site.smelters.Count < MiningBaseState.SmelterSaturationCount)
						return false;
				}
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildRareSmelterKitTask(ownerID));
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, List<Action> unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Get idle convec with smelter
			ConvecState convec = owner.units.convecs.FirstOrDefault((unit) => unit.cargoType == map_id.RareOreSmelter && unit.curAction == ActionType.moDone);
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
					if (site.beacon.oreType != BeaconType.Rare) continue;
					if (site.mine == null) continue;
					if (site.smelters.Count >= MiningBaseState.SmelterSaturationCount) continue;

					int distance = convec.position.GetDiagonalDistance(site.mine.position);
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
			
			if (!DeploySmelter(stateSnapshot, unitActions, convec, closestMiningBase.commandCenter.position, closestMineSite.mine.position))
				return false;

			return true;
		}

		private bool DeploySmelter(StateSnapshot stateSnapshot, List<Action> unitActions, ConvecState convec, LOCATION commandCenterPosition, LOCATION minePosition)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Callback for determining tile cost
			Pathfinder.TileCostCallback getTileCostCB = (int x, int y) =>
			{
				if (!stateSnapshot.tileMap.IsTilePassable(x,y))
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
				GlobalStructureInfo info = stateSnapshot.structureInfo[convec.cargoType];
				LOCATION size = info.GetSize(true);
				MAP_RECT targetArea = new MAP_RECT(x-size.x+1, y-size.y+1, size.x,size.y);

				if (!BuildStructureTask.AreTilesPassable(stateSnapshot, targetArea, x, y))
					return false;

				// Check if area is blocked by structure or enemy
				if (BuildStructureTask.IsAreaBlocked(stateSnapshot, targetArea, ownerID))
					return false;

				// If can't build tubes, must be touching connected structure to be valid (Apply this to BuildStructureTask, too)
				if (!CanBuildTubes(owner))
				{
					MAP_RECT unbulldozedArea = targetArea;
					unbulldozedArea.Inflate(-1, -1);
					return stateSnapshot.commandMap.ConnectsTo(ownerID, unbulldozedArea);
				}

				return true;
			};

			// Find open location near mine
			LOCATION foundPt;
			if (!Pathfinder.GetClosestValidTile(minePosition, getTileCostCB, validTileCB, out foundPt))
				return false;

			// Clear units out of deploy area
			BuildStructureTask.ClearDeployArea(convec, convec.cargoType, foundPt, stateSnapshot, ownerID, unitActions);

			// Build structure
			unitActions.Add(() => GameState.GetUnit(convec.unitID)?.DoBuild(convec.cargoType, foundPt.x, foundPt.y));

			return true;
		}

		private bool CanBuildTubes(PlayerState owner)
		{
			// Must have enough ore to build tubes
			if (owner.ore < 500)
				return false;

			if (owner.units.earthWorkers.Count > 0)
				return true;

			// Can build earthworker?
			if (owner.units.vehicleFactories.FirstOrDefault((FactoryState unit) => unit.isEnabled) != null)
				return true;

			return false;
		}
	}
}
