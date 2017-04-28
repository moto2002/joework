/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.server.io.mina;

import cmd.user.Login;
import cmd.user.LoginResult;

//import com.sun.org.apache.bcel.internal.util.ByteSequence;
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
public class MinaCMDDecoder extends CumulativeProtocolDecoder {

    public MinaCMDDecoder() {
    }

    @Override
    protected boolean doDecode(IoSession is, IoBuffer ib, ProtocolDecoderOutput pdo) throws Exception {
        System.out.println("doDecode");
        ByteBuffer buffer = ib.buf();
        if (buffer.remaining() >= 4) {

            int size = buffer.getInt(0);
            if (buffer.remaining() >= size + 4 + 4) {
                int length = buffer.getInt(size + 4);
                if (buffer.remaining() >= size + 4 + 4 + length) {
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
                                   
//                    Login login = Login.parseFrom(bytes);
//                    System.out.println(login.getUsername() + " " + login.getPassword());
                    
        			cmd.user.Login l = (cmd.user.Login)object;      		    
					System.out.println(l.getUsername()+":"+l.getPassword());
					
					//Object  t= cls.cast(object);
					

                    pdo.write(object);
                    return true;
                }

            } else {
                return false;
            }
        } else {
            return false;
        }
        return false;
    }
}
