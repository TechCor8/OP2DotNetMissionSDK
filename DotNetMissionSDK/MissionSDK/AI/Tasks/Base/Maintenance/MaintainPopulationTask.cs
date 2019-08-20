using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;
using System;
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
		

		public MaintainPopulationTask(int ownerID) : base(ownerID) { }
		
		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			foreach (Task task in m_Prerequisites)
			{
				if (!task.IsTaskComplete(stateSnapshot))
					return false;
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			m_Prerequisites.Add(new BuildNurseryTask(ownerID));
			m_Prerequisites.Add(new BuildUniversityTask(ownerID));
			m_Prerequisites.Add(m_BuildResidenceTask = new BuildResidenceTask(ownerID));
			m_Prerequisites.Add(m_BuildEdenResidenceTask = new BuildAdvancedResidenceTask(ownerID));
			m_Prerequisites.Add(m_BuildPlymouthResidenceTask = new BuildReinforcedResidenceTask(ownerID));
			m_Prerequisites.Add(m_BuildMedicalCenterTask = new BuildMedicalCenterTask(ownerID));
			m_Prerequisites.Add(new BuildStandardLabTask(ownerID));
			m_Prerequisites.Add(new BuildAdvancedLabTask(ownerID));
			m_Prerequisites.Add(new BuildRobotCommandTask(ownerID));
			m_Prerequisites.Add(m_BuildRecreationTask = new BuildRecreationTask(ownerID));
			m_Prerequisites.Add(m_BuildForumTask = new BuildForumTask(ownerID));
			m_Prerequisites.Add(m_BuildDirtTask = new BuildDIRTTask(ownerID));
			m_Prerequisites.Add(new BuildGORFTask(ownerID));

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

		protected override bool PerformTask(StateSnapshot stateSnapshot, List<Action> unitActions)
		{
			return true;
		}

		public void UpdateRequirements(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			BuildResidence(stateSnapshot, owner);
			BuildMedicalCenter(owner);
			BuildRecreation(stateSnapshot, owner);
			BuildDIRT(stateSnapshot, owner);
		}

		private void BuildResidence(StateSnapshot stateSnapshot, PlayerState owner)
		{
			// Don't build more residences if we aren't using all the ones we have
			List<StructureState> residences = new List<StructureState>(owner.units.residences);
			residences.AddRange(owner.units.advancedResidences);
			residences.AddRange(owner.units.reinforcedResidences);

			foreach (StructureState residence in residences)
			{
				if (!residence.isEnabled)
					return;
			}

			if (owner.totalResidenceCapacity >= owner.totalPopulation)
				return;

			// Determine residence type to build
			map_id residenceTypeToBuild = map_id.Residence;

			if (owner.isEden)
			{
				if (owner.HasTechnologyForUnit(stateSnapshot, map_id.AdvancedResidence) && owner.units.residences.Count > owner.units.advancedResidences.Count * 2)
					residenceTypeToBuild = map_id.AdvancedResidence;
			}
			else
			{
				if (owner.HasTechnologyForUnit(stateSnapshot, map_id.ReinforcedResidence) && owner.units.residences.Count > owner.units.reinforcedResidences.Count * 2)
					residenceTypeToBuild = map_id.ReinforcedResidence;
			}

			switch (residenceTypeToBuild)
			{
				case map_id.Residence:				m_BuildResidenceTask.targetCountToBuild = owner.units.residences.Count+1;						break;
				case map_id.AdvancedResidence:		m_BuildEdenResidenceTask.targetCountToBuild = owner.units.advancedResidences.Count+1;			break;
				case map_id.ReinforcedResidence:	m_BuildPlymouthResidenceTask.targetCountToBuild = owner.units.reinforcedResidences.Count+1;		break;
			}
		}

		private void BuildMedicalCenter(PlayerState owner)
		{
			// Don't build more medical centers if we aren't using all the ones we have
			foreach (StructureState medicalCenter in owner.units.medicalCenters)
			{
				if (!medicalCenter.isEnabled)
					return;
			}

			if (owner.totalMedCenterCapacity < owner.totalPopulation)
				m_BuildMedicalCenterTask.targetCountToBuild = owner.units.medicalCenters.Count+1;
		}

		private void BuildRecreation(StateSnapshot stateSnapshot, PlayerState owner)
		{
			// Don't build more recreation facilities if we aren't using all the ones we have
			List<StructureState> recreations = new List<StructureState>(owner.units.recreationFacilities);
			recreations.AddRange(owner.units.forums);

			foreach (StructureState recreation in recreations)
			{
				if (!recreation.isEnabled)
					return;
			}

			if (owner.totalRecreationFacilityCapacity + owner.totalForumCapacity >= owner.totalPopulation)
				return;

			// Determine recreation type to build
			map_id recreationTypeToBuild = map_id.RecreationFacility;

			if (!owner.isEden)
			{
				if (owner.HasTechnologyForUnit(stateSnapshot, map_id.Forum) && owner.units.recreationFacilities.Count > owner.units.forums.Count * 2)
					recreationTypeToBuild = map_id.Forum;
			}

			switch (recreationTypeToBuild)
			{
				case map_id.RecreationFacility:		m_BuildRecreationTask.targetCountToBuild = owner.units.recreationFacilities.Count+1;		break;
				case map_id.Forum:					m_BuildForumTask.targetCountToBuild = owner.units.forums.Count+1;							break;
			}
		}

		private void BuildDIRT(StateSnapshot stateSnapshot, PlayerState owner)
		{
			m_BuildDirtTask.targetCountToBuild = 0;

			// Don't build more DIRT if we aren't using all the ones we have
			foreach (StructureState dirt in owner.units.dirts)
			{
				if (!dirt.isEnabled)
					return;
			}

			StructureInfo dirtInfo = owner.structureInfo[map_id.DIRT];
			int productionCap = dirtInfo.productionCapacity;

			bool shouldBuildDIRT = false;

			// Calculate DIRT required per CC. Force construction at correct base
			foreach (StructureState cc in owner.units.commandCenters)
			{
				// Get needed DIRT count
				List<StructureState> connectedStructures = stateSnapshot.commandMap.GetConnectedStructures(ownerID, cc.position);
				int neededDIRTs = connectedStructures.Count / productionCap + 1;

				m_BuildDirtTask.targetCountToBuild += neededDIRTs;

				// Get DIRTs connected to this CC
				int currentDIRTs = 0;
				foreach (StructureState building in connectedStructures)
				{
					if (building.unitType != map_id.DIRT)
						continue;

					++currentDIRTs;
				}

				// If already planning to build DIRT, don't set a new location
				if (shouldBuildDIRT)
					continue;

				if (currentDIRTs >= neededDIRTs)
					continue;

				// Add DIRT to this CC
				m_BuildDirtTask.SetLocation(cc.position);
				shouldBuildDIRT = true;
			}
		}
	}
}
