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
 * 生成value的数组
 * 
 * @author Administrator
 */
public class ObjectToValue
{

	public static Object deleteValue(Object object)
	{
		Class cls = object.getClass();
		Object id = null;
		try
		{

			Field field = cls.getDeclaredField("id");
			field.setAccessible(true);
			id = field.get(object);
		} catch (IllegalArgumentException ex)
		{
			Logger.getLogger(KeyValue.class.getName()).log(Level.SEVERE, null, ex);
		} catch (NoSuchFieldException ex)
		{
			Logger.getLogger(ObjectToValue.class.getName()).log(Level.SEVERE, null, ex);
		} catch (SecurityException ex)
		{
			Logger.getLogger(ObjectToValue.class.getName()).log(Level.SEVERE, null, ex);
		} catch (IllegalAccessException ex)
		{
			Logger.getLogger(ObjectToValue.class.getName()).log(Level.SEVERE, null, ex);
		}
		return id;
	}

	public static Object[] insertValueArray(Object object)
	{
		Class cls = object.getClass();
		Field[] fields = cls.getDeclaredFields();
		Object[] objects = new Object[fields.length];
		for (int i = 0; i < objects.length; i++)
		{

			try
			{
				fields[i].setAccessible(true);
				objects[i] = fields[i].get(object);

			} catch (IllegalArgumentException ex)
			{
				Logger.getLogger(KeyValue.class.getName()).log(Level.SEVERE, null, ex);
			} catch (IllegalAccessException ex)
			{
				Logger.getLogger(KeyValue.class.getName()).log(Level.SEVERE, null, ex);
			}
		}
		return objects;
	}

	public static Object[] updateValueArray(Object object)
	{
		Class cls = object.getClass();
		Field[] fields = cls.getDeclaredFields();
		Object[] objects = new Object[fields.length];
		int i = 0;
		try
		{
			for (Field field : fields)
			{
				field.setAccessible(true);
				if (field.getName().equals("id"))
				{
					objects[objects.length - 1] = field.get(object);
				} else
				{
					objects[i] = field.get(object);
					i++;
				}
			}
		} catch (IllegalArgumentException ex)
		{
			Logger.getLogger(ObjectToValue.class.getName()).log(Level.SEVERE, null, ex);
		} catch (IllegalAccessException ex)
		{
			Logger.getLogger(ObjectToValue.class.getName()).log(Level.SEVERE, null, ex);
		}
		return objects;
	}
}
