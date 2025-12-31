using DotNetMissionReader;

namespace DotNetMissionSDK
{
	public static class OP2TriggerDataExtensions
	{
		public static TriggerType GetTriggerType(this OP2TriggerData triggerData) => GetEnum<TriggerType>(triggerData.TriggerType);
		public static void SetTriggerType(this OP2TriggerData triggerData, TriggerType triggerType) => triggerData.TriggerType = triggerType.ToString();

		public static CompareMode GetCompareType(this OP2TriggerData triggerData) => GetEnum<CompareMode>(triggerData.CompareType);
		public static void SetCompareType(this OP2TriggerData triggerData, CompareMode compareType) => triggerData.CompareType = compareType.ToString();

		public static map_id GetUnitType(this OP2TriggerData triggerData) => GetEnum<map_id>(triggerData.UnitType);
		public static void SetUnitType(this OP2TriggerData triggerData, map_id unitType) => triggerData.UnitType = unitType.ToString();

		public static map_id GetCargoOrWeaponType(this OP2TriggerData triggerData) => GetEnum<map_id>(triggerData.CargoOrWeaponType);
		public static void SetCargoOrWeaponType(this OP2TriggerData triggerData, map_id cargoOrWeaponType) => triggerData.CargoOrWeaponType = cargoOrWeaponType.ToString();

		public static TriggerResource GetResourceType(this OP2TriggerData triggerData) => GetEnum<TriggerResource>(triggerData.ResourceType);
		public static void SetResourceType(this OP2TriggerData triggerData, TriggerResource resourceType) => triggerData.ResourceType = resourceType.ToString();

		public static TruckCargo GetCargoType(this OP2TriggerData triggerData) => GetEnum<TruckCargo>(triggerData.CargoType);
		public static void SetCargoType(this OP2TriggerData triggerData, TruckCargo cargoType) => triggerData.CargoType = cargoType.ToString();

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}