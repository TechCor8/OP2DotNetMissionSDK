// TethysGameEx.cpp
#include "HFL.h"

#pragma pack(push,1)
struct OP2Game
{
	int unlimitedRes;
	int produceAll;
	int enableMoraleLog;
	int boolDamage4x;
	int fastUnits;
	int fastProduction;
	short unknown;
	short unitRoutes;
	int unknown2[7];
	int disableRcc;
	int forceRcc;
	int unknown3[3];
	int boolDaylightEverywhere;
	int unknown4[6];
	int numPlayers;
	int numHumanPlayers;
	int unknown5[5];
	int tick;
	int tickOfLastSetGameOpt;
	int unknown6;
	int localPlayer;
	int unknown7[4];
	int flags;
	int unknown8[3];
};
#pragma pack(pop)

TethysGameEx gGame;
void *gameObj;
void *researchObj;
void *frameObj;
void *statusBarObj;

int TethysGameEx::CreateUnitEx(map_id unitType, int pixelX, int pixelY, int creatorId, int cargoWeapon, int unitIndex, int boolCenterInTile)
{
	// todo: move to UnitInfo.cpp some time?

	if (!isInited)
		return HFLNOTINITED;

	int (__fastcall *func)(void *classPtr, int dummy, int unitType, int pixelX, int pixelY, int creatorId, int cargoWeapon, int unitIndex, int centerInTile) = (int (__fastcall *)(void*,int,int,int,int,int,int,int,int))(imageBase + 0x467C0);

	return func(unitInfoObj, 0, unitType, pixelX, pixelY, creatorId, cargoWeapon, unitIndex, boolCenterInTile);
}

void TethysGameEx::ReloadSheets()
{
	if (!isInited)
		return;

	void (__fastcall *func)(void *classPtr) = (void (__fastcall *)(void*))(imageBase + 0x45180);

	func(unitInfoObj);
}

void TethysGameEx::LoadTechtree(char *fileName, int maxTechLevel)
{
	if (!isInited)
		return;

	void (__fastcall *func)(void *classPtr, int dummy, char *fileName, int maxTechLevel) = (void (__fastcall *)(void*,int,char*,int))(imageBase + 0x73470);

	func(researchObj, 0, fileName, maxTechLevel);
}

int TethysGameEx::NumHumanPlayers()
{
	if (!isInited)
		return HFLNOTINITED;

	OP2Game *p = (OP2Game*)gameObj;

	return p->numHumanPlayers;
}

void TethysGameEx::SetCheatFastProductionEx(int boolOn)
{
	if (!isInited)
		return;

	OP2Game *p = (OP2Game*)gameObj;

	p->fastProduction = boolOn;
}

void TethysGameEx::SetCheatFastUnitsEx(int boolOn)
{
	if (!isInited)
		return;

	OP2Game *p = (OP2Game*)gameObj;

	p->fastUnits = boolOn;
}

void TethysGameEx::SetCheatProduceAllEx(int boolOn)
{
	if (!isInited)
		return;

	OP2Game *p = (OP2Game*)gameObj;

	p->fastUnits = boolOn;
}

void TethysGameEx::SetCheatUnlimitedResourcesEx(int boolOn)
{
	if (!isInited)
		return;

	OP2Game *p = (OP2Game*)gameObj;

	p->unlimitedRes = boolOn;
}

void TethysGameEx::SetShowVehicleRoutes(int boolOn)
{
	if (!isInited)
		return;

	OP2Game *p = (OP2Game*)gameObj;

	p->unitRoutes = boolOn;
}

void TethysGameEx::SetEnableMoraleLog(int boolOn)
{
	if (!isInited)
		return;

	OP2Game *p = (OP2Game*)gameObj;

	p->enableMoraleLog = boolOn;
}

void TethysGameEx::SetDamage4X(int boolOn)
{
	if (!isInited)
		return;

	OP2Game *p = (OP2Game*)gameObj;

	p->boolDamage4x = boolOn;
}

void TethysGameEx::SetRCCEffect(RCCEffectState setting)
{
	if (!isInited)
		return;

	OP2Game *p = (OP2Game*)gameObj;

	switch (setting)
	{
	case rccNormal:
	default:
		p->disableRcc = 0;
		p->forceRcc = 0;
		break;
	case rccDisable:
		p->disableRcc = 1;
		p->forceRcc = 0;
		break;
	case rccForce:
		p->disableRcc = 0;
		p->forceRcc = 1;
		break;
	}
}

void TethysGameEx::SetTopStatusBar(char *text)
{
	if (!isInited)
		return;

	void (__fastcall *func)(void *classPtr, int dummy, char *text, int unknown) = (void (__fastcall *)(void*,int,char*,int))(imageBase + 0x11CE0);

	func(statusBarObj, 0, text, 0);
}

void TethysGameEx::SetBottomStatusBar(char *text, COLORREF color)
{
	if (!isInited)
		return;

	void (__fastcall *func)(void *classPtr, int dummy, char *text, COLORREF color) = (void (__fastcall *)(void*,int,char*,COLORREF))(imageBase + 0x9BFE0);

	func(frameObj, 0, text, color);
}

int TethysGameEx::MsgBox(HWND hwnd, LPCTSTR lpText, LPCTSTR lpCaption, UINT uType)
{
	if (!isInited)
		return HFLNOTINITED;

	int (__fastcall *func)(HWND hwnd, LPCTSTR lpText, LPCTSTR lpCaption, UINT uType) = (int (__fastcall *)(HWND,LPCTSTR,LPCTSTR,UINT))(imageBase + 0x1E0E0);

	return func(hwnd, lpText, lpCaption, uType);
}

void TethysGameEx::ResetCheatedGame()
{
	if (!isInited)
		return;

	OP2Game *p = (OP2Game*)gameObj;

	p->tickOfLastSetGameOpt = 0;
}
