using DotNetMissionSDK.AI;
using DotNetMissionSDK.Triggers;
using DotNetMissionSDK.State;
using System;
using System.Collections.Generic;
using DotNetMissionSDK.State.Snapshot;
using System.Linq;
using DotNetMissionReader;

namespace DotNetMissionSDK
{
	public class MissionLogic
	{
		private MissionRoot m_Root;
		private SaveData m_SaveData;
		private BotPlayer[] m_BotPlayer = new BotPlayer[8];

		private TriggerManager m_TriggerManager;
		private EventSystem m_EventSystem;

		private List<DisasterData> m_Disasters = new List<DisasterData>();
		private Dictionary<int, OP2TriggerData> m_TriggerData = new Dictionary<int, OP2TriggerData>();

		
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

			m_EventSystem = new EventSystem(saveData, root);
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

			// Startup Flags
			bool isMultiplayer = (int)root.levelDetails.GetMissionType() <= -4 && (int)root.levelDetails.GetMissionType() >= -8;
			int localDifficulty = TethysGame.GetPlayer(TethysGame.LocalPlayer()).Difficulty();

			// Select mission variant (random)
			m_SaveData.missionVariantIndex = (byte)TethysGame.GetRandomRange(0, root.missionVariants.Count);

			// Combine master variant with selected variant. The master variant is always used as a base.
			MissionVariant missionVariant = GetCombinedDifficultyVariant(root);
			GameData tethysGame = missionVariant.tethysGame;

			// Setup Game
			TethysGame.SetDaylightEverywhere(tethysGame.daylightEverywhere);
			TethysGame.SetDaylightMoves(tethysGame.daylightMoves);
			GameMap.SetInitialLightLevel(tethysGame.initialLightLevel);

			// If this is a multiplayer game, use the game-specified light settings
			if (isMultiplayer && !TethysGame.UsesDayNight())
				TethysGame.SetDaylightEverywhere(true);

			TethysGame.SetMusicPlayList(tethysGame.musicPlayList.songIDs.Length, tethysGame.musicPlayList.repeatStartIndex, tethysGame.musicPlayList.songIDs);

			// Select Beacons
			List<GameData.Beacon> beacons = new List<GameData.Beacon>();
			foreach (var group in new List<GameData.Beacon>(tethysGame.beacons).GroupBy(b => b.id))
			{
				List<GameData.Beacon> groupBeacons = group.ToList();
				if (groupBeacons[0].id <= 0)
					beacons.AddRange(groupBeacons);
				else
				{
					// Get a random beacon for this group ID
					int randomIndex = TethysGame.GetRandomRange(0, groupBeacons.Count);
					beacons.Add(groupBeacons[randomIndex]);
				}
			}

			// Create beacons
			foreach (GameData.Beacon beacon in beacons)
			{
				LOCATION spawnPt = TethysGame.GetMapCoordinates(beacon.position.ToLocation());

				int stubIndex = TethysGame.CreateBeacon(beacon.GetMapID(), spawnPt.x, spawnPt.y, beacon.GetOreType(), beacon.GetBarYield(), beacon.GetBarVariant());
				SetUnitID(beacon.id, stubIndex);
			}

			// Select markers
			List<GameData.Marker> markers = new List<GameData.Marker>();
			foreach (var group in new List<GameData.Marker>(tethysGame.markers).GroupBy(m => m.id))
			{
				List<GameData.Marker> groupMarkers = group.ToList();
				if (groupMarkers[0].id <= 0)
					markers.AddRange(groupMarkers);
				else
				{
					// Get a random marker for this group ID
					int randomIndex = TethysGame.GetRandomRange(0, groupMarkers.Count);
					markers.Add(groupMarkers[randomIndex]);
				}
			}

			// Create markers
			foreach (GameData.Marker marker in markers)
			{
				LOCATION spawnPt = TethysGame.GetMapCoordinates(marker.position.ToLocation());

				Unit unit = TethysGame.PlaceMarker(spawnPt.x, spawnPt.y, marker.GetMarkerType());
				SetUnitID(marker.id, unit.GetStubIndex());
			}

