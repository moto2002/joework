/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.server.io.mina;

import java.nio.ByteBuffer;

import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.filter.codec.ProtocolEncoder;
import org.apache.mina.filter.codec.ProtocolEncoderOutput;
import org.springframework.util.SystemPropertyUtils;

import com.hq.datapool.DyncDataPool;
import com.hq.packet.Request_Login;
import com.hq.packet.Response_Login;

/**
 *
 * @author Administrator
 */
public class MinaCMDEncoder implements ProtocolEncoder
{
	@Override
	public void encode(IoSession is, Object o, ProtocolEncoderOutput peo) throws Exception
	{
		System.out.println("编码");
		
		byte[] b = (byte[])o;	
		int size = b.length;	
		IoBuffer buffer = IoBuffer.allocate(size+4);//用的是mina的ioBuffer
		buffer.putInt(size);//消息包长度
		buffer.put(b);//消息包内容字节数组
		buffer.flip();// 把limit设为当前position，把position设为0
		peo.write(buffer);
	}

	@Override
	public void dispose(IoSession is) throws Exception
	{
	}

}
