using DotNetMissionSDK.HFL;
using DotNetMissionSDK.State.Game;
using DotNetMissionSDK.Triggers;
using DotNetMissionSDK.Units;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetMissionSDK.State
{
	/// <summary>
	/// Represents the live game state.
	/// References contained in this state may cease to be valid on the next update.
	/// </summary>
	public class GameState
	{
		private static SaveData m_SaveData;        // The save data object to store persistent state

		private static Dictionary<int, UnitEx> m_Units = new Dictionary<int, UnitEx>();
		private static List<UnitEx> m_UnitCache = new List<UnitEx>();

		// Players
		public static ReadOnlyCollection<PlayerEx> players						{ get; private set; }
		public static ReadOnlyCollection<PlayerStarship> playerStarships		{ get; private set; }

		/// <summary>
		/// Contains all gaia and player units. Key is the unit's ID.
		/// </summary>
		public static ReadOnlyDictionary<int, UnitEx> units						{ get { return new ReadOnlyDictionary<int, UnitEx>(m_Units);	}	}

		// Delegates
		//public delegate void OnUnitCallback(UnitEx unit);

		// Events
		//public static event OnUnitCallback onUnitAdded;
		//public static event OnUnitCallback onUnitRemoved;

		/// <summary>
		/// Returns the unit that has the given ID or null, if unit is not found.
		/// </summary>
		public static UnitEx GetUnit(int unitID)
		{
			UnitEx unit;
			units.TryGetValue(unitID, out unit);
			return unit;
		}


		/// <summary>
		/// Initializes the game state. Always call when the mission is started.
		/// </summary>
		/// <param name="triggerManager">The trigger manager for tracking player events.</param>
		/// <param name="saveData">The global save object for storing persistent state.</param>
		public static void Initialize(TriggerManager triggerManager, SaveData saveData)
		{
			m_SaveData = saveData;

			// Initialize players
			PlayerEx[] players = new PlayerEx[TethysGame.NoPlayers()];
			for (int i=0; i < players.Length; ++i)
				players[i] = TethysGame.GetPlayer(i);

			GameState.players = new ReadOnlyCollection<PlayerEx>(players);


			// Initialize player starships
			PlayerStarship[] starships = new PlayerStarship[players.Length];

			for (int i=0; i < starships.Length; ++i)
				starships[i] = new PlayerStarship(triggerManager, i, m_SaveData.playerStarship[i]);

			playerStarships = new ReadOnlyCollection<PlayerStarship>(starships);
		}

		/// <summary>
		/// Creates triggers for new mission. Only call when a new mission is started.
		/// NOTE: Initialize must be called first.
		/// </summary>
		public static void InitializeNew()
		{
			// Initialize player starships for a new mission
			for (int i=0; i < playerStarships.Count; ++i)
			{
				PlayerStarshipSaveData starshipSaveData = new PlayerStarshipSaveData();
				m_SaveData.playerStarship[i] = starshipSaveData;
				playerStarships[i].InitializeNewMission(starshipSaveData);
			}
		}

		/// <summary>
		/// Updates the game state.
		/// Must be called at the beginning of every frame.
		/// </summary>
		public static void Update()
		{
			// Mark lookup table for deletion
			Dictionary<int, bool> deleteLookup = new Dictionary<int, bool>(m_Units.Count);
			foreach (int key in m_Units.Keys)
				deleteLookup[key] = true;

			// Update units list
			for (int playerID=0; playerID < players.Count; ++playerID)
			{
				for (int i=21; i < 59; ++i)
				{
					foreach (UnitEx unit in new PlayerBuildingEnum(playerID, (map_id)i))
					{
						// If unit already registered, skip it
						if (m_Units.ContainsKey(unit.GetStubIndex()))
						{
							deleteLookup[unit.GetStubIndex()] = false;
							continue;
						}

						switch ((map_id)i)
						{
							case map_id.CommonOreMine:			m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.RareOreMine:			m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.GuardPost:				m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.LightTower:				m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.CommonStorage:			m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.RareStorage:			m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.Forum:					m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.CommandCenter:			m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.MHDGenerator:			m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.Residence:				m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.RobotCommand:			m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.TradeCenter:			m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.BasicLab:				m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.MedicalCenter:			m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.Nursery:				m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.SolarPowerArray:		m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.RecreationFacility:		m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.University:				m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.Agridome:				m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.DIRT:					m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.Garage:					m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.MagmaWell:				m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.MeteorDefense:			m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.GeothermalPlant:		m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.ArachnidFactory:		m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.ConsumerFactory:		m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.StructureFactory:		m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.VehicleFactory:			m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.StandardLab:			m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.AdvancedLab:			m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.Observatory:			m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.ReinforcedResidence:	m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.AdvancedResidence:		m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.CommonOreSmelter:		m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.Spaceport:				m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.RareOreSmelter:			m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.GORF:					m_Units.Add(unit.GetStubIndex(), unit);		break;
							case map_id.Tokamak:				m_Units.Add(unit.GetStubIndex(), unit);		break;
						}
					}
				}

				foreach (UnitEx unit in new PlayerUnitEnum(playerID))
				{
					// If unit already registered, skip it
					if (m_Units.ContainsKey(unit.GetStubIndex()))
					{
						deleteLookup[unit.GetStubIndex()] = false;
						continue;
					}

					switch (unit.GetUnitType())
					{
						case map_id.CargoTruck:				m_Units.Add(unit.GetStubIndex(), new Vehicle(unit));		break;
						case map_id.ConVec:					m_Units.Add(unit.GetStubIndex(), new Vehicle(unit));		break;
						case map_id.Spider:					m_Units.Add(unit.GetStubIndex(), new Vehicle(unit));		break;
						case map_id.Scorpion:				m_Units.Add(unit.GetStubIndex(), new Vehicle(unit));		break;
						case map_id.Lynx:					m_Units.Add(unit.GetStubIndex(), new Vehicle(unit));		break;
						case map_id.Panther:				m_Units.Add(unit.GetStubIndex(), new Vehicle(unit));		break;
						case map_id.Tiger:					m_Units.Add(unit.GetStubIndex(), new Vehicle(unit));		break;
						case map_id.RoboSurveyor:			m_Units.Add(unit.GetStubIndex(), new Vehicle(unit));		break;
						case map_id.RoboMiner:				m_Units.Add(unit.GetStubIndex(), new Vehicle(unit));		break;
						case map_id.GeoCon:					m_Units.Add(unit.GetStubIndex(), new Vehicle(unit));		break;
						case map_id.Scout:					m_Units.Add(unit.GetStubIndex(), new Vehicle(unit));		break;
						case map_id.RoboDozer:				m_Units.Add(unit.GetStubIndex(), new Vehicle(unit));		break;
						case map_id.EvacuationTransport:	m_Units.Add(unit.GetStubIndex(), new Vehicle(unit));		break;
						case map_id.RepairVehicle:			m_Units.Add(unit.GetStubIndex(), new Vehicle(unit));		break;
						case map_id.Earthworker:			m_Units.Add(unit.GetStubIndex(), new Vehicle(unit));		break;
					}
				}
			}

			// Any lookups still marked for deletion should be deleted. The units no longer exist.
			foreach (KeyValuePair<int, bool> kvDeleteUnit in deleteLookup)
			{
				if (kvDeleteUnit.Value)
				{
					m_Units[kvDeleteUnit.Key].OnDestroy();
					m_Units.Remove(kvDeleteUnit.Key);
				}
			}

			// Update all units
			foreach (UnitEx unit in m_Units.Values)
				unit.Update();

			// Clear cache
			m_UnitCache.Clear();
		}

		/// <summary>
		/// Disposes state data.
		/// </summary>
		public static void Dispose()
		{
			foreach (PlayerStarship starship in playerStarships)
				starship.Dispose();
		}
	}
}
