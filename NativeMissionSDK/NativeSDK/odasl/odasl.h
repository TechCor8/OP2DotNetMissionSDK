// ODASL.DLL public interface.
// ODASL is the Sierra library that implements the ownerdraw in Outpost 2.

#ifndef ODASL_H
#define ODASL_H

#include <windows.h>

// Structure used to pass information to ODASL in wplInit.
struct wplOptions
{
	ULONG structSize; // 60
	ULONG unknown1; // 87 // Flags?
	HINSTANCE hAppInst; // HINSTANCE OP2Shell.dll
	HINSTANCE hResDllInst; // HINSTANCE op2shres.dll
	ULONG startResId; // 200 // Starting resource id?
	ULONG unknown3; // 0 // Palette?
	ULONG unknown4; // 0 // More flags?
	ULONG unknown5; // 96 // Size scale? Large values make windows huge
	ULONG unknown[7]; // All 0
};

#ifdef BUILDING_ODASL
#define ODASL_API(rt) __declspec(dllexport) rt __cdecl
#else
#define ODASL_API(rt) __declspec(dllimport) rt __cdecl
#endif // BUILDING_ODASL

#ifdef __cplusplus
extern "C" {
#endif // __cplusplus

ODASL_API(int) wplInit(wplOptions *inf);
ODASL_API(void) wplExit();
ODASL_API(void) wplSetPalette(HPALETTE pal);
ODASL_API(int) wplGetSystemMetrics(int nIndex);
ODASL_API(BOOL) wplAdjustWindowRect(LPRECT lpRect, DWORD dwStyle, BOOL bMenu);
ODASL_API(HBITMAP) wplLoadResourceBitmap(HMODULE hModule, LPCTSTR lpName);
ODASL_API(int) wplManualDialogSubclass(HWND dlg);
ODASL_API(void) wplEnable();
ODASL_API(void) wplDisable();


#ifdef __cplusplus
}
#endif // __cplusplus

#endif // ODASL_H