using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public class LevelDetails
	{
		[DataMember(Name = "LevelDescription")]		public string Description		{ get; set; }
		[DataMember(Name = "MapName")]				public string MapName			{ get; set; }
		[DataMember(Name = "TechTreeName")]			public string TechTreeName		{ get; set; }
		[DataMember(Name = "MissionType")]			public string MissionType		{ get; set; }
		[DataMember(Name = "NumPlayers")]			public int NumPlayers			{ get; set; }
		[DataMember(Name = "MaxTechLevel")]			public int MaxTechLevel			{ get; set; }
		[DataMember(Name = "UnitOnlyMission")]		public bool UnitOnlyMission		{ get; set; }

		
		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}

		public LevelDetails()
		{
			Description = "No description";
			MapName = "newworld.map";
			TechTreeName = "MULTITEK.TXT";
			MissionType = "Colony";
			NumPlayers = 2;
			MaxTechLevel = 12;
		}
	}
}
