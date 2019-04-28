using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Pathfinding;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	/// <summary>
	/// This abstract class finds a suitable location and deploys a structure.
	/// </summary>
	public abstract class BuildStructureTask : Task
	{
		protected map_id m_KitToBuild = map_id.Agridome;
		protected int m_DesiredDistance = 0;                // Desired minimum distance to nearest structure

		private bool m_CanBuildDisconnected;

		private bool m_OverrideLocation = false;
		private LOCATION m_TargetLocation;

		public int targetCountToBuild = 1;

		public BuildStructureTask() { }
		public BuildStructureTask(PlayerInfo owner) : base(owner) { }

		public override bool IsTaskComplete()
		{
			List<UnitEx> units = owner.units.GetListForType(m_KitToBuild);
			return units.Count >= targetCountToBuild;
		}

		public void SetLocation(LOCATION targetPosition)
		{
			m_OverrideLocation = true;
			m_TargetLocation = targetPosition;
		}

		public override void GeneratePrerequisites()
		{
		}

		protected override bool CanPerformTask()
		{
			// Get convec with kit
			return owner.units.convecs.Find((UnitEx unit) => unit.GetCargo() == m_KitToBuild) != null;
		}

		protected override bool PerformTask()
		{
			// Get idle convec with kit
			UnitEx convec = owner.units.convecs.Find((UnitEx unit) =>
			{
				return unit.GetCargo() == m_KitToBuild;
			});

			if (convec == null)
				return false;

			// Wait for docking or building to complete
			if (convec.GetCurAction() != ActionType.moDone)
				return true;

			// If we can build earthworkers or have one, we can deploy disconnected structures
			m_CanBuildDisconnected = owner.units.earthWorkers.Count > 0 || owner.units.vehicleFactories.Count > 0 || !NeedsTube(m_KitToBuild);

			if (!m_OverrideLocation)
			{
				// Find closest CC
				UnitEx closestCC = null;
				int closestDistance = 900000;
				foreach (UnitEx cc in owner.units.commandCenters)
				{
					int distance = convec.GetPosition().GetDiagonalDistance(cc.GetPosition());
					if (distance < closestDistance)
					{
						closestCC = cc;
						closestDistance = distance;
					}
				}
				m_TargetLocation = closestCC.GetPosition();
			}

			// Find open location near CC
			LOCATION foundPt;
			if (!Pathfinder.GetClosestValidTile(m_TargetLocation, GetTileCost, IsValidTile, out foundPt))
				return false;

			ClearDeployArea(convec, convec.GetCargo(), foundPt, owner.player);

			// Build structure
			convec.DoBuild(m_KitToBuild, foundPt.x, foundPt.y);

			return true;
		}

		public static void ClearDeployArea(UnitEx deployUnit, map_id buildingType, LOCATION deployPt, Player owner)
		{
			// Get area to deploy structure
			UnitInfo info = new UnitInfo(buildingType);
			LOCATION size = info.GetSize(true);
			MAP_RECT targetArea = new MAP_RECT(deployPt.x-size.x+1, deployPt.y-size.y+1, size.x,size.y);

			// Order all units except this convec to clear the area
			foreach (UnitEx unit in new InRectEnumerator(targetArea))
			{
				if (!unit.IsVehicle())
					continue;

				if (unit.GetStubIndex() == deployUnit.GetStubIndex())
					continue;

				LOCATION position = unit.GetPosition();

				// Move units away from center
				LOCATION dir = position - deployPt;
				if (dir.x == 0 && dir.y == 0)
					dir.x = 1;

				if (dir.x > 1) dir.x = 1;
				if (dir.y > 1) dir.y = 1;
				if (dir.x < -1) dir.x = -1;
				if (dir.y < -1) dir.y = -1;

				position += dir;

				LOCATION normal = dir.normal;

				if (!GameMap.IsTilePassable(position) || IsAreaBlocked(new MAP_RECT(position, new LOCATION(1,1)), owner.playerID))
					position = unit.GetPosition() + normal;
				if (!GameMap.IsTilePassable(position) || IsAreaBlocked(new MAP_RECT(position, new LOCATION(1,1)), owner.playerID))
					position = unit.GetPosition() - normal;
				if (!GameMap.IsTilePassable(position) || IsAreaBlocked(new MAP_RECT(position, new LOCATION(1,1)), owner.playerID))
					continue;

				unit.DoMove(position.x, position.y);
			}
		}

		// Callback for determining tile cost
		public static int GetTileCost(int x, int y)
		{
			if (!GameMap.IsTilePassable(x,y))
				return Pathfinder.Impassable;

			return 1;
		}

		// Callback for determining if tile is a valid place point
		protected bool IsValidTile(int x, int y)
		{
			// Get area to deploy structure
			UnitInfo info = new UnitInfo(m_KitToBuild);
			LOCATION size = info.GetSize(true);
			MAP_RECT targetArea = new MAP_RECT(x-size.x+1, y-size.y+1, size.x,size.y);

			if (!AreTilesPassable(targetArea, x, y))
				return false;

			// Apply minimum distance if we can build this disconnected
			if (m_CanBuildDisconnected)
				targetArea.Inflate(m_DesiredDistance, m_DesiredDistance);
			else
			{
				// Force structure to build on connected ground
				MAP_RECT unbulldozedArea = targetArea;
				unbulldozedArea.Inflate(-1, -1);
				owner.commandGrid.ConnectsTo(unbulldozedArea);
			}

			// Check if area is blocked by structure or enemy
			if (IsAreaBlocked(targetArea, owner.player.playerID))
				return false;

			return true;
		}

		public static bool AreTilesPassable(MAP_RECT targetArea, int x, int y)
		{
			// Check if target tiles are impassable
			for (int tx=targetArea.xMin; tx < targetArea.xMax; ++tx)
			{
				for (int ty=targetArea.yMin; ty < targetArea.yMax; ++ty)
				{
					if (!GameMap.IsTilePassable(tx, ty))
						return false;
				}
			}

			return true;
		}

		public static bool IsAreaBlocked(MAP_RECT targetArea, int ownerID, bool includeBulldozedArea=false)
		{
			// Check if area is blocked by structure or enemy
			for (int i=0; i < TethysGame.NoPlayers(); ++i)
			{
				foreach (UnitEx unit in new PlayerAllBuildingEnum(i))
				{
					if (unit.IsBuilding())
					{
						MAP_RECT unitArea = unit.GetUnitInfo().GetRect(unit.GetPosition(), includeBulldozedArea);
						if (targetArea.DoesRectIntersect(unitArea))
							return true;
					}
					else if (unit.IsVehicle())
					{
						if (unit.GetOwnerID() != ownerID && targetArea.Contains(unit.GetPosition()))
							return true;
					}
				}
			}

			// Don't allow structure to be built on ground where a mine can be deployed
			foreach (UnitEx beacon in new PlayerUnitEnum(6))
			{
				if (beacon.GetUnitType() != map_id.MiningBeacon)
					continue;

				if (targetArea.DoesRectIntersect(new MAP_RECT(beacon.GetTileX()-2, beacon.GetTileY()-1, 5,3)))
					return true;
			}

			return false;
		}

		public static bool NeedsTube(map_id typeID)
		{
			switch (typeID)
			{
				case map_id.CommandCenter:
				case map_id.LightTower:
				case map_id.CommonOreMine:
				case map_id.RareOreMine:
				case map_id.Tokamak:
				case map_id.SolarPowerArray:
				case map_id.MHDGenerator:
					return false;
			}

			return IsStructure(typeID);
		}

		private static bool IsStructure(map_id typeID)
		{
			return (int)typeID >= 21 && (int)typeID <= 58;
		}
	}
}
