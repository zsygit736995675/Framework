using System.Collections;
using System.Collections.Generic;
using ClientCore;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

/// <summary>
/// 玩家状态
/// </summary>
public enum PlayerState
{

}



/// <summary>
/// 玩家角色基类
/// </summary>
public class Player 
{

    /// <summary>
    /// 用户id
    /// </summary>
    int uid = 0;
    public int Uid { get { return uid; } set { uid = value; } }

    /// <summary>
    /// 用户名称
    /// </summary>
    string name = "";
    public string Name { get { return name; } set { name = value; } }

    string third_id = "";
    public string Third_Id { get { return third_id; }set { third_id = value; } }

    /// <summary>
    /// 状态  0 离线   1 在线     2 对战中
    /// </summary>
    int status = 0;
    public int Status { get { return status; } set { status = value; } }

    /// <summary>
    /// 角色id（对应角色表）
    /// </summary>
    int currentEntityID = 6;
    public int CurrentEntityID { get { return currentEntityID; } set { currentEntityID = value; } }

    /// <summary>
    /// 拖尾id
    /// </summary>
    int smearid = 6;
    public int CurrentSmearID { get { return smearid; } set { smearid = value; } }

    int total_stars = 0;
    public int Total_Stars { get { return total_stars; } set { total_stars = value; } }

    int total_gold = 0;
    public int Total_Gold { get { return total_gold; } set { total_gold = value; } }

    int total_skins = 0;
    public int Total_Skins { get { return total_skins; } set { total_skins = value; } }

    /// <summary>
    /// 金币
    /// </summary>
    int gold = 0;
    public int Gold { get { return gold; } set { gold = value; } }

    int stars = 0;
    public int Stars { get { return stars; } set { stars = value; } }

    
    int hp = 10;
    public int Hp { get { return hp; } set { hp = value; } }

    int hp_time = 0;
    public int Hp_Time { get { return hp_time; }set{ hp_time = value; } }

    int max_level = 0;
    public int Max_Level { get { return max_level; } set { max_level = value; } }

    int current_level = 0;
    public int Current_Level { get { return current_level; } set { current_level = value; } }

    /// <summary>
    /// 头像
    /// </summary>
    string icon = "";
    public string Icon { get { return icon; } set { icon = value; } }

    /// <summary>
    /// 设备编号
    /// </summary>
    string deviceid = "";
    public string Deviceid { get { return deviceid; } set { deviceid = value; } }
    /// <summary>
    /// 国家编码
    /// </summary>
    string country_code = "";
    public string Country_Code { get { return country_code; } set { country_code = value; } }
    /// <summary>
    /// 语言编码
    /// </summary>
    string lang_code = "";
    public string Lang_Code { get { return lang_code; } set { lang_code = value; } }

    /// <summary>
    /// 设备型号
    /// </summary>
    string device_version = "";
    public string Device_Version { get { return device_version; } set { device_version = value; } }
    /// <summary>
    /// 系统版本 
    /// </summary>
    string system_version = "";
    public string System_Version { get { return system_version; } set { system_version = value; } }

    /// <summary>
    /// 是否注册
    /// </summary>
    int is_register;
    public int Is_Register { get { return is_register; } set { is_register = value; } }

    int level_history_rank;
    public int Level_History_Rank { get { return level_history_rank; } set { level_history_rank = value; } }

    string login_type;
    public string Login_Type { get { return login_type; } set { login_type = value; } }


    /// <summary>
    /// 增加体力的按钮显示概率
    /// </summary>
    float btnHpNum = 0.3f;
    public float BtnHpNum { get { return btnHpNum; } }

    /// <summary>
    /// 商店试用按钮显示概率
    /// </summary>
    float skinfreeNum = 0.3f;
    public float SkinFreeNum { get { return skinfreeNum; } }




  
}