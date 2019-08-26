using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.State.Snapshot;
using System;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	/// <summary>
	/// Makes sure there are enough labs for scientists.
	/// </summary>
	public class MaintainResearchTask : Task
	{
		private BuildAdvancedLabTask m_BuildAdvancedLabTask;

		
		public MaintainResearchTask(int ownerID) : base(ownerID) { }
		
		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// If scientist count exceeds advanced lab count
			int scientistsForResearch = owner.numAvailableScientists + owner.numScientistsAssignedToResearch;
			scientistsForResearch -= 16; // Standard lab
			scientistsForResearch -= 16; // Advanced lab (first one)

			// If we have extra scientists, build more advanced labs
			if (scientistsForResearch > 8)
				m_BuildAdvancedLabTask.targetCountToBuild = (int)Math.Ceiling(scientistsForResearch / 16.0f) + 1;

			return owner.units.standardLabs.Count > 0 && owner.units.advancedLabs.Count >= m_BuildAdvancedLabTask.targetCountToBuild;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new BuildStandardLabTask(ownerID));
			AddPrerequisite(m_BuildAdvancedLabTask = new BuildAdvancedLabTask(ownerID));
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			return true;
		}
	}
}
