using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetMissionReader
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
			[DataMember(Name = "MapID")]				public string mapID						{ get; set; }
			[DataMember(Name = "OreType")]				public string oreType					{ get; set; }
			[DataMember(Name = "BarYield")]				public string barYield					{ get; set; }
			[DataMember(Name = "BarVariant")]			public string barVariant				{ get; set; }
			[DataMember(Name = "Position")]				public DataLocation position			{ get; set; }

			
			public Beacon() { }
			public Beacon(Beacon clone)
			{
				id = clone.id;
				mapID = clone.mapID;
				oreType = clone.oreType;
				barYield = clone.barYield;
				barVariant = clone.barVariant;
				position = clone.position;
			}
		}

		[DataContract]
		public class Marker
		{
			[DataMember(Name = "ID")]					public int id							{ get; set; }
			[DataMember(Name = "MarkerType")]			public string markerType				{ get; set; }
			[DataMember(Name = "Position")]				public DataLocation position			{ get; set; }


			public Marker() { }
			public Marker(Marker clone)
			{
				id = clone.id;
				markerType = clone.markerType;
				position = clone.position;
			}
		}

		[DataContract]
		public class Wreckage
		{
			[DataMember(Name = "ID")]					public int id							{ get; set; }
			[DataMember(Name = "TechID")]				public string techID					{ get; set; }
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
