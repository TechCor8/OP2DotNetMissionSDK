using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class MissionRoot
	{
		[DataMember(Name = "LevelDetails")]		public LevelDetails levelDetails			{ get; set; }
		[DataMember(Name = "MasterVariant")]	public MissionVariant masterVariant			{ get; set; }
		[DataMember(Name = "MissionVariants")]	public List<MissionVariant> missionVariants	{ get; set; }
		[DataMember(Name = "Disasters")]		public DisasterData[] disasters				{ get; set; }
		[DataMember(Name = "Triggers")]			public TriggerData[] triggers				{ get; set; }

		public MissionRoot()
		{
			levelDetails = new LevelDetails();
			masterVariant = new MissionVariant();
			missionVariants = new List<MissionVariant>();
			disasters = new DisasterData[0];
			triggers = new TriggerData[0];
		}
	}
}
