// Contains marshaling help functions and the mission SDK namespace.
#pragma once

#include <string>

using namespace System;
using namespace System::IO;
using namespace System::Reflection;
using namespace System::Runtime::InteropServices;

using namespace DotNetMissionSDK;


namespace DotNetInterop
{
	// String Conversion Functions

	inline String^ ToManagedString(const char * pString)
	{
		return Marshal::PtrToStringAnsi(IntPtr((char *)pString));
	}

	inline const std::string ToStdString(String ^ strString)
	{
		IntPtr ptrString = IntPtr::Zero;
		std::string strStdString;
		
		try
		{
			ptrString = Marshal::StringToHGlobalAnsi(strString);
			strStdString = (char *)ptrString.ToPointer();
		}
		finally
		{
			if (ptrString != IntPtr::Zero)
			{
				Marshal::FreeHGlobal(ptrString);
			}
		}
		return strStdString;
	}
}

