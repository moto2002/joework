/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package socket.io.mina;

import java.net.InetSocketAddress;
import org.apache.mina.core.filterchain.DefaultIoFilterChainBuilder;
import org.apache.mina.filter.codec.ProtocolCodecFilter;
import org.apache.mina.transport.socket.SocketSessionConfig;
import org.apache.mina.transport.socket.nio.NioSocketAcceptor;

/**
 *
 * @author Administrator
 */
public class ServerMina {

    public void start(int port) {
        try {
        	//创建服务器监听
            NioSocketAcceptor acceptor = new NioSocketAcceptor();
            
            //设置事件监听触发
            acceptor.setHandler(new SocketMinaHandle());

            //取得拦截
            DefaultIoFilterChainBuilder chain = acceptor.getFilterChain();
            
            //设置协议的拦截器，new了过滤工厂：编码，解码
            chain.addLast("protocol", new ProtocolCodecFilter(new MinaFilterFactory()));
            
            //socket会话的配置
            SocketSessionConfig scfg = acceptor.getSessionConfig();
            
            //绑定端口，用来监听端口
            acceptor.bind(new InetSocketAddress(port));
            
            System.out.println("Server started... " + port);
        } catch (Exception e) {
            e.printStackTrace();
        }

    }
    
    public static void main(String[] args){
        new ServerMina().start(10052);
    }
}
