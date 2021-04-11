using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 打开到系统文件夹
/// </summary>
public class PathOpen {
    
    [MenuItem("Tools/打开到文件夹/dataPath")]
    static void OpendataPath()
    {
        System.Diagnostics.Process.Start(Application.dataPath);
    }

    [MenuItem("Tools/打开到文件夹/persistentDataPath")]
    static void OpenpersistentDataPath()
    {
        System.Diagnostics.Process.Start(Application.persistentDataPath);
    }

    [MenuItem("Tools/打开到文件夹/streamingAssetsPath")]
    static void OpenstreamingAssetsPath()
    {
        System.Diagnostics.Process.Start(Application.streamingAssetsPath);
    }

    [MenuItem("Tools/打开到文件夹/temporaryCachePath")]
    static void OpentemporaryCachePath()
    {
        System.Diagnostics.Process.Start(Application.temporaryCachePath);
    }
}
