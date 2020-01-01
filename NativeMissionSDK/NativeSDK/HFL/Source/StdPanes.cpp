#include "HFL.h"


struct OP2CommandPane;

#pragma pack(push, 1)
struct OP2CommandPaneVtbl
{
	int iwnd[6]; // don't care about these
	int pane[5]; // don't care here either TODO fill these in later
	void (__fastcall *AddUpdateRect)(void *classPtr, int dummy, RECT *updateRect);
	int Paint; // don't care
};

struct OP2CommandPane
{
	OP2CommandPaneVtbl *vtbl;
	int iwnd[4]; // don't care
	void *gfxSurface;
	int controls[20]; // don't care
	int numControls;
	int reportButtons[6][43]; // Don't need to know the structure of the buttons
	RECT updateRect;
	int unknown1;
	int numReportButtons;
	int unknown2;
	void *curReport;
	int reportButtonHeight;
};
#pragma pack(pop)

void *cmdPane;

CommandPane gCommandPane;

void CommandPane::ActivateReport(PaneReport *report)
{
	if (!isInited) {
		return;
	}

	void (__fastcall *func)(void *classPtr, int dummy, void *report) = (void (__fastcall *)(void*,int,void*))(imageBase + 0x5D160);

	func(cmdPane, 0, report->internalRpt);
}

void CommandPane::AddUpdateRect(RECT *updateRect)
{
	if (!isInited) {
		return;
	}

	OP2CommandPane *p = (OP2CommandPane*)cmdPane;
	p->vtbl->AddUpdateRect(cmdPane, 0, updateRect);
}

int CommandPane::GetReportButtonHeight()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	int (__fastcall *func)(void *classPtr) = (int (__fastcall *)(void*))(imageBase + 0x5CC40);

	return func(cmdPane);
}

ReportButton CommandPane::GetReportButton(ReportButtonType which)
{
	if (!isInited) {
		return ReportButton(0);
	}

	if (which < 1 || which > 6) {
		return ReportButton(0);
	}

	OP2CommandPane *p = (OP2CommandPane*)cmdPane;

	return ReportButton((OP2Button*)p->reportButtons[which-1]);
}

void CommandPane::AddButton(PaneButton *btn)
{
	if (!isInited) {
		return;
	}

	void (__fastcall *func)(void *classPtr, int dummy, void *button) = (void (__fastcall *)(void*,int,void*))(imageBase + 0x9D1A0);

	func(cmdPane, 0, btn->internalBtn);
}

void CommandPane::RemoveButton(PaneButton *btn)
{
	if (!isInited) {
		return;
	}

	void (__fastcall *func)(void *classPtr, int dummy, void *button) = (void (__fastcall *)(void*,int,void*))(imageBase + 0x9D1C0);

	func(cmdPane, 0, btn->internalBtn);
}
