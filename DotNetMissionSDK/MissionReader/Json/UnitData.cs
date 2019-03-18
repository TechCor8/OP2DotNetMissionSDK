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

		[DataMember(Name = "IgnoreLayout")]	public bool ignoreLayout			{ get; private set; }
		[DataMember(Name = "MinDistance")]	public int minDistance				{ get; private set; }
		[DataMember(Name = "SpawnDistance")]public int spawnDistance			{ get; private set; }
		[DataMember(Name = "CreateWall")]	public bool createWall				{ get; private set; }

		public map_id typeID				{ get { return GetEnum<map_id>(m_TypeID);			} }
		public int cargoType				{ get { return GetCargoType(m_CargoType);			} }
		public UnitDirection direction		{ get { return GetEnum<UnitDirection>(m_Direction);	} }


		/// <summary>
		/// Creates an unmodifiable instance of UnitData with the passed in values.
		/// </summary>
		/// <param name="typeID"></param>
		/// <param name="cargoType"></param>
		/// <param name="direction"></param>
		/// <param name="location"></param>
		/// <param name="ignoreLayout"></param>
		/// <param name="minDistance"></param>
		/// <param name="spawnDistance"></param>
		public UnitData(map_id typeID, int cargoType, UnitDirection direction, LOCATION location, bool ignoreLayout, int minDistance, int spawnDistance)
		{
			m_TypeID = typeID.ToString();
			m_CargoType = GetCargoType(cargoType);
			m_Direction = direction.ToString();
			this.location = new DataLocation(location);
			this.ignoreLayout = ignoreLayout;
			this.minDistance = minDistance;
			this.spawnDistance = spawnDistance;
		}

		private int GetCargoType(string val)
		{
			switch (typeID)
			{
				case map_id.CargoTruck:
					return (int)GetEnum<TruckCargo>(val);
			}

			return (int)GetEnum<map_id>(val);
		}

		private string GetCargoType(int val)
		{
			switch (typeID)
			{
				case map_id.CargoTruck:
					return ((TruckCargo)val).ToString();
			}

			return ((map_id)val).ToString();
		}

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
