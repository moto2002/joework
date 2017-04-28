/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.server.io.mina;

import cmd.user.LoginResult;
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
public class MinaCMDEncoder implements ProtocolEncoder {

    @Override
    public void encode(IoSession is, Object o, ProtocolEncoderOutput peo) throws Exception {
        System.out.println("发送" + o);
        String name = o.getClass().getName();
        byte[] nameBytes = name.getBytes();
        int nameSize = nameBytes.length;
        
        //LoginResult result = (LoginResult) o;
        //Method method = Class.forName("com.google.protobuf.AbstractMessageLite").getDeclaredMethod("toByteArray");
        //class.forName,执行效率比较低，最好用下面这种方式
        Method method = o.getClass().getMethod("toByteArray");
        byte[] bytes = (byte[]) method.invoke(o);
        
         //byte[] bytes = result.toByteArray();
        int size = bytes.length;
        IoBuffer buffer = IoBuffer.allocate(nameSize + 4 + size + 4);
        buffer.putInt(nameSize);
        buffer.put(nameBytes);
        buffer.putInt(size);
        buffer.put(bytes);
        buffer.flip();
        peo.write(buffer);
    }

    @Override
    public void dispose(IoSession is) throws Exception {
    }

}
