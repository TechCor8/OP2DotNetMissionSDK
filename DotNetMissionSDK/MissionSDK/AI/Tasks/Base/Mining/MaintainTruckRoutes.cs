using DotNetMissionSDK.AI.Managers;
using DotNetMissionSDK.AI.Tasks.Base.VehicleTasks;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Mining
{
	public class MaintainTruckRoutes : Task
	{
		private MiningBaseState m_MiningBaseState;

		private BuildCargoTruckTask m_BuildCargoTruckTask;


		public MaintainTruckRoutes(MiningBaseState miningBaseState)									{ m_MiningBaseState = miningBaseState; }
		public MaintainTruckRoutes(PlayerInfo owner, MiningBaseState miningBaseState) : base(owner)	{ m_MiningBaseState = miningBaseState; }

		public override bool IsTaskComplete()
		{
			return m_BuildCargoTruckTask.IsTaskComplete();
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(m_BuildCargoTruckTask = new BuildCargoTruckTask());
		}

		protected override bool PerformTask()
		{
			return true;
		}

		public void UpdateNeededTrucks()
		{
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
						if (!smelter.smelter.IsEnabled())
							continue;

						assignedTrucks += smelter.trucks.Count;
						desiredTrucks += smelter.desiredTruckCount;
					}
				}
			}

			int trucksDoingSomethingElse = owner.units.cargoTrucks.Count - assignedTrucks;

			m_BuildCargoTruckTask.targetCountToBuild = desiredTrucks + trucksDoingSomethingElse;
		}

		public void PerformTruckRoutes()
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
						if (!smelter.smelter.IsEnabled())
							continue;

						foreach (UnitEx truck in smelter.trucks)
						{
							if (truck.GetCurAction() == ActionType.moObjDocking || truck.GetLastCommand() == CommandType.ctMoDumpCargo)
								continue;

							switch (truck.GetCargoType())
							{
								case TruckCargo.CommonOre:
									if (smelter.smelter.GetUnitType() == map_id.RareOreSmelter)
									{
										// Wrong ore type, dump it
										truck.DoDumpCargo();
										break;
									}

									truck.DoDock(smelter.smelter);
									break;

								case TruckCargo.RareOre:
									if (smelter.smelter.GetUnitType() == map_id.CommonOreSmelter)
									{
										// Wrong ore type, dump it
										truck.DoDumpCargo();
										break;
									}

									truck.DoDock(smelter.smelter);
									break;
	
							case TruckCargo.Empty:
									truck.DoDock(site.mine);
									break;

								default:
									truck.DoDumpCargo();
									break;
							}
						}
					}
				}
			}
		}
	}
}
