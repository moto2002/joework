package com.hq.packet;

import java.nio.ByteBuffer;

import com.hq.define.Define;

public class Response_Login extends BasePkt
{
	public int result;
	
	public Response_Login()
	{
		PktType = Define.RESPONE_LOGIN;
	}
	
	public void Write(ByteBuffer bf)
	{
		bf.putShort(PktType);
		bf.putInt(result);
		
		System.out.println("result:"+result);
	}
}
