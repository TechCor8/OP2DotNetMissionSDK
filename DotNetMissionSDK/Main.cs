using DotNetMissionSDK.Json;
using System;
using System.IO;
using System.Runtime.InteropServices;

/*
 * TODO:
 * 
 * Create mission Format Serializer/Deserializer
 * Create OP2Script API wrapper (Maybe try to link directly to Outpost2DLL instead)
 * Create OP2DotNetLib API wrapper
 * Create objects based on mission format
*/

namespace DotNetMissionSDK
{
	/// <summary>
	/// The save data class for storing values that must persist when loading the mission from a save file.
	/// Make sure this class does not contain any reference types.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi, Pack=1)]
	public class SaveData
	{
		public int test;
		public int test2;

		[MarshalAs(UnmanagedType.LPStr, SizeConst = 50)]
		public string testStr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] testArr;
	}

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

		// Native DLL name and Save Buffer
		private string m_MissionDLLName;
		private SaveBuffer<SaveData> m_SaveBuffer = new SaveBuffer<SaveData>();

		// Mission logic
		private MissionLogic m_MissionLogic = new MissionLogic();


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
			m_MissionDLLName = Path.GetFileNameWithoutExtension(dllPath);

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

			return true;
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
			// Prepare save buffer
			m_SaveBuffer.Load();

			// Read JSON data
			if (!File.Exists(m_MissionDLLName + ".opm"))
				return false;

			MissionRoot missionData = MissionReader.GetMissionData(m_MissionDLLName + ".opm");

			// Initialize mission with JSON data
			m_MissionLogic.Initialize(missionData);

			// TODO: Remove Me
			Console.WriteLine("Initialized!");
			Console.WriteLine("Mission DLL: " + m_MissionDLLName);
			Console.WriteLine("Mission Desc: " + missionData.levelDetails.description);
			Console.WriteLine("Tube: " + missionData.tethysGame.wallTubes[0].typeID);
			Console.WriteLine("Tube: " + missionData.tethysGame.wallTubes[0].location.x);
			Console.WriteLine("Tube: " + missionData.tethysGame.wallTubes[0].location.y);
			Console.WriteLine("m_SaveBuffer" + m_SaveBuffer);
			// **End TODO**

			Console.WriteLine("PreSet");
			Console.WriteLine("Test = " + m_SaveBuffer.saveData.test);
			Console.WriteLine("Test2 = " + m_SaveBuffer.saveData.test2);

			m_SaveBuffer.saveData.test = TethysGame.GetRand(600);
			m_SaveBuffer.saveData.test2 = TethysGame.GetRand(600);

			Console.WriteLine("PostSet");
			Console.WriteLine("Test = " + m_SaveBuffer.saveData.test);
			Console.WriteLine("Test2 = " + m_SaveBuffer.saveData.test2);

			return true;
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
				m_SaveBuffer.Load();

			// Update mission logic
			m_MissionLogic.Update();

			/*Console.WriteLine("Update");
			Console.WriteLine("Test = " + m_SaveBuffer.saveData.test);
			Console.WriteLine("Test2 = " + m_SaveBuffer.saveData.test2);*/

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

		public void Dispose()
		{
			if (m_LogFileStream != null)
			{
				m_LogFileStream.Close();
				m_LogWriter.Close();
			}
		}
	}
}
