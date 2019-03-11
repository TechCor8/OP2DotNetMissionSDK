#include "stdafx.h"
#include "CppInterface.h"
#include "DotNetMissionSDK.h"

namespace DotNetInterop
{
	// Interface for Mission Entry in the C# mission DLL
	public interface class IMissionEntry
	{
		bool Attach(String^ dllPath);
		bool Initialize();
		void Update();
		IntPtr GetSaveBuffer();
		int GetSaveBufferLength();
		void Detach();
	};

	// Singleton class for accessing DotNetMissionEntry
	public ref class DotNetMissionDLL
	{
		static Assembly^ m_Assembly;
		static IMissionEntry^ m_Instance;

	public:
		static property IMissionEntry^ Instance { IMissionEntry^ get() { return m_Instance; } }

		// Attach creates the app domain, loads the assembly and creates the entry point
		static bool Load(String^ dotNetPath)
		{
			// Load app domain and assembly
			m_Assembly = Assembly::LoadFile(dotNetPath);

			// Create entry point
			m_Instance = (IMissionEntry^)m_Assembly->CreateInstance("DotNetMissionSDK.DotNetMissionEntry");

			if (m_Instance == nullptr)
				return false;

			return true;
		}
	};

	bool Attach(const char* dllPath, bool useCustomDLL)
	{
		String^ strDllPath = ToManagedString(dllPath);
		String^ dotNetPath;

		if (useCustomDLL)
		{
			// Custom mission DLL based on the native plugin name
			dotNetPath = Path::Combine(Path::GetDirectoryName(strDllPath), Path::GetFileNameWithoutExtension(strDllPath));
			dotNetPath += "_DotNet.dll";
		}
		else
		{
			// Common Interop DLL
			dotNetPath = Path::Combine(Path::GetDirectoryName(strDllPath), "DotNetMissionSDK.dll");
		}
		
		// Load DLL and create mission entry instance
		if (!DotNetMissionDLL::Load(dotNetPath))
			return false;
		
		// Call Attach() on DLL
		return DotNetMissionDLL::Instance->Attach(strDllPath);
	}

	bool Initialize()
	{
		return DotNetMissionDLL::Instance->Initialize();
	}

	void Update()
	{
		DotNetMissionDLL::Instance->Update();
	}

	void* GetSaveBuffer()
	{
		return (void*)DotNetMissionDLL::Instance->GetSaveBuffer();
	}

	int GetSaveBufferLength()
	{
		return DotNetMissionDLL::Instance->GetSaveBufferLength();
	}

	void Detach()
	{
		DotNetMissionDLL::Instance->Detach();
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