using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public class DataRect
	{
		[DataMember(Name = "MinX")]		public int xMin			{ get; set; }
		[DataMember(Name = "MinY")]		public int yMin			{ get; set; }
		[DataMember(Name = "MaxX")]		public int xMax			{ get; set; }
		[DataMember(Name = "MaxY")]		public int yMax			{ get; set; }
	}
}
