/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.jdbc.util;

import java.lang.reflect.Field;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author Administrator
 */
public class ClassUtil
{

	public static int getID(Object object)
	{
		try
		{
			Class cls = object.getClass();
			Field field = cls.getDeclaredField("id");
			field.setAccessible(true);
			Object o = field.get(object);
			Integer id = (Integer) o;
			return id;
		} catch (NoSuchFieldException ex)
		{
			Logger.getLogger(ClassUtil.class.getName()).log(Level.SEVERE, null, ex);
		} catch (SecurityException ex)
		{
			Logger.getLogger(ClassUtil.class.getName()).log(Level.SEVERE, null, ex);
		} catch (IllegalArgumentException ex)
		{
			Logger.getLogger(ClassUtil.class.getName()).log(Level.SEVERE, null, ex);
		} catch (IllegalAccessException ex)
		{
			Logger.getLogger(ClassUtil.class.getName()).log(Level.SEVERE, null, ex);
		}
		return 0;
	}

	public static void setID(Object object, int id)
	{
		try
		{
			Class cls = object.getClass();
			Field field = cls.getDeclaredField("id");
			field.setAccessible(true);
			field.set(object, id);
		} catch (NoSuchFieldException ex)
		{
			Logger.getLogger(ClassUtil.class.getName()).log(Level.SEVERE, null, ex);
		} catch (SecurityException ex)
		{
			Logger.getLogger(ClassUtil.class.getName()).log(Level.SEVERE, null, ex);
		} catch (IllegalArgumentException ex)
		{
			Logger.getLogger(ClassUtil.class.getName()).log(Level.SEVERE, null, ex);
		} catch (IllegalAccessException ex)
		{
			Logger.getLogger(ClassUtil.class.getName()).log(Level.SEVERE, null, ex);
		}
	}
}
