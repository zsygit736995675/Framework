using UnityEditor;
using UnityEngine;

namespace SY_FrameWork
{
    /// <summary>
    /// 打开到系统文件夹
    public class PathOpen {
    
        [MenuItem("SY_Tools/打开到文件夹/dataPath",false,4)]
        static void OpendataPath()
        {
            System.Diagnostics.Process.Start(Application.dataPath);
        }

        [MenuItem("SY_Tools/打开到文件夹/persistentDataPath",false,4)]
        static void OpenpersistentDataPath()
        {
            System.Diagnostics.Process.Start(Application.persistentDataPath);
        }

        [MenuItem("SY_Tools/打开到文件夹/streamingAssetsPath",false,4)]
        static void OpenstreamingAssetsPath()
        {
            System.Diagnostics.Process.Start(Application.streamingAssetsPath);
        }

        [MenuItem("SY_Tools/打开到文件夹/temporaryCachePath",false,4)]
        static void OpentemporaryCachePath()
        {
            System.Diagnostics.Process.Start(Application.temporaryCachePath);
        }
    }

}
