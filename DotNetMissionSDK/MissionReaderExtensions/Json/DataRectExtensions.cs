using DotNetMissionReader;

namespace DotNetMissionSDK
{
	public static class DataRectExtensions
	{
		/// <summary>
		/// Gets random point in rect. Does not clip.
		/// </summary>
		/// <returns>Random point in rect.</returns>
		public static LOCATION GetRandomPointInRect(this DataRect dataRect)
		{
			return dataRect.ToMapRect().GetRandomPointInRect();
		}

		public static MAP_RECT ToMapRect(this DataRect data)
		{
			return MAP_RECT.FromMinMax(data.xMin, data.yMin, data.xMax, data.yMax);
		}
	}
}