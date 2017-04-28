/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package socket.io.mina;

import org.apache.mina.core.service.IoHandlerAdapter;
import org.apache.mina.core.session.IdleStatus;
import org.apache.mina.core.session.IoSession;

/**
 * 监听器
 */
public class SocketMinaHandle extends IoHandlerAdapter {

    /**
     * 信息发送
     */
    @Override
    public void messageSent(IoSession session, Object message) {
        System.out.println("messageSent");
    }

    /**
     * 消息接受
     */
    @Override
    public void messageReceived(IoSession session, Object message) {
        System.out.println("messageReceived");
    }

    /**
     * 发生异常
     */
    @Override
    public void exceptionCaught(IoSession session, Throwable cause) {
        System.out.println("exceptionCaught");
        cause.printStackTrace();
    }

    /**
     * session等待
     */
    @Override
    public void sessionIdle(IoSession session, IdleStatus status) {
        System.out.println("sessionIdle");
    }

    /**
     * session关闭
     */
    @Override
    public void sessionClosed(IoSession session) {
        System.out.println("sessionClosed " + session.getRemoteAddress().toString());
    }

    /**
     * session打开
     */
    @Override
    public void sessionOpened(IoSession session) {
        System.out.println("sessionOpened " + session.getRemoteAddress().toString());
    }

    /**
     * session创建
     */
    @Override
    public void sessionCreated(IoSession session) {
        System.out.println("sessionCreated");
    }

}
