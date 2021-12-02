using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;



/// <summary>
/// 本地数据管理
/// </summary>
public class PlayerPrefsManager
{

    /// <summary>
    /// 国家
    /// </summary>
    static string country;

    /// <summary>
    /// ios是否是国内
    /// </summary>
    public static bool IsDomestic
    {
        get {
            if (string.IsNullOrEmpty(country))
            {
                return false;
            }
            else if (country == "CN")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// 玩家名称用作key
    /// </summary>
    static string PlayerUid
    {
        get {
            return "";
        }
    }

    /// <summary>
    /// 是否第一次进游戏
    /// </summary>
    public static bool IsFirst
    {
        get
        {
            string swi = PlayerPrefs.GetString(PlayerUid + "first");
            if (swi == "")
            {
                IsFirst = false;
                return true;
            }
            return bool.Parse(swi);
        }

        set
        {
            PlayerPrefs.SetString(PlayerUid + "first", value.ToString());
        }
    }

    private static string device;
    /// <summary>
    /// 设备编码
    /// </summary>
    public static string Device
    {
        get
        {
            return device;
        }

        set
        {
            device = value;
        }
    }

    public static void InitDevice()
    {
#if UNITY_EDITOR
            device = SystemInfo.deviceUniqueIdentifier;
#elif UNITY_ANDROID && !UNITY_EDITOR
		    device = GetDevice();
#elif UNITY_IOS && !UNITY_EDITOR
            getUUIDInKeychain();
            country =  getCountryCode();
#endif
    }

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void _getUUIDInKeychain();
    public static void getUUIDInKeychain()
    {
        _getUUIDInKeychain();
    }

    [DllImport("__Internal")]
    private static extern string _getCountryCode();
    public static string getCountryCode()
    {
        return _getCountryCode();
    }
#endif



    public static string GetDevice()
    {
        AndroidJavaObject _ajc = new AndroidJavaObject("com.davidwang.deviceid.Unity2Android");
        AndroidJavaObject context = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"); //获得Context
        return _ajc.CallStatic<string>("getUniversalID", context);
    }

   

    /// <summary>
    /// 摇杆灵敏度
    /// </summary>
    public static float Sensitivity
    {
        get
        {
            float so = PlayerPrefs.GetFloat(PlayerUid + "Sensitivity");
            if (so == 0)
            {
                Sensitivity = 10;
                so = 10;
            }
            return so;
        }

        set
        {
            PlayerPrefs.SetFloat(PlayerUid + "Sensitivity", value);
        }
    }


    /// <summary>
    /// 控制方式 true打开陀螺仪 false关闭陀螺仪
    /// </summary>
    public static bool GyroSwitch
    {
        get
        {
            string swi = PlayerPrefs.GetString(PlayerUid + "GyroSwitch");
            if (swi == "")
            {
                GyroSwitch = false;
                return false;
            }
            return bool.Parse(swi);
        }

        set
        {
            PlayerPrefs.SetString(PlayerUid + "GyroSwitch", value.ToString());
        }
    }

    /// <summary>
    /// 手机振动开关
    /// </summary>
    public static bool VibSwitch
    {
        get
        {
            string swi = PlayerPrefs.GetString(PlayerUid + "VibSwitch");
            if (swi == "")
            {
                VibSwitch = false;
                return false;
            }
            return bool.Parse(swi);
        }

        set
        {
            PlayerPrefs.SetString(PlayerUid + "VibSwitch", value.ToString());
        }
    }
    /// <summary>
    /// 复活卡开关
    /// </summary>
    public static bool AngleSwitch
    {
        get
        {
            string swi = PlayerPrefs.GetString(PlayerUid + "AngleSwitch");
            if (swi == "")
            {
                AngleSwitch = false;
                return false;
            }
            return bool.Parse(swi);
        }

        set
        {
            PlayerPrefs.SetString(PlayerUid + "AngleSwitch", value.ToString());
        }
    }

    /// <summary>
    /// 试用开关
    /// </summary>
    public static bool FreeSwitch
    {
        get
        {
            string swi = PlayerPrefs.GetString(PlayerUid + "FreeSwitch");
            if (swi == "")
            {
                FreeSwitch = false;
                return false;
            }
            return bool.Parse(swi);
        }

        set
        {
            PlayerPrefs.SetString(PlayerUid + "FreeSwitch", value.ToString());
        }
    }

    /// <summary>
    /// 携带的技能
    /// </summary>
    public static string CarryProp
    {
        get
        {
            string swi = PlayerPrefs.GetString(PlayerUid + "CarryProp");
            if (swi == "")
            {
                swi = "[-1,-1,-1]";
            }
            return swi;
        }

        set
        {
            PlayerPrefs.SetString(PlayerUid + "CarryProp", value.ToString());
        }
    }

    /// <summary>
    /// 新手引导本地记录
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool GetGuideSwitch(int index)
    {
        string swi = PlayerPrefs.GetString(PlayerUid + "GuideSwitch" + index);
        if (swi == "")
        {
            return false;
        }
        return bool.Parse(PlayerPrefs.GetString(PlayerUid + "GuideSwitch" + index));
    }

    public static void SetGuideSwitch(int index, string b)
    {
        PlayerPrefs.SetString(PlayerUid + "GuideSwitch" + index, b);
    }

    public static bool UserEvaluation
    {
        get
        {
            string swi = PlayerPrefs.GetString(PlayerUid + "UserEvaluation");
            if (swi == "")
            {
                FreeSwitch = false;
                return false;
            }
            return bool.Parse(swi);
        }

        set
        {
            PlayerPrefs.SetString(PlayerUid + "UserEvaluation", value.ToString());
        }
    }

    /// <summary>
    /// 授权
    /// </summary>
    public static bool Authorized
    {
        get
        {
            string swi = PlayerPrefs.GetString(  "Authorized");
            if (swi == "")
            {
                return false;
            }
            return bool.Parse(swi);
        }

        set
        {
            PlayerPrefs.SetString(  "Authorized", value.ToString());
        }
    }


    /// <summary>
    /// 登录次数
    /// </summary>
    public static int LoginCount
    {
        get
        {
            int swi = PlayerPrefs.GetInt(PlayerUid + "LoginCount");
            return swi;
        }

        set
        {
            PlayerPrefs.SetInt(PlayerUid + "LoginCount", value);
        }
    }

}
