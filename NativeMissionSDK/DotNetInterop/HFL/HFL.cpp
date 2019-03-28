// HFL version 1.0 header file
#include "stdafx_unmanaged.h"

#include <HFL/Source/HFL.h>

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif


extern "C"
{
	extern EXPORT int __stdcall HFL_Init()
	{
		return HFLInit();
	}

	extern EXPORT int __stdcall HFL_Cleanup()
	{
		return HFLCleanup();
	}
}