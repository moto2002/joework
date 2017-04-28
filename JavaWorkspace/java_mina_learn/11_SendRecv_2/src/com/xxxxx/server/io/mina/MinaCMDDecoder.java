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
        if (buffer.limit() - buffer.position() >= 4) 
        {
            int size = buffer.getInt(0);
            if (buffer.limit() - buffer.position() >= size + 4) {
                buffer.getInt();
                byte[] bytes = new byte[size];
                buffer.get(bytes);
                
                Login login = Login.parseFrom(bytes);
                System.out.println(login.getUsername() + " " + login.getPassword());

                pdo.write(login);
//                is.write(pdo);
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }

        //buffer.
    }
}
