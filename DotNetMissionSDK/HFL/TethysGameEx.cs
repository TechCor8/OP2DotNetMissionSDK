using System.Runtime.InteropServices;

namespace DotNetMissionSDK.HFL
{
	/// <summary>
	/// Extension class for TethysGame.
	/// </summary>
	public class TethysGameEx : TethysGame
	{
		// TODO: Confirm that this is actually returning a unitID and is not a success flag or "unit count"
		public static Unit CreateUnitEx(map_id unitType, int pixelX, int pixelY, int creatorId, int cargoWeapon, int unitIndex, bool centerInTile)
		{
			int unitID = TethysGameEx_CreateUnitEx(unitType, pixelX, pixelY, creatorId, cargoWeapon, unitIndex, centerInTile ? 1 : 0);
			return new UnitEx(unitID);
		}
		public static void ReloadSheets()																	{ TethysGameEx_ReloadSheets();											}
		public static void LoadTechTree(string fileName, int maxTechLevel)									{ TethysGameEx_LoadTechTree(fileName, maxTechLevel);					}

		public static int NumHumanPlayers()																	{ return TethysGameEx_NumHumanPlayers();								}

		public static void SetCheatFastProductionEx(bool active)											{ TethysGameEx_SetCheatFastProductionEx(active ? 1 : 0);				}
		public static void SetCheatFastUnitsEx(bool active)													{ TethysGameEx_SetCheatFastUnitsEx(active ? 1 : 0);						}
		public static void SetCheatProduceAllEx(bool active)												{ TethysGameEx_SetCheatProduceAllEx(active ? 1 : 0);					}
		public static void SetCheatUnlimitedResourcesEx(bool active)										{ TethysGameEx_SetCheatUnlimitedResourcesEx(active ? 1 : 0);			}

		public static void SetShowVehicleRoutes(bool active)												{ TethysGameEx_SetShowVehicleRoutes(active ? 1 : 0);					}
		public static void SetEnableMoraleLog(bool active)													{ TethysGameEx_SetEnableMoraleLog(active ? 1 : 0);						}
		public static void SetDamage4X(bool active)															{ TethysGameEx_SetDamage4X(active ? 1 : 0);								}
		public static void SetRCCEffect(RCCEffectState setting)												{ TethysGameEx_SetRCCEffect(setting);									}
	
		public static void SetTopStatusBar(string text)														{ TethysGameEx_SetTopStatusBar(text);									}
		public static void SetBottomStatusBar(string text, uint color)										{ TethysGameEx_SetBottomStatusBar(text, color);							}
	
		public static void ResetCheatedGame()																{ TethysGameEx_ResetCheatedGame();										}




		[DllImport("DotNetInterop.dll")] private static extern int TethysGameEx_CreateUnitEx(map_id unitType, int pixelX, int pixelY, int creatorId, int cargoWeapon, int unitIndex, int boolCenterInTile);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGameEx_ReloadSheets();
		[DllImport("DotNetInterop.dll")] private static extern void TethysGameEx_LoadTechTree(string fileName, int maxTechLevel);
	
		[DllImport("DotNetInterop.dll")] private static extern int TethysGameEx_NumHumanPlayers();
	
		[DllImport("DotNetInterop.dll")] private static extern void TethysGameEx_SetCheatFastProductionEx(int boolOn);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGameEx_SetCheatFastUnitsEx(int boolOn);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGameEx_SetCheatProduceAllEx(int boolOn);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGameEx_SetCheatUnlimitedResourcesEx(int boolOn);
	
		[DllImport("DotNetInterop.dll")] private static extern void TethysGameEx_SetShowVehicleRoutes(int boolOn);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGameEx_SetEnableMoraleLog(int boolOn);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGameEx_SetDamage4X(int boolOn);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGameEx_SetRCCEffect(RCCEffectState setting);
	
		[DllImport("DotNetInterop.dll")] private static extern void TethysGameEx_SetTopStatusBar(string text);
		[DllImport("DotNetInterop.dll")] private static extern void TethysGameEx_SetBottomStatusBar(string text, uint color);
	
		//extern EXPORT int __stdcall TethysGameEx_MsgBox(const char* text, const char* caption, unsigned int type)
	
		[DllImport("DotNetInterop.dll")] private static extern void TethysGameEx_ResetCheatedGame();
	}
}
