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

	public sealed class BuildMedicalCenterKitTask : BuildStructureKitTask
	{
		public BuildMedicalCenterKitTask()									{ m_KitToBuild = map_id.MedicalCenter;					}
		public BuildMedicalCenterKitTask(PlayerInfo owner) : base(owner)	{ m_KitToBuild = map_id.MedicalCenter;					}
	}

	public sealed class BuildVehicleFactoryKitTask : BuildStructureKitTask
	{
		public BuildVehicleFactoryKitTask()									{ m_KitToBuild = map_id.VehicleFactory;					}
		public BuildVehicleFactoryKitTask(PlayerInfo owner) : base(owner)	{ m_KitToBuild = map_id.VehicleFactory;					}
	}

	public sealed class BuildSpaceportKitTask : BuildStructureKitTask
	{
		public BuildSpaceportKitTask()										{ m_KitToBuild = map_id.Spaceport;						}
		public BuildSpaceportKitTask(PlayerInfo owner) : base(owner)		{ m_KitToBuild = map_id.Spaceport;						}
	}
}
