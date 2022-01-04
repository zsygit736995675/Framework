using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DbAccess 
{
    private SqliteConnection dbConnection;

    private SqliteCommand dbCommand;

    private SqliteDataReader reader;

    /// <summary>
    /// 链接数据库 
    /// </summary>
    /// <param name="connectionString">数据库名称</param>
    public DbAccess(string connectionString)
    {
        OpenDB(connectionString);
    }

    private void OpenDB(string connectionString)
    {
        try
        {
            dbConnection = new SqliteConnection(connectionString);

            dbConnection.Open();

            Debug.Log("Connected to db");
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public void CloseSqlConnection()
    {
        if (dbCommand != null)
        {
            dbCommand.Dispose();
        }

        dbCommand = null;

        if (reader != null)
        {
            reader.Dispose();
        }

        reader = null;

        if (dbConnection != null)
        {
            dbConnection.Close();
        }

        dbConnection = null;

        Debug.Log("Disconnected from db.");
    }

    /// <summary>
    /// 执行sql语句
    /// </summary>
    /// <param name="sqlQuery"></param>
    /// <returns></returns>
    public SqliteDataReader ExecuteQuery(string sqlQuery)
    {
        dbCommand = dbConnection.CreateCommand();

        dbCommand.CommandText = sqlQuery;

        try
        {
            reader = dbCommand.ExecuteReader();
        }
        catch (Exception e)
        {
            Debug.LogError("ExecuteQuery:"+e.ToString());
        }

        return reader;
    }

    public SqliteDataReader ReadFullTable(string tableName)
    {
        string query = "SELECT * FROM " + tableName;

        return ExecuteQuery(query);
    }

    /// <summary>
    /// 插入数据
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public SqliteDataReader InsertInto(string tableName, string[] values)
    {
        string query = "INSERT INTO " + tableName + " VALUES (" + values[0];

        for (int i = 1; i < values.Length; ++i)
        {
            query += ", " + values[i];
        }
        query += ")";

        Debug.Log("InsertInto:" + query);

        return ExecuteQuery(query);
    }


    /// <summary>
    /// 更改数据
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="cols"></param>
    /// <param name="colsvalues"></param>
    /// <param name="selectkey"></param>
    /// <param name="selectvalue"></param>
    /// <returns></returns>
    public SqliteDataReader UpdateInto(string tableName, string[] cols, string[] colsvalues, string selectkey, string selectvalue)
    {

        string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + colsvalues[0];

        for (int i = 1; i < colsvalues.Length; ++i)
        {

            query += ", " + cols[i] + " =" + colsvalues[i];
        }

        query += " WHERE " + selectkey + " = " + selectvalue + " ";

        Debug.Log("UpdateInto:"+query);
        return ExecuteQuery(query);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="cols"></param>
    /// <param name="colsvalues"></param>
    /// <returns></returns>
    public SqliteDataReader Delete(string tableName, string[] cols, string[] colsvalues)
    {
        string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + colsvalues[0];

        for (int i = 1; i < colsvalues.Length; ++i)
        {

            query += " or " + cols[i] + " = " + colsvalues[i];
        }
        Debug.Log("Delete:"+query);
        return ExecuteQuery(query);
    }

    /// <summary>
    /// 插入具体的
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="cols"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public SqliteDataReader InsertIntoSpecific(string tableName, string[] cols, string[] values)
    {

        if (cols.Length != values.Length)
        {

            throw new SqliteException("columns.Length != values.Length");

        }

        string query = "INSERT INTO " + tableName + "(" + cols[0];

        for (int i = 1; i < cols.Length; ++i)
        {

            query += ", " + cols[i];

        }

        query += ") VALUES (" + values[0];

        for (int i = 1; i < values.Length; ++i)
        {

            query += ", " + values[i];

        }

        query += ")";

        Debug.Log("InsertIntoSpecific:"+query);
        return ExecuteQuery(query);

    }

    /// <summary>
    /// 删除所有
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public SqliteDataReader DeleteContents(string tableName)
    {
        string query = "DELETE FROM " + tableName;
        return ExecuteQuery(query);
    }


    /// <summary>
    /// 创建表格 ,如果表格不存在创建，存在跳过
    /// </summary>
    /// <param name="name"> 表名</param>
    /// <param name="col">字段名 </param>
    /// <param name="colType">字段类型</param>
    /// <returns></returns>
    public SqliteDataReader CreateTable(string name, string[] col, string[] colType)
    {
        if (col.Length != colType.Length)
        {
            throw new SqliteException("columns.Length != colType.Length");
        }

        string query = "CREATE TABLE if not exists " + name + " (" + col[0] + " " + colType[0];

        for (int i = 1; i < col.Length; ++i)
        {
            query += ", " + col[i] + " " + colType[i];
        }

        query += ")";

        Debug.Log("CreateTable:" + query);
        return ExecuteQuery(query);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="items"></param>
    /// <param name="col"></param>
    /// <param name="operation"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public SqliteDataReader SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)
    {
        if (col.Length != operation.Length || operation.Length != values.Length) {

            throw new SqliteException("col.Length != operation.Length != values.Length");
        }

        string query = "SELECT " + items[0];

        for (int i = 1; i < items.Length; ++i)
        {
            query += ", " + items[i];
        }

        query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";

        for (int i = 1; i < col.Length; ++i)
        {
            query += " AND " + col[i] + operation[i] + "'" + values[0] + "' ";
        }

        Debug.LogError("SelectWhere:"+query);
        return ExecuteQuery(query);
    }


}
