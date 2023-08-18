/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ABLoadManager.cs
 * author:    taoye
 * created:   2020/12/18
 * descrip:   AB加载管理器-对外接口
 *            用于管理Asset与AB加载管理器
 ***************************************************************/
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Solar.Runtime
{
    public sealed partial class ABLoadManager
    {
        /// <summary>
        /// AB包加载管理器构造
        /// </summary>
        public ABLoadManager(AssetComponent assetComponent)
        {
            m_AssetComponent = assetComponent;
            m_TempLoadeds = new List<ABObject>();
            m_DependsDataList = new Dictionary<string, string[]>();
            m_ReadyABList = new Dictionary<string, ABObject>();
            m_LoadingABList = new Dictionary<string, ABObject>();
            m_LoadedABList = new Dictionary<string, ABObject>();
            m_UnloadABList = new Dictionary<string, ABObject>();
        }

        /// <summary>
        /// 管理器心跳
        /// </summary>
        public void Update()
        {
            UpdateLoadingList();
            UpdateReadyList();
            UpdateUnLoadList();
        }

        /// <summary>
        /// 加载Manifest信息
        /// </summary>
        public void LoadManifest()
        {
            string abFormatPath = $"{Path.PlatformName}.bundle";
            GetABLoadPathOnDisk(abFormatPath, out string path, out OriginType origin);

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            m_DependsDataList.Clear();

            AssetBundle ab = null;
#if UNITY_WEBGL
            ab = LoadAssetBundleFromWebGL(abFormatPath);
#else
            ab = AssetBundle.LoadFromFile(path);
#endif
            if (ab == null)
            {
                Debug.LogError($"加载AB包{Path.PlatformName}错误！");
                return;
            }

            AssetBundleManifest mainfest = ab.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            if (mainfest == null)
            {
                Debug.LogError($"加载{Path.PlatformName}.Manifest信息错误！");
                return;
            }

            string[] assetBundleNames = mainfest.GetAllAssetBundles();
            foreach (string assetBundleName in assetBundleNames)
            {
                string[] dpsNames = mainfest.GetAllDependencies(assetBundleName);
                m_DependsDataList.Add(assetBundleName, dpsNames);
            }

            ab.Unload(true);
            ab = null;

            Debug.Log($"AB加载管理器中全局依赖资源数量：{m_DependsDataList.Count}");
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <returns></returns>
        public AssetBundle LoadSync(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            var abObj = Internal_LoadAssetBundleSync(formatPath);
            return abObj.AB;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <param name="abLoadOverCallback">加载完成回调函数</param>
        public void LoadAsync(string abPath, AssetBundleLoadOverCallBack abLoadOverCallback)
        {
            string formatPath = GetABFormatPath(abPath);
            Internal_LoadAssetBundleAsync(formatPath, abLoadOverCallback);
        }

        /// <summary>
        /// 卸载（异步）
        /// </summary>
        /// <param name="abPath">ab路径</param>
        public void Unload(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            Internal_UnloadAssetBundleAsync(formatPath);
        }

        /// <summary>
        /// AB资源文件是否存在
        /// AssetBundleManifest中是否记录
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <returns></returns>
        public bool IsABExist(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            return m_DependsDataList.ContainsKey(formatPath);
        }

        /// <summary>
        /// AB资源文件在Persistent读写目录中是否存在
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <returns></returns>
        public bool IsABExistInPersistentDataPath(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            string filePath = Path.AB.Persistent.GetFileFullPath(formatPath);
            return File.Exists(filePath);
        }

        /// <summary>
        /// 获取AB包的格式化路径
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <returns></returns>
        public string GetABFormatPath(string abPath)
        {
            return $"{abPath.Replace('/', '@').ToLower()}.bundle";
        }

        /// <summary>
        /// 获取AB包还原后的路径
        /// 注意：还原后的路径大小写不敏感
        /// </summary>
        /// <param name="abFormatPath">ab格式化路径</param>
        /// <returns></returns>
        public string GetABRestoredPath(string abFormatPath)
        {
            string restoredPath = string.Empty;
            string[] picesNames = abFormatPath.Replace(".bundle", string.Empty).Split('@');
            foreach(var piceName in picesNames)
            {
                string name = $"{piceName.Substring(0, 1).ToUpper()}{piceName.Substring(1)}";
                restoredPath = string.IsNullOrEmpty(restoredPath) ? name : $"{restoredPath}/{name}";
            }
            return restoredPath;
        }

    }
}
