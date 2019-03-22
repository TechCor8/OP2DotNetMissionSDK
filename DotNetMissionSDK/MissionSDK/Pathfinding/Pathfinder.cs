﻿using DotNetMissionSDK.Pathfinding.Internal;
using System;
using System.Collections.Generic;

namespace DotNetMissionSDK.Pathfinding
{
	/// <summary>
	/// This static class is used to generate paths between points and discover closest locations that meet a criteria.
	/// The class is bound to the map grid and will clamp or wrap around appropriately.
	/// <para>
	/// Use TileCostCallback to determine whether a tile is passable, impassible or expensive to cross.
	/// </para>
	/// <para>
	/// Use ValidTileCallback to determine if the current tile the pathfinder is on meets your criteria and that it should stop the search.
	/// </para>
	/// </summary>
	public class Pathfinder
	{
		/// <summary>
		/// Constant that represents an impassable tile.
		/// Pass this as the tile cost to block traversal to that tile.
		/// </summary>
		public const int Impassable = 0;

		public delegate int TileCostCallback(int x, int y);
		public delegate bool ValidTileCallback(int x, int y);

		/// <summary>
		/// Gets an optimal path from a start point to an end point.
		/// </summary>
		/// <param name="startPt">The starting point for the path.</param>
		/// <param name="endPt">The target point to reach.</param>
		/// <param name="allowDiagonal">Can the path move diagonally?</param>
		/// <param name="tileCostCB">A callback for getting the cost of a tile.</param>
		/// <returns>The path from startPt to endPt, or null, if a path could not be found.</returns>
		public static LOCATION[] GetPath(LOCATION startPt, LOCATION endPt, bool allowDiagonal, TileCostCallback tileCostCB)
		{
			if (!endPt.IsInMapBounds())
				return null;

			LOCATION goal = new LOCATION(endPt);
			goal.ClipToMap();

			List<PathNode> openSet = new List<PathNode>();
			Dictionary<int, PathNode> closedSet = new Dictionary<int, PathNode>();

			int adjacentCount = 4;
			if (allowDiagonal)
				adjacentCount = 8;

			// Add start point
			openSet.Add(new PathNode(startPt));

			while (openSet.Count > 0)
			{
				// Get lowest cost node in open list
				PathNode node = openSet[openSet.Count-1];
				openSet.RemoveAt(openSet.Count-1);

				// If goal reached, return path from current node
				if (node.x == goal.x && node.y == goal.y)
				{
					return node.ToArray();
				}
				else
				{
					closedSet.Add(node.GetHashCode(), node);

					for (int i=0; i < adjacentCount; ++i)
					{
						PathNode adjacentNode = null;

						switch (i)
						{
							case 0:		adjacentNode = new PathNode(node.x, node.y+1);		break;	// Down
							case 1:		adjacentNode = new PathNode(node.x+1, node.y);		break;	// Right
							case 2:		adjacentNode = new PathNode(node.x, node.y-1);		break;	// Up
							case 3:		adjacentNode = new PathNode(node.x-1, node.y);		break;	// Left
							case 4:		adjacentNode = new PathNode(node.x+1, node.y+1);	break;	// Down-Right
							case 5:		adjacentNode = new PathNode(node.x+1, node.y-1);	break;	// Up-Right
							case 6:		adjacentNode = new PathNode(node.x-1, node.y-1);	break;	// Up-Left
							case 7:		adjacentNode = new PathNode(node.x-1, node.y+1);	break;	// Down-Left
						}

						// If adjacent is closed, skip it
						PathNode val;
						if (closedSet.TryGetValue(adjacentNode.GetHashCode(), out val) && val.x == adjacentNode.x && val.y == adjacentNode.y)
							continue;

						// If adjacent is on open list, skip it
						if (openSet.Contains(adjacentNode))
							continue;

						int tileCost = tileCostCB(adjacentNode.x, adjacentNode.y);
						
						// If adjacent is obstacle, skip it
						if (tileCost == Impassable)
							continue;

						// Calculate cost
						if (allowDiagonal)
							adjacentNode.cost = Heuristic_Diagonal(node, goal, tileCost);
						else
							adjacentNode.cost = Heuristic_Manhattan(node, goal, tileCost);

						// Add to open list
						adjacentNode.parent = node;

						int index = openSet.FindLastIndex(element => element.cost > adjacentNode.cost);
						if (index <= 0)
							openSet.Insert(0, adjacentNode);
						else
							openSet.Insert(index + 1, adjacentNode);
					}
				}
			}

			// Unreachable / Path not found
			return null;
		}

