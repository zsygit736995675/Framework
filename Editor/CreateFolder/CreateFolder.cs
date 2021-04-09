using UnityEngine;
using UnityEditor;
using System.IO;


/// <summary>
/// 创建文件夹  
/// </summary>
public class CreateFile
{
    [MenuItem("Tools/创建默认文件夹")]//需要引入命名空间using UnityEditor
    public static void CreateFolder()
    {
        string path = Application.dataPath + "/";
        Directory.CreateDirectory(path + "Resources");//需要引入命名空间 using System.Io
        Directory.CreateDirectory(path + "Resources/Texture");

        Directory.CreateDirectory(path + "Res");
        Directory.CreateDirectory(path + "Res/Texture");

        Directory.CreateDirectory(path + "Plugins");
        Directory.CreateDirectory(path + "Plugins/Android");
        Directory.CreateDirectory(path + "Plugins/IOS");

        Directory.CreateDirectory(path + "StreamingAssets");
        Directory.CreateDirectory(path + "Editor");
        Directory.CreateDirectory(path + "Scenes");
        Directory.CreateDirectory(path + "Scripts");
    }
}
