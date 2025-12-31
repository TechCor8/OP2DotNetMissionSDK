using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public struct DataLocation
	{
		[DataMember(Name = "X")]		public int X			{ get; set; }
		[DataMember(Name = "Y")]		public int Y			{ get; set; }


		public DataLocation(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
	}
}
