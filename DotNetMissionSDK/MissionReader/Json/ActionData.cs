using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public class ActionData
	{
		[DataMember(Name = "Type")]				public string Type			{ get; set; } = string.Empty;

		[DataMember(Name = "Message")]			public string Message		{ get; set; } = string.Empty;
		[DataMember(Name = "PlayerID")]			public int PlayerId			{ get; set; }
		[DataMember(Name = "SoundID")]			public int SoundId			{ get; set; }
	}
}