			// Select wreckage
			List<GameData.Wreckage> wreckages = new List<GameData.Wreckage>();
			foreach (var group in new List<GameData.Wreckage>(tethysGame.wreckage).GroupBy(w => w.id))
			{
				List<GameData.Wreckage> groupWreckage = group.ToList();
				if (groupWreckage[0].id <= 0)
					wreckages.AddRange(groupWreckage);
				else
				{
					// Get a random wreckage for this group ID
					int randomIndex = TethysGame.GetRandomRange(0, groupWreckage.Count);
					wreckages.Add(groupWreckage[randomIndex]);
				}
			}

			// Create wreckage
			foreach (GameData.Wreckage wreck in tethysGame.wreckage)
			{
				LOCATION spawnPt = TethysGame.GetMapCoordinates(wreck.position.ToLocation());

				TethysGame.CreateWreck(spawnPt.x, spawnPt.y, wreck.GetTechID(), wreck.isVisible);
			}

			// Setup Players
			foreach (PlayerData data in missionVariant.players)
			{
				Player player = TethysGame.GetPlayer(data.id);
				PlayerData.ResourceData resourceData = data.resources;
				
				// Process resources
				player.SetTechLevel(resourceData.techLevel);

				switch ((MoraleLevel)resourceData.GetMoraleLevel())
				{
					case MoraleLevel.Excellent:		TethysGame.ForceMoraleGreat(data.id);	TethysGame.ForceMoraleGreat(data.id);	break;
					case MoraleLevel.Good:			TethysGame.ForceMoraleGood(data.id);	TethysGame.ForceMoraleGood(data.id);	break;
					case MoraleLevel.Fair:			TethysGame.ForceMoraleOK(data.id);		TethysGame.ForceMoraleOK(data.id);		break;
					case MoraleLevel.Poor:			TethysGame.ForceMoralePoor(data.id);	TethysGame.ForceMoralePoor(data.id);	break;
					case MoraleLevel.Terrible:		TethysGame.ForceMoraleRotten(data.id);	TethysGame.ForceMoraleRotten(data.id);	break;
				}

				if ((TethysGame.UsesMorale() || !isMultiplayer) && resourceData.freeMorale)
					TethysGame.FreeMoraleLevel(data.id);

				// Only set player colony type and color if playing single player
				if (!data.isHuman || !isMultiplayer)
				{
					if (data.isEden)
						player.GoEden();
					else
						player.GoPlymouth();

					player.SetColorNumber(data.GetColor());
				}

				if (data.isHuman)
					player.GoHuman();
				else
					player.GoAI();
				
				foreach (int allyID in data.allies)
					player.AllyWith(allyID);

				// Set camera position
				LOCATION centerView = TethysGame.GetMapCoordinates(resourceData.centerView.ToLocation());
				player.CenterViewOn(centerView.x, centerView.y);

				// Set population
				player.SetKids(resourceData.kids);
				player.SetWorkers(resourceData.workers);
				player.SetScientists(resourceData.scientists);
				player.SetOre(resourceData.commonOre);
				player.SetRareOre(resourceData.rareOre);
				player.SetFoodStored(resourceData.food);
				player.SetSolarSat(resourceData.solarSatellites);

				// Set completed research
				foreach (int techID in resourceData.completedResearch)
					player.MarkResearchComplete(techID);

				// Select units
				List<UnitData> units = new List<UnitData>();
				foreach (var group in new List<UnitData>(resourceData.units).GroupBy(u => u.id))
				{
					List<UnitData> groupUnits = group.ToList();
					if (groupUnits[0].id <= 0)
						units.AddRange(groupUnits);
					else
					{
						// Get a random unit for this group ID
						int randomIndex = TethysGame.GetRandomRange(0, groupUnits.Count);
						units.Add(groupUnits[randomIndex]);
					}
				}

				// Create units
				foreach (UnitData unitData in units)
				{
					LOCATION spawnPt = TethysGame.GetMapCoordinates(unitData.position.ToLocation());

					Unit unit = unitData.CreateUnit(data.id, spawnPt);
					SetUnitID(data.id, unit.GetStubIndex());
					createdUnits.Add(unit);
				}

				// Create walls and tubes
				foreach (WallTubeData wallTube in resourceData.wallTubes)
				{
					LOCATION location = TethysGame.GetMapCoordinates(wallTube.position.ToLocation());
					TethysGame.CreateWallOrTube(location.x, location.y, 0, wallTube.GetTypeID());
				}
			}

