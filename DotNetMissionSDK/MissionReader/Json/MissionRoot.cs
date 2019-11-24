using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class MissionRoot
	{
		[DataMember(Name = "LevelDetails")]		public LevelDetails levelDetails	{ get; set; }
		[DataMember(Name = "TethysGame")]		public GameData tethysGame			{ get; set; }
		[DataMember(Name = "Players")]			public PlayerData[] players			{ get; set; }
		[DataMember(Name = "AutoLayouts")]		public AutoLayout[] layouts			{ get; set; }
		[DataMember(Name = "Disasters")]		public DisasterData[] disasters		{ get; set; }
		[DataMember(Name = "Triggers")]			public TriggerData[] triggers		{ get; set; }

		public MissionRoot()
		{
			levelDetails = new LevelDetails();
			tethysGame = new GameData();
			players = new PlayerData[2];
			layouts = new AutoLayout[0];
			disasters = new DisasterData[0];
			triggers = new TriggerData[0];

			for (int i=0; i < players.Length; ++i)
				players[i] = new PlayerData(i);
		}
	}
}
