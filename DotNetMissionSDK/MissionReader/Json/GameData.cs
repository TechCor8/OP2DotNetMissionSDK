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

		[DataMember(Name = "Triggers")]					public List<TriggerData> triggers		{ get; set; }


		// [Data Classes]
		[DataContract]
		public class MusicPlayList
		{
			[DataMember(Name = "RepeatStartIndex")]		public int repeatStartIndex				{ get; set; }
			[DataMember(Name = "SongIDs")]				public int[] songIDs					{ get; set; }

			public MusicPlayList() { }
			public MusicPlayList(MusicPlayList clone)
			{
				repeatStartIndex = clone.repeatStartIndex;
				songIDs = new int[clone.songIDs.Length];
				System.Array.Copy(clone.songIDs, songIDs, songIDs.Length);
			}
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

			public Beacon() { }
			public Beacon(Beacon clone)
			{
				id = clone.id;
				m_MapID = clone.m_MapID;
				m_OreType = clone.m_OreType;
				m_BarYield = clone.m_BarYield;
				m_BarVariant = clone.m_BarVariant;
				position = clone.position;
			}
		}

		[DataContract]
		public class Marker
		{
			[DataMember(Name = "ID")]					public int id							{ get; set; }
			[DataMember(Name = "MarkerType")]			private string m_MarkerType				{ get; set; }
			[DataMember(Name = "Position")]				public DataLocation position			{ get; set; }

			public MarkerType markerType			{ get { return GetEnum<MarkerType>(m_MarkerType);			} set { m_MarkerType = value.ToString();	} }

			public Marker() { }
			public Marker(Marker clone)
			{
				id = clone.id;
				m_MarkerType = clone.m_MarkerType;
				position = clone.position;
			}
		}

		[DataContract]
		public class Wreckage
		{
			[DataMember(Name = "ID")]					public int id							{ get; set; }
			[DataMember(Name = "TechID")]				public map_id techID					{ get; set; }
			[DataMember(Name = "IsVisible")]			public bool isVisible					{ get; set; }
			[DataMember(Name = "Position")]				public DataLocation position			{ get; set; }

			public Wreckage() { }
			public Wreckage(Wreckage clone)
			{
				id = clone.id;
				techID = clone.techID;
				isVisible = clone.isVisible;
				position = clone.position;
			}
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
			triggers = new List<TriggerData>();
		}

		[OnDeserializing]
		private void OnDeserializing(StreamingContext context)
		{
			triggers = new List<TriggerData>();
		}

		public GameData(GameData clone)
		{
			daylightEverywhere = clone.daylightEverywhere;
			daylightMoves = clone.daylightMoves;
			initialLightLevel = clone.initialLightLevel;

			musicPlayList = new MusicPlayList(clone.musicPlayList);
			beacons = new List<Beacon>(clone.beacons.Count);
			markers = new List<Marker>(clone.markers.Count);
			wreckage = new List<Wreckage>(clone.wreckage.Count);
			triggers = new List<TriggerData>(clone.triggers.Count);

			foreach (Beacon beacon in clone.beacons)
				beacons.Add(new Beacon(beacon));
			foreach (Marker marker in clone.markers)
				markers.Add(new Marker(marker));
			foreach (Wreckage wreck in clone.wreckage)
				wreckage.Add(new Wreckage(wreck));
			foreach (TriggerData trigger in clone.triggers)
				triggers.Add(new TriggerData(trigger));
		}

		public void Concat(GameData dataToConcat)
		{
			daylightEverywhere = dataToConcat.daylightEverywhere;
			daylightMoves = dataToConcat.daylightMoves;
			initialLightLevel = dataToConcat.initialLightLevel;

			musicPlayList = new MusicPlayList(dataToConcat.musicPlayList);

			beacons.Capacity = beacons.Count + dataToConcat.beacons.Count;
			markers.Capacity = markers.Count + dataToConcat.markers.Count;
			wreckage.Capacity = wreckage.Count + dataToConcat.wreckage.Count;
			triggers.Capacity = triggers.Count + dataToConcat.triggers.Count;

			foreach (Beacon beacon in dataToConcat.beacons)
				beacons.Add(new Beacon(beacon));
			foreach (Marker marker in dataToConcat.markers)
				markers.Add(new Marker(marker));
			foreach (Wreckage wreck in dataToConcat.wreckage)
				wreckage.Add(new Wreckage(wreck));
			foreach (TriggerData trigger in dataToConcat.triggers)
				triggers.Add(new TriggerData(trigger));
		}

		public static GameData Concat(GameData a, GameData b)
		{
			GameData result = new GameData(a);
			result.Concat(b);
			return result;
		}
	}
}
