using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.VehicleTasks
{
	/// <summary>
	/// This task checks for stuck vehicles and resets them.
	/// </summary>
	public class ResetVehicleTask : Task
	{
		private class VehicleState
		{
			public LOCATION position;
			public int timeStartedSpinningWheels;
		}

		private Dictionary<int, VehicleState> m_VehicleStates = new Dictionary<int, VehicleState>(); // <UnitID, VehicleState>


		public ResetVehicleTask(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			return false;
		}

		public override void GeneratePrerequisites()
		{
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, BotCommands unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			foreach (UnitState vehicle in owner.units.GetVehicles())
			{
				if (GetVehicleStuckDuration(vehicle, stateSnapshot.time) > 50)
				{
					unitActions.AddUnitCommand(vehicle.unitID, 0, () =>
					{
						UnitEx unit = GameState.GetUnit(vehicle.unitID);
						if (unit == null)
							return;

						unit.DoStop();
						TethysGame.AddMessage(unit, "Destination unreachable", ownerID, 0);
					});
				}
			}

			// Remove unused vehicle IDs
			//m_VehicleStates.Remove(unitID);

			return true;
		}

		private int GetVehicleStuckDuration(UnitState vehicle, int time)
		{
			VehicleState vehicleState;
			if (!m_VehicleStates.TryGetValue(vehicle.unitID, out vehicleState))
			{
				// If state not found, add a new one
				vehicleState = new VehicleState();
				vehicleState.timeStartedSpinningWheels = time;
				vehicleState.position = vehicle.position;
				m_VehicleStates.Add(vehicle.unitID, vehicleState);
			}

			// Reset "spinning wheels" time if not in the move state or vehicle is actually moving
			ActionType curAction = vehicle.curAction;
			if (curAction != ActionType.moMove || !vehicleState.position.Equals(vehicle.position))
			{
				vehicleState.timeStartedSpinningWheels = time;
				vehicleState.position = vehicle.position;
			}

			return time - vehicleState.timeStartedSpinningWheels;
		}
	}
}
