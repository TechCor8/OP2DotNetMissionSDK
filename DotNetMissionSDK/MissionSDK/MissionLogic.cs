using DotNetMissionSDK.AI;
using DotNetMissionSDK.Json;
using DotNetMissionSDK.Triggers;
using DotNetMissionSDK.State;
using System;
using System.Collections.Generic;
using DotNetMissionSDK.State.Snapshot;

namespace DotNetMissionSDK
{
	public class MissionLogic
	{
		private MissionRoot m_Root;
		private SaveData m_SaveData;
		private BotPlayer[] m_BotPlayer = new BotPlayer[8];

		private TriggerManager m_TriggerManager;

		private List<DisasterData> m_Disasters = new List<DisasterData>();
		private Dictionary<int, TriggerData> m_TriggerData = new Dictionary<int, TriggerData>();

		
		/// <summary>
		/// Prepares mission logic for use.
		/// </summary>
		/// <param name="root">The filled JSON data root.</param>
		/// <param name="saveData">The save data class.</param>
		/// <param name="triggerManager">The trigger manager used for the mission.</param>
		public MissionLogic(MissionRoot root, SaveData saveData, TriggerManager triggerManager)
		{
			m_Root = root;
			m_SaveData = saveData;
			m_TriggerManager = triggerManager;

			m_TriggerManager.onTriggerFired += OnTriggerExecuted;

			// Initialize game state
			GameState.Initialize(triggerManager, saveData);
		}

