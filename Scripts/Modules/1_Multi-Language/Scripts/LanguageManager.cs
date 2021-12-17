using UnityEngine;

namespace SY_FrameWork
{
    /// <summary>
    /// 语言
    /// </summary>
    public struct LanguageConst
    {
        public const string zh = "zh";//中文
        public const string en = "en";//英文
        public const string hi = "hi";//印地语
    }

    /// <summary>
    /// 多语言管理
    /// </summary>
    public class LanguageManager 
    {
        private LanguageManager() { }
        private static LanguageManager instance;
        public static LanguageManager Instance { get { if (instance == null) instance = new LanguageManager(); return instance; } }

        /// <summary>
        /// 代理定義
        /// </summary>
        public delegate void GameVoidDelegate();
        
        /// <summary>
        /// 回调
        /// </summary>
        public static GameVoidDelegate OnLocalize;
        
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
        string defaultLanguage = LanguageConst.en;

        
        /// <summary>
        ///	初始化语言类型
        /// </summary>
        /// <param name="lang"></param>
        public void InitLanguage()
        {
            if (PlayerPrefs.HasKey(saveKey))
            {
                CurrentLanguage = PlayerPrefs.GetString(saveKey);
                return;
            }
            CurrentLanguage = defaultLanguage;
    #if  UNITY_ANDROID && !UNITY_EDITOR
           CurrentLanguage = GetLocalLanguage();
    #endif
        }

        /// <summary>
        /// 设置语言
        /// </summary>
        public void SetLanguage(string lang)
        {
            if (lang == CurrentLanguage)
                return;

            CurrentLanguage = lang;
            PlayerPrefs.SetString(saveKey, CurrentLanguage);
            OnLocalize?.Invoke();
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

            if (config == null) return "";

            switch (CurrentLanguage)
            {
                case LanguageConst.en:
                    return config._en;
                case LanguageConst.zh:
                    return config._zh;
                 case LanguageConst.hi:
                     return config._hi;
            }
            return config._zh;
        }

        // /// <summary>
        // /// 图片加载
        // /// </summary>
        // M_Atlas imgs;
        //
        // M_Atlas Imgs
        // {
        //     get
        //     {
        //         if (imgs == null)
        //         {
        //             imgs = Resources.Load<M_Atlas>("Scriptable/LanguagePic");
        //         }
        //
        //         return imgs;
        //     }
        // }

        // <summary>
        /// 获取图片
        /// </summary>
        // public Sprite GetSprite(string tag)
        // {
        //     foreach (var item in Imgs.spriteTags)
        //     {
        //         if (item.key == tag)
        //         {
        //             switch (CurrentLanguage)
        //             {
        //                 case LanguageConst.zh:
        //                     return item.Chinese;
        //                 case LanguageConst.en:
        //                     return item.English;
        //                 case LanguageConst.hi:
        //                     return item.Hindi;
        //             }
        //         }
        //     }
        //     return null;
        // }
        
        /// <summary>
        /// 获取系统语言
        /// </summary>
        /// <returns></returns>
        public string GetLocalLanguage()
        {
    #if UNITY_EDITOR
            return defaultLanguage;
    #else
            AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject _unityContext = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject locale = _unityContext.Call<AndroidJavaObject>("getResources").Call<AndroidJavaObject>("getConfiguration").Get<AndroidJavaObject>("locale");
            string systemLanguage = locale.Call<string>("getLanguage");
            return systemLanguage;
    #endif
        }
    }
}

