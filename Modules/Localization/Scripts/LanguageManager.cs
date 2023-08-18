using System;
using System.Collections.Generic;
using SY_FrameWork;
using UnityEngine;

/// <summary>
/// 语言
/// </summary>
public struct LanguageConst
{
    public const string fw = "fw"; //跟随系统语言
    public const string zh = "zh"; //中文
    public const string en = "en"; //英文
    public const string hi = "hi"; //印地语
    public const string po = "po"; //葡萄牙
    public const string ar = "ar"; //阿拉伯语
    public const string fr = "fr"; //法语
    public const string id = "id"; //印尼语
    public const string tr = "tr"; //土耳其语
    public const string de = "de"; //德语
    public const string es = "es"; //西语
    public const string ja = "ja"; //日语
    public const string kr = "kr"; //韩语
    public const string tc = "tc"; //繁体中文
    
}

/// <summary>
/// 多语言管理
/// </summary>
public class LanguageManager : SingletonGetMono<LanguageManager>
{
    /// <summary>
    /// 语言配置
    /// </summary>
    public CountryAndCode[] countrys;

    /// <summary>
    /// 代理
    /// </summary>
    public delegate void GameVoidDelegate();

    /// <summary>
    /// 回调
    /// </summary>
    public static GameVoidDelegate OnLocalize;

    /// <summary>
    /// 当前字体
    /// </summary>
    public Font currentFont;

    /// <summary>
    /// 当前语言
    /// </summary>
    private string CurrentLanguage { set; get; }

    /// <summary>
    /// 本地存储的key
    /// </summary>
    string saveKey = "LanguageSetting";

    /// <summary>
    /// 默认语言
    /// </summary>
    string defaultLanguage = LanguageConst.fw;  
    
    [ContextMenu("Sort")]
    void Sort()
    {
        List<CountryAndCode> list = new List<CountryAndCode>(countrys);
        list.Sort((a, b) =>
        {
            return a.sort - b.sort;
        });
        countrys = list.ToArray();
    }
    
    /// <summary>
    ///	初始化语言类型
    /// </summary>
    /// <param name="lang"></param>
    public void InitLanguage()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            CurrentLanguage = PlayerPrefs.GetString(saveKey);
            ResetFont();
            return;
        }

        CurrentLanguage = defaultLanguage;
#if UNITY_ANDROID && !UNITY_EDITOR

        CurrentLanguage = GetLocalLanguage();
#endif
        ResetFont();
    }

    /// <summary>
    /// 设置语言
    /// </summary>
    public void SetLanguage(string lang)
    {
        if (lang == CurrentLanguage)
            return;

        CurrentLanguage = lang;
        ResetFont();
        PlayerPrefs.SetString(saveKey, CurrentLanguage);
        OnLocalize?.Invoke();
    }
    
    /// <summary>
    /// 返回语言
    /// </summary>
    /// <returns></returns>
    public string GetLanguage()
    {
        return CurrentLanguage;
    }

    /// <summary>
    /// 设置语言
    /// </summary>
    public void SetLanguage(int index)
    {
        if (countrys == null)
        {
            return;
        }
        
        if (index >= countrys.Length)
        {
            return;
        }

        CountryAndCode cac = countrys[index];

        if (cac.shortName == LanguageConst.fw)
        {
            CurrentLanguage = GetLocalLanguage();
        }
        else
        {
            if (cac.shortName == CurrentLanguage)
                return;
            CurrentLanguage = cac.shortName;
        }
        currentFont = cac.font;
        PlayerPrefs.SetString(saveKey, CurrentLanguage);
        OnLocalize?.Invoke();
    }

    /// <summary>
    /// 获取当前语言下标
    /// </summary>
    /// <returns></returns>
    public int GetIndex()
    {
        for (int i = 0; i < countrys.Length; i++)
        {
            if (CurrentLanguage == countrys[i].shortName)
            {
                return i;
            }
        }

        return 0;
    }

    /// <summary>
    /// 方便调用
    /// </summary>
    public static string GetStr(int key, params object[] param)
    {
        if (param.Length <= 0)
        {
            return Instance.GetLangByKey(key);
        }
        else
        {
            return string.Format(Instance.GetLangByKey(key), param);
        }
    }

    /// <summary>
    /// 获取对应的语言的文字
    /// </summary>
    public string GetLangByKey(int key)
    {
        StrConfig config = StrConfig.Get(key);

        if (config == null)
        {
            return "";
        }

        string str = "";
        switch (CurrentLanguage)
        {
            case LanguageConst.en:
                str = config._en;
                break;
            case LanguageConst.zh:
                str = config._zh;
                break;
            case LanguageConst.hi:
                str = config._hi;
                break;
            // case LanguageConst.po:
            //     str = config.PORTUGUESE;
            //     break;
            // case LanguageConst.ar:
            //     str = config.Arabic;
            //     break;
            // case LanguageConst.fr:
            //     str = config.FRENCH;
            //     break;
            // case LanguageConst.id:
            //     str = config.INDONESIAN;
            //     break;
            // case LanguageConst.tr:
            //     str = config.TURKEY;
            //     break;
            case LanguageConst.de:
                str = config._ge;
                break;
            case LanguageConst.es:
                str = config._sp;       
                break;
            case LanguageConst.ja:
                str = config._jp;
                break;
            case LanguageConst.kr:
                str = config._kr;
                break;
            // case LanguageConst.tc:
            //     str = config.TC;
            //     break;
        }

        if (string.IsNullOrEmpty(str))
        {
            str = config._en;
        }

        if (string.IsNullOrEmpty(str))
        {
            return "";
        }

        return str.TrimStart();
    }

    /// <summary>
    /// 重置字体
    /// </summary>
    private void ResetFont()
    {
        foreach (var item in countrys)
        {
            if (CurrentLanguage == item.shortName)
            {
                currentFont = item.font;
                break;
            }
        }
    }

    public int preIndex
    {
        get
        {
            return PlayerPrefs.GetInt("PREINDEX", 0);
        }

        set
        {
            PlayerPrefs.SetInt("PREINDEX", value);
        }
    }
    
    /// <summary>
    /// 获取系统语言
    /// </summary>
    /// <returns></returns>
    public static string GetLocalLanguage()
    {
#if UNITY_EDITOR
        return LanguageConst.en;
#else
        return AndroidManager.GetLocalLanguage();
#endif
    }
    
}

/// <summary>
/// 国家简称和在下拉框中的位置
/// </summary>
[Serializable]
public class CountryAndCode
{
    public string Country; //全称

    public string shortName; //简称
    
    public Font font;//字体
    
    public int sort;//排序

    public string annotate; //注释
}