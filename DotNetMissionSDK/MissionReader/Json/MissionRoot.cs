﻿using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class MissionRoot
	{
		[DataMember(Name = "LevelDetails")]		public LevelDetails levelDetails	{ get; private set; }
		[DataMember(Name = "TethysGame")]		public GameData tethysGame			{ get; private set; }
		[DataMember(Name = "Players")]			public PlayerData[] players			{ get; private set; }
		[DataMember(Name = "AutoLayouts")]		public AutoLayout[] layouts			{ get; private set; }
		[DataMember(Name = "Disasters")]		public DisasterData[] disasters		{ get; private set; }
		[DataMember(Name = "Triggers")]			public TriggerData[] triggers		{ get; private set; }
	}
}
