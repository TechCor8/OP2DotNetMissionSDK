using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of maintaining structure factories.
	/// </summary>
	public class MaintainStructureFactoryGoal : Goal
	{
		private MaintainStructureFactoryTask m_MaintainTask;


		public MaintainStructureFactoryGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			m_Task = m_MaintainTask = new MaintainStructureFactoryTask(ownerID);
			m_Task.GeneratePrerequisites();
		}

		/// <summary>
		/// Updates the importance of this goal.
		/// </summary>
		public override void UpdateImportance(StateSnapshot stateSnapshot, PlayerState owner)
		{
			// If we don't have a command center, we don't need a structure factory
			if (owner.units.commandCenters.Count == 0)
			{
				importance = 0;
				return;
			}

			// If we can't build a structure factory, this goal is useless
			if (owner.units.structureFactories.Count == 0 && owner.units.convecs.FirstOrDefault((convec) => convec.cargoType == map_id.StructureFactory) == null)
			{
				importance = 0;
				return;
			}

			// If we have a structure factory but don't have enough resources to repair or connect it, make it low priority
			if (owner.ore < 100 && owner.units.structureFactories.Count > 0)
			{
				importance = 0.25f;
				importance = Clamp(importance * weight);
				return;
			}

			int healthyFactories = 0;
			int maxHitPoints = owner.structureInfo[map_id.StructureFactory].hitPoints;

			// Get number of healthy, connected factories
			foreach (StructureState factory in owner.units.structureFactories)
			{
				if (factory.damage / maxHitPoints >= RepairStructureTask.CriticalDamagePercentage)
					continue;

				if (!stateSnapshot.commandMap.ConnectsTo(m_OwnerID, factory.GetRect()))
					continue;

				++healthyFactories;
			}

			m_MaintainTask.targetCountToMaintain = owner.units.commandCenters.Count;

			// Ideally, we'll have 1 structure factory per CC
			importance = 1 - (healthyFactories / (float)owner.units.commandCenters.Count);

			importance = Clamp(importance * weight);
		}

		/// <summary>
		/// Performs this goal's task.
		/// </summary>
		public override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			if (m_Task.IsTaskComplete(stateSnapshot))
				return true;

			PlayerState owner = stateSnapshot.players[m_OwnerID];

			if (owner.units.structureFactories.Count < owner.units.commandCenters.Count)
			{
				// Get unoccupied cc
				List<StructureState> ccWithoutFactory = new List<StructureState>(owner.units.commandCenters);

				foreach (StructureState factory in owner.units.structureFactories)
				{
					StructureState cc = (StructureState)owner.units.GetClosestUnitOfType(map_id.CommandCenter, factory.position);
					if (cc != null)
						ccWithoutFactory.Remove(cc);
				}

				// Build next to unoccupied CC
				m_MaintainTask.buildTask.SetLocation(ccWithoutFactory[0].position);
			}

			return m_Task.PerformTaskTree(stateSnapshot, unitActions);
		}
	}
}
