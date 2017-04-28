/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package socket.io.mina;

import org.apache.mina.core.session.IoSession;
import org.apache.mina.filter.codec.ProtocolCodecFactory;
import org.apache.mina.filter.codec.ProtocolDecoder;
import org.apache.mina.filter.codec.ProtocolEncoder;

/**
 *过滤工厂，设置编码类和解码类
 */
public class MinaFilterFactory implements ProtocolCodecFactory {

    private ProtocolEncoder encoder = new MinaCMDEncoder();
    private ProtocolDecoder decoder = new MinaCMDDecoder();

    @Override
    public ProtocolEncoder getEncoder(IoSession is) throws Exception {
        return encoder;
    }

    @Override
    public ProtocolDecoder getDecoder(IoSession is) throws Exception {
        return decoder;
    }

}
