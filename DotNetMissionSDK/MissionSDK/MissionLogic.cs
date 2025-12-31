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
			bool isMultiplayer = (int)root.LevelDetails.GetMissionType() <= -4 && (int)root.LevelDetails.GetMissionType() >= -8;
			int localDifficulty = TethysGame.GetPlayer(TethysGame.LocalPlayer()).Difficulty();

			// Select mission variant (random)
			m_SaveData.missionVariantIndex = (byte)TethysGame.GetRandomRange(0, root.MissionVariants.Count);

			// Combine master variant with selected variant. The master variant is always used as a base.
			MissionVariant missionVariant = GetCombinedDifficultyVariant(root);
			GameData tethysGame = missionVariant.TethysGame;

			// Setup Game
			TethysGame.SetDaylightEverywhere(tethysGame.DaylightEverywhere);
			TethysGame.SetDaylightMoves(tethysGame.DaylightMoves);
			GameMap.SetInitialLightLevel(tethysGame.InitialLightLevel);

			// If this is a multiplayer game, use the game-specified light settings
			if (isMultiplayer && !TethysGame.UsesDayNight())
				TethysGame.SetDaylightEverywhere(true);

			TethysGame.SetMusicPlayList(tethysGame.MusicPlayList.SongIds.Length, tethysGame.MusicPlayList.RepeatStartIndex, tethysGame.MusicPlayList.SongIds);

			// Select Beacons
			List<Beacon> beacons = new List<Beacon>();
			foreach (var group in new List<Beacon>(tethysGame.Beacons).GroupBy(b => b.Id))
			{
				List<Beacon> groupBeacons = group.ToList();
				if (groupBeacons[0].Id <= 0)
					beacons.AddRange(groupBeacons);
				else
				{
					// Get a random beacon for this group ID
					int randomIndex = TethysGame.GetRandomRange(0, groupBeacons.Count);
					beacons.Add(groupBeacons[randomIndex]);
				}
			}

			// Create beacons
			foreach (Beacon beacon in beacons)
			{
				LOCATION spawnPt = TethysGame.GetMapCoordinates(beacon.Position.ToLocation());

				int stubIndex = TethysGame.CreateBeacon(beacon.GetMapID(), spawnPt.x, spawnPt.y, beacon.GetOreType(), beacon.GetBarYield(), beacon.GetBarVariant());
				SetUnitID(beacon.Id, stubIndex);
			}

			// Select markers
			List<Marker> markers = new List<Marker>();
			foreach (var group in new List<Marker>(tethysGame.Markers).GroupBy(m => m.Id))
			{
				List<Marker> groupMarkers = group.ToList();
				if (groupMarkers[0].Id <= 0)
					markers.AddRange(groupMarkers);
				else
				{
					// Get a random marker for this group ID
					int randomIndex = TethysGame.GetRandomRange(0, groupMarkers.Count);
					markers.Add(groupMarkers[randomIndex]);
				}
			}

			// Create markers
			foreach (Marker marker in markers)
			{
				LOCATION spawnPt = TethysGame.GetMapCoordinates(marker.Position.ToLocation());

				Unit unit = TethysGame.PlaceMarker(spawnPt.x, spawnPt.y, marker.GetMarkerType());
				SetUnitID(marker.Id, unit.GetStubIndex());
			}

			// Select wreckage
			List<Wreckage> wreckages = new List<Wreckage>();
			foreach (var group in new List<Wreckage>(tethysGame.Wreckage).GroupBy(w => w.Id))
			{
				List<Wreckage> groupWreckage = group.ToList();
				if (groupWreckage[0].Id <= 0)
					wreckages.AddRange(groupWreckage);
				else
				{
					// Get a random wreckage for this group ID
					int randomIndex = TethysGame.GetRandomRange(0, groupWreckage.Count);
					wreckages.Add(groupWreckage[randomIndex]);
				}
			}

			// Create wreckage
			foreach (Wreckage wreck in tethysGame.Wreckage)
			{
				LOCATION spawnPt = TethysGame.GetMapCoordinates(wreck.Position.ToLocation());

				TethysGame.CreateWreck(spawnPt.x, spawnPt.y, wreck.GetTechID(), wreck.IsVisible);
			}

			// Setup Players
			foreach (PlayerData data in missionVariant.Players)
			{
				Player player = TethysGame.GetPlayer(data.Id);
				PlayerData.ResourceData resourceData = data.Resources;
				
				// Process resources
				player.SetTechLevel(resourceData.TechLevel);

				switch ((MoraleLevel)resourceData.GetMoraleLevel())
				{
					case MoraleLevel.Excellent:		TethysGame.ForceMoraleGreat(data.Id);	TethysGame.ForceMoraleGreat(data.Id);	break;
					case MoraleLevel.Good:			TethysGame.ForceMoraleGood(data.Id);	TethysGame.ForceMoraleGood(data.Id);	break;
					case MoraleLevel.Fair:			TethysGame.ForceMoraleOK(data.Id);		TethysGame.ForceMoraleOK(data.Id);		break;
					case MoraleLevel.Poor:			TethysGame.ForceMoralePoor(data.Id);	TethysGame.ForceMoralePoor(data.Id);	break;
					case MoraleLevel.Terrible:		TethysGame.ForceMoraleRotten(data.Id);	TethysGame.ForceMoraleRotten(data.Id);	break;
				}

				if ((TethysGame.UsesMorale() || !isMultiplayer) && resourceData.FreeMorale)
					TethysGame.FreeMoraleLevel(data.Id);

				// Only set player colony type and color if playing single player
				if (!data.IsHuman || !isMultiplayer)
				{
					if (data.IsEden)
						player.GoEden();
					else
						player.GoPlymouth();

					player.SetColorNumber(data.GetColor());
				}

				if (data.IsHuman)
					player.GoHuman();
				else
					player.GoAI();
				
				foreach (int allyID in data.Allies)
					player.AllyWith(allyID);

				// Set camera position
				LOCATION centerView = TethysGame.GetMapCoordinates(resourceData.CenterView.ToLocation());
				player.CenterViewOn(centerView.x, centerView.y);

				// Set population
				player.SetKids(resourceData.Kids);
				player.SetWorkers(resourceData.Workers);
				player.SetScientists(resourceData.Scientists);
				player.SetOre(resourceData.CommonOre);
				player.SetRareOre(resourceData.RareOre);
				player.SetFoodStored(resourceData.Food);
				player.SetSolarSat(resourceData.SolarSatellites);

				// Set completed research
				foreach (int techID in resourceData.CompletedResearch)
					player.MarkResearchComplete(techID);

				// Select units
				List<UnitData> units = new List<UnitData>();
				foreach (var group in new List<UnitData>(resourceData.Units).GroupBy(u => u.Id))
				{
					List<UnitData> groupUnits = group.ToList();
					if (groupUnits[0].Id <= 0)
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
					LOCATION spawnPt = TethysGame.GetMapCoordinates(unitData.Position.ToLocation());

					Unit unit = unitData.CreateUnit(data.Id, spawnPt);
					SetUnitID(data.Id, unit.GetStubIndex());
					createdUnits.Add(unit);
				}

				// Create walls and tubes
				foreach (WallTubeData wallTube in resourceData.WallTubes)
				{
					LOCATION location = TethysGame.GetMapCoordinates(wallTube.Position.ToLocation());
					TethysGame.CreateWallOrTube(location.x, location.y, 0, wallTube.GetTypeID());
				}
			}

			// Setup Autolayout bases
			BaseGenerator baseGenerator = new BaseGenerator(createdUnits);

			foreach (AutoLayout layout in missionVariant.Layouts)
			{
				// Select units
				List<UnitData> units = new List<UnitData>();
				foreach (var group in new List<UnitData>(layout.Units).GroupBy(u => u.Id))
				{
					List<UnitData> groupUnits = group.ToList();
					if (groupUnits[0].Id <= 0)
						units.AddRange(groupUnits);
					else
					{
						// Get a random unit for this group ID
						int randomIndex = TethysGame.GetRandomRange(0, groupUnits.Count);
						units.Add(groupUnits[randomIndex]);
					}
				}

				// Generate autolayout base
				baseGenerator.Generate(TethysGame.GetPlayer(layout.PlayerId), layout.BaseCenterPt.ToLocation(), units.ToArray());
			}

			// Setup Disasters
			InitializeDisasters();

			// Setup Triggers
			List<OP2TriggerData> triggers = new List<OP2TriggerData>(root.Triggers);
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
					if (data.TriggerId > 0)
						triggerLookup.TryGetValue(data.TriggerId, out parentTrigger);

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
							
							trigger = TriggerStub.CreateVictoryCondition(data.Id, data.Enabled, parentTrigger, data.Message);
							break;

						case TriggerType.Failure:
							if (parentTrigger == null)
							{
								wasProcessed = false;
								break;
							}

							trigger = TriggerStub.CreateFailureCondition(data.Id, data.Enabled, parentTrigger);
							break;

						case TriggerType.OnePlayerLeft:
							trigger = TriggerStub.CreateOnePlayerLeftTrigger(data.Id, data.Enabled, data.OneShot);
							break;

						case TriggerType.Evac:
							trigger = TriggerStub.CreateEvacTrigger(data.Id, data.Enabled, data.OneShot, data.PlayerId);
							break;

						case TriggerType.Midas:
							trigger = TriggerStub.CreateMidasTrigger(data.Id, data.Enabled, data.OneShot, data.Time);
							break;

						case TriggerType.Operational:
							trigger = TriggerStub.CreateOperationalTrigger(data.Id, data.Enabled, data.OneShot, data.PlayerId, data.GetUnitType(), data.Count, data.GetCompareType());
							break;

						case TriggerType.Research:
							trigger = TriggerStub.CreateResearchTrigger(data.Id, data.Enabled, data.OneShot, data.TechId, data.PlayerId);
							break;

						case TriggerType.Resource:
							trigger = TriggerStub.CreateResourceTrigger(data.Id, data.Enabled, data.OneShot, data.GetResourceType(), data.Count, data.PlayerId, data.GetCompareType());
							break;

						case TriggerType.Kit:
							trigger = TriggerStub.CreateKitTrigger(data.Id, data.Enabled, data.OneShot, data.PlayerId, data.GetUnitType(), data.Count);
							break;

						case TriggerType.Escape:
							trigger = TriggerStub.CreateEscapeTrigger(data.Id, data.Enabled, data.OneShot, data.PlayerId, data.X, data.Y, data.Width, data.Height, data.Count, data.GetUnitType(), data.GetCargoType(), data.cargoAmount);
							break;

						case TriggerType.Count:
							trigger = TriggerStub.CreateCountTrigger(data.Id, data.Enabled, data.OneShot, data.PlayerId, data.GetUnitType(), data.GetCargoOrWeaponType(), data.Count, data.GetCompareType());
							break;

						case TriggerType.VehicleCount:
							trigger = TriggerStub.CreateVehicleCountTrigger(data.Id, data.Enabled, data.OneShot, data.PlayerId, data.Count, data.GetCompareType());
							break;

						case TriggerType.BuildingCount:
							trigger = TriggerStub.CreateBuildingCountTrigger(data.Id, data.Enabled, data.OneShot, data.PlayerId, data.Count, data.GetCompareType());
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
							trigger = TriggerStub.CreateTimeTrigger(data.Id, data.Enabled, data.OneShot, data.Time);
							break;

						case TriggerType.TimeRange:
							trigger = TriggerStub.CreateTimeTrigger(data.Id, data.Enabled, data.OneShot, data.MinTime, data.MaxTime);
							break;

						case TriggerType.Point:
							trigger = TriggerStub.CreatePointTrigger(data.Id, data.Enabled, data.OneShot, data.PlayerId, data.X, data.Y);
							break;

						case TriggerType.Rect:
							trigger = TriggerStub.CreateRectTrigger(data.Id, data.Enabled, data.OneShot, data.PlayerId, data.X, data.Y, data.Width, data.Height);
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
							triggerLookup.Add(data.Id, trigger);
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
			if (TethysGame.CanHaveDisasters() || root.LevelDetails.GetMissionType() == MissionType.Colony)
			{
				foreach (DisasterData disaster in root.Disasters)
				{
					disaster.timeUntilNextDisaster = disaster.StartTime;
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

			foreach (OP2TriggerData data in m_Root.Triggers)
				m_TriggerData.Add(data.Id, data);
		}

		private MissionVariant GetCombinedDifficultyVariant(MissionRoot root)
		{
			// Combine master variant with selected variant. The master variant is always used as a base.
			MissionVariant missionVariant = root.MasterVariant;
			if (root.MissionVariants.Count > 0)
				missionVariant = MissionVariant.Concat(root.MasterVariant, root.MissionVariants[m_SaveData.missionVariantIndex]);

			// Startup Flags
			bool isMultiplayer = (int)root.LevelDetails.GetMissionType() <= -4 && (int)root.LevelDetails.GetMissionType() >= -8;
			int localDifficulty = TethysGame.GetPlayer(TethysGame.LocalPlayer()).Difficulty();

			// Combine master gaia resources with difficulty gaia resources
			if (!isMultiplayer && localDifficulty < missionVariant.TethysDifficulties.Count)
				missionVariant.TethysGame = GameData.Concat(missionVariant.TethysGame, missionVariant.TethysDifficulties[localDifficulty]);

			foreach (PlayerData data in missionVariant.Players)
			{
				Player player = TethysGame.GetPlayer(data.Id);

				// Get difficulty
				int difficulty = player.Difficulty();

				// If playing single player, all players get assigned the local player's difficulty
				if (!isMultiplayer && data.Id != TethysGame.LocalPlayer())
					difficulty = localDifficulty;

				// Add difficulty resources
				if (difficulty < data.Difficulties.Count)
					data.Resources = PlayerData.ResourceData.Concat(data.Resources, data.Difficulties[difficulty]);
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
			for (int i=0; i < missionVariant.Players.Count; ++i)
			{
				if (missionVariant.Players[i].GetBotType() == BotType.None)
					continue;

				m_BotPlayer[i] = new BotPlayer(missionVariant.Players[i].GetBotType(), i);
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
			foreach (ActionData action in data.Actions)
				ExecuteAction(action);
		}

		private void ExecuteAction(ActionData action)
		{
			switch (action.Type)
			{
				case "AddMessage":
					TethysGame.AddMessage(0, 0, action.Message, action.PlayerId, action.SoundId);
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
				if (currentTime > disaster.EndTime)
					continue;

				if (currentTime >= disaster.timeUntilNextDisaster)
				{
					disaster.timeUntilNextDisaster += TethysGame.GetRandomRange(disaster.MinDelay, disaster.MaxDelay);

					FireDisaster(disaster);
				}
			}

			// Update bots
			for (int i=0; i < m_BotPlayer.Length; ++i)
				m_BotPlayer[i]?.Update(stateSnapshot);
		}

		private void FireDisaster(DisasterData disaster)
		{
			LOCATION spawnPt = TethysGame.GetMapCoordinates(disaster.SrcRect.GetRandomPointInRect());
			LOCATION destPt = TethysGame.GetMapCoordinates(disaster.DestRect.GetRandomPointInRect());

			switch (disaster.Type)
			{
				case DisasterType.Meteor:		TethysGame.SetMeteor(spawnPt.x, spawnPt.y, disaster.Size);								break;
				case DisasterType.Earthquake:	TethysGame.SetEarthquake(spawnPt.x, spawnPt.y, disaster.Size);							break;
				case DisasterType.Lightning:	TethysGame.SetLightning(spawnPt.x, spawnPt.y, disaster.Duration, destPt.x, destPt.y);	break;
				case DisasterType.Tornado:		TethysGame.SetTornado(spawnPt.x, spawnPt.y, disaster.Duration, destPt.x, destPt.y, 0);	break;
				case DisasterType.Eruption:		TethysGame.SetEruption(spawnPt.x, spawnPt.y, disaster.Duration);						break;
				case DisasterType.Blight:
					GameMap.SetVirusUL(spawnPt.x, spawnPt.y, true);
					TethysGame.SetMicrobeSpreadSpeed(disaster.Duration);
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
