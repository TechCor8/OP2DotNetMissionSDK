
namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	public sealed class RepairCommandCenterTask : RepairStructureTask
	{
		public RepairCommandCenterTask(int ownerID) : base(ownerID)			{ m_StructureToRepair = map_id.CommandCenter;					}
	}

	public sealed class RepairStructureFactoryTask : RepairStructureTask
	{
		public RepairStructureFactoryTask(int ownerID) : base(ownerID)		{ m_StructureToRepair = map_id.StructureFactory;				}
	}

	public sealed class RepairCommonSmelterTask : RepairStructureTask
	{
		public RepairCommonSmelterTask(int ownerID) : base(ownerID)			{ m_StructureToRepair = map_id.CommonOreSmelter;				}
	}

	public sealed class RepairRareSmelterTask : RepairStructureTask
	{
		public RepairRareSmelterTask(int ownerID) : base(ownerID)			{ m_StructureToRepair = map_id.RareOreSmelter;					}
	}

	public sealed class RepairCommonMineTask : RepairStructureTask
	{
		public RepairCommonMineTask(int ownerID) : base(ownerID)			{ m_StructureToRepair = map_id.CommonOreMine;					}
	}

	public sealed class RepairRareMineTask : RepairStructureTask
	{
		public RepairRareMineTask(int ownerID) : base(ownerID)				{ m_StructureToRepair = map_id.RareOreMine;						}
	}

	public sealed class RepairAgridomeTask : RepairStructureTask
	{
		public RepairAgridomeTask(int ownerID) : base(ownerID)				{ m_StructureToRepair = map_id.Agridome;						}
	}

	public sealed class RepairTokamakTask : RepairStructureTask
	{
		public RepairTokamakTask(int ownerID) : base(ownerID)				{ m_StructureToRepair = map_id.Tokamak;							}
	}

	public sealed class RepairMHDGeneratorTask : RepairStructureTask
	{
		public RepairMHDGeneratorTask(int ownerID) : base(ownerID)			{ m_StructureToRepair = map_id.MHDGenerator;					}
	}

	public sealed class RepairSolarArrayTask : RepairStructureTask
	{
		public RepairSolarArrayTask(int ownerID) : base(ownerID)			{ m_StructureToRepair = map_id.SolarPowerArray;					}
	}

	public sealed class RepairNurseryTask : RepairStructureTask
	{
		public RepairNurseryTask(int ownerID) : base(ownerID)				{ m_StructureToRepair = map_id.Nursery;							}
	}

	public sealed class RepairUniversityTask : RepairStructureTask
	{
		public RepairUniversityTask(int ownerID) : base(ownerID)			{ m_StructureToRepair = map_id.University;						}
	}

	public sealed class RepairResidenceTask : RepairStructureTask
	{
		public RepairResidenceTask(int ownerID) : base(ownerID)				{ m_StructureToRepair = map_id.Residence;						}
	}

	public sealed class RepairReinforcedResidenceTask : RepairStructureTask
	{
		public RepairReinforcedResidenceTask(int ownerID) : base(ownerID)	{ m_StructureToRepair = map_id.ReinforcedResidence;				}
	}

	public sealed class RepairAdvancedResidenceTask : RepairStructureTask
	{
		public RepairAdvancedResidenceTask(int ownerID) : base(ownerID)		{ m_StructureToRepair = map_id.AdvancedResidence;				}
	}

	public sealed class RepairMedicalCenterTask : RepairStructureTask
	{
		public RepairMedicalCenterTask(int ownerID) : base(ownerID)			{ m_StructureToRepair = map_id.MedicalCenter;					}
	}

	public sealed class RepairDIRTTask : RepairStructureTask
	{
		public RepairDIRTTask(int ownerID) : base(ownerID)					{ m_StructureToRepair = map_id.DIRT;							}
	}

	public sealed class RepairRecreationTask : RepairStructureTask
	{
		public RepairRecreationTask(int ownerID) : base(ownerID)			{ m_StructureToRepair = map_id.RecreationFacility;				}
	}

	public sealed class RepairForumTask : RepairStructureTask
	{
		public RepairForumTask(int ownerID) : base(ownerID)					{ m_StructureToRepair = map_id.Forum;							}
	}

	public sealed class RepairGORFTask : RepairStructureTask
	{
		public RepairGORFTask(int ownerID) : base(ownerID)					{ m_StructureToRepair = map_id.GORF;							}
	}

	public sealed class RepairVehicleFactoryTask : RepairStructureTask
	{
		public RepairVehicleFactoryTask(int ownerID) : base(ownerID)		{ m_StructureToRepair = map_id.VehicleFactory;					}
	}

	public sealed class RepairArachnidFactoryTask : RepairStructureTask
	{
		public RepairArachnidFactoryTask(int ownerID) : base(ownerID)		{ m_StructureToRepair = map_id.ArachnidFactory;					}
	}

	public sealed class RepairBasicLabTask : RepairStructureTask
	{
		public RepairBasicLabTask(int ownerID) : base(ownerID)				{ m_StructureToRepair = map_id.BasicLab;						}
	}

	public sealed class RepairStandardLabTask : RepairStructureTask
	{
		public RepairStandardLabTask(int ownerID) : base(ownerID)			{ m_StructureToRepair = map_id.StandardLab;						}
	}

	public sealed class RepairAdvancedLabTask : RepairStructureTask
	{
		public RepairAdvancedLabTask(int ownerID) : base(ownerID)			{ m_StructureToRepair = map_id.AdvancedLab;						}
	}

	public sealed class RepairRobotCommandTask : RepairStructureTask
	{
		public RepairRobotCommandTask(int ownerID) : base(ownerID)			{ m_StructureToRepair = map_id.RobotCommand;					}
	}

	public sealed class RepairSpaceportTask : RepairStructureTask
	{
		public RepairSpaceportTask(int ownerID) : base(ownerID)				{ m_StructureToRepair = map_id.Spaceport;						}
	}

	public sealed class RepairObservatoryTask : RepairStructureTask
	{
		public RepairObservatoryTask(int ownerID) : base(ownerID)			{ m_StructureToRepair = map_id.Observatory;						}
	}

	public sealed class RepairMeteorDefenseTask : RepairStructureTask
	{
		public RepairMeteorDefenseTask(int ownerID) : base(ownerID)			{ m_StructureToRepair = map_id.MeteorDefense;					}
	}

	public sealed class RepairGuardPostTask : RepairStructureTask
	{
		public RepairGuardPostTask(int ownerID) : base(ownerID)				{ m_StructureToRepair = map_id.GuardPost;						}
	}
}
