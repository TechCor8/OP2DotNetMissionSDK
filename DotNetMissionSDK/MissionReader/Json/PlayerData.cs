using DotNetMissionSDK.AI;
using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class PlayerData
	{
		[DataMember(Name = "ID")]				public int id						{ get; set; }
		[DataMember(Name = "TechLevel")]		public int techLevel				{ get; set; }
		[DataMember(Name = "MoraleLevel")]		private string m_MoraleLevel		{ get; set; }
		[DataMember(Name = "FreeMorale")]		public bool freeMorale				{ get; set; }
		[DataMember(Name = "IsEden")]			public bool isEden					{ get; set; }
		[DataMember(Name = "IsHuman")]			public bool isHuman					{ get; set; }
		[DataMember(Name = "BotType")]			private string m_BotType			{ get; set; }
		[DataMember(Name = "Color")]			private string m_Color				{ get; set; }
		[DataMember(Name = "Allies")]			public int[] allies					{ get; set; }
		[DataMember(Name = "CenterView")]		public DataLocation centerView		{ get; set; }

		[DataMember(Name = "Kids")]				public int kids						{ get; set; }
		[DataMember(Name = "Workers")]			public int workers					{ get; set; }
		[DataMember(Name = "Scientists")]		public int scientists				{ get; set; }
		[DataMember(Name = "CommonOre")]		public int commonOre				{ get; set; }
		[DataMember(Name = "RareOre")]			public int rareOre					{ get; set; }
		[DataMember(Name = "Food")]				public int food						{ get; set; }
		[DataMember(Name = "SolarSatellites")]	public int solarSatellites			{ get; set; }

		[DataMember(Name = "CompletedResearch")]public int[] completedResearch		{ get; set; }

		[DataMember(Name = "Units")]			public UnitData[] units				{ get; set; }

		public MoraleLevel moraleLevel			{ get { return GetEnum<MoraleLevel>(m_MoraleLevel);	} set { m_MoraleLevel = value.ToString();	} }
		public BotType botType					{ get { return GetEnum<BotType>(m_BotType);			} set { m_BotType = value.ToString();		} }
		public PlayerColor color				{ get { return GetEnum<PlayerColor>(m_Color);		} set { m_Color = value.ToString();			} }
		

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}

		public PlayerData(int id)
		{
			this.id = id;
			moraleLevel = MoraleLevel.Good;
			isEden = id % 2 == 0;
			isHuman = true;
			botType = id == 0 ? BotType.None : BotType.Balanced;
			color = (PlayerColor)id;
			allies = new int[0];
			centerView = new DataLocation(new LOCATION(0,0));
			kids = 10;
			workers = 14;
			scientists = 8;
			food = 1000;
			completedResearch = new int[0];
			units = new UnitData[0];
		}
	}
}
