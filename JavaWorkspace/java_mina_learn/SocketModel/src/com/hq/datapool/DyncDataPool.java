package com.hq.datapool;

import java.nio.ByteBuffer;

import com.alibaba.fastjson.JSON;
import com.hq.define.Define;
import com.hq.define.Link;
import com.hq.mytools.ConvertTools;
import com.hq.packet.Request_Login;
import com.xxxxx.server.client.Client;

public class DyncDataPool
{
	
	public Request_Login request_Login = new Request_Login();
	
	// 收到的东西放到buffer
	public ByteBuffer buffer;

	public Client client;
	
	private static DyncDataPool instance;
	public static DyncDataPool GetInstance()
	{
		if (instance == null)
		{
			instance = new DyncDataPool();
		}
		return instance;
	}

	public void Parse(Short type,byte[] dataBytes)
	{	
		String s = ConvertTools.Bytes2String(dataBytes);
		System.out.println("JsonString:"+s);
		
		switch (type)
		{
		case Define.REQUEST_LOGIN:
			request_Login = new Request_Login();		
			request_Login = JSON.parseObject(s,Request_Login.class);
			request_Login.Check();
			break;

		default:
			break;
		}
		
		
		// String cname = Link.Link.get(type);
		// System.out.println("type link is :"+cname);
		//
		// //进行反射咯
		// Class cls = Class.forName(cname);
		// Method method = cls.getDeclaredMethod("Read",ByteBuffer.class);
		// Object object = method.invoke(null, buffer);
		//
		//
		// Class classname = Class.forName("com.hq.exe.Login_EXE");
		// classname.getMethod("execute",cls,Client.class).invoke(null,
		// object,client);

//		switch (type)
//		{
//		case Define.REQUEST_LOGIN:
//			int size = buffer.limit() - buffer.position();
//			System.out.println("datasize:"+size);
//			Request_Login login = new Request_Login();
//			login.Read(buffer);
//			buffer2 = ByteBuffer.allocate(1024);
//			login.DoIt(buffer2);
//			break;
//
//		default:
//			break;
//		}
		
		
		
	}
	
	public void SendBack(short type,String dataJson)
	{	
		System.out.println("return type:"+type);
		byte[] typeb = ConvertTools.Short2Bytes(type);	
		byte[] datab = ConvertTools.String2Bytes(dataJson);		
		byte[] newb = ConvertTools.BytesAdd(typeb, datab);	
		client.getSession().write(newb);
	}

}
