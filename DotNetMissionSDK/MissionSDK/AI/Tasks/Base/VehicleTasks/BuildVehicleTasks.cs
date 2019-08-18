using DotNetMissionSDK.State.Snapshot;

namespace DotNetMissionSDK.AI.Tasks.Base.VehicleTasks
{
	public class BuildSurveyorTask : BuildVehicleTask
	{
		public BuildSurveyorTask(int ownerID) : base(ownerID)				{ m_VehicleToBuild = map_id.RoboSurveyor;				}

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			if (owner.units.EDWARDSatelliteCount > 0)
				return true;

			return base.IsTaskComplete(stateSnapshot);
		}
	}

	public class BuildMinerTask : BuildVehicleTask
	{
		public BuildMinerTask(int ownerID) : base(ownerID)					{ m_VehicleToBuild = map_id.RoboMiner;					}
	}

	public class BuildConvecTask : BuildVehicleTask
	{
		public BuildConvecTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.ConVec;						}
	}

	public class BuildCargoTruckTask : BuildVehicleTask
	{
		public BuildCargoTruckTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.CargoTruck;					}
	}

	public class BuildEarthworkerTask : BuildVehicleTask
	{
		public BuildEarthworkerTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Earthworker;				}
	}

	public class BuildEvacTransportTask : BuildVehicleTask
	{
		public BuildEvacTransportTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.EvacuationTransport;		}
	}

	public class BuildGeoConTask : BuildVehicleTask
	{
		public BuildGeoConTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.GeoCon;						}
	}

	public class BuildRepairVehicleTask : BuildVehicleTask
	{
		public BuildRepairVehicleTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.RepairVehicle;				}
	}

	public class BuildRoboDozerTask : BuildVehicleTask
	{
		public BuildRoboDozerTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.RoboDozer;					}
	}

	public class BuildScoutTask : BuildVehicleTask
	{
		public BuildScoutTask(int ownerID) : base(ownerID) 					{ m_VehicleToBuild = map_id.Scout;						}
	}

	public class BuildSpiderTask : BuildArachnidTask
	{
		public BuildSpiderTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.Spider;						}
	}

	public class BuildScorpionTask : BuildArachnidTask
	{
		public BuildScorpionTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.Scorpion;					}
	}

	// *** Military Units - Lynx ***
	public class BuildLynxAcidCloudTask : BuildVehicleTask
	{
		public BuildLynxAcidCloudTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.AcidCloud;		}
	}

	public class BuildLynxEMPTask : BuildVehicleTask
	{
		public BuildLynxEMPTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.EMP;			}
	}

	public class BuildLynxLaserTask : BuildVehicleTask
	{
		public BuildLynxLaserTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Laser;			}
	}

	public class BuildLynxMicrowaveTask : BuildVehicleTask
	{
		public BuildLynxMicrowaveTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Microwave;		}
	}

	public class BuildLynxRailGunTask : BuildVehicleTask
	{
		public BuildLynxRailGunTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RailGun;		}
	}

	public class BuildLynxRPGTask : BuildVehicleTask
	{
		public BuildLynxRPGTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RPG;			}
	}

	public class BuildLynxStarflareTask : BuildVehicleTask
	{
		public BuildLynxStarflareTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Starflare;		}
	}

	public class BuildLynxSupernovaTask : BuildVehicleTask
	{
		public BuildLynxSupernovaTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Supernova;		}
	}

	public class BuildLynxESGTask : BuildVehicleTask
	{
		public BuildLynxESGTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ESG;			}
	}

	public class BuildLynxStickyfoamTask : BuildVehicleTask
	{
		public BuildLynxStickyfoamTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Stickyfoam;	}
	}

	public class BuildLynxThorsHammerTask : BuildVehicleTask
	{
		public BuildLynxThorsHammerTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ThorsHammer;	}
	}

	// *** Military Units - Panther ***
	public class BuildPantherAcidCloudTask : BuildVehicleTask
	{
		public BuildPantherAcidCloudTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.AcidCloud;		}
	}

	public class BuildPantherEMPTask : BuildVehicleTask
	{
		public BuildPantherEMPTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.EMP;			}
	}

	public class BuildPantherLaserTask : BuildVehicleTask
	{
		public BuildPantherLaserTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Laser;			}
	}

	public class BuildPantherMicrowaveTask : BuildVehicleTask
	{
		public BuildPantherMicrowaveTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Microwave;		}
	}

	public class BuildPantherRailGunTask : BuildVehicleTask
	{
		public BuildPantherRailGunTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RailGun;		}
	}

	public class BuildPantherRPGTask : BuildVehicleTask
	{
		public BuildPantherRPGTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RPG;			}
	}

	public class BuildPantherStarflareTask : BuildVehicleTask
	{
		public BuildPantherStarflareTask(int ownerID) : base(ownerID)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Starflare;		}
	}

	public class BuildPantherSupernovaTask : BuildVehicleTask
	{
		public BuildPantherSupernovaTask(int ownerID) : base(ownerID)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Supernova;		}
	}

	public class BuildPantherESGTask : BuildVehicleTask
	{
		public BuildPantherESGTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ESG;			}
	}

	public class BuildPantherStickyfoamTask : BuildVehicleTask
	{
		public BuildPantherStickyfoamTask(int ownerID) : base(ownerID)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Stickyfoam;	}
	}

	public class BuildPantherThorsHammerTask : BuildVehicleTask
	{
		public BuildPantherThorsHammerTask(int ownerID) : base(ownerID) 	{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ThorsHammer;	}
	}

	// *** Military Units - Tiger ***
	public class BuildTigerAcidCloudTask : BuildVehicleTask
	{
		public BuildTigerAcidCloudTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.AcidCloud;		}
	}

	public class BuildTigerEMPTask : BuildVehicleTask
	{
		public BuildTigerEMPTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.EMP;			}
	}

	public class BuildTigerLaserTask : BuildVehicleTask
	{
		public BuildTigerLaserTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Laser;			}
	}

	public class BuildTigerMicrowaveTask : BuildVehicleTask
	{
		public BuildTigerMicrowaveTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Microwave;		}
	}

	public class BuildTigerRailGunTask : BuildVehicleTask
	{
		public BuildTigerRailGunTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RailGun;		}
	}

	public class BuildTigerRPGTask : BuildVehicleTask
	{
		public BuildTigerRPGTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RPG;			}
	}

	public class BuildTigerStarflareTask : BuildVehicleTask
	{
		public BuildTigerStarflareTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Starflare;		}
	}

	public class BuildTigerSupernovaTask : BuildVehicleTask
	{
		public BuildTigerSupernovaTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Supernova;		}
	}

	public class BuildTigerESGTask : BuildVehicleTask
	{
		public BuildTigerESGTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ESG;			}
	}

	public class BuildTigerStickyfoamTask : BuildVehicleTask
	{
		public BuildTigerStickyfoamTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Stickyfoam;	}
	}

	public class BuildTigerThorsHammerTask : BuildVehicleTask
	{
		public BuildTigerThorsHammerTask(int ownerID) : base(ownerID)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ThorsHammer;	}
	}
}
