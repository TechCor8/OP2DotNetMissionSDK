using DotNetMissionReader;
using static DotNetMissionReader.GameData;

namespace DotNetMissionSDK
{
	public static class GameDataBeaconExtensions
	{
		public static map_id GetMapID(this BeaconData beacon) => GetEnum<map_id>(beacon.MapId);
		public static void SetMapID(this BeaconData beacon, map_id mapID) => beacon.MapId = mapID.ToString();

		public static BeaconType GetOreType(this BeaconData beacon) => GetEnum<BeaconType>(beacon.OreType);
		public static void SetOreType(this BeaconData beacon, BeaconType oreType) => beacon.OreType = oreType.ToString();

		public static Yield GetBarYield(this BeaconData beacon) => GetEnum<Yield>(beacon.BarYield);
		public static void SetBarYield(this BeaconData beacon, Yield barYield) => beacon.BarYield = barYield.ToString();

		public static Variant GetBarVariant(this BeaconData beacon) => GetEnum<Variant>(beacon.BarVariant);
		public static void SetBarVariant(this BeaconData beacon, Variant barVariant) => beacon.BarVariant = barVariant.ToString();

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}

	public static class GameDataMarkerExtensions
	{
		public static MarkerType GetMarkerType(this MarkerData marker) => GetEnum<MarkerType>(marker.MarkerType);
		public static void SetMarkerType(this MarkerData marker, MarkerType markerType) => marker.MarkerType = markerType.ToString();

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}

	public static class GameDataWreckageExtensions
	{
		public static map_id GetTechID(this WreckageData wreckage) => GetEnum<map_id>(wreckage.TechId);
		public static void SetTechID(this WreckageData wreckage, map_id techID) => wreckage.TechId = techID.ToString();

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}