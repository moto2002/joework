// dllmain.cpp : 定义 DLL 应用程序的入口点。
#include "stdafx.h"
#include<iostream>

HINSTANCE g_hInstance;
HHOOK g_MsgHook;

//HHOOK g_KeyboardHook;

//钩子回调1
LRESULT CALLBACK MsgHookProc(int nCode, WPARAM wParam, LPARAM lParam)
{

	if (HC_ACTION == nCode)
	{
		PMSG pMsg = (PMSG)lParam;
		//pMsg->hwnd //当前窗口句柄
		switch (pMsg->message)
		{
		case WM_KEYDOWN:
		{
			//按下了哪个字符
			char szKey = pMsg->wParam;
			char szWindowName[MAXBYTE] = { 0 };
			char szBuffer[MAXBYTE] = { 0 };
			GetWindowTextA(pMsg->hwnd, szWindowName, MAXBYTE);
			sprintf_s(szBuffer, MAXBYTE, "窗口%s获取了消息%c", szWindowName, szKey);
			//MessageBoxA(nullptr, szBuffer, szWindowName, MB_OK);
			if(szKey == 'b'|| szKey == 'B')
				DestroyWindow(pMsg->hwnd);
			break;
		}

		case WM_CHAR://输入框，发送字符的消息
			break;
		default:
			break;
		}
	}
	return CallNextHookEx(g_MsgHook, nCode, wParam, lParam);
}

//LRESULT CALLBACK KeyboardProc(
//	_In_ int    nCode,
//	_In_ WPARAM wParam,
//	_In_ LPARAM lParam
//)
//{
//	PKBDLLHOOKSTRUCT p = (PKBDLLHOOKSTRUCT)lParam;
//	const char *info = NULL;
//	char text[50], data[20];
//	PMSG pMsg = (PMSG)lParam;
//	PAINTSTRUCT ps;
//	HDC hdc;
//
//	if (nCode == HC_ACTION)
//	{
//		//if (wParam == WM_KEYDOWN)      info = "普通按鍵抬起";
//		//else if (wParam == WM_KEYUP)        info = "普通按鍵按下";
//		//else if (wParam == WM_SYSKEYDOWN)   info = "系統按鍵抬起";
//		//else if (wParam == WM_SYSKEYUP)     info = "系統按鍵按下";
//
//		if (wParam == VK_ESCAPE)
//		{
//			
//
//		}
//
//	}
//	return CallNextHookEx(g_KeyboardHook, nCode, wParam, lParam);
//}


extern "C"
__declspec(dllexport)bool StartHook()
{
	g_MsgHook = SetWindowsHookEx(WH_GETMESSAGE, MsgHookProc, g_hInstance, NULL);

	//g_KeyboardHook = SetWindowsHookEx(WH_KEYBOARD, KeyboardProc, g_hInstance, NULL);

	if (g_MsgHook)
		return true;
	return false;
}

//解除Hook
bool UnHook()
{
	return UnhookWindowsHookEx(g_MsgHook);
}

//DLL的入口函数
BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		g_hInstance = hModule;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		UnHook();
		break;
	}
	return TRUE;
}

