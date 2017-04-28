/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.xxxxx.jdbc.statement;

import com.xxxxx.jdbc.DBUtil;
import java.io.Closeable;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *‘§±‡“Î
 * @author sf
 */
public class PreparedStatementObject implements Closeable
{

	private PreparedStatement preparedStatement;

	public PreparedStatementObject(PreparedStatement preparedStatement)
	{
		this.preparedStatement = preparedStatement;
	}

	private void setValues(Object... objects)
	{

		for (int i = 0; i < objects.length; i++)
		{
			try
			{
				// System.out.println(2555);
				preparedStatement.setObject(i + 1, objects[i]);
			} catch (SQLException ex)
			{
				Logger.getLogger(PreparedStatementObject.class.getName()).log(
						Level.SEVERE, null, ex);
			}
		}
	}

	public ResultSet executeQuery(Object... objects)
	{
		try
		{
			setValues(objects);
			return preparedStatement.executeQuery();
		} catch (SQLException ex)
		{
			Logger.getLogger(DBUtil.class.getName())
					.log(Level.SEVERE, null, ex);
		}
		return null;
	}

	public int executeSave(Object... objects)
	{
		ResultSet resultSet = null;
		try
		{
			setValues(objects);
			preparedStatement.executeUpdate();
			resultSet = preparedStatement.getGeneratedKeys();
			while (resultSet.next())
			{
				return resultSet.getInt(1);
			}
		} catch (SQLException ex)
		{
			Logger.getLogger(DBUtil.class.getName())
					.log(Level.SEVERE, null, ex);
		} finally
		{
			try
			{
				resultSet.close();
			} catch (SQLException ex)
			{
				Logger.getLogger(PreparedStatementObject.class.getName()).log(
						Level.SEVERE, null, ex);
			}
		}
		return 0;
	}

	public int executeUpdate(Object... objects)
	{
		try
		{
			setValues(objects);
			preparedStatement.execute();
			return preparedStatement.getUpdateCount();
		} catch (SQLException ex)
		{
			Logger.getLogger(DBUtil.class.getName())
					.log(Level.SEVERE, null, ex);
		}
		return 0;
	}

	public void close()
	{
		try
		{
			preparedStatement.close();
		} catch (SQLException ex)
		{
			Logger.getLogger(PreparedStatementObject.class.getName()).log(
					Level.SEVERE, null, ex);
		}
	}

}
