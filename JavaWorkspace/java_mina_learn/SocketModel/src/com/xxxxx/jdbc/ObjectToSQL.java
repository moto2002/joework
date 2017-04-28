/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.jdbc;

import com.xxxxx.pojo.User;
import java.lang.reflect.Field;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author sf
 */
public class ObjectToSQL
{

	public static void main(String[] args)
	{
		Class cls = User.class;
		String name = cls.getSimpleName();
		String sql = "select * from " + name;
		System.out.println(sql);

		sql = "update " + name + " set ";
		Field[] fields = cls.getDeclaredFields();
		for (int i = 0; i < fields.length; i++)
		{
			Field field = fields[i];
			String fieldName = field.getName();
			if (!fieldName.equals("id"))
			{

				sql += fieldName + " = ?";
				if (i != fields.length - 1)
				{
					sql += ",";
				}
			}
		}
		sql += " where id = ?";
		System.out.println(sql);

		sql = "insert into " + name + "(";
		for (int i = 0; i < fields.length; i++)
		{
			Field field = fields[i];
			String fieldName = field.getName();
			sql += fieldName;
			if (i != fields.length - 1)
			{
				sql += ",";
			}
		}
		sql += ")values(";
		for (int i = 0; i < fields.length; i++)
		{
			sql += "?";
			if (i != fields.length - 1)
			{
				sql += ",";
			}
		}
		sql += ")";
		System.out.println(sql);
	}

	public static String selectSQL(Class cla, KeyValue... kvs)
	{
		// System.out.println(kvs.length);
		String name = cla.getSimpleName();
		String sql = "select * from " + name;
		// if (kvs.length >0) {
		//
		// }
		for (int i = 0; i < kvs.length; i++)
		{
			if (i == 0)
			{
				sql += " where ";
			}
			KeyValue keyValue = kvs[i];
			sql += keyValue.getKey() + " = ?";
			if (i != kvs.length - 1)
			{
				sql += " and ";
			}
		}
		// for (KeyValue keyValue : kvs) {
		// sql += keyValue.getKey() + " = ?";
		// }
		return sql;
	}

	public static String deleteSQL(Object object)
	{
		Class cla = object.getClass();
		String name = cla.getSimpleName();
		String sql = "delete from " + name + " where id = ?";
		return sql;
	}

	public static String updateSQL(Object object)
	{
		Class cla = object.getClass();
		String name = cla.getSimpleName();
		String sql = "update " + name + " set ";
		Field[] fields = cla.getDeclaredFields();
		int length = fields.length;
		String where = " where ";
		for (int i = 0; i < length; ++i)
		{
			try
			{
				fields[i].setAccessible(true);
				if (fields[i].getName().equals("id"))
				{
					where += fields[i].getName();
					// where += "=" + fields[i].get(object);
					where += "= ?";
				} else
				{
					sql += fields[i].getName();
					// sql += "=" + fields[i].get(object);
					sql += "= ?";
					if (i < length - 1)
					{
						sql += ", ";
					}
				}
			} catch (IllegalArgumentException ex)
			{
				Logger.getLogger(ObjectToSQL.class.getName()).log(Level.SEVERE, null, ex);
			}
		}
		sql += where;
		return sql;
	}

	public static String insertSQL(Object object)
	{
		Class cla = object.getClass();
		String name = cla.getSimpleName();
		String insert = "insert into " + name + " (";
		Field[] fields = cla.getDeclaredFields();
		int length = fields.length;
		String value = "";
		for (int i = 0; i < length; ++i)
		{
			insert += fields[i].getName();
			if (i == length - 1)
			{
				insert += ")values(";
				value += "?)";
			} else
			{
				insert += ", ";
				value += "?, ";
			}
		}
		insert += value;
		return insert;
	}
}
