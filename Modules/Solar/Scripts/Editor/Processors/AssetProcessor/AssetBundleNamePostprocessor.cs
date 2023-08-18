/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetBundleNamePostprocessor.cs
 * author:    taoye
 * created:   2022/2/15
 * descrip:   AssetBundle名称自动刷新处理器
 ***************************************************************/
using Newtonsoft.Json.Linq;
using Solar.Runtime;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Solar.Editor
{
    public class AssetBundleNamePostprocessor : AssetPostprocessor
    {
        /// <summary>
        /// AB配置信息
        /// <Path, ABConfigInfo>
        /// </summary>
        public static Dictionary<string, ABConfigInfo> s_ABConfigs = new Dictionary<string, ABConfigInfo>();

        /// <summary>
        /// 收集ABConfig信息
        /// </summary>
        public static void RefreshABConfigs()
        {
            string filePathList = Runtime.Path.Project.GetProjectSettingsDirPath() + "/ABConfigs.json";
            string content = File.ReadAllText(filePathList);
            JObject jObject = JObject.Parse(content);

            s_ABConfigs.Clear();

            foreach (var itr in jObject)
            {
                JToken data = itr.Value;
                s_ABConfigs.Add(data["Path"].ToString(), new ABConfigInfo(int.Parse(data["ID"].ToString()), data["Path"].ToString(), int.Parse(data["PackageMeasureType"].ToString()), data["Rename"].ToString(), data["GroupName"].ToString(), bool.Parse(data["IsIncreaserGroup"].ToString())));
            }
        }

        /// <summary>
        /// 清除所有AssetBundle名称
        /// </summary>
        public static void CleanAllAsssetBundleNames()
        {
            RefreshABConfigs();
            string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            for (int j = 0; j < assetBundleNames.Length; j++)
            {
                AssetDatabase.RemoveAssetBundleName(assetBundleNames[j], true);
            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 刷新所有AssetBundle名称
        /// </summary>
        public static void RefreshAllAsssetBundleNames()
        {
            CleanAllAsssetBundleNames();

            foreach (var config in s_ABConfigs)
            {
                ABConfigInfo info = config.Value;

                // 将Path文件/目录单独作为一个AB进行打包
                if (info.PackageMeasureType == 0)
                {
                    AssetImporter importer = AssetImporter.GetAtPath(info.Path);
                    if (importer)
                    {
                        if (!string.IsNullOrEmpty(info.Rename))
                        {
                            importer.assetBundleName = info.Rename.ToLower();
                        }
                        else
                        {
                            int indexOfSuffixFlag = info.Path.LastIndexOf('.');
                            indexOfSuffixFlag = indexOfSuffixFlag < 0 ? info.Path.Length : indexOfSuffixFlag;
                            importer.assetBundleName = $"{info.Path.Substring(0, indexOfSuffixFlag).Replace('/', '@').ToLower()}.bundle";
                        }
                    }
                }
                // 将Path目录下每个文件单独作为一个AB进行打包
                else if (info.PackageMeasureType == 1)
                {
                    string[] fullPaths = Directory.GetFiles(System.IO.Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length), info.Path).Replace('\\', '/'));
                    foreach (string fullPath in fullPaths)
                    {
                        if (!fullPath.EndsWith(".meta"))
                        {
                            string formatFullPath = fullPath.Replace('\\', '/');
                            string filePath = $"Assets/{formatFullPath.Substring(Application.dataPath.Length + 1, formatFullPath.Length - (Application.dataPath.Length + 1))}";
                            AssetImporter importer = AssetImporter.GetAtPath(filePath);
                            if (importer)
                            {
                                int indexOfSuffixFlag = filePath.LastIndexOf('.');
                                indexOfSuffixFlag = indexOfSuffixFlag < 0 ? filePath.Length : indexOfSuffixFlag;
                                importer.assetBundleName = $"{filePath.Substring(0, indexOfSuffixFlag).Replace('/', '@').ToLower()}.bundle";
                            }
                        }
                    }
                }
                // 将Path目录下每个第一级文件夹单独作为一个AB进行打包
                else if (info.PackageMeasureType == 2)
                {
                    string[] fullPaths = Directory.GetDirectories(System.IO.Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length), info.Path).Replace('\\', '/'));
                    foreach (string fullPath in fullPaths)
                    {
                        if(Directory.Exists(fullPath))
                        {
                            string formatFullPath = fullPath.Replace('\\', '/');
                            string directoryPath = $"Assets/{formatFullPath.Substring(Application.dataPath.Length + 1, formatFullPath.Length - (Application.dataPath.Length + 1))}";
                            AssetImporter importer = AssetImporter.GetAtPath(directoryPath);
                            if (importer)
                            {
                                int indexOfSuffixFlag = directoryPath.LastIndexOf('.');
                                indexOfSuffixFlag = indexOfSuffixFlag < 0 ? directoryPath.Length : indexOfSuffixFlag;
                                importer.assetBundleName = $"{directoryPath.Substring(0, indexOfSuffixFlag).Replace('/', '@').ToLower()}.bundle";
                            }
                        }
                    }
                }
                // 将Path目录下每个第一级文件与文件夹单独作为一个AB进行打包
                else if (info.PackageMeasureType == 3)
                {
                    // 先处理文件
                    string[] fullPaths = Directory.GetFiles(System.IO.Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length), info.Path).Replace('\\', '/'));
                    foreach (string fullPath in fullPaths)
                    {
                        if (!fullPath.EndsWith(".meta"))
                        {
                            string formatFullPath = fullPath.Replace('\\', '/');
                            string filePath = $"Assets/{formatFullPath.Substring(Application.dataPath.Length + 1, formatFullPath.Length - (Application.dataPath.Length + 1))}";
                            AssetImporter importer = AssetImporter.GetAtPath(filePath);
                            if (importer)
                            {
                                int indexOfSuffixFlag = filePath.LastIndexOf('.');
                                indexOfSuffixFlag = indexOfSuffixFlag < 0 ? filePath.Length : indexOfSuffixFlag;
                                importer.assetBundleName = $"{filePath.Substring(0, indexOfSuffixFlag).Replace('/', '@').ToLower()}.bundle";
                            }
                        }
                    }

                    // 再处理文件夹
                    fullPaths = Directory.GetDirectories(System.IO.Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length), info.Path).Replace('\\', '/'));
                    foreach (string fullPath in fullPaths)
                    {
                        if (Directory.Exists(fullPath))
                        {
                            string formatFullPath = fullPath.Replace('\\', '/');
                            string directoryPath = $"Assets/{formatFullPath.Substring(Application.dataPath.Length + 1, formatFullPath.Length - (Application.dataPath.Length + 1))}";
                            AssetImporter importer = AssetImporter.GetAtPath(directoryPath);
                            if (importer)
                            {
                                int indexOfSuffixFlag = directoryPath.LastIndexOf('.');
                                indexOfSuffixFlag = indexOfSuffixFlag < 0 ? directoryPath.Length : indexOfSuffixFlag;
                                importer.assetBundleName = $"{directoryPath.Substring(0, indexOfSuffixFlag).Replace('/', '@').ToLower()}.bundle";
                            }
                        }
                    }
                }
            }
            AssetDatabase.Refresh();
        }

        ///// <summary>
        ///// 个别资源变动回调
        ///// </summary>
        ///// <param name="imported">导入资源路径集合</param>
        ///// <param name="deleted">删除资源路径集合</param>
        ///// <param name="moved">移动资源路径集合</param>
        ///// <param name="movedFromAssetPaths">移动资源来源路径集合</param>
        //public static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved, string[] movedFromAssetPaths)
        //{
        //    RefreshABConfigs();

        //    string abConfigsFilePathList = Runtime.Path.Json.GetRootDirectoryRelativePath() + "/ABConfigs.json";
        //    for (int index = 0; index < imported.Length; index++)
        //    {
        //        if(imported[index] == abConfigsFilePathList)
        //        {
        //            RefreshAllAsssetBundleNames();
        //            AssetDatabase.Refresh();
        //            return;
        //        }
        //    }

        //    // 清除需要被清除的assetbundleName
        //    List<string> needRemovePaths = new List<string>();
        //    for(int index = 0; index < deleted.Length; index++)
        //    {
        //        if(!needRemovePaths.Contains(deleted[index]))
        //        {
        //            needRemovePaths.Add(deleted[index]);
        //        }
        //    }
        //    for (int index = 0; index < movedFromAssetPaths.Length; index++)
        //    {
        //        if (!needRemovePaths.Contains(movedFromAssetPaths[index]))
        //        {
        //            needRemovePaths.Add(movedFromAssetPaths[index]);
        //        }
        //    }

        //    List<string> assetBundleNames = new List<string>(AssetDatabase.GetAllAssetBundleNames());
        //    needRemovePaths.ForEach((needRemovePath) =>
        //    {
        //        int indexOfSuffixFlag = needRemovePath.LastIndexOf('.');
        //        indexOfSuffixFlag = indexOfSuffixFlag < 0 ? needRemovePath.Length : indexOfSuffixFlag;
        //        string assetBundleName = needRemovePath.Substring(0, indexOfSuffixFlag).Replace('/', '@').ToLower();
        //        assetBundleNames.ForEach((checkBundleName) =>
        //        {
        //            if (checkBundleName == assetBundleName)
        //            {
        //                AssetDatabase.RemoveAssetBundleName(assetBundleName, true);
        //            }
        //        });
        //    });

        //    List<string> modifiedPaths = new List<string>();
        //    for (int index = 0; index < imported.Length; index++)
        //    {
        //        if (!modifiedPaths.Contains(imported[index]))
        //        {
        //            modifiedPaths.Add(imported[index]);
        //        }
        //    }
        //    for (int index = 0; index < moved.Length; index++)
        //    {
        //        if (!modifiedPaths.Contains(moved[index]))
        //        {
        //            modifiedPaths.Add(moved[index]);
        //        }
        //    }

        //    if (modifiedPaths.Count > 0)
        //    {
        //        RefreshAllAsssetBundleNames();
        //    }

        //    AssetDatabase.Refresh();
        //}

    }

}
