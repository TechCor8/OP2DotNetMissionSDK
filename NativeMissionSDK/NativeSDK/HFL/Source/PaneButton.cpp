#include "HFL.h"


struct OP2Button;

struct OP2ButtonVtbl
{
	void* (__fastcall *Destructor)(OP2Button *classPtr, int dummy, int unknown);
	void (__fastcall *Paint)(OP2Button *classPtr, int dummy, void *gfxSurface);
	LRESULT (__fastcall *OnUIEvent)(OP2Button *classPtr, int dummy, void *pane, UINT msg, WPARAM wParam, LPARAM lParam);
	void (__fastcall *OnAdd)(OP2Button *classPtr, int dummy, void *pane);
	void (__fastcall *OnRemove)(OP2Button *classPtr, int dummy, void *pane);
	void (__fastcall *SetEnabled)(OP2Button *classPtr, int dummy, int boolEnabled);
	void (__fastcall *unknown1)(OP2Button *classPtr, int dummy, int unknown);
	void (__fastcall *GetHelpText)(OP2Button *classPtr, int dummy, char *buffer, int bufferLen);
	void (__fastcall *OnClick)(OP2Button *classPtr);
};

#pragma pack(push, 1)
struct OP2ButtonData
{
	int animId;
	short normalFrameId;
	short activeFrameId;
	short disabledFrameId;
	short unknown1;
	char *helpText;
	char *buttonLabel;
	int unknown2[3];
};

struct OP2Button
{
	OP2ButtonVtbl *vtbl;
	unsigned int flags;
	RECT rect;
	int accelKey;
	int unknown[3];
	int hasButtonText; // set to 4 if the button has text drawn on it, 0 if not
	int unknown2[22];
	OP2ButtonData data;
	OP2Report *report; // all buttons are 'report buttons' but we won't use them that way necessarily
	int unknown3;
	int fillingSpace[10]; // maybe more stuff that OP2 depends on here..
	PaneButton *btnPtr; // our PaneButton
};
#pragma pack(pop)

// 'dispatcher' functions to redirect calls to different places.
// (and insulate the user defined classes / methods from harsh OP2)
// NOTE: don't use dummy! caller functions in OP2 may depend on the value of EDX not being altered.
// __fastcall convention alters EDX.
// todo (maybe): if crashes/oddness start happening, EDX will need to be saved before ASM function call
// and restored afterwards
void __fastcall ButtonPaintDispatcher(OP2Button *classPtr, int dummy, void *gfxSurface)
{
	PaneButton *pb = classPtr->btnPtr;

	if (pb) {
		pb->Paint(PaneGFXSurface(gfxSurface));
	}

	// call default implementation
	void (__fastcall *func)(OP2Button *classPtr, int dummy, void *gfxSurface) = (void (__fastcall *)(OP2Button*,int,void*))(imageBase + 0xAFD0);
	func(classPtr, 0, gfxSurface);
}

LRESULT __fastcall ButtonOnUIEventDispatcher(OP2Button *classPtr, int dummy, void *pane, UINT msg, WPARAM wParam, LPARAM lParam)
{
	PaneButton *pb = classPtr->btnPtr;

	if (pb) {
		pb->OnUIEvent(msg, wParam, lParam);
	}

	// call default implementation
	// (must always take place since it does things with filters and such that we don't)
	LRESULT (__fastcall *func)(OP2Button *classPtr, int dummy, void *pane, UINT msg, WPARAM wParam, LPARAM lParam) = (LRESULT (__fastcall *)(OP2Button*,int,void*,UINT,WPARAM,LPARAM))(imageBase + 0xAA50);
	return func(classPtr, 0, pane, msg, wParam, lParam);
}

