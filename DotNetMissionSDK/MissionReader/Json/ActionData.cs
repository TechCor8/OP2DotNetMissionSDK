using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class ActionData
	{
		[DataMember(Name = "Type")]				public string type			{ get; set; }

		[DataMember(Name = "Message")]			public string message		{ get; set; }
		[DataMember(Name = "PlayerID")]			public int playerID			{ get; set; }
		[DataMember(Name = "SoundID")]			public int soundID			{ get; set; }
	}
}
