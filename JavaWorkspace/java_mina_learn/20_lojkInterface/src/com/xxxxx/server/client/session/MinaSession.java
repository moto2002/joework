/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.server.client.session;

import com.xxxxx.server.client.ClientSession;
import java.net.SocketAddress;
import org.apache.mina.core.session.IoSession;

/**
 *
 * @author Administrator
 */
public class MinaSession implements ClientSession {

    private IoSession session;

    public MinaSession(IoSession session) {
        this.session = session;
    }

    @Override
    public Object write(Object object) {
        return session.write(object);
    }

    @Override
    public Object getAttribute(Object object) {
        if (object instanceof String) {
            return session.getAttribute(object);
        } else if (object instanceof Class) {
            return session.getAttribute(((Class) object).getSimpleName());
        } else {
            return session.getAttribute(object.getClass().getSimpleName());
        }
    }

    @Override
    public Object setAttribute(Object key, Object value) {
        return session.setAttribute(key, value);
    }

    @Override
    public Object setAttributeIfAbsent(Object key, Object value) {
        return session.setAttributeIfAbsent(key, value);
    }

    @Override
    public Object setAttribute(Object value) {
        return setAttribute(value.getClass().getSimpleName(), value);
    }

    @Override
    public SocketAddress getRemoteAddress() {
        return session.getRemoteAddress();
    }

}
