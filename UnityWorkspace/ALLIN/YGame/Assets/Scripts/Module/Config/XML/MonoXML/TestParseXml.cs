using System;
using UnityEngine;
using System.Collections;
using System.Reflection;
using Mono.Xml;
using System.Security;

public class TestParseXml : MonoBehaviour
{
    public TextAsset ta;
	void Start ()
	{
	    string s = ta.text.Trim();
        SecurityParser sp = new SecurityParser ();
		sp.LoadXml (s);
		SecurityElement se = sp.ToXml ();
		foreach (SecurityElement child in se.Children) 
		{
			//比对下是否使自己所需要得节点
			if(child.Tag == "person")
			{
				//获得节点得属性
				string id = child.Attribute("id");
				string age = child.Attribute("age");
				string name = child.Attribute("name");
				Debug.Log("id:" + id + " age:" + age + " name:" + name);
			}
		}

        //根据类的名字，获取类型
        Type t = Type.GetType("PersonItem");
        //使用指定类型的默认构造函数来创建该类型的实例
        object inst = Activator.CreateInstance(t);
        FieldInfo[] fields = t.GetFields();
        for (int j = 0; j < fields.Length; j++)
        {
            Debug.Log("fields:" + fields[j].Name);
        }
        t.GetField("id").SetValue(inst, 1);
        t.GetField("age").SetValue(inst, 22);
        t.GetField("name").SetValue(inst, "joe");

        //调用实例里面的方法
        MethodInfo methodInfo = t.GetMethod("Say");
        methodInfo.Invoke(inst, new object[] { "hello refection" });

        TestPerson  tp = new TestPerson();
        tp.pi = inst as PersonItem;
        Debug.Log(tp.pi.name);
    }


}
