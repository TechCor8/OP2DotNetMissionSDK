#include "stdafx.h"
#include "CppInterface.h"
#include "DotNetMissionSDK.h"

namespace DotNetInterop
{
	// Singleton class for accessing DotNetMissionEntry
	public ref class DotNetMissionDLL
	{
		static Assembly^ m_Assembly;
		
	public:
		// Interface for Mission Entry in the C# mission DLL
		delegate bool AttachDelegate(String^ dllPath);
		delegate bool InitializeDelegate();
		delegate void UpdateDelegate();
		delegate IntPtr GetSaveBufferDelegate();
		delegate int GetSaveBufferLengthDelegate();
		delegate void DetachDelegate();

	public:
		static Object^ Instance;

		static AttachDelegate^ Attach;
		static InitializeDelegate^ Initialize;
		static UpdateDelegate^ Update;
		static GetSaveBufferDelegate^ GetSaveBuffer;
		static GetSaveBufferLengthDelegate^ GetSaveBufferLength;
		static DetachDelegate^ Detach;

		// Attach creates the app domain, loads the assembly and creates the entry point
		static bool Load(String^ dotNetPath)
		{
			// Load app domain and assembly
			m_Assembly = Assembly::LoadFile(dotNetPath);

			// Create entry point
			Type^ entryType = m_Assembly->GetType("DotNetMissionSDK.DotNetMissionEntry");

			Instance = Activator::CreateInstance(entryType);

			MethodInfo^ methodAttach = entryType->GetMethod("Attach");
			MethodInfo^ methodInitialize = entryType->GetMethod("Initialize");
			MethodInfo^ methodUpdate = entryType->GetMethod("Update");
			MethodInfo^ methodGetSaveBuffer = entryType->GetMethod("GetSaveBuffer");
			MethodInfo^ methodGetSaveBufferLength = entryType->GetMethod("GetSaveBufferLength");
			MethodInfo^ methodDetach = entryType->GetMethod("Detach");

			Attach				= (AttachDelegate^)				methodAttach->CreateDelegate(AttachDelegate::typeid, Instance);
			Initialize			= (InitializeDelegate^)			methodInitialize->CreateDelegate(InitializeDelegate::typeid, Instance);
			Update				= (UpdateDelegate^)				methodUpdate->CreateDelegate(UpdateDelegate::typeid, Instance);
			GetSaveBuffer		= (GetSaveBufferDelegate^)		methodGetSaveBuffer->CreateDelegate(GetSaveBufferDelegate::typeid, Instance);
			GetSaveBufferLength = (GetSaveBufferLengthDelegate^)methodGetSaveBufferLength->CreateDelegate(GetSaveBufferLengthDelegate::typeid, Instance);
			Detach				= (DetachDelegate^)				methodDetach->CreateDelegate(DetachDelegate::typeid, Instance);
			
			// Check if everything is valid
			if (Instance == nullptr)			return false;
			if (Attach == nullptr)				return false;
			if (Initialize == nullptr)			return false;
			if (Update == nullptr)				return false;
			if (GetSaveBuffer == nullptr)		return false;
			if (GetSaveBufferLength == nullptr)	return false;
			if (Detach == nullptr)				return false;

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
		return DotNetMissionDLL::Attach(strDllPath);
	}

	bool Initialize()
	{
		return DotNetMissionDLL::Initialize();
	}

	void Update()
	{
		DotNetMissionDLL::Update();
	}

	void* GetSaveBuffer()
	{
		return (void*)DotNetMissionDLL::GetSaveBuffer();
	}

	int GetSaveBufferLength()
	{
		return DotNetMissionDLL::GetSaveBufferLength();
	}

	void Detach()
	{
		DotNetMissionDLL::Detach();
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