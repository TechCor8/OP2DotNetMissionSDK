using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class GameData
	{
		// [Data Accessors]
		[DataMember(Name = "DaylightEverywhere")]		public bool daylightEverywhere			{ get; private set; }
		[DataMember(Name = "DaylightMoves")]			public bool daylightMoves				{ get; private set; }
		[DataMember(Name = "InitialLightLevel")]		public int initialLightLevel			{ get; private set; }

		[DataMember(Name = "MusicPlayList")]			public MusicPlayList musicPlayList		{ get; private set; }
		[DataMember(Name = "Beacons")]					public Beacon[] beacons					{ get; private set; }
		[DataMember(Name = "Markers")]					public Marker[] markers					{ get; private set; }
		[DataMember(Name = "Wreckage")]					public Wreckage[] wreckage				{ get; private set; }
		[DataMember(Name = "WallTubes")]				public WallTube[] wallTubes				{ get; private set; }


		// [Data Classes]
		[DataContract]
		public class MusicPlayList
		{
			[DataMember(Name = "RepeatStartIndex")]		public int repeatStartIndex				{ get; private set; }
			[DataMember(Name = "SongIDs")]				public int[] songIDs					{ get; private set; }
		}

		[DataContract]
		public class Beacon
		{
			[DataMember(Name = "MapID")]				private string m_MapID					{ get; set; }
			[DataMember(Name = "CommonRareType")]		private string m_CommonRareType			{ get; set; }
			[DataMember(Name = "BarYield")]				private string m_BarYield				{ get; set; }
			[DataMember(Name = "BarVariant")]			private string m_BarVariant				{ get; set; }
			[DataMember(Name = "SpawnRect")]			public DataRect spawnRect				{ get; private set; }

			public map_id mapID						{ get { return GetEnum<map_id>(m_MapID);					} }
			public BeaconTypes commonRareType		{ get { return GetEnum<BeaconTypes>(m_CommonRareType);		} }
			public Yield barYield					{ get { return GetEnum<Yield>(m_BarYield);					} }
			public Variant barVariant				{ get { return GetEnum<Variant>(m_BarVariant);				} }
		}

		[DataContract]
		public class Marker
		{
			[DataMember(Name = "MarkerType")]			private string m_MarkerType				{ get; set; }
			[DataMember(Name = "SpawnRect")]			public DataRect spawnRect				{ get; private set; }

			public MarkerTypes markerType			{ get { return GetEnum<MarkerTypes>(m_MarkerType);			} }
		}

		[DataContract]
		public class Wreckage
		{
			[DataMember(Name = "TechID")]				public map_id techID					{ get; private set; }
			[DataMember(Name = "IsVisible")]			public bool isVisible					{ get; private set; }
			[DataMember(Name = "SpawnRect")]			public DataRect spawnRect				{ get; private set; }
		}

		[DataContract]
		public class WallTube
		{
			[DataMember(Name = "TypeID")]				private string m_TypeID					{ get; set; }
			[DataMember(Name = "Location")]				public DataLocation location			{ get; private set; }

			public map_id typeID					{ get { return GetEnum<map_id>(m_TypeID);					} }
		}

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
