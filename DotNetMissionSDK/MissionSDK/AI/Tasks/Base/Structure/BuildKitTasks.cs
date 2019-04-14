using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	public sealed class BuildCommandCenterKitTask : BuildStructureKitTask
	{
		public BuildCommandCenterKitTask()									{ m_KitToBuild = map_id.CommandCenter;					}
		public BuildCommandCenterKitTask(PlayerInfo owner) : base(owner)	{ m_KitToBuild = map_id.CommandCenter;					}
	}

	public sealed class BuildStructureFactoryKitTask : BuildStructureKitTask
	{
		public BuildStructureFactoryKitTask()								{ m_KitToBuild = map_id.StructureFactory;				}
		public BuildStructureFactoryKitTask(PlayerInfo owner) : base(owner)	{ m_KitToBuild = map_id.StructureFactory;				}
	}

	public sealed class BuildCommonSmelterKitTask : BuildStructureKitTask
	{
		public BuildCommonSmelterKitTask()									{ m_KitToBuild = map_id.CommonOreSmelter;				}
		public BuildCommonSmelterKitTask(PlayerInfo owner) : base(owner)	{ m_KitToBuild = map_id.CommonOreSmelter;				}
	}

	public sealed class BuildRareSmelterKitTask : BuildStructureKitTask
	{
		public BuildRareSmelterKitTask()									{ m_KitToBuild = map_id.RareOreSmelter;					}
		public BuildRareSmelterKitTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.RareOreSmelter;					}
	}

	public sealed class BuildAgridomeKitTask : BuildStructureKitTask
	{
		public BuildAgridomeKitTask()										{ m_KitToBuild = map_id.Agridome;						}
		public BuildAgridomeKitTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.Agridome;						}
	}

	public sealed class BuildTokamakKitTask : BuildStructureKitTask
	{
		public BuildTokamakKitTask()										{ m_KitToBuild = map_id.Tokamak;						}
		public BuildTokamakKitTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.Tokamak;						}
	}

	public sealed class BuildMHDGeneratorKitTask : BuildStructureKitTask
	{
		public BuildMHDGeneratorKitTask()									{ m_KitToBuild = map_id.MHDGenerator;					}
		public BuildMHDGeneratorKitTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.MHDGenerator;					}
	}

	public sealed class BuildSolarArrayKitTask : BuildStructureKitTask
	{
		public BuildSolarArrayKitTask()										{ m_KitToBuild = map_id.SolarPowerArray;				}
		public BuildSolarArrayKitTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.SolarPowerArray;				}
	}

	public sealed class BuildNurseryKitTask : BuildStructureKitTask
	{
		public BuildNurseryKitTask()										{ m_KitToBuild = map_id.Nursery;						}
		public BuildNurseryKitTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.Nursery;						}
	}

	public sealed class BuildUniversityKitTask : BuildStructureKitTask
	{
		public BuildUniversityKitTask()										{ m_KitToBuild = map_id.University;						}
		public BuildUniversityKitTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.University;						}
	}

	public sealed class BuildResidenceKitTask : BuildStructureKitTask
	{
		public BuildResidenceKitTask()										{ m_KitToBuild = map_id.Residence;						}
		public BuildResidenceKitTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.Residence;						}
	}

	public sealed class BuildReinforcedResidenceKitTask : BuildStructureKitTask
	{
		public BuildReinforcedResidenceKitTask()								{ m_KitToBuild = map_id.ReinforcedResidence;		}
		public BuildReinforcedResidenceKitTask(PlayerInfo owner) : base(owner)	{ m_KitToBuild = map_id.ReinforcedResidence;		}
	}

	public sealed class BuildAdvancedResidenceKitTask : BuildStructureKitTask
	{
		public BuildAdvancedResidenceKitTask()								{ m_KitToBuild = map_id.AdvancedResidence;				}
		public BuildAdvancedResidenceKitTask(PlayerInfo owner) : base(owner){ m_KitToBuild = map_id.AdvancedResidence;				}
	}

	public sealed class BuildMedicalCenterKitTask : BuildStructureKitTask
	{
		public BuildMedicalCenterKitTask()									{ m_KitToBuild = map_id.MedicalCenter;					}
		public BuildMedicalCenterKitTask(PlayerInfo owner) : base(owner)	{ m_KitToBuild = map_id.MedicalCenter;					}
	}

	public sealed class BuildDIRTKitTask : BuildStructureKitTask
	{
		public BuildDIRTKitTask()											{ m_KitToBuild = map_id.DIRT;							}
		public BuildDIRTKitTask(PlayerInfo owner) : base(owner)				{ m_KitToBuild = map_id.DIRT;							}
	}

	public sealed class BuildRecreationKitTask : BuildStructureKitTask
	{
		public BuildRecreationKitTask()										{ m_KitToBuild = map_id.RecreationFacility;				}
		public BuildRecreationKitTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.RecreationFacility;				}
	}

	public sealed class BuildForumKitTask : BuildStructureKitTask
	{
		public BuildForumKitTask()											{ m_KitToBuild = map_id.Forum;							}
		public BuildForumKitTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.Forum;							}
	}

	public sealed class BuildGORFKitTask : BuildStructureKitTask
	{
		public BuildGORFKitTask()											{ m_KitToBuild = map_id.GORF;							}
		public BuildGORFKitTask(PlayerInfo owner) : base(owner)				{ m_KitToBuild = map_id.GORF;							}
	}

	public sealed class BuildVehicleFactoryKitTask : BuildStructureKitTask
	{
		public BuildVehicleFactoryKitTask()									{ m_KitToBuild = map_id.VehicleFactory;					}
		public BuildVehicleFactoryKitTask(PlayerInfo owner) : base(owner)	{ m_KitToBuild = map_id.VehicleFactory;					}
	}

	public sealed class BuildStandardLabKitTask : BuildStructureKitTask
	{
		public BuildStandardLabKitTask()									{ m_KitToBuild = map_id.StandardLab;					}
		public BuildStandardLabKitTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.StandardLab;					}
	}

	public sealed class BuildAdvancedLabKitTask : BuildStructureKitTask
	{
		public BuildAdvancedLabKitTask()									{ m_KitToBuild = map_id.AdvancedLab;					}
		public BuildAdvancedLabKitTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.AdvancedLab;					}
	}

	public sealed class BuildRobotCommandKitTask : BuildStructureKitTask
	{
		public BuildRobotCommandKitTask()									{ m_KitToBuild = map_id.RobotCommand;					}
		public BuildRobotCommandKitTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.RobotCommand;					}
	}

	public sealed class BuildSpaceportKitTask : BuildStructureKitTask
	{
		public BuildSpaceportKitTask()										{ m_KitToBuild = map_id.Spaceport;						}
		public BuildSpaceportKitTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.Spaceport;						}
	}

	public sealed class BuildObservatoryKitTask : BuildStructureKitTask
	{
		public BuildObservatoryKitTask()									{ m_KitToBuild = map_id.Observatory;					}
		public BuildObservatoryKitTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.Observatory;					}
	}

	public sealed class BuildMeteorDefenseKitTask : BuildStructureKitTask
	{
		public BuildMeteorDefenseKitTask()									{ m_KitToBuild = map_id.MeteorDefense;					}
		public BuildMeteorDefenseKitTask(PlayerInfo owner) : base(owner)	{ m_KitToBuild = map_id.MeteorDefense;					}
	}

	public sealed class BuildGuardPostKitTask : BuildStructureKitTask
	{
		public BuildGuardPostKitTask()										{ m_KitToBuild = map_id.GuardPost;						}
		public BuildGuardPostKitTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.GuardPost;						}

		public void SetTurret(map_id turret)								{ m_KitToBuildCargo = turret;							}

		public void RandomizeTurret(bool includeLineOfSight=true, bool includeBombs=false)
		{
			if (owner.player.IsEden())
			{
				int min = includeLineOfSight ? 0 : 2;
				int max = includeBombs ? 6 : 5;

				switch (TethysGame.GetRandomRange(min, max))
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

				switch (TethysGame.GetRandomRange(min, max))
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
