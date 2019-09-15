using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using DotNetMissionSDK.State.Snapshot.UnitTypeInfo;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class MaintainMoraleTask : Task
	{
		private List<MaintainStructureTask> m_Prerequisites = new List<MaintainStructureTask>();

		private MaintainAgridomeTask m_BuildAgridomeTask;
		private MaintainResidenceTask m_BuildResidenceTask;
		private MaintainAdvancedResidenceTask m_BuildEdenResidenceTask;
		private MaintainReinforcedResidenceTask m_BuildPlymouthResidenceTask;
		private MaintainMedicalCenterTask m_BuildMedicalCenterTask;
		private MaintainRecreationTask m_BuildRecreationTask;
		private MaintainForumTask m_BuildForumTask;
		private MaintainDIRTTask m_BuildDirtTask;
		

		public MaintainMoraleTask(int ownerID) : base(ownerID) { }
		
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
			m_Prerequisites.Add(new MaintainNurseryTask(ownerID));
			m_Prerequisites.Add(new MaintainUniversityTask(ownerID));
			m_Prerequisites.Add(m_BuildAgridomeTask = new MaintainAgridomeTask(ownerID));
			m_Prerequisites.Add(m_BuildResidenceTask = new MaintainResidenceTask(ownerID));
			m_Prerequisites.Add(m_BuildEdenResidenceTask = new MaintainAdvancedResidenceTask(ownerID));
			m_Prerequisites.Add(m_BuildPlymouthResidenceTask = new MaintainReinforcedResidenceTask(ownerID));
			m_Prerequisites.Add(m_BuildMedicalCenterTask = new MaintainMedicalCenterTask(ownerID));
			m_Prerequisites.Add(new MaintainRobotCommandTask(ownerID)); // Should find a better place for this
			m_Prerequisites.Add(m_BuildRecreationTask = new MaintainRecreationTask(ownerID));
			m_Prerequisites.Add(m_BuildForumTask = new MaintainForumTask(ownerID));
			m_Prerequisites.Add(m_BuildDirtTask = new MaintainDIRTTask(ownerID));
			m_Prerequisites.Add(new MaintainGORFTask(ownerID));

			m_BuildAgridomeTask.targetCountToMaintain = 0;
			m_BuildResidenceTask.targetCountToMaintain = 0;
			m_BuildEdenResidenceTask.targetCountToMaintain = 0;
			m_BuildPlymouthResidenceTask.targetCountToMaintain = 0;
			m_BuildMedicalCenterTask.targetCountToMaintain = 0;
			m_BuildRecreationTask.targetCountToMaintain = 0;
			m_BuildForumTask.targetCountToMaintain = 0;
			m_BuildDirtTask.targetCountToMaintain = 0;

			foreach (MaintainStructureTask task in m_Prerequisites)
				AddPrerequisite(task);
		}

		protected override TaskResult PerformTask(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions)
		{
			return new TaskResult(TaskRequirements.None);
		}

		public void UpdateRequirements(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			UpdateAgridomeCount(stateSnapshot, owner);
			UpdateResidenceCount(stateSnapshot, owner);
			UpdateMedicalCenterCount(owner);
			UpdateRecreationCenterCount(stateSnapshot, owner);
			UpdateDIRTCount(stateSnapshot, owner);
		}

		private void UpdateAgridomeCount(StateSnapshot stateSnapshot, PlayerState owner)
		{
			// Calculate number of agridomes needed for net positive
			int neededAgridomes = 1;
			int numActiveAgridomes = owner.units.agridomes.Count((StructureState unit) => unit.isEnabled);
			if (numActiveAgridomes > 0)
			{
				int foodPerAgridome = owner.totalFoodProduction / numActiveAgridomes;
				if (foodPerAgridome == 0) foodPerAgridome = 1;
				neededAgridomes = owner.totalFoodConsumption / foodPerAgridome + 1;
			}

			m_BuildAgridomeTask.targetCountToMaintain = neededAgridomes;
		}

		private void UpdateResidenceCount(StateSnapshot stateSnapshot, PlayerState owner)
		{
			StructureInfo residenceInfo = owner.structureInfo[map_id.Residence];
			StructureInfo advancedInfo = owner.structureInfo[map_id.AdvancedResidence];
			StructureInfo reinforcedInfo = owner.structureInfo[map_id.ReinforcedResidence];

			int plannedCapacity = 0;

			m_BuildResidenceTask.targetCountToMaintain = 0;
			m_BuildEdenResidenceTask.targetCountToMaintain = 0;
			m_BuildPlymouthResidenceTask.targetCountToMaintain = 0;

			for (int i=0; plannedCapacity < owner.totalPopulation; ++i)
			{
				if (i % 3 == 2)
				{
					// Build tech residence
					if (owner.isEden && owner.HasTechnologyForUnit(stateSnapshot, map_id.AdvancedResidence))
					{
						++m_BuildEdenResidenceTask.targetCountToMaintain;
						plannedCapacity += advancedInfo.productionCapacity;
						continue;
					}
					else if (!owner.isEden && owner.HasTechnologyForUnit(stateSnapshot, map_id.ReinforcedResidence))
					{
						++m_BuildPlymouthResidenceTask.targetCountToMaintain;
						plannedCapacity += reinforcedInfo.productionCapacity;
						continue;
					}
				}

				++m_BuildResidenceTask.targetCountToMaintain;
				plannedCapacity += residenceInfo.productionCapacity;
			}

			// If we have more residences than planned, subtract from the tech residences
			while (owner.units.residences.Count > m_BuildResidenceTask.targetCountToMaintain + 1)
			{
				m_BuildResidenceTask.targetCountToMaintain += 2;
				if (owner.isEden)
					--m_BuildEdenResidenceTask.targetCountToMaintain;
				else
					--m_BuildPlymouthResidenceTask.targetCountToMaintain;
			}

			if (m_BuildEdenResidenceTask.targetCountToMaintain < 0)		m_BuildEdenResidenceTask.targetCountToMaintain = 0;
			if (m_BuildPlymouthResidenceTask.targetCountToMaintain < 0)	m_BuildPlymouthResidenceTask.targetCountToMaintain = 0;

			// If we have more tech residences than planned, subtract from normal residences
			int techResidenceCount = owner.isEden ? owner.units.advancedResidences.Count : owner.units.reinforcedResidences.Count;
			int plannedTechCount = owner.isEden ? m_BuildEdenResidenceTask.targetCountToMaintain : m_BuildPlymouthResidenceTask.targetCountToMaintain;

			while (techResidenceCount > plannedTechCount)
			{
				if (owner.isEden)
					plannedTechCount = ++m_BuildEdenResidenceTask.targetCountToMaintain;
				else
					plannedTechCount = ++m_BuildPlymouthResidenceTask.targetCountToMaintain;

				--m_BuildResidenceTask.targetCountToMaintain;
			}

			if (m_BuildResidenceTask.targetCountToMaintain < 0)			m_BuildResidenceTask.targetCountToMaintain = 0;
		}

		private void UpdateMedicalCenterCount(PlayerState owner)
		{
			StructureInfo info = owner.structureInfo[map_id.MedicalCenter];
			
			int plannedCapacity = 0;

			m_BuildMedicalCenterTask.targetCountToMaintain = 0;
			
			for (int i=0; plannedCapacity < owner.totalPopulation; ++i)
			{
				++m_BuildMedicalCenterTask.targetCountToMaintain;
				plannedCapacity += info.productionCapacity;
			}
		}

		private void UpdateRecreationCenterCount(StateSnapshot stateSnapshot, PlayerState owner)
		{
			StructureInfo recreationInfo = owner.structureInfo[map_id.RecreationFacility];
			StructureInfo forumInfo = owner.structureInfo[map_id.Forum];

			int plannedCapacity = 0;

			m_BuildRecreationTask.targetCountToMaintain = 0;
			m_BuildForumTask.targetCountToMaintain = 0;

			for (int i=0; plannedCapacity < owner.totalPopulation; ++i)
			{
				if (i % 2 == 1)
				{
					// Build forum
					if (!owner.isEden && owner.HasTechnologyForUnit(stateSnapshot, map_id.Forum))
					{
						++m_BuildForumTask.targetCountToMaintain;
						plannedCapacity += forumInfo.productionCapacity;
						continue;
					}
				}

				++m_BuildRecreationTask.targetCountToMaintain;
				plannedCapacity += recreationInfo.productionCapacity;
			}

			// If we have more recreation than planned, subtract from the forums
			while (owner.units.recreationFacilities.Count > m_BuildRecreationTask.targetCountToMaintain + 1)
			{
				m_BuildRecreationTask.targetCountToMaintain += 2;
				--m_BuildForumTask.targetCountToMaintain;
			}

			if (m_BuildForumTask.targetCountToMaintain < 0)
				m_BuildForumTask.targetCountToMaintain = 0;

			// If we have more forums than planned, subtract from recreation
			while (owner.units.forums.Count > m_BuildForumTask.targetCountToMaintain)
			{
				++m_BuildForumTask.targetCountToMaintain;
				--m_BuildRecreationTask.targetCountToMaintain;
			}

			if (m_BuildRecreationTask.targetCountToMaintain < 0)
				m_BuildRecreationTask.targetCountToMaintain = 0;
		}

		private void UpdateDIRTCount(StateSnapshot stateSnapshot, PlayerState owner)
		{
			m_BuildDirtTask.targetCountToMaintain = 0;

			StructureInfo dirtInfo = owner.structureInfo[map_id.DIRT];
			int productionCap = dirtInfo.productionCapacity;

			bool shouldBuildDIRT = false;

			// Calculate DIRT required per CC. Force construction at correct base
			foreach (StructureState cc in owner.units.commandCenters)
			{
				// Get needed DIRT count
				List<StructureState> connectedStructures = stateSnapshot.commandMap.GetConnectedStructures(ownerID, cc.position);
				int neededDIRTs = connectedStructures.Count / productionCap + 1;

				m_BuildDirtTask.targetCountToMaintain += neededDIRTs;

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
				m_BuildDirtTask.buildTask.SetLocation(cc.position);
				shouldBuildDIRT = true;
			}
		}
	}
}
