using DotNetMissionSDK.Json;
using DotNetMissionSDK.Pathfinding;
using DotNetMissionSDK.HFL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetMissionSDK
{
	/// <summary>
	/// Generates a base for a player based on a center point and list of unit definitions.
	/// </summary>
	public class BaseGenerator
	{
		/// <summary>
		/// Stores the link between a vehicle and the structure area that needs to spawn it.
		/// </summary>
		private class VehicleSpawnArea
		{
			public UnitData vehicleData		{ get; private set; }
			public MAP_RECT spawnArea		{ get; private set; }

			/// <summary>
			/// Creates a spawn area for a vehicle.
			/// </summary>
			/// <param name="vehicleData">The vehicle definition to spawn.</param>
			/// <param name="spawnArea">The area to spawn the vehicle around.</param>
			public VehicleSpawnArea(UnitData vehicleData, MAP_RECT spawnArea)
			{
				this.vehicleData = vehicleData;
				this.spawnArea = spawnArea;
			}
		}

		/// <summary>
		/// Store information about a created unit.
		/// </summary>
		private class CreatedUnitData
		{
			public UnitData unitData	{ get; private set; }
			public int tubesCreated		{ get; private set; }

			public void AddTube()		{ ++tubesCreated;	}

			public CreatedUnitData(UnitData data)
			{
				unitData = data;
			}
		}


		private bool m_FirstStructure;								// Is this the first structure being generated for this base?

		private List<Unit> m_CreatedUnits;							// The list of units that already exist on the map
		private Dictionary<int, CreatedUnitData> m_CreatedUnitData;	// Unit definitions that have already been created. Key is the unit stubIndex.

		private List<Unit> m_GeneratedUnits = new List<Unit>();		// The list of units generated in the most recent call to Generate().

		/// <summary>
		/// The list of units generated in the most recent call to Generate().
		/// </summary>
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

		/// <summary>
		/// Generates a base for a player based on a center point and list of unit definitions.
		/// </summary>
		/// <param name="owner">The player that owns the base.</param>
		/// <param name="baseCenterPt">The center of the base.</param>
		/// <param name="unitData">The list of unit definitions to create the base with.</param>
		public void Generate(Player owner, LOCATION baseCenterPt, UnitData[] unitData)
		{
			m_FirstStructure = true;
			baseCenterPt = TethysGame.GetMapCoordinates(baseCenterPt);
			MAP_RECT spawnArea = new MAP_RECT(baseCenterPt, new LOCATION(1,1));
			m_CreatedUnitData = new Dictionary<int, CreatedUnitData>();
			m_GeneratedUnits.Clear();

			List<VehicleSpawnArea> vehicleSpawns = new List<VehicleSpawnArea>();
			List<MAP_RECT> wallSpawns = new List<MAP_RECT>(); // List of structure rects that need walls

			// Generate all structures in order
			foreach (UnitData data in unitData)
			{
				if (data.ignoreLayout)
				{
					// Create unit that ignores layout
					LOCATION loc = TethysGame.GetMapCoordinates(data.location);
					Unit unit = data.CreateUnit(owner.playerID, loc);
					CreatedUnitData createdUnitData = new CreatedUnitData(data);

					// If unit overlaps another unit, it will be moved to fit by OP2. Make sure location is up-to-date.
					loc = new LOCATION(unit.GetTileX(), unit.GetTileY());

					m_CreatedUnits.Add(unit);
					m_CreatedUnitData.Add(unit.GetStubIndex(), createdUnitData);
					m_GeneratedUnits.Add(unit);

					// Store spawn area for units that spawn after this structure
					if (IsStructure(data.typeID))
					{
						spawnArea = new UnitInfo(data.typeID).GetRect(loc);

						GenerateTubes(owner, unit, loc, createdUnitData);

						// If structure should have a wall, add it to the spawn list to generate later
						if (data.createWall)
							wallSpawns.Add(MAP_RECT.FromMinMax(spawnArea.xMin-1, spawnArea.yMin-1, spawnArea.xMax+1, spawnArea.yMax+1));
					}
				}
				else
				{
					if (IsStructure(data.typeID))
					{
						// Create structure with auto-layout
						spawnArea = GenerateUnit(owner, baseCenterPt, data);

						// If structure should have a wall, add it to the spawn list to generate later
						if (data.createWall)
							wallSpawns.Add(MAP_RECT.FromMinMax(spawnArea.xMin-1, spawnArea.yMin-1, spawnArea.xMax+1, spawnArea.yMax+1));
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
				GenerateUnit(owner, spawn.spawnArea, spawn.vehicleData);
			}
		}

		private MAP_RECT GenerateUnit(Player owner, LOCATION spawnPt, UnitData data)
		{
			return GenerateUnit(owner, new MAP_RECT(spawnPt, new LOCATION(1,1)), data);
		}

		/// <summary>
		/// Finds an open position and creates a unit on it.
		/// </summary>
		/// <param name="owner">The player that owns this unit.</param>
		/// <param name="spawnArea">The area to begin searching for an open position. The search area will circle out from it.</param>
		/// <param name="data">The definition of the unit to spawn.</param>
		/// <returns>The area the unit occupies on the map.</returns>
		private MAP_RECT GenerateUnit(Player owner, MAP_RECT spawnArea, UnitData data)
		{
			// Callback for determining if tile is a valid place point
			Pathfinder.ValidTileCallback validTileCB = (int x, int y) =>
			{
				return IsValidTile(x,y, spawnArea, data);
			};

			// Get the closest valid tile for this unit
			LOCATION foundPt;
			if (!Pathfinder.GetClosestValidTile(GetTilesInRect(spawnArea), GetTileCost, validTileCB, out foundPt))
			{
				Console.WriteLine("Failed to place unit: " + data.typeID);
				return spawnArea;
			}

			// Create unit
			Unit unit = data.CreateUnit(owner.playerID, foundPt);
			CreatedUnitData createdUnitData = new CreatedUnitData(data);

			// Add to generation lists
			m_CreatedUnits.Add(unit);
			m_CreatedUnitData.Add(unit.GetStubIndex(), createdUnitData);
			m_GeneratedUnits.Add(unit);

			// Generate tubes
			GenerateTubes(owner, unit, foundPt, createdUnitData);

			// Return spawn rect for found point
			return new UnitInfo(data.typeID).GetRect(foundPt);
		}

		private LOCATION[] GetTilesInRect(MAP_RECT rect)
		{
			LOCATION[] tiles = new LOCATION[rect.width * rect.height];

			int i=0;
			for (int x=rect.xMin; x < rect.xMax; ++x)
			{
				for (int y=rect.yMin; y < rect.yMax; ++y, ++i)
					tiles[i] = new LOCATION(x,y);
			}

			return tiles;
		}

		// Callback for determining tile cost
		private int GetTileCost(int x, int y)
		{
			if (!GameMap.IsTilePassable(x,y))
				return Pathfinder.Impassable;

			return 1;
		}

		/// <summary>
		/// Callback for determining a valid tile for unit placement.
		/// </summary>
		/// <param name="x">The x position of the tile.</param>
		/// <param name="y">The y position of the tile.</param>
		/// <param name="spawnArea">The center area the unit should spawn from.</param>
		/// <param name="unitToSpawn">The definition of the unit to spawn.</param>
		/// <returns>True, if the tile is valid.</returns>
		private bool IsValidTile(int x, int y, MAP_RECT spawnArea, UnitData unitToSpawn)
		{
			// Vehicles must be at least spawnDistance away from spawnArea
			if (IsVehicle(unitToSpawn.typeID))
			{
				spawnArea.Inflate(unitToSpawn.spawnDistance, unitToSpawn.spawnDistance);

				// Tile is not valid if extended spawn area contains the tile
				if (spawnArea.Contains(x,y))
					return false;
			}

			// Get spawn rect
			MAP_RECT targetSpawnRect = new UnitInfo(unitToSpawn.typeID).GetRect(new LOCATION(x,y), true);

			// Check if colliding
			if (IsColliding(targetSpawnRect, unitToSpawn.minDistance, IsStructure(unitToSpawn.typeID), IsVehicle(unitToSpawn.typeID)))
				return false;
				
			return true;
		}

		/// <summary>
		/// Checks if an area is colliding with any tiles or units.
		/// </summary>
		/// <param name="spawnRect">The area to check for collisions.</param>
		/// <param name="minDistance">The additional distance around this area to check for collisions. Only applies if useStructureMinDistance or useVehicleMinDistance is true. Does not include tiles.</param>
		/// <param name="useStructureMinDistance">Should structures use their minimum distance for the collision check?</param>
		/// <param name="useVehicleMinDistance">Should vehicles use their minimum distance for the collision check?</param>
		/// <returns>True, if the area collides with tiles or units.</returns>
		private bool IsColliding(MAP_RECT spawnRect, int minDistance, bool useStructureMinDistance, bool useVehicleMinDistance)
		{
			// Check if rect is out of bounds
			if (!spawnRect.IsInMapBounds())
				return true;

			// Check if colliding with ground
			for (int x=spawnRect.xMin; x < spawnRect.xMax; ++x)
			{
				for (int y=spawnRect.yMin; y < spawnRect.yMax; ++y)
				{
					LOCATION position = new LOCATION(x,y);
					position.ClipToMap();

					// Check for impassible tile types
					if (!GameMap.IsTilePassable(x,y))
						return true;
				}
			}

			// Check if colliding with units
			foreach (Unit unit in m_CreatedUnits)
			{
				// Unit data
				map_id unitType = unit.GetUnitType();
				LOCATION loc = new LOCATION(unit.GetTileX(), unit.GetTileY());
				MAP_RECT area = new UnitInfo(unitType).GetRect(loc);

				if (useStructureMinDistance && IsStructure(unitType) ||
					useVehicleMinDistance && IsVehicle(unitType))
				{
					// Get unit creation data. If found, get min distance.
					CreatedUnitData data;
					int targetMinDistance = 0;
					if (m_CreatedUnitData.TryGetValue(unit.GetStubIndex(), out data))
						targetMinDistance = data.unitData.minDistance;

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

		/// <summary>
		/// Generates tubes from a source unit to a player's other structures.
		/// </summary>
		/// <param name="owner">The player to connect tubes to.</param>
		/// <param name="sourceUnit">The source unit to connect tubes from.</param>
		/// <param name="sourcePosition">The source unit's position to connect tubes from.</param>
		/// <param name="createdUnitData">The source unit's created unit data.</param>
		private void GenerateTubes(Player owner, Unit sourceUnit, LOCATION sourcePosition, CreatedUnitData createdUnitData)
		{
			int maxDistance = createdUnitData.unitData.minDistance;
			int maxTubes = createdUnitData.unitData.maxTubes;

			// First structure does not generate any tubes.
			// We check this here to prevent multiple bases from attempting to connect to each other.
			if (m_FirstStructure)
			{
				m_FirstStructure = false;
				return;
			}

			map_id sourceUnitType = sourceUnit.GetUnitType();

			// If tube limit reached, or doesn't need tubes, cancel
			if (maxTubes == createdUnitData.tubesCreated || (maxTubes < 0 && !NeedsTube(sourceUnitType)))
				return;

			// Get unit area
			MAP_RECT unitArea = new UnitInfo(sourceUnitType).GetRect(sourceUnit.GetPosition(), true);
			
			// Find structures for tubes
			List<Unit> connections = new List<Unit>();
			Unit closestUnit = null;
			int closestDistance = 9999999;

			MAP_RECT region = new MAP_RECT(unitArea);
			region.Inflate(maxDistance, maxDistance);

			foreach (Unit target in m_GeneratedUnits)
			{
				// Don't connect to self
				if (sourceUnit == target)
					continue;

				// Only connect to player's own structures
				if (target.GetOwnerID() != owner.playerID)
					continue;

				// Target must be a structure that uses tubes
				map_id targetType = target.GetUnitType();
				if (!NeedsTube(targetType))
					continue;

				// Check if target can receive tubes
				CreatedUnitData targetCreatedData;
				if (!m_CreatedUnitData.TryGetValue(target.GetStubIndex(), out targetCreatedData))
				{
					Console.WriteLine("StubIndex not found in m_CreatedUnitData!");
					continue;
				}

				// If tube limit reached, or doesn't need tubes, cancel
				if (targetCreatedData.unitData.maxTubes == targetCreatedData.tubesCreated || (targetCreatedData.unitData.maxTubes < 0 && !NeedsTube(targetType)))
					continue;

				// Get target location and area
				LOCATION targetPosition = new LOCATION(target.GetTileX(), target.GetTileY());
				MAP_RECT targetRect = new UnitInfo(targetType).GetRect(targetPosition, true);
				
				// Find closest unit
				int distance = Math.Abs(targetPosition.x - sourcePosition.x) + Math.Abs(targetPosition.y - sourcePosition.y);
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
				connections.Insert(0, closestUnit);

			// Remove excess tubing
			if (maxTubes >= 0)
			{
				int removeCount = connections.Count - maxTubes;
				if (removeCount > 0)
					connections.RemoveRange(connections.Count - removeCount, removeCount);
			}

			// Create tubes
			foreach (Unit target in connections)
			{
				// Create tube
				LOCATION targetPosition = new LOCATION(target.GetTileX(), target.GetTileY());

				// GetPath will avoid obstacles, but pass through structures so that tubes don't wrap around them.
				LOCATION[] tubePath = Pathfinder.GetPath(sourcePosition, targetPosition, false, GetTileCost);
				if (tubePath == null)
				{
					Console.WriteLine("Failed to find path for tube: " + sourcePosition + " to " + targetPosition);
					return;
				}

				// Get target created unit data to add tube connection
				CreatedUnitData targetCreatedData;
				if (!m_CreatedUnitData.TryGetValue(target.GetStubIndex(), out targetCreatedData))
				{
					Console.WriteLine("StubIndex not found in m_CreatedUnitData!");
					continue;
				}

				// Track tubes created on these units
				createdUnitData.AddTube();
				targetCreatedData.AddTube();
				
				foreach (LOCATION tile in tubePath)
				{
					// We need to check if the tile has a structure on it, and skip creating the tube if it does.
					// Pathfinding passes through structures, but we don't want to have an underlayer of tubes.
					if (IsColliding(new MAP_RECT(tile, new LOCATION(1,1)), 0, false, false))
						continue;

					TethysGame.CreateWallOrTube(tile.x, tile.y, 0, map_id.Tube);
				}
			}
		}

		/// <summary>
		/// Generates walls around an area.
		/// Does not place walls in the area.
		/// </summary>
		/// <param name="area">The area to generate walls around.</param>
		private void GenerateWalls(MAP_RECT area)
		{
			// Generate walls around area
			for (int x=area.xMin-1; x <= area.xMax; ++x)
			{
				CreateWall(x, area.yMin-1);
				CreateWall(x, area.yMax);
			}

			for (int y=area.yMin-1; y <= area.yMax; ++y)
			{
				CreateWall(area.xMin-1, y);
				CreateWall(area.xMax, y);
			}
		}

		/// <summary>
		/// Creates a wall at the specified position if it meets requirements. Accounts for map clipping.
		/// </summary>
		/// <param name="x">The x position of the wall.</param>
		/// <param name="y">The y position of the wall.</param>
		private void CreateWall(int x, int y)
		{
			LOCATION position = new LOCATION(x,y);
			position.ClipToMap();

			if (CanBuildWall(position.x, position.y))
				TethysGame.CreateWallOrTube(position.x, position.y, 0, map_id.Wall);
		}

		/// <summary>
		/// Check if a tile is passable and isn't a dozed area or a tube.
		/// </summary>
		/// <param name="x">The x position of the tile for the wall.</param>
		/// <param name="y">The y position of the tile for the wall.</param>
		/// <returns>True, if the wall can be built.</returns>
		private bool CanBuildWall(int x, int y)
		{
			switch (GameMap.GetCellType(x,y))
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

		private bool IsVehicle(map_id typeID)
		{
			return (int)typeID >= 1 && (int)typeID <= 15;
		}

		private bool IsStructure(map_id typeID)
		{
			return (int)typeID >= 21 && (int)typeID <= 58;
		}

		private bool NeedsTube(map_id typeID)
		{
			switch (typeID)
			{
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
	}
}
