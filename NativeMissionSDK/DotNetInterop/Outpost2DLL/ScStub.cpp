#include "stdafx_unmanaged.h"

#include <Outpost2DLL/Outpost2DLL.h>	// Main Outpost 2 header to interface with the game

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif

// Note: ScStub is the parent class of Triggers, Groups, and Pinwheel classes.
//		 All functions in this class are available to derived classes
// Note: Do not try to create an instance of this class. It was meant
//		 simply as a base parent class from which other classes inherit
//		 functions from. Creating an instance of this class serves little
//		 (or no) purpose and may even crash the game.

/*class OP2 ScStub
{
public:
	ScStub();
	~ScStub();
	ScStub& operator = (const ScStub& scStub);

	// Methods
	void Destroy();
	void Disable();
	void Enable();
	// [Get]
	int Id() const;
	int IsEnabled();
	int IsInitialized();
	// [Set]
	void SetId(int stubIndex);

public:	// Why not? ;)
	int stubIndex;
};*/

extern "C"
{
	extern EXPORT int __stdcall ScStub_GetIndex(ScStub* handle)
	{
		return handle->Id();
	}

	extern EXPORT void __stdcall ScStub_Release(ScStub* handle)
	{
		delete handle;
	}
}
