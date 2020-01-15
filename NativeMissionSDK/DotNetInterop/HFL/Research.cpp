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

	extern EXPORT int __stdcall TechInfo_IsValid(OP2TechInfo* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.IsValid();
	}
	extern EXPORT int __stdcall TechInfo_GetTechID(OP2TechInfo* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetTechID();
	}
	extern EXPORT TechCategory __stdcall TechInfo_GetCategory(OP2TechInfo* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetCategory();
	}
	extern EXPORT int __stdcall TechInfo_GetTechLevel(OP2TechInfo* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetTechLevel();
	}
	extern EXPORT int __stdcall TechInfo_GetPlymouthCost(OP2TechInfo* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetPlymouthCost();
	}
	extern EXPORT int __stdcall TechInfo_GetEdenCost(OP2TechInfo* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetEdenCost();
	}
	extern EXPORT int __stdcall TechInfo_GetMaxScientists(OP2TechInfo* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetMaxScientists();
	}
	extern EXPORT LabType __stdcall TechInfo_GetLab(OP2TechInfo* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetLab();
	}
	extern EXPORT int __stdcall TechInfo_GetPlayerHasTech(OP2TechInfo* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetPlayerHasTech();
	}
	extern EXPORT int __stdcall TechInfo_GetNumUpgrades(OP2TechInfo* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetNumUpgrades();
	}
	extern EXPORT int __stdcall TechInfo_GetNumRequiredTechs(OP2TechInfo* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetNumRequiredTechs();
	}
	extern EXPORT char* __stdcall TechInfo_GetTechName(OP2TechInfo* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetTechName();
	}
	extern EXPORT char* __stdcall TechInfo_GetDescription(OP2TechInfo* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetDescription();
	}
	extern EXPORT char* __stdcall TechInfo_GetTeaser(OP2TechInfo* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetTeaser();
	}
	extern EXPORT char* __stdcall TechInfo_GetImproveDesc(OP2TechInfo* handle)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetImproveDesc();
	}
	extern EXPORT int __stdcall TechInfo_GetRequiredTechIndex(OP2TechInfo* handle, int index)
	{
		TechInfo info;
		info.internalPtr = handle;

		return info.GetRequiredTechIndex(index);
	}
}
