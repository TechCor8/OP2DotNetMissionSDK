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
}
