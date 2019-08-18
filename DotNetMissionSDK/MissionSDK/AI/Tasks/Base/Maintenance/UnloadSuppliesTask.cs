using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class UnloadSuppliesTask : Task
	{
		private class TruckState
		{
			public ActionType lastState;
			public int timeSinceStateChanged;
		}

		private Dictionary<int, TruckState> m_TruckTimeSinceStateChanged = new Dictionary<int, TruckState>(); // <UnitID, TruckState>


		public UnloadSuppliesTask(int ownerID) : base(ownerID) { }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Task is complete when all trucks have unloaded metals and food
			foreach (CargoTruckState truck in owner.units.cargoTrucks)
			{
				switch (truck.cargoType)
				{
					case TruckCargo.CommonMetal:
						if (owner.units.commonOreSmelters.Count > 0)
							return false;

						break;

					case TruckCargo.RareMetal:
						if (owner.units.rareOreSmelters.Count > 0)
							return false;

						break;

					case TruckCargo.Food:
						if (owner.units.agridomes.Count > 0)
							return false;

						break;
				}
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, List<Action> unitActions)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Remove unused truck IDs
			IEnumerable<int> truckUnitIDs = owner.units.cargoTrucks.Select((CargoTruckState unit) => unit.unitID);
			foreach (int unitID in m_TruckTimeSinceStateChanged.Keys.Except(truckUnitIDs).ToList())
				m_TruckTimeSinceStateChanged.Remove(unitID);

			// Unload trucks
			foreach (CargoTruckState truck in owner.units.cargoTrucks)
			{
				// Do nothing if truck is docking
				if (truck.curAction == ActionType.moObjDocking)
					continue;

				switch (truck.cargoType)
				{
					case TruckCargo.CommonMetal:
						StructureState smelter = (StructureState)owner.units.GetClosestUnitOfType(map_id.CommonOreSmelter, truck.position);
						if (smelter != null)
						{
							if (!truck.IsOnDock(smelter))
								unitActions.Add(() =>
								{
									UnitEx liveSmelter = GameState.GetUnit(smelter.unitID);
									if (liveSmelter != null)
										GameState.GetUnit(truck.unitID)?.DoDock(liveSmelter);
								});
							else
							{
								unitActions.Add(() => GameState.GetUnit(smelter.unitID)?.DoUnloadCargo());
								FixTruckUnloading(stateSnapshot, unitActions, truck);
							}
						}
						break;

					case TruckCargo.RareMetal:
						StructureState rareSmelter = (StructureState)owner.units.GetClosestUnitOfType(map_id.RareOreSmelter, truck.position);
						if (rareSmelter != null)
						{
							if (!truck.IsOnDock(rareSmelter))
							{
								unitActions.Add(() =>
								{
									UnitEx liveSmelter = GameState.GetUnit(rareSmelter.unitID);
									if (liveSmelter != null)
										GameState.GetUnit(truck.unitID)?.DoDock(liveSmelter);
								});
							}
							else
							{
								unitActions.Add(() => GameState.GetUnit(rareSmelter.unitID)?.DoUnloadCargo());
								FixTruckUnloading(stateSnapshot, unitActions, truck);
							}
						}
						break;

					case TruckCargo.Food:
						StructureState agridome = (StructureState)owner.units.GetClosestUnitOfType(map_id.Agridome, truck.position);
						if (agridome != null && agridome.isEnabled)
						{
							if (!truck.IsOnDock(agridome))
							{
								unitActions.Add(() =>
								{
									UnitEx liveAgridome = GameState.GetUnit(agridome.unitID);
									if (liveAgridome != null)
										GameState.GetUnit(truck.unitID)?.DoDock(liveAgridome);
								});
							}
							else
							{
								unitActions.Add(() => GameState.GetUnit(agridome.unitID)?.DoUnloadCargo());
								FixTruckUnloading(stateSnapshot, unitActions, truck);
							}
						}
						break;
				}
			}

			return true;
		}

		private void FixTruckUnloading(StateSnapshot stateSnapshot, List<Action> unitActions, CargoTruckState truck)
		{
			if (truck.curAction != ActionType.moDone)
				return;

			int stateDuration = stateSnapshot.time - GetTruckStateTime(stateSnapshot, truck);
			if (stateDuration > 8)
				unitActions.Add(() => GameState.GetUnit(truck.unitID)?.DoMove(truck.position.x, truck.position.y+1));
		}

		private int GetTruckStateTime(StateSnapshot stateSnapshot, CargoTruckState truck)
		{
			TruckState truckState;
			if (!m_TruckTimeSinceStateChanged.TryGetValue(truck.unitID, out truckState))
			{
				// If truck state not found, add a new one
				truckState = new TruckState();
				truckState.lastState = truck.curAction;
				truckState.timeSinceStateChanged = stateSnapshot.time;
				m_TruckTimeSinceStateChanged.Add(truck.unitID, truckState);
			}

			// Update state if it has changed
			ActionType curAction = truck.curAction;
			if (truckState.lastState != curAction)
			{
				truckState.lastState = curAction;
				truckState.timeSinceStateChanged = stateSnapshot.time;
			}

			return truckState.timeSinceStateChanged;
		}
	}
}
