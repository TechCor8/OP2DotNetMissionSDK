using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public class PlayerData
	{
		[DataMember(Name = "ID")]					public int Id							{ get; set; }
		[DataMember(Name = "IsEden")]				public bool IsEden						{ get; set; }
		[DataMember(Name = "IsHuman")]				public bool IsHuman						{ get; set; }
		[DataMember(Name = "BotType")]				public string BotType					{ get; set; } = string.Empty;
		[DataMember(Name = "Color")]				public string Color						{ get; set; } = string.Empty;
		[DataMember(Name = "Allies")]				public int[] Allies						{ get; set; }
		[DataMember(Name = "Resources")]			public ResourceData Resources			{ get; set; }
		[DataMember(Name = "DifficultyResources")]	public List<ResourceData> Difficulties	{ get; set; }


		[DataContract]
		public class ResourceData
		{
			[DataMember(Name = "TechLevel")]		public int TechLevel					{ get; set; }
			[DataMember(Name = "MoraleLevel")]		public string MoraleLevel				{ get; set; } = string.Empty;
			[DataMember(Name = "FreeMorale")]		public bool FreeMorale					{ get; set; }
			[DataMember(Name = "CenterView")]		public DataLocation CenterView			{ get; set; }

			[DataMember(Name = "Kids")]				public int Kids							{ get; set; }
			[DataMember(Name = "Workers")]			public int Workers						{ get; set; }
			[DataMember(Name = "Scientists")]		public int Scientists					{ get; set; }
			[DataMember(Name = "CommonOre")]		public int CommonOre					{ get; set; }
			[DataMember(Name = "RareOre")]			public int RareOre						{ get; set; }
			[DataMember(Name = "Food")]				public int Food							{ get; set; }
			[DataMember(Name = "SolarSatellites")]	public int SolarSatellites				{ get; set; }

			[DataMember(Name = "CompletedResearch")]public int[] CompletedResearch			{ get; set; }

			[DataMember(Name = "Units")]			public List<UnitData> Units				{ get; set; }
			[DataMember(Name = "WallTubes")]		public List<WallTubeData> WallTubes		{ get; set; }

			[DataMember(Name = "Triggers")]			public List<TriggerData> Triggers		{ get; set; }

			public ResourceData()
			{
				MoraleLevel = "Good";
				CenterView = new DataLocation();
				Kids = 10;
				Workers = 14;
				Scientists = 8;
				Food = 1000;
				CompletedResearch = new int[0];
				Units = new List<UnitData>();
				WallTubes = new List<WallTubeData>();
				Triggers = new List<TriggerData>();
			}

			[OnDeserializing]
			private void OnDeserializing(StreamingContext context)
			{
				WallTubes = new List<WallTubeData>();
				Triggers = new List<TriggerData>();
			}

			public ResourceData(ResourceData clone)
			{
				TechLevel = clone.TechLevel;
				MoraleLevel = clone.MoraleLevel;
				FreeMorale = clone.FreeMorale;
				CenterView = clone.CenterView;

				Kids = clone.Kids;
				Workers = clone.Workers;
				Scientists = clone.Scientists;
				CommonOre = clone.CommonOre;
				RareOre = clone.RareOre;
				Food = clone.Food;
				SolarSatellites = clone.SolarSatellites;

				CompletedResearch = new int[clone.CompletedResearch.Length];
				System.Array.Copy(clone.CompletedResearch, CompletedResearch, CompletedResearch.Length);

				Units = new List<UnitData>(clone.Units.Count);
				WallTubes = new List<WallTubeData>(clone.WallTubes.Count);
				Triggers = new List<TriggerData>(clone.Triggers.Count);

				for (int i=0; i < clone.Units.Count; ++i)
					Units.Add(new UnitData(clone.Units[i]));
				foreach (WallTubeData wallTube in clone.WallTubes)
				WallTubes.Add(new WallTubeData(wallTube));
				foreach (TriggerData trigger in clone.Triggers)
					Triggers.Add(new TriggerData(trigger));
			}

			public void Concat(ResourceData dataToConcat)
			{
				TechLevel = dataToConcat.TechLevel;
				MoraleLevel = dataToConcat.MoraleLevel;
				FreeMorale = dataToConcat.FreeMorale;
				CenterView = dataToConcat.CenterView;

				Kids += dataToConcat.Kids;
				Workers += dataToConcat.Workers;
				Scientists += dataToConcat.Scientists;
				CommonOre += dataToConcat.CommonOre;
				RareOre += dataToConcat.RareOre;
				Food += dataToConcat.Food;
				SolarSatellites += dataToConcat.SolarSatellites;

				int[] newArr = new int[CompletedResearch.Length + dataToConcat.CompletedResearch.Length];
				System.Array.Copy(CompletedResearch, newArr, CompletedResearch.Length);
				System.Array.Copy(dataToConcat.CompletedResearch, 0, newArr, CompletedResearch.Length, dataToConcat.TechLevel);
				CompletedResearch = newArr;

				Units.Capacity = Units.Count + dataToConcat.Units.Count;
				WallTubes.Capacity = WallTubes.Count + dataToConcat.WallTubes.Count;
				Triggers.Capacity = Triggers.Count + dataToConcat.Triggers.Count;

				foreach (UnitData unit in dataToConcat.Units)
					Units.Add(new UnitData(unit));
				foreach (WallTubeData wallTube in dataToConcat.WallTubes)
				WallTubes.Add(new WallTubeData(wallTube));
				foreach (TriggerData trigger in dataToConcat.Triggers)
					Triggers.Add(new TriggerData(trigger));
			}

			public static ResourceData Concat(ResourceData a, ResourceData b)
			{
				ResourceData result = new ResourceData(a);
				result.Concat(b);
				return result;
			}
		}

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}

		public PlayerData(int id)
		{
			this.Id = id;
			IsEden = id % 2 == 0;
			IsHuman = true;
			BotType = id == 0 ? "None" : "Balanced";
			Color = GetPlayerColor(id);
			Allies = new int[0];

			Resources = new ResourceData();
			Difficulties = new List<ResourceData>();
		}

		private string GetPlayerColor(int id)
		{
			switch (id)
			{
				case 0: return "Blue";
				case 1: return "Red";
				case 2: return "Green";
				case 3: return "Yellow";
				case 4: return "Cyan";
				case 5: return "Magenta";
				case 6: return "Black";
				default: return "Clear";
			}
		}

		public PlayerData(PlayerData clone)
		{
			Id = clone.Id;
			IsEden = clone.IsEden;
			IsHuman = clone.IsHuman;
			BotType = clone.BotType;
			Color = clone.Color;
			Allies = new int[clone.Allies.Length];
			System.Array.Copy(clone.Allies, Allies, Allies.Length);

			Resources = new ResourceData(clone.Resources);
			Difficulties = new List<ResourceData>(clone.Difficulties.Count);

			for (int i=0; i < clone.Difficulties.Count; ++i)
				Difficulties.Add(new ResourceData(clone.Difficulties[i]));
		}

		public void Concat(PlayerData dataToConcat)
		{
			IsEden = dataToConcat.IsEden;
			IsHuman = dataToConcat.IsHuman;
			BotType = dataToConcat.BotType;
			Color = dataToConcat.Color;
			Allies = new int[dataToConcat.Allies.Length];
			System.Array.Copy(dataToConcat.Allies, Allies, Allies.Length);

			Resources = ResourceData.Concat(Resources, dataToConcat.Resources);

			for (int i=0; i < dataToConcat.Difficulties.Count; ++i)
				Difficulties[i].Concat(dataToConcat.Difficulties[i]);
		}
	}
}
