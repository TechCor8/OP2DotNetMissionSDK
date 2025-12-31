using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	// Save Data: Index, Enabled, ActionIndex (currently executing)
	[DataContract]
	public class TriggerData
	{
		[DataMember(Name = "ID")]					public int Id									{ get; set; }
		[DataMember(Name = "Enabled")]				public bool Enabled								{ get; set; }
		[DataMember(Name = "EventType")]			private string _eventType						{ get; set; } = string.Empty;
		[DataMember(Name = "Condition")]			public List<TriggerConditionData> Conditions	{ get; set; }
		[DataMember(Name = "Actions")]				public List<TriggerActionData> Actions			{ get; set; }

		public TriggerEventType EventType			{ get { return GetEnum<TriggerEventType>(_eventType);			} set { _eventType = value.ToString();		} }


		public TriggerData()
		{
			Enabled = true;

			Conditions = new List<TriggerConditionData>();
			Actions = new List<TriggerActionData>();
		}

		public TriggerData(TriggerData clone)
		{
			Id = clone.Id;
			_eventType = clone._eventType;

			Conditions = new List<TriggerConditionData>(clone.Conditions.Count);
			Actions = new List<TriggerActionData>(clone.Actions.Count);

			for (int i=0; i < clone.Conditions.Count; ++i)
				Conditions.Add(new TriggerConditionData(clone.Conditions[i]));
			for (int i=0; i < clone.Actions.Count; ++i)
				Actions.Add(new TriggerActionData(clone.Actions[i]));
		}

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
