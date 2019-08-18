using DotNetMissionSDK.HFL;

namespace DotNetMissionSDK.State.Snapshot.UnitTypeInfo
{
	/// <summary>
	/// Contains immutable unit info state at a moment in TethysGame.Time().
	/// Should only be created by StateSnapshot.
	/// </summary>
	public class GlobalUnitInfo
	{
		public map_id unitType										{ get; private set; }

		/// <summary>
		/// Gets the unit's research topic.
		/// Returns the TechInfo array index NOT the techID.
		/// </summary>
		public int researchTopic									{ get; private set; }
		public string unitName										{ get; private set; }
		public string produceListName								{ get; private set; }
		public string codeName										{ get; private set; }

		public int explosionSize									{ get; private set; }
		
		
		public bool CanColonyUseUnit(bool isEden)
		{
			if (isEden)
				return (GetOwnerFlags() & OwnerFlags.Eden) != 0;
			else
				return (GetOwnerFlags() & OwnerFlags.Plymouth) != 0;
		}

		public OwnerFlags GetOwnerFlags()
		{
			switch (unitType)
			{
				case map_id.CargoTruck:						return OwnerFlags.Both;
				case map_id.ConVec:							return OwnerFlags.Both;
				case map_id.Spider:							return OwnerFlags.Plymouth;
				case map_id.Scorpion:						return OwnerFlags.Plymouth;
				case map_id.Lynx:							return OwnerFlags.Both;
				case map_id.Panther:						return OwnerFlags.Both;
				case map_id.Tiger:							return OwnerFlags.Both;
				case map_id.RoboSurveyor:					return OwnerFlags.Both;
				case map_id.RoboMiner:						return OwnerFlags.Both;
				case map_id.GeoCon:							return OwnerFlags.Eden;
				case map_id.Scout:							return OwnerFlags.Both;
				case map_id.RoboDozer:						return OwnerFlags.Both;
				case map_id.EvacuationTransport:			return OwnerFlags.Both;
				case map_id.RepairVehicle:					return OwnerFlags.Eden;
				case map_id.Earthworker:					return OwnerFlags.Both;
				case map_id.SmallCapacityAirTransport:		return OwnerFlags.Gaia;

				case map_id.Tube:							return OwnerFlags.Both;
				case map_id.Wall:							return OwnerFlags.Both;
				case map_id.LavaWall:						return OwnerFlags.Plymouth;
				case map_id.MicrobeWall:					return OwnerFlags.Eden;

				case map_id.CommonOreMine:					return OwnerFlags.Both;
				case map_id.RareOreMine:					return OwnerFlags.Both;
				case map_id.GuardPost:						return OwnerFlags.Both;
				case map_id.LightTower:						return OwnerFlags.Both;
				case map_id.CommonStorage:					return OwnerFlags.Both;
				case map_id.RareStorage:					return OwnerFlags.Both;
				case map_id.Forum:							return OwnerFlags.Plymouth;
				case map_id.CommandCenter:					return OwnerFlags.Both;
				case map_id.MHDGenerator:					return OwnerFlags.Plymouth;
				case map_id.Residence:						return OwnerFlags.Both;
				case map_id.RobotCommand:					return OwnerFlags.Both;
				case map_id.TradeCenter:					return OwnerFlags.Both;
				case map_id.BasicLab:						return OwnerFlags.Both;
				case map_id.MedicalCenter:					return OwnerFlags.Both;
				case map_id.Nursery:						return OwnerFlags.Both;
				case map_id.SolarPowerArray:				return OwnerFlags.Both;
				case map_id.RecreationFacility:				return OwnerFlags.Both;
				case map_id.University:						return OwnerFlags.Both;
				case map_id.Agridome:						return OwnerFlags.Both;
				case map_id.DIRT:							return OwnerFlags.Both;
				case map_id.Garage:							return OwnerFlags.Both;
				case map_id.MagmaWell:						return OwnerFlags.Eden;
				case map_id.MeteorDefense:					return OwnerFlags.Eden;
				case map_id.GeothermalPlant:				return OwnerFlags.Eden;
				case map_id.ArachnidFactory:				return OwnerFlags.Plymouth;
				case map_id.ConsumerFactory:				return OwnerFlags.Eden;
				case map_id.StructureFactory:				return OwnerFlags.Both;
				case map_id.VehicleFactory:					return OwnerFlags.Both;
				case map_id.StandardLab:					return OwnerFlags.Both;
				case map_id.AdvancedLab:					return OwnerFlags.Both;
				case map_id.Observatory:					return OwnerFlags.Eden;
				case map_id.ReinforcedResidence:			return OwnerFlags.Plymouth;
				case map_id.AdvancedResidence:				return OwnerFlags.Eden;
				case map_id.CommonOreSmelter:				return OwnerFlags.Both;
				case map_id.Spaceport:						return OwnerFlags.Both;
				case map_id.RareOreSmelter:					return OwnerFlags.Both;
				case map_id.GORF:							return OwnerFlags.Both;
				case map_id.Tokamak:						return OwnerFlags.Both;

				case map_id.AcidCloud:						return OwnerFlags.Eden;
				case map_id.EMP:							return OwnerFlags.Both;
				case map_id.Laser:							return OwnerFlags.Eden;
				case map_id.Microwave:						return OwnerFlags.Plymouth;
				case map_id.RailGun:						return OwnerFlags.Eden;
				case map_id.RPG:							return OwnerFlags.Plymouth;
				case map_id.Starflare:						return OwnerFlags.Both;
				case map_id.Supernova:						return OwnerFlags.Plymouth;
				case map_id.Starflare2:						return OwnerFlags.Eden;
				case map_id.Supernova2:						return OwnerFlags.Plymouth;
				case map_id.ESG:							return OwnerFlags.Plymouth;
				case map_id.Stickyfoam:						return OwnerFlags.Plymouth;
				case map_id.ThorsHammer:					return OwnerFlags.Eden;
				case map_id.EnergyCannon:					return OwnerFlags.Plymouth;

				case map_id.EDWARDSatellite:				return OwnerFlags.Both;
				case map_id.SolarSatellite:					return OwnerFlags.Both;
				case map_id.IonDriveModule:					return OwnerFlags.Both;
				case map_id.FusionDriveModule:				return OwnerFlags.Both;
				case map_id.CommandModule:					return OwnerFlags.Both;
				case map_id.FuelingSystems:					return OwnerFlags.Both;
				case map_id.HabitatRing:					return OwnerFlags.Both;
				case map_id.SensorPackage:					return OwnerFlags.Both;
				case map_id.Skydock:						return OwnerFlags.Both;
				case map_id.StasisSystems:					return OwnerFlags.Both;
				case map_id.OrbitalPackage:					return OwnerFlags.Both;
				case map_id.PhoenixModule:					return OwnerFlags.Both;

				case map_id.RareMetalsCargo:				return OwnerFlags.Both;
				case map_id.CommonMetalsCargo:				return OwnerFlags.Both;
				case map_id.FoodCargo:						return OwnerFlags.Both;
				case map_id.EvacuationModule:				return OwnerFlags.Both;
				case map_id.ChildrenModule:					return OwnerFlags.Both;

				case map_id.SULV:							return OwnerFlags.Both;
				case map_id.RLV:							return OwnerFlags.Eden;
				case map_id.EMPMissile:						return OwnerFlags.Plymouth;

				case map_id.ImpulseItems:					return OwnerFlags.Eden;
				case map_id.Wares:							return OwnerFlags.Eden;
				case map_id.LuxuryWares:					return OwnerFlags.Eden;

				case map_id.Spider3Pack:					return OwnerFlags.Plymouth;
				case map_id.Scorpion3Pack:					return OwnerFlags.Plymouth;
			}

			return OwnerFlags.Gaia;
		}


		/// <summary>
		/// Creates an immutable state for a unit type.
		/// </summary>
		/// <param name="unitTypeID">The unit type to pull data from.</param>
		public GlobalUnitInfo(map_id unitTypeID)
		{
			UnitInfo info = new UnitInfo(unitTypeID);

			unitType		= unitTypeID;

			researchTopic	= info.GetResearchTopic();
			unitName		= info.GetUnitName();
			produceListName = info.GetProduceListName();
			codeName		= info.GetCodeName();

			explosionSize	= info.GetExplosionSize();
		}
	}
}
