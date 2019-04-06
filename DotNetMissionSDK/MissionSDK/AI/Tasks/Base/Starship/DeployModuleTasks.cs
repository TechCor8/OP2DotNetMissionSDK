using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Starship
{
	public sealed class DeployEvacModuleTask : DeployStarshipModuleTask
	{
		private DeployEvacModuleTask()									{ m_StarshipModule = map_id.EvacuationModule;			}
		public DeployEvacModuleTask(PlayerInfo owner) : base(owner)		{ m_StarshipModule = map_id.EvacuationModule;			}

		// Check if starship module was deployed
		public override bool IsTaskComplete()							{ return owner.units.evacuationModuleCount > 0;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeployFoodCargoTask());
			AddPrerequisite(new DeployCommonCargoTask());
			AddPrerequisite(new DeployRareCargoTask());
		}
	}

	public sealed class DeployFoodCargoTask : DeployStarshipModuleTask
	{
		public DeployFoodCargoTask()									{ m_StarshipModule = map_id.FoodCargo;					}
		public DeployFoodCargoTask(PlayerInfo owner) : base(owner)		{ m_StarshipModule = map_id.FoodCargo;					}

		// Check if starship module was deployed
		public override bool IsTaskComplete()							{ return owner.units.foodCargoCount > 0;				}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeployPhoenixModuleTask());
		}

		protected override bool PerformTask()
		{
			if (owner.player.FoodStored() < 10000)
				return false;

			return base.PerformTask();
		}
	}

	public sealed class DeployCommonCargoTask : DeployStarshipModuleTask
	{
		public DeployCommonCargoTask()									{ m_StarshipModule = map_id.CommonMetalsCargo;			}
		public DeployCommonCargoTask(PlayerInfo owner) : base(owner)	{ m_StarshipModule = map_id.CommonMetalsCargo;			}

		// Check if starship module was deployed
		public override bool IsTaskComplete()							{ return owner.units.commonMetalsCargoCount > 0;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeployPhoenixModuleTask());
		}
	}

	public sealed class DeployRareCargoTask : DeployStarshipModuleTask
	{
		public DeployRareCargoTask()									{ m_StarshipModule = map_id.RareMetalsCargo;			}
		public DeployRareCargoTask(PlayerInfo owner) : base(owner)		{ m_StarshipModule = map_id.RareMetalsCargo;			}

		// Check if starship module was deployed
		public override bool IsTaskComplete()							{ return owner.units.rareMetalsCargoCount > 0;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeployPhoenixModuleTask());
		}
	}

	public sealed class DeployPhoenixModuleTask : DeployStarshipModuleTask
	{
		public DeployPhoenixModuleTask()								{ m_StarshipModule = map_id.PhoenixModule;				}
		public DeployPhoenixModuleTask(PlayerInfo owner) : base(owner)	{ m_StarshipModule = map_id.PhoenixModule;				}

		// Check if starship module was deployed
		public override bool IsTaskComplete()							{ return owner.units.phoenixModuleCount > 0;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeployOrbitalPackageTask());
			AddPrerequisite(new DeploySensorPackageTask());
			AddPrerequisite(new DeployStasisSystemsTask());
			AddPrerequisite(new DeployHabitatRingTask());
			AddPrerequisite(new DeployFuelingSystemsTask());
			AddPrerequisite(new DeployCommandModuleTask());
			AddPrerequisite(new DeployFusionDriveTask());
			AddPrerequisite(new DeployIonDriveTask());
		}
	}

	public sealed class DeployOrbitalPackageTask : DeployStarshipModuleTask
	{
		public DeployOrbitalPackageTask()								{ m_StarshipModule = map_id.OrbitalPackage;				}
		public DeployOrbitalPackageTask(PlayerInfo owner) : base(owner)	{ m_StarshipModule = map_id.OrbitalPackage;				}

		// Check if starship module was deployed
		public override bool IsTaskComplete()							{ return owner.units.orbitalPackageCount > 0;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeploySkydockTask());
		}
	}

	public sealed class DeploySensorPackageTask : DeployStarshipModuleTask
	{
		public DeploySensorPackageTask()								{ m_StarshipModule = map_id.SensorPackage;				}
		public DeploySensorPackageTask(PlayerInfo owner) : base(owner)	{ m_StarshipModule = map_id.SensorPackage;				}

		// Check if starship module was deployed
		public override bool IsTaskComplete()							{ return owner.units.sensorPackageCount > 0;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeploySkydockTask());
		}
	}

	public sealed class DeployStasisSystemsTask : DeployStarshipModuleTask
	{
		public DeployStasisSystemsTask()								{ m_StarshipModule = map_id.StasisSystems;				}
		public DeployStasisSystemsTask(PlayerInfo owner) : base(owner)	{ m_StarshipModule = map_id.StasisSystems;				}

		// Check if starship module was deployed
		public override bool IsTaskComplete()							{ return owner.units.stasisSystemsCount > 0;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeploySkydockTask());
		}
	}

	public sealed class DeployHabitatRingTask : DeployStarshipModuleTask
	{
		public DeployHabitatRingTask()									{ m_StarshipModule = map_id.HabitatRing;				}
		public DeployHabitatRingTask(PlayerInfo owner) : base(owner)	{ m_StarshipModule = map_id.HabitatRing;				}

		// Check if starship module was deployed
		public override bool IsTaskComplete()							{ return owner.units.habitatRingCount > 0;				}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeploySkydockTask());
		}
	}

	public sealed class DeployFuelingSystemsTask : DeployStarshipModuleTask
	{
		public DeployFuelingSystemsTask()								{ m_StarshipModule = map_id.FuelingSystems;				}
		public DeployFuelingSystemsTask(PlayerInfo owner) : base(owner)	{ m_StarshipModule = map_id.FuelingSystems;				}

		// Check if starship module was deployed
		public override bool IsTaskComplete()							{ return owner.units.fuelingSystemsCount > 0;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeploySkydockTask());
		}
	}

	public sealed class DeployCommandModuleTask : DeployStarshipModuleTask
	{
		public DeployCommandModuleTask()								{ m_StarshipModule = map_id.CommandModule;				}
		public DeployCommandModuleTask(PlayerInfo owner) : base(owner)	{ m_StarshipModule = map_id.CommandModule;				}

		// Check if starship module was deployed
		public override bool IsTaskComplete()							{ return owner.units.commandModuleCount > 0;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeploySkydockTask());
		}
	}

	public sealed class DeployFusionDriveTask : DeployStarshipModuleTask
	{
		public DeployFusionDriveTask()									{ m_StarshipModule = map_id.FusionDriveModule;				}
		public DeployFusionDriveTask(PlayerInfo owner) : base(owner)	{ m_StarshipModule = map_id.FusionDriveModule;				}

		// Check if starship module was deployed
		public override bool IsTaskComplete()							{ return owner.units.fusionDriveModuleCount > 0;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeploySkydockTask());
		}
	}

	public sealed class DeployIonDriveTask : DeployStarshipModuleTask
	{
		public DeployIonDriveTask()										{ m_StarshipModule = map_id.IonDriveModule;					}
		public DeployIonDriveTask(PlayerInfo owner) : base(owner)		{ m_StarshipModule = map_id.IonDriveModule;					}

		// Check if starship module was deployed
		public override bool IsTaskComplete()							{ return owner.units.ionDriveModuleCount > 0;				}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(new DeploySkydockTask());
		}
	}

	public sealed class DeploySkydockTask : DeployStarshipModuleTask
	{
		public DeploySkydockTask()										{ m_StarshipModule = map_id.Skydock;					}
		public DeploySkydockTask(PlayerInfo owner) : base(owner)		{ m_StarshipModule = map_id.Skydock;					}

		// Check if starship module was deployed
		public override bool IsTaskComplete()							{ return owner.units.skydockCount > 0;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
		}
	}

	public sealed class DeploySolarSatTask : DeployStarshipModuleTask
	{
		public DeploySolarSatTask()										{ m_StarshipModule = map_id.SolarSatellite;				}
		public DeploySolarSatTask(PlayerInfo owner) : base(owner)		{ m_StarshipModule = map_id.SolarSatellite;				}

		// Check if satellite was deployed
		public override bool IsTaskComplete()							{ return owner.units.solarSatelliteCount > 0;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
		}
	}

	public sealed class DeployEDWARDSatTask : DeployStarshipModuleTask
	{
		public DeployEDWARDSatTask()									{ m_StarshipModule = map_id.EDWARDSatellite;			}
		public DeployEDWARDSatTask(PlayerInfo owner) : base(owner)		{ m_StarshipModule = map_id.EDWARDSatellite;			}

		// Check if satellite was deployed
		public override bool IsTaskComplete()							{ return owner.units.EDWARDSatelliteCount > 0;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
		}
	}
}
