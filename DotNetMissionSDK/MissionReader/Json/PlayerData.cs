﻿using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class PlayerData
	{
		[DataMember(Name = "ID")]				public int id						{ get; private set; }
		[DataMember(Name = "TechLevel")]		public int techLevel				{ get; private set; }
		[DataMember(Name = "MoraleLevel")]		private string m_MoraleLevel		{ get; set; }
		[DataMember(Name = "FreeMorale")]		public bool freeMorale				{ get; private set; }
		[DataMember(Name = "IsEden")]			public bool isEden					{ get; private set; }
		[DataMember(Name = "IsHuman")]			public bool isHuman					{ get; private set; }
		[DataMember(Name = "Color")]			private string m_Color				{ get; set; }
		[DataMember(Name = "Allies")]			public int[] allies					{ get; private set; }
		[DataMember(Name = "CenterView")]		public DataLocation centerView		{ get; private set; }

		[DataMember(Name = "Kids")]				public int kids						{ get; private set; }
		[DataMember(Name = "Workers")]			public int workers					{ get; private set; }
		[DataMember(Name = "Scientists")]		public int scientists				{ get; private set; }
		[DataMember(Name = "CommonOre")]		public int commonOre				{ get; private set; }
		[DataMember(Name = "RareOre")]			public int rareOre					{ get; private set; }
		[DataMember(Name = "Food")]				public int food						{ get; private set; }
		[DataMember(Name = "SolarSatellites")]	public int solarSatellites			{ get; private set; }

		[DataMember(Name = "CompletedResearch")]public int[] completedResearch		{ get; private set; }

		[DataMember(Name = "Units")]			public UnitData[] units				{ get; private set; }

		public MoraleLevel moraleLevel			{ get { return GetEnum<MoraleLevel>(m_MoraleLevel);	} }
		public PlayerColor color				{ get { return GetEnum<PlayerColor>(m_Color);		} }
		

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
