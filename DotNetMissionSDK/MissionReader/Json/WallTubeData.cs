using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public class WallTubeData
	{
		[DataMember(Name = "TypeID")]				public string typeID					{ get; set; } = string.Empty;
		[DataMember(Name = "Position")]				public DataLocation position			{ get; set; }

		public WallTubeData() { }
		public WallTubeData(WallTubeData clone)
		{
			typeID = clone.typeID;
			position = clone.position;
		}
	}
}
