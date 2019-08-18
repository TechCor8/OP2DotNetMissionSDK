using DotNetMissionSDK.AI.Managers;
using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Mining
{
	public class MaintainTruckRoutes : Task
	{
		private MiningBaseState m_MiningBaseState;

		private BuildCargoTruckTask m_BuildCargoTruckTask;


		public MaintainTruckRoutes(int ownerID, MiningBaseState miningBaseState) : base(ownerID)	{ m_MiningBaseState = miningBaseState; }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			return m_BuildCargoTruckTask.IsTaskComplete(stateSnapshot);
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(m_BuildCargoTruckTask = new BuildCargoTruckTask(ownerID));
		}

		protected override bool PerformTask(StateSnapshot stateSnapshot, List<Action> unitActions)
		{
			return true;
		}

		public void UpdateNeededTrucks(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			// Determine total assigned trucks and total desired trucks
			int assignedTrucks = 0;
			int desiredTrucks = 0;

			foreach (MiningBase miningBase in m_MiningBaseState.miningBases)
			{
				foreach (MiningSite site in miningBase.miningSites)
				{
					if (site.mine == null)
						continue;

					foreach (MiningSmelter smelter in site.smelters)
					{
						if (!smelter.smelter.isEnabled)
							continue;

						assignedTrucks += smelter.trucks.Count;
						desiredTrucks += smelter.desiredTruckCount;
					}
				}
			}

			int trucksDoingSomethingElse = owner.units.cargoTrucks.Count - assignedTrucks;

			m_BuildCargoTruckTask.targetCountToBuild = desiredTrucks + trucksDoingSomethingElse;
		}

		public void PerformTruckRoutes(List<Action> unitActions)
		{
			// Assigned trucks should cycle to mine and smelter
			foreach (MiningBase miningBase in m_MiningBaseState.miningBases)
			{
				foreach (MiningSite site in miningBase.miningSites)
				{
					if (site.mine == null)
						continue;

					foreach (MiningSmelter smelter in site.smelters)
					{
						if (!smelter.smelter.isEnabled)
							continue;

						foreach (CargoTruckState truck in smelter.trucks)
						{
							if (truck.curAction == ActionType.moObjDocking || truck.lastCommand == CommandType.ctMoDumpCargo)
								continue;

							switch (truck.cargoType)
							{
								case TruckCargo.CommonOre:
									if (smelter.smelter.unitType == map_id.RareOreSmelter)
									{
										// Wrong ore type, dump it
										unitActions.Add(() => GameState.GetUnit(truck.unitID)?.DoDumpCargo());
										break;
									}

									// Dock truck to smelter
									unitActions.Add(() =>
									{
										UnitEx liveSmelter = GameState.GetUnit(smelter.smelter.unitID);
										if (liveSmelter == null)
											return;
										GameState.GetUnit(truck.unitID)?.DoDock(liveSmelter);
									});
									break;

								case TruckCargo.RareOre:
									if (smelter.smelter.unitType == map_id.CommonOreSmelter)
									{
										// Wrong ore type, dump it
										unitActions.Add(() => GameState.GetUnit(truck.unitID)?.DoDumpCargo());
										break;
									}

									// Dock truck to smelter
									unitActions.Add(() =>
									{
										UnitEx liveSmelter = GameState.GetUnit(smelter.smelter.unitID);
										if (liveSmelter == null)
											return;
										GameState.GetUnit(truck.unitID)?.DoDock(liveSmelter);
									});
									break;
	
							case TruckCargo.Empty:
									// Dock to mine
									unitActions.Add(() =>
									{
										UnitEx liveMine = GameState.GetUnit(site.mine.unitID);
										if (liveMine == null)
											return;
										GameState.GetUnit(truck.unitID)?.DoDock(liveMine);
									});
									break;

								default:
									// Unknown cargo. Dump it.
									unitActions.Add(() => GameState.GetUnit(truck.unitID)?.DoDumpCargo());
									break;
							}
						}
					}
				}
			}
		}
	}
}
