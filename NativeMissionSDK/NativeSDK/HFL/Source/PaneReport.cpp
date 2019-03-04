// PaneReport.cpp
#include "HFL.h"
#include <stdarg.h> // var args on CreateStdButtons

// forward declare
struct OP2Report;

#pragma pack(push,1)
struct OP2ReportVtbl
{
	void (__fastcall *Update)(OP2Report *classPtr);
	void (__fastcall *Init)(OP2Report *classPtr);
	void (__fastcall *unknown1)(OP2Report *classPtr);
	int (__fastcall *unknown2)(OP2Report *classPtr);
	void (__fastcall *Paint)(OP2Report *classPtr, int dummy, RECT *updateRect, void *gfxSurface);
	void (__fastcall *unknown3)(OP2Report *classPtr);
	int (__fastcall *unknown4)(OP2Report *classPtr);
	void (__fastcall *unknown5)(OP2Report *classPtr);
	int (__fastcall *GetButtonId)(OP2Report *classPtr);
	// .. more but we can't use those till the List control is understood
};

struct OP2Report
{
	OP2ReportVtbl *vtbl;
	int unknown[112];
	int curPage;
	int fillingSpace[10]; // maybe some more stuff here, be safe
	PaneReport *rptPtr; // our PaneReport
	// not totally understood yet
};
#pragma pack(pop)

// Dispatchers. See PaneButton.cpp for more details on these
void __fastcall ReportUpdateDispatcher(OP2Report *classPtr)
{
	PaneReport *p = classPtr->rptPtr;

	if (p)
		p->Update();

	// call default implementation
	void (__fastcall *func)(OP2Report *classPtr) = (void (__fastcall *)(OP2Report*))(imageBase + 0x59F10);
	func(classPtr);
}

void __fastcall ReportInitDispatcher(OP2Report *classPtr)
{
	PaneReport *p = classPtr->rptPtr;

	if (p)
		p->Initialize();

	// call default implementation
	void (__fastcall *func)(OP2Report *classPtr) = (void (__fastcall *)(OP2Report*))(imageBase + 0x59F20);
	func(classPtr);
}

void __fastcall ReportPaintDispatcher(OP2Report *classPtr, int dummy, RECT *updateRect, void *gfxSurface)
{
	PaneReport *p = classPtr->rptPtr;

	if (p)
		p->Paint(updateRect, PaneGFXSurface(gfxSurface));

	// there is no default implementation!
}

int __fastcall ReportGetButtonIdDispatcher(OP2Report *classPtr)
{
	PaneReport *p = classPtr->rptPtr;

	if (p)
		return p->GetLinkedButtonId();

	// call default implementation
	int (__fastcall *func)(OP2Report *classPtr) = (int (__fastcall *)(OP2Report*))(imageBase + 0x59F70);
	return func(classPtr);
}

PaneReport::PaneReport()
{
	isInternalObj = 0;
	internalRpt = NULL;
	internalVtbl = NULL;

	if (!isInited) // automatic initialization since objects in static storage will be constructed before InitProc is reached
	{
		if (HFLInit() == HFLCANTINIT)
		{
			return;
		}
	}

	// set up report object
	internalRpt = new OP2Report;

	OP2Report *p = (OP2Report*)internalRpt;
	memset(p, 0, sizeof(OP2Report));
	// save a pointer so the dispatcher function can redirect the call to the right object
	p->rptPtr = this;

	// set up vtbl
	internalVtbl = new OP2ReportVtbl;

	OP2ReportVtbl *vp = (OP2ReportVtbl*)internalVtbl;
	p->vtbl = vp;

	// todo: some vtbl entries need changing for reports with lists
	vp->Update = ReportUpdateDispatcher;
	vp->Init = ReportInitDispatcher;
	vp->unknown1 = (void (__fastcall *)(OP2Report*))(imageBase + 0x59F30);
	vp->unknown2 = (int (__fastcall *)(OP2Report*))(imageBase + 0x59F40);
	vp->Paint = ReportPaintDispatcher;
	vp->unknown3 = (void (__fastcall *)(OP2Report*))(imageBase + 0x59F50);
	vp->unknown4 = (int (__fastcall *)(OP2Report*))(imageBase + 0x59F60);
	vp->unknown5 = (void (__fastcall *)(OP2Report*))(imageBase + 0x6D960);
	vp->GetButtonId = ReportGetButtonIdDispatcher;
}

