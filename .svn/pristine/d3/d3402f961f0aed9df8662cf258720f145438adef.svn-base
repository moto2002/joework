/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.server.io;

import cmd.user.LoginResult;

import com.define.RequestLinkResponse;
import com.xxxxx.server.client.ClientSession;

/**
 *
 * @author Administrator
 */
public class ServerHandler
{

	public void messageSent(ClientSession session, Object message)
	{
	}

	public void messageReceived(ClientSession session, Object message)
	{
		System.out.println("messageReceived");
		System.out.println(message);

		// 得到请求对应的回复
		int respone = RequestLinkResponse.Link
				.get((Integer)message);
		System.out.print(respone);

		LoginResult result = LoginResult.newBuilder().setResult(respone).build();
		session.write(result);
	}

	public void exceptionCaught(ClientSession session, Throwable cause)
	{
		System.out.println("exceptionCaught");
		cause.printStackTrace();
	}

	public void sessionClosed(ClientSession session)
	{
		System.out.println("sessionClosed");
	}

	public void sessionOpened(ClientSession session)
	{
		System.out.println("sessionOpened");
	}

}
