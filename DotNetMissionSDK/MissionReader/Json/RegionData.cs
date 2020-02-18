using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class RegionData
	{
		//[DataMember(Name = "ID")]					public int id							{ get; set; }
		[DataMember(Name = "Name")]					public string name						{ get; set; }
		[DataMember(Name = "Rect")]					public DataRect rect					{ get; set; }
	}
}
