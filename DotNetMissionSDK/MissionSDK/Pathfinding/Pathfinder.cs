using System;
using System.Collections.Generic;

namespace DotNetMissionSDK.Pathfinding
{
	public class Pathfinder
	{
		private class Node
		{
			public Node parent;
			public int hash;

			public int x;
			public int y;

			public int cost;

			public Node(LOCATION pt)
			{
				x = pt.x;
				y = pt.y;

				hash = x * 1000000 + y;
			}

			public Node(int x, int y)
			{
				this.x = x;
				this.y = y;

				hash = x * 1000000 + y;
			}

			/// <summary>
			/// Converts node path to LOCATION array.
			/// </summary>
			/// <returns>The position array from root to current node.</returns>
			public LOCATION[] ToArray()
			{
				Node cur = this;

				Stack<LOCATION> path = new Stack<LOCATION>();

				while (cur != null)
				{
					path.Push(new LOCATION(cur.x, cur.y));
					cur = cur.parent;
				}

				return path.ToArray();
			}
		}

		private class CompareNodeCost : IComparer<Node>
		{
			public int Compare(Node a, Node b)
			{
				return a.cost.CompareTo(b.cost);
			}
		}

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
			List<Node> openSet = new List<Node>();
			Dictionary<int, Node> closedSet = new Dictionary<int, Node>();

			int adjacentCount = 4;
			if (allowDiagonal)
				adjacentCount = 8;

			// Add start point
			openSet.Add(new Node(startPt));

			while (openSet.Count > 0)
			{
				// Get lowest cost node in open list
				Node node = openSet[openSet.Count-1];
				openSet.RemoveAt(openSet.Count-1);

				// If goal reached, return path from current node
				if (node.x == endPt.x && node.y == endPt.y)
				{
					return node.ToArray();
				}
				else
				{
					closedSet.Add(node.hash, node);

					for (int i=0; i < adjacentCount; ++i)
					{
						Node adjacentNode = null;

						switch (i)
						{
							case 0:		adjacentNode = new Node(node.x, node.y+1);		break;	// Down
							case 1:		adjacentNode = new Node(node.x+1, node.y);		break;	// Right
							case 2:		adjacentNode = new Node(node.x, node.y-1);		break;	// Up
							case 3:		adjacentNode = new Node(node.x-1, node.y);		break;	// Left
							case 4:		adjacentNode = new Node(node.x+1, node.y+1);	break;	// Down-Right
							case 5:		adjacentNode = new Node(node.x+1, node.y-1);	break;	// Up-Right
							case 6:		adjacentNode = new Node(node.x-1, node.y-1);	break;	// Up-Left
							case 7:		adjacentNode = new Node(node.x-1, node.y+1);	break;	// Down-Left
						}

						// If adjacent is closed, skip it
						Node val;
						if (closedSet.TryGetValue(adjacentNode.hash, out val) && val.x == adjacentNode.x && val.y == adjacentNode.y)
							continue;

						// If adjacent is on open list, skip it
						bool inOpenList = false;
						foreach (Node openNode in openSet)
						{
							if (openNode.x == adjacentNode.x && openNode.y == adjacentNode.y)
							{
								inOpenList = true;
								break;
							}
						}
						if (inOpenList)
							continue;

						int tileCost = tileCostCB(adjacentNode.x, adjacentNode.y);

						// If adjacent is obstacle, skip it
						if (tileCost == Impassable)
							continue;

						// Calculate cost
						if (allowDiagonal)
							adjacentNode.cost = Heuristic_Diagonal(node, endPt, tileCost);
						else
							adjacentNode.cost = Heuristic_Manhattan(node, endPt, tileCost);

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

		private static int Heuristic_Manhattan(Node current, LOCATION goal, int tileCost)
		{
			int dx = Math.Abs(current.x - goal.x);
			int dy = Math.Abs(current.y - goal.y);
			return tileCost * (dx + dy);
		}

		private static int Heuristic_Diagonal(Node current, LOCATION goal, int tileCost)
		{
			int dx = Math.Abs(current.x - goal.x);
			int dy = Math.Abs(current.y - goal.y);
			int cost2 = tileCost;
			return tileCost * (dx + dy) + (cost2 - 2 * tileCost) * Math.Min(dx, dy);
		}

		/// <summary>
		/// Performs a breadth-first search. When validTileCB returns true, the search ends and the tile location is returned.
		/// </summary>
		/// <param name="startPt">The starting point for the search.</param>
		/// <param name="tileCostCB">A callback for getting the cost of a tile.</param>
		/// <param name="validTileCB">A callback for determining if the tile completes the search.</param>
		/// <returns>The first valid tile found.</returns>
		public static LOCATION GetValidTile(LOCATION startPt, TileCostCallback tileCostCB, ValidTileCallback validTileCB)
		{
			return GetValidTile(new LOCATION[] { startPt }, tileCostCB, validTileCB);
		}

		/// <summary>
		/// Performs a breadth-first search. When validTileCB returns true, the search ends and the tile location is returned.
		/// </summary>
		/// <param name="startPt">The starting point for the search.</param>
		/// <param name="tileCostCB">A callback for getting the cost of a tile.</param>
		/// <param name="validTileCB">A callback for determining if the tile completes the search.</param>
		/// <returns>The first valid tile found.</returns>
		public static LOCATION GetValidTile(IEnumerable<LOCATION> startPts, TileCostCallback tileCostCB, ValidTileCallback validTileCB)
		{
			List<Node> openSet = new List<Node>();
			Dictionary<int, Node> closedSet = new Dictionary<int, Node>();

			int adjacentCount = 8;
			
			// Add start points
			foreach (LOCATION startPt in startPts)
				openSet.Add(new Node(startPt));

			while (openSet.Count > 0)
			{
				// Get lowest cost node in open list
				Node node = openSet[openSet.Count-1];
				openSet.RemoveAt(openSet.Count-1);

				// If adjacent tile is goal, return it
				if (validTileCB(node.x, node.y))
					return new LOCATION(node.x, node.y);
				else
				{
					closedSet.Add(node.hash, node);

					for (int i=0; i < adjacentCount; ++i)
					{
						Node adjacentNode = null;

						switch (i)
						{
							case 0:		adjacentNode = new Node(node.x, node.y+1);		break;	// Down
							case 1:		adjacentNode = new Node(node.x+1, node.y);		break;	// Right
							case 2:		adjacentNode = new Node(node.x, node.y-1);		break;	// Up
							case 3:		adjacentNode = new Node(node.x-1, node.y);		break;	// Left
							case 4:		adjacentNode = new Node(node.x+1, node.y+1);	break;	// Down-Right
							case 5:		adjacentNode = new Node(node.x+1, node.y-1);	break;	// Up-Right
							case 6:		adjacentNode = new Node(node.x-1, node.y-1);	break;	// Up-Left
							case 7:		adjacentNode = new Node(node.x-1, node.y+1);	break;	// Down-Left
						}

						// If adjacent is closed, skip it
						Node val;
						if (closedSet.TryGetValue(adjacentNode.hash, out val) && val.x == adjacentNode.x && val.y == adjacentNode.y)
							continue;

						// If adjacent is on open list, skip it
						bool inOpenList = false;
						foreach (Node openNode in openSet)
						{
							if (openNode.x == adjacentNode.x && openNode.y == adjacentNode.y)
							{
								inOpenList = true;
								break;
							}
						}
						if (inOpenList)
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
