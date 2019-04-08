using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	public sealed class BuildStructureFactoryTask : BuildStructureTask
	{
		public BuildStructureFactoryTask()									{ m_KitToBuild = map_id.StructureFactory;						}
		public BuildStructureFactoryTask(PlayerInfo owner) : base(owner)	{ m_KitToBuild = map_id.StructureFactory;						}

		public override void GeneratePrerequisites()
		{
			//AddPrerequisite(new BuildCommandCenterTask());
			AddPrerequisite(new BuildStructureFactoryKitTask());
		}
	}

	public sealed class BuildCommonSmelterTask : BuildStructureTask
	{
		public BuildCommonSmelterTask()										{ m_KitToBuild = map_id.CommonOreSmelter;						}
		public BuildCommonSmelterTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.CommonOreSmelter;						}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildCommonSmelterKitTask());
		}
	}

	public sealed class BuildAgridomeTask : BuildStructureTask
	{
		public BuildAgridomeTask()											{ m_KitToBuild = map_id.Agridome;	m_DesiredDistance = 2;		}
		public BuildAgridomeTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.Agridome;	m_DesiredDistance = 2;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildAgridomeKitTask());
		}
	}

	public sealed class BuildTokamakTask : BuildStructureTask
	{
		public BuildTokamakTask()											{ m_KitToBuild = map_id.Tokamak;	m_DesiredDistance = 5;		}
		public BuildTokamakTask(PlayerInfo owner) : base(owner)				{ m_KitToBuild = map_id.Tokamak;	m_DesiredDistance = 5;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildTokamakKitTask());
		}
	}

	public sealed class BuildMHDGeneratorTask : BuildStructureTask
	{
		public BuildMHDGeneratorTask()										{ m_KitToBuild = map_id.MHDGenerator;	m_DesiredDistance = 5;	}
		public BuildMHDGeneratorTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.MHDGenerator;	m_DesiredDistance = 5;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildMHDGeneratorKitTask());
		}
	}

	public sealed class BuildSolarArrayTask : BuildStructureTask
	{
		public BuildSolarArrayTask()										{ m_KitToBuild = map_id.SolarPowerArray;	m_DesiredDistance = 5;	}
		public BuildSolarArrayTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.SolarPowerArray;	m_DesiredDistance = 5;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildSolarArrayKitTask());
		}
	}

	public sealed class BuildNurseryTask : BuildStructureTask
	{
		public BuildNurseryTask()											{ m_KitToBuild = map_id.Nursery;	m_DesiredDistance = 1;		}
		public BuildNurseryTask(PlayerInfo owner) : base(owner)				{ m_KitToBuild = map_id.Nursery;	m_DesiredDistance = 1;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildNurseryKitTask());
		}
	}

	public sealed class BuildUniversityTask : BuildStructureTask
	{
		public BuildUniversityTask()										{ m_KitToBuild = map_id.University;	m_DesiredDistance = 1;		}
		public BuildUniversityTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.University;	m_DesiredDistance = 1;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildUniversityKitTask());
		}
	}

	public sealed class BuildResidenceTask : BuildStructureTask
	{
		public BuildResidenceTask()											{ m_KitToBuild = map_id.Residence;	m_DesiredDistance = 1;		}
		public BuildResidenceTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.Residence;	m_DesiredDistance = 1;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildResidenceKitTask());
		}
	}

	public sealed class BuildReinforcedResidenceTask : BuildStructureTask
	{
		public BuildReinforcedResidenceTask()								{ m_KitToBuild = map_id.ReinforcedResidence;	m_DesiredDistance = 1;	}
		public BuildReinforcedResidenceTask(PlayerInfo owner) : base(owner)	{ m_KitToBuild = map_id.ReinforcedResidence;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildReinforcedResidenceKitTask());
		}
	}

	public sealed class BuildAdvancedResidenceTask : BuildStructureTask
	{
		public BuildAdvancedResidenceTask()									{ m_KitToBuild = map_id.AdvancedResidence;	m_DesiredDistance = 1;	}
		public BuildAdvancedResidenceTask(PlayerInfo owner) : base(owner)	{ m_KitToBuild = map_id.AdvancedResidence;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildAdvancedResidenceKitTask());
		}
	}

	public sealed class BuildMedicalCenterTask : BuildStructureTask
	{
		public BuildMedicalCenterTask()										{ m_KitToBuild = map_id.MedicalCenter;	m_DesiredDistance = 1;	}
		public BuildMedicalCenterTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.MedicalCenter;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildMedicalCenterKitTask());
		}
	}

	public sealed class BuildDIRTTask : BuildStructureTask
	{
		public BuildDIRTTask()												{ m_KitToBuild = map_id.DIRT;	m_DesiredDistance = 2;			}
		public BuildDIRTTask(PlayerInfo owner) : base(owner)				{ m_KitToBuild = map_id.DIRT;	m_DesiredDistance = 2;			}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildDIRTKitTask());
		}
	}

	public sealed class BuildRecreationTask : BuildStructureTask
	{
		public BuildRecreationTask()										{ m_KitToBuild = map_id.RecreationFacility;	m_DesiredDistance = 1;	}
		public BuildRecreationTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.RecreationFacility;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildRecreationKitTask());
		}
	}

	public sealed class BuildForumTask : BuildStructureTask
	{
		public BuildForumTask()												{ m_KitToBuild = map_id.Forum;	m_DesiredDistance = 1;			}
		public BuildForumTask(PlayerInfo owner) : base(owner)				{ m_KitToBuild = map_id.Forum;	m_DesiredDistance = 1;			}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildForumKitTask());
		}
	}

	public sealed class BuildGORFTask : BuildStructureTask
	{
		public BuildGORFTask()												{ m_KitToBuild = map_id.GORF;	m_DesiredDistance = 2;			}
		public BuildGORFTask(PlayerInfo owner) : base(owner)				{ m_KitToBuild = map_id.GORF;	m_DesiredDistance = 2;			}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildGORFKitTask());
		}
	}

	public sealed class BuildVehicleFactoryTask : BuildStructureTask
	{
		public BuildVehicleFactoryTask()									{ m_KitToBuild = map_id.VehicleFactory;	m_DesiredDistance = 2;	}
		public BuildVehicleFactoryTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.VehicleFactory;	m_DesiredDistance = 2;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildVehicleFactoryKitTask());
		}
	}

	public sealed class BuildStandardLabTask : BuildStructureTask
	{
		public BuildStandardLabTask()										{ m_KitToBuild = map_id.StandardLab;	m_DesiredDistance = 2;	}
		public BuildStandardLabTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.StandardLab;	m_DesiredDistance = 2;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildStandardLabKitTask());
		}
	}

	public sealed class BuildAdvancedLabTask : BuildStructureTask
	{
		public BuildAdvancedLabTask()										{ m_KitToBuild = map_id.AdvancedLab;	m_DesiredDistance = 2;	}
		public BuildAdvancedLabTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.AdvancedLab;	m_DesiredDistance = 2;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildAdvancedLabKitTask());
		}
	}

	public sealed class BuildRobotCommandTask : BuildStructureTask
	{
		public BuildRobotCommandTask()										{ m_KitToBuild = map_id.RobotCommand;	m_DesiredDistance = 2;	}
		public BuildRobotCommandTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.RobotCommand;	m_DesiredDistance = 2;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildRobotCommandKitTask());
		}
	}

	public sealed class BuildSpaceportTask : BuildStructureTask
	{
		public BuildSpaceportTask()											{ m_KitToBuild = map_id.Spaceport;	m_DesiredDistance = 2;		}
		public BuildSpaceportTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.Spaceport;	m_DesiredDistance = 2;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildSpaceportKitTask());
		}
	}
}
