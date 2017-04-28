/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.server.client;

import java.net.SocketAddress;

/**
 *
 * @author Administrator
 */
public interface ClientSession {

    Object write(Object object);

    Object getAttribute(Object object);

    Object setAttribute(Object key, Object value);
    
    Object setAttribute(Object value);

    Object setAttributeIfAbsent(Object key, Object value);
    
    SocketAddress  getRemoteAddress();
}
