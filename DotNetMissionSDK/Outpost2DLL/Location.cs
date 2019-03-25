using System;

namespace DotNetMissionSDK
{
	public struct LOCATION
	{
		public int x;
		public int y;

		public LOCATION(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		public LOCATION(LOCATION location)
		{
			this.x = location.x;
			this.y = location.y;
		}

		/// <summary>
		/// If the map wraps, the coordinates will roll over. Otherwise, the coordinates will clamp to the min/max of the map.
		/// </summary>
		public void ClipToMap()
		{
			if (GameMap.doesWrap)
			{
				// Wrap around x-axis
				x = x % GameMap.bounds.width;
				if (x < 0)
					x = GameMap.bounds.width + x;
			}
			else
			{
				// Clamp x-axis
				if (x < GameMap.bounds.xMin) x = GameMap.bounds.xMin;
				if (x >= GameMap.bounds.xMax) x = GameMap.bounds.xMax-1;
			}

			// Clamp y-axis
			if (y < GameMap.bounds.yMin) y = GameMap.bounds.yMin;
			if (y >= GameMap.bounds.yMax) y = GameMap.bounds.yMax-1;
		}

		/// <summary>
		/// If the map wraps, the coordinates will roll over. Otherwise, the coordinates will clamp to the min/max of the map.
		/// Bounds max is inclusive.
		/// </summary>
		public void ClipToMapInclusive()
		{
			if (GameMap.doesWrap)
			{
				// Wrap around x-axis
				x = x % GameMap.bounds.width;
				if (x < 0)
					x = GameMap.bounds.width + x;
			}
			else
			{
				// Clamp x-axis
				if (x < GameMap.bounds.xMin) x = GameMap.bounds.xMin;
				if (x > GameMap.bounds.xMax) x = GameMap.bounds.xMax;
			}

			// Clamp y-axis
			if (y < GameMap.bounds.yMin) y = GameMap.bounds.yMin;
			if (y > GameMap.bounds.yMax) y = GameMap.bounds.yMax;
		}

		/// <summary>
		/// Returns true if location fits in the map. Returns false if the location goes out of bounds.
		/// <para>Note: If the map wraps, the x-axis is always inside the bounds.</para>
		/// <para>Does not clip the location.</para>
		/// </summary>
		/// <returns>True if the location is in the map bounds.</returns>
		public bool IsInMapBounds()
		{
			if (y < GameMap.bounds.yMin) return false;
			if (y >= GameMap.bounds.yMax) return false;

			if (!GameMap.doesWrap)
			{
				if (x < GameMap.bounds.xMin) return false;
				if (x >= GameMap.bounds.xMax) return false;
			}

			return true;
		}

		public static LOCATION operator +(LOCATION point1, LOCATION point2)
		{
			return new LOCATION(point1.x + point2.x,
								point1.y + point2.y);
		}

		public static LOCATION operator -(LOCATION point1, LOCATION point2)
		{
			return new LOCATION(point1.x - point2.x,
								point1.y - point2.y);
		}

		public static LOCATION operator *(LOCATION point1, int scalar)
		{
			return new LOCATION(point1.x * scalar,
								point1.y * scalar);
		}

		public static LOCATION operator /(LOCATION point1, int divisor)
		{
			return new LOCATION(point1.x / divisor,
								point1.y / divisor);
		}

		public override string ToString()
		{
			return "(" + x + ", " + y + ")";
		}
	}
}
