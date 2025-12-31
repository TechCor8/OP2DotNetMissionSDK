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
		[DataMember(Name = "Name")]					public string Name							{ get; set; } = string.Empty;

		[DataMember(Name = "TethysGame")]			public GameData TethysGame					{ get; set; }
		[DataMember(Name = "TethysDifficulties")]	public List<GameData> TethysDifficulties	{ get; set; }
		[DataMember(Name = "Players")]				public List<PlayerData> Players				{ get; set; }
		[DataMember(Name = "AutoLayouts")]			public List<AutoLayout> Layouts				{ get; set; }


		public MissionVariant()
		{
			TethysGame = new GameData();
			TethysDifficulties = new List<GameData>();
			Players = new List<PlayerData>(2);
			Layouts = new List<AutoLayout>();

			for (int i=0; i < 2; ++i)
				Players.Add(new PlayerData(i));
		}

		public MissionVariant(MissionVariant clone)
		{
			Name = clone.Name;

			TethysGame = new GameData(clone.TethysGame);
			TethysDifficulties = new List<GameData>(clone.TethysDifficulties.Count);
			Players = new List<PlayerData>(clone.Players.Count);
			Layouts = new List<AutoLayout>(clone.Layouts.Count);

			for (int i=0; i < clone.TethysDifficulties.Count; ++i)
				TethysDifficulties.Add(new GameData(clone.TethysDifficulties[i]));

			for (int i=0; i < clone.Players.Count; ++i)
				Players.Add(new PlayerData(clone.Players[i]));

			for (int i=0; i < clone.Layouts.Count; ++i)
				Layouts.Add(new AutoLayout(clone.Layouts[i]));
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
			result.TethysGame.Concat(b.TethysGame);

			for (int i=0; i < b.TethysDifficulties.Count; ++i)
				result.TethysDifficulties[i].Concat(b.TethysDifficulties[i]);

			for (int i=0; i < b.Layouts.Count; ++i)
				result.Layouts.Add(new AutoLayout(b.Layouts[i]));

			for (int i=0; i < result.Players.Count; ++i)
				result.Players[i].Concat(b.Players[i]);

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
