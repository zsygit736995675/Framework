using Newtonsoft.Json;
using Solar.Runtime;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ConfigUitls 
{
    
    /// <summary>
    /// 加载configTextAssets文件
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="typeName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T LoadConfigTextAsset<T>(string assetName, string typeName = "JsonAsset")  
    {
        if(Application.isPlaying)
        {
            var jsonStr = (TextAsset)Root.Asset.LoadAssetSync(typeName, "Assets/Framework/Config/Json", assetName);
            var config = Deserialize<T>(jsonStr.text);
            Root.Asset.UnloadAsset(jsonStr);
            return config;
        }
        else
        {
#if UNITY_EDITOR
            var text = (TextAsset)AssetDatabase.LoadAssetAtPath($"Assets/Framework/Config/Json/{assetName}.json", typeof(TextAsset));
            var config = Deserialize<T>(text.text);
            return config;
#endif
        }

        return default(T);
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="text"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Deserialize<T>(string text)
    {
        var config = JsonConvert.DeserializeObject<T>(text);
        return config;
    }



}
