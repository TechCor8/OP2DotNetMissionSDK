
namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	public sealed class ConnectStructureFactoryTask : ConnectStructureTask
	{
		public ConnectStructureFactoryTask(int ownerID) : base(ownerID)		{ m_StructureToConnect = map_id.StructureFactory;				}
	}

	public sealed class ConnectCommonSmelterTask : ConnectStructureTask
	{
		public ConnectCommonSmelterTask(int ownerID) : base(ownerID)		{ m_StructureToConnect = map_id.CommonOreSmelter;				}
	}

	public sealed class ConnectRareSmelterTask : ConnectStructureTask
	{
		public ConnectRareSmelterTask(int ownerID) : base(ownerID)			{ m_StructureToConnect = map_id.RareOreSmelter;					}
	}

	public sealed class ConnectAgridomeTask : ConnectStructureTask
	{
		public ConnectAgridomeTask(int ownerID) : base(ownerID)				{ m_StructureToConnect = map_id.Agridome;						}
	}

	public sealed class ConnectTokamakTask : ConnectStructureTask
	{
		public ConnectTokamakTask(int ownerID) : base(ownerID)				{ m_StructureToConnect = map_id.Tokamak;						}
	}

	public sealed class ConnectMHDGeneratorTask : ConnectStructureTask
	{
		public ConnectMHDGeneratorTask(int ownerID) : base(ownerID)			{ m_StructureToConnect = map_id.MHDGenerator;					}
	}

	public sealed class ConnectSolarArrayTask : ConnectStructureTask
	{
		public ConnectSolarArrayTask(int ownerID) : base(ownerID)			{ m_StructureToConnect = map_id.SolarPowerArray;				}
	}

	public sealed class ConnectNurseryTask : ConnectStructureTask
	{
		public ConnectNurseryTask(int ownerID) : base(ownerID)				{ m_StructureToConnect = map_id.Nursery;						}
	}

	public sealed class ConnectUniversityTask : ConnectStructureTask
	{
		public ConnectUniversityTask(int ownerID) : base(ownerID)			{ m_StructureToConnect = map_id.University;						}
	}

	public sealed class ConnectResidenceTask : ConnectStructureTask
	{
		public ConnectResidenceTask(int ownerID) : base(ownerID)			{ m_StructureToConnect = map_id.Residence;						}
	}

	public sealed class ConnectReinforcedResidenceTask : ConnectStructureTask
	{
		public ConnectReinforcedResidenceTask(int ownerID) : base(ownerID)	{ m_StructureToConnect = map_id.ReinforcedResidence;			}
	}

	public sealed class ConnectAdvancedResidenceTask : ConnectStructureTask
	{
		public ConnectAdvancedResidenceTask(int ownerID) : base(ownerID)	{ m_StructureToConnect = map_id.AdvancedResidence;				}
	}

	public sealed class ConnectMedicalCenterTask : ConnectStructureTask
	{
		public ConnectMedicalCenterTask(int ownerID) : base(ownerID)		{ m_StructureToConnect = map_id.MedicalCenter;					}
	}

	public sealed class ConnectDIRTTask : ConnectStructureTask
	{
		public ConnectDIRTTask(int ownerID) : base(ownerID)					{ m_StructureToConnect = map_id.DIRT;							}
	}

	public sealed class ConnectRecreationTask : ConnectStructureTask
	{
		public ConnectRecreationTask(int ownerID) : base(ownerID)			{ m_StructureToConnect = map_id.RecreationFacility;				}
	}

	public sealed class ConnectForumTask : ConnectStructureTask
	{
		public ConnectForumTask(int ownerID) : base(ownerID)				{ m_StructureToConnect = map_id.Forum;							}
	}

	public sealed class ConnectGORFTask : ConnectStructureTask
	{
		public ConnectGORFTask(int ownerID) : base(ownerID)					{ m_StructureToConnect = map_id.GORF;							}
	}

	public sealed class ConnectVehicleFactoryTask : ConnectStructureTask
	{
		public ConnectVehicleFactoryTask(int ownerID) : base(ownerID)		{ m_StructureToConnect = map_id.VehicleFactory;					}
	}

	public sealed class ConnectArachnidFactoryTask : ConnectStructureTask
	{
		public ConnectArachnidFactoryTask(int ownerID) : base(ownerID)		{ m_StructureToConnect = map_id.ArachnidFactory;				}
	}

	public sealed class ConnectBasicLabTask : ConnectStructureTask
	{
		public ConnectBasicLabTask(int ownerID) : base(ownerID)				{ m_StructureToConnect = map_id.BasicLab;						}
	}

	public sealed class ConnectStandardLabTask : ConnectStructureTask
	{
		public ConnectStandardLabTask(int ownerID) : base(ownerID)			{ m_StructureToConnect = map_id.StandardLab;					}
	}

	public sealed class ConnectAdvancedLabTask : ConnectStructureTask
	{
		public ConnectAdvancedLabTask(int ownerID) : base(ownerID)			{ m_StructureToConnect = map_id.AdvancedLab;					}
	}

	public sealed class ConnectRobotCommandTask : ConnectStructureTask
	{
		public ConnectRobotCommandTask(int ownerID) : base(ownerID)			{ m_StructureToConnect = map_id.RobotCommand;					}
	}

	public sealed class ConnectSpaceportTask : ConnectStructureTask
	{
		public ConnectSpaceportTask(int ownerID) : base(ownerID)			{ m_StructureToConnect = map_id.Spaceport;						}
	}

	public sealed class ConnectObservatoryTask : ConnectStructureTask
	{
		public ConnectObservatoryTask(int ownerID) : base(ownerID)			{ m_StructureToConnect = map_id.Observatory;					}
	}

	public sealed class ConnectMeteorDefenseTask : ConnectStructureTask
	{
		public ConnectMeteorDefenseTask(int ownerID) : base(ownerID)		{ m_StructureToConnect = map_id.MeteorDefense;					}
	}

	public sealed class ConnectGuardPostTask : ConnectStructureTask
	{
		public ConnectGuardPostTask(int ownerID) : base(ownerID)			{ m_StructureToConnect = map_id.GuardPost;						}
	}
}
