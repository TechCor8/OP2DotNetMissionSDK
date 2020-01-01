#include "HFL.h"
#include <malloc.h>

struct OP2GFXCDSSurface
{
	void *vtbl;
	int unk[11];
	void *bmpData;
	int scanlineByteWidth;
	int width;
	int height;
	int bpp;
	int unk2[11];
};

void *graphicsObj;
void *textRenderObj;

PaneGFXSurface::PaneGFXSurface(void *gfxPointer)
{
	internalSurface = NULL;

	if (!isInited) // automatic initialization since objects in static storage will be constructed before InitProc is reached
	{
		if (HFLInit() == HFLCANTINIT)
		{
			return;
		}
	}

	internalSurface = gfxPointer;
}

void PaneGFXSurface::DrawGraphic(int animIdx, int frameIdx, int pixelX, int pixelY, int playerId)
{
	OP2GFXCDSSurface *p = (OP2GFXCDSSurface*)internalSurface;

	if (!p) {
		return;
	}

	void (__fastcall *func)(OP2GFXCDSSurface* classPtr, int dummy, int animIdx, int frameIdx, int pixelX, int pixelY, int playerId, int unknown) = (void (__fastcall *)(OP2GFXCDSSurface*,int,int,int,int,int,int,int))(imageBase + 0x3F20);
	func(p, 0, animIdx, frameIdx, pixelX, pixelY, playerId, -1);
}

int PaneGFXSurface::GetWidth()
{
	OP2GFXCDSSurface *p = (OP2GFXCDSSurface*)internalSurface;

	if (!p) {
		return HFLNOTINITED;
	}

	return p->width;
}

int PaneGFXSurface::GetHeight()
{
	OP2GFXCDSSurface *p = (OP2GFXCDSSurface*)internalSurface;

	if (!p) {
		return HFLNOTINITED;
	}

	return p->height;
}

RenderData* PaneGFXSurface::InitRenderData(int numChunks)
{
	if (!isInited) {
		return (RenderData*)HFLNOTINITED;
	}

	RenderData *p = (RenderData*)malloc(sizeof(RenderHeader)+sizeof(RenderData)*numChunks);

	if (p) {
		p->header.numChunks = numChunks;
	}

	return p;
}

void PaneGFXSurface::FreeRenderData(RenderData *data)
{
	if (!isInited) {
		return;
	}

	free(data);
}

void PaneGFXSurface::ParseText(char *string, int stringLen, int boundingBoxWidth, RenderData *data, SIZE *extents)
{
	if (!isInited) {
		return;
	}

	void (__fastcall *func)(void *classPtr, int dummy, char *string, int stringLen, int boundingBoxWidth, RenderData *data, SIZE *extents) = (void (__fastcall *)(void*,int,char*,int,int,RenderData*,SIZE*))(imageBase + 0x16770);

	func(textRenderObj, 0, string, stringLen, boundingBoxWidth, data, extents);
}

void PaneGFXSurface::DrawText(char *string, RenderData *data, RECT *boundingBox, int vSpacing)
{
	// todo: find out why it takes the bounding box twice
	OP2GFXCDSSurface *p = (OP2GFXCDSSurface*)internalSurface;

	if (!p) {
		return;
	}

	void (__fastcall *func)(void *classPtr, int dummy, char *string, RenderData *data, void *gfxSurface, RECT *boundingBox, RECT *boundingBox2, int vSpacing) = (void (__fastcall *)(void*,int,char*,RenderData*,void*,RECT*,RECT*,int))(imageBase + 0x16B20);
	
	func(textRenderObj, 0, string, data, p, boundingBox, boundingBox, vSpacing);
}

int PaneGFXSurface::GetAnimFrameWidth(int animIdx, int frameIdx)
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	int (__fastcall *func)(void *classPtr, int dummy, int animIdx, int frameIdx) = (int (__fastcall *)(void*,int,int,int))(imageBase + 0x4DC0);

	return func(graphicsObj, 0, animIdx, frameIdx);
}

int PaneGFXSurface::GetAnimFrameHeight(int animIdx, int frameIdx)
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	int (__fastcall *func)(void *classPtr, int dummy, int animIdx, int frameIdx) = (int (__fastcall *)(void*,int,int,int))(imageBase + 0x4E00);

	return func(graphicsObj, 0, animIdx, frameIdx);
}

SIZE PaneGFXSurface::GetSelectionBoxDimensions(int animIdx)
{
	SIZE s;
	s.cx = HFLNOTINITED;
	s.cy = HFLNOTINITED;

	if (!isInited) {
		return s;
	}

	void (__fastcall *func)(void *classPtr, int dummy, int animIdx, LONG *retX, LONG *retY) = (void (__fastcall *)(void*,int,int,LONG*,LONG*))(imageBase + 0x4E40);

	func(graphicsObj, 0, animIdx, &s.cx, &s.cy);

	return s;
}

int PaneGFXSurface::GetNumFrames(int animIdx)
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	int (__fastcall *func)(void *classPtr, int dummy, int animIdx) = (int (__fastcall *)(void*,int,int))(imageBase + 0x4D80);

	return func(graphicsObj, 0, animIdx);
}
