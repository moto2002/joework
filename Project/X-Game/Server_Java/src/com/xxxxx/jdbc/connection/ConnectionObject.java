/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.jdbc.connection;

import com.xxxxx.jdbc.statement.PreparedStatementObject;
import java.io.Closeable;
import java.sql.Connection;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author Administrator
 */
public class ConnectionObject implements Closeable
{

	private Connection connection;

	public ConnectionObject(Connection connection)
	{
		this.connection = connection;
	}

	public PreparedStatementObject createPreparedStatementObject(String sql)
	{
		try
		{
			PreparedStatementObject preparedStatementObject = new PreparedStatementObject(connection.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS));
			return preparedStatementObject;
		} catch (SQLException ex)
		{
			Logger.getLogger(ConnectionObject.class.getName()).log(Level.SEVERE, null, ex);
		}
		return null;
	}

	public void close()
	{
		try
		{
			connection.close();
		} catch (SQLException ex)
		{
			Logger.getLogger(PreparedStatementObject.class.getName()).log(Level.SEVERE, null, ex);
		}
	}

	public void commit()
	{
		try
		{
			connection.commit();
		} catch (SQLException ex)
		{
			Logger.getLogger(ConnectionObject.class.getName()).log(Level.SEVERE, null, ex);
		}
	}

	public void setAutoCommit(boolean autoCommit)
	{
		try
		{
			connection.setAutoCommit(true);
		} catch (SQLException ex)
		{
			Logger.getLogger(ConnectionObject.class.getName()).log(Level.SEVERE, null, ex);
		}
	}
}
