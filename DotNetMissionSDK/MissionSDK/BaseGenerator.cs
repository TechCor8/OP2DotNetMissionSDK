using DotNetMissionSDK.Json;
using DotNetMissionSDK.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetMissionSDK
{
	public class BaseGenerator
	{
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
			baseCenterPt = TethysGame.GetMapCoordinates(baseCenterPt);
			LOCATION spawnPt = baseCenterPt;
			m_CreatedUnitData = new Dictionary<int, UnitData>();

			// Generate all units in order
			foreach (UnitData data in unitData)
			{
				if (data.ignoreLayout)
				{
					// Create unit that ignores layout
					LOCATION loc = TethysGame.GetMapCoordinates(new LOCATION(data.location));
					Unit unit = TethysGame.CreateUnit(data.typeID, loc.x, loc.y, owner.playerID, data.cargoType, data.direction);
					m_CreatedUnits.Add(unit);
					m_GeneratedUnits.Add(unit);
					continue;
				}

				// Create unit with auto-layout
				if (IsStructure(data.typeID))
					spawnPt = GenerateUnit(owner, baseCenterPt, data);
				else if (IsVehicle(data.typeID))
					GenerateUnit(owner, spawnPt, data, data.spawnDistance);
			}
		}

		private LOCATION GenerateUnit(Player owner, LOCATION spawnPt, UnitData data, int startDistance=0)
		{
			LOCATION preferredPt = new LOCATION(spawnPt.x, spawnPt.y);

			Console.WriteLine("Placing unit: " + data.typeID.ToString());
			Console.WriteLine("Preferred Pt = " + preferredPt.x + ", " + preferredPt.y);

			int step = 0;
			int turn = 0;

			// Move rect by the start distance
			if (startDistance > 0)
			{
				turn = startDistance * 4;
				preferredPt.y += startDistance;
			}

			// Attempt to place unit
			for (int i=0; i < 1000000; ++i)
			{
				// Calculate spawn rect
				MAP_RECT spawnRect = UnitInfo.GetRect(preferredPt, data.typeID);
				spawnRect.Inflate(1, 1); // Include bulldozed area

				Console.WriteLine("pt = " + preferredPt.x + ", " + preferredPt.y);
				Console.WriteLine("Rect = " + spawnRect.minX + ", " + spawnRect.minY + " Max = " + spawnRect.maxX + ", " + spawnRect.maxY);

				// Check if clipped by map
				MAP_RECT mapClip = new MAP_RECT(spawnRect.minX, spawnRect.minY, spawnRect.maxX, spawnRect.maxY);
				mapClip.ClipToMap();
				if (spawnRect.minX != mapClip.minX || spawnRect.minY != mapClip.minY ||
					spawnRect.maxX != mapClip.maxX || spawnRect.maxY != mapClip.maxY)
				{
					ShiftPoint(preferredPt, ref step, ref turn);
					continue;
				}
				
				// Check if colliding
				if (IsColliding(spawnRect, data.minDistance, IsStructure(data.typeID), IsVehicle(data.typeID)))
				{
					ShiftPoint(preferredPt, ref step, ref turn);
					continue;
				}

				// Check if reachable

				// Calculate unit direction
				UnitDirection direction = UnitDirection.East;

				// Place unit
				Unit unit = TethysGame.CreateUnit(data.typeID, preferredPt.x, preferredPt.y, owner.playerID, data.cargoType, direction);

				if (IsStructure(data.typeID))
				{
					// Structures that aren't power plants need tubes
					if (data.typeID != map_id.Tokamak && data.typeID != map_id.SolarPowerArray && data.typeID != map_id.MHDGenerator)
						GenerateTubes(owner, preferredPt, spawnRect, data.minDistance);

					// Generate walls
					if (data.createWall)
						GenerateWalls(spawnRect);
				}

				// Add to generation lists
				m_CreatedUnits.Add(unit);
				m_CreatedUnitData.Add(unit.GetStubIndex(), data);
				m_GeneratedUnits.Add(unit);

				return preferredPt;
			}

			Console.WriteLine("Failed to place unit: " + data.typeID);

			return spawnPt;
		}

		// Moves the rect in a widening circle with each iteration
		private void ShiftPoint(LOCATION pt, ref int step, ref int turn)
		{
			int direction = turn % 4;
			int requiredSteps = turn / 2 + 1;

			switch (direction)
			{
				case 0:		++pt.y;	break;	// Down
				case 1:		++pt.x;	break;	// Right
				case 2:		--pt.y;	break;	// Up
				case 3:		--pt.x;	break;	// Left
			}

			++step;

			if (step == requiredSteps)
			{
				step = 0;
				++turn;
			}
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

					Console.WriteLine("InflateRect = " + area.minX + ", " + area.minY + " Max = " + area.maxX + ", " + area.maxY);
				}

				if (spawnRect.DoesRectIntersect(area))
					return true;
			}

			return false;
		}

		private void GenerateTubes(Player owner, LOCATION unitPosition, MAP_RECT unitArea, int maxDistance)
		{
			// Find structures for tubes
			List<Unit> connections = new List<Unit>();
			Unit closestUnit = null;
			int closestDistance = 9999999;

			MAP_RECT region = new MAP_RECT(unitArea.minX, unitArea.minY, unitArea.maxX, unitArea.maxY);
			region.Inflate(maxDistance, maxDistance);

			foreach (Unit target in m_CreatedUnits)
			{
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
				//targetRect.Inflate(1, 1); // Include bulldozed area

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

				int xMin, yMin, xMax, yMax;

				if (targetPosition.x < unitPosition.x)
				{
					xMin = targetPosition.x;
					xMax = unitPosition.x;
				}
				else
				{
					xMax = targetPosition.x;
					xMin = unitPosition.x;
				}

				if (targetPosition.y < unitPosition.y)
				{
					yMin = targetPosition.y;
					yMax = unitPosition.y;
				}
				else
				{
					yMax = targetPosition.y;
					yMin = unitPosition.y;
				}
				
				for (int x=xMin; x <= xMax; ++x)
				{
					if (!CanBuildTube(x, unitPosition.y))
						continue;

					TethysGame.CreateWallOrTube(x, unitPosition.y, 0, map_id.Tube);
				}

				for (int y=yMin; y <= yMax; ++y)
				{
					if (!CanBuildTube(targetPosition.x, y))
						continue;

					TethysGame.CreateWallOrTube(targetPosition.x, y, 0, map_id.Tube);
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
				case CellType.NormalWall:
				case CellType.LavaWall:
				case CellType.MicrobeWall:
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
	Tube generation needs to ignore underside of buildings
	Tube generation that comes out of right and left sides should use default tubes
	Regeneration for tubes that are simple blocked
	Pathfinding for completely blocked tubes
	Pathfinding to detect unreachable building location
	Pathfinding to determine building distance


	For the purposes of the rules above, distance is calculated based on the "walking distance", so that units are not placed on the other side of impassable terrain.

	Tubes will be generated as right angles to any structures that are at "MinDistance" to each other. Longest uninterrupted axis gets placed first. If a tube is blocked during generation, the tube will use the last point as the start point and attempt to regenerate from there. This is to create as few corners as possible.

	If a direct tube route is not possible, perform a* pathfinding.
	*/
}
