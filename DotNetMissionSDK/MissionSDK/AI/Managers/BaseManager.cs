using DotNetMissionSDK.AI.Tasks.Base.Starship;
using DotNetMissionSDK.Utility;
using DotNetMissionSDK.AI.Tasks;
using DotNetMissionSDK.AI.Tasks.Base.Mining;
using DotNetMissionSDK.AI.Tasks.Base.Maintenance;
using DotNetMissionSDK.AI.Tasks.Base.Vehicle;
using System.Collections.Generic;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.AI.CombatGroups;

namespace DotNetMissionSDK.AI.Managers
{
	public class BaseManager
	{
		public const int ExpandCommonMining_GoalID			= 0;
		public const int MaintainTruckRoutes_GoalID			= 1;
		public const int MaintainPower_GoalID				= 2;
		public const int UnloadSupplies_GoalID				= 3;
		public const int FixDisconnectedStructures_GoalID	= 4;
		public const int RepairStructures_GoalID			= 5;
		public const int MaintainFood_GoalID				= 6;
		public const int MaintainPopulation_GoalID			= 7;
		public const int MaintainDefenses_GoalID			= 8;
		public const int MaintainWalls_GoalID				= 9;
		public const int ExpandRareMining_GoalID			= 10;
		public const int LaunchStarship_GoalID				= 11;

		private ResetVehicleTask m_ResetVehicleTask;

		private MiningBaseState m_MiningBaseState;

		private BuildVehicleTask[] m_BuildVehicleTasks;

		public BotPlayer botPlayer	{ get; private set; }
		public PlayerInfo owner		{ get; private set; }

		public Goal[] goals			{ get; private set; }			// This manager's top-level goals.


		public BaseManager(BotPlayer botPlayer, PlayerInfo owner)
		{
			this.botPlayer = botPlayer;
			this.owner = owner;

			m_MiningBaseState = new MiningBaseState(owner);

			m_BuildVehicleTasks = CreateVehicleTasks();

			// Initialize goals
			goals = new Goal[]
			{
				new Goal(new CreateCommonMiningBaseTask(owner, m_MiningBaseState), 1),
				new Goal(new MaintainTruckRoutes(owner, m_MiningBaseState), 1),
				new Goal(new MaintainPowerTask(owner), 1),
				new Goal(new UnloadSuppliesTask(owner), 1),
				new Goal(new FixDisconnectedStructures(owner), 1),
				new Goal(new RepairStructuresTask(owner), 1),
				new Goal(new MaintainFoodTask(owner), 1),
				new Goal(new MaintainPopulationTask(owner), 1),
				new Goal(new MaintainDefenseTask(owner), 1),
				new Goal(new MaintainWallsTask(owner), 1),
				new Goal(new CreateRareMiningBaseTask(owner, m_MiningBaseState), 1),
				new Goal(new DeployEvacModuleTask(owner), 1),
			};

			// TODO: Fill out goal weights
			switch (botPlayer.botType)
			{
				case BotType.PopulationGrowth:
				case BotType.LaunchStarship:
				case BotType.EconomicGrowth:
				case BotType.Passive:
				case BotType.Defender:
				case BotType.Balanced:
				case BotType.Aggressive:
				case BotType.Harassment:
				case BotType.Wreckless:
					break;
			}

			m_ResetVehicleTask = new ResetVehicleTask(owner);
		}

		public void Update()
		{
			m_MiningBaseState.Update();
			
			UpdateGoal();

			// Fix stuck vehicles
			m_ResetVehicleTask.PerformTaskTree();
		}

		private void UpdateGoal()
		{
			// Top priority is establishing a base with common metals
			if (owner.units.commandCenters.Count == 0 || owner.units.commonOreMines.Count == 0 || owner.units.commonOreSmelters.Count == 0)
			{
				goals[ExpandCommonMining_GoalID].task.PerformTaskTree();
				return;
			}

			// Power level is high priority
			if (!goals[MaintainPower_GoalID].task.IsTaskComplete())
				goals[MaintainPower_GoalID].task.PerformTaskTree();

			// Unload supplies
			if (!goals[UnloadSupplies_GoalID].task.IsTaskComplete())
				goals[UnloadSupplies_GoalID].task.PerformTaskTree();

			// Maintain truck routes
			MaintainTruckRoutes truckRoutes = (MaintainTruckRoutes)goals[MaintainTruckRoutes_GoalID].task;
			truckRoutes.UpdateNeededTrucks();

			if (!truckRoutes.IsTaskComplete())
				truckRoutes.PerformTaskTree();

			truckRoutes.PerformTruckRoutes();

			// Emergency repairs
			RepairStructuresTask repairStructureTask = (RepairStructuresTask)goals[RepairStructures_GoalID].task;
			repairStructureTask.repairCriticalOnly = true;

			if (!repairStructureTask.IsTaskComplete())
				repairStructureTask.PerformTaskTree();
			
			// Fix disconnected structures
			if (!goals[FixDisconnectedStructures_GoalID].task.IsTaskComplete())
				goals[FixDisconnectedStructures_GoalID].task.PerformTaskTree(); // Tubes
			else if (!goals[MaintainWalls_GoalID].task.IsTaskComplete())
				goals[MaintainWalls_GoalID].task.PerformTaskTree(); // Walls

			// Keep people fed
			if (!goals[MaintainFood_GoalID].task.IsTaskComplete())
				goals[MaintainFood_GoalID].task.PerformTaskTree();

			// Grow population
			MaintainPopulationTask maintainPopulationTask = (MaintainPopulationTask)goals[MaintainPopulation_GoalID].task;
			maintainPopulationTask.UpdateRequirements();

			if (!maintainPopulationTask.IsTaskComplete())
			{
				maintainPopulationTask.PerformTaskTree();
				PerformFullRepairs();
				return;
			}

			// Clear combat unit tasks
			foreach (BuildVehicleTask vehicleTask in m_BuildVehicleTasks)
				vehicleTask.targetCountToBuild = 0;

			// Build defenses
			if (!goals[MaintainDefenses_GoalID].task.IsTaskComplete())
				goals[MaintainDefenses_GoalID].task.PerformTaskTree();

			// Expand rare mining
			if (owner.units.rareOreMines.Count == 0 || owner.units.rareOreSmelters.Count == 0)
			{
				goals[ExpandRareMining_GoalID].task.PerformTaskTree();
				PerformFullRepairs();
				return;
			}
			
			// Expand common mining
			goals[ExpandCommonMining_GoalID].task.PerformTaskTree();
			PerformFullRepairs();

			// Build combat units
			if (owner.player.Ore() > 2800)
			{
				List<CombatGroup.UnitWithWeapon> desiredCombatUnits = botPlayer.combatManager.GetDesiredUnits();
				foreach (CombatGroup.UnitWithWeapon unitWithWeapon in desiredCombatUnits)
					GetVehicleTask(unitWithWeapon).targetCountToBuild += 1;

				foreach (BuildVehicleTask vehicleTask in m_BuildVehicleTasks)
				{
					if (!vehicleTask.IsTaskComplete())
						vehicleTask.PerformTaskTree();
				}
			}

			//return goals[LaunchStarship_GoalID];
		}

