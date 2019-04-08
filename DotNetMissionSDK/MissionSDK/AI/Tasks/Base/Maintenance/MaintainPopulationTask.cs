using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class MaintainPopulationTask : Task
	{
		private List<BuildStructureTask> m_Prerequisites = new List<BuildStructureTask>();

		private BuildResidenceTask m_BuildResidenceTask;
		private BuildAdvancedResidenceTask m_BuildEdenResidenceTask;
		private BuildReinforcedResidenceTask m_BuildPlymouthResidenceTask;
		private BuildMedicalCenterTask m_BuildMedicalCenterTask;
		private BuildRecreationTask m_BuildRecreationTask;
		private BuildForumTask m_BuildForumTask;
		private BuildDIRTTask m_BuildDirtTask;
		

		public MaintainPopulationTask() { }
		public MaintainPopulationTask(PlayerInfo owner) : base(owner) { }

		public override bool IsTaskComplete()
		{
			foreach (Task task in m_Prerequisites)
			{
				if (!task.IsTaskComplete())
					return false;
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			m_Prerequisites.Add(new BuildNurseryTask());
			m_Prerequisites.Add(new BuildUniversityTask());
			m_Prerequisites.Add(m_BuildResidenceTask = new BuildResidenceTask());
			m_Prerequisites.Add(m_BuildEdenResidenceTask = new BuildAdvancedResidenceTask());
			m_Prerequisites.Add(m_BuildPlymouthResidenceTask = new BuildReinforcedResidenceTask());
			m_Prerequisites.Add(m_BuildMedicalCenterTask = new BuildMedicalCenterTask());
			m_Prerequisites.Add(new BuildStandardLabTask());
			m_Prerequisites.Add(new BuildAdvancedLabTask());
			m_Prerequisites.Add(new BuildRobotCommandTask());
			m_Prerequisites.Add(m_BuildRecreationTask = new BuildRecreationTask());
			m_Prerequisites.Add(m_BuildForumTask = new BuildForumTask());
			m_Prerequisites.Add(m_BuildDirtTask = new BuildDIRTTask());
			m_Prerequisites.Add(new BuildGORFTask());

			m_BuildResidenceTask.targetCountToBuild = 0;
			m_BuildEdenResidenceTask.targetCountToBuild = 0;
			m_BuildPlymouthResidenceTask.targetCountToBuild = 0;
			m_BuildMedicalCenterTask.targetCountToBuild = 0;
			m_BuildRecreationTask.targetCountToBuild = 0;
			m_BuildForumTask.targetCountToBuild = 0;
			m_BuildDirtTask.targetCountToBuild = 0;

			foreach (BuildStructureTask task in m_Prerequisites)
				AddPrerequisite(task);
		}

		protected override bool PerformTask()
		{
			return true;
		}

		public void UpdateRequirements()
		{
			BuildResidence();
			BuildMedicalCenter();
			BuildRecreation();
			BuildDIRT();
		}

		private void BuildResidence()
		{
			// Don't build more residences if we aren't using all the ones we have
			List<UnitEx> residences = new List<UnitEx>(owner.units.residences);
			residences.AddRange(owner.units.advancedResidences);
			residences.AddRange(owner.units.reinforcedResidences);

			foreach (UnitEx residence in residences)
			{
				if (!residence.IsEnabled())
					return;
			}

			System.Console.WriteLine(owner.player.TotalPopulation() + " > " + owner.player.GetTotalResidenceCapacity());

			if (owner.player.GetTotalResidenceCapacity() >= owner.player.TotalPopulation())
				return;

			// Determine residence type to build
			map_id residenceTypeToBuild = map_id.Residence;

			if (owner.player.IsEden())
			{
				if (owner.player.HasTechnology(TechInfo.GetTechID(map_id.AdvancedResidence)) && owner.units.residences.Count > owner.units.advancedResidences.Count * 2)
					residenceTypeToBuild = map_id.AdvancedResidence;
			}
			else
			{
				if (owner.player.HasTechnology(TechInfo.GetTechID(map_id.ReinforcedResidence)) && owner.units.residences.Count > owner.units.reinforcedResidences.Count * 2)
					residenceTypeToBuild = map_id.ReinforcedResidence;
			}

			switch (residenceTypeToBuild)
			{
				case map_id.Residence:				m_BuildResidenceTask.targetCountToBuild = owner.units.residences.Count+1;						break;
				case map_id.AdvancedResidence:		m_BuildEdenResidenceTask.targetCountToBuild = owner.units.advancedResidences.Count+1;			break;
				case map_id.ReinforcedResidence:	m_BuildPlymouthResidenceTask.targetCountToBuild = owner.units.reinforcedResidences.Count+1;		break;
			}
		}

		private void BuildMedicalCenter()
		{
			// Don't build more medical centers if we aren't using all the ones we have
			foreach (UnitEx medicalCenter in owner.units.medicalCenters)
			{
				if (!medicalCenter.IsEnabled())
					return;
			}

			if (owner.player.GetTotalMedCenterCapacity() < owner.player.TotalPopulation())
				m_BuildMedicalCenterTask.targetCountToBuild = owner.units.medicalCenters.Count+1;
		}

		private void BuildRecreation()
		{
			// Don't build more recreation facilities if we aren't using all the ones we have
			List<UnitEx> recreations = new List<UnitEx>(owner.units.recreationFacilities);
			recreations.AddRange(owner.units.forums);

			foreach (UnitEx recreation in recreations)
			{
				if (!recreation.IsEnabled())
					return;
			}

			if (owner.player.GetTotalRecreationFacilityCapacity() + owner.player.GetTotalForumCapacity() >= owner.player.TotalPopulation())
				return;

			// Determine recreation type to build
			map_id recreationTypeToBuild = map_id.RecreationFacility;

			if (!owner.player.IsEden())
			{
				if (owner.player.HasTechnology(TechInfo.GetTechID(map_id.Forum)) && owner.units.recreationFacilities.Count > owner.units.forums.Count * 2)
					recreationTypeToBuild = map_id.Forum;
			}

			switch (recreationTypeToBuild)
			{
				case map_id.RecreationFacility:		m_BuildRecreationTask.targetCountToBuild = owner.units.recreationFacilities.Count+1;		break;
				case map_id.Forum:					m_BuildForumTask.targetCountToBuild = owner.units.forums.Count+1;							break;
			}
		}

		private void BuildDIRT()
		{
			// Don't build more DIRT if we aren't using all the ones we have
			foreach (UnitEx dirt in owner.units.dirts)
			{
				if (!dirt.IsEnabled())
					return;
			}

			// TODO: Calculate DIRT required per CC. Force construction at correct base

			//if (owner.player.TotalPopulation() < owner.player.GetTotalMedCenterCapacity())
			//	m_BuildDirtTask.targetCountToBuild = owner.units.dirts.Count+1;
		}
	}
}
