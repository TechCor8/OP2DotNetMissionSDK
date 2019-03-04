using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class UnitData
	{
		[DataMember(Name = "TypeID")]		public map_id typeID				{ get; private set; }
		[DataMember(Name = "CargoType")]	public map_id cargoType				{ get; private set; }
		[DataMember(Name = "Rotation")]		public int rotation					{ get; private set; }
		[DataMember(Name = "Location")]		public DataLocation location		{ get; private set; }
	}
}
