using System.Runtime.Serialization;

namespace DotNetMissionReader
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
		[DataMember(Name = "ID")]				public int Id				{ get; set; }
		[DataMember(Name = "Type")]				private string _Type		{ get; set; } = string.Empty;
		[DataMember(Name = "SrcRect")]			public DataRect SrcRect		{ get; set; } = new DataRect();
		[DataMember(Name = "DestRect")]			public DataRect DestRect	{ get; set; } = new DataRect();
		[DataMember(Name = "Size")]				public int Size				{ get; set; }
		[DataMember(Name = "Duration")]			public int Duration			{ get; set; }
		[DataMember(Name = "StartTime")]		public int StartTime		{ get; set; }
		[DataMember(Name = "EndTime")]			public int EndTime			{ get; set; }
		[DataMember(Name = "MinDelay")]			public int MinDelay			{ get; set; }
		[DataMember(Name = "MaxDelay")]			public int MaxDelay			{ get; set; }

		public int timeUntilNextDisaster;

		public DisasterType Type				{ get { return GetEnum<DisasterType>(_Type);		} set { _Type = value.ToString();			} }
		

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
