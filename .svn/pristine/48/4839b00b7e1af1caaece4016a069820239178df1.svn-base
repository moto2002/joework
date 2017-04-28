/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.server.io.mina;

import com.xxxxx.server.io.ServerIO;
import java.net.InetSocketAddress;
import org.apache.mina.core.filterchain.DefaultIoFilterChainBuilder;
import org.apache.mina.filter.codec.ProtocolCodecFilter;
import org.apache.mina.transport.socket.SocketSessionConfig;
import org.apache.mina.transport.socket.nio.NioSocketAcceptor;

/**
 *
 * @author Administrator
 */
public class ServerMina implements ServerIO
{

	public void start(int port)
	{
		try
		{
			NioSocketAcceptor acceptor = new NioSocketAcceptor();
			acceptor.setHandler(new SocketMinaHandler());

			DefaultIoFilterChainBuilder chain = acceptor.getFilterChain();
			chain.addLast("protocol", new ProtocolCodecFilter(new MinaFilterFactory()));

			SocketSessionConfig scfg = acceptor.getSessionConfig();

			acceptor.bind(new InetSocketAddress(port));
		} catch (Exception e)
		{
			e.printStackTrace();
		}
	}
}
