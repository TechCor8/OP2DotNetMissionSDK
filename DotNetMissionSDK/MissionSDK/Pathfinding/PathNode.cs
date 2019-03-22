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
		public int x;
		public int y;

		public int cost;

		public PathNode(LOCATION pt)
		{
			LOCATION pos = new LOCATION(pt.x, pt.y);
			pos.ClipToMap();

			x = pos.x;
			y = pos.y;
		}

		public PathNode(int x, int y)
		{
			LOCATION pos = new LOCATION(x, y);
			pos.ClipToMap();

			this.x = pos.x;
			this.y = pos.y;
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
				path.Push(new LOCATION(cur.x, cur.y));
				cur = cur.parent;
			}

			return path.ToArray();
		}

		public override bool Equals(object obj)
		{
			PathNode t = obj as PathNode;
			if (t == null)
				return false;

			return x == t.x && y == t.y;
		}

		public override int GetHashCode()
		{
			return x * 1000000 + y;
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
