using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class GameData
	{
		// [Data Accessors]
		[DataMember(Name = "DaylightEverywhere")]		public bool daylightEverywhere			{ get; set; }
		[DataMember(Name = "DaylightMoves")]			public bool daylightMoves				{ get; set; }
		[DataMember(Name = "InitialLightLevel")]		public int initialLightLevel			{ get; set; }

		[DataMember(Name = "MusicPlayList")]			public MusicPlayList musicPlayList		{ get; set; }
		[DataMember(Name = "Beacons")]					public List<Beacon> beacons				{ get; set; }
		[DataMember(Name = "Markers")]					public List<Marker> markers				{ get; set; }
		[DataMember(Name = "Wreckage")]					public List<Wreckage> wreckage			{ get; set; }
		[DataMember(Name = "WallTubes")]				public List<WallTube> wallTubes			{ get; set; }


		// [Data Classes]
		[DataContract]
		public class MusicPlayList
		{
			[DataMember(Name = "RepeatStartIndex")]		public int repeatStartIndex				{ get; set; }
			[DataMember(Name = "SongIDs")]				public int[] songIDs					{ get; set; }
		}

		[DataContract]
		public class Beacon
		{
			[DataMember(Name = "ID")]					public int id							{ get; set; }
			[DataMember(Name = "MapID")]				private string m_MapID					{ get; set; }
			[DataMember(Name = "OreType")]				private string m_OreType				{ get; set; }
			[DataMember(Name = "BarYield")]				private string m_BarYield				{ get; set; }
			[DataMember(Name = "BarVariant")]			private string m_BarVariant				{ get; set; }
			[DataMember(Name = "Position")]				public DataLocation position			{ get; set; }

			public map_id mapID						{ get { return GetEnum<map_id>(m_MapID);					} set { m_MapID = value.ToString();			} }
			public BeaconType oreType				{ get { return GetEnum<BeaconType>(m_OreType);				} set { m_OreType = value.ToString();		} }
			public Yield barYield					{ get { return GetEnum<Yield>(m_BarYield);					} set { m_BarYield = value.ToString();		} }
			public Variant barVariant				{ get { return GetEnum<Variant>(m_BarVariant);				} set { m_BarVariant = value.ToString();	} }
		}

		[DataContract]
		public class Marker
		{
			[DataMember(Name = "ID")]					public int id							{ get; set; }
			[DataMember(Name = "MarkerType")]			private string m_MarkerType				{ get; set; }
			[DataMember(Name = "Position")]				public DataLocation position			{ get; set; }

			public MarkerType markerType			{ get { return GetEnum<MarkerType>(m_MarkerType);			} set { m_MarkerType = value.ToString();	} }
		}

		[DataContract]
		public class Wreckage
		{
			[DataMember(Name = "ID")]					public int id							{ get; set; }
			[DataMember(Name = "TechID")]				public map_id techID					{ get; set; }
			[DataMember(Name = "IsVisible")]			public bool isVisible					{ get; set; }
			[DataMember(Name = "Position")]				public DataLocation position			{ get; set; }
		}

		[DataContract]
		public class WallTube
		{
			[DataMember(Name = "TypeID")]				private string m_TypeID					{ get; set; }
			[DataMember(Name = "Location")]				public DataLocation location			{ get; set; }

			public map_id typeID					{ get { return GetEnum<map_id>(m_TypeID);					} set { m_TypeID = value.ToString();		} }
		}

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}

		public GameData()
		{
			daylightMoves = true;
			musicPlayList = new MusicPlayList();
			musicPlayList.songIDs = new int[] { 0 };
			beacons = new List<Beacon>();
			markers = new List<Marker>();
			wreckage = new List<Wreckage>();
			wallTubes = new List<WallTube>();
		}
	}
}
