using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	public class BuildStructureFactoryTask : BuildStructureTask
	{
		public BuildStructureFactoryTask()									{ m_KitToBuild = map_id.StructureFactory;				}
		public BuildStructureFactoryTask(PlayerInfo owner) : base(owner)	{ m_KitToBuild = map_id.StructureFactory;				}

		public override void GeneratePrerequisites()
		{
			//AddPrerequisite(new BuildCommandCenterTask());
			AddPrerequisite(new BuildStructureFactoryKitTask());
		}
	}

	public class BuildCommonSmelterTask : BuildStructureTask
	{
		public BuildCommonSmelterTask()										{ m_KitToBuild = map_id.CommonOreSmelter;				}
		public BuildCommonSmelterTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.CommonOreSmelter;				}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildCommonSmelterKitTask());
		}
	}

	public class BuildAgridomeTask : BuildStructureTask
	{
		public BuildAgridomeTask()											{ m_KitToBuild = map_id.Agridome;						}
		public BuildAgridomeTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.Agridome;						}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildCommonSmelterKitTask());
		}
	}

	public class BuildTokamakTask : BuildStructureTask
	{
		public BuildTokamakTask()											{ m_KitToBuild = map_id.Tokamak;						}
		public BuildTokamakTask(PlayerInfo owner) : base(owner)				{ m_KitToBuild = map_id.Tokamak;						}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildCommonSmelterKitTask());
		}
	}

	public class BuildVehicleFactoryTask : BuildStructureTask
	{
		public BuildVehicleFactoryTask()									{ m_KitToBuild = map_id.VehicleFactory;					}
		public BuildVehicleFactoryTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.VehicleFactory;					}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildVehicleFactoryKitTask());
		}
	}

	public class BuildSpaceportTask : BuildStructureTask
	{
		public BuildSpaceportTask()											{ m_KitToBuild = map_id.Spaceport;						}
		public BuildSpaceportTask(PlayerInfo owner) : base(owner)			{ m_KitToBuild = map_id.Spaceport;						}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildSpaceportKitTask());
		}
	}
}
