using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetMissionReader
{
	/// <summary>
	/// Represents a mission variant that is randomly selected at start (unless it is a difficulty variant).
	/// </summary>
	[DataContract]
	public class MissionVariant
	{
		// The internal name of this variant to aid the mission designer.
		[DataMember(Name = "Name")]					public string name							{ get; set; } = string.Empty;

		[DataMember(Name = "TethysGame")]			public GameData tethysGame					{ get; set; }
		[DataMember(Name = "TethysDifficulties")]	public List<GameData> tethysDifficulties	{ get; set; }
		[DataMember(Name = "Players")]				public List<PlayerData> players				{ get; set; }
		[DataMember(Name = "AutoLayouts")]			public List<AutoLayout> layouts				{ get; set; }


		public MissionVariant()
		{
			tethysGame = new GameData();
			tethysDifficulties = new List<GameData>();
			players = new List<PlayerData>(2);
			layouts = new List<AutoLayout>();

			for (int i=0; i < 2; ++i)
				players.Add(new PlayerData(i));
		}

		public MissionVariant(MissionVariant clone)
		{
			name = clone.name;

			tethysGame = new GameData(clone.tethysGame);
			tethysDifficulties = new List<GameData>(clone.tethysDifficulties.Count);
			players = new List<PlayerData>(clone.players.Count);
			layouts = new List<AutoLayout>(clone.layouts.Count);

			for (int i=0; i < clone.tethysDifficulties.Count; ++i)
				tethysDifficulties.Add(new GameData(clone.tethysDifficulties[i]));

			for (int i=0; i < clone.players.Count; ++i)
				players.Add(new PlayerData(clone.players[i]));

			for (int i=0; i < clone.layouts.Count; ++i)
				layouts.Add(new AutoLayout(clone.layouts[i]));
		}

		/// <summary>
		/// Combines two variants additively.
		/// Where values cannot be added, second variant overrides.
		/// </summary>
		/// <param name="a">Source variant.</param>
		/// <param name="b">Variant to add. Non-countable properties are overridden with this variant's values.</param>
		/// <returns>Combined variant.</returns>
		public static MissionVariant Concat(MissionVariant a, MissionVariant b)
		{
			MissionVariant result = new MissionVariant(a);
			result.tethysGame.Concat(b.tethysGame);

			for (int i=0; i < b.tethysDifficulties.Count; ++i)
				result.tethysDifficulties[i].Concat(b.tethysDifficulties[i]);

			for (int i=0; i < b.layouts.Count; ++i)
				result.layouts.Add(new AutoLayout(b.layouts[i]));

			for (int i=0; i < result.players.Count; ++i)
				result.players[i].Concat(b.players[i]);

			return result;
		}

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}
	}
}
