using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public struct DataLocation
	{
		[DataMember(Name = "X")]		public int x			{ get; set; }
		[DataMember(Name = "Y")]		public int y			{ get; set; }


		public DataLocation(LOCATION location)
		{
			x = location.x;
			y = location.y;
		}

		public static implicit operator LOCATION(DataLocation data)
		{
			return new LOCATION(data.x, data.y);
		}

		public static implicit operator DataLocation(LOCATION data)
		{
			return new DataLocation(data);
		}
	}
}
