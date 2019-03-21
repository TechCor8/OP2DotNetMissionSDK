using DotNetMissionSDK.Json;
using DotNetMissionSDK.Pathfinding;
using DotNetMissionSDK.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetMissionSDK
{
	public class BaseGenerator
	{
		// Stores the link between a vehicle and the structure area that needs to spawn it
		private struct VehicleSpawnArea
		{
			public UnitData vehicleData;
			public MAP_RECT spawnArea;

			public VehicleSpawnArea(UnitData vehicleData, MAP_RECT spawnArea)
			{
				this.vehicleData = vehicleData;
				this.spawnArea = spawnArea;
			}
		}


		private bool m_FirstStructure;		// Is this the first structure being generated for this base?

		private List<Unit> m_CreatedUnits;
		private Dictionary<int, UnitData> m_CreatedUnitData;

		private List<Unit> m_GeneratedUnits = new List<Unit>();

		public ReadOnlyCollection<Unit> generatedUnits { get { return new ReadOnlyCollection<Unit>(m_GeneratedUnits); } }


		/// <summary>
		/// Prepares the base generator for use.
		/// </summary>
		/// <param name="existingUnits">A list of units that have already been created. For collision processing.</param>
		public BaseGenerator(IEnumerable<Unit> existingUnits)
		{
			m_CreatedUnits = new List<Unit>(existingUnits);
		}

		/// <summary>
		/// Creates a default base generator with no units currently on the map.
		/// </summary>
		public BaseGenerator()
		{
			m_CreatedUnits = new List<Unit>();
		}

		public void Generate(Player owner, LOCATION baseCenterPt, UnitData[] unitData)
		{
			m_FirstStructure = true;
			baseCenterPt = TethysGame.GetMapCoordinates(baseCenterPt);
			MAP_RECT spawnArea = new MAP_RECT(baseCenterPt, baseCenterPt);
			m_CreatedUnitData = new Dictionary<int, UnitData>();

			List<VehicleSpawnArea> vehicleSpawns = new List<VehicleSpawnArea>();
			List<MAP_RECT> wallSpawns = new List<MAP_RECT>(); // List of structure rects that need walls

			// Generate all structures in order
			foreach (UnitData data in unitData)
			{
				if (data.ignoreLayout)
				{
					// Create unit that ignores layout
					LOCATION loc = TethysGame.GetMapCoordinates(new LOCATION(data.location));
					Unit unit = TethysGame.CreateUnit(data.typeID, loc.x, loc.y, owner.playerID, data.cargoType, data.direction);
					m_CreatedUnits.Add(unit);
					m_GeneratedUnits.Add(unit);

					// Store spawn area for units that spawn after this structure
					if (IsStructure(data.typeID))
						spawnArea = UnitInfo.GetRect(loc, data.typeID);
				}
				else
				{
					if (IsStructure(data.typeID))
					{
						// Create structure with auto-layout
						spawnArea = GenerateUnit(owner, baseCenterPt, data);

						// If structure should have a wall, add it to the spawn list to generate later
						if (data.createWall)
							wallSpawns.Add(spawnArea);
					}
					else if (IsVehicle(data.typeID))
					{
						// Save vehicle for later
						vehicleSpawns.Add(new VehicleSpawnArea(data, spawnArea));
					}
				}
			}

			// Generate walls
			foreach (MAP_RECT rect in wallSpawns)
				GenerateWalls(rect);

			// Generate vehicles that spawn from structure
			foreach (VehicleSpawnArea spawn in vehicleSpawns)
			{
				// Create vehicle with auto-layout
				GenerateUnit(owner, spawn.spawnArea, spawn.vehicleData, spawn.vehicleData.spawnDistance);
			}
		}

		private MAP_RECT GenerateUnit(Player owner, LOCATION spawnPt, UnitData data, int startDistance = 0)
		{
			return GenerateUnit(owner, new MAP_RECT(spawnPt, spawnPt), data, startDistance);
		}

		private MAP_RECT GenerateUnit(Player owner, MAP_RECT spawnArea, UnitData data, int startDistance=0)
		{
			// Determine if tile is passable
			Pathfinder.TileCostCallback tileCostCB = (int x, int y) =>
			{
				// Get spawn rect
				MAP_RECT targetSpawnRect = UnitInfo.GetRect(new LOCATION(x,y), data.typeID);
				if (IsStructure(data.typeID))
					targetSpawnRect.Inflate(1, 1); // Include bulldozed area
				
				// Check if clipped by map
				MAP_RECT mapClip = new MAP_RECT(targetSpawnRect);
				mapClip.ClipToMap();
				if (targetSpawnRect.minX != mapClip.minX || targetSpawnRect.minY != mapClip.minY ||
					targetSpawnRect.maxX != mapClip.maxX || targetSpawnRect.maxY != mapClip.maxY)
				{
					return Pathfinder.Impassable;
				}

				// Check if terrain is passable
				if (!IsTilePassable(x,y))
					return Pathfinder.Impassable;

				return 1;
			};

			// Determine if tile is a valid place point
			Pathfinder.ValidTileCallback validTileCB = (int x, int y) =>
			{
				// Get spawn rect
				MAP_RECT targetSpawnRect = UnitInfo.GetRect(new LOCATION(x,y), data.typeID);
				if (IsStructure(data.typeID))
					targetSpawnRect.Inflate(1, 1); // Include bulldozed area

				// Check if colliding
				if (IsColliding(targetSpawnRect, data.minDistance, IsStructure(data.typeID), IsVehicle(data.typeID)))
					return false;
				
				return true;
			};

			LOCATION foundPt = Pathfinder.GetValidTile(GetTilesInRect(spawnArea), tileCostCB, validTileCB);
			if (foundPt == null)
			{
				Console.WriteLine("Failed to place unit: " + data.typeID);
				return spawnArea;
			}

			// Get spawn rect
			MAP_RECT spawnRect = UnitInfo.GetRect(foundPt, data.typeID);
			if (IsStructure(data.typeID))
				spawnRect.Inflate(1, 1); // Include bulldozed area

			// Place unit
			Unit unit = TethysGame.CreateUnit(data.typeID, foundPt.x, foundPt.y, owner.playerID, data.cargoType, data.direction);

			// Add to generation lists
			m_CreatedUnits.Add(unit);
			m_CreatedUnitData.Add(unit.GetStubIndex(), data);
			m_GeneratedUnits.Add(unit);

			if (IsStructure(data.typeID))
			{
				// Structures that aren't power plants need tubes
				if (data.typeID != map_id.Tokamak && data.typeID != map_id.SolarPowerArray && data.typeID != map_id.MHDGenerator)
					GenerateTubes(owner, foundPt, unit, data.minDistance);
			}

			return spawnRect;
		}

		private LOCATION[] GetTilesInRect(MAP_RECT rect)
		{
			LOCATION[] tiles = new LOCATION[rect.width * rect.height];

			int i=0;
			for (int x=rect.minX; x <= rect.maxX; ++x)
			{
				for (int y=rect.minY; y <= rect.maxY; ++y, ++i)
					tiles[i] = new LOCATION(x,y);
			}

			return tiles;
		}

		private bool IsColliding(MAP_RECT spawnRect, int minDistance, bool useStructureMinDistance, bool useVehicleMinDistance)
		{
			// Check if colliding with ground
			for (int x=spawnRect.minX; x <= spawnRect.maxX; ++x)
			{
				for (int y=spawnRect.minY; y <= spawnRect.maxY; ++y)
				{
					bool didCollide = true;

					switch ((CellType)GameMap.GetCellType(x,y))
					{
						case CellType.FastPassible1:
						case CellType.SlowPassible1:
						case CellType.SlowPassible2:
						case CellType.MediumPassible1:
						case CellType.MediumPassible2:
						case CellType.FastPassible2:
						case CellType.DozedArea:
						case CellType.Rubble:
						case CellType.Tube0:
							didCollide = false;
							break;
					}

					if (didCollide)
						return true;
				}
			}

			// Check if colliding with units
			foreach (Unit unit in m_CreatedUnits)
			{
				// Unit position/area
				LOCATION loc = new LOCATION(unit.GetTileX(), unit.GetTileY());
				MAP_RECT area = new MAP_RECT(loc, loc);
				map_id unitType = unit.GetUnitType();

				// If unit is structure, inflate by structure size
				if (IsStructure(unitType))
					area = UnitInfo.GetRect(loc, unitType);

				if (useStructureMinDistance && IsStructure(unitType) ||
					useVehicleMinDistance && IsVehicle(unitType))
				{
					// Get unit creation data. If found, get min distance.
					UnitData data;
					int targetMinDistance = 0;
					if (m_CreatedUnitData.TryGetValue(unit.GetStubIndex(), out data))
						targetMinDistance = data.minDistance;

					// Inflate by min distance
					if (minDistance < targetMinDistance)
						area.Inflate(targetMinDistance, targetMinDistance);
					else
						area.Inflate(minDistance, minDistance);
				}

				if (spawnRect.DoesRectIntersect(area))
					return true;
			}

			return false;
		}

		private void GenerateTubes(Player owner, LOCATION unitPosition, Unit sourceUnit, int maxDistance)
		{
			// First structure does not generate any tubes.
			// We check this here to prevent multiple bases from attempting to connect to each other.
			if (m_FirstStructure)
			{
				m_FirstStructure = false;
				return;
			}

			map_id sourceUnitType = sourceUnit.GetUnitType();

			// Get unit area
			MAP_RECT unitArea = UnitInfo.GetRect(new LOCATION(sourceUnit.GetTileX(), sourceUnit.GetTileY()), sourceUnitType);
			if (IsStructure(sourceUnitType))
				unitArea.Inflate(1, 1); // Include bulldozed area

			// Find structures for tubes
			List<Unit> connections = new List<Unit>();
			Unit closestUnit = null;
			int closestDistance = 9999999;

			MAP_RECT region = new MAP_RECT(unitArea.minX, unitArea.minY, unitArea.maxX, unitArea.maxY);
			region.Inflate(maxDistance, maxDistance);

			foreach (Unit target in m_CreatedUnits)
			{
				// Don't connect to self
				if (sourceUnit == target)
					continue;

				// Only connect to player's own structures
				if (target.GetOwnerID() != owner.playerID)
					continue;

				// Target must be a structure that uses tubes
				map_id targetType = target.GetUnitType();
				if (!IsStructure(targetType) || targetType == map_id.Tokamak || targetType == map_id.SolarPowerArray || targetType == map_id.MHDGenerator)
					continue;

				// Get target location and area
				LOCATION targetPosition = new LOCATION(target.GetTileX(), target.GetTileY());
				MAP_RECT targetRect = UnitInfo.GetRect(targetPosition, targetType);
				targetRect.Inflate(1, 1); // Include bulldozed area

				// Find closest unit
				int distance = Math.Abs(targetPosition.x - unitPosition.x) + Math.Abs(targetPosition.y - unitPosition.y);
				if (distance < closestDistance)
				{
					closestUnit = target;
					closestDistance = distance;
				}

				// Check for units within max distance
				if (region.DoesRectIntersect(targetRect))
					connections.Add(target);
			}

			// Connect to closest unit
			if (closestUnit != null && !connections.Contains(closestUnit))
				connections.Add(closestUnit);

			// Create tubes
			foreach (Unit target in connections)
			{
				LOCATION targetPosition = new LOCATION(target.GetTileX(), target.GetTileY());

				// GetPath will avoid obstacles, but ignore structures so that tubes don't wrap around them.
				LOCATION[] tubePath = Pathfinder.GetPath(unitPosition, targetPosition, false, (int x, int y) => { return CanBuildTube(x, y) ? 1 : 0; });

				foreach (LOCATION tile in tubePath)
				{
					// We need to check if the tile has a structure on it, and skip creating the tube if it does.
					// Pathfinding passes through structures, but we don't want to have an underlayer of tubes.
					if (IsColliding(new MAP_RECT(tile, tile), 0, false, false))
						continue;

					TethysGame.CreateWallOrTube(tile.x, tile.y, 0, map_id.Tube);
				}
			}
		}

		private void GenerateWalls(MAP_RECT area)
		{
			// Generate walls around area
			for (int x=area.minX-1; x <= area.maxX+1; ++x)
			{
				if (CanBuildWall(x, area.minY-1))
					TethysGame.CreateWallOrTube(x, area.minY-1, 0, map_id.Wall);
				if (CanBuildWall(x, area.maxY+1))
					TethysGame.CreateWallOrTube(x, area.maxY+1, 0, map_id.Wall);
			}

			for (int y=area.minY-1; y <= area.maxY+1; ++y)
			{
				if (CanBuildWall(area.minX-1, y))
					TethysGame.CreateWallOrTube(area.minX-1, y, 0, map_id.Wall);
				if (CanBuildWall(area.maxX+1, y))
					TethysGame.CreateWallOrTube(area.maxX+1, y, 0, map_id.Wall);
			}
		}

		private bool IsTilePassable(int x, int y)
		{
			switch ((CellType)GameMap.GetCellType(x,y))
			{
				case CellType.FastPassible1:
				case CellType.SlowPassible1:
				case CellType.SlowPassible2:
				case CellType.MediumPassible1:
				case CellType.MediumPassible2:
				case CellType.FastPassible2:
				case CellType.DozedArea:
				case CellType.Rubble:
				case CellType.Tube0:
					return true;
			}

			return false;
		}

		private bool CanBuildWall(int x, int y)
		{
			switch ((CellType)GameMap.GetCellType(x,y))
			{
				case CellType.FastPassible1:
				case CellType.SlowPassible1:
				case CellType.SlowPassible2:
				case CellType.MediumPassible1:
				case CellType.MediumPassible2:
				case CellType.FastPassible2:
				//case CellType.DozedArea:
				case CellType.Rubble:
					return true;
			}

			return false;
		}

		private bool CanBuildTube(int x, int y)
		{
			switch ((CellType)GameMap.GetCellType(x,y))
			{
				case CellType.FastPassible1:
				case CellType.SlowPassible1:
				case CellType.SlowPassible2:
				case CellType.MediumPassible1:
				case CellType.MediumPassible2:
				case CellType.FastPassible2:
				case CellType.DozedArea:
				case CellType.Rubble:
				case CellType.Tube0:
				//case CellType.NormalWall:
				//case CellType.LavaWall:
				//case CellType.MicrobeWall:
					return true;
			}

			return false;
		}

		private bool IsVehicle(map_id typeID)
		{
			return (int)typeID >= 1 && (int)typeID <= 15;
		}

		private bool IsStructure(map_id typeID)
		{
			return (int)typeID >= 21 && (int)typeID <= 58;
		}
	}

	/*
	Tube generation that comes out of right and left sides should use default tubes
	
	map wrap around support

	*/
}
