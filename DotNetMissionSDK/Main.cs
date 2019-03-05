using DotNetMissionSDK.Json;
using DotNetMissionSDK.Triggers;
using System;
using System.Collections.Generic;
using System.IO;


namespace DotNetMissionSDK
{
	/// <summary>
	/// The class containing entry points from Outpost 2.
	/// </summary>
    public class DotNetMissionEntry
	{
		// Mission singleton instance
		private static DotNetMissionEntry m_Instance = new DotNetMissionEntry();

		// Debug log streams
		private FileStream m_LogFileStream;
		private StreamWriter m_LogWriter;

		// Mission Data
		private string m_MissionDLLName;
		MissionRoot m_MissionData;
		private SaveBuffer<SaveData> m_SaveBuffer = new SaveBuffer<SaveData>();

		// Essential Systems
		TriggerManager m_Triggers;

		// Mission logic
		private MissionLogic m_MissionLogic;


		/// <summary>
		/// Called when DLL is first loaded.
		/// </summary>
		/// <param name="dllPath">The path to the mission DLL.</param>
		/// <returns>True on success.</returns>
		public bool Attach(string dllPath)
		{
			// Call singleton
			return m_Instance._Attach(dllPath);
		}

		// Put code that must be called from DLLMain.ProcessAttach here.
		// There should not be much need to put code here in most cases.
		// DO NOT PUT MISSION CODE HERE. THE GAME IS NOT GUARANTEED TO BE READY.
		private bool _Attach(string dllPath)
		{
			InitializeLog();

			m_MissionDLLName = Path.GetFileNameWithoutExtension(dllPath);

			// Read JSON data
			if (!File.Exists(m_MissionDLLName + ".opm"))
				return false;

			// Load JSON data
			m_MissionData = MissionReader.GetMissionData(m_MissionDLLName + ".opm");

			return true;
		}

		private void InitializeLog()
		{
			// Initialize debug log
			try
			{
				m_LogFileStream = new FileStream("DotNetLog.txt", FileMode.Create, FileAccess.Write, FileShare.Read);
				m_LogWriter = new StreamWriter(m_LogFileStream);
				m_LogWriter.AutoFlush = true;
				Console.SetOut(m_LogWriter);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		/// <summary>
		/// Called when the mission is first loaded.
		/// NOTE: This is NOT called when the mission is loaded from a save file!
		/// </summary>
		/// <returns>True on success.</returns>
		public bool Initialize()
		{
			// Call singleton
			return m_Instance._Initialize();
		}

		// ** Put mission init code here. **
		private bool _Initialize()
		{
			InitializeSystems();
			
			// Initialize mission with JSON data
			m_MissionLogic.InitializeNewMission();

			// TODO: Remove Me
			Console.WriteLine("Initialized!");
			Console.WriteLine("Mission DLL: " + m_MissionDLLName);
			Console.WriteLine("Mission Desc: " + m_MissionData.levelDetails.description);
			
			m_Triggers.AddTrigger(TriggerStub.CreateVehicleCountTrigger(true, true, TethysGame.LocalPlayer(), 3, compare_mode.cmpGreaterEqual));
			// **End TODO**

			return true;
		}

		private void InitializeSystems()
		{
			// Prepare save buffer
			m_SaveBuffer.Load();

			// Init essential systems
			m_Triggers = new TriggerManager(m_SaveBuffer.saveData);
			m_Triggers.onTriggerFired += OnTriggerFired;

			m_MissionLogic = new MissionLogic(m_MissionData, m_SaveBuffer.saveData, m_Triggers);
		}

		private void OnTriggerFired(TriggerStub trigger)
		{
			TethysGame.AddMessage(0, 0, "Trigger Fired!", TethysGame.LocalPlayer(), 0);
			Console.WriteLine("Trigger Fired!");
		}

		/// <summary>
		/// Called every update cycle.
		/// </summary>
		public void Update()
		{
			m_Instance._Update();
		}

		// ** Put mission update code here. **
		private void _Update()
		{
			// Load the save buffer if it isn't loaded
			if (!m_SaveBuffer.isLoaded)
				InitializeSystems();

			// Update essential systems
			m_Triggers.Update();

			// Update mission logic
			m_MissionLogic.Update();

			// Update save buffer
			m_SaveBuffer.Save();
		}

		/// <summary>
		/// Gets the save buffer for mission variables that need to be saved.
		/// </summary>
		/// <returns>The save buffer.</returns>
		public IntPtr GetSaveBuffer()
		{
			return m_Instance.m_SaveBuffer.GetSaveBuffer();
		}

		/// <summary>
		/// Gets the save buffer length for mission variables that need to be saved.
		/// </summary>
		/// <returns>The length of the save buffer.</returns>
		public int GetSaveBufferLength()
		{
			return m_Instance.m_SaveBuffer.GetSaveBufferLength();
		}

		/// <summary>
		/// Must be called when the mission restarts.
		/// </summary>
		public void Restart()
		{
			m_MissionLogic.Restart();

			m_Triggers.onTriggerFired -= OnTriggerFired;
		}
		
		private void Dispose()
		{
			if (m_LogFileStream != null)
			{
				m_LogFileStream.Close();
				m_LogWriter.Close();
			}

			m_SaveBuffer.Dispose();

			m_Triggers.onTriggerFired -= OnTriggerFired;
		}
	}
}
