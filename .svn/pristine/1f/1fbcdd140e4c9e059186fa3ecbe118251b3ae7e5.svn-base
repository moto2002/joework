/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.server.io.mina;

import java.nio.ByteBuffer;
import java.nio.ByteOrder;

import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.filter.codec.CumulativeProtocolDecoder;
import org.apache.mina.filter.codec.ProtocolDecoderOutput;
import org.springframework.util.SystemPropertyUtils;

import com.alibaba.fastjson.JSON;
import com.hq.datapool.DyncDataPool;
import com.hq.define.Link;
import com.hq.mytools.*;
import com.hq.packet.*;
import com.xxxxx.server.client.Client;

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
		System.out.println("解码");
	
		DyncDataPool.GetInstance().buffer = ib.buf();
		DyncDataPool.GetInstance().client = (Client) is.getAttribute(Client.class.getSimpleName());
		
		if(DyncDataPool.GetInstance().buffer.remaining()>=4)
		{
			//取出包大小
			int size = DyncDataPool.GetInstance().buffer.getInt();
			System.out.println("pktsize:"+ size);
			
			if (DyncDataPool.GetInstance().buffer.remaining()>=2)
			{
				//取出包类型
				short type = DyncDataPool.GetInstance().buffer.getShort();	
				System.out.println("type:"+ type);				
				size = size-2;
				
				if (DyncDataPool.GetInstance().buffer.remaining()>=size)
				{
					//解析包内容
					byte[] bbb = new byte[size];
					DyncDataPool.GetInstance().buffer.get(bbb);
					DyncDataPool.GetInstance().Parse(type,bbb);
				}
				
			}		
		}
		
		   return false;	
	}
}
