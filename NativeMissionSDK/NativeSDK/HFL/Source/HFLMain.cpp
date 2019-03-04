// Main entry points, etc

#include "HFL.h"

// Global vars
bool isInited = false;
DWORD imageBase; // = 0x00400000;

int HFLInit()
{
	if (isInited)
		return HFLALREADYINITED;

	// get the image base
	imageBase = (DWORD)GetModuleHandle("Outpost2.exe");
	if (!imageBase)
		return HFLCANTINIT;

	unitArray = (OP2Unit**)(imageBase + 0x14F848);
	playerArray = (OP2Player*)(imageBase + 0x16EF1C);
	unitInfoArray = (OP2UnitInfo**)(imageBase + 0xE1348);

	unitInfoObj = (void*)(imageBase + 0x15B780);
	gameObj = (void*)(imageBase + 0x16EA98);
	researchObj = (void*)(imageBase + 0x16C230);
	frameObj = (void*)(imageBase + 0x1756C0);
	statusBarObj = (void*)(imageBase + 0x1761B0);

	graphicsObj = (void*)(imageBase + 0xEFD68);
	textRenderObj = (void*)(imageBase + 0x177EE8);
	mapObj = (void*)(imageBase + 0x14F7F8);
	mapTileData = (void*)(imageBase + 0x14FC5C);
	cmdPane = (void*)(imageBase + 0x175D18);

	isInited = true;
	return HFLLOADED;
}

int HFLCleanup()
{
	isInited = false;
	imageBase = -1;
	return 1;
}
