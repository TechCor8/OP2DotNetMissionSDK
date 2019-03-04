using DotNetMissionSDK.Json;
using System.Collections.Generic;

namespace DotNetMissionSDK
{
	public class MissionLogic
	{
		private List<Unit> m_Markers = new List<Unit>();

		private List<DisasterData> m_Disasters = new List<DisasterData>();


		public bool Initialize(MissionRoot root)
		{
			// Setup Game
			TethysGame.SetDaylightEverywhere(root.tethysGame.daylightEverywhere);
			TethysGame.SetDaylightMoves(root.tethysGame.daylightMoves);
			GameMap.SetInitialLightLevel(root.tethysGame.initialLightLevel);
			TethysGame.SetMusicPlayList(root.tethysGame.musicPlayList.songIDs.Length, root.tethysGame.musicPlayList.repeatStartIndex, root.tethysGame.musicPlayList.songIDs);

			// Beacons
			foreach (GameData.Beacon beacon in root.tethysGame.beacons)
			{
				LOCATION spawnPt = TethysGame.GetRandomLocation(new MAP_RECT(beacon.spawnRect));
				spawnPt = TethysGame.GetMapCoordinates(spawnPt);

				TethysGame.CreateBeacon(beacon.mapID, spawnPt.x, spawnPt.y, beacon.commonRareType, beacon.barYield, beacon.barVariant);
			}

			// Markers
			foreach (GameData.Marker marker in root.tethysGame.markers)
			{
				LOCATION spawnPt = TethysGame.GetRandomLocation(new MAP_RECT(marker.spawnRect));
				spawnPt = TethysGame.GetMapCoordinates(spawnPt);

				Unit unit;
				TethysGame.PlaceMarker(out unit, spawnPt.x, spawnPt.y, marker.markerType);
				m_Markers.Add(unit);
			}

			// Wreckage
			foreach (GameData.Wreckage wreck in root.tethysGame.wreckage)
			{
				LOCATION spawnPt = TethysGame.GetRandomLocation(new MAP_RECT(wreck.spawnRect));
				spawnPt = TethysGame.GetMapCoordinates(spawnPt);

				TethysGame.CreateWreck(spawnPt.x, spawnPt.y, wreck.techID, wreck.isVisible);
			}

			// Tubes
			foreach (GameData.WallTube wallTube in root.tethysGame.wallTubes)
			{
				LOCATION location = TethysGame.GetMapCoordinates(new LOCATION(wallTube.location));
				TethysGame.CreateWallOrTube(location.x, location.y, 0, wallTube.typeID);
			}

			// Setup Players
			foreach (PlayerData data in root.players)
			{
				Player player = TethysGame.GetPlayer(data.id);

				switch ((MoraleLevels)data.moraleLevel)
				{
					case MoraleLevels.moraleGreat:		TethysGame.ForceMoraleGreat(data.id);		break;
					case MoraleLevels.moraleGood:		TethysGame.ForceMoraleGood(data.id);		break;
					case MoraleLevels.moraleOK:			TethysGame.ForceMoraleOK(data.id);			break;
					case MoraleLevels.moralePoor:		TethysGame.ForceMoralePoor(data.id);		break;
					case MoraleLevels.moraleRotten:		TethysGame.ForceMoraleRotten(data.id);		break;
				}

				if ((TethysGame.UsesMorale() || root.levelDetails.missionType == MissionTypes.Colony) && data.freeMorale)
					TethysGame.FreeMoraleLevel(data.id);

				if (data.isEden)
					player.GoEden();
				else
					player.GoPlymouth();

				// TODO: data.isHuman - If not human, use fancy AI code

				player.SetColorNumber(data.colorID);

				foreach (int allyID in data.allies)
					player.AllyWith(allyID);

				player.CenterViewOn(data.centerView.x, data.centerView.y);

				player.SetKids(data.kids);
				player.SetWorkers(data.workers);
				player.SetScientists(data.scientists);
				player.SetOre(data.commonOre);
				player.SetRareOre(data.rareOre);
				player.SetFoodStored(data.food);

				// Units
				foreach (UnitData unitData in data.units)
				{
					LOCATION spawnPt = TethysGame.GetMapCoordinates(new LOCATION(unitData.location));

					Unit unit;
					TethysGame.CreateUnit(out unit, unitData.typeID, spawnPt.x, spawnPt.y, data.id, unitData.cargoType, unitData.rotation);
					unit.Dispose();
				}
			}

			// Setup Disasters
			if (TethysGame.CanHaveDisasters() || root.levelDetails.missionType == MissionTypes.Colony)
			{
				foreach (DisasterData disaster in root.disasters)
				{
					disaster.timeUntilNextDisaster = disaster.startTime;
					m_Disasters.Add(disaster);
				}
			}

			// Setup Triggers
			//Trigger trig = Trigger.CreateOperationalTrigger(true, true, TethysGame.LocalPlayer(), map_id.mapCommandCenter, 0, compare_mode.cmpEqual, Trigger.EmptyFunction);
			//Trigger.CreateFailureCondition(true, trig);

			return true;
		}

		public void Update()
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
		}

		private void FireDisaster(DisasterData disaster)
		{
			LOCATION spawnPt = TethysGame.GetMapCoordinates(TethysGame.GetRandomLocation(new MAP_RECT(disaster.srcRect)));
			LOCATION destPt = TethysGame.GetMapCoordinates(TethysGame.GetRandomLocation(new MAP_RECT(disaster.destRect)));

			/*using (System.IO.FileStream fs = new System.IO.FileStream("DotNetLog.txt", System.IO.FileMode.Append))
			using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fs))
			{
				writer.WriteLine();
				writer.WriteLine("Disaster: " + disaster.type);
				writer.WriteLine("Current Time: " + TethysGame.Time());
				writer.WriteLine("Disaster Time: " + disaster.timeUntilNextDisaster);
				writer.WriteLine("End Time: " + disaster.endTime);
			}*/
			
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

		// Unused: Releases all mission resources
		private void Dispose()
		{
			// Dispose markers
			foreach (Unit marker in m_Markers)
				marker.Dispose();

			m_Markers.Clear();
		}
	}
}
