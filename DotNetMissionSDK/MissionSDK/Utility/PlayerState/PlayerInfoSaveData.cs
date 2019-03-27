using System.Runtime.InteropServices;

namespace DotNetMissionSDK.Utility.PlayerState
{
	/// <summary>
	/// Contains persistent state that is stored in the SaveData object. Used by PlayerInfo class.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi, Pack=1)]
	public class PlayerInfoSaveData
	{
		// Starship module counts
		public byte EDWARDSatelliteCount;
		public byte solarSatelliteCount;
		public byte ionDriveModuleCount;
		public byte fusionDriveModuleCount;
		public byte commandModuleCount;
		public byte fuelingSystemsCount;
		public byte habitatRingCount;
		public byte sensorPackageCount;
		public byte skydockCount;
		public byte stasisSystemsCount;
		public byte orbitalPackageCount;
		public byte phoenixModuleCount;

		public byte rareMetalsCargoCount;
		public byte commonMetalsCargoCount;
		public byte foodCargoCount;
		public byte evacuationModuleCount;
		public byte childrenModuleCount;
	}
}
