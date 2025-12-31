using System.IO;
using System.Runtime.Serialization.Json;


namespace DotNetMissionReader
{
	// NOTE:
	// This reader is the same in both DotNetMissionSDK and OP2GameLogic projects.
	// When making changes, copy the files over to keep them in sync.
	// Eventually, this should be made into a shared library.

	/// <summary>
	/// Reads JSON data for mission.
	/// </summary>
	public class MissionReader
	{
		public static MissionRoot GetMissionData(string filePath)
		{
			using (FileStream fs = new FileStream(filePath, FileMode.Open))
			{
				// Read mission file
				DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(MissionRoot));
				MissionRoot root = (MissionRoot)deserializer.ReadObject(fs);

				return root;
			}
		}

		public static void WriteMissionData(string filePath, MissionRoot root)
		{
			using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
			{
				// Write mission file
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MissionRoot));
				serializer.WriteObject(fs, root);
			}
		}
	}
}
