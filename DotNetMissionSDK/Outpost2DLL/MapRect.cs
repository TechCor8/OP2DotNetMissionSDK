using DotNetMissionSDK.Json;

namespace DotNetMissionSDK
{
	/// <summary>
	/// Extended wrapper for OP2 MAP_RECT.
	/// </summary>
	public struct MAP_RECT
	{
		// Min = Top-Left inclusive
		// Max = Bottom-Right exclusive
		public int xMin;
		public int yMin;
		public int xMax;
		public int yMax;

		public int x				{ get { return xMin;						} set { xMax = value+width; xMin = value;	} }
		public int y				{ get { return yMin;						} set { yMax = value+height; yMin = value;	} }
		public int width			{ get { return xMax - xMin;					} set { xMax = xMin + value;				} }
		public int height			{ get { return yMax - yMin;					} set { yMax = yMin + value;				} }

		public LOCATION position	{ get { return new LOCATION(x, y);			} set { x = value.x; y = value.y;			} }
		public LOCATION size		{ get { return new LOCATION(width, height); } set { width = value.x; height = value.y;	} }

		
		//public MAP_RECT() { }
		public MAP_RECT(int x, int y, int width, int height)
		{
			xMin = x;
			yMin = y;
			xMax = x + width;
			yMax = y + height;
		}
		public MAP_RECT(LOCATION point, LOCATION size)
		{
			this.xMin = point.x;
			this.yMin = point.y;
			this.xMax = point.x + size.x;
			this.yMax = point.y + size.y;
		}
		public MAP_RECT(MAP_RECT rect)
		{
			this.xMin = rect.xMin;
			this.yMin = rect.yMin;
			this.xMax = rect.xMax;
			this.yMax = rect.yMax;
		}
		public static MAP_RECT FromMinMax(int xMin, int yMin, int xMax, int yMax)
		{
			return new MAP_RECT(xMin, yMin, xMax-xMin, yMax-yMin);
		}

		public void Inflate(int size)
		{
			Inflate(size, size);
		}

		public void Inflate(int unitsWide, int unitsHigh)
		{
			xMin -= unitsWide;
			xMax += unitsWide;

			yMin -= unitsHigh;
			yMax += unitsHigh;
		}

		public void Offset(int shiftRight, int shiftDown)
		{
			xMin += shiftRight;
			xMax += shiftRight;

			yMin += shiftDown;
			yMax += shiftDown;
		}

		/// <summary>
		/// Gets random point in rect. Does not clip.
		/// </summary>
		/// <returns>Random point in rect.</returns>
		public LOCATION GetRandomPointInRect()
		{
			return new LOCATION(xMin + TethysGame.GetRand(width),
								yMin + TethysGame.GetRand(height));
		}

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
		/// Checks if point is inside rect. Min inclusive and max exclusive.
		/// </summary>
		/// <param name="point">The point to check.</param>
		/// <returns>True, if the point is in the rect.</returns>
		public bool Contains(LOCATION point)
		{
			MAP_RECT rect = this;

			if (GameMap.doesWrap)
			{
				rect.ClipToMap();
				point.ClipToMap();

				if (rect.xMin > rect.xMax)
				{
					// Rect is wrapped. Compare point to rect above map max and below map min
					MAP_RECT aHigher = new MAP_RECT(rect.x, rect.y, rect.width + GameMap.bounds.width, rect.height);
					MAP_RECT aLower = new MAP_RECT(rect.x - GameMap.bounds.width, rect.y, rect.width, rect.height);

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
			return (point.x >= rect.xMin && point.x < rect.xMax &&
					point.y >= rect.yMin && point.y < rect.yMax);
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
				MAP_RECT a = this;
				MAP_RECT b = other;

				// Clip rects to map
				a.ClipToMap();
				b.ClipToMap();

				if (a.xMin > a.xMax && b.xMin > b.xMax)
				{
					// If both rects are wrapped, unwrap both to above max and compare
					a.xMax += GameMap.bounds.width;
					b.xMax += GameMap.bounds.width;

					return CheckAABB(a,b);
				}
				else if (a.xMin > a.xMax)
				{
					// Only a is wrapped, compare b to a above map max and below map min
					MAP_RECT aHigher = FromMinMax(a.xMin, a.yMin, a.xMax + GameMap.bounds.width, a.yMax);
					MAP_RECT aLower = FromMinMax(a.xMin - GameMap.bounds.width, a.yMin, a.xMax, a.yMax);

					if (CheckAABB(aLower, b))
						return true;

					return CheckAABB(aHigher, b);
				}
				else if (b.xMin > b.xMax)
				{
					// Only b is wrapped, compare a to b above map max and below map min
					MAP_RECT bHigher = FromMinMax(b.xMin, b.yMin, b.xMax + GameMap.bounds.width, b.yMax);
					MAP_RECT bLower = FromMinMax(b.xMin - GameMap.bounds.width, b.yMin, b.xMax, b.yMax);

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
			return b.xMin < a.xMax && b.yMin < a.yMax &&
					b.xMax > a.xMin && b.yMax > a.yMin;
		}

		/// <summary>
		/// If the map wraps, the coordinates will roll over. Otherwise, the coordinates will clamp to the min/max of the map.
		/// </summary>
		public void ClipToMap()
		{
			LOCATION min = new LOCATION(xMin, yMin);
			min.ClipToMap();
			xMin = min.x;
			yMin = min.y;

			LOCATION max = new LOCATION(xMax, yMax);
			max.ClipToMapInclusive();
			xMax = max.x;
			yMax = max.y;
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
			if (yMin < GameMap.bounds.yMin) return false;
			if (yMax > GameMap.bounds.yMax) return false;

			if (GameMap.doesWrap)
			{
				// Out of bounds if rect is too wide to fit
				if (xMin < GameMap.bounds.xMin && xMax > GameMap.bounds.xMax)
					return false;
			}
			else
			{
				// Check if x-axis is out of bounds
				if (xMin < GameMap.bounds.xMin) return false;
				if (xMax > GameMap.bounds.xMax) return false;
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
			if (xMin < GameMap.bounds.xMin || xMax > GameMap.bounds.xMax)
				return true;

			// Clipped wrap
			if (xMin > xMax)
				return true;

			return false;
		}

		public override string ToString()
		{
			return "min (" + xMin + ", " + yMin + ")" + " max (" + xMax + ", " + yMax + ")";
		}
	}
}
