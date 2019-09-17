using System.Collections.Generic;
using DotNetMissionSDK.Async;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Snapshot;

namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	public class BuildResearchedStructureKitTask : BuildStructureKitTask
	{
		private ResearchTask m_ResearchKitTask;
		private ResearchTask m_ResearchKitCargoTask;

		public BuildResearchedStructureKitTask(int ownerID) : base(ownerID)		{	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(m_ResearchKitTask = new ResearchTask(ownerID, new UnitInfo(m_KitToBuild).GetResearchTopic()));

			if (m_KitToBuildCargo != map_id.None)
				AddPrerequisite(m_ResearchKitCargoTask = new ResearchTask(ownerID, new UnitInfo(m_KitToBuildCargo).GetResearchTopic()));
		}

		/// <summary>
		/// Gets the list of structures to activate.
		/// </summary>
		/// <param name="stateSnapshot">The state snapshot to use for performing task calculations.</param>
		/// <param name="structureIDs">The list to add structures to.</param>
		public override void GetStructuresToActivate(StateSnapshot stateSnapshot, List<int> structureIDs)
		{
			// Assign scientists to labs to complete required research
			if (!m_ResearchKitTask.IsTaskComplete(stateSnapshot))
				m_ResearchKitTask.GetStructuresToActivate(stateSnapshot, structureIDs);

			if (m_ResearchKitCargoTask != null && !m_ResearchKitCargoTask.IsTaskComplete(stateSnapshot))
				m_ResearchKitCargoTask.GetStructuresToActivate(stateSnapshot, structureIDs);

			base.GetStructuresToActivate(stateSnapshot, structureIDs);
		}
	}

	public sealed class BuildCommandCenterKitTask : BuildStructureKitTask
	{
		public BuildCommandCenterKitTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.CommandCenter;					}
	}

	public sealed class BuildStructureFactoryKitTask : BuildStructureKitTask
	{
		public BuildStructureFactoryKitTask(int ownerID) : base(ownerID)	{ m_KitToBuild = map_id.StructureFactory;				}
	}

	public sealed class BuildCommonSmelterKitTask : BuildStructureKitTask
	{
		public BuildCommonSmelterKitTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.CommonOreSmelter;				}
	}

	public sealed class BuildRareSmelterKitTask : BuildResearchedStructureKitTask
	{
		public BuildRareSmelterKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.RareOreSmelter;					}
	}

	public sealed class BuildAgridomeKitTask : BuildStructureKitTask
	{
		public BuildAgridomeKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.Agridome;						}
	}

	public sealed class BuildTokamakKitTask : BuildStructureKitTask
	{
		public BuildTokamakKitTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.Tokamak;						}
	}

	public sealed class BuildMHDGeneratorKitTask : BuildResearchedStructureKitTask
	{
		public BuildMHDGeneratorKitTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.MHDGenerator;					}
	}

	public sealed class BuildSolarArrayKitTask : BuildResearchedStructureKitTask
	{
		public BuildSolarArrayKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.SolarPowerArray;				}
	}

	public sealed class BuildNurseryKitTask : BuildResearchedStructureKitTask
	{
		public BuildNurseryKitTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.Nursery;						}
	}

	public sealed class BuildUniversityKitTask : BuildResearchedStructureKitTask
	{
		public BuildUniversityKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.University;						}
	}

	public sealed class BuildResidenceKitTask : BuildStructureKitTask
	{
		public BuildResidenceKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.Residence;						}
	}

	public sealed class BuildReinforcedResidenceKitTask : BuildResearchedStructureKitTask
	{
		public BuildReinforcedResidenceKitTask(int ownerID) : base(ownerID)	{ m_KitToBuild = map_id.ReinforcedResidence;			}
	}

	public sealed class BuildAdvancedResidenceKitTask : BuildResearchedStructureKitTask
	{
		public BuildAdvancedResidenceKitTask(int ownerID) : base(ownerID)	{ m_KitToBuild = map_id.AdvancedResidence;				}
	}

	public sealed class BuildMedicalCenterKitTask : BuildResearchedStructureKitTask
	{
		public BuildMedicalCenterKitTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.MedicalCenter;					}
	}

	public sealed class BuildDIRTKitTask : BuildResearchedStructureKitTask
	{
		public BuildDIRTKitTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.DIRT;							}
	}

	public sealed class BuildRecreationKitTask : BuildResearchedStructureKitTask
	{
		public BuildRecreationKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.RecreationFacility;				}
	}

	public sealed class BuildForumKitTask : BuildResearchedStructureKitTask
	{
		public BuildForumKitTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.Forum;							}
	}

	public sealed class BuildGORFKitTask : BuildResearchedStructureKitTask
	{
		public BuildGORFKitTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.GORF;							}
	}

	public sealed class BuildVehicleFactoryKitTask : BuildResearchedStructureKitTask
	{
		public BuildVehicleFactoryKitTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.VehicleFactory;					}
	}

	public sealed class BuildArachnidFactoryKitTask : BuildResearchedStructureKitTask
	{
		public BuildArachnidFactoryKitTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.ArachnidFactory;				}
	}

	public sealed class BuildBasicLabKitTask : BuildStructureKitTask
	{
		public BuildBasicLabKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.BasicLab;						}
	}

	public sealed class BuildStandardLabKitTask : BuildStructureKitTask
	{
		public BuildStandardLabKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.StandardLab;					}
	}

	public sealed class BuildAdvancedLabKitTask : BuildResearchedStructureKitTask
	{
		public BuildAdvancedLabKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.AdvancedLab;					}
	}

	public sealed class BuildRobotCommandKitTask : BuildResearchedStructureKitTask
	{
		public BuildRobotCommandKitTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.RobotCommand;					}
	}

	public sealed class BuildSpaceportKitTask : BuildResearchedStructureKitTask
	{
		public BuildSpaceportKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.Spaceport;						}
	}

	public sealed class BuildObservatoryKitTask : BuildResearchedStructureKitTask
	{
		public BuildObservatoryKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.Observatory;					}
	}

	public sealed class BuildMeteorDefenseKitTask : BuildResearchedStructureKitTask
	{
		public BuildMeteorDefenseKitTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.MeteorDefense;					}
	}

	public sealed class BuildGuardPostKitTask : BuildResearchedStructureKitTask
	{
		public BuildGuardPostKitTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.GuardPost;						}

		public map_id turret								{ get { return m_KitToBuildCargo; } set { m_KitToBuildCargo = value; }	}

		public void RandomizeTurret(bool isEden, bool includeLineOfSight=true, bool includeBombs=false)
		{
			if (isEden)
			{
				int min = includeLineOfSight ? 0 : 2;
				int max = includeBombs ? 6 : 5;

				int rand = AsyncRandom.Range(min, max);
				switch (rand)
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

				int rand = AsyncRandom.Range(min, max);
				switch (rand)
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
