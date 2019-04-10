// HFL version 1.0 header file
#ifndef _HFL_H_
#define _HFL_H_

// Other headers first
#define WIN32_LEAN_AND_MEAN
#include <windows.h>

#include "../../Outpost2DLL/Outpost2DLL.h"

// Include other HFL related headers
#include "HFLTypes.h"
#include "UnitInfo.h"
#include "UnitEx.h"
#include "PlayerEx.h"
#include "TriggerEx.h"
#include "GameMapEx.h"
#include "TethysGameEx.h"
#include "Research.h"
#include "PaneGFXSurface.h"
#include "PaneReport.h"
#include "PaneButton.h"
#include "StdPanes.h"

// Global functions
int HFLInit();
int HFLCleanup();

extern bool isInited;
extern DWORD imageBase;

#endif // _HFL_H_