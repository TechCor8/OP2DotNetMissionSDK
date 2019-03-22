using System;

namespace DotNetMissionSDK.Utility
{
	public class UnitInfo
	{
		public static LOCATION GetSize(map_id type, bool includeBulldozedArea=false)
		{
			LOCATION result;

			switch (type)
			{
				case map_id.CommonOreMine:			result = new LOCATION(2,1);		break;
				case map_id.RareOreMine:			result = new LOCATION(2,1);		break;
				case map_id.GuardPost:				result = new LOCATION(1,1);		break;
				case map_id.LightTower:				result = new LOCATION(1,1);		break;
				case map_id.CommonStorage:			result = new LOCATION(1,2);		break;
				case map_id.RareStorage:			result = new LOCATION(1,2);		break;
				case map_id.Forum:					result = new LOCATION(2,2);		break;
				case map_id.CommandCenter:			result = new LOCATION(3,2);		break;
				case map_id.MHDGenerator:			result = new LOCATION(2,2);		break;
				case map_id.Residence:				result = new LOCATION(2,2);		break;
				case map_id.RobotCommand:			result = new LOCATION(2,2);		break;
				case map_id.TradeCenter:			result = new LOCATION(2,2);		break;
				case map_id.BasicLab:				result = new LOCATION(2,2);		break;
				case map_id.MedicalCenter:			result = new LOCATION(2,2);		break;
				case map_id.Nursery:				result = new LOCATION(2,2);		break;
				case map_id.SolarPowerArray:		result = new LOCATION(3,2);		break;
				case map_id.RecreationFacility:		result = new LOCATION(2,2);		break;
				case map_id.University:				result = new LOCATION(2,2);		break;
				case map_id.Agridome:				result = new LOCATION(3,2);		break;
				case map_id.DIRT:					result = new LOCATION(3,2);		break;
				case map_id.Garage:					result = new LOCATION(3,2);		break;
				case map_id.MagmaWell:				result = new LOCATION(2,1);		break;
				case map_id.MeteorDefense:			result = new LOCATION(2,2);		break;
				case map_id.GeothermalPlant:		result = new LOCATION(2,1);		break;
				case map_id.ArachnidFactory:		result = new LOCATION(2,2);		break;
				case map_id.ConsumerFactory:		result = new LOCATION(3,3);		break;
				case map_id.StructureFactory:		result = new LOCATION(4,3);		break;
				case map_id.VehicleFactory:			result = new LOCATION(4,3);		break;
				case map_id.StandardLab:			result = new LOCATION(3,2);		break;
				case map_id.AdvancedLab:			result = new LOCATION(3,3);		break;
				case map_id.Observatory:			result = new LOCATION(2,2);		break;
				case map_id.ReinforcedResidence:	result = new LOCATION(3,2);		break;
				case map_id.AdvancedResidence:		result = new LOCATION(3,3);		break;
				case map_id.CommonOreSmelter:		result = new LOCATION(4,3);		break;
				case map_id.Spaceport:				result = new LOCATION(5,4);		break;
				case map_id.RareOreSmelter:			result = new LOCATION(4,3);		break;
				case map_id.GORF:					result = new LOCATION(3,2);		break;
				case map_id.Tokamak:				result = new LOCATION(2,2);		break;
				default:
					// Not a structure
					return new LOCATION(1,1);
			}

			if (includeBulldozedArea)
			{
				result.x += 2;
				result.y += 2;
			}

			return result;
		}

		public static MAP_RECT GetRect(LOCATION position, map_id unitType, bool includeBulldozedArea=false)
		{
			LOCATION size = GetSize(unitType, includeBulldozedArea);

			return new MAP_RECT(
				position.x - size.x / 2,
				position.y - size.y / 2,
				position.x + (size.x-1) / 2,
				position.y + (size.y-1) / 2);
		}
	}
}