		/// <summary>
		/// Called when a new mission should start. Performs initial setup.
		/// </summary>
		/// <returns>True on success.</returns>
		public virtual bool InitializeNewMission()
		{
			Console.WriteLine("Mission started.");

			// Initialize game state triggers
			GameState.InitializeNew();

			MissionRoot root = m_Root;

			// If JSON not loaded, skip it
			if (root == null)
				return true;

			List<Unit> createdUnits = new List<Unit>();

			// Setup Game
			TethysGame.SetDaylightEverywhere(root.tethysGame.daylightEverywhere);
			TethysGame.SetDaylightMoves(root.tethysGame.daylightMoves);
			GameMap.SetInitialLightLevel(root.tethysGame.initialLightLevel);
			if (!TethysGame.UsesDayNight())
				TethysGame.SetDaylightEverywhere(true);

			TethysGame.SetMusicPlayList(root.tethysGame.musicPlayList.songIDs.Length, root.tethysGame.musicPlayList.repeatStartIndex, root.tethysGame.musicPlayList.songIDs);

			// Beacons
			foreach (GameData.Beacon beacon in root.tethysGame.beacons)
			{
				LOCATION spawnPt = beacon.spawnRect.GetRandomPointInRect();
				spawnPt = TethysGame.GetMapCoordinates(spawnPt);

				TethysGame.CreateBeacon(beacon.mapID, spawnPt.x, spawnPt.y, beacon.oreType, beacon.barYield, beacon.barVariant);
			}

			// Markers
			foreach (GameData.Marker marker in root.tethysGame.markers)
			{
				LOCATION spawnPt = marker.spawnRect.GetRandomPointInRect();
				spawnPt = TethysGame.GetMapCoordinates(spawnPt);

				Unit unit = TethysGame.PlaceMarker(spawnPt.x, spawnPt.y, marker.markerType);
			}

			// Wreckage
			foreach (GameData.Wreckage wreck in root.tethysGame.wreckage)
			{
				LOCATION spawnPt = wreck.spawnRect.GetRandomPointInRect();
				spawnPt = TethysGame.GetMapCoordinates(spawnPt);

				TethysGame.CreateWreck(spawnPt.x, spawnPt.y, wreck.techID, wreck.isVisible);
			}

			// Tubes
			foreach (GameData.WallTube wallTube in root.tethysGame.wallTubes)
			{
				LOCATION location = TethysGame.GetMapCoordinates(wallTube.location);
				TethysGame.CreateWallOrTube(location.x, location.y, 0, wallTube.typeID);
			}

			// Setup Players
			foreach (PlayerData data in root.players)
			{
				Player player = TethysGame.GetPlayer(data.id);

				player.SetTechLevel(data.techLevel);

				switch ((MoraleLevel)data.moraleLevel)
				{
					case MoraleLevel.Excellent:		TethysGame.ForceMoraleGreat(data.id);		break;
					case MoraleLevel.Good:			TethysGame.ForceMoraleGood(data.id);		break;
					case MoraleLevel.Fair:			TethysGame.ForceMoraleOK(data.id);			break;
					case MoraleLevel.Poor:			TethysGame.ForceMoralePoor(data.id);		break;
					case MoraleLevel.Terrible:		TethysGame.ForceMoraleRotten(data.id);		break;
				}

				if ((TethysGame.UsesMorale() || root.levelDetails.missionType == MissionType.Colony) && data.freeMorale)
					TethysGame.FreeMoraleLevel(data.id);

				// Only set player colony type and color if playing single player
				if (!data.isHuman ||
					root.levelDetails.missionType == MissionType.Colony ||
					root.levelDetails.missionType == MissionType.Tutorial ||
					root.levelDetails.missionType == MissionType.AutoDemo)
				{
					if (data.isEden)
						player.GoEden();
					else
						player.GoPlymouth();

					player.SetColorNumber(data.color);
				}

				if (data.isHuman)
					player.GoHuman();
				else
					player.GoAI();
				
				foreach (int allyID in data.allies)
					player.AllyWith(allyID);

				LOCATION centerView = TethysGame.GetMapCoordinates(data.centerView);
				player.CenterViewOn(centerView.x, centerView.y);

				player.SetKids(data.kids);
				player.SetWorkers(data.workers);
				player.SetScientists(data.scientists);
				player.SetOre(data.commonOre);
				player.SetRareOre(data.rareOre);
				player.SetFoodStored(data.food);
				player.SetSolarSat(data.solarSatellites);

				foreach (int techID in data.completedResearch)
					player.MarkResearchComplete(techID);

				// Units
				foreach (UnitData unitData in data.units)
				{
					LOCATION spawnPt = TethysGame.GetMapCoordinates(unitData.location);

					Unit unit = unitData.CreateUnit(data.id, spawnPt);
					unit.DoSetLights(true);

					createdUnits.Add(unit);
				}
			}

			// Setup Autolayout bases
			BaseGenerator baseGenerator = new BaseGenerator(createdUnits);

			foreach (AutoLayout layout in root.layouts)
			{
				baseGenerator.Generate(TethysGame.GetPlayer(layout.playerID), layout.baseCenterPt, layout.units);
			}

			foreach (Unit unit in baseGenerator.generatedUnits)
				unit.DoSetLights(true);

			// Setup Disasters
			InitializeDisasters();

			// Setup Triggers
			List<TriggerData> triggers = new List<TriggerData>(root.triggers);
			Dictionary<int, TriggerStub> triggerLookup = new Dictionary<int, TriggerStub>();
			int previousCount = 0;

			// Keep performing passes until all triggers have been processed
			while (triggers.Count > 0)
			{
				// Prevent infinite loop. If parent triggers are not found, they will stay in the trigger queue.
				if (triggers.Count == previousCount)
				{
					Console.WriteLine("Warning: " + triggers.Count + " triggers not processed.");
					break;
				}

				previousCount = triggers.Count;

				// Perform a processing pass on the triggers
				for (int i=0; i < triggers.Count; ++i)
				{
					TriggerData data = triggers[i];

					// Get parent trigger if there is one
					TriggerStub parentTrigger = null;
					if (data.triggerID > 0)
						triggerLookup.TryGetValue(data.triggerID, out parentTrigger);

					bool wasProcessed = true;

					TriggerStub trigger = null;

					switch (data.type)
					{
						case TriggerType.None:
							Console.WriteLine("Warning: Trigger Type None");
							break;

						case TriggerType.Victory:
							if (parentTrigger == null)
							{
								wasProcessed = false;
								break;
							}
							
							trigger = TriggerStub.CreateVictoryCondition(data.id, data.enabled, parentTrigger, data.message);
							break;

						case TriggerType.Failure:
							if (parentTrigger == null)
							{
								wasProcessed = false;
								break;
							}

							trigger = TriggerStub.CreateFailureCondition(data.id, data.enabled, parentTrigger);
							break;

						case TriggerType.OnePlayerLeft:
							trigger = TriggerStub.CreateOnePlayerLeftTrigger(data.id, data.enabled, data.oneShot);
							break;

						case TriggerType.Evac:
							trigger = TriggerStub.CreateEvacTrigger(data.id, data.enabled, data.oneShot, data.playerID);
							break;

						case TriggerType.Midas:
							trigger = TriggerStub.CreateMidasTrigger(data.id, data.enabled, data.oneShot, data.time);
							break;

						case TriggerType.Operational:
							trigger = TriggerStub.CreateOperationalTrigger(data.id, data.enabled, data.oneShot, data.playerID, data.unitType, data.count, data.compareType);
							break;

						case TriggerType.Research:
							trigger = TriggerStub.CreateResearchTrigger(data.id, data.enabled, data.oneShot, data.techID, data.playerID);
							break;

						case TriggerType.Resource:
							trigger = TriggerStub.CreateResourceTrigger(data.id, data.enabled, data.oneShot, data.resourceType, data.count, data.playerID, data.compareType);
							break;

						case TriggerType.Kit:
							trigger = TriggerStub.CreateKitTrigger(data.id, data.enabled, data.oneShot, data.playerID, data.unitType, data.count);
							break;

						case TriggerType.Escape:
							trigger = TriggerStub.CreateEscapeTrigger(data.id, data.enabled, data.oneShot, data.playerID, data.x, data.y, data.width, data.height, data.count, data.unitType, data.cargoType, data.cargoAmount);
							break;

						case TriggerType.Count:
							trigger = TriggerStub.CreateCountTrigger(data.id, data.enabled, data.oneShot, data.playerID, data.unitType, data.cargoOrWeaponType, data.count, data.compareType);
							break;

						case TriggerType.VehicleCount:
							trigger = TriggerStub.CreateVehicleCountTrigger(data.id, data.enabled, data.oneShot, data.playerID, data.count, data.compareType);
							break;

						case TriggerType.BuildingCount:
							trigger = TriggerStub.CreateBuildingCountTrigger(data.id, data.enabled, data.oneShot, data.playerID, data.count, data.compareType);
							break;

						case TriggerType.Attacked:
							wasProcessed = false;
							Console.WriteLine("Attacked trigger not implemented!");
							break;

						case TriggerType.Damaged:
							wasProcessed = false;
							Console.WriteLine("Damaged trigger not implemented!!");
							break;

						case TriggerType.Time:
							trigger = TriggerStub.CreateTimeTrigger(data.id, data.enabled, data.oneShot, data.time);
							break;

						case TriggerType.TimeRange:
							trigger = TriggerStub.CreateTimeTrigger(data.id, data.enabled, data.oneShot, data.minTime, data.maxTime);
							break;

						case TriggerType.Point:
							trigger = TriggerStub.CreatePointTrigger(data.id, data.enabled, data.oneShot, data.playerID, data.x, data.y);
							break;

						case TriggerType.Rect:
							trigger = TriggerStub.CreateRectTrigger(data.id, data.enabled, data.oneShot, data.playerID, data.x, data.y, data.width, data.height);
							break;

						case TriggerType.SpecialTarget:
							Console.WriteLine("SpecialTarget not implemented!");
							break;

						default:
							wasProcessed = false;
							Console.WriteLine("Invalid Trigger Type: " + data.type);
							break;
					}

					if (wasProcessed)
					{
						try
						{
							triggerLookup.Add(data.id, trigger);
						}
						catch (ArgumentException e)
						{
							// More than one trigger has the same ID
							Console.WriteLine(e);
						}

						m_TriggerManager.AddTrigger(trigger);

						triggers.RemoveAt(i--);
					}
				}
			}

			CreateTriggerDataLookupTable();

			StartMission();

			return true;
		}

