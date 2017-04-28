/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.jdbc.connection;

import com.xxxxx.jdbc.DBUtil;
import com.mchange.v2.c3p0.ComboPooledDataSource;
import java.beans.PropertyVetoException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author Administrator
 */
public class ConnectionCache
{

	private static ComboPooledDataSource cpds = createC3P0ConnectionPool();

	private static ComboPooledDataSource createC3P0ConnectionPool()
	{
		try
		{
			cpds = new ComboPooledDataSource();
			cpds.setDriverClass("com.mysql.jdbc.Driver"); // loads the jdbc
															// driver
			cpds.setJdbcUrl("jdbc:mysql://localhost:3306/game");
			cpds.setUser("root");
			cpds.setPassword("111111");
			cpds.setMaxStatements(180);// 最好设置一个值，设置成0，就把预编译的功能关闭了
			return cpds;
			// return
			// DriverManager.getConnection("jdbc:mysql://localhost:3306/test",
			// "root", "root");
		} catch (PropertyVetoException ex)
		{
			Logger.getLogger(ConnectionCache.class.getName()).log(Level.SEVERE, null, ex);
		}
		return null;
	}

	public static ConnectionObject createGameConnectionObject()
	{
		try
		{
			ConnectionObject connectionObject = new ConnectionObject(cpds.getConnection());
			return connectionObject;
		} catch (SQLException ex)
		{
			Logger.getLogger(DBUtil.class.getName()).log(Level.SEVERE, null, ex);
		}
		return null;
	}

}
