using DotNetMissionReader;

namespace DotNetMissionSDK
{
	public static class UnitDataExtensions
	{
		public static map_id GetTypeID(this UnitData unitData) => GetEnum<map_id>(unitData.typeID);
		public static void SetTypeID(this UnitData unitData, map_id typeId) => unitData.typeID = typeId.ToString();

		public static int GetCargoType(this UnitData unitData) => GetCargoType(GetTypeID(unitData), unitData.cargoType);
		public static void SetCargoType(this UnitData unitData, int cargoType) => unitData.cargoType = GetCargoType(GetTypeID(unitData), cargoType);

		public static UnitDirection GetDirection(this UnitData unitData) => GetEnum<UnitDirection>(unitData.direction);
		public static void SetDirection(this UnitData unitData, UnitDirection direction) => unitData.direction = direction.ToString();

		// Used for ore mines
		public static Yield GetBarYield(this UnitData unitData) => GetEnum<Yield>(unitData.barYield);
		public static void SetBarYield(this UnitData unitData, Yield barYield) => unitData.barYield = barYield.ToString();

		public static Variant GetBarVariant(this UnitData unitData) => GetEnum<Variant>(unitData.barVariant);
		public static void SetBarVariant(this UnitData unitData, Variant barVariant) => unitData.barVariant = barVariant.ToString();

		/// <summary>
		/// Creates a unit from UnitData.
		/// </summary>
		/// <param name="playerID">The player to assign the unit to.</param>
		/// <param name="pt">The position to spawn the unit.</param>
		/// <returns></returns>
		public static Unit CreateUnit(this UnitData unitData, int playerID, LOCATION pt)
		{
			UnitData data = unitData;

			HFL.UnitEx unit = null;

			// Must create the beacon for mines
			switch (GetTypeID(data))
			{
				case map_id.CommonOreMine:
					TethysGame.CreateBeacon(map_id.MiningBeacon, pt.x, pt.y, BeaconType.Common, data.GetBarYield(), data.GetBarVariant());
					break;

				case map_id.RareOreMine:
					// Game has a bug where rare ore mines can't be created. Common ore mines will turn into the rare mine instead.
					TethysGame.CreateBeacon(map_id.MiningBeacon, pt.x, pt.y, BeaconType.Rare, data.GetBarYield(), data.GetBarVariant());
					unit = TethysGame.CreateUnit(map_id.CommonOreMine, pt.x, pt.y, playerID, data.GetCargoType(), data.GetDirection());
					break;

				case map_id.CargoTruck:
					unit = TethysGame.CreateUnit(data.GetTypeID(), pt.x, pt.y, playerID, data.GetCargoType(), data.GetDirection());
					unit.SetTruckCargo((TruckCargo)data.GetCargoType(), data.cargoAmount);
					break;

				case map_id.ConVec:
					unit = TethysGame.CreateUnit(data.GetTypeID(), pt.x, pt.y, playerID, data.GetCargoType(), data.GetDirection());
					unit.SetCargo((map_id)data.GetCargoType(), (map_id)data.cargoAmount);
					break;

				default:
					// All other units
					unit = TethysGame.CreateUnit(data.GetTypeID(), pt.x, pt.y, playerID, data.GetCargoType(), data.GetDirection());
					break;
			}

			int hp = unit.GetUnitInfo().GetHitPoints(playerID);
			int damage = (int)(hp * (1.0f - data.health));
			unit.SetDamage(damage);

			if (unit.IsVehicle())
				unit.DoSetLights(data.lights);

			return unit;
		}

		private static int GetCargoType(map_id typeID, string val)
		{
			switch (typeID)
			{
				case map_id.CargoTruck:
					return (int)GetEnum<TruckCargo>(val);
			}

			return (int)GetEnum<map_id>(val);
		}

		private static string GetCargoType(map_id typeID, int val)
		{
			switch (typeID)
			{
				case map_id.CargoTruck:
					return ((TruckCargo)val).ToString();
			}

			return ((map_id)val).ToString();
		}

		private static T GetEnum<T>(string val) where T : struct
		{
			T result;
			System.Enum.TryParse(val, out result);
			return result;
		}

		/// <summary>
		/// Constructor for manual layout unit.
		/// </summary>
		public static UnitData CreateManualLayoutUnit(map_id typeID, int cargoType, int cargoAmount, UnitDirection direction, LOCATION position)
		{
			UnitData unitData = new UnitData();
			SetUnitData(unitData, typeID, cargoType, cargoAmount, direction, position, Yield.Random, Variant.Random, true, 0, 0, false, 0);
			return unitData;
		}

		/// <summary>
		/// Constructor for manual layout mine.
		/// </summary>
		public static UnitData CreateManualLayoutMine(map_id typeID, int cargoType, int cargoAmount, UnitDirection direction, LOCATION position, Yield barYield, Variant barVariant)
		{
			UnitData unitData = new UnitData();
			SetUnitData(unitData, typeID, cargoType, cargoAmount, direction, position, barYield, barVariant, true, 0, 0, false, 0);
			return unitData;
		}

		/// <summary>
		/// Constructor for AutoLayout structure.
		/// </summary>
		public static UnitData CreateAutoLayoutStructure(map_id typeID, int cargoType, int cargoAmount, UnitDirection direction, int minDistance, bool createWall=false, int maxTubes=-1)
		{
			UnitData unitData = new UnitData();
			SetUnitData(unitData, typeID, cargoType, cargoAmount, direction, new LOCATION(), Yield.Random, Variant.Random, true, minDistance, 0, createWall, maxTubes);
			return unitData;
		}

		/// <summary>
		/// Constructor for AutoLayout mine.
		/// </summary>
		public static UnitData CreateAutoLayoutMine(map_id typeID, int cargoType, int cargoAmount, UnitDirection direction, Yield barYield, Variant barVariant, int minDistance, bool createWall=false, int maxTubes=-1)
		{
			UnitData unitData = new UnitData();
			SetUnitData(unitData, typeID, cargoType, cargoAmount, direction, new LOCATION(), barYield, barVariant, true, minDistance, 0, createWall, maxTubes);
			return unitData;
		}

		/// <summary>
		/// Constructor for AutoLayout vehicle.
		/// </summary>
		public static UnitData CreateAutolayoutVehicle(map_id typeID, int cargoType, int cargoAmount, UnitDirection direction, int minDistance, int spawnDistance)
		{
			UnitData unitData = new UnitData();
			SetUnitData(unitData, typeID, cargoType, cargoAmount, direction, new LOCATION(), Yield.Random, Variant.Random, true, minDistance, spawnDistance, false, 0);
			return unitData;
		}

		private static void SetUnitData(UnitData unitData, map_id typeID, int cargoType, int cargoAmount, UnitDirection direction, LOCATION position, Yield barYield, Variant barVariant,
								bool ignoreLayout, int minDistance, int spawnDistance, bool createWall, int maxTubes)
		{
			unitData.typeID = typeID.ToString();
			unitData.lights = true;
			unitData.cargoType = GetCargoType(typeID, cargoType);
			unitData.cargoAmount = cargoAmount;
			unitData.direction = direction.ToString();
			unitData.position = position.ToDataLocation();
			unitData.barYield = barYield.ToString();
			unitData.barVariant = barVariant.ToString();
			unitData.ignoreLayout = ignoreLayout;
			unitData.minDistance = minDistance;
			unitData.spawnDistance = spawnDistance;
			unitData.createWall = createWall;
			unitData.maxTubes = maxTubes;
		}
	}
}