package com.define;

import java.util.HashMap;
import java.util.Map;

public class RequestLinkResponse
{
	// ------REQUEST---------
	public static final int REQUEST_START = 0;
	public static final int REQUEST_LOGIN = REQUEST_START + 1;

	// -----RESPONE----------
	public static final int RESPONE_START = 100;
	public static final int RESPONE_LOGIN = RESPONE_START + 1;

	// Binding
	public static Map<Integer, Integer> Link = new HashMap<>();

	public static void Reg()
	{
		Link.put(REQUEST_LOGIN, RESPONE_LOGIN);
	}

	public static void Init()
	{
		Reg();
		// System.out.println("REQUEST_LOGIN-"+Link.get(REQUEST_LOGIN));

	}
}
