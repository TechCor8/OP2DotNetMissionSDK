using DotNetMissionSDK.Json;
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
		}

		/// <summary>
		/// Called when a new mission should start. Performs initial setup.
		/// </summary>
		/// <returns>True on success.</returns>
		public override bool InitializeNewMission()
		{
			Console.WriteLine("Mission started.");

			if (!base.InitializeNewMission())
				return false;

			// Test code
			AddTrigger(TriggerStub.CreateVehicleCountTrigger(999, true, true, TethysGame.LocalPlayer(), 3, compare_mode.cmpGreaterEqual));
			
			return true;
		}

		/// <summary>
		/// Called when a mission is loaded from a saved game. Performs reinitialization of data lost during quit.
		/// </summary>
		public override void LoadMission()
		{
			Console.WriteLine("Mission loaded.");

			base.LoadMission();
		}

		/// <summary>
		/// Called when a trigger has been executed.
		/// </summary>
		/// <param name="trigger">The trigger that was executed.</param>
		protected override void OnTriggerExecuted(TriggerStub trigger)
		{
			base.OnTriggerExecuted(trigger);

			if (trigger.id == 999)
			{
				TethysGame.AddMessage(0, 0, "Trigger Fired!", TethysGame.LocalPlayer(), 0);
				Console.WriteLine("Trigger Fired!");

				using (PlayerUnitEnum myEnum = new PlayerUnitEnum(TethysGame.LocalPlayer()))
				using (Unit unit = new Unit())
				{
					while (myEnum.GetNext(unit))
					{
						unit.DoMove(30, 30);
					}
				}
			}

			//Console.WriteLine("PRE");
			//Player p = TethysGame.GetPlayer(TethysGame.LocalPlayer());
			//Console.WriteLine("P");
			//ScGroup g = p.GetDefaultGroup();

			//Console.WriteLine("G");
			//Console.WriteLine("Stub: " + g.stubIndex);
		}

		/// <summary>
		/// Called every game cycle.
		/// </summary>
		public override void Update()
		{
			base.Update();
		}

		/// <summary>
		/// Releases all mission resources.
		/// </summary>
		public override void Dispose()
		{
			Console.WriteLine("Mission Ended.");

			base.Dispose();
		}
	}
}
