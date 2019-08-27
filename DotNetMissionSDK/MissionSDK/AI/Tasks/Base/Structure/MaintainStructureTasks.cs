
namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	public sealed class MaintainStructureFactoryTask : MaintainStructureTask
	{
		public MaintainStructureFactoryTask(int ownerID) : base(ownerID)		{ m_StructureToMaintain = map_id.StructureFactory;				}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectStructureFactoryTask(ownerID), true);
			AddPrerequisite(new RepairStructureFactoryTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildStructureFactoryTask(ownerID));
		}
	}

	public sealed class MaintainCommonSmelterTask : MaintainStructureTask
	{
		public MaintainCommonSmelterTask(int ownerID) : base(ownerID)			{ m_StructureToMaintain = map_id.CommonOreSmelter;				}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectCommonSmelterTask(ownerID), true);
			AddPrerequisite(new RepairCommonSmelterTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildCommonSmelterTask(ownerID));
		}
	}

	public sealed class MaintainAgridomeTask : MaintainStructureTask
	{
		public MaintainAgridomeTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.Agridome;						}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectAgridomeTask(ownerID), true);
			AddPrerequisite(new RepairAgridomeTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildAgridomeTask(ownerID));
		}
	}

	public sealed class MaintainTokamakTask : MaintainStructureTask
	{
		public MaintainTokamakTask(int ownerID) : base(ownerID)					{ m_StructureToMaintain = map_id.Tokamak;						}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectTokamakTask(ownerID), true);
			AddPrerequisite(new RepairTokamakTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildTokamakTask(ownerID));
		}
	}

	public sealed class MaintainMHDGeneratorTask : MaintainStructureTask
	{
		public MaintainMHDGeneratorTask(int ownerID) : base(ownerID)			{ m_StructureToMaintain = map_id.MHDGenerator;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectMHDGeneratorTask(ownerID), true);
			AddPrerequisite(new RepairMHDGeneratorTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildMHDGeneratorTask(ownerID));
		}
	}

	public sealed class MaintainSolarArrayTask : MaintainStructureTask
	{
		public MaintainSolarArrayTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.SolarPowerArray;				}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectSolarArrayTask(ownerID), true);
			AddPrerequisite(new RepairSolarArrayTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildSolarArrayTask(ownerID));
		}
	}

	public sealed class MaintainNurseryTask : MaintainStructureTask
	{
		public MaintainNurseryTask(int ownerID) : base(ownerID)					{ m_StructureToMaintain = map_id.Nursery;						}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectNurseryTask(ownerID), true);
			AddPrerequisite(new RepairNurseryTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildNurseryTask(ownerID));
		}
	}

	public sealed class MaintainUniversityTask : MaintainStructureTask
	{
		public MaintainUniversityTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.University;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectUniversityTask(ownerID), true);
			AddPrerequisite(new RepairUniversityTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildUniversityTask(ownerID));
		}
	}

	public sealed class MaintainResidenceTask : MaintainStructureTask
	{
		public MaintainResidenceTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.Residence;						}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectResidenceTask(ownerID), true);
			AddPrerequisite(new RepairResidenceTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildResidenceTask(ownerID));
		}
	}

	public sealed class MaintainReinforcedResidenceTask : MaintainStructureTask
	{
		public MaintainReinforcedResidenceTask(int ownerID) : base(ownerID)		{ m_StructureToMaintain = map_id.ReinforcedResidence;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectReinforcedResidenceTask(ownerID), true);
			AddPrerequisite(new RepairReinforcedResidenceTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildReinforcedResidenceTask(ownerID));
		}
	}

	public sealed class MaintainAdvancedResidenceTask : MaintainStructureTask
	{
		public MaintainAdvancedResidenceTask(int ownerID) : base(ownerID)		{ m_StructureToMaintain = map_id.AdvancedResidence;				}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectAdvancedResidenceTask(ownerID), true);
			AddPrerequisite(new RepairAdvancedResidenceTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildAdvancedResidenceTask(ownerID));
		}
	}

	public sealed class MaintainMedicalCenterTask : MaintainStructureTask
	{
		public MaintainMedicalCenterTask(int ownerID) : base(ownerID)			{ m_StructureToMaintain = map_id.MedicalCenter;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectMedicalCenterTask(ownerID), true);
			AddPrerequisite(new RepairMedicalCenterTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildMedicalCenterTask(ownerID));
		}
	}

	public sealed class MaintainDIRTTask : MaintainStructureTask
	{
		public MaintainDIRTTask(int ownerID) : base(ownerID)					{ m_StructureToMaintain = map_id.DIRT;							}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectDIRTTask(ownerID), true);
			AddPrerequisite(new RepairDIRTTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildDIRTTask(ownerID));
		}
	}

	public sealed class MaintainRecreationTask : MaintainStructureTask
	{
		public MaintainRecreationTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.RecreationFacility;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectRecreationTask(ownerID), true);
			AddPrerequisite(new RepairRecreationTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildRecreationTask(ownerID));
		}
	}

	public sealed class MaintainForumTask : MaintainStructureTask
	{
		public MaintainForumTask(int ownerID) : base(ownerID)					{ m_StructureToMaintain = map_id.Forum;							}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectForumTask(ownerID), true);
			AddPrerequisite(new RepairForumTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildForumTask(ownerID));
		}
	}

	public sealed class MaintainGORFTask : MaintainStructureTask
	{
		public MaintainGORFTask(int ownerID) : base(ownerID)					{ m_StructureToMaintain = map_id.GORF;							}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectGORFTask(ownerID), true);
			AddPrerequisite(new RepairGORFTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildGORFTask(ownerID));
		}
	}

	public sealed class MaintainVehicleFactoryTask : MaintainStructureTask
	{
		public MaintainVehicleFactoryTask(int ownerID) : base(ownerID)			{ m_StructureToMaintain = map_id.VehicleFactory;				}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectVehicleFactoryTask(ownerID), true);
			AddPrerequisite(new RepairVehicleFactoryTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildVehicleFactoryTask(ownerID));
		}
	}

	public sealed class MaintainArachnidFactoryTask : MaintainStructureTask
	{
		public MaintainArachnidFactoryTask(int ownerID) : base(ownerID)			{ m_StructureToMaintain = map_id.ArachnidFactory;				}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectArachnidFactoryTask(ownerID), true);
			AddPrerequisite(new RepairArachnidFactoryTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildArachnidFactoryTask(ownerID));
		}
	}

	public sealed class MaintainStandardLabTask : MaintainStructureTask
	{
		public MaintainStandardLabTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.StandardLab;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectStandardLabTask(ownerID), true);
			AddPrerequisite(new RepairStandardLabTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildStandardLabTask(ownerID));
		}
	}

	public sealed class MaintainAdvancedLabTask : MaintainStructureTask
	{
		public MaintainAdvancedLabTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.AdvancedLab;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectAdvancedLabTask(ownerID), true);
			AddPrerequisite(new RepairAdvancedLabTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildAdvancedLabTask(ownerID));
		}
	}

	public sealed class MaintainRobotCommandTask : MaintainStructureTask
	{
		public MaintainRobotCommandTask(int ownerID) : base(ownerID)			{ m_StructureToMaintain = map_id.RobotCommand;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectRobotCommandTask(ownerID), true);
			AddPrerequisite(new RepairRobotCommandTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildRobotCommandTask(ownerID));
		}
	}

	public sealed class MaintainSpaceportTask : MaintainStructureTask
	{
		public MaintainSpaceportTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.Spaceport;						}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectSpaceportTask(ownerID), true);
			AddPrerequisite(new RepairSpaceportTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildSpaceportTask(ownerID));
		}
	}

	public sealed class MaintainObservatoryTask : MaintainStructureTask
	{
		public MaintainObservatoryTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.Observatory;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectObservatoryTask(ownerID), true);
			AddPrerequisite(new RepairObservatoryTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildObservatoryTask(ownerID));
		}
	}

	public sealed class MaintainMeteorDefenseTask : MaintainStructureTask
	{
		public MaintainMeteorDefenseTask(int ownerID) : base(ownerID)			{ m_StructureToMaintain = map_id.MeteorDefense;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectMeteorDefenseTask(ownerID), true);
			AddPrerequisite(new RepairMeteorDefenseTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildMeteorDefenseTask(ownerID));
		}
	}

	public sealed class MaintainGuardPostTask : MaintainStructureTask
	{
		public BuildGuardPostTask guardKitTask { get; private set; }

		public MaintainGuardPostTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.GuardPost;						}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectGuardPostTask(ownerID), true);
			AddPrerequisite(new RepairGuardPostTask(ownerID), true);
			AddPrerequisite(buildTask = new BuildGuardPostTask(ownerID));

			guardKitTask = (BuildGuardPostTask)buildTask;

			//kitTask.RandomizeTurret(GameState.players[ownerID].IsEden());
		}
	}
}
