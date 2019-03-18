using System;

namespace DotNetMissionSDK.Utility
{
	public class UnitInfo
	{
		public static LOCATION GetSize(map_id type)
		{
			switch (type)
			{
				case map_id.CommonOreMine:			return new LOCATION(2,1);
				case map_id.RareOreMine:			return new LOCATION(2,1);
				case map_id.GuardPost:				return new LOCATION(1,1);
				case map_id.LightTower:				return new LOCATION(1,1);
				case map_id.CommonStorage:			return new LOCATION(1,2);
				case map_id.RareStorage:			return new LOCATION(1,2);
				case map_id.Forum:					return new LOCATION(2,2);
				case map_id.CommandCenter:			return new LOCATION(3,2);
				case map_id.MHDGenerator:			return new LOCATION(2,2);
				case map_id.Residence:				return new LOCATION(2,2);
				case map_id.RobotCommand:			return new LOCATION(2,2);
				case map_id.TradeCenter:			return new LOCATION(2,2);
				case map_id.BasicLab:				return new LOCATION(2,2);
				case map_id.MedicalCenter:			return new LOCATION(2,2);
				case map_id.Nursery:				return new LOCATION(2,2);
				case map_id.SolarPowerArray:		return new LOCATION(3,2);
				case map_id.RecreationFacility:		return new LOCATION(2,2);
				case map_id.University:				return new LOCATION(2,2);
				case map_id.Agridome:				return new LOCATION(3,2);
				case map_id.DIRT:					return new LOCATION(3,2);
				case map_id.Garage:					return new LOCATION(3,2);
				case map_id.MagmaWell:				return new LOCATION(2,1);
				case map_id.MeteorDefense:			return new LOCATION(2,2);
				case map_id.GeothermalPlant:		return new LOCATION(2,1);
				case map_id.ArachnidFactory:		return new LOCATION(2,2);
				case map_id.ConsumerFactory:		return new LOCATION(3,3);
				case map_id.StructureFactory:		return new LOCATION(4,3);
				case map_id.VehicleFactory:			return new LOCATION(4,3);
				case map_id.StandardLab:			return new LOCATION(3,2);
				case map_id.AdvancedLab:			return new LOCATION(3,3);
				case map_id.Observatory:			return new LOCATION(2,2);
				case map_id.ReinforcedResidence:	return new LOCATION(3,2);
				case map_id.AdvancedResidence:		return new LOCATION(3,3);
				case map_id.CommonOreSmelter:		return new LOCATION(4,3);
				case map_id.Spaceport:				return new LOCATION(5,4);
				case map_id.RareOreSmelter:			return new LOCATION(4,3);
				case map_id.GORF:					return new LOCATION(3,2);
				case map_id.Tokamak:				return new LOCATION(2,2);
			}

			return new LOCATION(1,1);
		}

		public static MAP_RECT GetRect(LOCATION position, map_id unitType)
		{
			LOCATION size = GetSize(unitType);

			return new MAP_RECT(
				position.x - size.x / 2,
				position.y - size.y / 2,
				position.x + (size.x-1) / 2,
				position.y + (size.y-1) / 2);
		}
	}
}