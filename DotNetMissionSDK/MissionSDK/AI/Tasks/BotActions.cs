using System;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks
{
	public class BotCommands
	{
		/// <summary>
		/// Stores data about a unit command performed by a bot manager.
		/// </summary>
		private class UnitCommand
		{
			public int unitID		{ get; private set; }
			public int priority		{ get; private set; }
			public Action action	{ get; private set; }

			public UnitCommand(int unitID, int priority, Action action)
			{
				this.unitID = unitID;
				this.priority = priority;
				this.action = action;
			}
		}

		private List<UnitCommand> m_UnitCommands = new List<UnitCommand>();


		/// <summary>
		/// Adds a unit command to be executed.
		/// If the unit already has a pending command, the one with the highest priority will be issued.
		/// </summary>
		/// <param name="unitID">The unit that receives the command.</param>
		/// <param name="priority">The priority of the command.</param>
		/// <param name="action">The command to perform.</param>
		public bool AddUnitCommand(int unitID, int priority, Action action)
		{
			int prevIndex = m_UnitCommands.FindIndex((unitAction) => unitAction.unitID == unitID);

			// If there is no previous action for this unit, or the new action has a higher priority, issue the action.
			if (prevIndex < 0 || priority > m_UnitCommands[prevIndex].priority)
			{
				if (prevIndex >= 0)
					m_UnitCommands.RemoveAt(prevIndex);

				m_UnitCommands.Add(new UnitCommand(unitID, priority, action));
				return true;
			}

			return false;
		}

		/// <summary>
		/// Executes all pending commands.
		/// </summary>
		public void Execute()
		{
			foreach (UnitCommand command in m_UnitCommands)
				command.action();
		}
	}
}
