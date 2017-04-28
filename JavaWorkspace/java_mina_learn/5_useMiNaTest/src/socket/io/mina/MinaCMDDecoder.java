/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package socket.io.mina;

import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.filter.codec.CumulativeProtocolDecoder;
import org.apache.mina.filter.codec.ProtocolDecoderOutput;

/**
 *解码器，解开消息的格式，变成可以处理的数据格式。
 */
public class MinaCMDDecoder extends CumulativeProtocolDecoder {

    /**
     * 
     * @return true 解码完成，释放缓存，false 解码未完成，等待下一次解码。
     * @throws Exception 
     */
    @Override
    protected boolean doDecode(IoSession is, IoBuffer ib, ProtocolDecoderOutput pdo) throws Exception {
        System.out.println("解码");
        return false;
    }
}
