package com.hq.packet;

import java.nio.ByteBuffer;

public class BasePkt
{
	protected short PktType;
	//public int PktSize;
	//public int Operation;
	
	public String GetString(ByteBuffer bf)
	{
		//先获得这个字符串的长度
		int size =bf.getShort();
		byte[] b = new byte[size];
		bf.get(b);
		String s  = new String(b);
		return s;
	}
	
	public ByteBuffer PutString(ByteBuffer bf,String s)
	{		
		byte[] b = s.getBytes();
		short size =(short)b.length;
		bf.putShort(size);
		bf.put(b);		
		return bf;
	}
}

