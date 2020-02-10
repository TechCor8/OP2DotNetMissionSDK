using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class TriggerActionData
	{
		[DataMember(Name = "Type")]					private string m_Type				{ get; set; }
		[DataMember(Name = "Modifier")]				private string m_Modifier			{ get; set; } // Set, add, subtract
		[DataMember(Name = "Subject")]				public string subject				{ get; set; } // TriggerPlayerCategory, TriggerUnitCategory, Switch#
		[DataMember(Name = "SubjectPlayer")]		public string subjectPlayer			{ get; set; } // TriggerPlayerCategory
		[DataMember(Name = "SubjectRegion")]		public string subjectRegion			{ get; set; } // Region
		[DataMember(Name = "SubjectQuantity")]		public int subjectQuantity			{ get; set; }
		
		[DataMember(Name = "Value")]				public string value					{ get; set; } // Quantity, Value, TopicID, Colony, Difficulty, Morale, Region, etc.
		[DataMember(Name = "Value2")]				public string value2				{ get; set; } // Secondary value used by some conditions

		public TriggerActionData()
		{
		}

		public TriggerActionData(TriggerActionData clone)
		{
			m_Type = clone.m_Type;
			m_Modifier = clone.m_Modifier;
			subject = clone.subject;
			subjectPlayer = clone.subjectPlayer;
			subjectRegion = clone.subjectRegion;
			subjectQuantity = clone.subjectQuantity;

			value = clone.value;
			value2 = clone.value2;
		}

		private int GetEnumOrInt<T>(string val) where T : struct
		{
			int result;
			if (int.TryParse(val, out result))
				return result;

			return System.Convert.ToInt32(GetEnum<T>(val));
		}

		private T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
