using DotNetMissionReader;

namespace DotNetMissionSDK
{
	public static class OP2TriggerDataExtensions
	{
		public static TriggerType GetTriggerType(this OP2TriggerData triggerData) => GetEnum<TriggerType>(triggerData.triggerType);
		public static void SetTriggerType(this OP2TriggerData triggerData, TriggerType triggerType) => triggerData.triggerType = triggerType.ToString();

		public static CompareMode GetCompareType(this OP2TriggerData triggerData) => GetEnum<CompareMode>(triggerData.compareType);
		public static void SetCompareType(this OP2TriggerData triggerData, CompareMode compareType) => triggerData.compareType = compareType.ToString();

		public static map_id GetUnitType(this OP2TriggerData triggerData) => GetEnum<map_id>(triggerData.unitType);
		public static void SetUnitType(this OP2TriggerData triggerData, map_id unitType) => triggerData.unitType = unitType.ToString();

		public static map_id GetCargoOrWeaponType(this OP2TriggerData triggerData) => GetEnum<map_id>(triggerData.cargoOrWeaponType);
		public static void SetCargoOrWeaponType(this OP2TriggerData triggerData, map_id cargoOrWeaponType) => triggerData.cargoOrWeaponType = cargoOrWeaponType.ToString();

		public static TriggerResource GetResourceType(this OP2TriggerData triggerData) => GetEnum<TriggerResource>(triggerData.resourceType);
		public static void SetResourceType(this OP2TriggerData triggerData, TriggerResource resourceType) => triggerData.resourceType = resourceType.ToString();

		public static TruckCargo GetCargoType(this OP2TriggerData triggerData) => GetEnum<TruckCargo>(triggerData.cargoType);
		public static void SetCargoType(this OP2TriggerData triggerData, TruckCargo cargoType) => triggerData.cargoType = cargoType.ToString();

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}