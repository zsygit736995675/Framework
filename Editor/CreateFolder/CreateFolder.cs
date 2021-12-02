using UnityEngine;
using UnityEditor;
using System.IO;

namespace ZFrame
{
    /// <summary>
    /// 创建文件夹  
    /// </summary>
    public class CreateFile
    {
        [MenuItem("ZTools/创建默认文件夹")]//需要引入命名空间using UnityEditor
        public static void CreateFolder()
        {
            string path = Application.dataPath + "/";
            Directory.CreateDirectory(path + "Resources");
            Directory.CreateDirectory(path + "Resources/Texture");
            Directory.CreateDirectory(path + "Resources/Animation");
            Directory.CreateDirectory(path + "Resources/Effect");
            Directory.CreateDirectory(path + "Resources/Font");
            Directory.CreateDirectory(path + "Resources/Audio");
            Directory.CreateDirectory(path + "Resources/Audio/BGM");
            Directory.CreateDirectory(path + "Resources/Audio/Sound");
            Directory.CreateDirectory(path + "Resources/Prefabs");
            Directory.CreateDirectory(path + "Resources/Models");
            Directory.CreateDirectory(path + "Resources/Materials");

            Directory.CreateDirectory(path + "Res");
            Directory.CreateDirectory(path + "Res/Texture");
            Directory.CreateDirectory(path + "Res/Animation");
            Directory.CreateDirectory(path + "Res/Effect");
            Directory.CreateDirectory(path + "Res/Font");
            Directory.CreateDirectory(path + "Res/Audio");
            Directory.CreateDirectory(path + "Res/Audio/BGM");
            Directory.CreateDirectory(path + "Res/Audio/Sound");
            Directory.CreateDirectory(path + "Res/Prefabs");
            Directory.CreateDirectory(path + "Res/Models");
            Directory.CreateDirectory(path + "Res/Materials");

            Directory.CreateDirectory(path + "Plugins");
            Directory.CreateDirectory(path + "Plugins/Android");
            Directory.CreateDirectory(path + "Plugins/IOS");

            Directory.CreateDirectory(path + "StreamingAssets");
            Directory.CreateDirectory(path + "Editor");
            Directory.CreateDirectory(path + "Scenes");
            
            Directory.CreateDirectory(path + "Scripts");
            Directory.CreateDirectory(path + "Scripts/Main");
            Directory.CreateDirectory(path + "Scripts/GamePlay");
            
            Directory.CreateDirectory(path + "3rdParty");
            
            
            AssetDatabase.Refresh();
        }
    }
}


