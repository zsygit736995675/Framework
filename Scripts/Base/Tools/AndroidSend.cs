using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;


/// <summary>
/// 与Android通讯脚本 
/// </summary>
public class AndroidSend :SingletonObject<AndroidSend>
{
    /// <summary>
    /// android原生代码对象
    /// </summary>
    static AndroidJavaObject _ajc;
    public  AndroidJavaObject _Ajc { get { if (_ajc == null) _ajc = new AndroidJavaObject("com.zsy.callandroid.Unity2Android"); return _ajc; } }


    protected override void Spawn()
    {
        base.Spawn();
        this.name = "AndroidSend";
    }

    /// <summary>
    /// 显示Toast
    /// </summary>
    public  void ShowToast(string str)
    {
        try
        {
            _Ajc.Call<bool>("showToast", str);
        }
        catch (System.Exception e)
        {
            Debug.LogError("ShowToast error:" + e);
        }
    }

    /// <summary>
    /// 将文本复制到剪切板
    /// </summary>
    public  void Copy(string str)
    {
        bool su = false;
        try
        {
            su = _Ajc.Call<bool>("CopyTextToClipboard", str);
            if (su)
                ShowToast("已复制到粘贴板！");
            else
                ShowToast("复制失败！");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Copy error:" + e);
        }
    }

    /// <summary>
    /// 获取设备码
    /// </summary>
    public  string GetDevice()
    {
        string str = "";
        try
        {
            str = _Ajc.Call<string>("getUniversalID");
        }
        catch (System.Exception e)
        {
            Debug.LogError("GetDevice error:" + e);
        }
        return str;
    }

    /// <summary>
    /// 打开商店
    /// </summary>
    public  void OpenShop()
    {
        bool su = false;
        try
        {
           su = _Ajc.Call<bool>("toMarket", "com.xxxxxx.XXX", "com.taptap");
        }
        catch (System.Exception e)
        {
            ShowToast("商店打开失败！");
            Debug.LogError("OpenShop error:" + e);
        }
    }

    /// <summary>
    /// 获取手机设置的当前国家码
    /// </summary>
    public  string GetCountryCode()
    {
        string su = "";
        try
        {
           su = _Ajc.Call<string>("getUniversalID");
        }
        catch (System.Exception e)
        {
            Debug.LogError("GetCountryCode error:" + e);
        }
        return su;
    }

    /// <summary>
    /// 通过包名判断应用是否已经安装
    /// </summary>
    /// <returns></returns>
    public  bool IsApplicationAvilible(string appPackageName) 
    {
        bool bo = false;
        try
        {
           bo = _Ajc.Call<bool>("isApplicationAvilible", appPackageName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("IsApplicationAvilible error:"+e);
        }
        return bo;
    }

    /// <summary>
    /// 安装APK
    /// </summary>
    public  void InstallAPK(string apkPath)
    {
        try
        {
            AndroidJavaClass javaClass = new AndroidJavaClass("example.administrator.myapplication.MainActivity");
            javaClass.CallStatic<bool>("installAPK", apkPath);
            //_Ajc.Call<bool>("installAPK", apkPath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("InstallAPK error:" + e);
        }
    }

    /// <summary>
    /// 来自android的通知
    /// </summary>
    public void AndroidCall(string code) 
    {
        Debug.Log(code) ;
    }

    /// <summary>
    /// 发往Android
    /// </summary>
    public void Send(string key,params object []  par) 
    {
        string parStr = "";
        foreach (var item in par)
        {
            parStr += item.ToString() + ",";
        }

        try
        {
            _Ajc.Call<bool>("UnitySendReceive", key, parStr);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Send error:" + e);
        }
    }

}
