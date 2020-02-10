using DotNetMissionSDK.Json;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK.Triggers
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi, Pack=1)]
	public class EventSystemData
	{
		public int gameMark;
		public int gameTick;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] // Maximum supported players
		public bool[] playersDefeated;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3000)] // Maximum supported trigger count
		public EventTriggerData[] triggers;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)] // Maximum supported switches
		public byte[] switches;
	}

	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi, Pack=1)]
	public class EventTriggerData
	{
		public sbyte playerIndex;
		public sbyte difficultyIndex;
		public ushort triggerIndex;

		public bool enabled;				// Whether or not the trigger is enabled
		public ushort executionIndex;		// The current action index being executed
	}

	/// <summary>
	/// Monitors events and executes triggers that are listening to them.
	/// </summary>
	public class EventSystem
	{
		private SaveData m_SaveData;
		private EventSystemData m_EventData;

		private MissionRoot m_MissionRoot;

		private StateSnapshot m_PrevSnapshot;


		/// <summary>
		/// Initializes the event system.
		/// Should be called every time the mission starts.
		/// </summary>
		public void Initialize(SaveData saveData, MissionRoot missionRoot, StateSnapshot startingSnapshot)
		{
			m_EventData = saveData.eventData;
			m_MissionRoot = missionRoot;
			m_PrevSnapshot = startingSnapshot;
		}

		/// <summary>
		/// Should be called whenever a new mission starts, but not when loading saved games.
		/// </summary>
		public void NewMission()
		{
			//CheckVictoryCondition, // Conditions restricted to "OP2 Trigger" type
			//CheckDefeatCondition, // Conditions restricted to "OP2 Trigger" type
		}

		public void Update(StateSnapshot stateSnapshot)
		{
			// OnGameMark Event
			if (m_EventData.gameMark < TethysGame.Time())
			{
				m_EventData.gameMark = TethysGame.Time();
				FireEvent(TriggerEventType.OnGameMark);
			}

			// OnGameTick Event
			if (m_EventData.gameTick < TethysGame.Tick())
			{
				m_EventData.gameTick = TethysGame.Time();
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
					foreach (RegionData region in m_MissionRoot.regions)
					{
						bool prevRectInRegion = prevStructure != null && prevStructure.GetRect().DoesRectIntersect(region.rect);

						if (structure.GetRect().DoesRectIntersect(region.rect))
						{
							if (!prevRectInRegion)
								FireEvent(TriggerEventType.OnStructureEnteredRegion, player, structure, region);

							FireEvent(TriggerEventType.OnStructureInRegion, player, structure, region);
						}
						else
						{
							if (prevRectInRegion)
								FireEvent(TriggerEventType.OnStructureExitedRegion, player, structure, region);
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
							FireEvent(TriggerEventType.OnResearchStarted, player, lab, null, lab.labCurrentTopic);
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
					foreach (RegionData region in m_MissionRoot.regions)
					{
						bool prevRectInRegion = prevVehicle != null && ((MAP_RECT)region.rect).Contains(prevVehicle.position);

						if (((MAP_RECT)region.rect).Contains(vehicle.position))
						{
							if (!prevRectInRegion)
								FireEvent(TriggerEventType.OnVehicleEnteredRegion, player, vehicle, region);

							FireEvent(TriggerEventType.OnVehicleInRegion, player, vehicle, region);
						}
						else
						{
							if (prevRectInRegion)
								FireEvent(TriggerEventType.OnVehicleExitedRegion, player, vehicle, region);
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
						FireEvent(TriggerEventType.OnResearchCompleted, player, null, null, stateSnapshot.techInfo[j].techID);
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

		private void FireEvent(TriggerEventType eventType, PlayerState eventPlayer=null, UnitState eventUnit=null, RegionData eventRegion=null, int eventTopic=-1)
		{
			int currentMark = TethysGame.Time();
			int currentTick = TethysGame.Tick();
		}
	}
}
