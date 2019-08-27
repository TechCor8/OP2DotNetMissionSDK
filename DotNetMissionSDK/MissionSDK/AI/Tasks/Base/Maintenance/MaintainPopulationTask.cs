using DotNetMissionSDK.AI.Tasks.Base.Structure;
using DotNetMissionSDK.State.Snapshot;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	/// <summary>
	/// Maintains population growth with a nursery and university.
	/// </summary>
	public class MaintainPopulationTask : Task
	{
		public MaintainPopulationTask(int ownerID) : base(ownerID) { }
		
		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			return owner.units.nurseries.Count > 0 && owner.units.universities.Count > 0;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(new MaintainNurseryTask(ownerID));
			AddPrerequisite(new MaintainUniversityTask(ownerID));
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			return true;
		}
	}
}
