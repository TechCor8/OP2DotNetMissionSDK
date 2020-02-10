using DotNetMissionSDK.State.Game;
using DotNetMissionSDK.Triggers;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK
{
	/// <summary>
	/// The save data class for storing values that must persist when loading the mission from a save file.
	/// Make sure this class does not contain any reference types.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi, Pack=1)]
	public class SaveData
	{
		// Required Data
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)] // Maximum supported triggers
		public TriggerStubData[] triggers;
		public int triggerCount;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] // Maximum supported players
		public PlayerStarshipSaveData[] playerStarship;

		// Json Data
		public byte missionVariantIndex;
		public EventSystemData eventData;

		// Custom Mission Data
		//[MarshalAs(UnmanagedType.LPStr, SizeConst = 50)]
		//public string testStr;
	}
}
