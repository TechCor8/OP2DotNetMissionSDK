// TriggerEx.h
#ifndef _TRIGGEREX_H_
#define _TRIGGEREX_H_

class TriggerEx : public Trigger
{
	void SetHasFired(int playerId, int boolSet);
	TRIGCB GetCallbackAddress();
	char *GetCallbackName();
	void SetCallback(char *funcName);
	void SetCallback(TRIGCB callback);
	int GetNoRepeat();
	void SetNoRepeat(int boolNoRepeat);
	int GetPlayerNum();
	void SetPlayerNum(int playerId);
	Trigger GetAttachedTrigger(); // todo: checking for VC trigger
	void SetAttachedTrigger(Trigger *trig);
	// todo: extra functions for each type of trigger
	// todo: any other get/set type of functions
};

#endif _TRIGGEREX_H_
