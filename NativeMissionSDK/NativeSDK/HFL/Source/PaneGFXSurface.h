// PaneGFXSurface.h
// Graphics rendering surface (as well as Text Rendering interface)
#ifndef _PANEGFXSURFACE_H_
#define _PANEGFXSURFACE_H_

class PaneGFXSurface
{
public:
	PaneGFXSurface(void *gfxPointer);

	// todo: last 'unknown' param?
	void DrawGraphic(int animIdx, int frameIdx, int pixelX, int pixelY, int playerId);
	int GetWidth();
	int GetHeight();

	static RenderData* InitRenderData(int numChunks);
	static void FreeRenderData(RenderData *data);
	static void ParseText(char *string, int stringLen, int boundingBoxWidth, RenderData *data, SIZE *extents);
	void DrawText(char *string, RenderData *data, RECT *boundingBox, int vSpacing);

	static int GetAnimFrameWidth(int animIdx, int frameIdx);
	static int GetAnimFrameHeight(int animIdx, int frameIdx);
	static SIZE GetSelectionBoxDimensions(int animIdx);
	static int GetNumFrames(int animIdx);

	void *internalSurface;
};

extern void *graphicsObj;
extern void *textRenderObj;

#endif // _PANEGFXSURFACE_H_
