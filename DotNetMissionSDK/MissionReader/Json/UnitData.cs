﻿using System.Runtime.Serialization;

namespace DotNetMissionSDK.Json
{
	[DataContract]
	public class UnitData
	{
		// Standard info
		[DataMember(Name = "TypeID")]		private string m_TypeID				{ get; set; }
		[DataMember(Name = "CargoType")]	private string m_CargoType			{ get; set; }
		[DataMember(Name = "Direction")]	private string m_Direction			{ get; set; }
		[DataMember(Name = "Location")]		public DataLocation location		{ get; private set; }

		// Used for ore mines
		[DataMember(Name = "BarYield")]		private string m_BarYield			{ get; set; }
		[DataMember(Name = "BarVariant")]	private string m_BarVariant			{ get; set; }

		// AutoLayout
		[DataMember(Name = "IgnoreLayout")]	public bool ignoreLayout			{ get; private set; }
		[DataMember(Name = "MinDistance")]	public int minDistance				{ get; private set; }
		[DataMember(Name = "SpawnDistance")]public int spawnDistance			{ get; private set; }
		[DataMember(Name = "CreateWall")]	public bool createWall				{ get; private set; }
		[DataMember(Name = "MaxTubes")]		private int? m_MaxTubes				{ get; set; }

		public map_id typeID				{ get { return GetEnum<map_id>(m_TypeID);			} }
		public int cargoType				{ get { return GetCargoType(m_CargoType);			} }
		public UnitDirection direction		{ get { return GetEnum<UnitDirection>(m_Direction);	} }

		// Used for ore mines
		public Yield barYield				{ get { return GetEnum<Yield>(m_BarYield);			} }
		public Variant barVariant			{ get { return GetEnum<Variant>(m_BarVariant);		} }

		// AutoLayout
		public int maxTubes					{ get { return m_MaxTubes != null ? m_MaxTubes.Value : -1; } }


		/// <summary>
		/// Constructor for manual layout unit.
		/// </summary>
		public UnitData(map_id typeID, int cargoType, UnitDirection direction, LOCATION position)
		{
			SetUnitData(typeID, cargoType, direction, position, Yield.Random, Variant.Random, true, 0, 0, false, 0);
		}

		/// <summary>
		/// Constructor for manual layout mine.
		/// </summary>
		public UnitData(map_id typeID, int cargoType, UnitDirection direction, LOCATION position, Yield barYield, Variant barVariant)
		{
			SetUnitData(typeID, cargoType, direction, position, barYield, barVariant, true, 0, 0, false, 0);
		}

		/// <summary>
		/// Constructor for AutoLayout structure.
		/// </summary>
		public UnitData(map_id typeID, int cargoType, UnitDirection direction, int minDistance, bool createWall=false, int maxTubes=-1)
		{
			SetUnitData(typeID, cargoType, direction, new LOCATION(), Yield.Random, Variant.Random, true, minDistance, 0, createWall, maxTubes);
		}

		/// <summary>
		/// Constructor for AutoLayout mine.
		/// </summary>
		public UnitData(map_id typeID, int cargoType, UnitDirection direction, Yield barYield, Variant barVariant, int minDistance, bool createWall=false, int maxTubes=-1)
		{
			SetUnitData(typeID, cargoType, direction, new LOCATION(), barYield, barVariant, true, minDistance, 0, createWall, maxTubes);
		}

		/// <summary>
		/// Constructor for AutoLayout vehicle.
		/// </summary>
		public UnitData(map_id typeID, int cargoType, UnitDirection direction, int minDistance, int spawnDistance)
		{
			SetUnitData(typeID, cargoType, direction, new LOCATION(), Yield.Random, Variant.Random, true, minDistance, spawnDistance, false, 0);
		}

		private void SetUnitData(map_id typeID, int cargoType, UnitDirection direction, LOCATION position, Yield barYield, Variant barVariant,
								bool ignoreLayout, int minDistance, int spawnDistance, bool createWall, int maxTubes)
		{
			m_TypeID = typeID.ToString();
			m_CargoType = GetCargoType(cargoType);
			m_Direction = direction.ToString();
			location = position;
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

			// Must create the beacon for mines
			switch (data.typeID)
			{
				case map_id.CommonOreMine:
					TethysGame.CreateBeacon(map_id.MiningBeacon, pt.x, pt.y, BeaconType.Common, data.barYield, data.barVariant);
					break;

				case map_id.RareOreMine:
					// Game has a bug where rare ore mines can't be created. Common ore mines will turn into the rare mine instead.
					TethysGame.CreateBeacon(map_id.MiningBeacon, pt.x, pt.y, BeaconType.Rare, data.barYield, data.barVariant);
					return TethysGame.CreateUnit(map_id.CommonOreMine, pt.x, pt.y, playerID, data.cargoType, data.direction);
			}

			return TethysGame.CreateUnit(data.typeID, pt.x, pt.y, playerID, data.cargoType, data.direction);
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
