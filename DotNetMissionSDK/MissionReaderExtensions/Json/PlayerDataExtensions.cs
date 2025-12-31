using DotNetMissionReader;
using DotNetMissionSDK.AI;

namespace DotNetMissionSDK
{
	public static class PlayerDataExtensions
	{
		// PlayerData extensions
		public static BotType GetBotType(this PlayerData playerData) => GetEnum<BotType>(playerData.BotType);
		public static void SetBotType(this PlayerData playerData, BotType botType) => playerData.BotType = botType.ToString();

		public static PlayerColor GetColor(this PlayerData playerData) => GetEnum<PlayerColor>(playerData.Color);
		public static void SetColor(this PlayerData playerData, PlayerColor color) => playerData.Color = color.ToString();

		// ResourceData extensions
		public static MoraleLevel GetMoraleLevel(this PlayerData.ResourceData resourceData) => GetEnum<MoraleLevel>(resourceData.MoraleLevel);
		public static void SetMoraleLevel(this PlayerData.ResourceData resourceData, MoraleLevel moraleLevel) => resourceData.MoraleLevel = moraleLevel.ToString();

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}