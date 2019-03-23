using System;

namespace DotNetMissionSDK
{
	public struct LOCATION
	{
		public int x;
		public int y;

		public LOCATION(int tileX, int tileY)
		{
			this.x = tileX;
			this.y = tileY;
		}
		public LOCATION(LOCATION location)
		{
			this.x = location.x;
			this.y = location.y;
		}
		public void Add(LOCATION vector)
		{
			x += vector.x;
			y += vector.y;
		}

		public static LOCATION Difference(LOCATION a, LOCATION b)
		{
			return new LOCATION(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y));
		}

		/// <summary>
		/// If the map wraps, the coordinates will roll over. Otherwise, the coordinates will clamp to the min/max of the map.
		/// </summary>
		public void ClipToMap()
		{
			if (GameMap.doesWrap)
			{
				// Wrap around x-axis
				x = x % GameMap.width;
				if (x < 0)
					x = GameMap.width + x;
			}
			else
			{
				// Clamp x-axis
				if (x < GameMap.bounds.minX) x = GameMap.bounds.minX;
				if (x > GameMap.bounds.maxX) x = GameMap.bounds.maxX;
			}

			// Clamp y-axis
			if (y < GameMap.bounds.minY) y = GameMap.bounds.minY;
			if (y > GameMap.bounds.maxY) y = GameMap.bounds.maxY;
		}

		/// <summary>
		/// Returns true if location fits in the map. Returns false if the location goes out of bounds.
		/// <para>Note: If the map wraps, the x-axis is always inside the bounds.</para>
		/// <para>Does not clip the location.</para>
		/// </summary>
		/// <returns>True if the location is in the map bounds.</returns>
		public bool IsInMapBounds()
		{
			if (y < GameMap.bounds.minY) return false;
			if (y > GameMap.bounds.maxY) return false;

			if (!GameMap.doesWrap)
			{
				if (x < GameMap.bounds.minX) return false;
				if (x > GameMap.bounds.maxX) return false;
			}

			return true;
		}

		public override string ToString()
		{
			return "(" + x + ", " + y + ")";
		}
	}
}
