using DotNetMissionReader;

namespace DotNetMissionSDK
{
	public static class LevelDetailsExtensions
	{
		public static MissionType GetMissionType(this LevelDetails levelDetails) => GetEnum<MissionType>(levelDetails.MissionType);
		public static void SetMissionType(this LevelDetails levelDetails, MissionType missionType) => levelDetails.MissionType = missionType.ToString();

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}