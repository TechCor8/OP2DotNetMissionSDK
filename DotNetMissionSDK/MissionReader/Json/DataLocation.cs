using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public struct DataLocation
	{
		[DataMember(Name = "X")]		public int x			{ get; set; }
		[DataMember(Name = "Y")]		public int y			{ get; set; }


		public DataLocation(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}
}
