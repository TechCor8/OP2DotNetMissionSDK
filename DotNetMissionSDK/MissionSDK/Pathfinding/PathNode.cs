using System.Collections.Generic;

namespace DotNetMissionSDK.Pathfinding.Internal
{
	/// <summary>
	/// This class represents path information about a tile.
	/// <para>Used by the Pathfinder class. There is not much need to use this class outside of the Pathfinder.</para>
	/// </summary>
	public class PathNode
	{
		public PathNode parent;
		public LOCATION position;
		public int cost;

		public int x		{ get { return position.x; } }
		public int y		{ get { return position.y; } }

		public PathNode(LOCATION pt)
		{
			position = pt;
			position.ClipToMap();
		}

		public PathNode(int x, int y)
		{
			position = new LOCATION(x, y);
			position.ClipToMap();
		}

		/// <summary>
		/// Converts node path to LOCATION array.
		/// </summary>
		/// <returns>The position array from root to current node.</returns>
		public LOCATION[] ToArray()
		{
			PathNode cur = this;

			Stack<LOCATION> path = new Stack<LOCATION>();

			while (cur != null)
			{
				path.Push(cur.position);
				cur = cur.parent;
			}

			return path.ToArray();
		}

		public override bool Equals(object obj)
		{
			PathNode t = obj as PathNode;
			if (t == null)
				return false;

			return position.x == t.position.x && position.y == t.position.y;
		}

		public override int GetHashCode()
		{
			return position.x * 1000000 + position.y;
		}
	}

	public class PathNodeCostComparer : IComparer<PathNode>
	{
		public int Compare(PathNode a, PathNode b)
		{
			return a.cost.CompareTo(b.cost);
		}
	}
}
