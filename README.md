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

cTest.dll
DotNetInterop.dll
DotNetMissionSDK.dll
NativeInterop.dll
cTest.opm

You can change the cTest.opm JSON file in any text editor.

At this time, to change mission details, you must open the dll in a hex editor and change the description, tech tree, mission type, etc. The number of bytes must not change or the indexes will be wrong and the DLL will not load!
It may be easier to simply change those values in C++ and recompile.
In the future, a scenario editor should be able to do this for you.

The name of the native plugin (cTest.dll) must match the name of the JSON file (cTest.opm). You can create multiple missions by changing these together.


## CREATING A NEW SCENARIO IN C#
If you are a programmer, you can dive right into the C# code. There are few things to  keep in mind:

Triggers don't work the same. You could forward every trigger function from the native plugin, but that would be tedious. It is better to register the trigger with the MissionLogic update loop (Not Yet Implemented) and check if the trigger has fired.

Mission details are set in the NativePlugin.

You will need to change DotNetInterop.dll to dynamically load the MissionSDK and remove the static reference. Load the assembly using the name provided by Attach() and use reflection to grab the functions to call. Make sure your C# DLL is named the same as the native plugin with some suffix.

For example: cTest.DLL (native) > DotNetInterop.dll > cTest_DotNet.dll

NOTE: The code to dynamically load C# DLLs is not available in the interop project yet. Eventually this code will be added and commented out for ease of use.
