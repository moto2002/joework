using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public class test : MonoBehaviour
{
    void Start()
    {
        //根据类的名字，获取类型
        Type t = Type.GetType("CPerson");

        //使用指定类型的默认构造函数来创建该类型的实例
        object inst = Activator.CreateInstance(t);

        //查看一下，这个实例里面有哪些 方法，字段，属性
        MethodInfo[] methods =  t.GetMethods();
        FieldInfo[] fields = t.GetFields();
        PropertyInfo[] propertys = t.GetProperties();

        for (int i = 0; i < methods.Length; i++)
        {
            Debug.Log("methods:"+ methods[i].Name);
        }

        for (int j = 0; j < fields.Length; j++)
        {
            Debug.Log("fields:"+fields[j].Name);
        }

        for (int z = 0; z < propertys.Length; z++)
        {
            Debug.Log("properties:"+ propertys[z].Name);

        }

        //调用实例里面的方法
        MethodInfo methodInfo = t.GetMethod("Say");
        methodInfo.Invoke(inst,new object[]{"hello refection"});

		t.GetField("name").SetValue(inst,"joe");
    }
}
