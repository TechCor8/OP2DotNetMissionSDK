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
		[DataMember(Name = "ID")]			public int Id						{ get; set; }
		[DataMember(Name = "TypeID")]		public string TypeId				{ get; set; } = string.Empty;
		[DataMember(Name = "Health")]		public float Health					{ get; set; }
		[DataMember(Name = "Lights")]		public bool Lights					{ get; set; }
		[DataMember(Name = "CargoType")]	public string CargoType				{ get; set; } = string.Empty;
		[DataMember(Name = "CargoAmount")]	public int CargoAmount				{ get; set; }
		[DataMember(Name = "Direction")]	public string Direction				{ get; set; } = string.Empty;
		[DataMember(Name = "Position")]		public DataLocation Position		{ get; set; }

		// Used for ore mines
		[DataMember(Name = "BarYield")]		public string BarYield				{ get; set; } = string.Empty;
		[DataMember(Name = "BarVariant")]	public string BarVariant			{ get; set; } = string.Empty;

		// AutoLayout
		[DataMember(Name = "IgnoreLayout")]	public bool IgnoreLayout			{ get; set; }
		[DataMember(Name = "MinDistance")]	public int MinDistance				{ get; set; }
		[DataMember(Name = "SpawnDistance")]public int SpawnDistance			{ get; set; }
		[DataMember(Name = "CreateWall")]	public bool CreateWall				{ get; set; }
		[DataMember(Name = "MaxTubes")]		private int? _MaxTubes				{ get; set; }

		
		// AutoLayout
		public int maxTubes					{ get { return _MaxTubes != null ? _MaxTubes.Value : -1; } set { _MaxTubes = value;			} }

		[OnDeserializing]
		private void OnDeserializing(StreamingContext context)
		{
			Health = 1;
			Lights = true;
		}

		public UnitData() { }
		public UnitData(UnitData clone)
		{
			Id = clone.Id;
			TypeId = clone.TypeId;
			Health = clone.Health;
			Lights = clone.Lights;
			CargoType = clone.CargoType;
			CargoAmount = clone.CargoAmount;
			Direction = clone.Direction;
			Position = clone.Position;

			BarYield = clone.BarYield;
			BarVariant = clone.BarVariant;

			IgnoreLayout = clone.IgnoreLayout;
			MinDistance = clone.MinDistance;
			SpawnDistance = clone.SpawnDistance;
			CreateWall = clone.CreateWall;
			_MaxTubes = clone._MaxTubes;
		}
	}
}
