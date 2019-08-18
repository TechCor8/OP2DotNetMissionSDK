using DotNetMissionSDK.State.Snapshot;
using System;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Starship
{
	public sealed class DeployEvacModuleTask : DeployStarshipModuleTask
	{
		public DeployEvacModuleTask(int ownerID) : base(ownerID)		{ m_StarshipModule = map_id.EvacuationModule;								}
		
		// Check if starship module was deployed
		public override bool IsTaskComplete(StateSnapshot stateSnapshot){ return stateSnapshot.players[ownerID].units.evacuationModuleCount > 0;	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeployFoodCargoTask(ownerID));
			AddPrerequisite(new DeployCommonCargoTask(ownerID));
			AddPrerequisite(new DeployRareCargoTask(ownerID));
		}
	}

	public sealed class DeployFoodCargoTask : DeployStarshipModuleTask
	{
		public DeployFoodCargoTask(int ownerID) : base(ownerID)			{ m_StarshipModule = map_id.FoodCargo;										}

		// Check if starship module was deployed
		public override bool IsTaskComplete(StateSnapshot stateSnapshot){ return stateSnapshot.players[ownerID].units.foodCargoCount > 0;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeployPhoenixModuleTask(ownerID));
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, List<Action> unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			if (owner.foodStored < 10000)
				return false;

			return base.PerformTask(stateSnapshot, unitActions);
		}
	}

	public sealed class DeployCommonCargoTask : DeployStarshipModuleTask
	{
		public DeployCommonCargoTask(int ownerID) : base(ownerID)		{ m_StarshipModule = map_id.CommonMetalsCargo;								}

		// Check if starship module was deployed
		public override bool IsTaskComplete(StateSnapshot stateSnapshot){ return stateSnapshot.players[ownerID].units.commonMetalsCargoCount > 0;	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeployPhoenixModuleTask(ownerID));
		}
	}

	public sealed class DeployRareCargoTask : DeployStarshipModuleTask
	{
		public DeployRareCargoTask(int ownerID) : base(ownerID)			{ m_StarshipModule = map_id.RareMetalsCargo;								}

		// Check if starship module was deployed
		public override bool IsTaskComplete(StateSnapshot stateSnapshot){ return stateSnapshot.players[ownerID].units.rareMetalsCargoCount > 0;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeployPhoenixModuleTask(ownerID));
		}
	}

	public sealed class DeployPhoenixModuleTask : DeployStarshipModuleTask
	{
		public DeployPhoenixModuleTask(int ownerID) : base(ownerID)		{ m_StarshipModule = map_id.PhoenixModule;									}

		// Check if starship module was deployed
		public override bool IsTaskComplete(StateSnapshot stateSnapshot){ return stateSnapshot.players[ownerID].units.phoenixModuleCount > 0;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeployOrbitalPackageTask(ownerID));
			AddPrerequisite(new DeploySensorPackageTask(ownerID));
			AddPrerequisite(new DeployStasisSystemsTask(ownerID));
			AddPrerequisite(new DeployHabitatRingTask(ownerID));
			AddPrerequisite(new DeployFuelingSystemsTask(ownerID));
			AddPrerequisite(new DeployCommandModuleTask(ownerID));
			AddPrerequisite(new DeployFusionDriveTask(ownerID));
			AddPrerequisite(new DeployIonDriveTask(ownerID));
		}
	}

	public sealed class DeployOrbitalPackageTask : DeployStarshipModuleTask
	{
		public DeployOrbitalPackageTask(int ownerID) : base(ownerID)	{ m_StarshipModule = map_id.OrbitalPackage;									}

		// Check if starship module was deployed
		public override bool IsTaskComplete(StateSnapshot stateSnapshot){ return stateSnapshot.players[ownerID].units.orbitalPackageCount > 0;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeploySkydockTask(ownerID));
		}
	}

	public sealed class DeploySensorPackageTask : DeployStarshipModuleTask
	{
		public DeploySensorPackageTask(int ownerID) : base(ownerID)		{ m_StarshipModule = map_id.SensorPackage;									}

		// Check if starship module was deployed
		public override bool IsTaskComplete(StateSnapshot stateSnapshot){ return stateSnapshot.players[ownerID].units.sensorPackageCount > 0;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeploySkydockTask(ownerID));
		}
	}

	public sealed class DeployStasisSystemsTask : DeployStarshipModuleTask
	{
		public DeployStasisSystemsTask(int ownerID) : base(ownerID)		{ m_StarshipModule = map_id.StasisSystems;									}

		// Check if starship module was deployed
		public override bool IsTaskComplete(StateSnapshot stateSnapshot){ return stateSnapshot.players[ownerID].units.stasisSystemsCount > 0;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeploySkydockTask(ownerID));
		}
	}

	public sealed class DeployHabitatRingTask : DeployStarshipModuleTask
	{
		public DeployHabitatRingTask(int ownerID) : base(ownerID)		{ m_StarshipModule = map_id.HabitatRing;									}

		// Check if starship module was deployed
		public override bool IsTaskComplete(StateSnapshot stateSnapshot){ return stateSnapshot.players[ownerID].units.habitatRingCount > 0;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeploySkydockTask(ownerID));
		}
	}

	public sealed class DeployFuelingSystemsTask : DeployStarshipModuleTask
	{
		public DeployFuelingSystemsTask(int ownerID) : base(ownerID)	{ m_StarshipModule = map_id.FuelingSystems;									}

		// Check if starship module was deployed
		public override bool IsTaskComplete(StateSnapshot stateSnapshot){ return stateSnapshot.players[ownerID].units.fuelingSystemsCount > 0;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeploySkydockTask(ownerID));
		}
	}

	public sealed class DeployCommandModuleTask : DeployStarshipModuleTask
	{
		public DeployCommandModuleTask(int ownerID) : base(ownerID)		{ m_StarshipModule = map_id.CommandModule;									}

		// Check if starship module was deployed
		public override bool IsTaskComplete(StateSnapshot stateSnapshot){ return stateSnapshot.players[ownerID].units.commandModuleCount > 0;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeploySkydockTask(ownerID));
		}
	}

	public sealed class DeployFusionDriveTask : DeployStarshipModuleTask
	{
		public DeployFusionDriveTask(int ownerID) : base(ownerID)		{ m_StarshipModule = map_id.FusionDriveModule;								}

		// Check if starship module was deployed
		public override bool IsTaskComplete(StateSnapshot stateSnapshot){ return stateSnapshot.players[ownerID].units.fusionDriveModuleCount > 0;	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeploySkydockTask(ownerID));
		}
	}

	public sealed class DeployIonDriveTask : DeployStarshipModuleTask
	{
		public DeployIonDriveTask(int ownerID) : base(ownerID)			{ m_StarshipModule = map_id.IonDriveModule;									}

		// Check if starship module was deployed
		public override bool IsTaskComplete(StateSnapshot stateSnapshot){ return stateSnapshot.players[ownerID].units.ionDriveModuleCount > 0;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeploySkydockTask(ownerID));
		}
	}

	public sealed class DeploySkydockTask : DeployStarshipModuleTask
	{
		public DeploySkydockTask(int ownerID) : base(ownerID)			{ m_StarshipModule = map_id.Skydock;										}

		// Check if starship module was deployed
		public override bool IsTaskComplete(StateSnapshot stateSnapshot){ return stateSnapshot.players[ownerID].units.skydockCount > 0;				}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
		}
	}

	public sealed class DeploySolarSatTask : DeployStarshipModuleTask
	{
		public DeploySolarSatTask(int ownerID) : base(ownerID)			{ m_StarshipModule = map_id.SolarSatellite;									}

		// Check if satellite was deployed
		public override bool IsTaskComplete(StateSnapshot stateSnapshot){ return stateSnapshot.players[ownerID].units.solarSatelliteCount > 0;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
		}
	}

	public sealed class DeployEDWARDSatTask : DeployStarshipModuleTask
	{
		public DeployEDWARDSatTask(int ownerID) : base(ownerID)			{ m_StarshipModule = map_id.EDWARDSatellite;								}

		// Check if satellite was deployed
		public override bool IsTaskComplete(StateSnapshot stateSnapshot){ return stateSnapshot.players[ownerID].units.EDWARDSatelliteCount > 0;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
		}
	}
}
