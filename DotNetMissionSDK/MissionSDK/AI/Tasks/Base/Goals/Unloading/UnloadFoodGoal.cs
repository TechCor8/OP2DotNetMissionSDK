using DotNetMissionSDK.AI.Tasks.Base.Unloading;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;

namespace DotNetMissionSDK.AI.Tasks.Base.Goals
{
	/// <summary>
	/// Controls the importance of unloading supplies.
	/// </summary>
	public class UnloadFoodGoal : Goal
	{
		public UnloadFoodGoal(int ownerID, float weight) : base(ownerID, weight)
		{
			m_Task = new UnloadFoodTask(ownerID);
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
				if (truck.cargoType == TruckCargo.Food)
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

			// Importance increases as reserves dwindle
			importance = 1 - Clamp(owner.foodStored / 10000);

			importance = Clamp(importance * weight);
		}
	}
}
