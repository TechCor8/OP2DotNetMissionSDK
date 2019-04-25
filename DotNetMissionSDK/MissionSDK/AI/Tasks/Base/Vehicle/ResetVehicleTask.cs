using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Vehicle
{
	/// <summary>
	/// This task checks for stuck vehicles and resets them.
	/// </summary>
	public class ResetVehicleTask : Task
	{
		private class VehicleState
		{
			public LOCATION position;
			public int tickStartedSpinningWheels;
		}

		private Dictionary<int, VehicleState> m_VehicleStates = new Dictionary<int, VehicleState>(); // <UnitID, VehicleState>


		public ResetVehicleTask() { }
		public ResetVehicleTask(PlayerInfo owner) : base(owner) { }

		public override bool IsTaskComplete()
		{
			return false;
		}

		public override void GeneratePrerequisites()
		{
		}

		protected override bool PerformTask()
		{
			foreach (UnitEx vehicle in new PlayerVehicleEnum(owner.player.playerID))
			{
				if (GetVehicleStuckDuration(vehicle) > 200)
				{
					vehicle.DoStop();
					TethysGame.AddMessage(vehicle, "Destination unreachable", owner.player.playerID, 0);
				}
			}

			// Remove unused vehicle IDs
			//m_VehicleStates.Remove(unitID);

			return true;
		}

		private int GetVehicleStuckDuration(UnitEx vehicle)
		{
			VehicleState vehicleState;
			if (!m_VehicleStates.TryGetValue(vehicle.GetStubIndex(), out vehicleState))
			{
				// If state not found, add a new one
				vehicleState = new VehicleState();
				vehicleState.tickStartedSpinningWheels = TethysGame.Tick();
				vehicleState.position = vehicle.GetPosition();
				m_VehicleStates.Add(vehicle.GetStubIndex(), vehicleState);
			}

			// Reset "spinning wheels" time if not in the move state or vehicle is actually moving
			ActionType curAction = vehicle.GetCurAction();
			if (curAction != ActionType.moMove || !vehicleState.position.Equals(vehicle.GetPosition()))
			{
				vehicleState.tickStartedSpinningWheels = TethysGame.Tick();
				vehicleState.position = vehicle.GetPosition();
			}

			return TethysGame.Tick() - vehicleState.tickStartedSpinningWheels;
		}
	}
}
