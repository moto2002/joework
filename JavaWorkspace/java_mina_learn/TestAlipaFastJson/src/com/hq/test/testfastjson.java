package com.hq.test;

import com.alibaba.fastjson.JSON;

public class testfastjson
{
	public static void main(String[] args)
	{
		group  g = new group();
		g.teamname = "gameteam";
		
		Person p1 = new Person();
		p1.name = "joe";
		p1.age = 22;
		
		Person p2 = new Person();
		p2.name = "hq";
		p2.age = 23;
		
		g.team.add(p1);
		g.team.add(p2);
	
		
		String s = 	JSON.toJSONString(g);
		System.out.println(s);
		
		
		String sss = "{\"teamname\":\"gameteam\",\"team\":[{\"age\":22,\"name\":\"joe\"},{\"age\":23,\"name\":\"hq\"}]}";
		System.out.println(sss);
		
		group _p = new group();
		_p = JSON.parseObject(sss,group.class);
		System.out.println(_p.teamname);
		System.out.println(_p.team.size());
		

	
	}

}
