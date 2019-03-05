using System.Collections.Generic;

namespace DotNetMissionSDK.Triggers
{
	public delegate void TriggerCallback(TriggerStub trigger);


	public class TriggerManager
	{
		private SaveData m_SaveData;

		private List<TriggerStub> m_Triggers = new List<TriggerStub>();

		// Called when a trigger is fired. Use cbData to determine what to do.
		public event TriggerCallback onTriggerFired;


		/// <summary>
		/// Initialize with save data.
		/// </summary>
		/// <param name="saveData">The loaded save data.</param>
		public TriggerManager(SaveData saveData)
		{
			m_SaveData = saveData;

			// Load triggers into system
			for (int i=0; i < saveData.triggerCount; ++i)
				m_Triggers.Add(new TriggerStub(saveData.triggers[i]));
		}

		/// <summary>
		/// Adds a trigger stub to the manager.
		/// </summary>
		/// <param name="triggerStub">The stub to add to the system.</param>
		/// <returns>True, if the trigger was successfully added.</returns>
		public bool AddTrigger(TriggerStub triggerStub)
		{
			bool foundSlot = false;

			// Place trigger stub in next available slot
			if (m_SaveData.triggerCount < m_SaveData.triggers.Length)
			{
				m_SaveData.triggers[m_SaveData.triggerCount++] = triggerStub.stubData;
				foundSlot = true;
			}

			// If we are out of slots, search for an open slot
			for (int i=0; i < m_SaveData.triggerCount; ++i)
			{
				if (!m_SaveData.triggers[i].isActive)
				{
					m_SaveData.triggers[i] = triggerStub.stubData;
					foundSlot = true;
					break;
				}
			}

			// Add trigger to system
			if (foundSlot)
			{
				m_Triggers.Add(triggerStub);
			}

			return foundSlot;
		}

		/// <summary>
		/// Checks if triggers have fired, executes them if they did, then releases them if necessary.
		/// </summary>
		public void Update()
		{
			// Check triggers
			for (int i=0; i < m_Triggers.Count; ++i)
			{
				TriggerStub trigger = m_Triggers[i];

				if (trigger.CheckTrigger())
				{
					onTriggerFired?.Invoke(trigger);

					if (!trigger.isActive)
						m_Triggers.RemoveAt(i--);
				}
			}
		}
	}
}
