using DotNetMissionSDK.Json;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.Triggers;
using System;

namespace DotNetMissionSDK
{
	/// <summary>
	/// This is the primary class for performing custom mission logic.
	/// Use SaveData to store data that must persist when the mission is saved and loaded.
	/// Other SDK classes should not need to be modified.
	/// </summary>
	public class CustomLogic : MissionLogic
	{
		/// <summary>
		/// If true, main will check for and load the JSON data file.
		/// </summary>
		public const bool useJson = true;

		/// <summary>
		/// Called when the mission is first loaded, regardless of whether it is a new game or saved game.
		/// </summary>
		/// <param name="root">The filled JSON data root.</param>
		/// <param name="saveData">The save data class.</param>
		/// <param name="triggerManager">The trigger manager used for the mission.</param>
		public CustomLogic(MissionRoot root, SaveData saveData, TriggerManager triggerManager) : base(root, saveData, triggerManager)
		{
			// Players, units and the map have not been fully initialized at this point.
			// In most cases, you want to add code to StartMission() instead.

			// *** Add custom pre-init code here ***
		}

		/// <summary>
		/// Called when a new mission should start. Performs initial setup.
		/// </summary>
		/// <returns>True on success.</returns>
		public override bool InitializeNewMission()
		{
			if (!base.InitializeNewMission())
				return false;

			// *** Add custom "New Mission" code here ***
			
			// Test code
			AddTrigger(TriggerStub.CreateVehicleCountTrigger(999, true, true, TethysGame.LocalPlayer(), 4, CompareMode.GreaterEqual));
			
			return true;
		}

		/// <summary>
		/// Called when a mission is loaded from a saved game. Performs reinitialization of data lost during quit.
		/// </summary>
		public override void LoadMission()
		{
			base.LoadMission();

			// *** Add custom "Load Mission" code here ***
		}

		/// <summary>
		/// Called when the mission has finished initializing, regardless of whether it is a new game or saved game.
		/// </summary>
		protected override void StartMission()
		{
			base.StartMission();

			// *** Add custom start code here ***
		}

		/// <summary>
		/// Called when a trigger has been executed.
		/// </summary>
		/// <param name="trigger">The trigger that was executed.</param>
		protected override void OnTriggerExecuted(TriggerStub trigger)
		{
			switch (trigger.id)
			{
				case 999:
					TethysGame.AddMessage(0, 0, "Check me out!", TethysGame.LocalPlayer(), 0);
					Console.WriteLine("Check me out!");
					break;

				default:
					base.OnTriggerExecuted(trigger);
					break;
			}
		}

		/// <summary>
		/// Called every game cycle.
		/// </summary>
		/// <param name="stateSnapshot">The current immutable state of the game.</param>
		public override void Update(StateSnapshot stateSnapshot)
		{
			base.Update(stateSnapshot);

			// *** Add custom update code here ***
		}

		/// <summary>
		/// Releases all mission resources.
		/// </summary>
		public override void Dispose()
		{
			// *** Add Custom "Dispose" code here ***

			base.Dispose();
		}
	}
}
