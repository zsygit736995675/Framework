using System;
using Solar.Runtime;
using SY_FrameWork;
using UnityEngine;

/// <summary>
/// 语言
/// </summary>
public struct LanguageConst
{
    public const string zh = "zh"; //中文
    public const string en = "en"; //英文
    public const string hi = "hi"; //印地语
    public const string pt = "pt"; //葡萄牙
    public const string ar = "ar"; //阿拉伯语
    public const string fr = "fr"; //法语
    public const string id = "id"; //印尼语
    public const string tr = "tr"; //土耳其语
    public const string de = "de"; //德语
    public const string es = "es"; //西语
    public const string ja = "ja"; //日语
    public const string ko = "ko"; //韩语
    public const string tc = "tc"; //繁体中文
}

/// <summary>
/// 代理
/// </summary>
public delegate void GameVoidDelegate();

/// <summary>
/// 多语言管理
/// </summary>
public class LanguageManager : MonoBehaviour
{
    /// <summary>
    /// 多语言修改监听
    /// </summary>
    public GameVoidDelegate OnLocalize;

    /// <summary>
    /// 语言配置列表
    /// </summary>
    public CountryAndCode[] countrys;
    

    /// <summary>
    /// 本地存储的key
    /// </summary>
    private const string SaveKey = "LanguageSetting";

    /// <summary>
    /// 默认语言
    /// </summary>
    private string _defaultLanguage = LanguageConst.en;
    
    
    /// <summary>
    /// 单例
    /// </summary>
    private static LanguageManager _instance = null;
    public static LanguageManager Instance { get { return _instance; } }

    /// <summary>
    /// 当前语言
    /// </summary>
    private CountryAndCode _currentCountry;

    private CountryAndCode CurrentCountry
    {
        get
        {
            if (_currentCountry == null)
            {
                ResetCountry(_defaultLanguage);
            }

            return _currentCountry;
        }
    }
    
    /// <summary>
    /// 当前字体
    /// </summary>
    public Font CurrentFont { get { return CurrentCountry.Font; } }

    private void Awake()
    {
        if( _instance == null )
        {
            _instance = GetComponent<LanguageManager>();
        }
    }
    
    /// <summary>
    ///	初始化语言类型
    /// </summary>
    /// <param name="lang"></param>
    public void InitLanguage()
    {
        foreach (var country in countrys)
        {
            Font font;
            // 加载字体资源文件
            if(country.FontAssetName.Equals("Arial"))
            {
                font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            }
            else
            {
                font = (Font)Root.Asset.LoadAssetSync("Font", "Assets/StaticRes/Fonts", country.FontAssetName);
            }
            
            country.Font = font;
        }
        
        string currentLanguage;
        if (PlayerPrefs.HasKey(SaveKey))
        {
            //非首次登陆,获取本地存储
            currentLanguage = PlayerPrefs.GetString(SaveKey);
        }
        else
        {
            //首次登陆获取系统语言
            currentLanguage = _defaultLanguage;
#if UNITY_ANDROID && !UNITY_EDITOR
            currentLanguage = GetLocalLanguage();
#endif
        }

        ResetCountry(currentLanguage);
    }
    
    /// <summary>
    /// 获取系统语言
    /// </summary>
    /// <returns></returns>
    private string GetLocalLanguage()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        return AndroidManager.GetLocalLanguage();
#else
        return LanguageConst.en;
#endif
    }

    /// <summary>
    /// 设置语言
    /// </summary>
    public void SetLanguage(string lang)
    {
        if (lang == CurrentCountry.ShortName)
        {
            return;
        }

        ResetCountry(lang);
        OnLocalize?.Invoke();
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

        SetLanguage(cac.ShortName);
    }

    /// <summary>
    /// 返回语言
    /// </summary>
    /// <returns></returns>
    public string GetLanguage()
    {
        return CurrentCountry.ShortName;
    }

    /// <summary>
    /// 获取当前语言下标
    /// </summary>
    /// <returns></returns>
    public int GetIndex()
    {
        for (int i = 0; i < countrys.Length; i++)
        {
            if (CurrentCountry.ShortName == countrys[i].ShortName)
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
        if (param == null || param.Length <= 0)
        {
            return GetLangByKey(key, Instance.CurrentCountry);
        }
        else
        {
            return string.Format(GetLangByKey(key, Instance.CurrentCountry), param);
        }
    }
    
    /// <summary>
    /// 方便调用
    /// </summary>
    public static string GetStr(int key, CountryAndCode languag, params object[] param)
    {
        if (param == null || param.Length <= 0)
        {
            return GetLangByKey(key, languag);
        }
        else
        {
            return string.Format(GetLangByKey(key, languag), param);
        }
    }

    /// <summary>
    /// 刷新当前字体和语言
    /// </summary>
    private void ResetCountry(string currentLanguage)
    {
        //刷新当前语言
        foreach (var country in countrys)
        {
            if (currentLanguage == country.ShortName)
            {
                _currentCountry = country;
                PlayerPrefs.SetString(SaveKey, _currentCountry.ShortName);
                break;
            }
        }
        
        if (currentLanguage != _defaultLanguage && _currentCountry == null) 
        {
            ResetCountry(_defaultLanguage);
        }
    }

    /// <summary>
    /// 获取对应的语言的文字
    /// </summary>
    public static string GetLangByKey(int key, CountryAndCode curLange = null)
    {
        StrConfig config = StrConfig.Get(key);

        CountryAndCode country = curLange == null ? Instance.CurrentCountry : curLange;

        if (config == null || country == null || string.IsNullOrEmpty(country.StrKey))
        {
            return "";
        }

        var obj = config.GetType().GetProperty(country.StrKey)?.GetValue(config);
        string str = obj?.ToString();

        if (string.IsNullOrEmpty(str))
        {
            str = config.ENGLISH;
        }

        if (string.IsNullOrEmpty(str))
        {
            str = "";
        }

        return str.TrimStart();
    }
}

/// <summary>
/// 国家简称和在下拉框中的位置
/// </summary>
[Serializable]
public class CountryAndCode
{
    public string Country; //全称

    public string ShortName; //简称

    public string FontAssetName; // 字体的名称

    public int Sort; //排序

    public string Annotate; //注释

    public string StrKey; //再StrConfig中的key

    [NonSerialized] public Font Font; //对应字体
}