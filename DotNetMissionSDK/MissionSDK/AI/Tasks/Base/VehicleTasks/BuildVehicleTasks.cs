using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Snapshot;

namespace DotNetMissionSDK.AI.Tasks.Base.VehicleTasks
{
	public class BuildResearchedVehicleTask : BuildVehicleTask
	{
		public BuildResearchedVehicleTask(int ownerID) : base(ownerID)		{ 	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();

			AddPrerequisite(new ResearchTask(ownerID, new UnitInfo(m_VehicleToBuild).GetResearchTopic()));

			if (m_VehicleToBuildCargo != map_id.None)
				AddPrerequisite(new ResearchTask(ownerID, new UnitInfo(m_VehicleToBuildCargo).GetResearchTopic()));
		}
	}

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

	public class BuildGeoConTask : BuildResearchedVehicleTask
	{
		public BuildGeoConTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.GeoCon;						}
	}

	public class BuildRepairVehicleTask : BuildResearchedVehicleTask
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
	public class BuildLynxAcidCloudTask : BuildResearchedVehicleTask
	{
		public BuildLynxAcidCloudTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.AcidCloud;		}
	}

	public class BuildLynxEMPTask : BuildResearchedVehicleTask
	{
		public BuildLynxEMPTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.EMP;			}
	}

	public class BuildLynxLaserTask : BuildResearchedVehicleTask
	{
		public BuildLynxLaserTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Laser;			}
	}

	public class BuildLynxMicrowaveTask : BuildResearchedVehicleTask
	{
		public BuildLynxMicrowaveTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Microwave;		}
	}

	public class BuildLynxRailGunTask : BuildResearchedVehicleTask
	{
		public BuildLynxRailGunTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RailGun;		}
	}

	public class BuildLynxRPGTask : BuildResearchedVehicleTask
	{
		public BuildLynxRPGTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RPG;			}
	}

	public class BuildLynxStarflareTask : BuildResearchedVehicleTask
	{
		public BuildLynxStarflareTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Starflare;		}
	}

	public class BuildLynxSupernovaTask : BuildResearchedVehicleTask
	{
		public BuildLynxSupernovaTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Supernova;		}
	}

	public class BuildLynxESGTask : BuildResearchedVehicleTask
	{
		public BuildLynxESGTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ESG;			}
	}

	public class BuildLynxStickyfoamTask : BuildResearchedVehicleTask
	{
		public BuildLynxStickyfoamTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Stickyfoam;	}
	}

	public class BuildLynxThorsHammerTask : BuildResearchedVehicleTask
	{
		public BuildLynxThorsHammerTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ThorsHammer;	}
	}

	// *** Military Units - Panther ***
	public class BuildPantherAcidCloudTask : BuildResearchedVehicleTask
	{
		public BuildPantherAcidCloudTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.AcidCloud;		}
	}

	public class BuildPantherEMPTask : BuildResearchedVehicleTask
	{
		public BuildPantherEMPTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.EMP;			}
	}

	public class BuildPantherLaserTask : BuildResearchedVehicleTask
	{
		public BuildPantherLaserTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Laser;			}
	}

	public class BuildPantherMicrowaveTask : BuildResearchedVehicleTask
	{
		public BuildPantherMicrowaveTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Microwave;		}
	}

	public class BuildPantherRailGunTask : BuildResearchedVehicleTask
	{
		public BuildPantherRailGunTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RailGun;		}
	}

	public class BuildPantherRPGTask : BuildResearchedVehicleTask
	{
		public BuildPantherRPGTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RPG;			}
	}

	public class BuildPantherStarflareTask : BuildResearchedVehicleTask
	{
		public BuildPantherStarflareTask(int ownerID) : base(ownerID)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Starflare;		}
	}

	public class BuildPantherSupernovaTask : BuildResearchedVehicleTask
	{
		public BuildPantherSupernovaTask(int ownerID) : base(ownerID)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Supernova;		}
	}

	public class BuildPantherESGTask : BuildResearchedVehicleTask
	{
		public BuildPantherESGTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ESG;			}
	}

	public class BuildPantherStickyfoamTask : BuildResearchedVehicleTask
	{
		public BuildPantherStickyfoamTask(int ownerID) : base(ownerID)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Stickyfoam;	}
	}

	public class BuildPantherThorsHammerTask : BuildResearchedVehicleTask
	{
		public BuildPantherThorsHammerTask(int ownerID) : base(ownerID) 	{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ThorsHammer;	}
	}

	// *** Military Units - Tiger ***
	public class BuildTigerAcidCloudTask : BuildResearchedVehicleTask
	{
		public BuildTigerAcidCloudTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.AcidCloud;		}
	}

	public class BuildTigerEMPTask : BuildResearchedVehicleTask
	{
		public BuildTigerEMPTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.EMP;			}
	}

	public class BuildTigerLaserTask : BuildResearchedVehicleTask
	{
		public BuildTigerLaserTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Laser;			}
	}

	public class BuildTigerMicrowaveTask : BuildResearchedVehicleTask
	{
		public BuildTigerMicrowaveTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Microwave;		}
	}

	public class BuildTigerRailGunTask : BuildResearchedVehicleTask
	{
		public BuildTigerRailGunTask(int ownerID) : base(ownerID) 			{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RailGun;		}
	}

	public class BuildTigerRPGTask : BuildResearchedVehicleTask
	{
		public BuildTigerRPGTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.RPG;			}
	}

	public class BuildTigerStarflareTask : BuildResearchedVehicleTask
	{
		public BuildTigerStarflareTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Starflare;		}
	}

	public class BuildTigerSupernovaTask : BuildResearchedVehicleTask
	{
		public BuildTigerSupernovaTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Supernova;		}
	}

	public class BuildTigerESGTask : BuildResearchedVehicleTask
	{
		public BuildTigerESGTask(int ownerID) : base(ownerID) 				{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ESG;			}
	}

	public class BuildTigerStickyfoamTask : BuildResearchedVehicleTask
	{
		public BuildTigerStickyfoamTask(int ownerID) : base(ownerID) 		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.Stickyfoam;	}
	}

	public class BuildTigerThorsHammerTask : BuildResearchedVehicleTask
	{
		public BuildTigerThorsHammerTask(int ownerID) : base(ownerID)		{ m_VehicleToBuild = map_id.Lynx; m_VehicleToBuildCargo = map_id.ThorsHammer;	}
	}
}
