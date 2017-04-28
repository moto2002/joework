/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.server.io.mina;

//import com.sun.org.apache.bcel.internal.util.ByteSequence;
import cmd.user.Login;

import com.xxxxx.server.client.Client;

import java.awt.font.TextHitInfo;
import java.lang.reflect.Method;
import java.nio.ByteBuffer;

import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.filter.codec.CumulativeProtocolDecoder;
import org.apache.mina.filter.codec.ProtocolDecoderOutput;

/**
 *
 * @author Administrator
 */
public class MinaCMDDecoder extends CumulativeProtocolDecoder
{

	public MinaCMDDecoder()
	{
	}

	@Override
	protected boolean doDecode(IoSession is, IoBuffer ib, ProtocolDecoderOutput pdo) throws Exception
	{
		System.out.println("doDecode");
		ByteBuffer buffer = ib.buf();

		// 返回当前位置与限制之间的元素数
		if (buffer.remaining() >= 4)
		{

			int size = buffer.getInt(0);
			System.out.println("size:" + size);

			if (buffer.remaining() >= size + 4 + 4)
			{
				int length = buffer.getInt(size + 4);

				if (buffer.remaining() >= size + 4 + 4 + length)
				{
					buffer.getInt();

					byte[] stringBytes = new byte[size];
					buffer.get(stringBytes);
					String name = new String(stringBytes);
					System.out.println(name);

					buffer.getInt();
					byte[] bytes = new byte[length];
					buffer.get(bytes);

					Class cls = Class.forName(name);
					Method method = cls.getDeclaredMethod("parseFrom", byte[].class);
					Object object = method.invoke(null, bytes);

					// Login login = Login.parseFrom(bytes);
					// System.out.println(login.getUsername() + " " +
					// login.getPassword());
					// 例如com.user.Login
					String pgString = name.substring(0, name.lastIndexOf("."));// 得到com.user
					String claString = name.substring(name.lastIndexOf("."));// 得到.Login
					System.out.println("pgString:" + pgString + "|" + "claString:" + claString);
					// com.user.exe.Login
					Class classname = Class.forName(pgString + ".exe" + claString + "_EXE");
					classname.getMethod("execute", Class.forName(name), Client.class).invoke(null, object, is.getAttribute(Client.class.getSimpleName()));

					pdo.write(object);
					return true;
				}

			} else
			{
				return false;
			}
		} else
		{
			return false;
		}
		return false;
	}
}
