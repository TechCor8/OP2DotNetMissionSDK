
namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	public sealed class MaintainStructureFactoryTask : MaintainStructureTask
	{
		public MaintainStructureFactoryTask(int ownerID) : base(ownerID)		{ m_StructureToMaintain = map_id.StructureFactory;				}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectStructureFactoryTask(ownerID));
			AddPrerequisite(new RepairStructureFactoryTask(ownerID));
			AddPrerequisite(buildTask = new BuildStructureFactoryTask(ownerID), true);
		}
	}

	public sealed class MaintainCommonSmelterTask : MaintainStructureTask
	{
		public MaintainCommonSmelterTask(int ownerID) : base(ownerID)			{ m_StructureToMaintain = map_id.CommonOreSmelter;				}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectCommonSmelterTask(ownerID));
			AddPrerequisite(new RepairCommonSmelterTask(ownerID));
			AddPrerequisite(buildTask = new BuildCommonSmelterTask(ownerID), true);
		}
	}

	public sealed class MaintainAgridomeTask : MaintainStructureTask
	{
		public MaintainAgridomeTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.Agridome;						}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectAgridomeTask(ownerID));
			AddPrerequisite(new RepairAgridomeTask(ownerID));
			AddPrerequisite(buildTask = new BuildAgridomeTask(ownerID), true);
		}
	}

	public sealed class MaintainTokamakTask : MaintainStructureTask
	{
		public MaintainTokamakTask(int ownerID) : base(ownerID)					{ m_StructureToMaintain = map_id.Tokamak;						}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectTokamakTask(ownerID));
			AddPrerequisite(new RepairTokamakTask(ownerID));
			AddPrerequisite(buildTask = new BuildTokamakTask(ownerID), true);
		}
	}

	public sealed class MaintainMHDGeneratorTask : MaintainStructureTask
	{
		public MaintainMHDGeneratorTask(int ownerID) : base(ownerID)			{ m_StructureToMaintain = map_id.MHDGenerator;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectMHDGeneratorTask(ownerID));
			AddPrerequisite(new RepairMHDGeneratorTask(ownerID));
			AddPrerequisite(buildTask = new BuildMHDGeneratorTask(ownerID), true);
		}
	}

	public sealed class MaintainSolarArrayTask : MaintainStructureTask
	{
		public MaintainSolarArrayTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.SolarPowerArray;				}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectSolarArrayTask(ownerID));
			AddPrerequisite(new RepairSolarArrayTask(ownerID));
			AddPrerequisite(buildTask = new BuildSolarArrayTask(ownerID), true);
		}
	}

	public sealed class MaintainNurseryTask : MaintainStructureTask
	{
		public MaintainNurseryTask(int ownerID) : base(ownerID)					{ m_StructureToMaintain = map_id.Nursery;						}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectNurseryTask(ownerID));
			AddPrerequisite(new RepairNurseryTask(ownerID));
			AddPrerequisite(buildTask = new BuildNurseryTask(ownerID), true);
		}
	}

	public sealed class MaintainUniversityTask : MaintainStructureTask
	{
		public MaintainUniversityTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.University;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectUniversityTask(ownerID));
			AddPrerequisite(new RepairUniversityTask(ownerID));
			AddPrerequisite(buildTask = new BuildUniversityTask(ownerID), true);
		}
	}

	public sealed class MaintainResidenceTask : MaintainStructureTask
	{
		public MaintainResidenceTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.Residence;						}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectResidenceTask(ownerID));
			AddPrerequisite(new RepairResidenceTask(ownerID));
			AddPrerequisite(buildTask = new BuildResidenceTask(ownerID), true);
		}
	}

	public sealed class MaintainReinforcedResidenceTask : MaintainStructureTask
	{
		public MaintainReinforcedResidenceTask(int ownerID) : base(ownerID)		{ m_StructureToMaintain = map_id.ReinforcedResidence;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectReinforcedResidenceTask(ownerID));
			AddPrerequisite(new RepairReinforcedResidenceTask(ownerID));
			AddPrerequisite(buildTask = new BuildReinforcedResidenceTask(ownerID), true);
		}
	}

	public sealed class MaintainAdvancedResidenceTask : MaintainStructureTask
	{
		public MaintainAdvancedResidenceTask(int ownerID) : base(ownerID)		{ m_StructureToMaintain = map_id.AdvancedResidence;				}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectAdvancedResidenceTask(ownerID));
			AddPrerequisite(new RepairAdvancedResidenceTask(ownerID));
			AddPrerequisite(buildTask = new BuildAdvancedResidenceTask(ownerID), true);
		}
	}

	public sealed class MaintainMedicalCenterTask : MaintainStructureTask
	{
		public MaintainMedicalCenterTask(int ownerID) : base(ownerID)			{ m_StructureToMaintain = map_id.MedicalCenter;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectMedicalCenterTask(ownerID));
			AddPrerequisite(new RepairMedicalCenterTask(ownerID));
			AddPrerequisite(buildTask = new BuildMedicalCenterTask(ownerID), true);
		}
	}

	public sealed class MaintainDIRTTask : MaintainStructureTask
	{
		public MaintainDIRTTask(int ownerID) : base(ownerID)					{ m_StructureToMaintain = map_id.DIRT;							}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectDIRTTask(ownerID));
			AddPrerequisite(new RepairDIRTTask(ownerID));
			AddPrerequisite(buildTask = new BuildDIRTTask(ownerID), true);
		}
	}

	public sealed class MaintainRecreationTask : MaintainStructureTask
	{
		public MaintainRecreationTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.RecreationFacility;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectRecreationTask(ownerID));
			AddPrerequisite(new RepairRecreationTask(ownerID));
			AddPrerequisite(buildTask = new BuildRecreationTask(ownerID), true);
		}
	}

	public sealed class MaintainForumTask : MaintainStructureTask
	{
		public MaintainForumTask(int ownerID) : base(ownerID)					{ m_StructureToMaintain = map_id.Forum;							}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectForumTask(ownerID));
			AddPrerequisite(new RepairForumTask(ownerID));
			AddPrerequisite(buildTask = new BuildForumTask(ownerID), true);
		}
	}

	public sealed class MaintainGORFTask : MaintainStructureTask
	{
		public MaintainGORFTask(int ownerID) : base(ownerID)					{ m_StructureToMaintain = map_id.GORF;							}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectGORFTask(ownerID));
			AddPrerequisite(new RepairGORFTask(ownerID));
			AddPrerequisite(buildTask = new BuildGORFTask(ownerID), true);
		}
	}

	public sealed class MaintainVehicleFactoryTask : MaintainStructureTask
	{
		public MaintainVehicleFactoryTask(int ownerID) : base(ownerID)			{ m_StructureToMaintain = map_id.VehicleFactory;				}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectVehicleFactoryTask(ownerID));
			AddPrerequisite(new RepairVehicleFactoryTask(ownerID));
			AddPrerequisite(buildTask = new BuildVehicleFactoryTask(ownerID), true);
		}
	}

	public sealed class MaintainArachnidFactoryTask : MaintainStructureTask
	{
		public MaintainArachnidFactoryTask(int ownerID) : base(ownerID)			{ m_StructureToMaintain = map_id.ArachnidFactory;				}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectArachnidFactoryTask(ownerID));
			AddPrerequisite(new RepairArachnidFactoryTask(ownerID));
			AddPrerequisite(buildTask = new BuildArachnidFactoryTask(ownerID), true);
		}
	}

	public sealed class MaintainStandardLabTask : MaintainStructureTask
	{
		public MaintainStandardLabTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.StandardLab;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectStandardLabTask(ownerID));
			AddPrerequisite(new RepairStandardLabTask(ownerID));
			AddPrerequisite(buildTask = new BuildStandardLabTask(ownerID), true);
		}
	}

	public sealed class MaintainAdvancedLabTask : MaintainStructureTask
	{
		public MaintainAdvancedLabTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.AdvancedLab;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectAdvancedLabTask(ownerID));
			AddPrerequisite(new RepairAdvancedLabTask(ownerID));
			AddPrerequisite(buildTask = new BuildAdvancedLabTask(ownerID), true);
		}
	}

	public sealed class MaintainRobotCommandTask : MaintainStructureTask
	{
		public MaintainRobotCommandTask(int ownerID) : base(ownerID)			{ m_StructureToMaintain = map_id.RobotCommand;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectRobotCommandTask(ownerID));
			AddPrerequisite(new RepairRobotCommandTask(ownerID));
			AddPrerequisite(buildTask = new BuildRobotCommandTask(ownerID), true);
		}
	}

	public sealed class MaintainSpaceportTask : MaintainStructureTask
	{
		public MaintainSpaceportTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.Spaceport;						}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectSpaceportTask(ownerID));
			AddPrerequisite(new RepairSpaceportTask(ownerID));
			AddPrerequisite(buildTask = new BuildSpaceportTask(ownerID), true);
		}
	}

	public sealed class MaintainObservatoryTask : MaintainStructureTask
	{
		public MaintainObservatoryTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.Observatory;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectObservatoryTask(ownerID));
			AddPrerequisite(new RepairObservatoryTask(ownerID));
			AddPrerequisite(buildTask = new BuildObservatoryTask(ownerID), true);
		}
	}

	public sealed class MaintainMeteorDefenseTask : MaintainStructureTask
	{
		public MaintainMeteorDefenseTask(int ownerID) : base(ownerID)			{ m_StructureToMaintain = map_id.MeteorDefense;					}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectMeteorDefenseTask(ownerID));
			AddPrerequisite(new RepairMeteorDefenseTask(ownerID));
			AddPrerequisite(buildTask = new BuildMeteorDefenseTask(ownerID), true);
		}
	}

	public sealed class MaintainGuardPostTask : MaintainStructureTask
	{
		public BuildGuardPostTask guardKitTask { get; private set; }

		public MaintainGuardPostTask(int ownerID) : base(ownerID)				{ m_StructureToMaintain = map_id.GuardPost;						}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(connectTask = new ConnectGuardPostTask(ownerID));
			AddPrerequisite(new RepairGuardPostTask(ownerID));
			AddPrerequisite(buildTask = new BuildGuardPostTask(ownerID), true);

			guardKitTask = (BuildGuardPostTask)buildTask;

			//kitTask.RandomizeTurret(GameState.players[ownerID].IsEden());
		}
	}
}
