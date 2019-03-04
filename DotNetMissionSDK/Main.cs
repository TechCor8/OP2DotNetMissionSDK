using DotNetMissionSDK.Json;
using System;
using System.IO;

/*
 * TODO:
 * 
 * Create logger
 * Create mission Format Serializer/Deserializer
 * Create OP2Script API wrapper (Maybe try to link directly to Outpost2DLL instead)
 * Create OP2DotNetLib API wrapper
 * Create objects based on mission format
*/

namespace DotNetMissionSDK
{
    public class DotNetMissionEntry
	{
		private static DotNetMissionEntry m_Instance = new DotNetMissionEntry();

		private string m_MissionDLLName;

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
			
			return true;
		}

		/// <summary>
		/// Called when the mission is first loaded.
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
			if (!File.Exists(m_MissionDLLName + ".opm"))
				return false;

			MissionRoot missionData = MissionReader.GetMissionData(m_MissionDLLName + ".opm");

			m_MissionLogic.Initialize(missionData);

			// TODO: Remove Me
			using (FileStream fs = new FileStream("DotNetLog.txt", FileMode.Create))
			using (StreamWriter writer = new StreamWriter(fs))
			{
				writer.WriteLine("Initialized!");
				writer.WriteLine("Mission DLL: " + m_MissionDLLName);
				writer.WriteLine("Mission Desc: " + missionData.levelDetails.description);
				writer.WriteLine("Tube: " + missionData.tethysGame.wallTubes[0].typeID);
				writer.WriteLine("Tube: " + missionData.tethysGame.wallTubes[0].location.x);
				writer.WriteLine("Tube: " + missionData.tethysGame.wallTubes[0].location.y);
			}
			// **End TODO**

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
			m_MissionLogic.Update();
		}

		/// <summary>
		/// Used to return a buffer description to OP2 for storing data to saved game files
		/// </summary>
		/// <param name="bufferStart">Pointer to beginning of DLL data buffer</param>
		/// <param name="length">Length of this buffer</param>
		public void GetSaveRegions(IntPtr bufferStart, int length)
		{
			m_Instance._GetSaveRegions(bufferStart, length);
		}

		private void _GetSaveRegions(IntPtr bufferStart, int length)
		{
			byte[] buffer = new byte[length];

			using (MemoryStream stream = new MemoryStream(buffer, true))
			{
				// TODO: Write data to save up to maximum of "length"

				// Copy stream data into unmanaged buffer
				System.Runtime.InteropServices.Marshal.Copy(stream.ToArray(), 0, bufferStart, length);
			}
		}
	}
}
