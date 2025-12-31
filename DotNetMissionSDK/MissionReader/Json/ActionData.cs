using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public class ActionData
	{
		[DataMember(Name = "Type")]				public string type			{ get; set; } = string.Empty;

		[DataMember(Name = "Message")]			public string message		{ get; set; } = string.Empty;
		[DataMember(Name = "PlayerID")]			public int playerID			{ get; set; }
		[DataMember(Name = "SoundID")]			public int soundID			{ get; set; }
	}
}
