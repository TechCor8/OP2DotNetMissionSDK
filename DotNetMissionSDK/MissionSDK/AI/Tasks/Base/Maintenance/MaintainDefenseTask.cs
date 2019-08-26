using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class MaintainDefenseTask : Task
	{
		private List<Task> m_Prerequisites = new List<Task>();

		private BuildGuardPostTask m_BuildGuardPostTask;
		private BuildMeteorDefenseTask m_BuildMeteorDefenseTask;


		public MaintainDefenseTask(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			foreach (Task task in m_Prerequisites)
			{
				if (!task.IsTaskComplete(stateSnapshot))
					return false;
			}

			PlayerState owner = stateSnapshot.players[ownerID];

			UnitState ccWithLeastGuards = null;
			int leastGuardCount = 99999;

			// Build guard posts at each base
			foreach (UnitState cc in owner.units.commandCenters)
			{
				// Get all connected structures at this base so we can use the count to determine the appropriate amount of defense
				List<StructureState> connectedStructures = stateSnapshot.commandMap.GetConnectedStructures(ownerID, cc.position);

				// Do not build more guard posts if there are disconnected ones. This is to prevent earthworkers from becoming preoccupied.
				foreach (UnitState guardPost in owner.units.guardPosts)
				{
					if (!stateSnapshot.commandMap.ConnectsTo(ownerID, guardPost.position))
						return false;
				}

				// Get number of guard posts connected to this base. If we have fewer than desired, build another one at this base
				int guardPosts = connectedStructures.FindAll((StructureState unit) => unit.unitType == map_id.GuardPost).Count;

				if (guardPosts < leastGuardCount)
				{
					ccWithLeastGuards = cc;
					leastGuardCount = guardPosts;
				}

				// Build meteor defenses at this base. If task is null, we are plymouth and can't build these
				if (m_BuildMeteorDefenseTask != null)
				{
					int meteorDefenses = connectedStructures.FindAll((StructureState unit) => unit.unitType == map_id.MeteorDefense).Count;
					if (meteorDefenses >= (connectedStructures.Count / 7) + 1)
						continue;

					m_BuildMeteorDefenseTask.targetCountToBuild = m_BuildMeteorDefenseTask.targetCountToBuild+1;
					m_BuildMeteorDefenseTask.SetLocation(cc.position);
				}
			}

			if (ccWithLeastGuards != null)
			{
				m_BuildGuardPostTask.targetCountToBuild = owner.units.guardPosts.Count+1;
				m_BuildGuardPostTask.kitTask.RandomizeTurret(false);
				m_BuildGuardPostTask.SetLocation(ccWithLeastGuards.position);
			}
			
			return true;
		}

		public override void GeneratePrerequisites()
		{
			m_Prerequisites.Add(m_BuildGuardPostTask = new BuildGuardPostTask(ownerID));
			
			if (GameState.players[ownerID].IsEden())
			{
				m_Prerequisites.Add(new BuildObservatoryTask(ownerID));
				m_Prerequisites.Add(m_BuildMeteorDefenseTask = new BuildMeteorDefenseTask(ownerID));
			}

			foreach (Task task in m_Prerequisites)
				AddPrerequisite(task, false);

			m_BuildGuardPostTask.targetCountToBuild = 0;
			m_BuildGuardPostTask.kitTask.RandomizeTurret(false);
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			return true;
		}
	}
}
