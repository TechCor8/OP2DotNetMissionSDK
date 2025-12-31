using DotNetMissionReader;
using static DotNetMissionReader.GameData;

namespace DotNetMissionSDK
{
	public static class GameDataBeaconExtensions
	{
		public static map_id GetMapID(this Beacon beacon) => GetEnum<map_id>(beacon.MapId);
		public static void SetMapID(this Beacon beacon, map_id mapID) => beacon.MapId = mapID.ToString();

		public static BeaconType GetOreType(this Beacon beacon) => GetEnum<BeaconType>(beacon.OreType);
		public static void SetOreType(this Beacon beacon, BeaconType oreType) => beacon.OreType = oreType.ToString();

		public static Yield GetBarYield(this Beacon beacon) => GetEnum<Yield>(beacon.BarYield);
		public static void SetBarYield(this Beacon beacon, Yield barYield) => beacon.BarYield = barYield.ToString();

		public static Variant GetBarVariant(this Beacon beacon) => GetEnum<Variant>(beacon.BarVariant);
		public static void SetBarVariant(this Beacon beacon, Variant barVariant) => beacon.BarVariant = barVariant.ToString();

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}

	public static class GameDataMarkerExtensions
	{
		public static MarkerType GetMarkerType(this Marker marker) => GetEnum<MarkerType>(marker.MarkerType);
		public static void SetMarkerType(this Marker marker, MarkerType markerType) => marker.MarkerType = markerType.ToString();

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}

	public static class GameDataWreckageExtensions
	{
		public static map_id GetTechID(this Wreckage wreckage) => GetEnum<map_id>(wreckage.TechId);
		public static void SetTechID(this Wreckage wreckage, map_id techID) => wreckage.TechId = techID.ToString();

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}