using DotNetMissionSDK.Utility;

namespace DotNetMissionSDK.AI.Tasks.Base.Vehicle
{
	public class BuildSurveyorTask : BuildVehicleTask
	{
		public BuildSurveyorTask()											{ m_VehicleToBuild = map_id.RoboSurveyor;				}
		public BuildSurveyorTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.RoboSurveyor;				}

		public override bool IsTaskComplete()
		{
			if (owner.units.EDWARDSatelliteCount > 0)
				return true;

			return base.IsTaskComplete();
		}
	}

	public class BuildMinerTask : BuildVehicleTask
	{
		public BuildMinerTask()												{ m_VehicleToBuild = map_id.RoboMiner;					}
		public BuildMinerTask(PlayerInfo owner) : base(owner)				{ m_VehicleToBuild = map_id.RoboMiner;					}
	}

	public class BuildConvecTask : BuildVehicleTask
	{
		public BuildConvecTask()											{ m_VehicleToBuild = map_id.ConVec;						}
		public BuildConvecTask(PlayerInfo owner) : base(owner)				{ m_VehicleToBuild = map_id.ConVec;						}
	}

	public class BuildCargoTruckTask : BuildVehicleTask
	{
		public BuildCargoTruckTask()										{ m_VehicleToBuild = map_id.CargoTruck;					}
		public BuildCargoTruckTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.CargoTruck;					}
	}

	public class BuildEarthworkerTask : BuildVehicleTask
	{
		public BuildEarthworkerTask()										{ m_VehicleToBuild = map_id.Earthworker;				}
		public BuildEarthworkerTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.Earthworker;				}
	}

	public class BuildRepairVehicleTask : BuildVehicleTask
	{
		public BuildRepairVehicleTask()										{ m_VehicleToBuild = map_id.RepairVehicle;				}
		public BuildRepairVehicleTask(PlayerInfo owner) : base(owner)		{ m_VehicleToBuild = map_id.RepairVehicle;				}
	}

	public class BuildSpiderTask : BuildVehicleTask
	{
		public BuildSpiderTask()											{ m_VehicleToBuild = map_id.Spider;						}
		public BuildSpiderTask(PlayerInfo owner) : base(owner)				{ m_VehicleToBuild = map_id.Spider;						}
	}

	public class BuildScorpionTask : BuildVehicleTask
	{
		public BuildScorpionTask()											{ m_VehicleToBuild = map_id.Scorpion;					}
		public BuildScorpionTask(PlayerInfo owner) : base(owner)			{ m_VehicleToBuild = map_id.Scorpion;					}
	}
}