		/// <summary>
		/// Called when a mission is loaded from a saved game. Performs reinitialization of data lost during quit.
		/// </summary>
		public virtual void LoadMission()
		{
			Console.WriteLine("Mission loaded.");

			InitializeDisasters();
			CreateTriggerDataLookupTable();

			StartMission();
		}

		private void InitializeDisasters()
		{
			MissionRoot root = m_Root;

			m_Disasters.Clear();

			// Setup Disasters
			if (TethysGame.CanHaveDisasters() || root.levelDetails.missionType == MissionType.Colony)
			{
				foreach (DisasterData disaster in root.disasters)
				{
					disaster.timeUntilNextDisaster = disaster.startTime;
					m_Disasters.Add(disaster);
				}
			}
		}

		/// <summary>
		/// Creates a table used for looking up trigger actions
		/// </summary>
		private void CreateTriggerDataLookupTable()
		{
			m_TriggerData.Clear();

			foreach (TriggerData data in m_Root.triggers)
				m_TriggerData.Add(data.id, data);
		}

		/// <summary>
		/// Called when the mission has finished initializing, regardless of whether it is a new game or saved game.
		/// </summary>
		protected virtual void StartMission()
		{
			MissionRoot root = m_Root;

			for (int i=0; i < root.players.Length; ++i)
			{
				if (root.players[i].botType == BotType.None)
					continue;

				m_BotPlayer[i] = new BotPlayer(root.players[i].botType, i);
				m_BotPlayer[i].Start();
			}
		}

