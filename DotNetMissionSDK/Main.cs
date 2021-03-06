﻿using DotNetMissionSDK.Async;
using DotNetMissionSDK.HFL;
using DotNetMissionSDK.Json;
using DotNetMissionSDK.State;
using DotNetMissionSDK.State.Snapshot;
using DotNetMissionSDK.Triggers;
using System;
using System.IO;


namespace DotNetMissionSDK
{
	/// <summary>
	/// The class containing entry points from Outpost 2.
	/// </summary>
    public class DotNetMissionEntry
	{
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
			// Put code that must be called from DLLMain.ProcessAttach here.
			// There should not be much need to put code here in most cases.
			// DO NOT PUT MISSION INIT CODE HERE.
			
			InitializeLog();

			m_MissionDLLName = Path.GetFileNameWithoutExtension(dllPath);

			Console.WriteLine("Initializing DotNet DLL...");
			Console.WriteLine("Mission DLLName: " + m_MissionDLLName);

			if (CustomLogic.useJson)
			{
				Console.WriteLine("Initializing JSON...");

				// Read JSON data
				if (!File.Exists(m_MissionDLLName + ".opm"))
				{
					Console.WriteLine("JSON data file '" + m_MissionDLLName + ".opm" + " not found!");
					return false;
				}

				// Load JSON data
				try
				{
					m_MissionData = MissionReader.GetMissionData(m_MissionDLLName + ".opm");
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					return false;
				}
			}

			Console.WriteLine("DLL Init complete.");

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
				Console.SetError(m_LogWriter);
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
			InitializeSystems();
			
			// Initialize mission with JSON data
			return m_MissionLogic.InitializeNewMission();
		}

		private void InitializeSystems()
		{
			// Prepare save buffer
			m_SaveBuffer.Load();

			// Initialize core
			ThreadAssert.Initialize();
			AsyncRandom.Initialize(TethysGame.GetRand(int.MaxValue));

			if (!HFLCore.Init())
				Console.WriteLine("Could not initialize HFL!");

			GameMap.Initialize();

			// Initialize mission systems
			m_Triggers = new TriggerManager(m_SaveBuffer.saveData);
			
			m_MissionLogic = new CustomLogic(m_MissionData, m_SaveBuffer.saveData, m_Triggers);
		}

		/// <summary>
		/// Called every update cycle.
		/// </summary>
		public void Update()
		{
			// Load the save buffer if it isn't loaded
			if (!m_SaveBuffer.isLoaded)
			{
				InitializeSystems();
				m_MissionLogic.LoadMission();
			}

			// Update essential systems
			m_Triggers.Update();
			AsyncPump.Update();
			GameState.Update();
			StateSnapshot stateSnapshot = StateSnapshot.Create();
			
			// Update mission logic
			m_MissionLogic.Update(stateSnapshot);

			stateSnapshot.Release();

			// Update save buffer
			m_SaveBuffer.Save();
		}

		/// <summary>
		/// Gets the save buffer for mission variables that need to be saved.
		/// </summary>
		/// <returns>The save buffer.</returns>
		public IntPtr GetSaveBuffer()
		{
			return m_SaveBuffer.GetSaveBuffer();
		}

		/// <summary>
		/// Gets the save buffer length for mission variables that need to be saved.
		/// </summary>
		/// <returns>The length of the save buffer.</returns>
		public int GetSaveBufferLength()
		{
			return m_SaveBuffer.GetSaveBufferLength();
		}

		/// <summary>
		/// Called when the DLL is detached.
		/// </summary>
		public void Detach()
		{
			m_MissionLogic.Dispose();
			m_SaveBuffer.Dispose();
			StateSnapshot.Destroy();
			AsyncPump.Release();

			// Dispose log file
			if (m_LogFileStream != null)
			{
				m_LogWriter.Close();
				m_LogFileStream.Close();
			}

			HFLCore.Cleanup();

			// Restore console
			StreamWriter sOut = new StreamWriter(Console.OpenStandardOutput());
			StreamWriter sError = new StreamWriter(Console.OpenStandardError());
			sOut.AutoFlush = true;
			sError.AutoFlush = true;
			Console.SetOut(sOut);
			Console.SetError(sError);
		}
	}
}
