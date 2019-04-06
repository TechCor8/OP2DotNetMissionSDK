using System;

namespace DotNetMissionSDK.Utility
{
	public class TechInfo
	{
		// TODO: Pull this info directly from OP2?
		public static int GetTechID(map_id unitType)
		{
			switch (unitType)
			{
				case map_id.CommonOreMine:			return 5405;
				case map_id.RareOreMine:			return 5405;
				case map_id.GuardPost:				return 5405;
				case map_id.LightTower:				return 5405;
				case map_id.CommonStorage:			return 5405;
				case map_id.RareStorage:			return 5405;
				case map_id.Forum:					return 5405;
				case map_id.CommandCenter:			return 5405;
				case map_id.MHDGenerator:			return 5405;
				case map_id.Residence:				return 5405;
				case map_id.RobotCommand:			return 5405;
				case map_id.TradeCenter:			return 5405;
				case map_id.BasicLab:				return 5405;
				case map_id.MedicalCenter:			return 5405;
				case map_id.Nursery:				return 5405;
				case map_id.SolarPowerArray:		return 5405;
				case map_id.RecreationFacility:		return 5405;
				case map_id.University:				return 5405;
				case map_id.Agridome:				return 5405;
				case map_id.DIRT:					return 5405;
				case map_id.Garage:					return 5405;
				case map_id.MagmaWell:				return 5405;
				case map_id.MeteorDefense:			return 5405;
				case map_id.GeothermalPlant:		return 5405;
				case map_id.ArachnidFactory:		return 5405;
				case map_id.ConsumerFactory:		return 5405;
				case map_id.StructureFactory:		return 5405;
				case map_id.VehicleFactory:			return 5405;
				case map_id.StandardLab:			return 5405;
				case map_id.AdvancedLab:			return 5405;
				case map_id.Observatory:			return 5405;
				case map_id.ReinforcedResidence:	return 5405;
				case map_id.AdvancedResidence:		return 5405;
				case map_id.CommonOreSmelter:		return 5405;
				case map_id.Spaceport:				return 5405;
				case map_id.RareOreSmelter:			return 5405;
				case map_id.GORF:					return 5405;
				case map_id.Tokamak:				return 5405;

				case map_id.EDWARDSatellite:		return 5405;
				case map_id.SolarSatellite:			return 10204;
				case map_id.IonDriveModule:			return 8801;
				case map_id.FusionDriveModule:		return 8901;
				case map_id.CommandModule:			return 10202;
				case map_id.FuelingSystems:			return 8951;
				case map_id.HabitatRing:			return 10205;
				case map_id.SensorPackage:			return 10206;
				case map_id.Skydock:				return 8601;
				case map_id.StasisSystems:			return 10208;
				case map_id.OrbitalPackage:			return 10209;
				case map_id.PhoenixModule:			return 10401;

				case map_id.RareMetalsCargo:		return 10401;
				case map_id.CommonMetalsCargo:		return 10401;
				case map_id.FoodCargo:				return 10401;
				case map_id.EvacuationModule:		return 10401;
				case map_id.ChildrenModule:			return 10401;
			}

			return -1;
		}


		/*BEGIN_TECH "Solar Power" 10204
    CATEGORY        3
    DESCRIPTION     "Structure Factories may now produce Solar Power Array structure kits.  Spaceports may now produce Solar Power Satellites. _______________________________________  The solar power system, comprised of a collector satellite and ground-based receiver, is an inexpensive alternative energy source.  The satellite, once in orbit, can be retargeted at a new ground location after an evacuation, and the receivers are much less volatile than our Tokamak fusion reactors."
    TEASER          "Allows production of the Solar Power systems. _______________________________________  The technology behind solar power has been available for quite some time, the size of the solar collector panels needed to generate a significant amount of power has always been judged prohibitive, especially since our periodic evacuations began.  However, with the redevelopment of a space program, it is possible to build a solar collector satellite which beams the energy it collects to a ground-based receiver."
    REQUIRES        05405
    EDEN_COST       2800
    PLYMOUTH_COST   3700
    MAX_SCIENTISTS  16
    LAB             3*/
	}
}
