#include "stdafx.h"
#include "CppInterface.h"
#include "DotNetMissionSDK.h"

// NOTE: Currently this is set up to use a static reference to DotNetMissionSDK.dll.
// This is fine for the intended purpose of reading scenario data from a data file for use with an editor.
// However, if you want to hardcode missions in C#, you will need to make the reference C# dll unique for reach mission and have DotNetInterop.dll point to it.
// You can accomplish this by:
// A. Duplicating DotNetInterop with the updated reference name for DotNetMissionSDK.dll and updating the reference name for NativePlugin.dll (cMissionName.dll) to DotNetInterop.dll.
//    This creates unnecessary duplicates of DotNetInterop.
//    For example: cMyMission.dll > cMyMission_Interop.dll > cMyMission_DotNet.dll
//                 cMyMission2.dll > cMyMission2_Interop.dll > cMyMission2_DotNet.dll
// B. Changing this file to load DotNetMissionSDK.dll dynamically, by looking at the name of the NativePlugin.dll (which already must be named according to Outpost2 requirements),
//    and loading the C# dll using that name. The native plugin (cMyMission.dll), would not need to be recompiled for each mission, only renamed.
//    For example: cMyMission.dll > DotNetInterop.dll > cMyMission_DotNet.dll
//                 cMyMission2.dll > DotNetInterop.dll > cMyMission2_DotNet.dll
// C. The real purpose is to use data files for mission scripting, and C# will contain common logic used by all missions. No libraries would need to be recompiled.
//    For example: cMyMission.dll > DotNetInterop.dll > DotNetMissionSDK.dll > cMyMission.opm
//                 cMyMission2.dll > DotNetInterop.dll > DotNetMissionSDK.dll > cMyMission2.opm

namespace DotNetInterop
{
	bool Attach(const char* dllPath)
	{
		DotNetMissionEntry^ lib = gcnew DotNetMissionEntry();

		return lib->Attach(ToManagedString(dllPath));
	}

	bool Initialize()
	{
		DotNetMissionEntry^ lib = gcnew DotNetMissionEntry();

		return lib->Initialize();
	}

	void Update()
	{
		DotNetMissionEntry^ lib = gcnew DotNetMissionEntry();

		lib->Update();
	}

	void* GetSaveBuffer()
	{
		DotNetMissionEntry^ lib = gcnew DotNetMissionEntry();

		return (void*)lib->GetSaveBuffer();
	}

	int GetSaveBufferLength()
	{
		DotNetMissionEntry^ lib = gcnew DotNetMissionEntry();

		return lib->GetSaveBufferLength();
	}
}


/*
std::string GetDisplayString(const char * pName, int iValue) {
 CsharpClass ^ oCsharpObject = gcnew CsharpClass();

 oCsharpObject->Name = ToManagedString(pName);
 oCsharpObject->Value = iValue;

 return ToStdString(oCsharpObject->GetDisplayString());
}


public ref class CLRToDotNet
{
public:
	bool Initialize()
	{
		DotNetLib^ lib = gcnew DotNetLib();
		bool result = lib->Initialize();
		return result;
	}
};*/