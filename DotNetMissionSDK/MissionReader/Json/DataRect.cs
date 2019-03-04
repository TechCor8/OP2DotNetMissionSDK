using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class DataRect
	{
		[DataMember(Name = "MinX")]		public int minX			{ get; private set; }
		[DataMember(Name = "MinY")]		public int minY			{ get; private set; }
		[DataMember(Name = "MaxX")]		public int maxX			{ get; private set; }
		[DataMember(Name = "MaxY")]		public int maxY			{ get; private set; }
	}
}
