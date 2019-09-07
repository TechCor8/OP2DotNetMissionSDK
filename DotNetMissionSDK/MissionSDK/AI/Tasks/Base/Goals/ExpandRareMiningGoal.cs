using DotNetMissionSDK.AI.Managers;
using DotNetMissionSDK.AI.Tasks.Base.Mining;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of expanding rare mining operations.
	/// </summary>
	public class ExpandRareMiningGoal : Goal
	{
		public ExpandRareMiningGoal(int ownerID, MiningBaseState miningBaseState, float weight) : base(ownerID, weight)
		{
			m_Task = new ExpandRareMiningTask(ownerID, miningBaseState);
			m_Task.GeneratePrerequisites();
		}

		/// <summary>
		/// Updates the importance of this goal.
		/// </summary>
		public override void UpdateImportance(StateSnapshot stateSnapshot, PlayerState owner)
		{
			// Importance increases as reserves dwindle
			importance = 1 - Clamp(owner.rareOre / 10000.0f);

			importance = Clamp(importance * weight);
		}

		/// <summary>
		/// Performs this goal's task.
		/// </summary>
		public override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[m_OwnerID];

			// Don't expand if smelters are disabled
			foreach (StructureState structure in owner.units.rareOreSmelters)
			{
				if (!structure.isEnabled && !structure.isCritical && stateSnapshot.commandMap.ConnectsTo(m_OwnerID, structure.position))
					return true;
			}

			return base.PerformTask(stateSnapshot, unitActions);
		}
	}
}
