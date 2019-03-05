using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class ActionData
	{
		[DataMember(Name = "Type")]				public string type			{ get; private set; }

		[DataMember(Name = "Message")]			public string message		{ get; private set; }
		[DataMember(Name = "PlayerID")]			public int playerID			{ get; private set; }
		[DataMember(Name = "SoundID")]			public int soundID			{ get; private set; }
	}
}
