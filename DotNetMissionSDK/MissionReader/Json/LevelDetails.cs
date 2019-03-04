using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class LevelDetails
	{
		[DataMember(Name = "LevelDescription")]		public string description		{ get; private set; }
		[DataMember(Name = "MapName")]				public string mapName			{ get; private set; }
		[DataMember(Name = "TechTreeName")]			public string techTreeName		{ get; private set; }
		[DataMember(Name = "MissionType")]			public MissionTypes missionType	{ get; private set; }
		[DataMember(Name = "NumPlayers")]			public int numPlayers			{ get; private set; }
		[DataMember(Name = "MaxTechLevel")]			public int maxTechLevel			{ get; private set; }
		[DataMember(Name = "UnitOnlyMission")]		public bool unitOnlyMission		{ get; private set; }
	}
}
