/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Path.cs
 * author:    taoye
 * created:   2021/6/16
 * descrip:   路径实用函数
 ***************************************************************/
using System.IO;
using UnityEngine;

namespace Solar.Runtime
{
    public static class Path
    {
        /// <summary>
        /// 平台名称
        /// </summary>
#if UNITY_IOS
        public static string PlatformName = "iOS";
#elif UNITY_WEBGL
        public static string PlatformName = "WebGL";
#else
        public static string PlatformName = "Android";
#endif

        /// <summary>
        /// AssetBundle相关路径信息
        /// </summary>
        public static class AB
        {
            /// <summary>
            /// AB文件夹前缀
            /// </summary>
            public static string DirectoryPrefix = $"AssetBundles/{PlatformName}";

            /// <summary>
            /// 获取ABConfigs的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return $"{Application.dataPath}/../Docs/Excels/ABConfigs";
            }

            /// <summary>
            /// 获取ABConfigs的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelFileFullPath()
            {
                return $"{Application.dataPath}/../Docs/Excels/ABConfigs/ABConfigs.xlsm";
            }

            /// <summary>
            /// AssetBundle可写路径下相关路径信息
            /// </summary>
            public static class Persistent
            {
                /// <summary>
                /// 获取可写路径下AB根目录的绝对路径
                /// </summary>
                /// <returns></returns>
                public static string GetRootDirectoryFullPath(string platformName = null)
                {
                    if (string.IsNullOrEmpty(platformName))
                    {
                        return System.IO.Path.Combine(Application.persistentDataPath, DirectoryPrefix).Replace('\\', '/');
                    }
                    else
                    {
                        return System.IO.Path.Combine(Application.persistentDataPath, $"AssetBundles/{platformName}").Replace('\\', '/');
                    }
                }

                /// <summary>
                /// 获取可写路径下AB文件的绝对路径
                /// </summary>
                /// <param name="formatPath">ab的格式化路径（从asset/开始的全小写路径信息）</param>
                /// <returns></returns>
                public static string GetFileFullPath(string formatPath)
                {
                    return System.IO.Path.Combine(GetRootDirectoryFullPath(), formatPath).Replace('\\', '/');
                }
            }

            /// <summary>
            /// AssetBundle只读路径下相关路径信息
            /// </summary>
            public static class Streaming
            {
                /// <summary>
                /// 获取只读路径下AB根目录的绝对路径
                /// </summary>
                /// <returns></returns>
                public static string GetRootDirectoryFullPath(string platformName = null)
                {
#if UNITY_IOS && !UNITY_EDITOR
                    if (string.IsNullOrEmpty(platformName))
                    {
                        return $"{Application.dataPath}/Raw/{DirectoryPrefix}".Replace('\\', '/');
                    }
                    else
                    {
                        return $"{Application.dataPath}/Raw/AssetBundles/{platformName}".Replace('\\', '/');
                    }
#elif UNITY_WEBGL && UNITY_EDITOR
                    if (string.IsNullOrEmpty(platformName))
                    {
                        return $"{Application.dataPath}/StreamingAssets/{DirectoryPrefix}".Replace('\\', '/');
                    }
                    else
                    {
                        return $"{Application.dataPath}/StreamingAssets/AssetBundles/{platformName}".Replace('\\', '/');
                    }
#else
                    if (string.IsNullOrEmpty(platformName))
                    {
                        return System.IO.Path.Combine(Application.streamingAssetsPath, DirectoryPrefix).Replace('\\', '/');
                    }
                    else
                    {
                        return System.IO.Path.Combine(Application.streamingAssetsPath, $"AssetBundles/{platformName}").Replace('\\', '/');
                    }
#endif
                }

                /// <summary>
                /// 获取只读路径下AB文件的绝对路径
                /// </summary>
                /// <param name="formatPath">ab的格式化路径（从asset/开始的全小写路径信息）</param>
                /// <returns></returns>
                public static string GetFileFullPath(string formatPath)
                {
                    return System.IO.Path.Combine(GetRootDirectoryFullPath(), formatPath).Replace('\\', '/');
                }

            }

        }

        /// <summary>
        /// 持久化文件片段相关路径信息
        /// </summary>
        public static class FileFragment
        {
            /// <summary>
            /// 获取持久化文件片段根目录路径（绝对路径）
            /// </summary>
            public static string GetRootDirectoryFullPath()
            {
                return System.IO.Path.Combine(Application.persistentDataPath, "PersistFileFragments").Replace('\\', '/');
            }
        }

        /// <summary>
        /// Project相关路径信息
        /// </summary>
        public static class Project
        {
            /// <summary>
            /// 获取Project根目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetRootDirectoryFullPath()
            {
                return $"{Application.dataPath}/../".Replace('\\', '/');
            }

            /// <summary>
            /// 获取VS-Projrect文件的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetVSProjectFileFullPath()
            {
                string[] fileFullPaths = Directory.GetFiles(GetRootDirectoryFullPath(), "*.sln", SearchOption.AllDirectories);
                if (fileFullPaths != null && fileFullPaths.Length > 0)
                {
                    return fileFullPaths[0];
                }
                return string.Empty;
            }

            /// <summary>
            /// 获取ProjectSettings目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetProjectSettingsDirPath()
            {
                return $"{GetRootDirectoryFullPath()}ProjectSettings".Replace('\\', '/');
            }

        }

        /// <summary>
        /// Tool相关路径信息
        /// </summary>
        public static class Tool
        {
            /// <summary>
            /// 获取Tool根目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetRootDirectoryFullPath()
            {
                return $"{Application.dataPath}/../Tools".Replace('\\', '/');
            }

        }

        /// <summary>
        /// Editor编辑器相关路径信息
        /// </summary>
        public static class Editor
        {
            /// <summary>
            /// 获取ProjectSetting目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetProjectSettingDirectoryFullPath()
            {
                return $"{Application.dataPath}/../ProjectSettings".Replace('\\', '/');
            }

        }

    }

}


