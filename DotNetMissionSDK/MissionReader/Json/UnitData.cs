using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class UnitData
	{
		[DataMember(Name = "TypeID")]		private string m_TypeID				{ get; set; }
		[DataMember(Name = "CargoType")]	private string m_CargoType			{ get; set; }
		[DataMember(Name = "Direction")]	private string m_Direction			{ get; set; }
		[DataMember(Name = "Location")]		public DataLocation location		{ get; private set; }

		public map_id typeID				{ get { return GetEnum<map_id>(m_TypeID);			} }
		public int cargoType				{ get { return GetCargoType(m_CargoType);			} }
		public UnitDirection rotation		{ get { return GetEnum<UnitDirection>(m_Direction);	} }


		private int GetCargoType(string val)
		{
			switch (typeID)
			{
				case map_id.CargoTruck:
					return (int)GetEnum<TruckCargo>(val);
			}

			return (int)GetEnum<map_id>(val);
		}

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
