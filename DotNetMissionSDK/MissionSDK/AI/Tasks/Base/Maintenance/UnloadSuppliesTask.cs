using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class UnloadSuppliesTask : Task
	{
		private class TruckState
		{
			public ActionType lastState;
			public int tickSinceStateChanged;
		}

		private Dictionary<int, TruckState> m_TruckTickSinceStateChanged = new Dictionary<int, TruckState>(); // <UnitID, TruckState>


		public UnloadSuppliesTask() { }
		public UnloadSuppliesTask(PlayerInfo owner) : base(owner) { }

		public override bool IsTaskComplete()
		{
			// Task is complete when all trucks have unloaded metals and food
			foreach (UnitEx truck in owner.units.cargoTrucks)
			{
				switch (truck.GetCargoType())
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

		protected override bool PerformTask()
		{
			// Remove unused truck IDs
			IEnumerable<int> truckUnitIDs = owner.units.cargoTrucks.Select((UnitEx unit) => unit.GetStubIndex());
			foreach (int unitID in m_TruckTickSinceStateChanged.Keys.Except(truckUnitIDs).ToList())
				m_TruckTickSinceStateChanged.Remove(unitID);

			// Unload trucks
			foreach (UnitEx truck in owner.units.cargoTrucks)
			{
				// Do nothing if truck is docking
				if (truck.GetCurAction() == ActionType.moObjDocking)
					continue;

				switch (truck.GetCargoType())
				{
					case TruckCargo.CommonMetal:
						UnitEx smelter = GetClosestUnitOfType(map_id.CommonOreSmelter, truck.GetPosition());
						if (smelter != null)
						{
							if (!truck.IsOnDock(smelter))
								truck.DoDock(smelter);
							else
							{
								smelter.DoUnloadCargo();
								FixTruckUnloading(truck);
							}
						}
						break;

					case TruckCargo.RareMetal:
						UnitEx rareSmelter = GetClosestUnitOfType(map_id.RareOreSmelter, truck.GetPosition());
						if (rareSmelter != null)
						{
							if (!truck.IsOnDock(rareSmelter))
								truck.DoDock(rareSmelter);
							else
							{
								rareSmelter.DoUnloadCargo();
								FixTruckUnloading(truck);
							}
						}
						break;

					case TruckCargo.Food:
						UnitEx agridome = GetClosestUnitOfType(map_id.Agridome, truck.GetPosition());
						if (agridome != null && agridome.IsEnabled())
						{
							if (!truck.IsOnDock(agridome))
								truck.DoDock(agridome);
							else
							{
								agridome.DoUnloadCargo();
								FixTruckUnloading(truck);
							}
						}
						break;
				}
			}

			return true;
		}

		private void FixTruckUnloading(UnitEx truck)
		{
			if (truck.GetCurAction() != ActionType.moDone)
				return;

			int stateDuration = TethysGame.Tick() - GetTruckStateTime(truck);
			if (stateDuration > 8)
				truck.DoMove(truck.GetTileX(), truck.GetTileY()+1);
		}

		private int GetTruckStateTime(UnitEx truck)
		{
			TruckState truckState;
			if (!m_TruckTickSinceStateChanged.TryGetValue(truck.GetStubIndex(), out truckState))
			{
				// If truck state not found, add a new one
				truckState = new TruckState();
				truckState.lastState = truck.GetCurAction();
				truckState.tickSinceStateChanged = TethysGame.Tick();
				m_TruckTickSinceStateChanged.Add(truck.GetStubIndex(), truckState);
			}

			// Update state if it has changed
			ActionType curAction = truck.GetCurAction();
			if (truckState.lastState != curAction)
			{
				truckState.lastState = curAction;
				truckState.tickSinceStateChanged = TethysGame.Tick();
			}

			return truckState.tickSinceStateChanged;
		}

		private UnitEx GetClosestUnitOfType(map_id type, LOCATION position)
		{
			UnitEx closestUnit = null;
			int closestDistance = 999999;

			foreach (UnitEx unit in owner.units.GetListForType(type))
			{
				int distance = unit.GetPosition().GetDiagonalDistance(position);
				if (distance < closestDistance)
				{
					closestUnit = unit;
					closestDistance = distance;
				}
			}

			return closestUnit;
		}
	}
}
