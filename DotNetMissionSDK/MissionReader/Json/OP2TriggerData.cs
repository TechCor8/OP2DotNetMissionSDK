using System;
using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	public enum TriggerType
	{
		None,
		Victory,
		Failure,
		OnePlayerLeft,
		Evac,
		Midas,
		Operational,
		Research,
		Resource,
		Kit,
		Escape,
		Count,
		VehicleCount,
		BuildingCount,
		Attacked,
		Damaged,
		Time,
		TimeRange,
		Point,
		Rect,
		SpecialTarget
	}

	[DataContract]
	public class OP2TriggerData
	{
		[DataMember(Name = "ID")]					public int id						{ get; set; }
		[DataMember(Name = "Type")]					public string triggerType			{ get; set; } = string.Empty;
		[DataMember(Name = "Enabled")]				public bool enabled					{ get; set; }
		[DataMember(Name = "OneShot")]				public bool oneShot					{ get; set; }

		[DataMember(Name = "Actions")]				public ActionData[] actions			{ get; set; } = Array.Empty<ActionData>();


		[DataMember(Name = "TriggerID")]			public int triggerID				{ get; set; }

		[DataMember(Name = "Message")]				public string message				{ get; set; } = string.Empty;

		[DataMember(Name = "PlayerID")]				public int playerID					{ get; set; }
		[DataMember(Name = "Count")]				public int count					{ get; set; }
		[DataMember(Name = "CompareType")]			public string compareType			{ get; set; } = string.Empty;
		[DataMember(Name = "UnitType")]				public string unitType				{ get; set; } = string.Empty;

		[DataMember(Name = "Time")]					public int time						{ get; set; }
		[DataMember(Name = "MinTime")]				public int minTime					{ get; set; }
		[DataMember(Name = "MaxTime")]				public int maxTime					{ get; set; }

		[DataMember(Name = "TechID")]				public int techID					{ get; set; }

		[DataMember(Name = "X")]					public int x						{ get; set; }
		[DataMember(Name = "Y")]					public int y						{ get; set; }
		[DataMember(Name = "Width")]				public int width					{ get; set; }
		[DataMember(Name = "Height")]				public int height					{ get; set; }

		[DataMember(Name = "CargoOrWeaponType")]	public string cargoOrWeaponType		{ get; set; } = string.Empty;
		[DataMember(Name = "ResourceType")]			public string resourceType			{ get; set; } = string.Empty;
		[DataMember(Name = "CargoType")]			public string cargoType				{ get; set; } = string.Empty;

		[DataMember(Name = "CargoAmount")]			public int cargoAmount				{ get; set; }


		private T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
