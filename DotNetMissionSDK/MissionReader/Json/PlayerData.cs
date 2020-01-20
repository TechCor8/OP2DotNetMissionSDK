using DotNetMissionSDK.AI;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class PlayerData
	{
		[DataMember(Name = "ID")]					public int id							{ get; set; }
		[DataMember(Name = "IsEden")]				public bool isEden						{ get; set; }
		[DataMember(Name = "IsHuman")]				public bool isHuman						{ get; set; }
		[DataMember(Name = "BotType")]				private string m_BotType				{ get; set; }
		[DataMember(Name = "Color")]				private string m_Color					{ get; set; }
		[DataMember(Name = "Allies")]				public int[] allies						{ get; set; }
		[DataMember(Name = "DifficultyResources")]	public List<ResourceData> difficulties	{ get; set; }

		public BotType botType					{ get { return GetEnum<BotType>(m_BotType);			} set { m_BotType = value.ToString();		} }
		public PlayerColor color				{ get { return GetEnum<PlayerColor>(m_Color);		} set { m_Color = value.ToString();			} }


		[DataContract]
		public class ResourceData
		{
			[DataMember(Name = "TechLevel")]		public int techLevel					{ get; set; }
			[DataMember(Name = "MoraleLevel")]		private string m_MoraleLevel			{ get; set; }
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

			public MoraleLevel moraleLevel			{ get { return GetEnum<MoraleLevel>(m_MoraleLevel);	} set { m_MoraleLevel = value.ToString();	} }

			public ResourceData()
			{
				moraleLevel = MoraleLevel.Good;
				centerView = new DataLocation(new LOCATION(0,0));
				kids = 10;
				workers = 14;
				scientists = 8;
				food = 1000;
				completedResearch = new int[0];
				units = new List<UnitData>();
			}

			public ResourceData(ResourceData clone)
			{
				techLevel = clone.techLevel;
				m_MoraleLevel = clone.m_MoraleLevel;
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
				for (int i=0; i < clone.units.Count; ++i)
					units.Add(new UnitData(clone.units[i]));
			}

			public void Concat(ResourceData dataToConcat)
			{
				techLevel = dataToConcat.techLevel;
				m_MoraleLevel = dataToConcat.m_MoraleLevel;
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

				foreach (UnitData unit in dataToConcat.units)
					units.Add(new UnitData(unit));
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
			botType = id == 0 ? BotType.None : BotType.Balanced;
			color = (PlayerColor)id;
			allies = new int[0];

			difficulties = new List<ResourceData>();
			difficulties.Add(new ResourceData());
		}

		public PlayerData(PlayerData clone)
		{
			id = clone.id;
			isEden = clone.isEden;
			isHuman = clone.isHuman;
			m_BotType = clone.m_BotType;
			m_Color = clone.m_Color;
			allies = new int[clone.allies.Length];
			System.Array.Copy(clone.allies, allies, allies.Length);
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

			for (int i=0; i < dataToConcat.difficulties.Count; ++i)
				difficulties[i].Concat(dataToConcat.difficulties[i]);
		}
	}
}
