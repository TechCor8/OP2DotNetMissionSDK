using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class DataLocation
	{
		[DataMember(Name = "X")]		public int x			{ get; private set; }
		[DataMember(Name = "Y")]		public int y			{ get; private set; }
	}
}
