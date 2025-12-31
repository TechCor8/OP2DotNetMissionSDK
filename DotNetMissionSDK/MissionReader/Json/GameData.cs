using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public class GameData
	{
		// [Data Accessors]
		[DataMember(Name = "DaylightEverywhere")]		public bool DaylightEverywhere			{ get; set; }
		[DataMember(Name = "DaylightMoves")]			public bool DaylightMoves				{ get; set; }
		[DataMember(Name = "InitialLightLevel")]		public int InitialLightLevel			{ get; set; }

		[DataMember(Name = "MusicPlayList")]			public MusicPlayList MusicPlayList		{ get; set; }
		[DataMember(Name = "Beacons")]					public List<Beacon> Beacons				{ get; set; }
		[DataMember(Name = "Markers")]					public List<Marker> Markers				{ get; set; }
		[DataMember(Name = "Wreckage")]					public List<Wreckage> Wreckage			{ get; set; }

		[DataMember(Name = "Triggers")]					public List<TriggerData> Triggers		{ get; set; }


		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}

		public GameData()
		{
			DaylightMoves = true;
			MusicPlayList = new MusicPlayList();
			MusicPlayList.SongIds = new int[] { 0 };
			Beacons = new List<Beacon>();
			Markers = new List<Marker>();
			Wreckage = new List<Wreckage>();
			Triggers = new List<TriggerData>();
		}

		[OnDeserializing]
		private void OnDeserializing(StreamingContext context)
		{
			Triggers = new List<TriggerData>();
		}

		public GameData(GameData clone)
		{
			DaylightEverywhere = clone.DaylightEverywhere;
			DaylightMoves = clone.DaylightMoves;
			InitialLightLevel = clone.InitialLightLevel;

			MusicPlayList = new MusicPlayList(clone.MusicPlayList);
			Beacons = new List<Beacon>(clone.Beacons.Count);
			Markers = new List<Marker>(clone.Markers.Count);
			Wreckage = new List<Wreckage>(clone.Wreckage.Count);
			Triggers = new List<TriggerData>(clone.Triggers.Count);

			foreach (Beacon beacon in clone.Beacons)
				Beacons.Add(new Beacon(beacon));
			foreach (Marker marker in clone.Markers)
				Markers.Add(new Marker(marker));
			foreach (Wreckage wreck in clone.Wreckage)
				Wreckage.Add(new Wreckage(wreck));
			foreach (TriggerData trigger in clone.Triggers)
				Triggers.Add(new TriggerData(trigger));
		}

		public void Concat(GameData dataToConcat)
		{
			DaylightEverywhere = dataToConcat.DaylightEverywhere;
			DaylightMoves = dataToConcat.DaylightMoves;
			InitialLightLevel = dataToConcat.InitialLightLevel;

			MusicPlayList = new MusicPlayList(dataToConcat.MusicPlayList);

			Beacons.Capacity = Beacons.Count + dataToConcat.Beacons.Count;
			Markers.Capacity = Markers.Count + dataToConcat.Markers.Count;
			Wreckage.Capacity = Wreckage.Count + dataToConcat.Wreckage.Count;
			Triggers.Capacity = Triggers.Count + dataToConcat.Triggers.Count;

			foreach (Beacon beacon in dataToConcat.Beacons)
				Beacons.Add(new Beacon(beacon));
			foreach (Marker marker in dataToConcat.Markers)
				Markers.Add(new Marker(marker));
			foreach (Wreckage wreck in dataToConcat.Wreckage)
				Wreckage.Add(new Wreckage(wreck));
			foreach (TriggerData trigger in dataToConcat.Triggers)
				Triggers.Add(new TriggerData(trigger));
		}

		public static GameData Concat(GameData a, GameData b)
		{
			GameData result = new GameData(a);
			result.Concat(b);
			return result;
		}
	}

	// [Data Classes]
	[DataContract]
	public class MusicPlayList
	{
		[DataMember(Name = "RepeatStartIndex")]		public int RepeatStartIndex				{ get; set; }
		[DataMember(Name = "SongIDs")]				public int[] SongIds					{ get; set; }

		public MusicPlayList() { }
		public MusicPlayList(MusicPlayList clone)
		{
			RepeatStartIndex = clone.RepeatStartIndex;
			SongIds = new int[clone.SongIds.Length];
			System.Array.Copy(clone.SongIds, SongIds, SongIds.Length);
		}
	}

	[DataContract]
	public class Beacon
	{
		[DataMember(Name = "ID")]					public int Id							{ get; set; }
		[DataMember(Name = "MapID")]				public string MapId						{ get; set; }
		[DataMember(Name = "OreType")]				public string OreType					{ get; set; }
		[DataMember(Name = "BarYield")]				public string BarYield					{ get; set; }
		[DataMember(Name = "BarVariant")]			public string BarVariant				{ get; set; }
		[DataMember(Name = "Position")]				public DataLocation Position			{ get; set; }

			
		public Beacon() { }
		public Beacon(Beacon clone)
		{
			Id = clone.Id;
			MapId = clone.MapId;
			OreType = clone.OreType;
			BarYield = clone.BarYield;
			BarVariant = clone.BarVariant;
			Position = clone.Position;
		}
	}

	[DataContract]
	public class Marker
	{
		[DataMember(Name = "ID")]					public int Id							{ get; set; }
		[DataMember(Name = "MarkerType")]			public string MarkerType				{ get; set; }
		[DataMember(Name = "Position")]				public DataLocation Position			{ get; set; }


		public Marker() { }
		public Marker(Marker clone)
		{
			Id = clone.Id;
			MarkerType = clone.MarkerType;
			Position = clone.Position;
		}
	}

	[DataContract]
	public class Wreckage
	{
		[DataMember(Name = "ID")]					public int Id							{ get; set; }
		[DataMember(Name = "TechID")]				public string TechId					{ get; set; }
		[DataMember(Name = "IsVisible")]			public bool IsVisible					{ get; set; }
		[DataMember(Name = "Position")]				public DataLocation Position			{ get; set; }

		public Wreckage() { }
		public Wreckage(Wreckage clone)
		{
			Id = clone.Id;
			TechId = clone.TechId;
			IsVisible = clone.IsVisible;
			Position = clone.Position;
		}
	}
}