		/// <summary>
		/// Called when a trigger has been executed.
		/// </summary>
		/// <param name="trigger">The trigger that was executed.</param>
		protected virtual void OnTriggerExecuted(TriggerStub trigger)
		{
			TriggerData data;
			if (!m_TriggerData.TryGetValue(trigger.id, out data))
			{
				if (trigger.id < TriggerStub.ReservedIDStart)
					Console.WriteLine("Could not find trigger data for ID: " + trigger.id);
				return;
			}

			// Execute trigger actions
			foreach (ActionData action in data.actions)
				ExecuteAction(action);
		}

		private void ExecuteAction(ActionData action)
		{
			switch (action.type)
			{
				case "AddMessage":
					TethysGame.AddMessage(0, 0, action.message, action.playerID, action.soundID);
					break;
			}
		}

		/// <summary>
		/// Called every game cycle.
		/// </summary>
		/// <param name="stateSnapshot">The current immutable state of the game.</param>
		public virtual void Update(StateSnapshot stateSnapshot)
		{
			int currentTime = TethysGame.Time();

			// Update disasters
			foreach (DisasterData disaster in m_Disasters)
			{
				// Stop processing disaster if the time period has ended
				if (currentTime > disaster.endTime)
					continue;

				if (currentTime >= disaster.timeUntilNextDisaster)
				{
					disaster.timeUntilNextDisaster += TethysGame.GetRandomRange(disaster.minDelay, disaster.maxDelay);

					FireDisaster(disaster);
				}
			}

			// Update bots
			for (int i=0; i < m_BotPlayer.Length; ++i)
				m_BotPlayer[i]?.Update(stateSnapshot);
		}

		private void FireDisaster(DisasterData disaster)
		{
			LOCATION spawnPt = TethysGame.GetMapCoordinates(disaster.srcRect.GetRandomPointInRect());
			LOCATION destPt = TethysGame.GetMapCoordinates(disaster.destRect.GetRandomPointInRect());

			switch (disaster.type)
			{
				case "Meteor":			TethysGame.SetMeteor(spawnPt.x, spawnPt.y, disaster.size);								break;
				case "Earthquake":		TethysGame.SetEarthquake(spawnPt.x, spawnPt.y, disaster.size);							break;
				case "Lightning":		TethysGame.SetLightning(spawnPt.x, spawnPt.y, disaster.duration, destPt.x, destPt.y);	break;
				case "Tornado":			TethysGame.SetTornado(spawnPt.x, spawnPt.y, disaster.duration, destPt.x, destPt.y, 0);	break;
				case "Eruption":		TethysGame.SetEruption(spawnPt.x, spawnPt.y, disaster.duration);						break;
				case "Blight":
					GameMap.SetVirusUL(spawnPt.x, spawnPt.y, true);
					TethysGame.SetMicrobeSpreadSpeed(disaster.duration);
					break;
			}
		}

		protected bool AddTrigger(TriggerStub triggerStub)
		{
			return m_TriggerManager.AddTrigger(triggerStub);
		}

		/// <summary>
		/// Releases all mission resources.
		/// </summary>
		public virtual void Dispose()
		{
			Console.WriteLine("Mission Ended.");

			m_TriggerManager.onTriggerFired -= OnTriggerExecuted;

			// Dispose game state
			GameState.Dispose();
		}
	}
}
