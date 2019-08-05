using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Pathfinding;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class MaintainDefenseTask : Task
	{
		private const int GuardPostsPerBase = 6;

		private List<Task> m_Prerequisites = new List<Task>();

		private BuildGuardPostTask m_BuildGuardPostTask;
		private BuildMeteorDefenseTask m_BuildMeteorDefenseTask;


		public MaintainDefenseTask() { }
		public MaintainDefenseTask(PlayerInfo owner) : base(owner) { }

		public override bool IsTaskComplete()
		{
			foreach (Task task in m_Prerequisites)
			{
				if (!task.IsTaskComplete())
					return false;
			}

			// Build guard posts at each base
			foreach (UnitEx cc in owner.units.commandCenters)
			{
				// Get all connected structures at this base so we can use the count to determine the appropriate amount of defense
				List<UnitEx> connectedStructures = owner.commandGrid.GetConnectedStructures(cc.GetPosition());

				// Do not build more guard posts if there are disconnected ones. This is to prevent earthworkers from becoming preoccupied.
				foreach (UnitEx guardPost in owner.units.guardPosts)
				{
					if (!owner.commandGrid.ConnectsTo(guardPost.GetPosition()))
						return false;
				}

				// Get number of guard posts connected to this base. If we have fewer than desired, build another one at this base
				int guardPosts = connectedStructures.FindAll((UnitEx unit) => unit.GetUnitType() == map_id.GuardPost).Count;

				if (guardPosts < GuardPostsPerBase)
				{
					m_BuildGuardPostTask.targetCountToBuild = owner.units.guardPosts.Count+1;
					m_BuildGuardPostTask.kitTask.RandomizeTurret(false);
					m_BuildGuardPostTask.SetLocation(cc.GetPosition());
				}

				// Build meteor defenses at this base. If task is null, we are plymouth and can't build these
				if (m_BuildMeteorDefenseTask != null)
				{
					int meteorDefenses = connectedStructures.FindAll((UnitEx unit) => unit.GetUnitType() == map_id.MeteorDefense).Count;
					if (meteorDefenses >= (connectedStructures.Count / 7) + 1)
						continue;

					m_BuildMeteorDefenseTask.targetCountToBuild = m_BuildMeteorDefenseTask.targetCountToBuild+1;
					m_BuildMeteorDefenseTask.SetLocation(cc.GetPosition());
				}
			}
			
			return true;
		}

		public override void GeneratePrerequisites()
		{
			m_Prerequisites.Add(m_BuildGuardPostTask = new BuildGuardPostTask());
			
			if (owner.player.IsEden())
			{
				m_Prerequisites.Add(new BuildObservatoryTask());
				m_Prerequisites.Add(m_BuildMeteorDefenseTask = new BuildMeteorDefenseTask());
			}

			foreach (Task task in m_Prerequisites)
				AddPrerequisite(task, false);

			m_BuildGuardPostTask.targetCountToBuild = 0;
			m_BuildGuardPostTask.kitTask.RandomizeTurret(false);
		}

		protected override bool PerformTask()
		{
			return true;
		}
	}
}