			// Setup Autolayout bases
			BaseGenerator baseGenerator = new BaseGenerator(createdUnits);

			foreach (AutoLayout layout in missionVariant.layouts)
			{
				// Select units
				List<UnitData> units = new List<UnitData>();
				foreach (var group in new List<UnitData>(layout.units).GroupBy(u => u.id))
				{
					List<UnitData> groupUnits = group.ToList();
					if (groupUnits[0].id <= 0)
						units.AddRange(groupUnits);
					else
					{
						// Get a random unit for this group ID
						int randomIndex = TethysGame.GetRandomRange(0, groupUnits.Count);
						units.Add(groupUnits[randomIndex]);
					}
				}

				// Generate autolayout base
				baseGenerator.Generate(TethysGame.GetPlayer(layout.playerID), layout.baseCenterPt.ToLocation(), units.ToArray());
			}

			// Setup Disasters
			InitializeDisasters();

			// Setup Triggers
			List<OP2TriggerData> triggers = new List<OP2TriggerData>(root.triggers);
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
					OP2TriggerData data = triggers[i];

					// Get parent trigger if there is one
					TriggerStub parentTrigger = null;
					if (data.triggerID > 0)
						triggerLookup.TryGetValue(data.triggerID, out parentTrigger);

					bool wasProcessed = true;

					TriggerStub trigger = null;

					switch (data.GetTriggerType())
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
							trigger = TriggerStub.CreateOperationalTrigger(data.id, data.enabled, data.oneShot, data.playerID, data.GetUnitType(), data.count, data.GetCompareType());
							break;

						case TriggerType.Research:
							trigger = TriggerStub.CreateResearchTrigger(data.id, data.enabled, data.oneShot, data.techID, data.playerID);
							break;

						case TriggerType.Resource:
							trigger = TriggerStub.CreateResourceTrigger(data.id, data.enabled, data.oneShot, data.GetResourceType(), data.count, data.playerID, data.GetCompareType());
							break;

						case TriggerType.Kit:
							trigger = TriggerStub.CreateKitTrigger(data.id, data.enabled, data.oneShot, data.playerID, data.GetUnitType(), data.count);
							break;

						case TriggerType.Escape:
							trigger = TriggerStub.CreateEscapeTrigger(data.id, data.enabled, data.oneShot, data.playerID, data.x, data.y, data.width, data.height, data.count, data.GetUnitType(), data.GetCargoType(), data.cargoAmount);
							break;

						case TriggerType.Count:
							trigger = TriggerStub.CreateCountTrigger(data.id, data.enabled, data.oneShot, data.playerID, data.GetUnitType(), data.GetCargoOrWeaponType(), data.count, data.GetCompareType());
							break;

						case TriggerType.VehicleCount:
							trigger = TriggerStub.CreateVehicleCountTrigger(data.id, data.enabled, data.oneShot, data.playerID, data.count, data.GetCompareType());
							break;

						case TriggerType.BuildingCount:
							trigger = TriggerStub.CreateBuildingCountTrigger(data.id, data.enabled, data.oneShot, data.playerID, data.count, data.GetCompareType());
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
							Console.WriteLine("Invalid Trigger Type: " + data.GetTriggerType());
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

			m_EventSystem.NewMission(missionVariant);

			StartMission();

			return true;
		}

		private void SetUnitID(int triggerUnitID, int stubIndex)
		{
			if (triggerUnitID < 0) return;
			if (triggerUnitID >= m_SaveData.eventData.unitIDs.Length)
			{
				Console.WriteLine("Unit ID out of range: " + triggerUnitID);
				return;
			}

			m_SaveData.eventData.unitIDs[triggerUnitID] = stubIndex;
		}

