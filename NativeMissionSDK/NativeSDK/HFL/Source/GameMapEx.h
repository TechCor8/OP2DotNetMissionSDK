// GameMapEx.h
// Extensions to the GameMap class
#ifndef _GAMEMAPEX_H_
#define _GAMEMAPEX_H_

class GameMapEx : public GameMap
{
public:
	static MapTile GetTileEx(LOCATION where);
	static void SetTileEx(LOCATION where, MapTile newData);
	static int GetMapWidth();
	static int GetMapHeight();
	static int GetNumUnits();
	static int LoadMap(char *fileName);
};

extern GameMapEx gMap;
extern void *mapObj;
extern void *mapTileData;

#endif // _GAMEMAPEX_H_
