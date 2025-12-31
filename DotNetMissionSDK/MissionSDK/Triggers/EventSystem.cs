using DotNetMissionReader;
using DotNetMissionSDK.AI;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK.Triggers
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi, Pack=1)]
	public struct EventSystemData
	{
		public int gameMark;
		public int gameTick;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] // Maximum supported players
		public bool[] playersDefeated;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3000)] // Maximum supported trigger count
		public EventTriggerData[] triggers;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)] // Maximum supported regions
		public EventRegionData[] regions;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)] // Maximum supported switches
		public int[] switches;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)] // Maximum supported unique unit IDs
		public int[] unitIDs;
	}

	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi, Pack=1)]
	public struct EventTriggerData
	{
		public bool enabled;                // Whether or not the trigger is enabled
		public bool isExecuting;			// Is this trigger executing right now?
		public ushort executionIndex;       // The current action index being executed
		public int executionTick;			// The starting tick for the executing action

		public int eventPlayerID;			// The event player for the executing action
		public int eventUnitID;				// The event unit for the executing action
		public int eventRegionID;			// The event region for the executing action
		public int eventTopicID;			// The event topic for the executing action
	}

	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi, Pack=1)]
	public struct EventRegionData
	{
		public int xMin;
		public int yMin;
		public int xMax;
		public int yMax;
	}

	/// <summary>
	/// Monitors events and executes triggers that are listening to them.
	/// </summary>
	public class EventSystem
	{
		private EventSystemData m_EventData;

		private MissionRoot m_MissionRoot;

		private StateSnapshot m_CurSnapshot;
		private StateSnapshot m_PrevSnapshot;

		private Dictionary<TriggerEventType, List<EventTrigger>> m_Triggers = new Dictionary<TriggerEventType, List<EventTrigger>>();
		private List<EventTrigger> m_AllTriggers = new List<EventTrigger>();
		private List<EventTrigger> m_ExecutingTriggers = new List<EventTrigger>();


		/// <summary>
		/// Initializes the event system.
		/// </summary>
		public EventSystem(SaveData saveData, MissionRoot missionRoot)
		{
			m_EventData = saveData.eventData;
			m_MissionRoot = missionRoot;
		}

		/// <summary>
		/// Should be called whenever a new mission starts, but not when loading saved games.
		/// </summary>
		public void NewMission(MissionVariant combinedVariant)
		{
			InitializeTriggers(combinedVariant);

			// Initialize regions
			for (int i=0; i < m_MissionRoot.regions.Count; ++i)
			{
				RegionData regionData = m_MissionRoot.regions[i];

				m_EventData.regions[i].xMin = regionData.rect.xMin;
				m_EventData.regions[i].yMin = regionData.rect.yMin;
				m_EventData.regions[i].xMax = regionData.rect.xMax;
				m_EventData.regions[i].yMax = regionData.rect.yMax;
			}

			// Initialize trigger save data
			foreach (EventTrigger trigger in m_AllTriggers)
				trigger.NewMission();

			//CheckVictoryCondition, // Conditions restricted to "OP2 Trigger" type
			//CheckDefeatCondition, // Conditions restricted to "OP2 Trigger" type
		}

		/// <summary>
		/// Should be called when a mission is loaded from a saved game. Performs reinitialization of data lost during quit.
		/// </summary>
		public void LoadMission(MissionVariant combinedVariant)
		{
			InitializeTriggers(combinedVariant);

			// Initialize executing triggers
			foreach (EventTrigger trigger in m_AllTriggers)
			{
				if (trigger.isExecuting)
					m_ExecutingTriggers.Add(trigger);
			}
		}

		private void InitializeTriggers(MissionVariant combinedVariant)
		{
			// Create all triggers from trigger data
			int triggerIndex = 0;

			// Gaia triggers
			foreach (TriggerData triggerData in combinedVariant.tethysGame.triggers)
				CreateTrigger(triggerIndex++, triggerData, -1);

			// Player triggers
			for (int i=0; i < combinedVariant.players.Count; ++i)
			{
				foreach (TriggerData triggerData in combinedVariant.players[i].resources.triggers)
					CreateTrigger(triggerIndex++, triggerData, i);
			}
		}

		private void CreateTrigger(int triggerIndex, TriggerData triggerData, int triggerOwnerID)
		{
			// Get list for trigger event type
			List<EventTrigger> list;
			if (!m_Triggers.TryGetValue(triggerData.eventType, out list))
			{
				list = new List<EventTrigger>();
				m_Triggers.Add(triggerData.eventType, list);
			}

			EventTrigger trigger = new EventTrigger(m_EventData, triggerData, m_EventData.triggers[triggerIndex], triggerOwnerID);
			list.Add(trigger);

			// All triggers
			m_AllTriggers.Add(trigger);
		}

		/// <summary>
		/// Should be called when the mission has finished initializing, regardless of whether it is a new game or saved game.
		/// </summary>
		public void StartMission(StateSnapshot startingSnapshot, BotPlayer[] botPlayers)
		{
			m_PrevSnapshot = startingSnapshot;

			foreach (EventTrigger trigger in m_AllTriggers)
				trigger.StartMission(botPlayers);
		}

		public void Update(StateSnapshot stateSnapshot)
		{
			m_CurSnapshot = stateSnapshot;

			// OnGameMark Event
			if (m_EventData.gameMark < TethysGame.Time())
			{
				m_EventData.gameMark = TethysGame.Time();
				FireEvent(TriggerEventType.OnGameMark);
			}

			// OnGameTick Event
			if (m_EventData.gameTick < TethysGame.Tick())
			{
				m_EventData.gameTick = TethysGame.Tick();
				FireEvent(TriggerEventType.OnGameMark);
			}

			// Unit processing
			for (int i=0; i < stateSnapshot.players.Count; ++i)
			{
				PlayerState player = stateSnapshot.players[i];
				PlayerState prevPlayer = m_PrevSnapshot.players[i];

				// Structures
				foreach (StructureState structure in player.units.GetStructures())
				{
					// Search previous snapshot for structure. If it can't be found, this structure was just created.
					StructureState prevStructure = prevPlayer.units.GetListForType(structure.unitType).FirstOrDefault((unit) => unit.unitID == structure.unitID) as StructureState;
					if (prevStructure == null)
						FireEvent(TriggerEventType.OnStructureCreated, player, structure);

					// Handle structure region events
					for (int regionIndex=0; regionIndex < m_EventData.regions.Length; ++regionIndex)
					{
						EventRegionData region = m_EventData.regions[regionIndex];

						bool prevRectInRegion = prevStructure != null && prevStructure.GetRect().DoesRectIntersect(GetMapRect(region));

						if (structure.GetRect().DoesRectIntersect(GetMapRect(region)))
						{
							if (!prevRectInRegion)
								FireEvent(TriggerEventType.OnStructureEnteredRegion, player, structure, regionIndex);

							FireEvent(TriggerEventType.OnStructureInRegion, player, structure, regionIndex);
						}
						else
						{
							if (prevRectInRegion)
								FireEvent(TriggerEventType.OnStructureExitedRegion, player, structure, regionIndex);
						}
					}

					// Handle structure operational events
					if (structure.isEnabled && !prevStructure.isEnabled)
						FireEvent(TriggerEventType.OnStructureOperational, player, structure);

					if (!structure.isEnabled && prevStructure.isEnabled)
						FireEvent(TriggerEventType.OnStructureInoperable, player, structure);

					// Handle factory bay events
					FactoryState factory = structure as FactoryState;
					if (factory != null)
					{
						FactoryState prevFactory = prevStructure as FactoryState;

						for (int j=0; j < factory.storageBayCount; ++j)
						{
							if (factory.GetFactoryCargo(j) == map_id.None)
								continue;

							if (prevFactory == null ||
								factory.GetFactoryCargo(j) != prevFactory.GetFactoryCargo(j) || 
								factory.GetFactoryCargoWeapon(j) != prevFactory.GetFactoryCargoWeapon(j))
							{
								FireEvent(TriggerEventType.OnStructureKitEnteredBay, player, structure);
								break;
							}
						}
					}

					// Handle lab events
					LabState lab = structure as LabState;
					if (lab != null)
					{
						LabState prevLab = prevStructure as LabState;
						if (lab.labCurrentTopic >= 0 && (prevLab == null || lab.labCurrentTopic != prevLab.labCurrentTopic))
							FireEvent(TriggerEventType.OnResearchStarted, player, lab, -1, lab.labCurrentTopic);
					}
				}

				foreach (StructureState prevStructure in prevPlayer.units.GetStructures())
				{
					// Search current snapshot for structure. If it can't be found, this structure was just destroyed.
					StructureState structure = prevPlayer.units.GetListForType(prevStructure.unitType).FirstOrDefault((unit) => unit.unitID == prevStructure.unitID) as StructureState;
					if (structure == null)
						FireEvent(TriggerEventType.OnStructureDestroyed, player, prevStructure);
				}

				// Vehicles
				foreach (VehicleState vehicle in player.units.GetVehicles())
				{
					// Search previous snapshot for vehicle. If it can't be found, this vehicle was just created.
					VehicleState prevVehicle = prevPlayer.units.GetListForType(vehicle.unitType).FirstOrDefault((unit) => unit.unitID == vehicle.unitID) as VehicleState;
					if (prevVehicle == null)
						FireEvent(TriggerEventType.OnVehicleCreated, player, vehicle);

					// Handle vehicle region events
					for (int regionIndex=0; regionIndex < m_EventData.regions.Length; ++regionIndex)
					{
						EventRegionData region = m_EventData.regions[regionIndex];

						bool prevRectInRegion = prevVehicle != null && GetMapRect(region).Contains(prevVehicle.position);

						if (GetMapRect(region).Contains(vehicle.position))
						{
							if (!prevRectInRegion)
								FireEvent(TriggerEventType.OnVehicleEnteredRegion, player, vehicle, regionIndex);

							FireEvent(TriggerEventType.OnVehicleInRegion, player, vehicle, regionIndex);
						}
						else
						{
							if (prevRectInRegion)
								FireEvent(TriggerEventType.OnVehicleExitedRegion, player, vehicle, regionIndex);
						}
					}

					// Wreckage loaded event
					CargoTruckState truck = vehicle as CargoTruckState;
					if (truck != null)
					{
						CargoTruckState prevTruck = prevVehicle as CargoTruckState;
						bool prevTruckHasWreckage = prevTruck != null && prevTruck.cargoType == TruckCargo.Garbage;

						if (!prevTruckHasWreckage && truck.cargoType == TruckCargo.Garbage)
							FireEvent(TriggerEventType.OnWreckageLoaded, player, truck);
					}

					// Convec loaded event
					ConvecState convec = vehicle as ConvecState;
					if (convec != null && convec.cargoType != map_id.None)
					{
						ConvecState prevConvec = prevVehicle as ConvecState;
						
						if (prevConvec == null || convec.cargoType != prevConvec.cargoType)
							FireEvent(TriggerEventType.OnConvecLoaded, player, convec);
					}
				}

				foreach (VehicleState prevVehicle in prevPlayer.units.GetVehicles())
				{
					// Search current snapshot for vehicle. If it can't be found, this vehicle was just destroyed.
					VehicleState vehicle = prevPlayer.units.GetListForType(prevVehicle.unitType).FirstOrDefault((unit) => unit.unitID == prevVehicle.unitID) as VehicleState;
					if (vehicle == null)
						FireEvent(TriggerEventType.OnVehicleDestroyed, player, prevVehicle);
				}

				// Research completed
				for (int j=0; j < stateSnapshot.techInfo.Count; ++j)
				{
					if (prevPlayer.HasTechnologyByIndex(j)) continue;
					if (player.HasTechnologyByIndex(j))
						FireEvent(TriggerEventType.OnResearchCompleted, player, null, -1, stateSnapshot.techInfo[j].techID);
				}

				// Starship module deployed
				if (player.starship.count > prevPlayer.starship.count)
					FireEvent(TriggerEventType.OnStarshipModuleDeployed, player);
			}

			// Handle wreckage states
			for (int i=0; i < stateSnapshot.gaia.wreckages.Count; ++i)
			{
				WreckageState wreckage = stateSnapshot.gaia.wreckages[i];

				if (wreckage.isDiscovered && !m_PrevSnapshot.gaia.wreckages[i].isDiscovered)
					FireEvent(TriggerEventType.OnWreckageDiscovered);
			}

			// Execute triggers
			for (int i=0; i < m_ExecutingTriggers.Count; ++i)
			{
				EventTrigger trigger = m_ExecutingTriggers[i];

				trigger.Update(this, stateSnapshot, m_EventData.gameTick);
				if (!trigger.isExecuting)
					m_ExecutingTriggers.RemoveAt(i--);
			}

			m_PrevSnapshot = stateSnapshot;
		}

		/// <summary>
		/// Called by trigger to indicate player has been defeated.
		/// </summary>
		public void MarkPlayerDefeated(PlayerState player)
		{
			if (m_EventData.playersDefeated[player.playerID])
				return;

			m_EventData.playersDefeated[player.playerID] = true;

			FireEvent(TriggerEventType.OnPlayerDefeated, player);
		}

		private void FireEvent(TriggerEventType eventType, PlayerState eventPlayer=null, UnitState eventUnit=null, int eventRegionIndex=-1, int eventTopic=-1)
		{
			int currentMark = TethysGame.Time();
			int currentTick = TethysGame.Tick();

			// Get triggers for this event
			List<EventTrigger> triggers;
			if (!m_Triggers.TryGetValue(eventType, out triggers))
				return;

			// Process all triggers for this event
			foreach (EventTrigger trigger in triggers)
			{
				if (trigger.CheckConditions(m_CurSnapshot, currentMark, currentTick, eventPlayer, eventUnit, eventRegionIndex, eventTopic))
				{
					trigger.Execute(currentTick, eventPlayer, eventUnit, eventRegionIndex, eventTopic);
					m_ExecutingTriggers.Add(trigger);
				}
			}
		}

		private MAP_RECT GetMapRect(EventRegionData eventRegion)
		{
			return MAP_RECT.FromMinMax(eventRegion.xMin, eventRegion.yMin, eventRegion.xMax, eventRegion.yMax);
		}
	}
}
