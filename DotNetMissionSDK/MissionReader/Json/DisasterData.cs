using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class DisasterData
	{
		[DataMember(Name = "ID")]				public int id				{ get; private set; }
		[DataMember(Name = "Type")]				public string type			{ get; private set; }
		[DataMember(Name = "SrcRect")]			public DataRect srcRect		{ get; private set; }
		[DataMember(Name = "DestRect")]			public DataRect destRect	{ get; private set; }
		[DataMember(Name = "Size")]				public int size				{ get; private set; }
		[DataMember(Name = "Duration")]			public int duration			{ get; private set; }
		[DataMember(Name = "StartTime")]		public int startTime		{ get; private set; }
		[DataMember(Name = "EndTime")]			public int endTime			{ get; private set; }
		[DataMember(Name = "MinDelay")]			public int minDelay			{ get; private set; }
		[DataMember(Name = "MaxDelay")]			public int maxDelay			{ get; private set; }

		public int timeUntilNextDisaster;
	}
}
