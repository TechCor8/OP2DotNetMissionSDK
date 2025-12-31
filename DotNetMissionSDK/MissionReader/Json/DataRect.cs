using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public class DataRect
	{
		[DataMember(Name = "MinX")]		public int MinX			{ get; set; }
		[DataMember(Name = "MinY")]		public int MinY			{ get; set; }
		[DataMember(Name = "MaxX")]		public int MaxX			{ get; set; }
		[DataMember(Name = "MaxY")]		public int MaxY			{ get; set; }
	}
}
