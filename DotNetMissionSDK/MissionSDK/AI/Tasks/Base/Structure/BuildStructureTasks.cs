using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	public class BuildStructureFactoryTask : BuildStructureTask
	{
		public BuildStructureFactoryTask()									{ m_KitToBuild = map_id.StructureFactory;						}
		public BuildStructureFactoryTask(PlayerInfo owner) : base(owner)	{ m_KitToBuild = map_id.StructureFactory;						}

		public override void GeneratePrerequisites()
		{
			//AddPrerequisite(new BuildCommandCenterTask());
			AddPrerequisite(new BuildStructureFactoryKitTask());
		}
	}

	public class BuildCommonSmelterTask : BuildStructureTask
	{
		public BuildCommonSmelterTask()										{ m_KitToBuild = map_id.CommonOreSmelter;						}
		public BuildCommonSmelterTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.CommonOreSmelter;						}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildCommonSmelterKitTask());
		}
	}

	public class BuildAgridomeTask : BuildStructureTask
	{
		public BuildAgridomeTask()											{ m_KitToBuild = map_id.Agridome;	m_DesiredDistance = 2;		}
		public BuildAgridomeTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.Agridome;	m_DesiredDistance = 2;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildAgridomeKitTask());
		}
	}

	public class BuildTokamakTask : BuildStructureTask
	{
		public BuildTokamakTask()											{ m_KitToBuild = map_id.Tokamak;	m_DesiredDistance = 5;		}
		public BuildTokamakTask(PlayerInfo owner) : base(owner)				{ m_KitToBuild = map_id.Tokamak;	m_DesiredDistance = 5;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildTokamakKitTask());
		}
	}

	public class BuildNurseryTask : BuildStructureTask
	{
		public BuildNurseryTask()											{ m_KitToBuild = map_id.Nursery;	m_DesiredDistance = 1;		}
		public BuildNurseryTask(PlayerInfo owner) : base(owner)				{ m_KitToBuild = map_id.Nursery;	m_DesiredDistance = 1;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildNurseryKitTask());
		}
	}

	public class BuildUniversityTask : BuildStructureTask
	{
		public BuildUniversityTask()										{ m_KitToBuild = map_id.University;	m_DesiredDistance = 1;		}
		public BuildUniversityTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.University;	m_DesiredDistance = 1;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildUniversityKitTask());
		}
	}

	public class BuildResidenceTask : BuildStructureTask
	{
		public BuildResidenceTask()											{ m_KitToBuild = map_id.Residence;	m_DesiredDistance = 1;		}
		public BuildResidenceTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.Residence;	m_DesiredDistance = 1;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildResidenceKitTask());
		}
	}

	public class BuildMedicalCenterTask : BuildStructureTask
	{
		public BuildMedicalCenterTask()										{ m_KitToBuild = map_id.MedicalCenter;	m_DesiredDistance = 1;	}
		public BuildMedicalCenterTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.MedicalCenter;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildMedicalCenterKitTask());
		}
	}

	public class BuildVehicleFactoryTask : BuildStructureTask
	{
		public BuildVehicleFactoryTask()									{ m_KitToBuild = map_id.VehicleFactory;	m_DesiredDistance = 2;	}
		public BuildVehicleFactoryTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.VehicleFactory;	m_DesiredDistance = 2;	}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildVehicleFactoryKitTask());
		}
	}

	public class BuildSpaceportTask : BuildStructureTask
	{
		public BuildSpaceportTask()											{ m_KitToBuild = map_id.Spaceport;	m_DesiredDistance = 2;		}
		public BuildSpaceportTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.Spaceport;	m_DesiredDistance = 2;		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildSpaceportKitTask());
		}
	}
}
