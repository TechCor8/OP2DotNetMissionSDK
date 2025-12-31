using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public class LevelDetails
	{
		[DataMember(Name = "LevelDescription")]		public string description		{ get; set; }
		[DataMember(Name = "MapName")]				public string mapName			{ get; set; }
		[DataMember(Name = "TechTreeName")]			public string techTreeName		{ get; set; }
		[DataMember(Name = "MissionType")]			public string missionType		{ get; set; }
		[DataMember(Name = "NumPlayers")]			public int numPlayers			{ get; set; }
		[DataMember(Name = "MaxTechLevel")]			public int maxTechLevel			{ get; set; }
		[DataMember(Name = "UnitOnlyMission")]		public bool unitOnlyMission		{ get; set; }

		
		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}

		public LevelDetails()
		{
			description = "No description";
			mapName = "newworld.map";
			techTreeName = "MULTITEK.TXT";
			missionType = "Colony";
			numPlayers = 2;
			maxTechLevel = 12;
		}
	}
}
