using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	public enum DisasterType
	{
		Meteor,
		Earthquake,
		Lightning,
		Tornado,
		Eruption,
		Blight
	}

	[DataContract]
	public class DisasterData
	{
		[DataMember(Name = "ID")]				public int id				{ get; set; }
		[DataMember(Name = "Type")]				private string m_Type		{ get; set; }
		[DataMember(Name = "SrcRect")]			public DataRect srcRect		{ get; set; }
		[DataMember(Name = "DestRect")]			public DataRect destRect	{ get; set; }
		[DataMember(Name = "Size")]				public int size				{ get; set; }
		[DataMember(Name = "Duration")]			public int duration			{ get; set; }
		[DataMember(Name = "StartTime")]		public int startTime		{ get; set; }
		[DataMember(Name = "EndTime")]			public int endTime			{ get; set; }
		[DataMember(Name = "MinDelay")]			public int minDelay			{ get; set; }
		[DataMember(Name = "MaxDelay")]			public int maxDelay			{ get; set; }

		public int timeUntilNextDisaster;

		public DisasterType type				{ get { return GetEnum<DisasterType>(m_Type);		} set { m_Type = value.ToString();			} }
		

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
