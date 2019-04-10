// Research.h
// Classes for modifying Research structures
#include "stdafx_unmanaged.h"

#include <HFL/Source/HFL.h>

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif


extern "C"
{
	extern EXPORT int __stdcall Research_GetTechCount()
	{
		return Research::GetTechCount();
	}
	extern EXPORT void* __stdcall Research_GetTechInfo(int index)
	{
		TechInfo info = Research::GetTechInfo(index);
		return info.internalPtr;
	}
	extern EXPORT int __stdcall Research_GetMaxTechID()
	{
		return Research::GetMaxTechID();
	}

	extern EXPORT int __stdcall TechInfo_IsValid(void* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.IsValid();
	}
	extern EXPORT int __stdcall TechInfo_GetTechID(void* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetTechID();
	}
	extern EXPORT TechCategory __stdcall TechInfo_GetCategory(void* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetCategory();
	}
	extern EXPORT int __stdcall TechInfo_GetTechLevel(void* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetTechLevel();
	}
	extern EXPORT int __stdcall TechInfo_GetPlymouthCost(void* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetPlymouthCost();
	}
	extern EXPORT int __stdcall TechInfo_GetEdenCost(void* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetEdenCost();
	}
	extern EXPORT int __stdcall TechInfo_GetMaxScientists(void* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetMaxScientists();
	}
	extern EXPORT LabType __stdcall TechInfo_GetLab(void* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetLab();
	}
	extern EXPORT int __stdcall TechInfo_GetPlayerHasTech(void* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetPlayerHasTech();
	}
	extern EXPORT int __stdcall TechInfo_GetNumUpgrades(void* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetNumUpgrades();
	}
	extern EXPORT int __stdcall TechInfo_GetNumRequiredTechs(void* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetNumRequiredTechs();
	}
	extern EXPORT char* __stdcall TechInfo_GetTechName(void* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetTechName();
	}
	extern EXPORT char* __stdcall TechInfo_GetDescription(void* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetDescription();
	}
	extern EXPORT char* __stdcall TechInfo_GetTeaser(void* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetTeaser();
	}
	extern EXPORT char* __stdcall TechInfo_GetImproveDesc(void* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetImproveDesc();
	}
}
