using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMissionSDK.AI.Tasks.Base.Maintenance
{
	public class UnloadSuppliesTask : Task
	{
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
			foreach (UnitEx truck in owner.units.cargoTrucks)
			{
				if (truck.GetCurAction() == ActionType.moObjDocking)
					continue;

				switch (truck.GetCargoType())
				{
					case TruckCargo.CommonMetal:
						UnitEx smelter = GetClosestUnitOfType(map_id.CommonOreSmelter, truck.GetPosition());
						if (smelter != null)
						{
							if (truck.IsOnDock(smelter))
								smelter.DoUnloadCargo();
							else
								truck.DoDock(smelter);
						}
						break;

					case TruckCargo.RareMetal:
						UnitEx rareSmelter = GetClosestUnitOfType(map_id.RareOreSmelter, truck.GetPosition());
						if (rareSmelter != null)
						{
							if (truck.IsOnDock(rareSmelter))
								rareSmelter.DoUnloadCargo();
							else
								truck.DoDock(rareSmelter);
						}
						break;

					case TruckCargo.Food:
						UnitEx agridome = GetClosestUnitOfType(map_id.Agridome, truck.GetPosition());
						if (agridome != null && agridome.IsEnabled())
						{
							if (truck.IsOnDock(agridome))
								agridome.DoUnloadCargo();
							else
								truck.DoDock(agridome);
						}
						break;
				}
			}

			return true;
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
