// HookWindowsDemo.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include<windows.h>
#include<iostream>

typedef bool(*FUN)();

int main()
{
	HINSTANCE hDll = LoadLibrary(L"HookDllDemo.dll");
	if (hDll)
	{

		FUN fun = (FUN)GetProcAddress(hDll, "StartHook");
		if (fun)
		{
			fun(); //下钩子
		}

		char szInput[MAXBYTE] = {0};
		while (true)
		{
			std::cin >> szInput;
			if (0 == strcmp(szInput,"yes"))
			{
				break; //输入yes 跳出
			}
		}

	}
	return 0;
}

