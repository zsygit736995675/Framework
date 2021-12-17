using UnityEditor;
using UnityEngine;

namespace SY_FrameWork
{
    /// <summary>
    /// 打开到系统文件夹
    public class PathOpen {
    
        [MenuItem("SY_Tools/打开到文件夹/dataPath")]
        static void OpendataPath()
        {
            System.Diagnostics.Process.Start(Application.dataPath);
        }

        [MenuItem("SY_Tools/打开到文件夹/persistentDataPath")]
        static void OpenpersistentDataPath()
        {
            System.Diagnostics.Process.Start(Application.persistentDataPath);
        }

        [MenuItem("SY_Tools/打开到文件夹/streamingAssetsPath")]
        static void OpenstreamingAssetsPath()
        {
            System.Diagnostics.Process.Start(Application.streamingAssetsPath);
        }

        [MenuItem("SY_Tools/打开到文件夹/temporaryCachePath")]
        static void OpentemporaryCachePath()
        {
            System.Diagnostics.Process.Start(Application.temporaryCachePath);
        }
    }

}
