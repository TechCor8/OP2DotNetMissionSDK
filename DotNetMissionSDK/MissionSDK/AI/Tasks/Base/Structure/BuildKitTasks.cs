using DotNetMissionSDK.Async;
using DotNetMissionSDK.State.Snapshot;

namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	public sealed class BuildCommandCenterKitTask : BuildStructureKitTask
	{
		public BuildCommandCenterKitTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.CommandCenter;					}
	}

	public sealed class BuildStructureFactoryKitTask : BuildStructureKitTask
	{
		public BuildStructureFactoryKitTask(int ownerID) : base(ownerID)	{ m_KitToBuild = map_id.StructureFactory;				}
	}

	public sealed class BuildCommonSmelterKitTask : BuildStructureKitTask
	{
		public BuildCommonSmelterKitTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.CommonOreSmelter;				}
	}

	public sealed class BuildRareSmelterKitTask : BuildStructureKitTask
	{
		public BuildRareSmelterKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.RareOreSmelter;					}
	}

	public sealed class BuildAgridomeKitTask : BuildStructureKitTask
	{
		public BuildAgridomeKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.Agridome;						}
	}

	public sealed class BuildTokamakKitTask : BuildStructureKitTask
	{
		public BuildTokamakKitTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.Tokamak;						}
	}

	public sealed class BuildMHDGeneratorKitTask : BuildStructureKitTask
	{
		public BuildMHDGeneratorKitTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.MHDGenerator;					}
	}

	public sealed class BuildSolarArrayKitTask : BuildStructureKitTask
	{
		public BuildSolarArrayKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.SolarPowerArray;				}
	}

	public sealed class BuildNurseryKitTask : BuildStructureKitTask
	{
		public BuildNurseryKitTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.Nursery;						}
	}

	public sealed class BuildUniversityKitTask : BuildStructureKitTask
	{
		public BuildUniversityKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.University;						}
	}

	public sealed class BuildResidenceKitTask : BuildStructureKitTask
	{
		public BuildResidenceKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.Residence;						}
	}

	public sealed class BuildReinforcedResidenceKitTask : BuildStructureKitTask
	{
		public BuildReinforcedResidenceKitTask(int ownerID) : base(ownerID)	{ m_KitToBuild = map_id.ReinforcedResidence;			}
	}

	public sealed class BuildAdvancedResidenceKitTask : BuildStructureKitTask
	{
		public BuildAdvancedResidenceKitTask(int ownerID) : base(ownerID)	{ m_KitToBuild = map_id.AdvancedResidence;				}
	}

	public sealed class BuildMedicalCenterKitTask : BuildStructureKitTask
	{
		public BuildMedicalCenterKitTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.MedicalCenter;					}
	}

	public sealed class BuildDIRTKitTask : BuildStructureKitTask
	{
		public BuildDIRTKitTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.DIRT;							}
	}

	public sealed class BuildRecreationKitTask : BuildStructureKitTask
	{
		public BuildRecreationKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.RecreationFacility;				}
	}

	public sealed class BuildForumKitTask : BuildStructureKitTask
	{
		public BuildForumKitTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.Forum;							}
	}

	public sealed class BuildGORFKitTask : BuildStructureKitTask
	{
		public BuildGORFKitTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.GORF;							}
	}

	public sealed class BuildVehicleFactoryKitTask : BuildStructureKitTask
	{
		public BuildVehicleFactoryKitTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.VehicleFactory;					}
	}

	public sealed class BuildArachnidFactoryKitTask : BuildStructureKitTask
	{
		public BuildArachnidFactoryKitTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.ArachnidFactory;				}
	}

	public sealed class BuildStandardLabKitTask : BuildStructureKitTask
	{
		public BuildStandardLabKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.StandardLab;					}
	}

	public sealed class BuildAdvancedLabKitTask : BuildStructureKitTask
	{
		public BuildAdvancedLabKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.AdvancedLab;					}
	}

	public sealed class BuildRobotCommandKitTask : BuildStructureKitTask
	{
		public BuildRobotCommandKitTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.RobotCommand;					}
	}

	public sealed class BuildSpaceportKitTask : BuildStructureKitTask
	{
		public BuildSpaceportKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.Spaceport;						}
	}

	public sealed class BuildObservatoryKitTask : BuildStructureKitTask
	{
		public BuildObservatoryKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.Observatory;					}
	}

	public sealed class BuildMeteorDefenseKitTask : BuildStructureKitTask
	{
		public BuildMeteorDefenseKitTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.MeteorDefense;					}
	}

	public sealed class BuildGuardPostKitTask : BuildStructureKitTask
	{
		public BuildGuardPostKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.GuardPost;						}

		public map_id turret								{ get { return m_KitToBuildCargo; } set { m_KitToBuildCargo = value; }	}

		public void RandomizeTurret(bool isEden, bool includeLineOfSight=true, bool includeBombs=false)
		{
			if (isEden)
			{
				int min = includeLineOfSight ? 0 : 2;
				int max = includeBombs ? 6 : 5;

				int rand = AsyncRandom.Range(min, max);
				switch (rand)
				{
					case 0:		m_KitToBuildCargo = map_id.Laser;		break;
					case 1:		m_KitToBuildCargo = map_id.RailGun;		break;
					case 2:		m_KitToBuildCargo = map_id.AcidCloud;	break;
					case 3:		m_KitToBuildCargo = map_id.EMP;			break;
					case 4:		m_KitToBuildCargo = map_id.ThorsHammer;	break;
					case 5:		m_KitToBuildCargo = map_id.Starflare2;	break;
				}
			}
			else
			{
				int min = includeLineOfSight ? 0 : 2;
				int max = includeBombs ? 6 : 5;

				int rand = AsyncRandom.Range(min, max);
				switch (rand)
				{
					case 0:		m_KitToBuildCargo = map_id.Microwave;	break;
					case 1:		m_KitToBuildCargo = map_id.EMP;			break;
					case 2:		m_KitToBuildCargo = map_id.RPG;			break;
					case 3:		m_KitToBuildCargo = map_id.ESG;			break;
					case 4:		m_KitToBuildCargo = map_id.Stickyfoam;	break;
					case 5:		m_KitToBuildCargo = map_id.Starflare2;	break;
					case 6:		m_KitToBuildCargo = map_id.Supernova2;	break;
				}
			}
		}
	}
}