		private bool PerformFullRepairs()
		{
			RepairStructuresTask repairStructureTask = (RepairStructuresTask)goals[RepairStructures_GoalID].task;
			repairStructureTask.repairCriticalOnly = false;

			if (!repairStructureTask.IsTaskComplete())
				return repairStructureTask.PerformTaskTree();

			return false;
		}



		private BuildVehicleTask[] CreateVehicleTasks()
		{
			return new BuildVehicleTask[]
			{
				new BuildScoutTask(owner),
				new BuildSpiderTask(owner),
				new BuildScorpionTask(owner),
				new BuildLynxAcidCloudTask(owner),
				new BuildLynxEMPTask(owner),
				new BuildLynxLaserTask(owner),
				new BuildLynxMicrowaveTask(owner),
				new BuildLynxRailGunTask(owner),
				new BuildLynxRPGTask(owner),
				new BuildLynxStarflareTask(owner),
				new BuildLynxSupernovaTask(owner),
				new BuildLynxESGTask(owner),
				new BuildLynxStickyfoamTask(owner),
				new BuildLynxThorsHammerTask(owner),
				new BuildPantherAcidCloudTask(owner),
				new BuildPantherEMPTask(owner),
				new BuildPantherLaserTask(owner),
				new BuildPantherMicrowaveTask(owner),
				new BuildPantherRailGunTask(owner),
				new BuildPantherRPGTask(owner),
				new BuildPantherStarflareTask(owner),
				new BuildPantherSupernovaTask(owner),
				new BuildPantherESGTask(owner),
				new BuildPantherStickyfoamTask(owner),
				new BuildPantherThorsHammerTask(owner),
				new BuildTigerAcidCloudTask(owner),
				new BuildTigerEMPTask(owner),
				new BuildTigerLaserTask(owner),
				new BuildTigerMicrowaveTask(owner),
				new BuildTigerRailGunTask(owner),
				new BuildTigerRPGTask(owner),
				new BuildTigerStarflareTask(owner),
				new BuildTigerSupernovaTask(owner),
				new BuildTigerESGTask(owner),
				new BuildTigerStickyfoamTask(owner),
				new BuildTigerThorsHammerTask(owner)
			};
		}

		private BuildVehicleTask GetVehicleTask(CombatGroup.UnitWithWeapon unitWithWeapon)
		{
			int index = -1;

			switch (unitWithWeapon.unit)
			{
				case map_id.Scout:		index = 0;											break;
				case map_id.Spider:		index = 1;											break;
				case map_id.Scorpion:	index = 2;											break;
				case map_id.Lynx:		index = 3 + GetWeaponIndex(unitWithWeapon.weapon);	break;
				case map_id.Panther:	index = 14 + GetWeaponIndex(unitWithWeapon.weapon);	break;
				case map_id.Tiger:		index = 25 + GetWeaponIndex(unitWithWeapon.weapon);	break;
			}

			if (index < 0)
				return null;

			return m_BuildVehicleTasks[index];
		}

		private int GetWeaponIndex(map_id weapon)
		{
			switch (weapon)
			{
				case map_id.AcidCloud:			return 0;
				case map_id.EMP:				return 1;
				case map_id.Laser:				return 2;
				case map_id.Microwave:			return 3;
				case map_id.RailGun:			return 4;
				case map_id.RPG:				return 5;
				case map_id.Starflare:			return 6;
				case map_id.Supernova:			return 7;
				case map_id.ESG:				return 8;
				case map_id.Stickyfoam:			return 9;
				case map_id.ThorsHammer:		return 10;
			}

			return -999;
		}
	}
}
