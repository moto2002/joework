// dllmain.cpp : 定义 DLL 应用程序的入口点。
#include "stdafx.h"
#include<iostream>

HINSTANCE g_hInstance;
//HHOOK g_MsgHook;
HHOOK g_CBTHook;


/*

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
break;
case WM_CHAR://输入框，发送字符的消息
{
//按下了哪个字符
char szKey = pMsg->wParam;
char szWindowName[MAXBYTE] = { 0 };
char szBuffer[MAXBYTE] = { 0 };
GetWindowTextA(pMsg->hwnd, szWindowName, MAXBYTE);
sprintf_s(szBuffer, MAXBYTE, "窗口%s获取了消息%c", szWindowName, szKey);
MessageBoxA(nullptr, szBuffer, szWindowName, MB_OK);
}
default:
break;
}
}
return CallNextHookEx(g_MsgHook, nCode, wParam, lParam);
}

*/


// 截获系统基本消息, 譬如: 窗口的创建、激活、关闭、最大最小化、移动等等
LRESULT CALLBACK CBTProc(int nCode, WPARAM wParam, LPARAM lParam)
{
	char *info = NULL;
	switch (nCode)
	{
	case HCBT_ACTIVATE: //激活
		info = "激活";
		break;
	case HCBT_CREATEWND://创建窗口
		info = "创建窗口";
		/*PMSG pMsg = (PMSG)lParam;
		char szWindowName[MAXBYTE] = { 0 };
		GetWindowTextA(pMsg->hwnd, szWindowName, MAXBYTE);*/
		break;
	case HCBT_DESTROYWND://销毁窗口
		info = "销毁窗口";
		break;
	case HCBT_MINMAX://最小化，最大化
		info = "最小化，最大化";
		break;
	case HCBT_MOVESIZE://移动
		info = "移动";
		break;
	default:break;
	}

	return CallNextHookEx(g_CBTHook, nCode, wParam, lParam);
}

extern "C"
__declspec(dllexport)bool StartHook()
{


	//钩子类型 idHook 选项:
	/*
	WH_MSGFILTER = -1; {线程级; 截获用户与控件交互的消息}
	WH_JOURNALRECORD = 0; {系统级; 记录所有消息队列从消息队列送出的输入消息, 在消息从队列中清除时发生; 可用于宏记录}
	WH_JOURNALPLAYBACK = 1; {系统级; 回放由 WH_JOURNALRECORD 记录的消息, 也就是将这些消息重新送入消息队列}
	WH_KEYBOARD = 2; {系统级或线程级; 截获键盘消息}
	WH_GETMESSAGE = 3; {系统级或线程级; 截获从消息队列送出的消息}
	WH_CALLWNDPROC = 4; {系统级或线程级; 截获发送到目标窗口的消息, 在 SendMessage 调用时发生}
	WH_CBT = 5; {系统级或线程级; 截获系统基本消息, 譬如: 窗口的创建、激活、关闭、最大最小化、移动等等}
	WH_SYSMSGFILTER = 6; {系统级; 截获系统范围内用户与控件交互的消息}
	WH_MOUSE = 7; {系统级或线程级; 截获鼠标消息}
	WH_HARDWARE = 8; {系统级或线程级; 截获非标准硬件(非鼠标、键盘)的消息}
	WH_DEBUG = 9; {系统级或线程级; 在其他钩子调用前调用, 用于调试钩子}
	WH_SHELL = 10; {系统级或线程级; 截获发向外壳应用程序的消息}
	WH_FOREGROUNDIDLE = 11; {系统级或线程级; 在程序前台线程空闲时调用}
	WH_CALLWNDPROCRET = 12; {系统级或线程级; 截获目标窗口处理完毕的消息, 在 SendMessage 调用后发生}
	*/
	//g_MsgHook = SetWindowsHookEx(WH_GETMESSAGE,MsgHookProc,g_hInstance,NULL);

	g_CBTHook = SetWindowsHookEx(WH_CBT, CBTProc, g_hInstance, NULL);// 截获系统基本消息, 譬如: 窗口的创建、激活、关闭、最大最小化、移动等等

	if (g_CBTHook)
		return true;
	return false;
}

//解除Hook
bool UnHook()
{
	return UnhookWindowsHookEx(g_CBTHook);
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

