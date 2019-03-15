#include "stdafx_unmanaged.h"

#include <Outpost2DLL/Outpost2DLL.h>	// Main Outpost 2 header to interface with the game

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif

// Note: ScStub is the parent class of Triggers, Groups, and Pinwheel classes.
//		 All functions in this class are available to derived classes
// Note: Do not try to create an instance of this class. It was meant
//		 simply as a base parent class from which other classes inherit
//		 functions from. Creating an instance of this class serves little
//		 (or no) purpose and may even crash the game.

extern "C"
{
		// Methods
	extern EXPORT void __stdcall ScStub_Destroy(int stubIndex)
	{
		ScStub stub;
		stub.stubIndex = stubIndex;

		stub.Destroy();
	}
	extern EXPORT void __stdcall ScStub_Disable(int stubIndex)
	{
		ScStub stub;
		stub.stubIndex = stubIndex;

		stub.Disable();
	}
	extern EXPORT void __stdcall ScStub_Enable(int stubIndex)
	{
		ScStub stub;
		stub.stubIndex = stubIndex;

		stub.Enable();
	}
	// [Get]
	extern EXPORT int __stdcall ScStub_Id(int stubIndex)
	{
		ScStub stub;
		stub.stubIndex = stubIndex;

		return stub.Id();
	}
	extern EXPORT int __stdcall ScStub_IsEnabled(int stubIndex)
	{
		ScStub stub;
		stub.stubIndex = stubIndex;

		return stub.IsEnabled();
	}
	extern EXPORT int __stdcall ScStub_IsInitialized(int stubIndex)
	{
		ScStub stub;
		stub.stubIndex = stubIndex;

		return stub.IsInitialized();
	}
	// [Set]
	//extern EXPORT void __stdcall ScStub_SetId(int stubIndex)
	//{
	//}
}
