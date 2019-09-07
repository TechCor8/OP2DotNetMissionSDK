using DotNetMissionSDK.Async;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	public sealed class BuildCommandCenterTask : BuildStructureTask
	{
		public BuildCommandCenterTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.CommandCenter;							}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildCommandCenterKitTask(ownerID));
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Move all non-military units if there is no command center. This new site will be the main base.
			if (owner.units.commandCenters.Count == 0)
			{
				ConvecState convec = owner.units.convecs.FirstOrDefault((ConvecState convec1) => convec1.cargoType == map_id.CommandCenter);

				foreach (VehicleState unit in owner.units.GetVehicles())
				{
					if (unit.unitID == convec.unitID)
						continue;

					map_id unitType = unit.unitType;

					// No military units. That is left up to the combat manager
					if (unitType == map_id.Lynx || unitType == map_id.Panther || unitType == map_id.Tiger ||
						unitType == map_id.Scorpion || unitType == map_id.Spider)
						continue;

					unitActions.AddUnitCommand(unit.unitID, 0, () => GameState.GetUnit(unit.unitID)?.DoMove(convec.position.x+AsyncRandom.Range(1,7), convec.position.y+AsyncRandom.Range(2,8)));
				}
			}

			return base.PerformTask(stateSnapshot, unitActions);
		}
	}

	public sealed class BuildStructureFactoryTask : BuildStructureTask
	{
		public BuildStructureFactoryTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.StructureFactory;						}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildStructureFactoryKitTask(ownerID));
		}
	}

	public sealed class BuildCommonSmelterTask : BuildStructureTask
	{
		public BuildCommonSmelterTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.CommonOreSmelter;						}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildCommonSmelterKitTask(ownerID));
		}
	}

	public sealed class BuildRareSmelterTask : BuildStructureTask
	{
		public BuildRareSmelterTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.RareOreSmelter;							}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildRareSmelterKitTask(ownerID));
		}
	}

	public sealed class BuildAgridomeTask : BuildStructureTask
	{
		public BuildAgridomeTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.Agridome;	m_DesiredDistance = 2;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildAgridomeKitTask(ownerID));
		}
	}

	public sealed class BuildTokamakTask : BuildStructureTask
	{
		public BuildTokamakTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.Tokamak;	m_DesiredDistance = 5;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildTokamakKitTask(ownerID));
		}
	}

	public sealed class BuildMHDGeneratorTask : BuildStructureTask
	{
		public BuildMHDGeneratorTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.MHDGenerator;	m_DesiredDistance = 5;	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildMHDGeneratorKitTask(ownerID));
		}
	}

	public sealed class BuildSolarArrayTask : BuildStructureTask
	{
		public BuildSolarArrayTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.SolarPowerArray;	m_DesiredDistance = 5;	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildSolarArrayKitTask(ownerID));
		}
	}

	public sealed class BuildNurseryTask : BuildStructureTask
	{
		public BuildNurseryTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.Nursery;	m_DesiredDistance = 1;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildNurseryKitTask(ownerID));
		}
	}

	public sealed class BuildUniversityTask : BuildStructureTask
	{
		public BuildUniversityTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.University;	m_DesiredDistance = 1;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildUniversityKitTask(ownerID));
		}
	}

	public sealed class BuildResidenceTask : BuildStructureTask
	{
		public BuildResidenceTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.Residence;	m_DesiredDistance = 1;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildResidenceKitTask(ownerID));
		}
	}

	public sealed class BuildReinforcedResidenceTask : BuildStructureTask
	{
		public BuildReinforcedResidenceTask(int ownerID) : base(ownerID)	{ m_KitToBuild = map_id.ReinforcedResidence;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildReinforcedResidenceKitTask(ownerID));
		}
	}

	public sealed class BuildAdvancedResidenceTask : BuildStructureTask
	{
		public BuildAdvancedResidenceTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.AdvancedResidence;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildAdvancedResidenceKitTask(ownerID));
		}
	}

	public sealed class BuildMedicalCenterTask : BuildStructureTask
	{
		public BuildMedicalCenterTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.MedicalCenter;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildMedicalCenterKitTask(ownerID));
		}
	}

	public sealed class BuildDIRTTask : BuildStructureTask
	{
		public BuildDIRTTask(int ownerID) : base(ownerID)					{ m_KitToBuild = map_id.DIRT;	m_DesiredDistance = 2;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildDIRTKitTask(ownerID));
		}
	}

	public sealed class BuildRecreationTask : BuildStructureTask
	{
		public BuildRecreationTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.RecreationFacility;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildRecreationKitTask(ownerID));
		}
	}

	public sealed class BuildForumTask : BuildStructureTask
	{
		public BuildForumTask(int ownerID) : base(ownerID)					{ m_KitToBuild = map_id.Forum;	m_DesiredDistance = 1;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildForumKitTask(ownerID));
		}
	}

	public sealed class BuildGORFTask : BuildStructureTask
	{
		public BuildGORFTask(int ownerID) : base(ownerID)					{ m_KitToBuild = map_id.GORF;	m_DesiredDistance = 2;			}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildGORFKitTask(ownerID));
		}
	}

	public sealed class BuildVehicleFactoryTask : BuildStructureTask
	{
		public BuildVehicleFactoryTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.VehicleFactory;	m_DesiredDistance = 2;	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildVehicleFactoryKitTask(ownerID));
		}
	}

	public sealed class BuildArachnidFactoryTask : BuildStructureTask
	{
		public BuildArachnidFactoryTask(int ownerID) : base(ownerID)		{ m_KitToBuild = map_id.ArachnidFactory;	m_DesiredDistance = 2;	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildArachnidFactoryKitTask(ownerID));
		}
	}

	public sealed class BuildStandardLabTask : BuildStructureTask
	{
		public BuildStandardLabTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.StandardLab;	m_DesiredDistance = 2;	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildStandardLabKitTask(ownerID));
		}
	}

	public sealed class BuildAdvancedLabTask : BuildStructureTask
	{
		public BuildAdvancedLabTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.AdvancedLab;	m_DesiredDistance = 2;	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildAdvancedLabKitTask(ownerID));
		}
	}

	public sealed class BuildRobotCommandTask : BuildStructureTask
	{
		public BuildRobotCommandTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.RobotCommand;	m_DesiredDistance = 2;	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildRobotCommandKitTask(ownerID));
		}
	}

	public sealed class BuildSpaceportTask : BuildStructureTask
	{
		public BuildSpaceportTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.Spaceport;	m_DesiredDistance = 2;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildSpaceportKitTask(ownerID));
		}
	}

	public sealed class BuildObservatoryTask : BuildStructureTask
	{
		public BuildObservatoryTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.Observatory;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildObservatoryKitTask(ownerID));
		}
	}

	public sealed class BuildMeteorDefenseTask : BuildStructureTask
	{
		public BuildMeteorDefenseTask(int ownerID) : base(ownerID)			{ m_KitToBuild = map_id.MeteorDefense;	m_DesiredDistance = 1;	}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = new BuildMeteorDefenseKitTask(ownerID));
		}
	}

	public sealed class BuildGuardPostTask : BuildStructureTask
	{
		private BuildGuardPostKitTask m_KitTask;

		public BuildGuardPostTask(int ownerID) : base(ownerID)				{ m_KitToBuild = map_id.GuardPost;	m_DesiredDistance = 4;		}

		public override void GeneratePrerequisites()
		{
			base.GeneratePrerequisites();
			AddPrerequisite(buildKitTask = m_KitTask = new BuildGuardPostKitTask(ownerID));

			m_KitTask.RandomizeTurret(GameState.players[ownerID].IsEden());
		}

		protected override bool CanPerformTask(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Get convec with kit
			if (owner.units.convecs.FirstOrDefault((unit) => unit.cargoType == m_KitToBuild /*&& unit.cargoType == m_KitTask.turret*/) != null)
				return true;

			if (owner.CanBuildUnit(stateSnapshot, m_KitToBuild, m_KitTask.turret))
				return true;

			return false;
		}

		public void RandomizeTurret(bool isEden, bool lineOfSight=true, bool includeBombs=false)
		{
			m_KitTask.RandomizeTurret(isEden, lineOfSight, includeBombs);
		}
	}
}
