# OP2DotNetMissionSDK
.Net Mission SDK for Outpost 2


## OVERVIEW
DotNetMissionSDK is a project to move Outpost 2 scenario development from C++ to C#, and finally to JSON where it can be used in an external editor. The DotNetMissionSDK will contain common functionality that will be available through the JSON data file, such as "real AI" and custom triggers.


## TECHNICAL OVERVIEW
DotNetMissionSDK intentionally avoids the use of Windows COM. To do this, the project has to jump through a few hoops. Starting from the unmanaged scenario DLL called by Outpost 2, the DLL calls the managed C++ CLR project DLL, which in turn calls the Managed C# DotNetMissionSDK DLL, which then calls the Native interface.

The DLL flow looks like this:
NativePlugin (cMissionName.dll) > DotNetInterop.dll > DotNetMissionSDK.dll (Mission Code and JSON reader) > NativeInterop > Outpost2.dll

### NativePlugin
This contains the scenario details which can't be forwarded to the C# DLL due to how it is loaded at app startup.
It forwards Init, AIProc, and GetSaveRegions to the CLR.
Also forwards Attach to the CLR with the DLLs name for determining the JSON filename to call.

### DotNetInterop
Contains Init, AIProc, GetSaveRegions and Attach interface for the NativePlugin and forwards them to the DotNetMissionSDK.

### DotNetMissionSDK
Calls JSON MissionReader and executes all mission logic.
Calls NativeInterop to interact with Outpost 2.

### NativeInterop
Forwards Outpost 2 DLL functions called from DotNetMissionSDK.


## CREATING A NEW SCENARIO IN JSON
If you want to avoid coding all together, copy the following files from the DotNetMissionSDK/Outpost2 subdirectory to your Outpost 2 directory:

cTest.dll\
DotNetInterop.dll\
DotNetMissionSDK.dll\
NativeInterop.dll\
cTest.opm

You can change the cTest.opm JSON file in any text editor.

At this time, to change mission details, you must open the dll in a hex editor and change the description, tech tree, mission type, etc. The number of bytes must not change or the indexes will be wrong and the DLL will not load!
It may be easier to simply change those values in C++ and recompile.
In the future, a scenario editor should be able to do this for you.

The name of the native plugin (cTest.dll) must match the name of the JSON file (cTest.opm). You can create multiple missions by changing these together.


## CREATING A NEW SCENARIO IN C#
If you are a programmer, you can dive right into the C# code. There are few things to keep in mind.

Mission details are set in the NativePlugin.

To have the interop load your custom C# mission DLL, in NativePlugin.LevelMain.cpp, set USE_CUSTOM_DLL to true. This will make the interop load the C# DLL using the native plugin name as the base: {NativePluginName}\_DotNet.dll

For example: cTest.DLL (native) > DotNetInterop.dll > cTest_DotNet.dll


## Native vs Managed SDK differences
Triggers work differently compared to the native SDK. All triggers must be registered to the TriggerManager. TriggerManager will execute a C# event when a trigger has fired, passing in the TriggerStub that was returned when the trigger was created. This stub is how you identify the trigger that has fired.
