using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Pathfinding;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;
using System;
using System.Collections.Generic;
using System.Linq;

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
		//private bool m_IsSearchingForDeployLocation;

		private bool m_OverrideLocation = false;
		private LOCATION m_TargetLocation;

		public int targetCountToBuild = 1;

		public BuildStructureTask(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			IReadOnlyCollection<UnitState> units = owner.units.GetListForType(m_KitToBuild);
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

		protected override bool CanPerformTask(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Get convec with kit
			return owner.units.convecs.FirstOrDefault((unit) => unit.cargoType == m_KitToBuild) != null;
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Get idle convec with kit
			ConvecState convec = owner.units.convecs.FirstOrDefault((unit) =>
			{
				return unit.cargoType == m_KitToBuild;
			});

			if (convec == null)
				return false;

			// Wait for docking or building to complete
			if (convec.curAction != ActionType.moDone)
				return true;

			// If we can build earthworkers or have one, we can deploy disconnected structures
			m_CanBuildDisconnected = owner.units.earthWorkers.Count > 0 || owner.units.vehicleFactories.Count > 0 || !NeedsTube(m_KitToBuild);

			if (!m_OverrideLocation)
			{
				// Find closest CC
				UnitState closestCC = owner.units.GetClosestUnitOfType(map_id.CommandCenter, convec.position);
				if (closestCC != null)
					m_TargetLocation = closestCC.position;
			}

			// Wait for search to complete
			//if (m_IsSearchingForDeployLocation)
			//	return true;

			// Find open location near CC
			LOCATION foundPt;
			if (!Pathfinder.GetClosestValidTile(m_TargetLocation, (x,y) => GetTileCost(stateSnapshot, x,y), (x,y) => IsValidTile(stateSnapshot, x,y), out foundPt))
				return false;

			// TODO: Run GetClosestValidTile asynchronously? ^^^

			ClearDeployArea(convec, convec.cargoType, foundPt, stateSnapshot, ownerID, unitActions);

			// Build structure
			unitActions.AddUnitCommand(convec.unitID, 2, () => GameState.GetUnit(convec.unitID)?.DoBuild(m_KitToBuild, foundPt.x, foundPt.y));

			return true;
		}

		public static void ClearDeployArea(UnitState deployUnit, map_id buildingType, LOCATION deployPt, StateSnapshot stateSnapshot, int ownerID, BotCommands unitActions)
		{
			// Get area to deploy structure
			GlobalStructureInfo info = stateSnapshot.structureInfo[buildingType];

			LOCATION size = info.GetSize(true);
			MAP_RECT targetArea = new MAP_RECT(deployPt.x-size.x+1, deployPt.y-size.y+1, size.x,size.y);

			// Order all units except this convec to clear the area
			foreach (UnitState unit in stateSnapshot.unitMap.GetUnitsInArea(targetArea))
			{
				if (!unit.isVehicle)
					continue;

				if (unit.unitID == deployUnit.unitID)
					continue;

				LOCATION position = unit.position;

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

				if (!stateSnapshot.tileMap.IsTilePassable(position) || IsAreaBlocked(stateSnapshot, new MAP_RECT(position, new LOCATION(1,1)), ownerID))
					position = unit.position + normal;
				if (!stateSnapshot.tileMap.IsTilePassable(position) || IsAreaBlocked(stateSnapshot, new MAP_RECT(position, new LOCATION(1,1)), ownerID))
					position = unit.position - normal;
				if (!stateSnapshot.tileMap.IsTilePassable(position) || IsAreaBlocked(stateSnapshot, new MAP_RECT(position, new LOCATION(1,1)), ownerID))
					continue;

				unitActions.AddUnitCommand(unit.unitID, 1, () => GameState.GetUnit(unit.unitID)?.DoMove(position.x, position.y));
			}
		}

		// Callback for determining tile cost
		public static int GetTileCost(StateSnapshot stateSnapshot, int x, int y)
		{
			if (!stateSnapshot.tileMap.IsTilePassable(x,y))
				return Pathfinder.Impassable;

			return 1;
		}

		// Callback for determining if tile is a valid place point
		protected bool IsValidTile(StateSnapshot stateSnapshot, int x, int y)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			GlobalStructureInfo info = stateSnapshot.structureInfo[m_KitToBuild];

			// Get area to deploy structure
			LOCATION size = info.GetSize(true);
			MAP_RECT targetArea = new MAP_RECT(x-size.x+1, y-size.y+1, size.x,size.y);

			if (!AreTilesPassable(stateSnapshot, targetArea, x, y))
				return false;

			// Apply minimum distance if we can build this disconnected
			if (m_CanBuildDisconnected)
				targetArea.Inflate(m_DesiredDistance, m_DesiredDistance);
			else
			{
				// Force structure to build on connected ground
				MAP_RECT unbulldozedArea = targetArea;
				unbulldozedArea.Inflate(-1, -1);
				if (!stateSnapshot.commandMap.ConnectsTo(ownerID, unbulldozedArea))
					return false;
			}

			// Check if area is blocked by structure or enemy
			if (IsAreaBlocked(stateSnapshot, targetArea, owner.playerID))
				return false;

			return true;
		}

		public static bool AreTilesPassable(StateSnapshot stateSnapshot, MAP_RECT targetArea, int x, int y)
		{
			// Check if target tiles are impassable
			for (int tx=targetArea.xMin; tx < targetArea.xMax; ++tx)
			{
				for (int ty=targetArea.yMin; ty < targetArea.yMax; ++ty)
				{
					if (!stateSnapshot.tileMap.IsTilePassable(tx, ty))
						return false;
				}
			}

			return true;
		}

		public static bool IsAreaBlocked(StateSnapshot stateSnapshot, MAP_RECT targetArea, int ownerID, bool includeBulldozedArea=false)
		{
			// Check if area is blocked by structure or enemy
			foreach (UnitState unit in stateSnapshot.unitMap.GetUnitsInArea(targetArea))
			{
				if (unit.isBuilding)
				{
					MAP_RECT unitArea = ((StructureState)unit).GetRect(includeBulldozedArea);
					if (targetArea.DoesRectIntersect(unitArea))
						return true;
				}
				else if (unit.isVehicle)
				{
					if (unit.ownerID != ownerID && targetArea.Contains(unit.position))
						return true;
				}
			}

			// Don't allow structure to be built on ground where a mine can be deployed
			foreach (GaiaUnitState beacon in stateSnapshot.gaia)
			{
				map_id beaconType = beacon.unitType;

				if (beaconType != map_id.MiningBeacon &&
					beaconType != map_id.Fumarole &&
					beaconType != map_id.MagmaVent)
					continue;

				if (targetArea.DoesRectIntersect(new MAP_RECT(beacon.position.x-2, beacon.position.y-1, 5,3)))
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
				case map_id.GeothermalPlant:
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
