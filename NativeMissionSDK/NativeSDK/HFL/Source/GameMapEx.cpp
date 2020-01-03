#include "HFL.h"

#pragma pack(push,1)
struct OP2Map
{
	int unknown;
	unsigned int pixelWidthMask;
	int pixelWidth;
	unsigned int tileXMask;
	int tileWidth;
	int logTileWidth; // base 2 log
	int tileHeight;
	MAP_RECT clipRect;
	int numTiles;
	int numTileSets;
	int logTileHeight; // base 2 log
	int paddingOffsetTileX;
	int numUnits;
	int lastUsedUnitIndex;
	int nextFreeUnitSlotIndex;
	int firstFreeUnitSlotIndex;
	int unknown2;
	void *unitArray;
	void *unitLinkedListHead;
};
#pragma pack(pop)

GameMapEx gMap;
void *mapObj;
void *mapTileData;

MapTile GameMapEx::GetTileEx(LOCATION where)
{
	MapTile t;
	memset(&t, 0, sizeof(t));

	if (!isInited) {
		return t;
	}

	OP2Map *p = (OP2Map*)mapObj;
	MapTile **tileArray = (MapTile**)(mapTileData);

	int xLower = p->tileXMask & where.x;
	int xUpper = xLower >> 5;
	xLower &= 31;
	xUpper <<= p->logTileHeight;
	xUpper += where.y;
	xUpper <<= 5;
	return (*tileArray)[xUpper + xLower];
}

void GameMapEx::SetTileEx(LOCATION where, MapTile newData)
{
	if (!isInited) {
		return;
	}

	OP2Map *p = (OP2Map*)mapObj;
	MapTile **tileArray = (MapTile**)(mapTileData);

	int xLower = p->tileXMask & where.x;
	int xUpper = xLower >> 5;
	xLower &= 31;
	xUpper <<= p->logTileHeight;
	xUpper += where.y;
	xUpper <<= 5;

	(*tileArray)[xUpper + xLower] = newData;
}

int GameMapEx::GetMapWidth()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	OP2Map *p = (OP2Map*)mapObj;

	return 1 << p->logTileWidth;
}

int GameMapEx::GetMapHeight()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	OP2Map *p = (OP2Map*)mapObj;

	return 1 << p->logTileHeight;
}


int GameMapEx::GetNumUnits()
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	OP2Map *p = (OP2Map*)mapObj;
	return p->numUnits;
}

int GameMapEx::LoadMap(char *fileName)
{
	if (!isInited) {
		return HFLNOTINITED;
	}

	OP2Map *p = (OP2Map*)mapObj;

	int (__fastcall *func)(OP2Map* classPtr, int dummy, char *fileName) = (int (__fastcall *)(OP2Map*,int,char*))(imageBase + 0x37310);
	
	return func(p, 0, fileName);
}

void GameMapEx::CopyTileMap(int* tileMap)
{
	int **tileArray = (int**)(mapTileData);
	
	memcpy(tileMap, *tileArray, GetMapWidth()*GetMapHeight()*sizeof(int));
}
