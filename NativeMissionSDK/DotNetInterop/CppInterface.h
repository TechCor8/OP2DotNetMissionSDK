// Include this file in native code to interface with the .NET library
#pragma once

#include <string>

// Sets the interface function's decoration as export or import
#ifdef DOTNET_EXPORTS 
#define EXPORT_SPEC __declspec( dllexport )
#else
#define EXPORT_SPEC __declspec( dllimport )
#endif

namespace DotNetInterop
{
	// Calls .NET library Attach.
	// Called when DLL is loaded.
	// Returns true on success.
	EXPORT_SPEC bool Attach(const char* dllPath);

	// Calls .NET library Initialize.
	// Called when the mission is first loaded.
	// Returns true on success.
	EXPORT_SPEC bool Initialize();

	// Calls .NET library Update.
	// Called every update cycle.
	EXPORT_SPEC void Update();

	// Used to get a buffer for storing data for saved game files
	EXPORT_SPEC void* GetSaveBuffer();
	EXPORT_SPEC int GetSaveBufferLength();

	//EXPORT_SPEC std::string GetDisplayString(const char * pName, int iValue);
}