#pragma once

// Extensions to TethysGame
class TethysGameEx : public TethysGame
{
public:
	static int CreateUnitEx(map_id unitType, int pixelX, int pixelY, int creatorId, int cargoWeapon, int unitIndex, int boolCenterInTile);
	static void ReloadSheets();
	static void LoadTechtree(char *fileName, int maxTechLevel);
	static int NumHumanPlayers();
	static void SetCheatFastProductionEx(int boolOn);
	static void SetCheatFastUnitsEx(int boolOn);
	static void SetCheatProduceAllEx(int boolOn);
	static void SetCheatUnlimitedResourcesEx(int boolOn);
	static void SetShowVehicleRoutes(int boolOn);
	static void SetEnableMoraleLog(int boolOn);
	static void SetDamage4X(int boolOn);
	static void SetRCCEffect(RCCEffectState setting);
	static void SetTopStatusBar(char *text);
	static void SetBottomStatusBar(char *text, COLORREF color);
	static int MsgBox(HWND hwnd, LPCTSTR lpText, LPCTSTR lpCaption, UINT uType);
	static void ResetCheatedGame();

	// todo: perhaps sound, music functions? as well as SetGameOpt
};

extern TethysGameEx gGame;
extern void *gameObj;

//extern void *researchObj; // TODO: move to research file when research functions are written

extern void *frameObj;
extern void *statusBarObj;
