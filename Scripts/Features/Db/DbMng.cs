using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 表格名称
/// </summary>
public enum Table 
{
    Player
}

/// <summary>
/// 数据库管理
/// </summary>
public class DbMng : BaseClass<DbMng>
{


    // DbMng.Ins.InsertInto(Table.Player, new string[] { "'zsy'", "'11111111'" });

    // DbMng.Ins.UpdateInto(Table.Player, new string[] { "name", "device" }, new string[] { "'lalal'", "'567894'" }, "name", "'zsy'");

    // DbMng.Ins.Delete(Table.Player, new string[] { "name", "device" }, new string[] { "'lalal'", "'567894'" });

    //  DbMng.Ins.Db.SelectWhere(Table.Player.ToString(),);

    DbAccess db;

    public DbAccess Db {
        get {
            if (db == null)
            {
                db = new DbAccess("data source=" + Application.persistentDataPath + "/MathDB.db");

                CreateTable();
            }
            return db;
        }
    }


    private void CreateTable()
    {
        Db.CreateTable(Table.Player.ToString(), TableStruct.playerNames, TableStruct.playerTypes);
    }

    public void InsertInto(Table table,string[] values)
    {
        Db.InsertInto(table.ToString(), values);
    }

    public void UpdateInto(Table table, string[] cols, string[] colsvalues, string selectkey, string selectvalue)
    {
        Db.UpdateInto(table.ToString(), cols, colsvalues,selectkey,selectvalue);
    }
    public void Delete(Table table,string[] cols, string[] colsvalues)
    {
        Db.Delete(table.ToString(), cols, colsvalues);
    }


}

public class TableStruct
{
    public  static string[] playerNames = {"device","" };
    public  static string[] playerTypes = {"text","" };


}


