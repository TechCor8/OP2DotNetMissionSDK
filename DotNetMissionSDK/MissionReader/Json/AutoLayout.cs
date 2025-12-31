using System;
using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public class AutoLayout
	{
		[DataMember(Name = "PlayerID")]			public int PlayerId					{ get; set; }
		[DataMember(Name = "BaseCenterPt")]		public DataLocation BaseCenterPt	{ get; set; }

		[DataMember(Name = "Units")]			public UnitData[] Units				{ get; set; } = Array.Empty<UnitData>();

		public AutoLayout() { }
		public AutoLayout(AutoLayout clone)
		{
			PlayerId = clone.PlayerId;
			BaseCenterPt = clone.BaseCenterPt;

			Units = new UnitData[clone.Units.Length];
			for (int i=0; i < Units.Length; ++i)
				Units[i] = new UnitData(clone.Units[i]);
		}
	}
}
