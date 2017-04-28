/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.server;

import com.define.RequestLinkResponse;
import com.xxxxx.server.io.ServerIO;
import com.xxxxx.server.io.mina.ServerMina;

/**
 *
 * @author sf
 */
public class Server
{

	private static ServerIO serverIO = new ServerMina();

	public static void start()
	{
		serverIO.start(10060);
	}

	public static void main(String[] args)
	{
		start();
		RequestLinkResponse.Init();
	}
}
