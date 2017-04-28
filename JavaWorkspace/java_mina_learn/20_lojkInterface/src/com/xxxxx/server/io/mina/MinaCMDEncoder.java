/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.server.io.mina;

import java.lang.reflect.Method;
import java.nio.ByteBuffer;
import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.filter.codec.ProtocolEncoder;
import org.apache.mina.filter.codec.ProtocolEncoderOutput;

/**
 *
 * @author Administrator
 */
public class MinaCMDEncoder implements ProtocolEncoder
{

	@Override
	public void encode(IoSession is, Object o, ProtocolEncoderOutput peo)
			throws Exception
	{
		System.out.println("发送" + o);
		
		String name = o.getClass().getName();
		byte[] nameBytes = name.getBytes();//类名的字节数组
		int nameSize = nameBytes.length;//类名的长度
		
		// LoginResult result = (LoginResult) o;
		// Method method =
		// Class.forName("com.google.protobuf.AbstractMessageLite").getDeclaredMethod("toByteArray");
		
		Method method = o.getClass().getMethod("toByteArray");
		byte[] bytes = (byte[]) method.invoke(o);//消息包内容字节数组
		// byte[] bytes = result.toByteArray();

		int size = bytes.length;//消息包内容长度
		
		// 用的是mina的ioBuffer
		IoBuffer buffer = IoBuffer.allocate(nameSize + 4 + size + 4);//类名的长度+存放长度的数的长度+消息内容长度+存放长度的数的长度
		buffer.putInt(nameSize);//类名的长度
		buffer.put(nameBytes);//类名字节数组

		buffer.putInt(size);//消息包长度
		buffer.put(bytes);//消息包内容字节数组

		// 把limit设为当前position，把position设为0
		buffer.flip();
		peo.write(buffer);
	}

	@Override
	public void dispose(IoSession is) throws Exception
	{
	}

}