PaneReport::PaneReport(void *internalPtr)
{
	isInternalObj = 0;
	internalRpt = NULL;
	internalVtbl = NULL;

	if (!isInited)
		return;

	int *p = (int*)internalPtr;

	internalRpt = internalPtr;
	internalVtbl = (void*)*p;
	isInternalObj = 1;
}

PaneReport::~PaneReport()
{
	// destroy objects.
	delete internalRpt;
	delete internalVtbl;
}

void PaneReport::Update()
{
	// nothing
}

void PaneReport::Initialize()
{
	// nothing
}

void PaneReport::Paint(RECT *updateRect, PaneGFXSurface gfxSurface)
{
	// nothing
}

int PaneReport::GetLinkedButtonId()
{
	// to be overridden...
	return 0;
}

void PaneReport::DrawTitle(RECT *updateRect, PaneGFXSurface gfxSurface, char *title)
{
	OP2Report *p = (OP2Report*)internalRpt;

	if (!p)
		return;

	void (__fastcall *func)(OP2Report *p, int dummy, RECT *updateRect, void *gfxSurface, char *title) = (void (__fastcall *)(OP2Report*,int,RECT*,void*,char*))(imageBase + 0x59CA0);
	func(p, 0, updateRect, gfxSurface.internalSurface, title);
}

void PaneReport::CreateStdButtons(int numButtons, PaneButton *button1, char *button1Label, char *button1HelpText, PaneButton *button2, char *button2Label, char *button2HelpText, PaneButton *button3, char *button3Label, char *button3HelpText, PaneButton *button4, char *button4Label, char *button4HelpText)
{
	// hack: since only up to 4 buttons are actually supported by the game, just
	// act as if the user passed parameters for all 4 buttons (the stack will balance out okay)
	// Performance hit but it will prevent having to deal with passing parameters manually

	OP2Report *p = (OP2Report*)internalRpt;

	if (!p)
		return;

	void *buttons[4] = {NULL, NULL, NULL, NULL};

	if (button1)
	{
		buttons[0] = button1->internalBtn;
		if (button2)
		{
			buttons[1] = button2->internalBtn;
			if (button3)
			{
				buttons[2] = button3->internalBtn;
				if (button4)
				{
					buttons[3] = button4->internalBtn;
				}
			}
		}
	}

	void (__cdecl *func)(OP2Report *p, int numButtons, void *button1, char *button1Label, char *button1HelpText, void *button2, char *button2Label, char *button2HelpText, void *button3, char *button3Label, char *button3HelpText, void *button4, char *button4Label, char *button4HelpText) = (void (__cdecl *)(OP2Report*,int,void*,char*,char*,void*,char*,char*,void*,char*,char*,void*,char*,char*))(imageBase + 0x58E50);
	func(p, numButtons, buttons[0], button1Label, button1HelpText, buttons[1], button2Label, button2HelpText, buttons[2], button3Label, button3HelpText, buttons[3], button4Label, button4HelpText);
}

int PaneReport::GetCurrentPage()
{
	OP2Report *p = (OP2Report*)internalRpt;

	if (!p)
		return HFLNOTINITED;

	return p->curPage;
}

void PaneReport::SetCurrentPage(int pageNum)
{
	OP2Report *p = (OP2Report*)internalRpt;

	if (!p)
		return;

	p->curPage = pageNum;
}
