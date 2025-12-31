using DotNetMissionReader;

namespace DotNetMissionSDK
{
	public static class WallTubeDataExtensions
	{
		public static map_id GetTypeID(this WallTubeData wallTubeData) => GetEnum<map_id>(wallTubeData.TypeId);
		public static void SetTypeID(this WallTubeData wallTubeData, map_id typeID) => wallTubeData.TypeId = typeID.ToString();

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}