#pragma once

struct OP2Button;
struct OP2ButtonVtbl;

// Definition of Outpost2 pane button / control, as well as report button.
class PaneButton
{
public:
	PaneButton();
	PaneButton(OP2Button *internalPtr);
	~PaneButton();

	virtual void Paint(PaneGFXSurface gfxSurface);
	virtual void OnUIEvent(UINT msg, WPARAM wParam, LPARAM lParam);
	virtual void OnAdd();
	virtual void OnRemove();
	virtual void SetEnabled(int boolEnabled);
	virtual void OnClick();

	void SetParams(int pixelX, int pixelY, int animId, int normalFrameId, int activeFrameId, int disabledFrameId, char *helpText, char *label);
	void SetHelpText(char *helpText);
	char* GetHelpText();
	void SetLabel(char *label);
	char* GetLabel();
	int GetPushedIn();
	void SetPushedIn(int boolPushed);
	int GetAcceleratorKey();
	void SetAcceleratorKey(int asciiCode);
	RECT* GetBoundingBox();

	OP2ButtonVtbl *internalVtbl;
	OP2Button *internalBtn;
	int isInternalObj;
};

class ReportButton : public PaneButton
{
public:
	ReportButton();
	ReportButton(OP2Button *internalPtr);
	PaneReport GetAttachedReport();
	void SetAttachedReport(PaneReport *newReport);
};
