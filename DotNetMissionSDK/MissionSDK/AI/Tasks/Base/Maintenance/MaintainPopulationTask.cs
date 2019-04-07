using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class MaintainPopulationTask : Task
	{
		private List<Task> m_Prerequisites = new List<Task>();

		private BuildResidenceTask m_BuildResidenceTask;
		private BuildMedicalCenterTask m_BuildMedicalCenterTask;


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
			m_Prerequisites.Add(m_BuildMedicalCenterTask = new BuildMedicalCenterTask());

			foreach (Task task in m_Prerequisites)
				AddPrerequisite(task);
		}

		protected override bool PerformTask()
		{
			BuildResidence();
			BuildMedicalCenter();

			return true;
		}

		private void BuildResidence()
		{
			// Don't build more residences if we aren't using all the ones we have
			foreach (UnitEx residence in owner.units.residences)
			{
				if (!residence.IsEnabled())
					return;
			}

			m_BuildResidenceTask.targetCountToBuild = 1;
		}

		private void BuildMedicalCenter()
		{
			// Don't build more medical centers if we aren't using all the ones we have
			foreach (UnitEx medicalCenter in owner.units.medicalCenters)
			{
				if (!medicalCenter.IsEnabled())
					return;
			}

			m_BuildMedicalCenterTask.targetCountToBuild = 1;
		}
	}
}
