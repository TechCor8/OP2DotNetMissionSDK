using DotNetMissionReader;
using DotNetMissionSDK.AI;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.State.Snapshot.Units;

namespace DotNetMissionSDK.Triggers
{
	public class EventTrigger
	{
		private TriggerData m_TriggerData;
		private EventTriggerData m_TriggerSaveData;
		
		private EventTriggerCondition[] m_TriggerConditions;
		private EventTriggerAction[] m_TriggerActions;

		private BotPlayer[] m_BotPlayers;

		public bool isExecuting { get { return m_TriggerSaveData.isExecuting;		}	}


		public EventTrigger(EventSystemData eventData, TriggerData triggerData, EventTriggerData triggerSaveData, int ownerID)
		{
			m_TriggerData = triggerData;
			m_TriggerSaveData = triggerSaveData;
			
			// Cache trigger conditions
			m_TriggerConditions = new EventTriggerCondition[triggerData.Conditions.Count];
			for (int i=0; i < m_TriggerConditions.Length; ++i)
				m_TriggerConditions[i] = new EventTriggerCondition(eventData, triggerData.Conditions[i], ownerID);

			// Cache trigger actions
			m_TriggerActions = new EventTriggerAction[triggerData.Actions.Count];
			for (int i=0; i < m_TriggerActions.Length; ++i)
				m_TriggerActions[i] = new EventTriggerAction(eventData, triggerData.Actions[i], ownerID);
		}

		public void NewMission()
		{
			// Initialize default state
			m_TriggerSaveData.enabled = m_TriggerData.Enabled;
			m_TriggerSaveData.isExecuting = false;
		}

		public void StartMission(BotPlayer[] botPlayers)
		{
			m_BotPlayers = botPlayers;
		}

		/// <summary>
		/// Checks if this trigger's conditions are met.
		/// </summary>
		public bool CheckConditions(StateSnapshot stateSnapshot, int currentMark, int currentTick, PlayerState eventPlayer, UnitState eventUnit, int eventRegionIndex, int eventTopic)
		{
			if (!m_TriggerSaveData.enabled) return false;

			// Check if all conditions are met
			foreach (EventTriggerCondition condition in m_TriggerConditions)
			{
				if (!condition.DidMeetCondition(stateSnapshot, currentMark, currentTick, eventPlayer, eventUnit, eventRegionIndex, eventTopic))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Marks the trigger for execution.
		/// </summary>
		public void Execute(int currentTick, PlayerState eventPlayer, UnitState eventUnit, int eventRegionIndex, int eventTopic)
		{
			if (!m_TriggerSaveData.enabled) return;
			if (m_TriggerSaveData.isExecuting) return;

			// Initialize execution which will start on the next update.
			m_TriggerSaveData.enabled = false;
			m_TriggerSaveData.isExecuting = true;
			m_TriggerSaveData.executionIndex = 0;
			m_TriggerSaveData.executionTick = currentTick;

			m_TriggerSaveData.eventPlayerID = eventPlayer != null ? eventPlayer.playerID : -1;
			m_TriggerSaveData.eventUnitID = eventUnit != null ? eventUnit.unitID : -1;
			m_TriggerSaveData.eventRegionID = eventRegionIndex;
			m_TriggerSaveData.eventTopicID = eventTopic;
		}

		/// <summary>
		/// Updates an executing trigger.
		/// </summary>
		public void Update(EventSystem eventSystem, StateSnapshot stateSnapshot, int currentTick)
		{
			if (!m_TriggerSaveData.isExecuting) return;

			// Get event state
			PlayerState eventPlayer = m_TriggerSaveData.eventPlayerID >= 0 ? stateSnapshot.players[m_TriggerSaveData.eventPlayerID] : null;
			UnitState eventUnit = stateSnapshot.GetUnit(m_TriggerSaveData.eventUnitID);
			int eventRegionIndex = m_TriggerSaveData.eventRegionID;
			int eventTopic = m_TriggerSaveData.eventTopicID;

			// Execute all pending actions
			for (; m_TriggerSaveData.executionIndex < m_TriggerActions.Length; ++m_TriggerSaveData.executionIndex)
			{
				EventTriggerAction action = m_TriggerActions[m_TriggerSaveData.executionIndex];
				switch (action.Execute(eventSystem, stateSnapshot, m_BotPlayers, m_TriggerSaveData.executionTick, currentTick, eventPlayer, eventUnit, eventRegionIndex, eventTopic))
				{
					case EventTriggerActionResult.PreserveTrigger:
						// Allow trigger to be used again
						m_TriggerSaveData.enabled = true;
						break;

					case EventTriggerActionResult.Wait:
						// Stop processing this trigger's actions for this update
						return;
				}

				// Set execution tick for next action
				m_TriggerSaveData.executionTick = currentTick;
			}

			// All actions executed
			m_TriggerSaveData.isExecuting = false;
			m_TriggerSaveData.executionIndex = 0;
			m_TriggerSaveData.executionTick = currentTick;
		}
	}
}
