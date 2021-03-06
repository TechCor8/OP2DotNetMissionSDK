﻿using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class UnitData
	{
		// Standard info
		[DataMember(Name = "ID")]			public int id						{ get; set; }
		[DataMember(Name = "TypeID")]		private string m_TypeID				{ get; set; }
		[DataMember(Name = "Health")]		public float health					{ get; set; }
		[DataMember(Name = "Lights")]		public bool lights					{ get; set; }
		[DataMember(Name = "CargoType")]	private string m_CargoType			{ get; set; }
		[DataMember(Name = "CargoAmount")]	public int cargoAmount				{ get; set; }
		[DataMember(Name = "Direction")]	private string m_Direction			{ get; set; }
		[DataMember(Name = "Position")]		public DataLocation position		{ get; set; }

		// Used for ore mines
		[DataMember(Name = "BarYield")]		private string m_BarYield			{ get; set; }
		[DataMember(Name = "BarVariant")]	private string m_BarVariant			{ get; set; }

		// AutoLayout
		[DataMember(Name = "IgnoreLayout")]	public bool ignoreLayout			{ get; set; }
		[DataMember(Name = "MinDistance")]	public int minDistance				{ get; set; }
		[DataMember(Name = "SpawnDistance")]public int spawnDistance			{ get; set; }
		[DataMember(Name = "CreateWall")]	public bool createWall				{ get; set; }
		[DataMember(Name = "MaxTubes")]		private int? m_MaxTubes				{ get; set; }

		public map_id typeID				{ get { return GetEnum<map_id>(m_TypeID);			} set { m_TypeID = value.ToString();		} }
		public int cargoType				{ get { return GetCargoType(m_CargoType);			} set { m_CargoType = GetCargoType(value);	} }
		public UnitDirection direction		{ get { return GetEnum<UnitDirection>(m_Direction);	} set { m_Direction = value.ToString();		} }

		// Used for ore mines
		public Yield barYield				{ get { return GetEnum<Yield>(m_BarYield);			} set { m_BarYield = value.ToString();		} }
		public Variant barVariant			{ get { return GetEnum<Variant>(m_BarVariant);		} set { m_BarVariant = value.ToString();	} }

		// AutoLayout
		public int maxTubes					{ get { return m_MaxTubes != null ? m_MaxTubes.Value : -1; } set { m_MaxTubes = value;			} }

		[OnDeserializing]
		private void OnDeserializing(StreamingContext context)
		{
			health = 1;
			lights = true;
		}

		public UnitData() { }
		public UnitData(UnitData clone)
		{
			id = clone.id;
			m_TypeID = clone.m_TypeID;
			health = clone.health;
			lights = clone.lights;
			m_CargoType = clone.m_CargoType;
			cargoAmount = clone.cargoAmount;
			m_Direction = clone.m_Direction;
			position = clone.position;

			m_BarYield = clone.m_BarYield;
			m_BarVariant = clone.m_BarVariant;

			ignoreLayout = clone.ignoreLayout;
			minDistance = clone.minDistance;
			spawnDistance = clone.spawnDistance;
			createWall = clone.createWall;
			m_MaxTubes = clone.m_MaxTubes;
		}

		/// <summary>
		/// Constructor for manual layout unit.
		/// </summary>
		public UnitData(map_id typeID, int cargoType, int cargoAmount, UnitDirection direction, LOCATION position)
		{
			SetUnitData(typeID, cargoType, cargoAmount, direction, position, Yield.Random, Variant.Random, true, 0, 0, false, 0);
		}

		/// <summary>
		/// Constructor for manual layout mine.
		/// </summary>
		public UnitData(map_id typeID, int cargoType, int cargoAmount, UnitDirection direction, LOCATION position, Yield barYield, Variant barVariant)
		{
			SetUnitData(typeID, cargoType, cargoAmount, direction, position, barYield, barVariant, true, 0, 0, false, 0);
		}

		/// <summary>
		/// Constructor for AutoLayout structure.
		/// </summary>
		public UnitData(map_id typeID, int cargoType, int cargoAmount, UnitDirection direction, int minDistance, bool createWall=false, int maxTubes=-1)
		{
			SetUnitData(typeID, cargoType, cargoAmount, direction, new LOCATION(), Yield.Random, Variant.Random, true, minDistance, 0, createWall, maxTubes);
		}

		/// <summary>
		/// Constructor for AutoLayout mine.
		/// </summary>
		public UnitData(map_id typeID, int cargoType, int cargoAmount, UnitDirection direction, Yield barYield, Variant barVariant, int minDistance, bool createWall=false, int maxTubes=-1)
		{
			SetUnitData(typeID, cargoType, cargoAmount, direction, new LOCATION(), barYield, barVariant, true, minDistance, 0, createWall, maxTubes);
		}

		/// <summary>
		/// Constructor for AutoLayout vehicle.
		/// </summary>
		public UnitData(map_id typeID, int cargoType, int cargoAmount, UnitDirection direction, int minDistance, int spawnDistance)
		{
			SetUnitData(typeID, cargoType, cargoAmount, direction, new LOCATION(), Yield.Random, Variant.Random, true, minDistance, spawnDistance, false, 0);
		}

		private void SetUnitData(map_id typeID, int cargoType, int cargoAmount, UnitDirection direction, LOCATION position, Yield barYield, Variant barVariant,
								bool ignoreLayout, int minDistance, int spawnDistance, bool createWall, int maxTubes)
		{
			m_TypeID = typeID.ToString();
			lights = true;
			m_CargoType = GetCargoType(cargoType);
			this.cargoAmount = cargoAmount;
			m_Direction = direction.ToString();
			this.position = position;
			m_BarYield = barYield.ToString();
			m_BarVariant = barVariant.ToString();
			this.ignoreLayout = ignoreLayout;
			this.minDistance = minDistance;
			this.spawnDistance = spawnDistance;
			this.createWall = createWall;
			m_MaxTubes = maxTubes;
		}

		/// <summary>
		/// Creates a unit from UnitData.
		/// </summary>
		/// <param name="playerID">The player to assign the unit to.</param>
		/// <param name="pt">The position to spawn the unit.</param>
		/// <returns></returns>
		public Unit CreateUnit(int playerID, LOCATION pt)
		{
			UnitData data = this;

			HFL.UnitEx unit = null;

			// Must create the beacon for mines
			switch (data.typeID)
			{
				case map_id.CommonOreMine:
					TethysGame.CreateBeacon(map_id.MiningBeacon, pt.x, pt.y, BeaconType.Common, data.barYield, data.barVariant);
					break;

				case map_id.RareOreMine:
					// Game has a bug where rare ore mines can't be created. Common ore mines will turn into the rare mine instead.
					TethysGame.CreateBeacon(map_id.MiningBeacon, pt.x, pt.y, BeaconType.Rare, data.barYield, data.barVariant);
					unit = TethysGame.CreateUnit(map_id.CommonOreMine, pt.x, pt.y, playerID, data.cargoType, data.direction);
					break;

				case map_id.CargoTruck:
					unit = TethysGame.CreateUnit(data.typeID, pt.x, pt.y, playerID, data.cargoType, data.direction);
					unit.SetTruckCargo((TruckCargo)data.cargoType, data.cargoAmount);
					break;

				case map_id.ConVec:
					unit = TethysGame.CreateUnit(data.typeID, pt.x, pt.y, playerID, data.cargoType, data.direction);
					unit.SetCargo((map_id)data.cargoType, (map_id)data.cargoAmount);
					break;

				default:
					// All other units
					unit = TethysGame.CreateUnit(data.typeID, pt.x, pt.y, playerID, data.cargoType, data.direction);
					break;
			}

			int hp = unit.GetUnitInfo().GetHitPoints(playerID);
			int damage = (int)(hp * (1.0f - health));
			unit.SetDamage(damage);

			if (unit.IsVehicle())
				unit.DoSetLights(lights);

			return unit;
		}

		private int GetCargoType(string val)
		{
			switch (typeID)
			{
				case map_id.CargoTruck:
					return (int)GetEnum<TruckCargo>(val);
			}

			return (int)GetEnum<map_id>(val);
		}

		private string GetCargoType(int val)
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
	}
}
