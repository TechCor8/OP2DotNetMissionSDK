// TethysGameEx.h
// Extensions to TethysGame
#include "stdafx_unmanaged.h"

#include <HFL/Source/HFL.h>

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif


extern "C"
{
	extern EXPORT int __stdcall TethysGameEx_CreateUnitEx(map_id unitType, int pixelX, int pixelY, int creatorId, int cargoWeapon, int unitIndex, int boolCenterInTile)
	{
		return TethysGameEx::CreateUnitEx(unitType, pixelX, pixelY, creatorId, cargoWeapon, unitIndex, boolCenterInTile);
	}

	extern EXPORT void __stdcall TethysGameEx_ReloadSheets()
	{
		TethysGameEx::ReloadSheets();
	}
	extern EXPORT void __stdcall TethysGameEx_LoadTechTree(char* fileName, int maxTechLevel)
	{
		TethysGameEx::LoadTechtree(fileName, maxTechLevel);
	}

	extern EXPORT int __stdcall TethysGameEx_NumHumanPlayers()
	{
		return TethysGameEx::NumHumanPlayers();
	}

	extern EXPORT void __stdcall TethysGameEx_SetCheatFastProductionEx(int boolOn)
	{
		return TethysGameEx::SetCheatFastProductionEx(boolOn);
	}
	extern EXPORT void __stdcall TethysGameEx_SetCheatFastUnitsEx(int boolOn)
	{
		return TethysGameEx::SetCheatFastUnitsEx(boolOn);
	}
	extern EXPORT void __stdcall TethysGameEx_SetCheatProduceAllEx(int boolOn)
	{
		return TethysGameEx::SetCheatProduceAllEx(boolOn);
	}
	extern EXPORT void __stdcall TethysGameEx_SetCheatUnlimitedResourcesEx(int boolOn)
	{
		return TethysGameEx::SetCheatUnlimitedResourcesEx(boolOn);
	}

	extern EXPORT void __stdcall TethysGameEx_SetShowVehicleRoutes(int boolOn)
	{
		return TethysGameEx::SetShowVehicleRoutes(boolOn);
	}
	extern EXPORT void __stdcall TethysGameEx_SetEnableMoraleLog(int boolOn)
	{
		return TethysGameEx::SetEnableMoraleLog(boolOn);
	}
	extern EXPORT void __stdcall TethysGameEx_SetDamage4X(int boolOn)
	{
		return TethysGameEx::SetDamage4X(boolOn);
	}
	extern EXPORT void __stdcall TethysGameEx_SetRCCEffect(RCCEffectState setting)
	{
		return TethysGameEx::SetRCCEffect(setting);
	}

	extern EXPORT void __stdcall TethysGameEx_SetTopStatusBar(const char* text)
	{
		// C# will delete string parameters, make a copy
		size_t len = strlen(text) + 1;
		char* copy = new char[len];
		strcpy_s(copy, len, text);

		return TethysGameEx::SetTopStatusBar(copy);
	}
	extern EXPORT void __stdcall TethysGameEx_SetBottomStatusBar(const char* text, unsigned int color)
	{
		// C# will delete string parameters, make a copy
		size_t len = strlen(text) + 1;
		char* copy = new char[len];
		strcpy_s(copy, len, text);

		return TethysGameEx::SetBottomStatusBar(copy, color);
	}

	// TODO: Implement this
	// Not sure how to get HWND for message box
	//extern EXPORT int __stdcall TethysGameEx_MsgBox(const char* text, const char* caption, unsigned int type)
	//{
	//	return TethysGameEx::MsgBox();
	//}

	extern EXPORT void __stdcall TethysGameEx_ResetCheatedGame()
	{
		return TethysGameEx::ResetCheatedGame();
	}
}
