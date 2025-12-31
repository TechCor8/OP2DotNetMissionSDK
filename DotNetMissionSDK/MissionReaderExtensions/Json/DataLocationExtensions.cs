using DotNetMissionReader;

namespace DotNetMissionSDK
{
	public static class DataLocationExtensions
	{
		public static LOCATION ToLocation(this DataLocation data)
		{
			return new LOCATION(data.X, data.Y);
		}

		public static DataLocation ToDataLocation(this LOCATION location)
		{
			return new DataLocation(location.x, location.y);
		}
	}
}