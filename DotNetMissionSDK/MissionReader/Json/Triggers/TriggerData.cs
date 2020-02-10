using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	// Save Data: Index, Enabled, ActionIndex (currently executing)
	[DataContract]
	public class TriggerData
	{
		[DataMember(Name = "ID")]					public int id									{ get; set; }
		[DataMember(Name = "Enabled")]				public bool enabled								{ get; set; }
		[DataMember(Name = "EventType")]			private string m_EventType						{ get; set; }
		[DataMember(Name = "Condition")]			public List<TriggerConditionData> conditions	{ get; set; }
		[DataMember(Name = "Actions")]				public List<TriggerActionData> actions			{ get; set; }

		public TriggerEventType eventType			{ get { return GetEnum<TriggerEventType>(m_EventType);			} set { m_EventType = value.ToString();		} }


		public TriggerData()
		{
			enabled = true;

			conditions = new List<TriggerConditionData>();
			actions = new List<TriggerActionData>();
		}

		public TriggerData(TriggerData clone)
		{
			id = clone.id;
			m_EventType = clone.m_EventType;

			conditions = new List<TriggerConditionData>(clone.conditions.Count);
			actions = new List<TriggerActionData>(clone.actions.Count);

			for (int i=0; i < clone.conditions.Count; ++i)
				conditions.Add(new TriggerConditionData(clone.conditions[i]));
			for (int i=0; i < clone.actions.Count; ++i)
				actions.Add(new TriggerActionData(clone.actions[i]));
		}

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
