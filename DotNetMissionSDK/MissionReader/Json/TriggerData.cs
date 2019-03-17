using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
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
	public class TriggerData
	{
		[DataMember(Name = "ID")]					public int id						{ get; private set; }
		[DataMember(Name = "Type")]					private string m_Type				{ get; set; }
		[DataMember(Name = "Enabled")]				public bool enabled					{ get; private set; }
		[DataMember(Name = "OneShot")]				public bool oneShot					{ get; private set; }

		[DataMember(Name = "Actions")]				public ActionData[] actions			{ get; private set; }


		[DataMember(Name = "TriggerID")]			public int triggerID				{ get; private set; }

		[DataMember(Name = "Message")]				public string message				{ get; private set; }

		[DataMember(Name = "PlayerID")]				public int playerID					{ get; private set; }
		[DataMember(Name = "Count")]				public int count					{ get; private set; }
		[DataMember(Name = "CompareType")]			private string m_CompareType		{ get; set; }
		[DataMember(Name = "UnitType")]				private string m_UnitType			{ get; set; }

		[DataMember(Name = "Time")]					public int time						{ get; private set; }
		[DataMember(Name = "MinTime")]				public int minTime					{ get; private set; }
		[DataMember(Name = "MaxTime")]				public int maxTime					{ get; private set; }

		[DataMember(Name = "TechID")]				public int techID					{ get; private set; }

		[DataMember(Name = "X")]					public int x						{ get; private set; }
		[DataMember(Name = "Y")]					public int y						{ get; private set; }
		[DataMember(Name = "Width")]				public int width					{ get; private set; }
		[DataMember(Name = "Height")]				public int height					{ get; private set; }

		[DataMember(Name = "CargoOrWeaponType")]	private string m_CargoOrWeaponType	{ get; set; }
		[DataMember(Name = "ResourceType")]			private string m_ResourceType		{ get; set; }
		[DataMember(Name = "CargoType")]			private string m_CargoType			{ get; set; }

		[DataMember(Name = "CargoAmount")]			public int cargoAmount				{ get; private set; }


		public TriggerType type						{ get { return GetEnum<TriggerType>(m_Type);				} }
		public CompareMode compareType				{ get { return GetEnum<CompareMode>(m_CompareType);		} }
		public map_id unitType						{ get { return GetEnum<map_id>(m_UnitType);					} }
		public map_id cargoOrWeaponType				{ get { return GetEnum<map_id>(m_CargoOrWeaponType);		} }
		public TriggerResource resourceType				{ get { return GetEnum<TriggerResource>(m_ResourceType);			} }
		public TruckCargo cargoType				{ get { return GetEnum<TruckCargo>(m_CargoType);			} }

		private T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
