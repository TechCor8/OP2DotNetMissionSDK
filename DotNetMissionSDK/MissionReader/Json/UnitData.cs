using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	/// <summary>
	/// NOTE: See UnitDataExtensions for creation methods.
	/// </summary>
	[DataContract]
	public class UnitData
	{
		// Standard info
		[DataMember(Name = "ID")]			public int id						{ get; set; }
		[DataMember(Name = "TypeID")]		public string typeID				{ get; set; } = string.Empty;
		[DataMember(Name = "Health")]		public float health					{ get; set; }
		[DataMember(Name = "Lights")]		public bool lights					{ get; set; }
		[DataMember(Name = "CargoType")]	public string cargoType				{ get; set; } = string.Empty;
		[DataMember(Name = "CargoAmount")]	public int cargoAmount				{ get; set; }
		[DataMember(Name = "Direction")]	public string direction				{ get; set; } = string.Empty;
		[DataMember(Name = "Position")]		public DataLocation position		{ get; set; }

		// Used for ore mines
		[DataMember(Name = "BarYield")]		public string barYield				{ get; set; } = string.Empty;
		[DataMember(Name = "BarVariant")]	public string barVariant			{ get; set; } = string.Empty;

		// AutoLayout
		[DataMember(Name = "IgnoreLayout")]	public bool ignoreLayout			{ get; set; }
		[DataMember(Name = "MinDistance")]	public int minDistance				{ get; set; }
		[DataMember(Name = "SpawnDistance")]public int spawnDistance			{ get; set; }
		[DataMember(Name = "CreateWall")]	public bool createWall				{ get; set; }
		[DataMember(Name = "MaxTubes")]		private int? m_MaxTubes				{ get; set; }

		
		// AutoLayout
		public int maxTubes					{ get { return m_MaxTubes != null ? m_MaxTubes.Value : -1; } set { m_MaxTubes = value;			} }

		[OnDeserializing]
		private void OnDeserializing(StreamingContext context)
		{
			health = 1;
			lights = true;
		}

		public UnitData() { }
		public UnitData(UnitData clone)
		{
			id = clone.id;
			typeID = clone.typeID;
			health = clone.health;
			lights = clone.lights;
			cargoType = clone.cargoType;
			cargoAmount = clone.cargoAmount;
			direction = clone.direction;
			position = clone.position;

			barYield = clone.barYield;
			barVariant = clone.barVariant;

			ignoreLayout = clone.ignoreLayout;
			minDistance = clone.minDistance;
			spawnDistance = clone.spawnDistance;
			createWall = clone.createWall;
			m_MaxTubes = clone.m_MaxTubes;
		}
	}
}
