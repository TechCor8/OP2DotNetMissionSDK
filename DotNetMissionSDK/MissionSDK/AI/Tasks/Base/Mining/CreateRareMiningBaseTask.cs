using DotNetMissionSDK.AI.Managers;
using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Pathfinding;
using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Mining
{
	/// <summary>
	/// Finds beacons outside of a command center's control area and builds a new command center.
	/// All bases must be fully saturated before a new base is created.
	/// </summary>
	public class CreateRareMiningBaseTask : Task
	{
		private MiningBaseState m_MiningBaseState;

		private CreateRareMineTask m_CreateMineTask;


		public CreateRareMiningBaseTask(MiningBaseState miningBaseState)									{ m_MiningBaseState = miningBaseState; }
		public CreateRareMiningBaseTask(PlayerInfo owner, MiningBaseState miningBaseState) : base(owner)	{ m_MiningBaseState = miningBaseState; }


		public override bool IsTaskComplete()
		{
			// Task is not complete until every CC beacon has been occupied and saturated
			if (!m_CreateMineTask.IsTaskComplete())
				return false;
			
			// Task is complete if there are no beacons outside of a command center's control area
			foreach (UnitEx beacon in new PlayerUnitEnum(6))
			{
				if (beacon.GetUnitType() != map_id.MiningBeacon)
					continue;
				
				if (beacon.GetOreType() != BeaconType.Rare)
					continue;
				
				// Detect if occupied
				bool isOccupied = false;
				for (int i=0; i < TethysGame.NoPlayers(); ++i)
				{
					UnitEx building = GetClosestBuildingOfType(i, map_id.Any, beacon.GetPosition());
					if (building != null && building.GetPosition().GetDiagonalDistance(beacon.GetPosition()) < MiningBaseState.MaxMineDistanceToCC)
					{
						isOccupied = true;
						break;
					}
				}
				
				if (isOccupied)
					continue;

				return false;
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(m_CreateMineTask = new CreateRareMineTask(m_MiningBaseState));
			AddPrerequisite(new BuildCommandCenterKitTask());
		}

		protected override bool PerformTask()
		{
			// Get idle convec with CC
			UnitEx convec = owner.units.convecs.Find((UnitEx unit) => unit.GetCargo() == map_id.CommandCenter && unit.GetCurAction() == ActionType.moDone);
			
			if (convec == null)
				return true;
			
			// Find unoccupied beacon
			UnitEx unoccupiedBeacon = GetClosestUnusedBeacon(convec.GetPosition());
			if (unoccupiedBeacon != null)
			{
				LOCATION beaconPosition = unoccupiedBeacon.GetPosition();
				return DeployCC(convec, beaconPosition);
			}
			
			return false;
		}

		private bool DeployCC(UnitEx convec, LOCATION targetPosition)
		{
			// Callback for determining if tile is a valid place point
			Pathfinder.ValidTileCallback validTileCB = (int x, int y) =>
			{
				// Ignore tiles within X distance of beacon
				if (targetPosition.GetDiagonalDistance(new LOCATION(x,y)) < 6)
					return false;

				// Get area to deploy structure
				UnitInfo info = new UnitInfo(convec.GetCargo());
				LOCATION size = info.GetSize(true);
				MAP_RECT targetArea = new MAP_RECT(x-size.x+1, y-size.y+1, size.x,size.y);

				if (!BuildStructureTask.AreTilesPassable(targetArea, x, y))
					return false;

				// Check if area is blocked by structure or enemy
				if (BuildStructureTask.IsAreaBlocked(targetArea, owner.player.playerID))
					return false;

				return true;
			};
			
			// Find open location near beacon
			LOCATION foundPt;
			if (!Pathfinder.GetClosestValidTile(targetPosition, BuildStructureTask.GetTileCost, validTileCB, out foundPt))
				return false;
			
			// Clear units out of deploy area
			BuildStructureTask.ClearDeployArea(convec, convec.GetCargo(), foundPt, owner.player);
			
			// Build structure
			convec.DoBuild(convec.GetCargo(), foundPt.x, foundPt.y);
			
			return true;
		}

		private UnitEx GetClosestUnusedBeacon(LOCATION position)
		{
			// Get closest beacon
			UnitEx closestBeacon = null;
			int closestDistance = 900000;

			foreach (UnitEx beacon in new PlayerUnitEnum(6))
			{
				if (beacon.GetUnitType() != map_id.MiningBeacon)
					continue;

				if (beacon.GetOreType() != BeaconType.Rare)
					continue;

				// Detect if occupied
				bool isOccupied = false;
				for (int i=0; i < TethysGame.NoPlayers(); ++i)
				{
					UnitEx building = GetClosestBuildingOfType(i, map_id.Any, beacon.GetPosition());
					if (building != null && building.GetPosition().GetDiagonalDistance(beacon.GetPosition()) < MiningBaseState.MaxMineDistanceToCC)
					{
						isOccupied = true;
						break;
					}
				}

				if (isOccupied)
					continue;
				
				// Closest distance
				int distance = position.GetDiagonalDistance(beacon.GetPosition());
				if (distance < closestDistance)
				{
					closestBeacon = beacon;
					closestDistance = distance;
				}
			}

			return closestBeacon;
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