		/// <summary>
		/// Returns the heuristic cost of the current node using the Manhattan algorithm. Accounts for map wrapping.
		/// <para>
		/// The Manhattan algorithm applies to 4 directional grid movement (North, East, South, West).
		/// </para>
		/// </summary>
		/// <param name="current">The node to calculate cost for.</param>
		/// <param name="goal">The goal to reach.</param>
		/// <param name="tileCost">The cost scalar.</param>
		/// <returns>The heuristic cost of the current node.</returns>
		private static int Heuristic_Manhattan(PathNode current, LOCATION goal, int tileCost)
		{
			int dx = Math.Abs(current.x - goal.x);
			int dy = Math.Abs(current.y - goal.y);

			if (GameMap.doesWrap)
			{
				// Check if going around the wrap side is shorter
				int wrapDistX = GetWrapDistance(current.x, goal.x, GameMap.area.minX, GameMap.area.maxX);
				dx = Math.Min(wrapDistX, dx);
			}

			return tileCost * (dx + dy);
		}

		/// <summary>
		/// Returns the heuristic cost of the current node using the Diagonal algorithm. Accounts for map wrapping.
		/// <para>
		/// The Diagonal algorithm applies to 8 directional grid movement (North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest).
		/// </para>
		/// </summary>
		/// <param name="current">The node to calculate cost for.</param>
		/// <param name="goal">The goal to reach.</param>
		/// <param name="tileCost">The cost scalar.</param>
		/// <returns>The heuristic cost of the current node.</returns>
		private static int Heuristic_Diagonal(PathNode current, LOCATION goal, int tileCost)
		{
			int dx = Math.Abs(current.x - goal.x);
			int dy = Math.Abs(current.y - goal.y);
			int cost2 = tileCost;

			if (GameMap.doesWrap)
			{
				// Check if going around the wrap side is shorter
				int wrapDistX = GetWrapDistance(current.x, goal.x, GameMap.area.minX, GameMap.area.maxX);
				dx = Math.Min(wrapDistX, dx);
			}

			return tileCost * (dx + dy) + (cost2 - 2 * tileCost) * Math.Min(dx, dy);
		}

		// For x-axis, pass all x values. For y-axis, pass all y values.
		/// <summary>
		/// Gets the wrap distance between current and goal.
		/// <para>The wrap distance is considered to be the distance going off the left or right side of the map to reach the goal.</para>
		/// <para>Does not check if map wrap is active.</para>
		/// </summary>
		/// <param name="currentXY">The x or y axis value of the current node.</param>
		/// <param name="goalXY">The x or y axis value of the goal.</param>
		/// <param name="mapMinXY">The x or y axis minimum value of the game map.</param>
		/// <param name="mapMaxXY">The x or y axis maximum value of the game map.</param>
		/// <returns>The wrap distance between current and goal.</returns>
		private static int GetWrapDistance(int currentXY, int goalXY, int mapMinXY, int mapMaxXY)
		{
			int dWrapCur = Math.Min(currentXY - mapMinXY, mapMaxXY - currentXY);
			int dWrapGoal = Math.Min(goalXY - mapMinXY, mapMaxXY - goalXY);
			return dWrapCur + dWrapGoal;
		}

