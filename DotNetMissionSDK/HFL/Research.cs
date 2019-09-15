using DotNetMissionSDK.Async;
using System;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK.HFL
{
	public enum LabType
	{
		ltBasic			= 1,
		ltStandard		= 2,
		ltAdvanced		= 3
	};

	// "CATEGORY" values (for both items and upgrades to these items)
	public enum TechCategory
	{
		tcFree			= 0, // 0 = Free technologies (and unavailable technologies)
		tcBasic			= 1, // 1 = Basic labratory sciences
		tcDefense		= 2, // 2 = Defenses (GP upgrade, walls, and efficiency engineering)
		tcPower			= 3, // 3 = Power
		tcVehicles		= 4, // 4 = Vehicles (which ones can be built, speed upgrades, armour upgrades)
		tcFood			= 5, // 5 = Food
		tcMetals		= 6, // 6 = Metals gathering
		tcWeapons		= 7, // 7 = Weapons
		tcSpace			= 8, // 8 = Space (spaceport, observatory, launch vehicle, skydock)
		tcMorale		= 9, // 9 = Population (happiness)
		tcDisaster		= 10,// 10 = Disaster warning (and defense)
		tcPopulation	= 11,// 11 = Population (health, growth)
		tcSpaceship		= 12 // 12 = Spaceship module
	};

	public class Research
	{
		public static int GetTechCount()							{ return Research_GetTechCount();										}
		
		/// <summary>
		/// Gets tech info by its index in the tech array.
		/// </summary>
		/// <param name="index">The index of the tech info in the tech array. NOT the tech ID.</param>
		public static TechInfo GetTechInfo(int index)				{ return new TechInfo(Research_GetTechInfo(index));						}
		public static int GetMaxTechID()							{ return Research_GetMaxTechID();										}

		[DllImport("DotNetInterop.dll")] private static extern int Research_GetTechCount();
		[DllImport("DotNetInterop.dll")] private static extern IntPtr Research_GetTechInfo(int index);
		[DllImport("DotNetInterop.dll")] private static extern int Research_GetMaxTechID();
	}

	public class TechInfo
	{
		private IntPtr m_Handle;

		public TechInfo(IntPtr handle)								{ m_Handle = handle;																	}

		public bool IsValid()										{ return TechInfo_IsValid(m_Handle) > 0;												}
		public int GetTechID()										{ return TechInfo_GetTechID(m_Handle);													}
		public TechCategory GetCategory()							{ return TechInfo_GetCategory(m_Handle);												}
		public int GetTechLevel()									{ return TechInfo_GetTechLevel(m_Handle);												}
		public int GetPlymouthCost()								{ return TechInfo_GetPlymouthCost(m_Handle);											}
		public int GetEdenCost()									{ return TechInfo_GetEdenCost(m_Handle);												}
		public int GetMaxScientists()								{ return TechInfo_GetMaxScientists(m_Handle);											}
		public LabType GetLab()										{ return TechInfo_GetLab(m_Handle);														}
		public int GetPlayerHasTech()								{ ThreadAssert.MainThreadRequired();	return TechInfo_GetPlayerHasTech(m_Handle);		}
		public int GetNumUpgrades()									{ return TechInfo_GetNumUpgrades(m_Handle);												}
		public int GetNumRequiredTechs()							{ return TechInfo_GetNumRequiredTechs(m_Handle);										}
		public string GetTechName()									{ return Marshal.PtrToStringAnsi(TechInfo_GetTechName(m_Handle));						}
		public string GetDescription()								{ return Marshal.PtrToStringAnsi(TechInfo_GetDescription(m_Handle));					}
		public string GetTeaser()									{ return Marshal.PtrToStringAnsi(TechInfo_GetTeaser(m_Handle));							}
		public string GetImproveDesc()								{ return Marshal.PtrToStringAnsi(TechInfo_GetImproveDesc(m_Handle));					}
		public int GetRequiredTechIndex(int index)					{ return TechInfo_GetRequiredTechIndex(m_Handle, index);								}


		[DllImport("DotNetInterop.dll")] private static extern int TechInfo_IsValid(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int TechInfo_GetTechID(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern TechCategory TechInfo_GetCategory(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int TechInfo_GetTechLevel(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int TechInfo_GetPlymouthCost(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int TechInfo_GetEdenCost(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int TechInfo_GetMaxScientists(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern LabType TechInfo_GetLab(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int TechInfo_GetPlayerHasTech(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int TechInfo_GetNumUpgrades(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int TechInfo_GetNumRequiredTechs(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern IntPtr TechInfo_GetTechName(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern IntPtr TechInfo_GetDescription(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern IntPtr TechInfo_GetTeaser(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern IntPtr TechInfo_GetImproveDesc(IntPtr handle);
		[DllImport("DotNetInterop.dll")] private static extern int TechInfo_GetRequiredTechIndex(IntPtr handle, int index);
	}
}
