using System.IO;
using UnityEngine;

/// <summary>
/// 路径工具
/// </summary>
public class PathTools
{
    /// <summary>
    /// StreamPath路径
    /// </summary>
    public static string StreamPath
    {
        get
        {
            return Application.streamingAssetsPath + "/Cache";
        }
    }

    /// <summary>
    /// 缓存路径
    /// </summary>
    public static string persistentPath
    {
        get
        {
            return Application.persistentDataPath + "/WebCache";
        }
    }

    static  string[] sufTypes = { "png","jpg","apk","mp3","ogg","txt","json"}; 
    /// <summary>
    /// 获取资源的保存路径
    /// </summary>
    public static string GetSavePath(string url) 
    {
        string _suffix = GetSuffix(url);
        string _name = "{0}." + _suffix;
        string path = Path.Combine(persistentPath, _suffix);
        CreatePath(path);
        path = Path.Combine(path, string.Format(_name, GetFileName(url)/*url.GetHashCode()*/)); ;
        return path;
    }

    /// <summary>
    /// 解析后缀名
    /// </summary>
    public static string GetFileName(string url)
    {
        string[] subArr = url.Split('.');
        string totalStr = subArr[subArr.Length - 2];
        string [] subArr1 = totalStr.Split('/');
        return subArr1[subArr1.Length-1];
    }

    /// <summary>
    /// 解析后缀名
    /// </summary>
    public static string GetSuffix(string url) 
    {
        string[] subArr = url.Split('.');
        string _suffix  = subArr[subArr.Length - 1];

        for (int i = 0; i < sufTypes.Length; i++)
        {
            if (_suffix.Contains(sufTypes[i]))
            {
                _suffix = sufTypes[i];
                break;
            }
        }
        return _suffix;
    }

    /// <summary>
    /// 获取资源的加载路径
    /// </summary>
    public static string GetLoadPath(string url)
    {
        return "file:///" + GetSavePath(url);
    }

    /// <summary>
    /// 创建路径
    /// </summary>
    static void CreatePath(string path) 
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    /// <summary>
    /// 获取资源保存路径，打包时保存路径
    /// </summary>
    public static string GetAssetOutPath(int target)
    {
        string path = "";
        switch (target)
        {
            case 5://win
                path = StreamPath + "/Android/";
                break;
            case 9://ios
                path = StreamPath + "/IOS/";
                break;
            case 13://android
                path = StreamPath + "/Android/";
                break;
        }
        return path;
    }

    /// <summary>
    /// 不同平台下StreamingAssets的路径
    /// </summary>
    public static readonly string dataPath =
#if UNITY_ANDROID && !UNITY_EDITOR
		"jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IOS && !UNITY_EDITOR
		"file://" + Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN
        "file://" + Application.dataPath + "/StreamingAssets/";
#else
        "file://" + Application.dataPath + "/StreamingAssets/";
#endif


    /// <summary>
    /// 平台名称，下载时只区分安卓和ios，pc使用安卓的资源
    /// </summary>
    public static readonly string platName =
#if UNITY_IOS 
		 "IOS";
#else
       "Android";
#endif
}
