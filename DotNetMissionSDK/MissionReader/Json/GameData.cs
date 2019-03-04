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
			[DataMember(Name = "MapID")]				public map_id mapID						{ get; private set; }
			[DataMember(Name = "CommonRareType")]		public int commonRareType				{ get; private set; }
			[DataMember(Name = "BarYield")]				public int barYield						{ get; private set; }
			[DataMember(Name = "BarVariant")]			public int barVariant					{ get; private set; }
			[DataMember(Name = "SpawnRect")]			public DataRect spawnRect				{ get; private set; }
		}

		[DataContract]
		public class Marker
		{
			[DataMember(Name = "MarkerType")]			public int markerType					{ get; private set; }
			[DataMember(Name = "SpawnRect")]			public DataRect spawnRect				{ get; private set; }
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
			[DataMember(Name = "TypeID")]				public map_id typeID					{ get; private set; }
			[DataMember(Name = "Location")]				public DataLocation location			{ get; private set; }
		}
	}
}
