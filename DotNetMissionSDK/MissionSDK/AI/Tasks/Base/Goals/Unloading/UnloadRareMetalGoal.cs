using DotNetMissionSDK.AI.Tasks.Base.Unloading;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of unloading supplies.
	/// </summary>
	public class UnloadRareMetalGoal : Goal
	{
		public UnloadRareMetalGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			m_Task = new UnloadRareMetalTask(ownerID);
			m_Task.GeneratePrerequisites();
		}

		/// <summary>
		/// Updates the importance of this goal.
		/// </summary>
		public override void UpdateImportance(StateSnapshot stateSnapshot, PlayerState owner)
		{
			bool hasTruckWithCargo = false;

			foreach (CargoTruckState truck in owner.units.cargoTrucks)
			{
				if (truck.cargoType == TruckCargo.RareMetal)
				{
					hasTruckWithCargo = true;
					break;
				}
			}

			if (!hasTruckWithCargo)
			{
				importance = 0;
				return;
			}

			if (owner.maxRareOre == 0)
			{
				importance = 0;
				return;
			}

			// Importance increases as reserves dwindle
			importance = 1 - Clamp(owner.rareOre / owner.maxRareOre);

			importance = Clamp(importance * weight);
		}

		/// <summary>
		/// Performs this goal's task.
		/// </summary>
		public override TaskResult PerformTask(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[m_OwnerID];

			// Don't perform this task if metals will be lost
			if (owner.rareOre > owner.maxRareOre - 1000)
				return new TaskResult(TaskRequirements.None);

			return base.PerformTask(stateSnapshot, restrictedRequirements, unitActions);
		}
	}
}
