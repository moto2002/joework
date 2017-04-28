/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.jdbc;

import java.lang.reflect.Field;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author sf
 */
public class KeyValue
{

	private String key;
	private Object value;

	public KeyValue(String key, Object value)
	{
		this.key = key;
		this.value = value;
	}

	public String getKey()
	{
		return key;
	}

	public void setKey(String key)
	{
		this.key = key;
	}

	public Object getValue()
	{
		return value;
	}

	public void setValue(Object value)
	{
		this.value = value;
	}

	public static KeyValue[] createKeyValueArray(Object object)
	{

		Class cls = object.getClass();
		Field[] fields = cls.getDeclaredFields();
		KeyValue[] kvs = new KeyValue[fields.length];
		for (int i = 0; i < kvs.length; i++)
		{
			try
			{
				kvs[i] = new KeyValue(fields[i].getName(), fields[i].get(object));
			} catch (IllegalArgumentException ex)
			{
				Logger.getLogger(KeyValue.class.getName()).log(Level.SEVERE, null, ex);
			} catch (IllegalAccessException ex)
			{
				Logger.getLogger(KeyValue.class.getName()).log(Level.SEVERE, null, ex);
			}
		}
		return kvs;
	}

	public static Object[] createValueArray(KeyValue... kvs)
	{
		Object[] objects = new Object[kvs.length];
		for (int i = 0; i < kvs.length; i++)
		{
			objects[i] = kvs[i].getValue();
		}
		return objects;
	}

}
