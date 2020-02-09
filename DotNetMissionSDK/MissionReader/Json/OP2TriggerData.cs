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
	public class OP2TriggerData
	{
		[DataMember(Name = "ID")]					public int id						{ get; set; }
		[DataMember(Name = "Type")]					private string m_Type				{ get; set; }
		[DataMember(Name = "Enabled")]				public bool enabled					{ get; set; }
		[DataMember(Name = "OneShot")]				public bool oneShot					{ get; set; }

		[DataMember(Name = "Actions")]				public ActionData[] actions			{ get; set; }


		[DataMember(Name = "TriggerID")]			public int triggerID				{ get; set; }

		[DataMember(Name = "Message")]				public string message				{ get; set; }

		[DataMember(Name = "PlayerID")]				public int playerID					{ get; set; }
		[DataMember(Name = "Count")]				public int count					{ get; set; }
		[DataMember(Name = "CompareType")]			private string m_CompareType		{ get; set; }
		[DataMember(Name = "UnitType")]				private string m_UnitType			{ get; set; }

		[DataMember(Name = "Time")]					public int time						{ get; set; }
		[DataMember(Name = "MinTime")]				public int minTime					{ get; set; }
		[DataMember(Name = "MaxTime")]				public int maxTime					{ get; set; }

		[DataMember(Name = "TechID")]				public int techID					{ get; set; }

		[DataMember(Name = "X")]					public int x						{ get; set; }
		[DataMember(Name = "Y")]					public int y						{ get; set; }
		[DataMember(Name = "Width")]				public int width					{ get; set; }
		[DataMember(Name = "Height")]				public int height					{ get; set; }

		[DataMember(Name = "CargoOrWeaponType")]	private string m_CargoOrWeaponType	{ get; set; }
		[DataMember(Name = "ResourceType")]			private string m_ResourceType		{ get; set; }
		[DataMember(Name = "CargoType")]			private string m_CargoType			{ get; set; }

		[DataMember(Name = "CargoAmount")]			public int cargoAmount				{ get; set; }


		public TriggerType type						{ get { return GetEnum<TriggerType>(m_Type);				} set { m_Type = value.ToString();				} }
		public CompareMode compareType				{ get { return GetEnum<CompareMode>(m_CompareType);			} set { m_CompareType = value.ToString();		} }
		public map_id unitType						{ get { return GetEnum<map_id>(m_UnitType);					} set { m_UnitType = value.ToString();			} }
		public map_id cargoOrWeaponType				{ get { return GetEnum<map_id>(m_CargoOrWeaponType);		} set { m_CargoOrWeaponType = value.ToString();	} }
		public TriggerResource resourceType			{ get { return GetEnum<TriggerResource>(m_ResourceType);	} set { m_ResourceType = value.ToString();		} }
		public TruckCargo cargoType					{ get { return GetEnum<TruckCargo>(m_CargoType);			} set { m_CargoType = value.ToString();			} }

		private T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
