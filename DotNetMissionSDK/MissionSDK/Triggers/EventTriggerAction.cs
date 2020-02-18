using DotNetMissionSDK.AI;
using DotNetMissionSDK.Json;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetMissionSDK.Triggers
{
	public enum EventTriggerActionResult
	{
		Complete,			// Action has finished executing
		Wait,				// Action wants to wait for the next update
		PreserveTrigger,	// Action wants to preserve the trigger
		Failure,			// Action failed to execute
	}

	public class EventTriggerAction
	{
		private EventSystemData m_EventData;
		private TriggerActionData m_ActionData;
		private int m_OwnerID;

		private TriggerActionType type						{ get; set; }
		private TriggerModifier modifier					{ get; set; } // Set, add, subtract
		private int subject									{ get; set; } // TriggerPlayerCategory, TriggerUnitCategory, Switch#
		private int subject2								{ get; set; } // map_id weapon
		private int subjectPlayer							{ get; set; } // TriggerPlayerCategory
		private int subjectRegion							{ get; set; } // Region
		private int subjectQuantity							{ get; set; }
		
		private int value									{ get; set; }
		private int value2									{ get; set; }
		private int value3									{ get; set; }
		private int value4									{ get; set; }
		private int value5									{ get; set; }

		private TriggerValueType subjectType				{ get; set; }

		/*private TriggerValueType valueType					{ get; set; }
		private TriggerValueType value2Type					{ get; set; }
		private TriggerValueType value3Type					{ get; set; }
		private TriggerValueType value4Type					{ get; set; }
		private TriggerValueType value5Type					{ get; set; }*/


		public EventTriggerAction(EventSystemData eventData, TriggerActionData actionData, int ownerID)
		{
			m_EventData = eventData;
			m_ActionData = actionData;
			m_OwnerID = ownerID;

			type = actionData.type;
			modifier = actionData.modifier;
			subject = TriggerValueTypeUtility.GetStringAsInt(actionData.subject, actionData.GetSubjectType());
			subject2 = TriggerValueTypeUtility.GetStringAsInt(actionData.subject2, actionData.GetSubject2Type());
			subjectPlayer = actionData.GetSubjectPlayer();
			subjectRegion = actionData.GetSubjectRegion();
			subjectQuantity = actionData.subjectQuantity;

			value = TriggerValueTypeUtility.GetStringAsInt(actionData.value, actionData.GetValueType());
			value2 = TriggerValueTypeUtility.GetStringAsInt(actionData.value2, actionData.GetValue2Type());
			value3 = TriggerValueTypeUtility.GetStringAsInt(actionData.value3, actionData.GetValue3Type());
			value4 = TriggerValueTypeUtility.GetStringAsInt(actionData.value4, actionData.GetValue4Type());
			value5 = TriggerValueTypeUtility.GetStringAsInt(actionData.value5, actionData.GetValue5Type());

			subjectType = actionData.GetSubjectType();
		}

		public EventTriggerActionResult Execute(EventSystem eventSystem, StateSnapshot stateSnapshot, BotPlayer[] botPlayers, int executionTick, int currentTick,
												PlayerState eventPlayer, UnitState eventUnit, int eventRegionIndex, int eventTopic)
		{
			switch (type)
			{
				case TriggerActionType.WaitTicks:                                   // Wait [value] Game Ticks
				{
					if (currentTick - executionTick <= value)
						return EventTriggerActionResult.Wait;

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.PreserveTrigger:								// Preserve Trigger
				{
					return EventTriggerActionResult.PreserveTrigger;
				}

				case TriggerActionType.MoveRegionToUnit:							// Move [region] to [CurrentUnit/UnitID]
				{
					if (!IsValidRegion(subjectRegion))
						return EventTriggerActionResult.Failure;

					// Get the unit to move to
					UnitState unit = GetUnit(stateSnapshot, eventUnit, value);
					if (unit == null)
						return EventTriggerActionResult.Failure;

					// Get region and move it to the unit position
					LOCATION region = GetRegionCenterPt(m_EventData.regions[subjectRegion]);
					MoveRegion(m_EventData.regions[subjectRegion], unit.position.x - region.x, unit.position.y - region.y);

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.MoveRegionToPosition:						// Move [region] to [x],[y]
				{
					if (!IsValidRegion(subjectRegion))
						return EventTriggerActionResult.Failure;

					// Get region and move it to the position
					LOCATION region = GetRegionCenterPt(m_EventData.regions[subjectRegion]);
					
					switch (modifier)
					{
						case TriggerModifier.Set:		MoveRegion(m_EventData.regions[subjectRegion], value - region.x, value2 - region.y);	break;
						case TriggerModifier.Add:		MoveRegion(m_EventData.regions[subjectRegion], value, value2);							break;
						case TriggerModifier.Subtract:	MoveRegion(m_EventData.regions[subjectRegion], -value, -value2);						break;
					}

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.MoveRegionToRegion:                          // Move [region] to [region]
				{
					if (!IsValidRegion(subjectRegion))
						return EventTriggerActionResult.Failure;

					if (!IsValidRegion(value))
						return EventTriggerActionResult.Failure;

					// Get region and move it to the value region
					LOCATION region = GetRegionCenterPt(m_EventData.regions[subjectRegion]);
					LOCATION region2 = GetRegionCenterPt(m_EventData.regions[value]);
					MoveRegion(m_EventData.regions[subjectRegion], region2.x - region.x, region2.y - region.y);

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.MoveRegionToUnitTypeInRegion:				// Move [region] to [unit.map_id] with [weapon] owned by [Player] in [region]
				{
					if (!IsValidRegion(subjectRegion))
						return EventTriggerActionResult.Failure;

					if (!IsValidRegion(value2))
						return EventTriggerActionResult.Failure;

					// Get region and move it to the value region
					LOCATION region = GetRegionCenterPt(m_EventData.regions[subjectRegion]);
					List<UnitState> units = GetUnitsInRegion(stateSnapshot, 1, (map_id)value, (map_id)value2, GetPlayer(stateSnapshot, eventPlayer, value3), value4);
					if (units.Count > 0)
						MoveRegion(m_EventData.regions[subjectRegion], units[0].position.x - region.x, units[0].position.y - region.y);

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.SetRegionSize:                               // Set size of [region] to [x],[y]
				{
					if (!IsValidRegion(subjectRegion))
						return EventTriggerActionResult.Failure;

					// Get region and resize it
					MAP_RECT rect = GetMapRect(m_EventData.regions[subjectRegion]);
					LOCATION centerPt = GetRegionCenterPt(m_EventData.regions[subjectRegion]);
					rect.width = value;
					rect.height = value2;

					m_EventData.regions[subjectRegion].xMin = rect.xMin;
					m_EventData.regions[subjectRegion].yMin = rect.yMin;
					m_EventData.regions[subjectRegion].xMax = rect.xMax;
					m_EventData.regions[subjectRegion].yMax = rect.yMax;

					// Center the region
					LOCATION centerPt2 = GetRegionCenterPt(m_EventData.regions[subjectRegion]);
					MoveRegion(m_EventData.regions[subjectRegion], centerPt.x - centerPt2.x, centerPt.y - centerPt2.y);

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.SetSwitch:                                   // Set [Switch#] to [value]
				{
					switch (modifier)
					{
						case TriggerModifier.Set:		m_EventData.switches[subject] = value;		break;
						case TriggerModifier.Add:		m_EventData.switches[subject] += value;		break;
						case TriggerModifier.Subtract:	m_EventData.switches[subject] -= value;		break;
					}

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.SetPlayerName:                               // Set [CurrentPlayer/Player#] name to [value]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						GameState.players[player.playerID].SetPlayerName(m_ActionData.value);

					return EventTriggerActionResult.Complete;
				
				case TriggerActionType.SetPlayerColor:                              // Set [CurrentPlayer/Player#] color to [value]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						GameState.players[player.playerID].SetColorNumber((PlayerColor)value);

					return EventTriggerActionResult.Complete;
				
				case TriggerActionType.SetPlayerBotType:                            // Set bot type for [CurrentPlayer/Player#] to [value]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						botPlayers[player.playerID].botType = (BotType)value;

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetPlayerDefeated:                           // [CurrentPlayer/Player#] defeated
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						eventSystem.MarkPlayerDefeated(player);

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetPlayerRLVs:								// [Add] [value] RLVs to [All/CurrentPlayer/Player#]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						GameState.players[player.playerID].SetRLVCount(GetModifiedValue(player.rlvCount, modifier, value));

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetPlayerSolarSatellites:                    // [Add] [value] solar satellites to [All/CurrentPlayer/Player#]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						GameState.players[player.playerID].SetSolarSatelliteCount(GetModifiedValue(player.starship.solarSatelliteCount, modifier, value));

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetPlayerEDWARDSatellites:					// [Add] [value] EDWARD satellites to [All/CurrentPlayer/Player#]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						GameState.players[player.playerID].SetEDWARDSatelliteCount(GetModifiedValue(player.starship.EDWARDSatelliteCount, modifier, value));

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetPlayerKids:								// [Add] [value] kids to [All/CurrentPlayer/Player#]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						GameState.players[player.playerID].SetKids(GetModifiedValue(player.kids, modifier, value));

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetPlayerWorkers:							// [Add] [value] workers to [All/CurrentPlayer/Player#]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						GameState.players[player.playerID].SetWorkers(GetModifiedValue(player.workers, modifier, value));

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetPlayerScientists:							// [Add] [value] scientists to [All/CurrentPlayer/Player#]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						GameState.players[player.playerID].SetScientists(GetModifiedValue(player.scientists, modifier, value));

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetPlayerCommonMetal:						// [Add] [value] common metal to [All/CurrentPlayer/Player#]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						GameState.players[player.playerID].SetOre(GetModifiedValue(player.ore, modifier, value));

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetPlayerRareMetal:							// [Add] [value] rare metal to [All/CurrentPlayer/Player#]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						GameState.players[player.playerID].SetRareOre(GetModifiedValue(player.rareOre, modifier, value));

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetPlayerFood:								// [Add] [value] food to [All/CurrentPlayer/Player#]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						GameState.players[player.playerID].SetFoodStored(GetModifiedValue(player.foodStored, modifier, value));

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetPlayerTechLevel:							// Set [All/CurrentPlayer/Player#] tech level to [value]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						GameState.players[player.playerID].SetTechLevel(value);

					return EventTriggerActionResult.Complete;

				case TriggerActionType.StarvePlayerPopulation:						// Starve [value] people for [All/CurrentPlayer/Player#] skip morale update [boolean]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						GameState.players[player.playerID].Starve(value, value2 != 0);

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetPlayerResearchCompleted:					// Mark research topic [TopicID] complete for [All/CurrentPlayer/Player#]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						GameState.players[player.playerID].MarkResearchComplete(value);

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetPlayerAlliance:                           // Ally [CurrentPlayer/Player#] with [Player#]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
					{
						foreach (PlayerState playerAlly in GetPlayer(stateSnapshot, eventPlayer, value))
						{
							if (player.playerID == playerAlly.playerID)
								continue;

							GameState.players[player.playerID].AllyWith(playerAlly.playerID);
						}
					}

					return EventTriggerActionResult.Complete;

				case TriggerActionType.CapturePlayerRLV:                            // Capture RLV from [CurrentPlayer/Player#] and give to [CurrentPlayer/Player#]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
					{
						foreach (PlayerState otherPlayer in GetPlayer(stateSnapshot, eventPlayer, value))
						{
							if (player.playerID == otherPlayer.playerID)
								continue;

							GameState.players[player.playerID].CaptureRLV(otherPlayer.playerID);
						}
					}

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetPlayerCameraToRegion:						// Center [All/CurrentPlayer/Player#] view on [region]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
					{
						LOCATION centerPt = GetRegionCenterPt(GetRegionRect(value));
						GameState.players[player.playerID].CenterViewOn(centerPt.x, centerPt.y);
					}

					return EventTriggerActionResult.Complete;

				case TriggerActionType.ShowMessageToPlayerAtRegion:                 // Display message [text] for player [All/CurrentPlayer/Player#] with sound [value] at [anywhere/region]
				{
					LOCATION centerPt = GetRegionCenterPt(GetRegionRect(value3));

					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						TethysGame.AddMessage(centerPt.x, centerPt.y, m_ActionData.value, player.playerID, value2);

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.ShowMessageToPlayerForUnit:                  // Display message [text] for player [All/CurrentPlayer/Player#] with sound [value] for unit [CurrentUnit/UnitID]
				{
					UnitState unitState = GetUnit(stateSnapshot, eventUnit, value3);
					if (unitState == null)
						return EventTriggerActionResult.Failure;

					Unit unit = GameState.GetUnit(unitState.unitID);
					if (unit == null)
						return EventTriggerActionResult.Failure;

					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						TethysGame.AddMessage(unit, m_ActionData.value, player.playerID, value2);

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.ShowMessageToPlayerFromPlayer:               // Display message [text] for player [All/CurrentPlayer/Player#] with sound [value] from player [CurrentPlayer/Player#]
					foreach (PlayerState fromPlayer in GetPlayer(stateSnapshot, eventPlayer, value3))
					{
						foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
							TethysGame.AddMessage(fromPlayer.playerID, m_ActionData.value, player.playerID, value2);
					}

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetDaylightEverywhere:                       // Set daylight everywhere [boolean]
					TethysGame.SetDaylightEverywhere(value != 0);

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetDaylightMoves:							// Set daylight moves [boolean]
					TethysGame.SetDaylightMoves(value != 0);

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetPlayerMorale:                             // Set morale [Excellent/Good/Fair/Poor/Terrible] for [All/CurrentPlayer/Player#]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
					{
						switch ((MoraleLevel)value)
						{
							case MoraleLevel.Excellent:		TethysGame.ForceMoraleGreat(player.playerID);	break;
							case MoraleLevel.Good:			TethysGame.ForceMoraleGood(player.playerID);	break;
							case MoraleLevel.Fair:			TethysGame.ForceMoraleOK(player.playerID);		break;
							case MoraleLevel.Poor:			TethysGame.ForceMoralePoor(player.playerID);	break;
							case MoraleLevel.Terrible:		TethysGame.ForceMoraleRotten(player.playerID);	break;
						}
					}

					return EventTriggerActionResult.Complete;

				case TriggerActionType.FreePlayerMorale:							// Free morale for [All/CurrentPlayer/Player#]
					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						TethysGame.FreeMoraleLevel(player.playerID);

					return EventTriggerActionResult.Complete;
					
				case TriggerActionType.SetRandomSeed:								// Set random seed [value]
					TethysGame.SetSeed((uint)value);

					return EventTriggerActionResult.Complete;

				case TriggerActionType.CreateMeteor:                                // Create meteor with size [value] in [region]
				{
					MAP_RECT region = GetRegionRect(subjectRegion);
					LOCATION spawnPt = region.GetRandomPointInRect();
					TethysGame.SetMeteor(spawnPt.x, spawnPt.y, value);

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.CreateEarthquake:							// Create earthquake with magnitude [value] in [region]
				{
					MAP_RECT region = GetRegionRect(subjectRegion);
					LOCATION spawnPt = region.GetRandomPointInRect();
					TethysGame.SetEarthquake(spawnPt.x, spawnPt.y, value);

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.CreateStorm:									// Create storm with duration [value] in [region] to [region]
				{
					MAP_RECT region = GetRegionRect(subjectRegion);
					LOCATION spawnPt = region.GetRandomPointInRect();
					MAP_RECT destRegion = GetRegionRect(value2);
					LOCATION destPt = destRegion.GetRandomPointInRect();
					TethysGame.SetLightning(spawnPt.x, spawnPt.y, value, destPt.x, destPt.y);

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.CreateVortex:								// Create tornado with duration [value] in [region] to [region] immediately [boolean]
				{
					MAP_RECT region = GetRegionRect(subjectRegion);
					LOCATION spawnPt = region.GetRandomPointInRect();
					MAP_RECT destRegion = GetRegionRect(value2);
					LOCATION destPt = destRegion.GetRandomPointInRect();
					TethysGame.SetTornado(spawnPt.x, spawnPt.y, value, destPt.x, destPt.y, value3);

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.CreateVolcano:                               // Create volcano with spread speed [value] in [region]
				{
					MAP_RECT region = GetRegionRect(subjectRegion);
					LOCATION spawnPt = region.GetRandomPointInRect();
					TethysGame.SetEruption(spawnPt.x, spawnPt.y, value);

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.CreateMicrobe:								// Create microbe in [region] fill [boolean]
				{
					MAP_RECT region = GetRegionRect(subjectRegion);
					if (value != 0)
					{
						// Fill entire region
						for (int x=region.xMin; x < region.xMax; ++x)
						{
							for (int y=region.yMin; y < region.yMax; ++y)
								GameMap.SetVirusUL(x, y, true);
						}
					}
					else
					{
						// Random point in region
						LOCATION spawnPt = region.GetRandomPointInRect();
						GameMap.SetVirusUL(spawnPt.x, spawnPt.y, true);
					}

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.SetMicrobeSpreadSpeed:                       // Set microbe spread speed to [value]
					TethysGame.SetMicrobeSpreadSpeed(value);

					return EventTriggerActionResult.Complete;

				case TriggerActionType.RemoveMicrobe:								// Remove microbe in [region] fill [boolean]
				{
					MAP_RECT region = GetRegionRect(subjectRegion);
					if (value != 0)
					{
						// Fill entire region
						for (int x=region.xMin; x < region.xMax; ++x)
						{
							for (int y=region.yMin; y < region.yMax; ++y)
								GameMap.SetVirusUL(x, y, false);
						}
					}
					else
					{
						// Random point in region
						LOCATION spawnPt = region.GetRandomPointInRect();
						GameMap.SetVirusUL(spawnPt.x, spawnPt.y, false);
					}

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.FireMissileEMP:								// Fire EMP missile from [region] to [region] for [CurrentPlayer/Player#]
				{
					MAP_RECT region = GetRegionRect(subjectRegion);
					LOCATION spawnPt = region.GetRandomPointInRect();
					MAP_RECT destRegion = GetRegionRect(value2);
					LOCATION destPt = destRegion.GetRandomPointInRect();

					foreach (PlayerState player in GetPlayer(stateSnapshot, eventPlayer, subjectPlayer))
						TethysGame.SetEMPMissile(spawnPt.x, spawnPt.y, player.playerID, destPt.x, destPt.y);

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.SetMusic:                                    // Set music [ID CSV] repeat to [index]
				{
					string[] songs = m_ActionData.value.Split(',');
					List<int> songIDs = new List<int>(songs.Length);
					foreach (string song in songs)
					{
						int intResult;
						if (int.TryParse(song, out intResult))
							songIDs.Add(intResult);
						else
							Console.WriteLine("Failed to parse song ID: " + song + " for trigger type: " + type);
					}
					TethysGame.SetMusicPlayList(songIDs.Count, value2, songIDs.ToArray());

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.SetTileIndex:								// Set tile index to [value] in [region] fill [boolean]
				{
					MAP_RECT region = GetRegionRect(subjectRegion);
					if (value2 != 0)
					{
						// Fill entire region
						for (int x=region.xMin; x < region.xMax; ++x)
						{
							for (int y=region.yMin; y < region.yMax; ++y)
								GameMap.SetTile(x, y, value);
						}
					}
					else
					{
						// Random point in region
						LOCATION spawnPt = region.GetRandomPointInRect();
						GameMap.SetTile(spawnPt.x, spawnPt.y, value);
					}

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.SetCellType:									// Set cell type to [value] in [region] fill [boolean]
				{
					MAP_RECT region = GetRegionRect(subjectRegion);
					if (value2 != 0)
					{
						// Fill entire region
						for (int x=region.xMin; x < region.xMax; ++x)
						{
							for (int y=region.yMin; y < region.yMax; ++y)
								GameMap.SetCellType(x, y, value);
						}
					}
					else
					{
						// Random point in region
						LOCATION spawnPt = region.GetRandomPointInRect();
						GameMap.SetCellType(spawnPt.x, spawnPt.y, value);
					}

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.SetLavaPossible:								// Set lava possible to [value] in [region] fill [boolean]
				{
					MAP_RECT region = GetRegionRect(subjectRegion);
					if (value2 != 0)
					{
						// Fill entire region
						for (int x=region.xMin; x < region.xMax; ++x)
						{
							for (int y=region.yMin; y < region.yMax; ++y)
								GameMap.SetLavaPossible(x, y, value != 0);
						}
					}
					else
					{
						// Random point in region
						LOCATION spawnPt = region.GetRandomPointInRect();
						GameMap.SetLavaPossible(spawnPt.x, spawnPt.y, value != 0);
					}

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.SetLightLevel:								// Set light level to [value]
					GameMap.SetInitialLightLevel(value);

					return EventTriggerActionResult.Complete;

				case TriggerActionType.SetUnitsInRegionToAttackEnemyUnits:          // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to attack enemy units in [region]
				{
					List<PlayerState> players = GetPlayer(stateSnapshot, eventPlayer, subjectPlayer);
					foreach (PlayerState player in players)
					{
						// Get this player's units
						List<UnitState> units = GetUnitsInRegion(stateSnapshot, subjectQuantity, (map_id)subject, (map_id)subject2, new PlayerState[] { player }, subjectRegion);
						if (units.Count == 0)
							continue;

						List<PlayerState> enemies = GetPlayer(stateSnapshot, player, (int)TriggerPlayerCategory.EventEnemies);
						List<UnitState> enemyUnits = GetUnitsInRegion(stateSnapshot, 9000, map_id.Any, map_id.Any, enemies, value);
						if (enemyUnits.Count == 0)
							continue;

						foreach (UnitState unit in units)
						{
							// Attack random enemy unit
							Unit enemyUnit = GameState.GetUnit(enemyUnits[TethysGame.GetRandomRange(0, enemyUnits.Count)].unitID);

							GameState.GetUnit(unit.unitID).DoAttack(enemyUnit);
						}
					}

					return EventTriggerActionResult.Complete;
				}

				case TriggerActionType.SetUnitsInRegionToAttackGround:                  // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to attack ground in [region]
				case TriggerActionType.DeployMinesInRegion:                         // Set [quantity] robominer for [All/CurrentPlayer/Player#] in [region] to deploy mine in [region]
				case TriggerActionType.SetUnitsInRegionToBulldoze:                      // Set [quantity] robodozer for [All/CurrentPlayer/Player#] in [region] to bulldoze [region]
				case TriggerActionType.SetUnitsInRegionToDock:							// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to dock at structure in [region]
				//SetUnitsToDockInRegion,						// Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to dock in [region]
				case TriggerActionType.SetUnitsInRegionToStandGround:                   // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to stand ground in [region]
				case TriggerActionType.SetEarthworkersInRegionToBuildWallOrTube:        // Set [quantity] earthworker for [All/CurrentPlayer/Player#] in [region] to build [wall.map_id] at [region]
				case TriggerActionType.SetEarthworkersInRegionToRemoveWallOrTube:       // Set [quantity] earthworker for [All/CurrentPlayer/Player#] in [region] to remove wall at [region]
				case TriggerActionType.SetFactoriesInRegionToProduceUnit:               // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to produce unit [map_id] with cargo or weapon [map_id]
				case TriggerActionType.SetStructuresInRegionToTransferBayCargo:     // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to transfer cargo in [bay]
				case TriggerActionType.SetStructuresInRegionToLoadCargo:                // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to load cargo
				case TriggerActionType.SetStructuresInRegionToUnloadCargo:              // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to unload cargo
				case TriggerActionType.SetTrucksInRegionToDumpCargo:                    // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to dump cargo
				case TriggerActionType.SetLabsInRegionToResearch:                       // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to research [techIndex] with [quantity] scientists
				case TriggerActionType.SetUniversitiesInRegionToTrain:                  // Set [quantity] universities for [All/CurrentPlayer/Player#] in [region] to train [quantity] scientists
				case TriggerActionType.SetUnitsInRegionToRepair:                        // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to repair unit in [region]
				case TriggerActionType.SetSpidersInRegionToReprogram:                   // Set [quantity] spider for [All/CurrentPlayer/Player#] in [region] to reprogram unit in [region]
				case TriggerActionType.SetConvecsInRegionToDismantle:                   // Set [quantity] convec for [All/CurrentPlayer/Player#] in [region] to dismantle unit in [region]
				case TriggerActionType.SetTrucksInRegionToSalvage:                      // Set [quantity] cargo truck for [All/CurrentPlayer/Player#] in [region] to salvage [region] and drop off at [region]
				case TriggerActionType.SetUnitsInRegionToGuard:                     // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to guard unit in [region]
				case TriggerActionType.SetUnitsInRegionToPoof:                          // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to poof
				case TriggerActionType.SetSpaceportsInRegionToTransferLaunchpadCargo:   // Set [quantity] spaceport for [All/CurrentPlayer/Player#] in [region] to transfer launchpad cargo in [bay]
				case TriggerActionType.SetHealthForUnitsInRegion:                       // Set health of [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to [value] %
				case TriggerActionType.SetAutotargetForUnitsInRegion:                   // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to autotarget [boolean]
				case TriggerActionType.KillUnitsInRegion:                               // Kill [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region]
				case TriggerActionType.SetUnitsInRegionToSelfDestruct:                  // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to self destruct
				case TriggerActionType.TransferUnitsInRegionToPlayer:                   // Transfer [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to [CurrentPlayer/Player#]
				case TriggerActionType.SetWeaponForUnitsInRegion:                       // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] weapon to [weapon.map_id]
				case TriggerActionType.SetLightsForUnitsInRegion:                       // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] lights [boolean]
				case TriggerActionType.SetUnitsInRegionToMove:                          // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to move to [region]
				case TriggerActionType.TeleportUnitsInRegion:                           // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to teleport to [region]
				case TriggerActionType.SetConvecsInRegionToBuild:                       // Set [quantity] convec for [All/CurrentPlayer/Player#] in [region] to build structure at [region]
				case TriggerActionType.SetCargoForConvecsInRegion:                      // Set cargo of [quantity] convec for [All/CurrentPlayer/Player#] in [region] to [map_id] with [weapon.map_id]
				case TriggerActionType.SetCargoForTrucksInRegion:                       // Set cargo of [quantity] truck for [All/CurrentPlayer/Player#] in [region] to [quantity] [TruckCargo]

				case TriggerActionType.SetStructuresInRegionToIdle:                 // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to idle
				case TriggerActionType.SetStructuresInRegionToActivate:             // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to activate
				case TriggerActionType.SetStructuresInRegionToStop:                 // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to stop
				case TriggerActionType.SetStructuresInRegionToInfected:             // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to infected
				case TriggerActionType.SetSpaceportsInRegionToLaunch:                   // Set [quantity] spaceport for [All/CurrentPlayer/Player#] in [region] to launch and force [boolean]
				case TriggerActionType.SetStructuresInRegionBayCargo:                   // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] [bay] to [map_id] with cargo or weapon [map_id]
				case TriggerActionType.SetFactoriesInRegionToDevelop:                   // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to develop [map_id]
				case TriggerActionType.SetUnitsInRegionToClearSpecialTarget:            // Set [quantity] [units.map_id] for [All/CurrentPlayer/Player#] in [region] to clear special target

				case TriggerActionType.SetUnitToAttackEnemyUnits:                   // Set [CurrentUnit/UnitID] to attack unit in [region]
				case TriggerActionType.SetUnitToAttackGround:                           // Set [CurrentUnit/UnitID] to attack ground in [region]
				case TriggerActionType.DeployMine:                                      // Set [CurrentUnit/UnitID] to deploy mine in [region]
				case TriggerActionType.SetUnitToBulldoze:                               // Set [CurrentUnit/UnitID] to bulldoze [region]
				case TriggerActionType.SetUnitToDock:                                   // Set [CurrentUnit/UnitID] to dock at structure in [region]
				case TriggerActionType.SetUnitToStandGround:                            // Set [CurrentUnit/UnitID] to stand ground in [region]
				case TriggerActionType.SetEarthworkerToBuildWallOrTube:             // Set [CurrentUnit/UnitID] to build [wall.map_id] at [region]
				case TriggerActionType.SetEarthworkerToRemoveWallOrTube:                // Set [CurrentUnit/UnitID] to remove wall at [region]
				case TriggerActionType.SetFactoryToProduceUnit:                     // Set [CurrentUnit/UnitID] to produce unit [map_id] with cargo or weapon [map_id]
				case TriggerActionType.SetStructureToTransferBayCargo:                  // Set [CurrentUnit/UnitID] to transfer cargo in [bay]
				case TriggerActionType.SetStructureToLoadCargo:                     // Set [CurrentUnit/UnitID] to load cargo
				case TriggerActionType.SetStructureToUnloadCargo:                       // Set [CurrentUnit/UnitID] to unload cargo
				case TriggerActionType.SetTruckToDumpCargo:                         // Set [CurrentUnit/UnitID] to dump cargo
				case TriggerActionType.SetLabToResearch:                                // Set [CurrentUnit/UnitID] to research tech [index] with [quantity] scientists
				case TriggerActionType.SetUniversityToTrain:                            // Set [CurrentUnit/UnitID] to train [quantity] scientists
				case TriggerActionType.SetUnitToRepair:                             // Set [CurrentUnit/UnitID] to repair unit in [region]
				case TriggerActionType.SetSpiderToReprogram:                            // Set [CurrentUnit/UnitID] to reprogram unit in [region]
				case TriggerActionType.SetConvecToDismantle:                            // Set [CurrentUnit/UnitID] to dismantle unit in [region]
				case TriggerActionType.SetTruckToSalvage:                               // Set [CurrentUnit/UnitID] to salvage [region] and drop off at [region]
				case TriggerActionType.SetUnitToGuard:                                  // Set [CurrentUnit/UnitID] to guard unit in [region]
				case TriggerActionType.SetUnitToPoof:                                   // Set [CurrentUnit/UnitID] to poof (self destruct?)
				case TriggerActionType.SetSpaceportToTransferLaunchpadCargo:            // Set [CurrentUnit/UnitID] to transfer launchpad cargo in [bay]
				case TriggerActionType.SetHealthForUnit:                                // Set [CurrentUnit/UnitID] health to [value] %
				case TriggerActionType.SetAutotargetForUnit:                            // Set [CurrentUnit/UnitID] to auto target [boolean]
				case TriggerActionType.KillUnit:                                        // Kill [CurrentUnit/UnitID]
				case TriggerActionType.SetUnitToSelfDestruct:                           // Set [CurrentUnit/UnitID] to self destruct
				case TriggerActionType.TransferUnitToPlayer:                            // Transfer [CurrentUnit/UnitID] to player [CurrentPlayer/Player#]
				case TriggerActionType.SetWeaponForUnit:                                // Set [CurrentUnit/UnitID] weapon to [map_id]
				case TriggerActionType.SetLightsForUnit:                                // Set [CurrentUnit/UnitID] lights [boolean]
				case TriggerActionType.SetUnitToMove:                                   // Set [CurrentUnit/UnitID] to move to [region]
				case TriggerActionType.TeleportUnit:                                    // Set [CurrentUnit/UnitID] to teleport to [region]
				case TriggerActionType.SetConvecToBuild:                                // Set convec [CurrentUnit/UnitID] to build structure at [region]
				case TriggerActionType.SetCargoForConvec:                               // Set convec [CurrentUnit/UnitID] cargo to [map_id] with weapon [map_id]
				case TriggerActionType.SetCargoForTruck:                                // Set truck [CurrentUnit/UnitID] cargo to [quantity] [TruckCargo]

				case TriggerActionType.SetStructureToIdle:                              // Set structure [CurrentUnit/UnitID] to idle
				case TriggerActionType.SetStructureToActivate:                          // Set structure [CurrentUnit/UnitID] to activate
				case TriggerActionType.SetStructureToStop:                              // Set structure [CurrentUnit/UnitID] to stop
				case TriggerActionType.SetStructureToInfected:                          // Set structure [CurrentUnit/UnitID] to infected
				case TriggerActionType.SetSpaceportToLaunch:                            // Set spaceport [CurrentUnit/UnitID] to launch and force [boolean]
																						// Put in garage?
				case TriggerActionType.SetStructureBayCargo:                            // Set factory [CurrentUnit/UnitID] [bay] to [map_id] with cargo or weapon [map_id]
				case TriggerActionType.SetFactoryToDevelop:                         // Set factory [CurrentUnit/UnitID] to develop [map_id]
				case TriggerActionType.SetUnitToClearSpecialTarget:                 // Set [CurrentUnit/UnitID] to clear special target

				case TriggerActionType.CreateCargoTruck:                            // Create cargo truck with [quantity] [TruckCargo] in [region] facing [direction] with [health] [lights] and [UnitID]
				case TriggerActionType.CreateConvec:                                    // Create convec with [cargo.map_id] [weapon.map_id] in [region] facing [direction] with [health] [lights] and [UnitID]
				case TriggerActionType.CreateVehicle:                                   // Create vehicle [map_id] [weapon.map_id] in [region] facing [direction] with [health] [lights] and [UnitID]
				case TriggerActionType.CreateStructure:                             // Create structure [map_id] [weapon.map_id] in [region] with [health] and [UnitID]
				case TriggerActionType.CreateMine:                                      // Create mine [map_id] in [region] with [barYield] [barVariant] [health] and [UnitID]
				case TriggerActionType.CreateBeacon:                                    // Create beacon [map_id] in [region] with [oreType] [barYield] [barVariant]
				case TriggerActionType.CreateMarker:                                    // Create marker [MarkerType] in [region] with [UnitID]
				case TriggerActionType.CreateWreckage:                                  // Create [visible] wreckage [TechID] in [region]
				case TriggerActionType.CreateWallOrTube:                                // Create wall or tube [map_id] in [region]
					break;
			}

			return EventTriggerActionResult.Failure;
		}

		private int GetModifiedValue(int currentValue, TriggerModifier modifier, int actionValue)
		{
			switch (modifier)
			{
				case TriggerModifier.Set:		return actionValue;
				case TriggerModifier.Add:		return currentValue + actionValue;
				case TriggerModifier.Subtract:	return currentValue - actionValue;
			}

			return actionValue;
		}

		private bool IsValidRegion(int regionID)
		{
			if (regionID < 0 || regionID >= m_EventData.regions.Length)
			{
				Console.WriteLine("Invalid region: " + regionID + " for trigger action: " + type);
				return false;
			}

			return true;
		}

		private UnitState GetUnit(StateSnapshot stateSnapshot, UnitState eventUnit, int unitValue)
		{
			switch ((TriggerUnitCategory)unitValue)
			{
				case TriggerUnitCategory.EventUnit:		return eventUnit;
			}

			if (unitValue < 0 || unitValue >= m_EventData.unitIDs.Length)
				return null;

			// Get unit by ID
			int stubIndex = m_EventData.unitIDs[subject];
			UnitState unit = stateSnapshot.GetUnit(stubIndex);
			
			if (unit == null) return null;
			if (!unit.isLive) return null;

			return unit;
		}

		private List<PlayerState> GetPlayer(StateSnapshot stateSnapshot, PlayerState eventPlayer, int playerID)
		{
			List<PlayerState> players = new List<PlayerState>();

			switch ((TriggerPlayerCategory)playerID)
			{
				case TriggerPlayerCategory.TriggerOwner:	players.Add(stateSnapshot.players[m_OwnerID]);		break;
				case TriggerPlayerCategory.EventPlayer:		players.Add(eventPlayer);							break;
				case TriggerPlayerCategory.Any:
					// "Any" doesn't mean anything in this context. Let's not do anything for now to discourage its use.
					return players;

				case TriggerPlayerCategory.All:
					return new List<PlayerState>(stateSnapshot.players);

				case TriggerPlayerCategory.OwnerAllies:
					foreach (PlayerState player in stateSnapshot.players)
					{
						if (!stateSnapshot.players[m_OwnerID].allyPlayerIDs.Contains(player.playerID))
							continue;

						players.Add(player);
					}
					break;

				case TriggerPlayerCategory.OwnerEnemies:
					foreach (PlayerState player in stateSnapshot.players)
					{
						if (!stateSnapshot.players[m_OwnerID].enemyPlayerIDs.Contains(player.playerID))
							continue;

						players.Add(player);
					}
					break;

				case TriggerPlayerCategory.EventAllies:
					foreach (PlayerState player in stateSnapshot.players)
					{
						if (!eventPlayer.allyPlayerIDs.Contains(player.playerID))
							continue;

						players.Add(player);
					}
					break;

				case TriggerPlayerCategory.EventEnemies:
					foreach (PlayerState player in stateSnapshot.players)
					{
						if (!eventPlayer.enemyPlayerIDs.Contains(player.playerID))
							continue;

						players.Add(player);
					}
					break;

				default:
					if (playerID < 0 || playerID >= stateSnapshot.players.Count)
						return players;

					players.Add(stateSnapshot.players[playerID]);
					break;
			}

			return players;
		}

		private List<UnitState> GetUnitsInRegion(StateSnapshot stateSnapshot, int quantity, map_id unitType, map_id weaponType, IEnumerable<PlayerState> players, int regionID)
		{
			List<UnitState> units = new List<UnitState>();

			if (quantity <= 0)
				return units;

			MAP_RECT region = GetRegionRect(regionID);

			foreach (PlayerState player in players)
			{
				foreach (UnitState unit in player.units.GetListForType(unitType))
				{
					if (unitType != map_id.Any && unit.unitType != unitType) continue;
					if (weaponType != map_id.Any && unit.weapon != weaponType) continue;
					if (!region.Contains(unit.position)) continue;

					units.Add(unit);

					if (units.Count >= quantity)
						return units;
				}
			}

			return units;
		}

		private MAP_RECT GetRegionRect(int regionID)
		{
			switch ((TriggerRegion)regionID)
			{
				case TriggerRegion.Anywhere:
					return GameMap.bounds;
			}

			if (!IsValidRegion(regionID))
				return new MAP_RECT(0,0,0,0);

			return GetMapRect(m_EventData.regions[regionID]);
		}

		private MAP_RECT GetMapRect(EventRegionData eventRegion)
		{
			return MAP_RECT.FromMinMax(eventRegion.xMin, eventRegion.yMin, eventRegion.xMax, eventRegion.yMax);
		}

		private LOCATION GetRegionCenterPt(EventRegionData eventRegion)
		{
			return new LOCATION((eventRegion.xMin + eventRegion.xMax) / 2,
								(eventRegion.yMin + eventRegion.yMax) / 2);
		}

		private LOCATION GetRegionCenterPt(MAP_RECT eventRegion)
		{
			return new LOCATION((eventRegion.xMin + eventRegion.xMax) / 2,
								(eventRegion.yMin + eventRegion.yMax) / 2);
		}

		private void MoveRegion(EventRegionData eventRegion, int moveX, int moveY)
		{
			m_EventData.regions[subjectRegion].xMin += moveX;
			m_EventData.regions[subjectRegion].yMin += moveY;
			m_EventData.regions[subjectRegion].xMax += moveX;
			m_EventData.regions[subjectRegion].yMax += moveY;
		}
	}
}
