﻿using DotNetMissionSDK.HFL;
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

		public int targetCountToBuild = 1;

		public BuildStructureTask() { }
		public BuildStructureTask(PlayerInfo owner) : base(owner) { }

		public override bool IsTaskComplete()
		{
			List<UnitEx> units = owner.units.GetListForType(m_KitToBuild);
			return units.Count >= targetCountToBuild;
		}


		public override void GeneratePrerequisites()
		{
		}

		protected override bool PerformTask()
		{
			// Get idle convec with kit
			UnitEx convec = owner.units.convecs.Find((UnitEx unit) => unit.GetCargo() == m_KitToBuild && (unit.GetCurAction() == ActionType.moDone || unit.GetCurAction() == ActionType.moObjDocking));
			if (convec == null)
				return false;

			// Wait for docking to complete
			if (convec.GetCurAction() == ActionType.moObjDocking)
				return true;

			// If we can build earthworkers or have one, we can deploy disconnected structures
			m_CanBuildDisconnected = owner.units.earthWorkers.Count > 0 || owner.units.vehicleFactories.Count > 0 || !NeedsTube(m_KitToBuild);

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

			// Find open location near CC
			LOCATION foundPt;
			if (!Pathfinder.GetClosestValidTile(closestCC.GetPosition(), GetTileCost, IsValidTile, out foundPt))
				return false;

			ClearDeployArea(convec, convec.GetCargo(), foundPt);

			// Build structure
			convec.DoBuild(m_KitToBuild, foundPt.x, foundPt.y);

			return true;
		}

		public static void ClearDeployArea(UnitEx deployUnit, map_id buildingType, LOCATION deployPt)
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
		private bool IsValidTile(int x, int y)
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

		public static bool IsAreaBlocked(MAP_RECT targetArea, int ownerID)
		{
			// Check if area is blocked by structure or enemy
			for (int i=0; i < TethysGame.NoPlayers(); ++i)
			{
				foreach (UnitEx unit in new PlayerAllBuildingEnum(i))
				{
					if (unit.IsBuilding())
					{
						MAP_RECT unitArea = unit.GetUnitInfo().GetRect(unit.GetPosition());
						if (targetArea.DoesRectIntersect(unitArea))
							return true;
					}

					if (unit.GetOwnerID() != ownerID)
						return true;
				}
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