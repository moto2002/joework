package com.hq.packet;

import java.nio.ByteBuffer;

import com.alibaba.fastjson.JSON;
import com.hq.datapool.DyncDataPool;
import com.hq.define.Define;
import com.xxxxx.jdbc.DBUtil;
import com.xxxxx.pojo.User;
import com.xxxxx.server.client.Client;

public class Request_Login 
{
	private int id;
	private String username;
	private String password;
	
	public int getId()
	{
		return id;
	}
	public void setId(int id)
	{
		this.id = id;
	}
	public String getUsername()
	{
		return username;
	}

	public void setUsername(String username)
	{
		this.username = username;
	}

	public String getPassword()
	{
		return password;
	}

	public void setPassword(String password)
	{
		this.password = password;
	}
	
	public void Check()
	{
		System.out.println(username+":"+password);
		
//		User us = (User)DBUtil.findByID(User.class,1);
//		String jsonString3 = JSON.toJSONString(us);
//		System.out.println("jsonstring:"+jsonString3);
		

		Object obj =DBUtil.findByName(Request_Login.class, username);
		if (obj!=null)
		{
			Request_Login u=(Request_Login)obj;
			String jsonString1 = JSON.toJSONString(u);
			System.out.println("jsonstring:"+jsonString1);
	
			if(username.equals(u.getUsername()))
			{			
				Response_Login result = new Response_Login();
				result.result =1;
				String jsonString = JSON.toJSONString(result);
				System.out.println("return jsonstring:"+jsonString);
				DyncDataPool.GetInstance().SendBack(result.PktType,jsonString);
			}
			else 
			{
				Response_Login result = new Response_Login();
				result.result =0;
				String jsonString = JSON.toJSONString(result);
				System.out.println("return jsonstring:"+jsonString);
				//发送error的数据包
				//DyncDataPool.GetInstance().SendBack(result.PktType,jsonString);
			}
			
		}
		else
		{
			System.out.println("on have this user");
//			//没有就创建一个
//			DBUtil.save(DyncDataPool.GetInstance().request_Login);
//			System.out.println("create this user ok");
//			
//			Response_Login result = new Response_Login();
//			result.result =1;
//			String jsonString = JSON.toJSONString(result);
//			System.out.println("return jsonstring:"+jsonString);			
//			DyncDataPool.GetInstance().SendBack(result.PktType,jsonString);
			
			Response_Login result = new Response_Login();
			result.result =0;
			String jsonString = JSON.toJSONString(result);
			System.out.println("return jsonstring:"+jsonString);
			DyncDataPool.GetInstance().SendBack(result.PktType,jsonString);
		}
		

	}
	
	
	
//	public void Read(ByteBuffer buffer)
//	{
//		username = GetString(buffer);		
//		password = GetString(buffer);	
//		
//		System.out.println("username:"+username +"|password:"+username);
//	}		
//	
//	public void DoIt(ByteBuffer buffer)
//	{
//	
//		if(username.equals("hongqiao") && password.equals("123456"))
//		{
//			System.out.println("true");
//			Response_Login loginresult = new Response_Login();
//			loginresult.result = 1;
//			loginresult.Write(buffer);
//
//		}
//		else 
//		{
//			System.out.println("false");
//			Response_Login loginresult = new Response_Login();
//			loginresult.result = 0;
//			loginresult.Write(buffer);			
//		}
//	}
	

}
