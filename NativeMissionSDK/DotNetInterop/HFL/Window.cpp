// Window.h
// Creates child window for rendering.
#include "stdafx_unmanaged.h"

/*#define WIN32_LEAN_AND_MEAN
#include <Windows.h>						// Required for DLLMain

// Windows.h requires %(AdditionalDependencies) in Linker > Input

#ifndef EXPORT
#define EXPORT __declspec(dllexport)
#endif

static HWND overlayWnd;

extern "C"
{
	// Forward declarations
	LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam);
	void OnSize(HWND hwnd, UINT flag, int width, int height);


	extern EXPORT bool __stdcall Window_Init()
	{
		const wchar_t CLASS_NAME[] = L"Mission Window";

		WNDCLASS wc = { };

		wc.lpfnWndProc = WindowProc;
		wc.hInstance = GetModuleHandle(NULL);
		wc.lpszClassName = CLASS_NAME;

		RegisterClass(&wc);

		overlayWnd = CreateWindowEx(0, CLASS_NAME, L"Title", WS_POPUP, CW_USEDEFAULT, CW_USEDEFAULT, 400, 300, NULL, NULL, GetModuleHandle(NULL), NULL);
		if (overlayWnd == NULL)
			return false;

		ShowWindow(overlayWnd, SW_SHOWNORMAL);

		return true;
	}

	LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
	{
		switch (uMsg)
		{
			case WM_SIZE:
			{
				int width = LOWORD(lParam);  // Macro to get the low-order word.
				int height = HIWORD(lParam); // Macro to get the high-order word.

				// Respond to the message:
				OnSize(hwnd, (UINT)wParam, width, height);
			}
			break;

			case WM_PAINT:
			{
				PAINTSTRUCT ps;
				HDC hdc = BeginPaint(hwnd, &ps);

				// All painting occurs here, between BeginPaint and EndPaint.

				FillRect(hdc, &ps.rcPaint, (HBRUSH)(COLOR_WINDOW + 1));

				RECT rect;
				rect.top = 10;
				rect.left = 10;
				rect.right = 50;
				rect.bottom = 50;
				FillRect(hdc, &rect, (HBRUSH)(COLOR_WINDOW + 1));

				EndPaint(hwnd, &ps);
			}
			return 0;

			case WM_CLOSE:
			{
			}
			return 0;

			case WM_DESTROY:
			{
			}
			return 0;
		}

		return DefWindowProc(hwnd, uMsg, wParam, lParam);
	}

	void OnSize(HWND hwnd, UINT flag, int width, int height)
	{
		// Handle resizing
	}

	HWND mainWnd = NULL;

	extern EXPORT void __stdcall Window_Update()
	{
		if (mainWnd == NULL)
		{
			mainWnd = GetActiveWindow();
			SetParent(overlayWnd, mainWnd);
		}

		RECT rc = { }; //temporary rectangle
		GetClientRect(mainWnd, &rc); //the "inside border" rectangle for a
		MoveWindow(overlayWnd, rc.left, rc.top, rc.right - rc.left, rc.bottom - rc.top, TRUE); //place b at (x,y,w,h) in a
		UpdateWindow(mainWnd);

		RedrawWindow(overlayWnd, NULL, NULL, RDW_INVALIDATE);
		//SendMessage(overlayWnd, WM_PAINT, NULL, NULL);
		//int error = GetLastError();
		//error += 0;
	}

	extern EXPORT void __stdcall Window_Destroy()
	{
		DestroyWindow(overlayWnd);
	}

	
	//HDC dc = GetDC(myWnd);
	//RECT rect;
	//rect.top = 10;
	//rect.left = 10;
	//rect.right = 50;
	//rect.bottom = 50;
	//FillRect(dc, &rect, (HBRUSH)(COLOR_WINDOW + 1));
		
}*/