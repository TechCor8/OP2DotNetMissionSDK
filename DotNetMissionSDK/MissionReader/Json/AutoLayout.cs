using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class AutoLayout
	{
		[DataMember(Name = "PlayerID")]			public int playerID					{ get; set; }
		[DataMember(Name = "BaseCenterPt")]		public DataLocation baseCenterPt	{ get; set; }

		[DataMember(Name = "Units")]			public UnitData[] units				{ get; set; }

		public AutoLayout() { }
		public AutoLayout(AutoLayout clone)
		{
			playerID = clone.playerID;
			baseCenterPt = clone.baseCenterPt;

			units = new UnitData[clone.units.Length];
			for (int i=0; i < units.Length; ++i)
				units[i] = new UnitData(clone.units[i]);
		}
	}
}
