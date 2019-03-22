using DotNetMissionSDK.Json;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK
{
	public class MAP_RECT
	{
		// Min = Top-Left, Max = Bottom-Right
		public int minX;
		public int minY;
		public int maxX;
		public int maxY;

		// Only valid for unclipped rects
		public int width		{ get { return maxX - minX + 1; } } // + 1 because max is inclusive
		public int height		{ get { return maxY - minY + 1; } } // + 1 because max is inclusive

		
		public MAP_RECT() { }
		public MAP_RECT(int minX, int minY, int maxX, int maxY)
		{
			this.minX = minX;
			this.minY = minY;
			this.maxX = maxX;
			this.maxY = maxY;
		}
		public MAP_RECT(LOCATION min, LOCATION max)
		{
			this.minX = min.x;
			this.minY = min.y;
			this.maxX = max.x;
			this.maxY = max.y;
		}
		public MAP_RECT(MAP_RECT rect)
		{
			this.minX = rect.minX;
			this.minY = rect.minY;
			this.maxX = rect.maxX;
			this.maxY = rect.maxY;
		}
		public MAP_RECT(DataRect rect)
		{
			this.minX = rect.minX;
			this.minY = rect.minY;
			this.maxX = rect.maxX;
			this.maxY = rect.maxY;
		}

		public static MAP_RECT FromPointSize(int x, int y, int width, int height)
		{
			return new MAP_RECT(x - width/2, y - height/2, x + width/2, y + height/2);
		}

		public void Inflate(int unitsWide, int unitsHigh)
		{
			minX -= unitsWide;
			maxX += unitsWide;

			minY -= unitsHigh;
			maxY += unitsHigh;
		}

		public void Offset(int shiftRight, int shiftDown)
		{
			minX += shiftRight;
			maxX += shiftRight;

			minY += shiftDown;
			maxY += shiftDown;
		}

		/// <summary>
		/// Gets random point in rect. Does not clip.
		/// </summary>
		/// <returns>Random point in rect.</returns>
		public LOCATION GetRandomPointInRect()
		{
			return new LOCATION(minX + TethysGame.GetRand(width)+1, minY + TethysGame.GetRand(height)+1);
		}

		// WARNING: IsInRect may not work the way MAP_RECT is being used in this SDK! Assumption has been that min and max are inclusive.
		// int bInRect  [Checks if the point is in the rect  [handles x wrap around for rect coordinates]]
		/*public bool IsInRect(int x, int y)
		{
			// NOTE: Is this necessary? Can we check in managed code instead of native?
			return MAP_RECT_Check(minX, minY, maxX, maxY, x, y) != 0;
		}*/

		/// <summary>
		/// Checks if point is inside rect. Min and max inclusive.
		/// </summary>
		/// <param name="x">The point x to check.</param>
		/// <param name="y">The point y to check.</param>
		/// <returns>True, if the point is in the rect.</returns>
		public bool Contains(int x, int y)
		{
			return Contains(new LOCATION(x,y));
		}

		/// <summary>
		/// Checks if point is inside rect. Min and max inclusive.
		/// </summary>
		/// <param name="point">The point to check.</param>
		/// <returns>True, if the point is in the rect.</returns>
		public bool Contains(LOCATION point)
		{
			MAP_RECT rect = this;

			if (GameMap.doesWrap)
			{
				// Copy data to prevent overwriting
				rect = new MAP_RECT(this);
				point = new LOCATION(point);

				rect.ClipToMap();
				point.ClipToMap();

				if (rect.minX > rect.maxX)
				{
					// Rect is wrapped. Compare point to rect above map max and below map min
					MAP_RECT aHigher = new MAP_RECT(rect.minX, rect.minY, rect.maxX + GameMap.width, rect.maxY);
					MAP_RECT aLower = new MAP_RECT(rect.minX - GameMap.width, rect.minY, rect.maxX, rect.maxY);

					if (CheckPointInRect(aHigher, point))
						return true;

					return CheckPointInRect(aLower, point);
				}
			}

			// No wrap, standard point in rect check
			return CheckPointInRect(this, point);
		}

		private static bool CheckPointInRect(MAP_RECT rect, LOCATION point)
		{
			return (point.x >= rect.minX && point.x <= rect.maxX &&
					point.y >= rect.minY && point.y <= rect.maxY);
		}

		/// <summary>
		/// Checks if this rect intersects another rect.
		/// Accounts for map clipping and wrapping.
		/// </summary>
		/// <param name="other">The other rect to check for intersection.</param>
		/// <returns>True if the rects intersect.</returns>
		public bool DoesRectIntersect(MAP_RECT other)
		{
			if (GameMap.doesWrap)
			{
				MAP_RECT a = new MAP_RECT(this);
				MAP_RECT b = new MAP_RECT(other);

				// Clip rects to map
				a.ClipToMap();
				b.ClipToMap();

				if (a.minX > a.maxX && b.minX > b.maxX)
				{
					// If both rects are wrapped, unwrap both to above max and compare
					a.maxX += GameMap.width;
					b.maxX += GameMap.width;

					return CheckAABB(a,b);
				}
				else if (a.minX > a.maxX)
				{
					// Only a is wrapped, compare b to a above map max and below map min
					MAP_RECT aHigher = new MAP_RECT(a.minX, a.minY, a.maxX + GameMap.width, a.maxY);
					MAP_RECT aLower = new MAP_RECT(a.minX - GameMap.width, a.minY, a.maxX, a.maxY);

					if (CheckAABB(aLower, b))
						return true;

					return CheckAABB(aHigher, b);
				}
				else if (b.minX > b.maxX)
				{
					// Only b is wrapped, compare a to b above map max and below map min
					MAP_RECT bHigher = new MAP_RECT(b.minX, b.minY, b.maxX + GameMap.width, b.maxY);
					MAP_RECT bLower = new MAP_RECT(b.minX - GameMap.width, b.minY, b.maxX, b.maxY);

					if (CheckAABB(bLower, a))
						return true;

					return CheckAABB(bHigher, a);
				}

				// Neither rects are wrapped
				return CheckAABB(a,b);
			}

			// No wrap, standard AABB check
			return CheckAABB(this, other);
		}

		private static bool CheckAABB(MAP_RECT a, MAP_RECT b)
		{
			return b.minX <= a.maxX && b.minY <= a.maxY &&
					b.maxX >= a.minX && b.maxY >= a.minY;
		}

		/// <summary>
		/// If the map wraps, the coordinates will roll over. Otherwise, the coordinates will clamp to the min/max of the map.
		/// </summary>
		public void ClipToMap()
		{
			long min = MAP_RECT_ClipToMapMin(minX, minY, maxX, maxY);
			long max = MAP_RECT_ClipToMapMax(minX, minY, maxX, maxY);

			minX = (int)(min & uint.MaxValue);
			minY = (int)(min >> 32);

			maxX = (int)(max & uint.MaxValue);
			maxY = (int)(max >> 32);
		}

		/// <summary>
		/// Returns true if rect fits in the map. Returns false if the rect goes out of bounds.
		/// <para>If the map wraps, the rect is considered inside the bounds on the x-axis unless it is larger than the map.</para>
		/// <para>Does not clip the rect.</para>
		/// </summary>
		/// <returns>True if the rect is in the map bounds.</returns>
		public bool IsInMapBounds()
		{
			// Check if y-axis is out of bounds
			if (minY < GameMap.area.minY) return false;
			if (maxY > GameMap.area.maxY) return false;

			if (GameMap.doesWrap)
			{
				// Out of bounds if rect is too wide to fit
				if (minX < GameMap.area.minX && maxX > GameMap.area.maxX)
					return false;
			}
			else
			{
				// Check if x-axis is out of bounds
				if (minX < GameMap.area.minX) return false;
				if (maxX > GameMap.area.maxX) return false;
			}

			return true;
		}

		/// <summary>
		/// Returns true if the rect wraps around the map.
		/// <para>
		/// This is considered to be true if the rect is out of bounds on the wrap axis (unclipped wrap),
		/// OR if the rect has its min greater than its max (clipped wrap).
		/// </para>
		/// </summary>
		/// <returns>True if the rect wraps.</returns>
		public bool DoesRectWrap()
		{
			// If the map doesn't wrap, the rect can't wrap
			if (!GameMap.doesWrap)
				return false;

			// Unclipped wrap
			if (minX < GameMap.area.minX || maxX > GameMap.area.maxX)
				return true;

			// Clipped wrap
			if (minX > maxX)
				return true;

			return false;
		}

		public override string ToString()
		{
			return "min (" + minX + ", " + minY + ")" + " max (" + maxX + ", " + maxY + ")";
		}

		//[DllImport("DotNetInterop.dll")] private static extern int MAP_RECT_Check(int minX, int minY, int maxX, int maxY, int x, int y);		
		[DllImport("DotNetInterop.dll")] private static extern long MAP_RECT_ClipToMapMin(int minX, int minY, int maxX, int maxY);
		[DllImport("DotNetInterop.dll")] private static extern long MAP_RECT_ClipToMapMax(int minX, int minY, int maxX, int maxY);
	}
}
