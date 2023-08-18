/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LineCommandBuild.cs
 * author:    taoye
 * created:   2021/12/23
 * descrip:   命令行构建接口
 ***************************************************************/
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using Solar.Runtime;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Solar.Editor
{
    public static class LineCommandBuild
    {
        /// <summary>
        /// AssetBundleName与分组名称之间的映射关系
        /// </summary>
        public static Dictionary<string, string> s_GroupMapInfos = new Dictionary<string, string>();

        /// <summary>
        /// AssetBundleName与是否为增量分组之间的映射关系
        /// </summary>
        public static Dictionary<string, bool> s_IncreaserGroupMapInfos = new Dictionary<string, bool>();

        /// <summary>
        /// 自动化打包
        /// </summary>
        public static void Build()
        {
            ParseLineArgs(out string buildTargetName, out bool buildAAB, out string packagePath, out int resMinor, out bool resIncreaseCut);

            BuildTarget buildTarget = (BuildTarget)Enum.Parse(typeof(BuildTarget), buildTargetName);

            if (buildTarget != BuildTarget.Android && buildTarget != BuildTarget.iOS)
            {
                Debug.LogError("构建平台不为Android or iOS。");
                return;
            }

            // 强制切换到对应的BuildTarget
            EditorUserBuildSettings.SwitchActiveBuildTarget((BuildTargetGroup)Enum.Parse(typeof(BuildTargetGroup), buildTargetName), buildTarget);

            Debug.Log("buildAAB = " + buildAAB);
            Debug.Log("packagePath = " + packagePath);
            Debug.Log("resMinor = " + resMinor);
            Debug.Log("resIncreaseCut = " + resIncreaseCut);

            EditorUserBuildSettings.buildAppBundle = buildTarget == BuildTarget.Android ? buildAAB : false;
            string suffixName = buildTarget == BuildTarget.Android ? (EditorUserBuildSettings.buildAppBundle ? "aab" : "apk") : "ipa";
            string packageName = buildTarget == BuildTarget.Android ? (string.Format("{0}_V{1}_DateTime{2}.{3}", Application.productName, Application.version, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), suffixName)):"IOS";

            // 构建AssetBundle资源
            BuildAssetBundles(buildTarget, resMinor, resIncreaseCut);

            // 强制刷新文件
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            // 开始编译打包
            BuildPipeline.BuildPlayer(new string[] { "Assets/Launching.unity" }, string.Format("{0}/{1}", packagePath, packageName), buildTarget, BuildOptions.CleanBuildCache);

        }

        /// <summary>
        /// 构建AssetBundle资源
        /// </summary>
        /// <param name="buildTarget">目标平台类型</param>
        /// <param name="resMinor">资源小版本号</param>
        /// <param name="resIncreaseCut">增量资源剔除</param>
        public static void BuildAssetBundles(BuildTarget buildTarget, int resMinor, bool resIncreaseCut)
        {
            // 收集ABConfig信息集合
            RefreshABConfigs();

            // 全局刷新所有AssetBundle名称
            AssetBundleNamePostprocessor.RefreshAllAsssetBundleNames();

            string tmpOutputPath = $"AssetBundles/{buildTarget.ToString()}/{buildTarget.ToString()}";
            string outputPathWithVersion = $"AssetBundles/{buildTarget.ToString()}/{Application.version}.{resMinor}";
            string streamingOutputPath = System.IO.Path.Combine(Application.streamingAssetsPath, $"AssetBundles/{buildTarget.ToString()}").Replace('\\', '/');
            BuildAssetBundleOptions opt = BuildAssetBundleOptions.ChunkBasedCompression;

            // 清理历史目录（AssetBundle+Streaming）
            if (Directory.Exists(outputPathWithVersion))
            {
                Directory.Delete(outputPathWithVersion, true);
            }
            string streamingAssetBundlePath = System.IO.Path.Combine(Application.streamingAssetsPath, $"AssetBundles/{buildTarget.ToString()}").Replace('\\', '/');
            if (Directory.Exists(streamingAssetBundlePath))
            {
                Directory.Delete(streamingAssetBundlePath, true);
            }

            // 创建临时输出文件夹
            if (!Directory.Exists(tmpOutputPath))
            {
                Directory.CreateDirectory(tmpOutputPath);
            }

            // 构建AssetBundle
            var buildManifest = BuildPipeline.BuildAssetBundles(tmpOutputPath, opt, buildTarget);
            if (buildManifest == null)
            {
                Debug.LogError("构建AssetBundle时出错。");
                return;
            }

            // 更改平台bundle文件的后缀名
            string targetAssetPath = $"{tmpOutputPath}/{buildTarget.ToString()}";
            File.Move(targetAssetPath, targetAssetPath + ".bundle");
            File.Move($"{tmpOutputPath}/{buildTarget.ToString()}.manifest", $"{tmpOutputPath}/{buildTarget.ToString()}.bundle.manifest");
            AssetDatabase.Refresh();

            // 拷贝AssetBundle目录到Streaming目录
            DirectoryCopy(tmpOutputPath, streamingOutputPath);

            // 修改outputPath目录的名称为资源版本号
            if (Directory.Exists(outputPathWithVersion))
            {
                Directory.Delete(outputPathWithVersion, true);
            }
            Directory.Move(tmpOutputPath, outputPathWithVersion);

            // 强制刷新
            AssetDatabase.Refresh();

        }

        /// <summary>
        /// 解析命令行
        /// </summary>
        /// <param name="buildTargetName">构建目标平台名称</param>
        /// <param name="buildAAB">是否构建AAB</param>
        /// <param name="packagePath">生成的包体路径</param>
        /// <param name="resMinor">资源小版本号</param>
        /// <param name="resIncreaseCut">增量资源剔除</param>
        private static void ParseLineArgs(out string buildTargetName, out bool buildAAB, out string packagePath, out int resMinor, out bool resIncreaseCut)
        {
            buildTargetName = string.Empty;
            buildAAB = false;
            packagePath = string.Empty;
            resMinor = 0;
            resIncreaseCut = true;

            BuildTarget buildTarget = BuildTarget.NoTarget;

            string[] strs = Environment.GetCommandLineArgs();
            foreach (var s in strs)
            {
                if (s.Contains("-arg:"))
                {
                    string[] args = s.Split(':')[1].Split('_');

                    buildTargetName = args[0].ToString();
                    buildTarget = (BuildTarget)Enum.Parse(typeof(BuildTarget), buildTargetName);
                    if (buildTarget == BuildTarget.Android)
                    {
                        if (args.Length == 5)
                        {
                            buildAAB = bool.Parse(args[1]);
                            packagePath = args[2].ToString();
                            resMinor = int.Parse(args[3]);
                            resIncreaseCut = bool.Parse(args[4]);
                        }
                        else
                        {
                            Debug.LogError("传入参数数量异常。");
                            return;
                        }
                    }
                    else if(buildTarget == BuildTarget.iOS)
                    {
                        if (args.Length == 4)
                        {
                            packagePath = args[1].ToString();
                            resMinor = int.Parse(args[2]);
                            resIncreaseCut = bool.Parse(args[3]);
                        }
                        else
                        {
                            Debug.LogError("传入参数数量异常。");
                            return;
                        }
                    }
                    break;
                }
            }
            if(buildAAB == false && resMinor == 0)
            {
                Debug.LogError("未找到匹配参数。");
                return;
            }
        }

        /// <summary>
        /// 收集ABConfig信息
        /// </summary>
        private static void RefreshABConfigs()
        {
            string filePathList = Runtime.Path.Project.GetProjectSettingsDirPath() + "/ABConfigs.json";
            string content = File.ReadAllText(filePathList);
            JObject jObject = JObject.Parse(content);
            // 收集AB的AssetBundleName与分组名称之间的映射关系
            s_GroupMapInfos.Clear();
            s_IncreaserGroupMapInfos.Clear();
            foreach (var itr in jObject)
            {
                JToken data = itr.Value;
                ABConfigInfo info = new ABConfigInfo(int.Parse(data["ID"].ToString()), data["Path"].ToString(), int.Parse(data["PackageMeasureType"].ToString()), data["Rename"].ToString(), data["GroupName"].ToString(), bool.Parse(data["IsIncreaserGroup"].ToString()));
                
                // 将Path文件/目录单独作为一个AB进行打包
                if (info.PackageMeasureType == 0)
                {
                    if (!string.IsNullOrEmpty(info.Rename))
                    {
                        string name = info.Rename.ToLower();
                        if (!s_GroupMapInfos.ContainsKey(name))
                        {
                            s_GroupMapInfos.Add(name, info.GroupName);
                        }
                        if (!s_IncreaserGroupMapInfos.ContainsKey(name))
                        {
                            s_IncreaserGroupMapInfos.Add(name, info.IsIncreaserGroup);
                        }
                    }
                    else
                    {
                        int indexOfSuffixFlag = info.Path.LastIndexOf('.');
                        indexOfSuffixFlag = indexOfSuffixFlag < 0 ? info.Path.Length : indexOfSuffixFlag;
                        string name = info.Path.Substring(0, indexOfSuffixFlag).Replace('/', '@');
                        if(!name.Equals("Android") && !name.Equals("iOS") && !name.Equals("WebGL"))
                        {
                            name = $"{name.ToLower()}.bundle";
                        }
                        if (!s_GroupMapInfos.ContainsKey(name))
                        {
                            s_GroupMapInfos.Add(name, info.GroupName);
                        }
                        if (!s_IncreaserGroupMapInfos.ContainsKey(name))
                        {
                            s_IncreaserGroupMapInfos.Add(name, info.IsIncreaserGroup);
                        }
                    }
                }
                // 将Path目录下每个文件单独作为一个AB进行打包
                else if (info.PackageMeasureType == 1)
                {
                    string[] fileFullPaths = Directory.GetFiles(System.IO.Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length), info.Path).Replace('\\', '/'));
                    foreach (string fileFullPath in fileFullPaths)
                    {
                        if (!fileFullPath.EndsWith(".meta"))
                        {
                            string fullPath = fileFullPath.Replace('\\', '/');
                            string filePath = $"Assets/{fullPath.Substring(Application.dataPath.Length + 1, fullPath.Length - (Application.dataPath.Length + 1))}";
                            int indexOfSuffixFlag = filePath.LastIndexOf('.');
                            indexOfSuffixFlag = indexOfSuffixFlag < 0 ? filePath.Length : indexOfSuffixFlag;
                            string name = filePath.Substring(0, indexOfSuffixFlag).Replace('/', '@');
                            if (!name.Equals("Android") && !name.Equals("iOS") && !name.Equals("WebGL"))
                            {
                                name = $"{name.ToLower()}.bundle";
                            }
                            if (!s_GroupMapInfos.ContainsKey(name))
                            {
                                s_GroupMapInfos.Add(name, info.GroupName);
                            }
                            if (!s_IncreaserGroupMapInfos.ContainsKey(name))
                            {
                                s_IncreaserGroupMapInfos.Add(name, info.IsIncreaserGroup);
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
                        if (Directory.Exists(fullPath))
                        {
                            string fullPath1 = fullPath.Replace('\\', '/');
                            string dirPath = $"Assets/{fullPath1.Substring(Application.dataPath.Length + 1, fullPath1.Length - (Application.dataPath.Length + 1))}";
                            int indexOfSuffixFlag = dirPath.LastIndexOf('.');
                            indexOfSuffixFlag = indexOfSuffixFlag < 0 ? dirPath.Length : indexOfSuffixFlag;
                            string name = dirPath.Substring(0, indexOfSuffixFlag).Replace('/', '@');
                            if (!name.Equals("Android") && !name.Equals("iOS") && !name.Equals("WebGL"))
                            {
                                name = $"{name.ToLower()}.bundle";
                            }
                            if (!s_GroupMapInfos.ContainsKey(name))
                            {
                                s_GroupMapInfos.Add(name, info.GroupName);
                            }
                            if (!s_IncreaserGroupMapInfos.ContainsKey(name))
                            {
                                s_IncreaserGroupMapInfos.Add(name, info.IsIncreaserGroup);
                            }
                        }
                    }

                }
                // 将Path目录下每个第一级文件与文件夹单独作为一个AB进行打包
                else if (info.PackageMeasureType == 3)
                {
                    // 先处理文件
                    string[] fileFullPaths = Directory.GetFiles(System.IO.Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length), info.Path).Replace('\\', '/'));
                    foreach (string fileFullPath in fileFullPaths)
                    {
                        if (!fileFullPath.EndsWith(".meta"))
                        {
                            string fullPath = fileFullPath.Replace('\\', '/');
                            string filePath = $"Assets/{fullPath.Substring(Application.dataPath.Length + 1, fullPath.Length - (Application.dataPath.Length + 1))}";
                            int indexOfSuffixFlag = filePath.LastIndexOf('.');
                            indexOfSuffixFlag = indexOfSuffixFlag < 0 ? filePath.Length : indexOfSuffixFlag;
                            string name = filePath.Substring(0, indexOfSuffixFlag).Replace('/', '@');
                            if (!name.Equals("Android") && !name.Equals("iOS") && !name.Equals("WebGL"))
                            {
                                name = $"{name.ToLower()}.bundle";
                            }
                            if (!s_GroupMapInfos.ContainsKey(name))
                            {
                                s_GroupMapInfos.Add(name, info.GroupName);
                            }
                            if (!s_IncreaserGroupMapInfos.ContainsKey(name))
                            {
                                s_IncreaserGroupMapInfos.Add(name, info.IsIncreaserGroup);
                            }
                        }
                    }

                    // 再处理文件夹
                    string[] fullPaths = Directory.GetDirectories(System.IO.Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length), info.Path).Replace('\\', '/'));
                    foreach (string fullPath in fullPaths)
                    {
                        if (Directory.Exists(fullPath))
                        {
                            string fullPath1 = fullPath.Replace('\\', '/');
                            string dirPath = $"Assets/{fullPath1.Substring(Application.dataPath.Length + 1, fullPath1.Length - (Application.dataPath.Length + 1))}";
                            int indexOfSuffixFlag = dirPath.LastIndexOf('.');
                            indexOfSuffixFlag = indexOfSuffixFlag < 0 ? dirPath.Length : indexOfSuffixFlag;
                            string name = dirPath.Substring(0, indexOfSuffixFlag).Replace('/', '@');
                            if (!name.Equals("Android") && !name.Equals("iOS") && !name.Equals("WebGL"))
                            {
                                name = $"{name.ToLower()}.bundle";
                            }
                            if (!s_GroupMapInfos.ContainsKey(name))
                            {
                                s_GroupMapInfos.Add(name, info.GroupName);
                            }
                            if (!s_IncreaserGroupMapInfos.ContainsKey(name))
                            {
                                s_IncreaserGroupMapInfos.Add(name, info.IsIncreaserGroup);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 目录拷贝
        /// </summary>
        /// <param name="sourceDirName">源目录</param>
        /// <param name="destDirName">目标目录</param>
        private static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            foreach (string folderPath in Directory.GetDirectories(sourceDirName, "*", SearchOption.AllDirectories))
            {
                if (!Directory.Exists(folderPath.Replace(sourceDirName, destDirName)))
                    Directory.CreateDirectory(folderPath.Replace(sourceDirName, destDirName));
            }

            foreach (string filePath in Directory.GetFiles(sourceDirName, "*.*", SearchOption.AllDirectories))
            {
                var fileDirName = System.IO.Path.GetDirectoryName(filePath).Replace("\\", "/");
                var fileName = System.IO.Path.GetFileName(filePath);
                string newFilePath = System.IO.Path.Combine(fileDirName.Replace(sourceDirName, destDirName), fileName).Replace("\\", "/");
                File.Copy(filePath, newFilePath, true);
            }
        }

        /// <summary>
        /// 直接删除指定目录下的所有文件及文件夹(保留目录)
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        private static void DeleteAllContentsInDirectory(string directoryPath)
        {
            try
            {
                // 判断文件夹是否还存在
                if (Directory.Exists(directoryPath))
                {
                    // 去除文件夹的只读属性
                    DirectoryInfo fileInfo = new DirectoryInfo(directoryPath);
                    fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

                    // 去除文件的只读属性
                    File.SetAttributes(directoryPath, FileAttributes.Normal);

                    foreach (string f in Directory.GetFileSystemEntries(directoryPath))
                    {
                        if (File.Exists(f))
                        {
                            // 如果有子文件删除文件
                            File.Delete(f);
                            Console.WriteLine(f);
                        }
                        else
                        {
                            // 循环递归删除子文件夹
                            DeleteAllContentsInDirectory(f);
                        }
                    }

                    // 删除空文件夹
                    Directory.Delete(directoryPath);
                    Console.WriteLine(directoryPath);
                }
            }
            catch (Exception ex) // 异常处理
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

    }
}
