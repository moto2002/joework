/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.jdbc;

import com.xxxxx.jdbc.statement.PreparedStatementObject;
import java.lang.reflect.Field;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * 结果集生成object
 * 
 * @author sf
 */
public class SQLToObject
{

	public static Object one(Class cla, ResultSet resultSet, boolean close)
	{
		try
		{
			Object object = cla.newInstance();
			for (Field field : cla.getDeclaredFields())
			{
				String name = field.getName();
				field.setAccessible(true);// 设置可访问的
				field.set(object, resultSet.getObject(name));
			}
			return object;
		} catch (InstantiationException ex)
		{
			Logger.getLogger(SQLToObject.class.getName()).log(Level.SEVERE, null, ex);
		} catch (IllegalAccessException ex)
		{
			Logger.getLogger(SQLToObject.class.getName()).log(Level.SEVERE, null, ex);
		} catch (SQLException ex)
		{
			Logger.getLogger(SQLToObject.class.getName()).log(Level.SEVERE, null, ex);
		} finally
		{
			try
			{
				if (close)
				{
					resultSet.close();
				}
			} catch (SQLException ex)
			{
				Logger.getLogger(PreparedStatementObject.class.getName()).log(Level.SEVERE, null, ex);
			}
		}
		return null;
	}

	public static Object one(Class cla, ResultSet resultSet)
	{
		try
		{
			while (resultSet.next())
			{
				return one(cla, resultSet, true);
			}
		} catch (SQLException ex)
		{
			Logger.getLogger(SQLToObject.class.getName()).log(Level.SEVERE, null, ex);
		}
		return null;
	}

	public static List list(Class cla, ResultSet resultSet)
	{
		try
		{
			List list = new ArrayList();
			while (resultSet.next())
			{
				list.add(one(cla, resultSet, false));
			}
			return list;
		} catch (SQLException ex)
		{
			Logger.getLogger(SQLToObject.class.getName()).log(Level.SEVERE, null, ex);
		} finally
		{
			try
			{
				resultSet.close();
			} catch (SQLException ex)
			{
				Logger.getLogger(PreparedStatementObject.class.getName()).log(Level.SEVERE, null, ex);
			}
		}
		return null;
	}
}
