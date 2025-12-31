using DotNetMissionReader;
using DotNetMissionSDK.AI;

namespace DotNetMissionSDK
{
	public static class PlayerDataExtensions
	{
		// PlayerData extensions
		public static BotType GetBotType(this PlayerData playerData) => GetEnum<BotType>(playerData.botType);
		public static void SetBotType(this PlayerData playerData, BotType botType) => playerData.botType = botType.ToString();

		public static PlayerColor GetColor(this PlayerData playerData) => GetEnum<PlayerColor>(playerData.color);
		public static void SetColor(this PlayerData playerData, PlayerColor color) => playerData.color = color.ToString();

		// ResourceData extensions
		public static MoraleLevel GetMoraleLevel(this PlayerData.ResourceData resourceData) => GetEnum<MoraleLevel>(resourceData.moraleLevel);
		public static void SetMoraleLevel(this PlayerData.ResourceData resourceData, MoraleLevel moraleLevel) => resourceData.moraleLevel = moraleLevel.ToString();

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}