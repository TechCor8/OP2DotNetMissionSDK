using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public class WallTubeData
	{
		[DataMember(Name = "TypeID")]				public string TypeId					{ get; set; } = string.Empty;
		[DataMember(Name = "Position")]				public DataLocation Position			{ get; set; }

		public WallTubeData() { }
		public WallTubeData(WallTubeData clone)
		{
			TypeId = clone.TypeId;
			Position = clone.Position;
		}
	}
}
