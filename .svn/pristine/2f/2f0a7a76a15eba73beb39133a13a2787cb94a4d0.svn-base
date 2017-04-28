/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.jdbc;

import com.xxxxx.jdbc.connection.ConnectionCache;
import com.xxxxx.jdbc.connection.ConnectionObject;
import com.xxxxx.jdbc.util.ClassUtil;
import com.xxxxx.jdbc.statement.PreparedStatementObject;

import java.sql.ResultSet;
import java.util.List;

import org.apache.commons.lang.ObjectUtils.Null;

/**
 *
 * @author sf
 */
public class DBUtil
{

	public static Object findByID(Class cla, int id)
	{
		return findOne(cla, new KeyValue("id", id));
	}

	public static Object findByName(Class cla, String name)
	{
		//return findOne(cla, new KeyValue("name", name));
		return findOne(cla, new KeyValue("username", name));
	}

	public static Object findOne(Class cla, KeyValue... kvs)
	{

		String sql = ObjectToSQL.selectSQL(cla, kvs);
		//System.out.println(sql);
		try (ConnectionObject connectionObject = ConnectionCache.createGameConnectionObject(); PreparedStatementObject statementObject = connectionObject.createPreparedStatementObject(sql))
		{

			ResultSet resultSet = statementObject.executeQuery(KeyValue.createValueArray(kvs));
			return SQLToObject.one(cla, resultSet);
		}catch (Exception e) {
			return null;
		}
	}

	public static List findAll(Class cla)
	{
		return findList(cla);
	}

	public static List findList(Class cla, KeyValue... kvs)
	{
		String sql = ObjectToSQL.selectSQL(cla, kvs);
		try (ConnectionObject connectionObject = ConnectionCache.createGameConnectionObject(); PreparedStatementObject statementObject = connectionObject.createPreparedStatementObject(sql))
		{

			ResultSet resultSet = statementObject.executeQuery(KeyValue.createValueArray(kvs));
			return SQLToObject.list(cla, resultSet);
		}

	}

	public static void save(Object object)
	{
		String sql = ObjectToSQL.insertSQL(object);
		try (ConnectionObject connectionObject = ConnectionCache.createGameConnectionObject(); PreparedStatementObject statementObject = connectionObject.createPreparedStatementObject(sql))
		{
			Object[] objects = ObjectToValue.insertValueArray(object);
			int id = statementObject.executeSave(objects);
			ClassUtil.setID(object, id);
		}

	}

	public static int update(Object object)
	{
		String sql = ObjectToSQL.updateSQL(object);

		try (ConnectionObject connectionObject = ConnectionCache.createGameConnectionObject(); PreparedStatementObject statementObject = connectionObject.createPreparedStatementObject(sql))
		{

			Object[] objects = ObjectToValue.updateValueArray(object);
			return statementObject.executeUpdate(objects);
		}

	}

	public static int delete(Object object)
	{
		String sql = ObjectToSQL.updateSQL(object);

		try (ConnectionObject connectionObject = ConnectionCache.createGameConnectionObject(); PreparedStatementObject statementObject = connectionObject.createPreparedStatementObject(sql))
		{
			Object id = ObjectToValue.deleteValue(object);

			return statementObject.executeUpdate(id);
		}

	}

}
