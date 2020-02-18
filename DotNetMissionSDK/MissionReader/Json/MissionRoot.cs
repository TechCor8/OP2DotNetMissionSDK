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
		[DataMember(Name = "Triggers")]			public OP2TriggerData[] triggers			{ get; set; }
		[DataMember(Name = "Regions")]			public List<RegionData> regions				{ get; set; }

		public MissionRoot()
		{
			levelDetails = new LevelDetails();
			masterVariant = new MissionVariant();
			missionVariants = new List<MissionVariant>();
			disasters = new DisasterData[0];
			triggers = new OP2TriggerData[0];
			regions = new List<RegionData>();
		}

		[OnDeserializing]
		private void OnDeserializing(StreamingContext context)
		{
			regions = new List<RegionData>();
		}
	}
}