		/// <summary>
		/// Called when a mission is loaded from a saved game. Performs reinitialization of data lost during quit.
		/// </summary>
		public virtual void LoadMission()
		{
			Console.WriteLine("Mission loaded.");

			InitializeDisasters();
			CreateTriggerDataLookupTable();

			MissionVariant missionVariant = GetCombinedDifficultyVariant(m_Root);

			m_EventSystem.LoadMission(missionVariant);

			StartMission();
		}

		private void InitializeDisasters()
		{
			MissionRoot root = m_Root;

			m_Disasters.Clear();

			// Setup Disasters
			if (TethysGame.CanHaveDisasters() || root.levelDetails.GetMissionType() == MissionType.Colony)
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

			foreach (OP2TriggerData data in m_Root.triggers)
				m_TriggerData.Add(data.id, data);
		}

		private MissionVariant GetCombinedDifficultyVariant(MissionRoot root)
		{
			// Combine master variant with selected variant. The master variant is always used as a base.
			MissionVariant missionVariant = root.masterVariant;
			if (root.missionVariants.Count > 0)
				missionVariant = MissionVariant.Concat(root.masterVariant, root.missionVariants[m_SaveData.missionVariantIndex]);

			// Startup Flags
			bool isMultiplayer = (int)root.levelDetails.GetMissionType() <= -4 && (int)root.levelDetails.GetMissionType() >= -8;
			int localDifficulty = TethysGame.GetPlayer(TethysGame.LocalPlayer()).Difficulty();

			// Combine master gaia resources with difficulty gaia resources
			if (!isMultiplayer && localDifficulty < missionVariant.tethysDifficulties.Count)
				missionVariant.tethysGame = GameData.Concat(missionVariant.tethysGame, missionVariant.tethysDifficulties[localDifficulty]);

			foreach (PlayerData data in missionVariant.players)
			{
				Player player = TethysGame.GetPlayer(data.id);

				// Get difficulty
				int difficulty = player.Difficulty();

				// If playing single player, all players get assigned the local player's difficulty
				if (!isMultiplayer && data.id != TethysGame.LocalPlayer())
					difficulty = localDifficulty;

				// Add difficulty resources
				if (difficulty < data.difficulties.Count)
					data.resources = PlayerData.ResourceData.Concat(data.resources, data.difficulties[difficulty]);
			}

			return missionVariant;
		}

		/// <summary>
		/// Called when the mission has finished initializing, regardless of whether it is a new game or saved game.
		/// </summary>
		protected virtual void StartMission()
		{
			MissionVariant missionVariant = GetCombinedDifficultyVariant(m_Root);

			// Initialize bots
			for (int i=0; i < missionVariant.players.Count; ++i)
			{
				if (missionVariant.players[i].GetBotType() == BotType.None)
					continue;

				m_BotPlayer[i] = new BotPlayer(missionVariant.players[i].GetBotType(), i);
				m_BotPlayer[i].Start();
			}

			// Start event system
			m_EventSystem.StartMission(StateSnapshot.Create(), m_BotPlayer);
		}

		/// <summary>
		/// Called when a trigger has been executed.
		/// </summary>
		/// <param name="trigger">The trigger that was executed.</param>
		protected virtual void OnTriggerExecuted(TriggerStub trigger)
		{
			OP2TriggerData data;
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
				case DisasterType.Meteor:		TethysGame.SetMeteor(spawnPt.x, spawnPt.y, disaster.size);								break;
				case DisasterType.Earthquake:	TethysGame.SetEarthquake(spawnPt.x, spawnPt.y, disaster.size);							break;
				case DisasterType.Lightning:	TethysGame.SetLightning(spawnPt.x, spawnPt.y, disaster.duration, destPt.x, destPt.y);	break;
				case DisasterType.Tornado:		TethysGame.SetTornado(spawnPt.x, spawnPt.y, disaster.duration, destPt.x, destPt.y, 0);	break;
				case DisasterType.Eruption:		TethysGame.SetEruption(spawnPt.x, spawnPt.y, disaster.duration);						break;
				case DisasterType.Blight:
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
