using DotNetMissionSDK.Json;
using System;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK
{
	public class LOCATION
	{
		public int x;
		public int y;

		public LOCATION() { }

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
		public LOCATION(DataLocation location)
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
			long result = LOCATION_Clip(x,y);
			x = (int)(result & uint.MaxValue);
			y = (int)(result >> 32);
		}

		/// <summary>
		/// Returns true if location fits in the map. Returns false if the location goes out of bounds.
		/// <para>Note: If the map wraps, the x-axis is always inside the bounds.</para>
		/// <para>Does not clip the location.</para>
		/// </summary>
		/// <returns>True if the location is in the map bounds.</returns>
		public bool IsInMapBounds()
		{
			if (y < GameMap.area.minY) return false;
			if (y > GameMap.area.maxY) return false;

			if (!GameMap.doesWrap)
			{
				if (x < GameMap.area.minX) return false;
				if (x > GameMap.area.maxX) return false;
			}

			return true;
		}

		public override string ToString()
		{
			return "(" + x + ", " + y + ")";
		}

		[DllImport("DotNetInterop.dll")] private static extern long LOCATION_Clip(int x, int y);
	}
}