void __fastcall ButtonOnAddDispatcher(OP2Button *classPtr, int dummy, void *pane)
{
	PaneButton *pb = classPtr->btnPtr;

	if (pb) {
		pb->OnAdd();
	}

	// call default implementation
	void (__fastcall *func)(OP2Button *classPtr, int dummy, void *pane) = (void (__fastcall *)(OP2Button*,int,void*))(imageBase + 0xA920);
	func(classPtr, 0, pane);
}

void __fastcall ButtonOnRemoveDispatcher(OP2Button *classPtr, int dummy, void *pane)
{
	PaneButton *pb = classPtr->btnPtr;

	if (pb) {
		pb->OnRemove();
	}

	// call default implementation
	void (__fastcall *func)(OP2Button *classPtr, int dummy, void *pane) = (void (__fastcall *)(OP2Button*,int,void*))(imageBase + 0xA950);
	func(classPtr, 0, pane);
}

void __fastcall ButtonSetEnabledDispatcher(OP2Button *classPtr, int dummy, int boolEnabled)
{
	PaneButton *pb = classPtr->btnPtr;

	if (pb) {
		pb->SetEnabled(boolEnabled);
	}

	// call default implementation
	void (__fastcall *func)(OP2Button *classPtr, int dummy, int boolEnabled) = (void (__fastcall *)(OP2Button*,int,int))(imageBase + 0xA9B0);
	func(classPtr, 0, boolEnabled);
}

void __fastcall ButtonOnClickDispatcher(OP2Button *classPtr)
{
	PaneButton *pb = classPtr->btnPtr;

	if (pb) {
		pb->OnClick();
	}

	// no harm done if they didn't provide an OnClick, better than exiting with 'pure virtual function call'
}

PaneButton::PaneButton()
{
	internalBtn = NULL;
	internalVtbl = NULL;
	isInternalObj = 0;

	if (!isInited) // automatic initialization since objects in static storage will be constructed before InitProc is reached
	{
		if (HFLInit() == HFLCANTINIT)
		{
			return;
		}
	}

	// set up button object
	internalBtn = new OP2Button;

	OP2Button *p = internalBtn;
	memset(p, 0, sizeof(OP2Button));
	// save a pointer so the dispatcher function can redirect the call to the right object
	p->btnPtr = this;
	
	// set up vtbl
	internalVtbl = new OP2ButtonVtbl;
	
	OP2ButtonVtbl *vp = internalVtbl;
	p->vtbl = vp;

	vp->Destructor = (void* (__fastcall *)(OP2Button*,int,int))(imageBase + 0xA8F0);
	vp->Paint = ButtonPaintDispatcher;
	vp->OnUIEvent = ButtonOnUIEventDispatcher;
	vp->OnAdd = ButtonOnAddDispatcher;
	vp->OnRemove = ButtonOnRemoveDispatcher;
	vp->SetEnabled = ButtonSetEnabledDispatcher;
	vp->unknown1 = (void (__fastcall *)(OP2Button*,int,int))(imageBase + 0xA830);
	vp->GetHelpText = (void (__fastcall *)(OP2Button*,int,char*,int))(imageBase + 0xB2B0);
	vp->OnClick = ButtonOnClickDispatcher;

	// Must be 4.. dunno why
	// todo: see if it crashes if the button doesn't have text and this is set
	p->hasButtonText = 4;
}

PaneButton::PaneButton(OP2Button *internalPtr)
{
	internalBtn = NULL;
	internalVtbl = NULL;
	isInternalObj = 0;

	if (!isInited) // automatic initialization since objects in static storage will be constructed before InitProc is reached
	{
		if (HFLInit() == HFLCANTINIT)
		{
			return;
		}
	}

	internalBtn = internalPtr;
	internalVtbl = internalPtr->vtbl;
	isInternalObj = 1;
}

PaneButton::~PaneButton()
{
	// destroy objects. (don't do anything special here as HFL will be cleaned-up by this time)
	if (!isInternalObj)
	{
		delete internalBtn;
		delete internalVtbl;
	}
}

void PaneButton::Paint(PaneGFXSurface gfxSurface)
{
	// nothing, default implementation
}