		/// <summary>
		/// Performs a breadth-first search. When validTileCB returns true, the search ends and the tile location is returned.
		/// </summary>
		/// <param name="startPt">The starting point for the search.</param>
		/// <param name="tileCostCB">A callback for getting the cost of a tile.</param>
		/// <param name="validTileCB">A callback for determining if the tile completes the search.</param>
		/// <returns>The first valid tile found.</returns>
		public static LOCATION GetClosestValidTile(LOCATION startPt, TileCostCallback tileCostCB, ValidTileCallback validTileCB)
		{
			return GetClosestValidTile(new LOCATION[] { startPt }, tileCostCB, validTileCB);
		}

		/// <summary>
		/// Performs a breadth-first search. When validTileCB returns true, the search ends and the tile location is returned.
		/// </summary>
		/// <param name="startPt">The starting point for the search.</param>
		/// <param name="tileCostCB">A callback for getting the cost of a tile.</param>
		/// <param name="validTileCB">A callback for determining if the tile completes the search.</param>
		/// <returns>The first valid tile found.</returns>
		public static LOCATION GetClosestValidTile(IEnumerable<LOCATION> startPts, TileCostCallback tileCostCB, ValidTileCallback validTileCB)
		{
			List<PathNode> openSet = new List<PathNode>();
			Dictionary<int, PathNode> closedSet = new Dictionary<int, PathNode>();

			int adjacentCount = 8;

			// Add start points
			foreach (LOCATION startPt in startPts)
			{
				PathNode node = new PathNode(startPt);
				if (!openSet.Contains(node))
					openSet.Add(node);
			}

			while (openSet.Count > 0)
			{
				// Get lowest cost node in open list
				PathNode node = openSet[openSet.Count-1];
				openSet.RemoveAt(openSet.Count-1);

				// If adjacent tile is goal, return it
				if (validTileCB(node.x, node.y))
					return new LOCATION(node.x, node.y);
				else
				{
					closedSet.Add(node.GetHashCode(), node);

					for (int i=0; i < adjacentCount; ++i)
					{
						PathNode adjacentNode = null;

						switch (i)
						{
							case 0:		adjacentNode = new PathNode(node.x, node.y+1);		break;	// Down
							case 1:		adjacentNode = new PathNode(node.x+1, node.y);		break;	// Right
							case 2:		adjacentNode = new PathNode(node.x, node.y-1);		break;	// Up
							case 3:		adjacentNode = new PathNode(node.x-1, node.y);		break;	// Left
							case 4:		adjacentNode = new PathNode(node.x+1, node.y+1);	break;	// Down-Right
							case 5:		adjacentNode = new PathNode(node.x+1, node.y-1);	break;	// Up-Right
							case 6:		adjacentNode = new PathNode(node.x-1, node.y-1);	break;	// Up-Left
							case 7:		adjacentNode = new PathNode(node.x-1, node.y+1);	break;	// Down-Left
						}

						// If adjacent is closed, skip it
						PathNode val;
						if (closedSet.TryGetValue(adjacentNode.GetHashCode(), out val) && val.x == adjacentNode.x && val.y == adjacentNode.y)
							continue;

						// If adjacent is on open list, skip it
						if (openSet.Contains(adjacentNode))
							continue;

						int tileCost = tileCostCB(adjacentNode.x, adjacentNode.y);

						// If adjacent is obstacle, skip it
						if (tileCost == Impassable)
							continue;

						// Calculate cost
						adjacentNode.cost = tileCost;

						// Add to open list
						adjacentNode.parent = node;

						int index = openSet.FindLastIndex(element => element.cost > adjacentNode.cost);
						if (index <= 0)
							openSet.Insert(0, adjacentNode);
						else
							openSet.Insert(index + 1, adjacentNode);
					}
				}
			}

			// Unreachable / Path not found
			return null;
		}
	}
}
