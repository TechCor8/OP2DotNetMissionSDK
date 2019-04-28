using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Vehicle
{
	public class BuildSurveyorTask : BuildVehicleTask
	{
		public BuildSurveyorTask()											{ m_VehicleToBuild = map_id.RoboSurveyor;				}
		public BuildSurveyorTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.RoboSurveyor;				}

		public override bool IsTaskComplete()
		{
			if (owner.units.EDWARDSatelliteCount > 0)
				return true;

			return base.IsTaskComplete();
		}
	}

	public class BuildMinerTask : BuildVehicleTask
	{
		public BuildMinerTask()												{ m_VehicleToBuild = map_id.RoboMiner;					}
		public BuildMinerTask(PlayerInfo owner) : base(owner)				{ m_VehicleToBuild = map_id.RoboMiner;					}
	}

	public class BuildConvecTask : BuildVehicleTask
	{
		public BuildConvecTask()											{ m_VehicleToBuild = map_id.ConVec;						}
		public BuildConvecTask(PlayerInfo owner) : base(owner)				{ m_VehicleToBuild = map_id.ConVec;						}
	}

	public class BuildCargoTruckTask : BuildVehicleTask
	{
		public BuildCargoTruckTask()										{ m_VehicleToBuild = map_id.CargoTruck;					}
		public BuildCargoTruckTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.CargoTruck;					}
	}

	public class BuildEarthworkerTask : BuildVehicleTask
	{
		public BuildEarthworkerTask()										{ m_VehicleToBuild = map_id.Earthworker;				}
		public BuildEarthworkerTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.Earthworker;				}
	}

	public class BuildEvacTransportTask : BuildVehicleTask
	{
		public BuildEvacTransportTask()										{ m_VehicleToBuild = map_id.EvacuationTransport;		}
		public BuildEvacTransportTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.EvacuationTransport;		}
	}

	public class BuildGeoConTask : BuildVehicleTask
	{
		public BuildGeoConTask()											{ m_VehicleToBuild = map_id.GeoCon;						}
		public BuildGeoConTask(PlayerInfo owner) : base(owner)				{ m_VehicleToBuild = map_id.GeoCon;						}
	}

	public class BuildRepairVehicleTask : BuildVehicleTask
	{
		public BuildRepairVehicleTask()										{ m_VehicleToBuild = map_id.RepairVehicle;				}
		public BuildRepairVehicleTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.RepairVehicle;				}
	}

	public class BuildRoboDozerTask : BuildVehicleTask
	{
		public BuildRoboDozerTask()											{ m_VehicleToBuild = map_id.RoboDozer;					}
		public BuildRoboDozerTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.RoboDozer;					}
	}

	public class BuildScoutTask : BuildVehicleTask
	{
		public BuildScoutTask()												{ m_VehicleToBuild = map_id.Scout;						}
		public BuildScoutTask(PlayerInfo owner) : base(owner)				{ m_VehicleToBuild = map_id.Scout;						}
	}

	public class BuildSpiderTask : BuildVehicleTask
	{
		public BuildSpiderTask()											{ m_VehicleToBuild = map_id.Spider;						}
		public BuildSpiderTask(PlayerInfo owner) : base(owner)				{ m_VehicleToBuild = map_id.Spider;						}
	}

	public class BuildScorpionTask : BuildVehicleTask
	{
		public BuildScorpionTask()											{ m_VehicleToBuild = map_id.Scorpion;					}
		public BuildScorpionTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.Scorpion;					}
	}

	// *** Military Units - Lynx ***
	public class BuildLynxAcidCloudTask : BuildVehicleTask
	{
		public BuildLynxAcidCloudTask()										{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.AcidCloud;		}
		public BuildLynxAcidCloudTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.AcidCloud;		}
	}

	public class BuildLynxEMPTask : BuildVehicleTask
	{
		public BuildLynxEMPTask()											{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.EMP;			}
		public BuildLynxEMPTask(PlayerInfo owner) : base(owner)				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.EMP;			}
	}

	public class BuildLynxLaserTask : BuildVehicleTask
	{
		public BuildLynxLaserTask()											{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Laser;			}
		public BuildLynxLaserTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Laser;			}
	}

	public class BuildLynxMicrowaveTask : BuildVehicleTask
	{
		public BuildLynxMicrowaveTask()										{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Microwave;		}
		public BuildLynxMicrowaveTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Microwave;		}
	}

	public class BuildLynxRailGunTask : BuildVehicleTask
	{
		public BuildLynxRailGunTask()										{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RailGun;		}
		public BuildLynxRailGunTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RailGun;		}
	}

	public class BuildLynxRPGTask : BuildVehicleTask
	{
		public BuildLynxRPGTask()											{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RPG;			}
		public BuildLynxRPGTask(PlayerInfo owner) : base(owner)				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RPG;			}
	}

	public class BuildLynxStarflareTask : BuildVehicleTask
	{
		public BuildLynxStarflareTask()										{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Starflare;		}
		public BuildLynxStarflareTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Starflare;		}
	}

	public class BuildLynxSupernovaTask : BuildVehicleTask
	{
		public BuildLynxSupernovaTask()										{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Supernova;		}
		public BuildLynxSupernovaTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Supernova;		}
	}

	public class BuildLynxESGTask : BuildVehicleTask
	{
		public BuildLynxESGTask()											{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ESG;			}
		public BuildLynxESGTask(PlayerInfo owner) : base(owner)				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ESG;			}
	}

	public class BuildLynxStickyfoamTask : BuildVehicleTask
	{
		public BuildLynxStickyfoamTask()									{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Stickyfoam;	}
		public BuildLynxStickyfoamTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Stickyfoam;	}
	}

	public class BuildLynxThorsHammerTask : BuildVehicleTask
	{
		public BuildLynxThorsHammerTask()									{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ThorsHammer;	}
		public BuildLynxThorsHammerTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ThorsHammer;	}
	}

	// *** Military Units - Panther ***
	public class BuildPantherAcidCloudTask : BuildVehicleTask
	{
		public BuildPantherAcidCloudTask()									{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.AcidCloud;		}
		public BuildPantherAcidCloudTask(PlayerInfo owner) : base(owner)	{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.AcidCloud;		}
	}

	public class BuildPantherEMPTask : BuildVehicleTask
	{
		public BuildPantherEMPTask()										{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.EMP;			}
		public BuildPantherEMPTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.EMP;			}
	}

	public class BuildPantherLaserTask : BuildVehicleTask
	{
		public BuildPantherLaserTask()										{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Laser;			}
		public BuildPantherLaserTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Laser;			}
	}

	public class BuildPantherMicrowaveTask : BuildVehicleTask
	{
		public BuildPantherMicrowaveTask()									{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Microwave;		}
		public BuildPantherMicrowaveTask(PlayerInfo owner) : base(owner)	{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Microwave;		}
	}

	public class BuildPantherRailGunTask : BuildVehicleTask
	{
		public BuildPantherRailGunTask()									{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RailGun;		}
		public BuildPantherRailGunTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RailGun;		}
	}

	public class BuildPantherRPGTask : BuildVehicleTask
	{
		public BuildPantherRPGTask()										{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RPG;			}
		public BuildPantherRPGTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RPG;			}
	}

	public class BuildPantherStarflareTask : BuildVehicleTask
	{
		public BuildPantherStarflareTask()									{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Starflare;		}
		public BuildPantherStarflareTask(PlayerInfo owner) : base(owner)	{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Starflare;		}
	}

	public class BuildPantherSupernovaTask : BuildVehicleTask
	{
		public BuildPantherSupernovaTask()									{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Supernova;		}
		public BuildPantherSupernovaTask(PlayerInfo owner) : base(owner)	{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Supernova;		}
	}

	public class BuildPantherESGTask : BuildVehicleTask
	{
		public BuildPantherESGTask()										{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ESG;			}
		public BuildPantherESGTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ESG;			}
	}

	public class BuildPantherStickyfoamTask : BuildVehicleTask
	{
		public BuildPantherStickyfoamTask()									{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Stickyfoam;	}
		public BuildPantherStickyfoamTask(PlayerInfo owner) : base(owner)	{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Stickyfoam;	}
	}

	public class BuildPantherThorsHammerTask : BuildVehicleTask
	{
		public BuildPantherThorsHammerTask()								{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ThorsHammer;	}
		public BuildPantherThorsHammerTask(PlayerInfo owner) : base(owner)	{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ThorsHammer;	}
	}

	// *** Military Units - Tiger ***
	public class BuildTigerAcidCloudTask : BuildVehicleTask
	{
		public BuildTigerAcidCloudTask()									{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.AcidCloud;		}
		public BuildTigerAcidCloudTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.AcidCloud;		}
	}

	public class BuildTigerEMPTask : BuildVehicleTask
	{
		public BuildTigerEMPTask()											{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.EMP;			}
		public BuildTigerEMPTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.EMP;			}
	}

	public class BuildTigerLaserTask : BuildVehicleTask
	{
		public BuildTigerLaserTask()										{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Laser;			}
		public BuildTigerLaserTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Laser;			}
	}

	public class BuildTigerMicrowaveTask : BuildVehicleTask
	{
		public BuildTigerMicrowaveTask()									{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Microwave;		}
		public BuildTigerMicrowaveTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Microwave;		}
	}

	public class BuildTigerRailGunTask : BuildVehicleTask
	{
		public BuildTigerRailGunTask()										{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RailGun;		}
		public BuildTigerRailGunTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RailGun;		}
	}

	public class BuildTigerRPGTask : BuildVehicleTask
	{
		public BuildTigerRPGTask()											{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RPG;			}
		public BuildTigerRPGTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RPG;			}
	}

	public class BuildTigerStarflareTask : BuildVehicleTask
	{
		public BuildTigerStarflareTask()									{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Starflare;		}
		public BuildTigerStarflareTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Starflare;		}
	}

	public class BuildTigerSupernovaTask : BuildVehicleTask
	{
		public BuildTigerSupernovaTask()									{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Supernova;		}
		public BuildTigerSupernovaTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Supernova;		}
	}

	public class BuildTigerESGTask : BuildVehicleTask
	{
		public BuildTigerESGTask()											{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ESG;			}
		public BuildTigerESGTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ESG;			}
	}

	public class BuildTigerStickyfoamTask : BuildVehicleTask
	{
		public BuildTigerStickyfoamTask()									{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Stickyfoam;	}
		public BuildTigerStickyfoamTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Stickyfoam;	}
	}

	public class BuildTigerThorsHammerTask : BuildVehicleTask
	{
		public BuildTigerThorsHammerTask()									{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ThorsHammer;	}
		public BuildTigerThorsHammerTask(PlayerInfo owner) : base(owner)	{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ThorsHammer;	}
	}
}