void PaneButton::OnUIEvent(UINT msg, WPARAM wParam, LPARAM lParam)
{
	// nothing, default implementation
}

void PaneButton::OnAdd()
{
	// nothing, default implementation
}

void PaneButton::OnRemove()
{
	// nothing, default implementation
}

void PaneButton::SetEnabled(int boolEnabled)
{
	// nothing, default implementation
}

void PaneButton::OnClick()
{
	// nothing, default implementation
}

void PaneButton::SetParams(int pixelX, int pixelY, int animId, int normalFrameId, int activeFrameId, int disabledFrameId, char *helpText, char *label)
{
	OP2Button *p = internalBtn;

	if (!p) { // not inited if this is null
		return;
	}

	OP2ButtonData data;
	memset(&data, 0, sizeof(data));

	data.animId = animId;
	data.normalFrameId = normalFrameId;
	data.activeFrameId = activeFrameId;
	data.disabledFrameId = disabledFrameId;
	data.helpText = helpText;
	data.buttonLabel = label;

	void (__fastcall *func)(OP2Button *classPtr, int dummy, OP2ButtonData *bdata, int pixelX, int pixelY, int unknown) = (void (__fastcall *)(OP2Button*,int,OP2ButtonData*,int,int,int))(imageBase + 0xAF40);

	func(p, 0, &data, pixelX, pixelY, 0);
}

void PaneButton::SetHelpText(char *helpText)
{
	OP2Button *p = internalBtn;

	if (!p) {// not inited if this is null
		return;
	}

	p->data.helpText = helpText;
}

char* PaneButton::GetHelpText()
{
	OP2Button *p = internalBtn;

	if (!p) { // not inited if this is null
		return (char*)HFLNOTINITED;
	}

	return p->data.helpText;
}

void PaneButton::SetLabel(char *label)
{
	OP2Button *p = internalBtn;

	if (!p) { // not inited if this is null
		return;
	}

	p->data.buttonLabel = label;
}

char* PaneButton::GetLabel()
{
	OP2Button *p = internalBtn;

	if (!p) { // not inited if this is null
		return (char*)HFLNOTINITED;
	}

	return p->data.buttonLabel;
}

int PaneButton::GetPushedIn()
{
	OP2Button *p = internalBtn;

	if (!p) { // not inited if this is null
		return HFLNOTINITED;
	}

	return (p->flags & 0x40);
}

void PaneButton::SetPushedIn(int boolPushed)
{
	OP2Button *p = internalBtn;

	if (!p) { // not inited if this is null
		return;
	}

	if (boolPushed) {
		p->flags |= 0x40;
	}
	else {
		p->flags &= ~0x40;
	}
}

int PaneButton::GetAcceleratorKey()
{
	OP2Button *p = internalBtn;

	if (!p) { // not inited if this is null
		return HFLNOTINITED;
	}

	return p->accelKey;
}

void PaneButton::SetAcceleratorKey(int asciiCode)
{
	OP2Button *p = internalBtn;

	if (!p) { // not inited if this is null
		return;
	}

	p->accelKey = asciiCode;
}

RECT* PaneButton::GetBoundingBox()
{
	OP2Button *p = internalBtn;

	if (!p) { // not inited if this is null
		return NULL;
	}

	return &p->rect;
}

ReportButton::ReportButton()
{
}

ReportButton::ReportButton(OP2Button *internalPtr) : PaneButton(internalPtr)
{
}

PaneReport ReportButton::GetAttachedReport()
{
	OP2Button *p = internalBtn;

	if (!p) { // not inited if this is null
		return PaneReport(0);
	}

	return PaneReport(p->report);
}

void ReportButton::SetAttachedReport(PaneReport *newReport)
{
	OP2Button *p = internalBtn;

	if (!p) { // not inited if this is null
		return;
	}

	p->report = newReport->internalRpt;
}
