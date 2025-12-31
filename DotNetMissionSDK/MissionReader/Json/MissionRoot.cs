using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public class MissionRoot
	{
		public const string SDKVersion = "0"; // May only use dot versioning (e.g. 1.0.0). Dot not required.

		[DataMember(Name = "SDKVersion")]		public string SdkVersion					{ get; set; }
		[DataMember(Name = "LevelDetails")]		public LevelDetails LevelDetails			{ get; set; }
		[DataMember(Name = "MasterVariant")]	public MissionVariant MasterVariant			{ get; set; }
		[DataMember(Name = "MissionVariants")]	public List<MissionVariant> MissionVariants	{ get; set; }
		[DataMember(Name = "Disasters")]		public DisasterData[] Disasters				{ get; set; }
		[DataMember(Name = "Triggers")]			public OP2TriggerData[] Triggers			{ get; set; }
		[DataMember(Name = "Regions")]			public List<RegionData> Regions				{ get; set; }

		public MissionRoot()
		{
			SdkVersion = SDKVersion;
			LevelDetails = new LevelDetails();
			MasterVariant = new MissionVariant();
			MissionVariants = new List<MissionVariant>();
			Disasters = new DisasterData[0];
			Triggers = new OP2TriggerData[0];
			Regions = new List<RegionData>();
		}

		[OnDeserializing]
		private void OnDeserializing(StreamingContext context)
		{
			SdkVersion = SDKVersion;
			Regions = new List<RegionData>();
		}
	}
}
