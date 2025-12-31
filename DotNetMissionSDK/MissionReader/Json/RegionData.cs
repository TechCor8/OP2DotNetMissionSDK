using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public class RegionData
	{
		//[DataMember(Name = "ID")]					public int id							{ get; set; }
		[DataMember(Name = "Name")]					public string name						{ get; set; } = string.Empty;
		[DataMember(Name = "Rect")]					public DataRect rect					{ get; set; } = new DataRect();
	}
}
