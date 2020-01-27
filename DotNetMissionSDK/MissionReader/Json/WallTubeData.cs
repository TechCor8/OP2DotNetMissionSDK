using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class WallTubeData
	{
		[DataMember(Name = "TypeID")]				private string m_TypeID					{ get; set; }
		[DataMember(Name = "Position")]				public DataLocation position			{ get; set; }

		public map_id typeID					{ get { return GetEnum<map_id>(m_TypeID);					} set { m_TypeID = value.ToString();		} }

		public WallTubeData() { }
		public WallTubeData(WallTubeData clone)
		{
			m_TypeID = clone.m_TypeID;
			position = clone.position;
		}

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
