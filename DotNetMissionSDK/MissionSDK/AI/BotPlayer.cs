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
		public BaseManager baseManager			{ get; private set; }

		public bool isActive					{ get; private set; }		// Is the bot controlling the player?


		public BotPlayer(BotType botType, PlayerInfo playerToControl)
		{
			baseManager = new BaseManager(botType, playerToControl);
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
		}

		/*
		 * Managers (Prioritizes top-level tasks)
		 * BaseManager
		 * ResearchManager
		 * CombatManager
		 * 
		 * ITask
		 * CanPerformTask
		 * IsTaskComplete
		 * PerformTask
		 * 
		 * Normal
		 * SpaceRace
		 * ResourceRace
		*/

		/*
		Initial Deployment
		- Find 3 closest unoccupied beacons, go to one randomly as long as the difference in distance is not too great. Avoid beacons that are closer to the enemy than self.
		- Avoid beacons that have lava possible unless no other option is available
		- Move close to (but not on top of) beacon
		- Deploy CC, Structure Factory, Smelter. Placement must not require tubing.
		- Survey Beacon
		- Deploy mine

		Essential Structures
		- If no structure factory, smelter, deploy them
		- If no tokamak, deploy tokamak
		- If trucks contain food or metals, unload them appropriately
		- Set up mining route from mine to smelter
		- Structure Factory and Smelter must always be connected with tubes
		- Must have enough power for all structures at all times
		- If agridomes are disconnected, determine if it should be connected (max distance)
		- Must have enough agridomes to feed population
		- Must have a standard lab  (make sure it's connected)

		Morale Structures
		- If Offspring Enhancement not researched, research it
		- If nursery not built, build it (make sure it's connected)

		- If Research Training Programs not researched, research it
		- If university not built, build it (make sure it's connected)

		If morale can NOT fluctuate, do not execute further

		- Must have enough residences for the population (make sure it's connected)
		- Research medical centers
		- Must have enough medical centers (make sure it's connected)

		If optional buildings turned on,
		- Research DIRT, must have enough
		- Research recreation, must have enough
		- Research GORF, build 1
		- Research Consumerism, build 1

		Update Research
		- Must have advanced lab
		- Perform random research (if flag is set)

		Defense
		- If researched, build vehicle factory, robot cc
		- If researched, build X amount of lynx

		Not required for next level of pyramid:
		- Build X number of each type of guard post
		- Surround guard posts with walls
		- X number of light towers at Y distance to colony

		Rare Metal
		- If researched, survey, expand if far away
		- Not considered met until cargo routes set

		Common Expansion
		if not enough common metal, expand common

		Rare Expansion
		if not enough rare metal, expand rare

		Military Buildup
		Build random military units at all vehicle factories
		Build military structures (this happens if all vehicle factories are producing and metal is left over)
*/
		/*
		 * if (!Deployment.IsMet() && CanMeet()) Deployment.Update(); return;
		 * if (!EssentialStructures.IsMet() && CanMeet()) EssentialStructures.Update(); return;
		 * if (!MoraleStructures.IsMet() && CanMeet()) MoraleStructures.Update(); if (morale < Fair && morale flux) return;
		 * UpdateResearch. Build AdvancedLab
		 * if (!Defense.IsMet()) Defense.Update();
		 * if (!RareMetal.IsMet()) RareMetal.Update();
		 * if (commonMetal < 5000) ExpandCommon; return;
		 * if (rareMetal < 5000) ExpandRare;
		 * BuildMilitaryUnits
		 * BuildMilitaryStructures
		*/
	}
}
