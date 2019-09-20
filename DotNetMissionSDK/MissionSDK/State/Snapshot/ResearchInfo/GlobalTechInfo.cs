using DotNetMissionSDK.HFL;

namespace DotNetMissionSDK.State.Snapshot.ResearchInfo
{
	/// <summary>
	/// Contains immutable tech info state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class GlobalTechInfo
	{
		/// <summary>
		/// The ID of this tech in the technology file e.g. "1001".
		/// This is NOT the tech index!
		/// </summary>
		public int techID										{ get; private set; }
		public TechCategory category							{ get; private set; }
		public int techLevel									{ get; private set; }
		public int plymouthCost									{ get; private set; }
		public int edenCost										{ get; private set; }
		public int maxScientists								{ get; private set; }
		public LabType labType									{ get; private set; }
		//public int playerHasTech								{ get; private set; }
		public int upgradeCount									{ get; private set; }
		public string techName									{ get; private set; }
		public string description								{ get; private set; }
		public string teaser									{ get; private set; }
		public string improveDescription						{ get; private set; }

		/// <summary>
		/// Topics required to research this technology.
		/// Contains the index into the techInfo collection.
		/// This is NOT the techID!
		/// </summary>
		public int[] requiredTopics								{ get; private set; }

		/// <summary>
		/// Creates an immutable state for research info.
		/// </summary>
		/// <param name="info">The tech info to pull data from.</param>
		public GlobalTechInfo(TechInfo info)
		{
			techID									= info.GetTechID();
			category								= info.GetCategory();
			techLevel								= info.GetTechLevel();
			plymouthCost							= info.GetPlymouthCost();
			edenCost								= info.GetEdenCost();
			maxScientists							= info.GetMaxScientists();
			labType									= info.GetLab();
			//playerHasTech							= info.GetPlayerHasTech();
			upgradeCount							= info.GetNumUpgrades();
			techName								= info.GetTechName();
			description								= info.GetDescription();
			teaser									= info.GetTeaser();
			improveDescription						= info.GetImproveDesc();

			requiredTopics = new int[info.GetNumRequiredTechs()];

			for (int i=0; i < requiredTopics.Length; ++i)
				requiredTopics[i] = info.GetRequiredTechIndex(i);
		}
	}
}
