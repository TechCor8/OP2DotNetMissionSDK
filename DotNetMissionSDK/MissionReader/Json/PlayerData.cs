using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	[DataContract]
	public class PlayerData
	{
		[DataMember(Name = "ID")]					public int id							{ get; set; }
		[DataMember(Name = "IsEden")]				public bool isEden						{ get; set; }
		[DataMember(Name = "IsHuman")]				public bool isHuman						{ get; set; }
		[DataMember(Name = "BotType")]				public string botType					{ get; set; } = string.Empty;
		[DataMember(Name = "Color")]				public string color					{ get; set; } = string.Empty;
		[DataMember(Name = "Allies")]				public int[] allies						{ get; set; }
		[DataMember(Name = "Resources")]			public ResourceData resources			{ get; set; }
		[DataMember(Name = "DifficultyResources")]	public List<ResourceData> difficulties	{ get; set; }


		[DataContract]
		public class ResourceData
		{
			[DataMember(Name = "TechLevel")]		public int techLevel					{ get; set; }
			[DataMember(Name = "MoraleLevel")]		public string moraleLevel				{ get; set; } = string.Empty;
			[DataMember(Name = "FreeMorale")]		public bool freeMorale					{ get; set; }
			[DataMember(Name = "CenterView")]		public DataLocation centerView			{ get; set; }

			[DataMember(Name = "Kids")]				public int kids							{ get; set; }
			[DataMember(Name = "Workers")]			public int workers						{ get; set; }
			[DataMember(Name = "Scientists")]		public int scientists					{ get; set; }
			[DataMember(Name = "CommonOre")]		public int commonOre					{ get; set; }
			[DataMember(Name = "RareOre")]			public int rareOre						{ get; set; }
			[DataMember(Name = "Food")]				public int food							{ get; set; }
			[DataMember(Name = "SolarSatellites")]	public int solarSatellites				{ get; set; }

			[DataMember(Name = "CompletedResearch")]public int[] completedResearch			{ get; set; }

			[DataMember(Name = "Units")]			public List<UnitData> units				{ get; set; }
			[DataMember(Name = "WallTubes")]		public List<WallTubeData> wallTubes		{ get; set; }

			[DataMember(Name = "Triggers")]			public List<TriggerData> triggers		{ get; set; }

			public ResourceData()
			{
				moraleLevel = "Good";
				centerView = new DataLocation();
				kids = 10;
				workers = 14;
				scientists = 8;
				food = 1000;
				completedResearch = new int[0];
				units = new List<UnitData>();
				wallTubes = new List<WallTubeData>();
				triggers = new List<TriggerData>();
			}

			[OnDeserializing]
			private void OnDeserializing(StreamingContext context)
			{
				wallTubes = new List<WallTubeData>();
				triggers = new List<TriggerData>();
			}

			public ResourceData(ResourceData clone)
			{
				techLevel = clone.techLevel;
				moraleLevel = clone.moraleLevel;
				freeMorale = clone.freeMorale;
				centerView = clone.centerView;

				kids = clone.kids;
				workers = clone.workers;
				scientists = clone.scientists;
				commonOre = clone.commonOre;
				rareOre = clone.rareOre;
				food = clone.food;
				solarSatellites = clone.solarSatellites;

				completedResearch = new int[clone.completedResearch.Length];
				System.Array.Copy(clone.completedResearch, completedResearch, completedResearch.Length);

				units = new List<UnitData>(clone.units.Count);
				wallTubes = new List<WallTubeData>(clone.wallTubes.Count);
				triggers = new List<TriggerData>(clone.triggers.Count);

				for (int i=0; i < clone.units.Count; ++i)
					units.Add(new UnitData(clone.units[i]));
				foreach (WallTubeData wallTube in clone.wallTubes)
				wallTubes.Add(new WallTubeData(wallTube));
				foreach (TriggerData trigger in clone.triggers)
					triggers.Add(new TriggerData(trigger));
			}

			public void Concat(ResourceData dataToConcat)
			{
				techLevel = dataToConcat.techLevel;
				moraleLevel = dataToConcat.moraleLevel;
				freeMorale = dataToConcat.freeMorale;
				centerView = dataToConcat.centerView;

				kids += dataToConcat.kids;
				workers += dataToConcat.workers;
				scientists += dataToConcat.scientists;
				commonOre += dataToConcat.commonOre;
				rareOre += dataToConcat.rareOre;
				food += dataToConcat.food;
				solarSatellites += dataToConcat.solarSatellites;

				int[] newArr = new int[completedResearch.Length + dataToConcat.completedResearch.Length];
				System.Array.Copy(completedResearch, newArr, completedResearch.Length);
				System.Array.Copy(dataToConcat.completedResearch, 0, newArr, completedResearch.Length, dataToConcat.techLevel);
				completedResearch = newArr;

				units.Capacity = units.Count + dataToConcat.units.Count;
				wallTubes.Capacity = wallTubes.Count + dataToConcat.wallTubes.Count;
				triggers.Capacity = triggers.Count + dataToConcat.triggers.Count;

				foreach (UnitData unit in dataToConcat.units)
					units.Add(new UnitData(unit));
				foreach (WallTubeData wallTube in dataToConcat.wallTubes)
				wallTubes.Add(new WallTubeData(wallTube));
				foreach (TriggerData trigger in dataToConcat.triggers)
					triggers.Add(new TriggerData(trigger));
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
			this.id = id;
			isEden = id % 2 == 0;
			isHuman = true;
			botType = id == 0 ? "None" : "Balanced";
			color = GetPlayerColor(id);
			allies = new int[0];

			resources = new ResourceData();
			difficulties = new List<ResourceData>();
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
			id = clone.id;
			isEden = clone.isEden;
			isHuman = clone.isHuman;
			botType = clone.botType;
			color = clone.color;
			allies = new int[clone.allies.Length];
			System.Array.Copy(clone.allies, allies, allies.Length);

			resources = new ResourceData(clone.resources);
			difficulties = new List<ResourceData>(clone.difficulties.Count);

			for (int i=0; i < clone.difficulties.Count; ++i)
				difficulties.Add(new ResourceData(clone.difficulties[i]));
		}

		public void Concat(PlayerData dataToConcat)
		{
			isEden = dataToConcat.isEden;
			isHuman = dataToConcat.isHuman;
			botType = dataToConcat.botType;
			color = dataToConcat.color;
			allies = new int[dataToConcat.allies.Length];
			System.Array.Copy(dataToConcat.allies, allies, allies.Length);

			resources = ResourceData.Concat(resources, dataToConcat.resources);

			for (int i=0; i < dataToConcat.difficulties.Count; ++i)
				difficulties[i].Concat(dataToConcat.difficulties[i]);
		}
	}
}
