using DotNetMissionReader;
using static DotNetMissionReader.GameData;

namespace DotNetMissionSDK
{
	public static class GameDataBeaconExtensions
	{
		public static map_id GetMapID(this Beacon beacon) => GetEnum<map_id>(beacon.mapID);
		public static void SetMapID(this Beacon beacon, map_id mapID) => beacon.mapID = mapID.ToString();

		public static BeaconType GetOreType(this Beacon beacon) => GetEnum<BeaconType>(beacon.oreType);
		public static void SetOreType(this Beacon beacon, BeaconType oreType) => beacon.oreType = oreType.ToString();

		public static Yield GetBarYield(this Beacon beacon) => GetEnum<Yield>(beacon.barYield);
		public static void SetBarYield(this Beacon beacon, Yield barYield) => beacon.barYield = barYield.ToString();

		public static Variant GetBarVariant(this Beacon beacon) => GetEnum<Variant>(beacon.barVariant);
		public static void SetBarVariant(this Beacon beacon, Variant barVariant) => beacon.barVariant = barVariant.ToString();

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}

	public static class GameDataMarkerExtensions
	{
		public static MarkerType GetMarkerType(this Marker marker) => GetEnum<MarkerType>(marker.markerType);
		public static void SetMarkerType(this Marker marker, MarkerType markerType) => marker.markerType = markerType.ToString();

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}

	public static class GameDataWreckageExtensions
	{
		public static map_id GetTechID(this Wreckage wreckage) => GetEnum<map_id>(wreckage.techID);
		public static void SetTechID(this Wreckage wreckage, map_id techID) => wreckage.techID = techID.ToString();

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}