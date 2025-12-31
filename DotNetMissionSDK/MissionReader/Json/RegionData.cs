using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public class RegionData
	{
		//[DataMember(Name = "ID")]					public int Id							{ get; set; }
		[DataMember(Name = "Name")]					public string Name						{ get; set; } = string.Empty;
		[DataMember(Name = "Rect")]					public DataRect Rect					{ get; set; } = new DataRect();
	}
}
