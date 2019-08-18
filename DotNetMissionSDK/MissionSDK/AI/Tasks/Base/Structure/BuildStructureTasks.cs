using DotNetMissionSDK.State;

namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	public sealed class BuildStructureFactoryTask : BuildStructureTask
	{
		public BuildStructureFactoryTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.StructureFactory;						}

		public override void GeneratePrerequisites()
		{
			//AddPrerequisite(new BuildCommandCenterTask());
			AddPrerequisite(new BuildStructureFactoryKitTask(ownerID));
		}
	}

	public sealed class BuildCommonSmelterTask : BuildStructureTask
	{
		public BuildCommonSmelterTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.CommonOreSmelter;						}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildCommonSmelterKitTask(ownerID));
		}
	}

	public sealed class BuildAgridomeTask : BuildStructureTask
	{
		public BuildAgridomeTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.Agridome;	m_DesiredDistance = 2;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildAgridomeKitTask(ownerID));
		}
	}

	public sealed class BuildTokamakTask : BuildStructureTask
	{
		public BuildTokamakTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.Tokamak;	m_DesiredDistance = 5;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildTokamakKitTask(ownerID));
		}
	}

	public sealed class BuildMHDGeneratorTask : BuildStructureTask
	{
		public BuildMHDGeneratorTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.MHDGenerator;	m_DesiredDistance = 5;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildMHDGeneratorKitTask(ownerID));
		}
	}

	public sealed class BuildSolarArrayTask : BuildStructureTask
	{
		public BuildSolarArrayTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.SolarPowerArray;	m_DesiredDistance = 5;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildSolarArrayKitTask(ownerID));
		}
	}

	public sealed class BuildNurseryTask : BuildStructureTask
	{
		public BuildNurseryTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.Nursery;	m_DesiredDistance = 1;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildNurseryKitTask(ownerID));
		}
	}

	public sealed class BuildUniversityTask : BuildStructureTask
	{
		public BuildUniversityTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.University;	m_DesiredDistance = 1;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildUniversityKitTask(ownerID));
		}
	}

	public sealed class BuildResidenceTask : BuildStructureTask
	{
		public BuildResidenceTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.Residence;	m_DesiredDistance = 1;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildResidenceKitTask(ownerID));
		}
	}

	public sealed class BuildReinforcedResidenceTask : BuildStructureTask
	{
		public BuildReinforcedResidenceTask(int ownerID) : base(ownerID)	{ m_KitToBuild = map_id.ReinforcedResidence;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildReinforcedResidenceKitTask(ownerID));
		}
	}

	public sealed class BuildAdvancedResidenceTask : BuildStructureTask
	{
		public BuildAdvancedResidenceTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.AdvancedResidence;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildAdvancedResidenceKitTask(ownerID));
		}
	}

	public sealed class BuildMedicalCenterTask : BuildStructureTask
	{
		public BuildMedicalCenterTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.MedicalCenter;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildMedicalCenterKitTask(ownerID));
		}
	}

	public sealed class BuildDIRTTask : BuildStructureTask
	{
		public BuildDIRTTask(int ownerID) : base(ownerID)					{ m_KitToBuild = map_id.DIRT;	m_DesiredDistance = 2;			}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildDIRTKitTask(ownerID));
		}
	}

	public sealed class BuildRecreationTask : BuildStructureTask
	{
		public BuildRecreationTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.RecreationFacility;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildRecreationKitTask(ownerID));
		}
	}

	public sealed class BuildForumTask : BuildStructureTask
	{
		public BuildForumTask(int ownerID) : base(ownerID)					{ m_KitToBuild = map_id.Forum;	m_DesiredDistance = 1;			}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildForumKitTask(ownerID));
		}
	}

	public sealed class BuildGORFTask : BuildStructureTask
	{
		public BuildGORFTask(int ownerID) : base(ownerID)					{ m_KitToBuild = map_id.GORF;	m_DesiredDistance = 2;			}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildGORFKitTask(ownerID));
		}
	}

	public sealed class BuildVehicleFactoryTask : BuildStructureTask
	{
		public BuildVehicleFactoryTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.VehicleFactory;	m_DesiredDistance = 2;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildVehicleFactoryKitTask(ownerID));
		}
	}

	public sealed class BuildArachnidFactoryTask : BuildStructureTask
	{
		public BuildArachnidFactoryTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.ArachnidFactory;	m_DesiredDistance = 2;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildArachnidFactoryKitTask(ownerID));
		}
	}

	public sealed class BuildStandardLabTask : BuildStructureTask
	{
		public BuildStandardLabTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.StandardLab;	m_DesiredDistance = 2;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildStandardLabKitTask(ownerID));
		}
	}

	public sealed class BuildAdvancedLabTask : BuildStructureTask
	{
		public BuildAdvancedLabTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.AdvancedLab;	m_DesiredDistance = 2;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildAdvancedLabKitTask(ownerID));
		}
	}

	public sealed class BuildRobotCommandTask : BuildStructureTask
	{
		public BuildRobotCommandTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.RobotCommand;	m_DesiredDistance = 2;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildRobotCommandKitTask(ownerID));
		}
	}

	public sealed class BuildSpaceportTask : BuildStructureTask
	{
		public BuildSpaceportTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.Spaceport;	m_DesiredDistance = 2;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildSpaceportKitTask(ownerID));
		}
	}

	public sealed class BuildObservatoryTask : BuildStructureTask
	{
		public BuildObservatoryTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.Observatory;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildObservatoryKitTask(ownerID));
		}
	}

	public sealed class BuildMeteorDefenseTask : BuildStructureTask
	{
		public BuildMeteorDefenseTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.MeteorDefense;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildMeteorDefenseKitTask(ownerID));
		}
	}

	public sealed class BuildGuardPostTask : BuildStructureTask
	{
		public BuildGuardPostKitTask kitTask;

		public BuildGuardPostTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.GuardPost;	m_DesiredDistance = 4;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(kitTask = new BuildGuardPostKitTask(ownerID));

			kitTask.RandomizeTurret(GameState.players[ownerID].IsEden());
		}
	}
}
