using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class AutoLayout
	{
		[DataMember(Name = "PlayerID")]			public int playerID					{ get; set; }
		[DataMember(Name = "BaseCenterPt")]		public DataLocation baseCenterPt	{ get; set; }

		[DataMember(Name = "Units")]			public UnitData[] units				{ get; set; }
	}
}
