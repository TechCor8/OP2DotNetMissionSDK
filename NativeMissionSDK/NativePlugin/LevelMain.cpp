#include <Outpost2DLL/Outpost2DLL.h>		// Main Outpost 2 header to interface with the game

#include "../DotNetInterop/CppInterface.h"	// Header for calling C# functions

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>						// Required for DLLMain


// Note: These exports are required by Outpost2.exe from every level
//		 DLL. They give values for the map and tech trees used by the
//		 level and a description to place in the level listbox. The
//		 last export is used to define characteristics of the level.
//		 See RequiredExports.h for more details.
//		 ** Be sure to set these when you build your own level**
// Required data exports  (Description, Map, TechTree, GameType, NumPlayers)
ExportLevelDetails("6P, LoS, '<map name>'", "on6_01.map", "MULTITEK.TXT", Colony, 6)
// Alternative style:
// Required data exports  (Description, Map, TechTree, GameType, NumPlayers, maxTechLevel, bUnitOnlyMission)
//ExportLevelDetailsEx("6P, LoS, '<map name>'", "on6_01.map", "MULTITEK.TXT", MultiLastOneStanding, 6, 12, false)


Export void __cdecl GetSaveRegions(struct BufferDesc &bufDesc)
{
	bufDesc.bufferStart = DotNetInterop::GetSaveBuffer();
	bufDesc.length = DotNetInterop::GetSaveBufferLength();
}


// Note: The following function is called once by Outpost2.exe when the
//		 level is first initialized. This is where you want to create
//		 all the initial units and structures as well as setup any 
//		 map/level environment settings such as day and night.
// Note: Returns true if level loaded successfully and is playable, false to abort
Export int InitProc()
{
	return DotNetInterop::Initialize();
}


// Note: The following function seems to be intended for use in
//		 controlling an AI. It is called once every game cycle. 
//		 Use it for whatever code needs to run on a continual basis.
// Note: The standard level DLLs released by Sierra leave this function
//		 empty and handle all AI controls through triggers.
Export void AIProc() 
{
	DotNetInterop::Update();
}


// Note: This is a trigger callback function. This function is
//		 intentionally left empty and is used as the trigger
//		 callback function for triggers that don't want or need
//		 any special callback function.
// Note: The use of Export is used by all trigger functions
//		 to ensure they are exported correctly.
Export void NoResponseToTrigger()
{
}

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
	switch (fdwReason)
	{
	case DLL_PROCESS_ATTACH:
		int len = MAX_PATH;
		LPSTR filePath = new char[len];

		// Get DLL path
		while (GetModuleFileName(hinstDLL, filePath, len) == len)
		{
			delete[] filePath;
			len += MAX_PATH;
			filePath = new char[len];
		}

		bool result = DotNetInterop::Attach(filePath);

		delete[] filePath;

		return result;

		break;
	}

	return true;
}

