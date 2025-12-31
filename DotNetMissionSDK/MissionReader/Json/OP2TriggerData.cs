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
		[DataMember(Name = "ID")]					public int Id						{ get; set; }
		[DataMember(Name = "Type")]					public string TriggerType			{ get; set; } = string.Empty;
		[DataMember(Name = "Enabled")]				public bool Enabled					{ get; set; }
		[DataMember(Name = "OneShot")]				public bool OneShot					{ get; set; }

		[DataMember(Name = "Actions")]				public ActionData[] Actions			{ get; set; } = Array.Empty<ActionData>();


		[DataMember(Name = "TriggerID")]			public int TriggerId				{ get; set; }

		[DataMember(Name = "Message")]				public string Message				{ get; set; } = string.Empty;

		[DataMember(Name = "PlayerID")]				public int PlayerId					{ get; set; }
		[DataMember(Name = "Count")]				public int Count					{ get; set; }
		[DataMember(Name = "CompareType")]			public string CompareType			{ get; set; } = string.Empty;
		[DataMember(Name = "UnitType")]				public string UnitType				{ get; set; } = string.Empty;

		[DataMember(Name = "Time")]					public int Time						{ get; set; }
		[DataMember(Name = "MinTime")]				public int MinTime					{ get; set; }
		[DataMember(Name = "MaxTime")]				public int MaxTime					{ get; set; }

		[DataMember(Name = "TechID")]				public int TechId					{ get; set; }

		[DataMember(Name = "X")]					public int X						{ get; set; }
		[DataMember(Name = "Y")]					public int Y						{ get; set; }
		[DataMember(Name = "Width")]				public int Width					{ get; set; }
		[DataMember(Name = "Height")]				public int Height					{ get; set; }

		[DataMember(Name = "CargoOrWeaponType")]	public string CargoOrWeaponType		{ get; set; } = string.Empty;
		[DataMember(Name = "ResourceType")]			public string ResourceType			{ get; set; } = string.Empty;
		[DataMember(Name = "CargoType")]			public string CargoType				{ get; set; } = string.Empty;

		[DataMember(Name = "CargoAmount")]			public int cargoAmount				{ get; set; }


		private T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
