using DotNetMissionSDK.Json;
using System.IO;
using System.Runtime.Serialization.Json;


namespace DotNetMissionSDK
{
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
