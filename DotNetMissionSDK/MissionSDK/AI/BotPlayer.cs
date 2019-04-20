using DotNetMissionSDK.AI.Managers;
using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI
{
	/// <summary>
	/// Represents different predefined bot goal weights.
	/// </summary>
	public enum BotType
	{
		PopulationGrowth,		// Bot focuses on growing population. Keeps enough defense to avoid being killed. Will build Recreation, DIRT and other optional structures.
		LaunchStarship,			// Bot focuses on launching starship. Keeps enough defense to avoid being killed.
		EconomicGrowth,			// Bot focuses on resource acquisition. Keeps enough defense to avoid being killed.
		Passive,				// Bot does not build new structures. Keeps enough defense to avoid being killed.
		Defender,				// Bot will build military units and defend itself and allies. Does not attack.
		Balanced,				// Bot will build military units and defend itself and allies. Attacks with best available strategy.
		Aggressive,				// Bot will build military units and won't defend itself or allies. Attacks with best available strategy.
		Harassment,				// Bot will build military units and harass cargo trucks, power plants, and unescorted or poorly defended utility vehicles.
		Wreckless,				// Bot will build military units and send them to attack even against overwhelming odds.
	}

	public class BotPlayer
	{/*
		// Customizable Flags - Can be changed while bot is active.
		public bool canResearchOptionalStructures = true;   // If true, bot will research recreational/forum, GORF, DIRT, consumer factory and other optional structures.
		public bool canBuildGuardPosts = true;              // If true, bot will build guard posts.
		public bool canBuildLightTowers;                    // If true, bot will build light posts around the exterior of its bases.
		public bool canBuildEvacTransports;                 // If true, bot will build enough evac transports for its population. Transports will roam around the colony.
		public bool canBuildScouts;                         // If true, bot will build scouts. Scouts will roam the map and investigate enemy activity.
		public bool canBuildMilitaryUnits = true;           // If true, bot will build military units (lynx, panther, tiger). Units will not move without military commander.
		public bool canLaunchEvacModule;                    // If true, bot will launch the 200 population evac module from the spaceport.
		*/
		public BaseManager baseManager				{ get; private set; }
		public LaborManager laborManager			{ get; private set; }

		public bool isActive						{ get; private set; }		// Is the bot controlling the player?


		public BotPlayer(BotType botType, PlayerInfo playerToControl)
		{
			baseManager = new BaseManager(botType, playerToControl);
			laborManager = new LaborManager(playerToControl);
		}

		public void Start()
		{
			isActive = true;
		}

		public void Stop()
		{
			isActive = false;
		}

		public void Update()
		{
			if (!isActive)
				return;

			// Update managers
			baseManager.Update();
			laborManager.Update();
		}
	}
}
