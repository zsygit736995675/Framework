/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetLoadManager.cs
 * author:    taoye
 * created:   2020/12/18
 * descrip:   Asset加载管理器-对外接口
 *            用于管理Asset与AB加载管理器
 ***************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Solar.Runtime
{
    public sealed partial class AssetLoadManager
    {
        /// <summary>
        /// Asset加载管理器构造方法
        /// </summary>
        public AssetLoadManager(AssetComponent assetComponent)
        {
            m_AssetComponent = assetComponent;
            m_LoadingList = new Dictionary<string, AssetObject>();
            m_LoadedList = new Dictionary<string, AssetObject>();
            m_UnloadList = new Dictionary<string, AssetObject>();
            m_LoadedAsyncTmpAgentList = new List<AssetObject>();
            m_PreloadedAsyncList = new Queue<PreloadAssetObject>();
            m_AssetInstanceIDList = new Dictionary<int, AssetObject>();
            m_Scenes = new List<AssetObject>();
            m_ABLoadManager = new ABLoadManager(assetComponent);
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public UnityEngine.Object LoadSync(string typeName, string abPath, string assetName)
        {
            string assetPath = GetAssetPath(typeName, abPath, assetName);
            if (!IsFileExist(typeName, abPath, assetName))
            {
                if (m_AssetComponent.StrictCheck)
                {
                    Debug.LogError($"AssetLoadManager 资源文件不存在 : {assetPath}");
                }
                return null;
            }

            AssetObject assetObj = null;
            if (m_LoadedList.ContainsKey(assetPath))
            {
                assetObj = m_LoadedList[assetPath];
                assetObj.RefCount++;
                return assetObj.Asset;
            }
            else if (m_LoadingList.ContainsKey(assetPath))
            {
                assetObj = m_LoadingList[assetPath];

                // 异步加载未完成
                if (assetObj.Request != null)
                {
                    // 异步转同步（直接取asset）
                    if (assetObj.Request is AssetBundleRequest)
                    {
                        assetObj.Asset = (assetObj.Request as AssetBundleRequest).asset;
                    }
                    assetObj.Request = null;
                }
                else  // 异步加载已经完成
                {
                    if (m_AssetComponent.EditorResourceMode)
                    {
#if UNITY_EDITOR
                        if (assetObj.IsScene)
                        {
                            // 开始加载场景
                            UnityEngine.SceneManagement.SceneManager.LoadScene(assetName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                        }
                        else
                        {
                            // 编辑器资源模式下的异步加载是不存在的，这里直接在当前帧中直接进行同步加载即可。
                            //if (UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abPath.ToLower(), assetName).Length > 0)
                            {
                                string assetRelativeFullPath = GetAssetRelativeFullPath(typeName, abPath, assetName);
                                Type assetType = Assembly.GetType($"UnityEngine.{typeName}");
                                if (assetType != null)
                                {
                                    assetObj.Asset = UnityEditor.AssetDatabase.LoadAssetAtPath(assetRelativeFullPath, assetType);
                                }
                                else
                                {
                                    assetObj.Asset = UnityEditor.AssetDatabase.LoadAssetAtPath(assetRelativeFullPath, typeof(UnityEngine.Object));
                                }
                            }
                        }
#endif
                    }
                    else
                    {
                        // 异步转同步，按照同步的方式重新加载
                        AssetBundle ab = m_ABLoadManager.LoadSync(abPath);

                        if (assetObj.IsScene)
                        {
                            // 开始加载场景
                            UnityEngine.SceneManagement.SceneManager.LoadScene(assetName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                        }
                        else
                        {
                            Type assetType = Assembly.GetType($"UnityEngine.{typeName}");
                            if (assetType != null)
                            {
                                assetObj.Asset = ab.LoadAsset(assetName, assetType);
                            }
                            else
                            {
                                assetObj.Asset = ab.LoadAsset(assetName);
                            }
                        }

                        // 由于AB刚刚进行了异步转同步，即多进行了一次引用计数的增加，所以为了确保引用计数的正确性，需要卸载AB异步时的引用计数
                        m_ABLoadManager.Unload(abPath);
                    }
                }

                if (assetObj.IsScene)
                {
                    m_Scenes.Add(assetObj);
                }
                else
                {
                    if (assetObj.Asset == null)
                    {
                        // 提取的资源失败，从加载列表删除
                        m_LoadingList.Remove(assetObj.AssetPath);
                        if (m_AssetComponent.StrictCheck)
                        {
                            Debug.LogError($"AssetLoadManager.LoadSync assetObj.Asset '{assetObj.AssetPath}' 为空。");
                        }
                        return null;
                    }
                    assetObj.InstanceID = assetObj.Asset.GetInstanceID();
                    m_AssetInstanceIDList.Add(assetObj.InstanceID, assetObj);
                }

                m_LoadingList.Remove(assetObj.AssetPath);
                m_LoadedList.Add(assetObj.AssetPath, assetObj);
                // 原先是异步加载的Asset封装对象，加入异步列表
                m_LoadedAsyncTmpAgentList.Add(assetObj);

                assetObj.RefCount++;

                return assetObj.Asset;
            }

            assetObj = new AssetObject();
            assetObj.TypeName = typeName;
            assetObj.ABPath = abPath;
            assetObj.AssetName = assetName;
            assetObj.AssetPath = assetPath;
            assetObj.IsScene = typeName.Equals("Scene");

            if (m_AssetComponent.EditorResourceMode)
            {
#if UNITY_EDITOR
                if (assetObj.IsScene)
                {
                    // 以追加的方式开始加载场景
                    UnityEngine.SceneManagement.SceneManager.LoadScene(assetName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                }
                else
                {
                    //if (UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abPath.ToLower(), assetName).Length > 0)
                    {
                        string assetRelativeFullPath = GetAssetRelativeFullPath(typeName, abPath, assetName);
                        Type assetType = Assembly.GetType($"UnityEngine.{typeName}");
                        if (assetType != null)
                        {
                            assetObj.Asset = UnityEditor.AssetDatabase.LoadAssetAtPath(assetRelativeFullPath, assetType);
                        }
                        else
                        {
                            assetObj.Asset = UnityEditor.AssetDatabase.LoadAssetAtPath(assetRelativeFullPath, typeof(UnityEngine.Object));
                        }
                    }
                }
#endif
                assetObj.Origin = OriginType.Editor;
            }
            else
            {
                if (m_ABLoadManager.IsABExist(abPath))
                {
                    AssetBundle ab = m_ABLoadManager.LoadSync(abPath);
                    if (assetObj.IsScene)
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene(assetName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                    }
                    else
                    {
                        Type assetType = Assembly.GetType($"UnityEngine.{typeName}");
                        if (assetType != null)
                        {
                            assetObj.Asset = ab.LoadAsset(assetName, assetType);
                        }
                        else
                        {
                            assetObj.Asset = ab.LoadAsset(assetName);
                        }
                    }
                    assetObj.Origin = m_ABLoadManager.LoadedABList[m_ABLoadManager.GetABFormatPath(assetObj.ABPath)].Origin;
                }
            }

            if (assetObj.IsScene)
            {
                m_Scenes.Add(assetObj);
            }
            else
            {
                if (assetObj.Asset == null)
                {
                    // 提取的资源失败，从加载列表删除
                    if (m_AssetComponent.StrictCheck)
                    {
                        Debug.LogError($"AssetLoadManager.LoadSync assetObj.Asset '{assetObj.AssetPath}' 为空。");
                    }
                    return null;
                }

                assetObj.InstanceID = assetObj.Asset.GetInstanceID();
                m_AssetInstanceIDList.Add(assetObj.InstanceID, assetObj);
            }

            m_LoadedList.Add(assetPath, assetObj);

            assetObj.RefCount = 1;

            return assetObj.Asset;
        }

        /// <summary>
        /// 异步加载
        /// 即使资源已经加载完成也会异步回调
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="overCallback">asset资源加载完成的异步回调</param>
        public void LoadAsync(string typeName, string abPath, string assetName, AssetLoadOverCallback overCallback)
        {
            string assetPath = GetAssetPath(typeName, abPath, assetName);
            if (!IsFileExist(typeName, abPath, assetName))
            {
                if (m_AssetComponent.StrictCheck)
                {
                    Debug.LogError($"AssetLoadManager Asset 文件不存在 : '{assetPath}'。");
                }
                return;
            }

            AssetObject assetObj = null;
            if (m_LoadedList.ContainsKey(assetPath))
            {
                assetObj = m_LoadedList[assetPath];
                assetObj.AssetLoadOverCallbackList.Add(overCallback);
                m_LoadedAsyncTmpAgentList.Add(assetObj);
                return;
            }
            else if (m_LoadingList.ContainsKey(assetPath))
            {
                assetObj = m_LoadingList[assetPath];
                assetObj.AssetLoadOverCallbackList.Add(overCallback);
                return;
            }

            assetObj = new AssetObject();
            assetObj.TypeName = typeName;
            assetObj.ABPath = abPath;
            assetObj.AssetName = assetName;
            assetObj.AssetPath = assetPath;
            assetObj.IsScene = typeName.Equals("Scene");

            assetObj.AssetLoadOverCallbackList.Add(overCallback);

            if (m_AssetComponent.EditorResourceMode)
            {
                if (assetObj.IsScene)
                {
                    assetObj.Request = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(assetName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                }
                assetObj.Origin = OriginType.Editor;
                m_LoadingList.Add(assetPath, assetObj);
            }
            else
            {
                if (m_ABLoadManager.IsABExist(abPath))
                {
                    m_LoadingList.Add(assetPath, assetObj);
                    m_ABLoadManager.LoadAsync(abPath, (ABLoadManager.ABObject abObject, AssetBundle ab) =>
                    {
                        if (ab == null)
                        {
                            if (m_AssetComponent.StrictCheck)
                            {
                                Debug.LogError($"AssetLoadManager.LoadAsync异步加载错误！{assetObj.AssetPath}");
                            }
                            m_LoadingList.Remove(assetPath);
                            return;
                        }
                        if (m_LoadingList.ContainsKey(assetPath) && assetObj.Request == null)
                        {
                            if (assetObj.IsScene)
                            {
                                assetObj.Request = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(assetName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                            }
                            else
                            {
                                if (assetObj.Asset == null)
                                {
                                    Type assetType = Assembly.GetType($"UnityEngine.{typeName}");
                                    if (assetType != null)
                                    {
                                        assetObj.Request = ab.LoadAssetAsync(assetName, assetType);
                                    }
                                    else
                                    {
                                        assetObj.Request = ab.LoadAssetAsync(assetName);
                                    }
                                }
                            }
                        }
                    });

                    ABLoadManager.ABObject abObjectTryGet;
                    string path = ABLoadManager.GetABFormatPath(assetObj.ABPath);
                    if (ABLoadManager.LoadedABList.TryGetValue(path, out abObjectTryGet) || ABLoadManager.LoadingABList.TryGetValue(path, out abObjectTryGet))
                    {
                        assetObj.Origin = abObjectTryGet.Origin;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 预加载
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="isWeak">弱引用(当为true时表示使用过后会销毁，为false时将不会销毁，常驻内存，慎用)</param>
        public void PreLoadAsync(string typeName, string abPath, string assetName, AssetLoadOverCallback overCallback, bool isWeak = true)
        {
            string assetPath = GetAssetPath(typeName, abPath, assetName);
            AssetObject assetObj = null;
            if (m_LoadedList.ContainsKey(assetPath))
            {
                assetObj = m_LoadedList[assetPath];
            }
            else if (m_LoadingList.ContainsKey(assetPath))
            {
                assetObj = m_LoadingList[assetPath];
            }
            // 如果已经存在，改变其弱引用关系
            if (assetObj != null)
            {
                assetObj.IsWeak = isWeak;
                if (isWeak && assetObj.RefCount == 0 && !m_UnloadList.ContainsKey(assetPath))
                {
                    m_UnloadList.Add(assetPath, assetObj);
                }
                return;
            }

            PreloadAssetObject plAssetObj = new PreloadAssetObject();
            plAssetObj.TypeName = typeName;
            plAssetObj.ABPath = abPath;
            plAssetObj.AssetName = assetName;
            plAssetObj.AssetPath = assetPath;
            plAssetObj.IsScene = typeName.Equals("Scene");

            plAssetObj.IsWeak = isWeak;

            if (overCallback != null)
            {
                plAssetObj.AssetLoadOverCallback = overCallback;
            }

            m_PreloadedAsyncList.Enqueue(plAssetObj);
        }

        /// <summary>
        /// 资源销毁
        /// 请保证资源销毁都要调用这个接口
        /// </summary>
        /// <param name="oriAsset">asset资源（原始资源）</param>
        /// <param name="rightNow">是否立刻卸载</param>
        public void Unload(object oriAsset, AssetUnloadOverCallback overCallback = null, bool rightNow = false)
        {
            if (oriAsset == null) return;

            if (oriAsset is UnityEngine.SceneManagement.Scene)
            {
                AssetObject matchedScene = GetSceneAssetObjectByScene((UnityEngine.SceneManagement.Scene)oriAsset);
                if (matchedScene != null)
                {
                    matchedScene.RefCount--;
                    if (matchedScene.RefCount < 0)
                    {
                        if (m_AssetComponent.StrictCheck)
                        {
                            Debug.LogError($"AssetLoadManager Destroy 引用计数错误 ! assetName:{matchedScene.AssetPath}");
                        }
                        return;
                    }
                    if (matchedScene.RefCount == 0 && !m_UnloadList.ContainsKey(matchedScene.AssetPath))
                    {
                        matchedScene.UnloadTickNum = -1;
                        if (overCallback != null)
                        {
                            matchedScene.AssetUnloadOverCallbackList.Add(overCallback);
                        }
                        m_UnloadList.Add(matchedScene.AssetPath, matchedScene);
                        if (matchedScene.UnloadTickNum < 0)
                        {
                            UpdateUnload();
                        }
                    }
                }
            }
            else
            {
                UnityEngine.Object asset = oriAsset as UnityEngine.Object;
                int instanceID = asset.GetInstanceID();

                if (!m_AssetInstanceIDList.ContainsKey(instanceID))
                {
                    // 非从本类创建的资源，直接销毁即可
                    if (asset is GameObject)
                    {
                        UnityEngine.Object.Destroy(asset);
                    }
                    //else
                    //{
                    //    if (m_AssetComponent.StrictCheck)
                    //    {
                    //        Debug.LogError("AssetLoadManager Destroy 无 GameObject name=" + asset.name + " type=" + asset.GetType().Name);
                    //    }
                    //}
                    return;
                }

                var assetObj = m_AssetInstanceIDList[instanceID];
                if (assetObj.InstanceID == instanceID)
                {
                    assetObj.RefCount--;
                }
                else
                {
                    if (m_AssetComponent.StrictCheck)
                    {
                        Debug.LogError($"AssetLoadManager Destroy 错误 ! assetName:{assetObj.AssetPath}");
                    }
                    return;
                }

                if (assetObj.RefCount < 0)
                {
                    if (m_AssetComponent.StrictCheck)
                    {
                        Debug.LogError($"AssetLoadManager Destroy 引用计数错误 ! assetName:{assetObj.AssetPath}");
                    }
                    return;
                }

                if (assetObj.RefCount == 0 && !m_UnloadList.ContainsKey(assetObj.AssetPath))
                {
                    assetObj.UnloadTickNum = rightNow ? -1 : m_AssetComponent.UnloadAssetDelayFrameNum + m_UnloadList.Count;
                    if (overCallback != null)
                    {
                        assetObj.AssetUnloadOverCallbackList.Add(overCallback);
                    }
                    m_UnloadList.Add(assetObj.AssetPath, assetObj);
                    if (assetObj.UnloadTickNum < 0)
                    {
                        UpdateUnload();
                    }
                }
            }

        }

        /// <summary>
        /// 强制卸载未使用的Asset资源（异步）
        /// </summary>
        /// <param name="overcallback">结束回调</param>
        public void ForceUnloadUnusedAssets(Action overcallback = null)
        {
            if (m_UnloadList.Count > 0)
            {
                m_TempLoadeds.Clear();
                foreach (var assetObj in m_UnloadList.Values)
                {
                    if (assetObj.IsWeak && assetObj.RefCount == 0 && assetObj.AssetLoadOverCallbackList.Count == 0)
                    {
                        m_LoadedList.Remove(assetObj.AssetPath);
                        DoUnload(assetObj);
                        m_TempLoadeds.Add(assetObj);
                    }

                    // 正常在unload列表中对象的引用计数应该为0，如果此时发现外界对该对象又进行了重新load从而导致引用计数>0，这时需要从unload列表中马上移除来取消后面即将触发的释放操作。
                    if (assetObj.RefCount > 0 || !assetObj.IsWeak)
                    {
                        // 延迟卸载帧数清0
                        assetObj.UnloadTickNum = 0;
                        // 引用计数增加（销毁期间有加载）
                        m_TempLoadeds.Add(assetObj);
                    }
                }

                foreach (var assetObj in m_TempLoadeds)
                {
                    m_UnloadList.Remove(assetObj.AssetPath);
                }
            }

            AsyncOperation operation = Resources.UnloadUnusedAssets();
            GC.Collect();
            operation.completed += (oper) =>
            {
                GC.Collect();
                if (overcallback != null)
                {
                    overcallback();
                }
            };
        }

        /// <summary>
        /// 管理器心跳
        /// </summary>
        public void Update()
        {
            UpdatePreload();
            UpdateLoadedAsync();
            UpdateLoading();
            UpdateUnload();
            m_ABLoadManager.Update();

        }

        /// <summary>
        /// 判断资源文件是否存在
        /// 对打入atlas的图片无法判断，图片请用AtlasLoadMgr
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="abPath"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public bool IsFileExist(string typeName, string abPath, string assetName)
        {
            if (m_AssetComponent.EditorResourceMode)
            {
                string absoluteFullPath = GetAssetAbsoluteFullPath(typeName, abPath, assetName);
                if (string.IsNullOrEmpty(absoluteFullPath))
                {
                    return false;
                }
                return File.Exists(absoluteFullPath);
            }
            else
            {
                return m_ABLoadManager.IsABExist(abPath);
            }
        }

        /// <summary>
        /// 获取资源文件的后缀名
        /// </summary>
        /// <param name="typeName">资源的类型名称</param>
        /// <returns></returns>
        public string GetAssetSuffix(string typeName)
        {
            string suffix = string.Empty;
            if (m_AssetComponent.EditorResourceMode)
            {
                if(Enum.TryParse(typeof(Definitions.AssetType), typeName, out object tmpAssetType))
                {
                    suffix = Definitions.AssetSuffix[(Definitions.AssetType)tmpAssetType];
                }
            }
            return suffix;
        }

        /// <summary>
        /// 将外部加载得到的资源加入管理器（给其他地方调用）
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="asset">asset原始资源</param>
        public void AddAsset(string typeName, string abPath, string assetName, UnityEngine.Object asset)
        {
            string assetPath = GetAssetPath(typeName, abPath, assetName);

            var assetObj = new AssetObject();
            assetObj.TypeName = typeName;
            assetObj.ABPath = abPath;
            assetObj.AssetName = assetName;
            assetObj.AssetPath = assetPath;
            assetObj.IsScene = typeName.Equals("Scene");

            assetObj.RefCount = 1;

            if (assetObj.IsScene)
            {
                m_Scenes.Add(assetObj);
            }
            else
            {
                assetObj.InstanceID = asset.GetInstanceID();
                assetObj.Asset = asset;
                m_AssetInstanceIDList.Add(assetObj.InstanceID, assetObj);
            }
            m_LoadedList.Add(assetObj.AssetPath, assetObj);
        }

        /// <summary>
        /// 针对特定资源需要添加引用计数以保证引用计数的正确
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        public void AddAssetRef(string typeName, string abPath, string assetName)
        {
            string assetPath = GetAssetPath(typeName, abPath, assetName);

            if (!m_LoadedList.ContainsKey(assetPath))
            {
                if (m_AssetComponent.StrictCheck)
                {
                    Debug.LogError($"AssetLoadManager 追加引用计数错误 : {assetPath}");
                }
                return;
            }
            var assetObj = m_LoadedList[assetPath];
            assetObj.RefCount++;
        }

        /// <summary>
        /// 解绑回调
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="overCallback">asset资源加载完成的异步回调</param>
        public void RemoveCallBack(string typeName, string abPath, string assetName, AssetLoadOverCallback overCallback)
        {
            if (overCallback == null)
            {
                return;
            }
            string assetPath = GetAssetPath(typeName, abPath, assetName);

            // 对于不确定asset的回调，将根据回调函数删除
            if (string.IsNullOrEmpty(assetPath))
            {
                RemoveCallBackByCallBack(overCallback);
            }

            AssetObject assetObj = null;
            if (m_LoadedList.ContainsKey(assetPath))
            {
                assetObj = m_LoadedList[assetPath];
            }
            else if (m_LoadingList.ContainsKey(assetPath))
            {
                assetObj = m_LoadingList[assetPath];
            }

            if (assetObj != null)
            {
                int index = assetObj.AssetLoadOverCallbackList.IndexOf(overCallback);
                if (index >= 0)
                {
                    assetObj.AssetLoadOverCallbackList.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// 获取资源的绝对路径
        /// EditorMode下使用
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public string GetAssetAbsoluteFullPath(string typeName, string abPath, string assetName)
        {
            // 如果assetName不直接在abPath下则需要计算出准确的fileFullPath
            string abFullPath = $"{Application.dataPath.Substring(0, Application.dataPath.Length - s_AssetsStringLength)}{abPath}";
            // 如果是一个文件则直接返回即可
            string tryFileName = $"{abFullPath}{GetAssetSuffix(typeName)}";
            if (File.Exists(tryFileName))
            {
                return tryFileName;
            }
            else // 如果不是文件，则开始在该目录下进行搜索
            {
                string[] fileFullPaths = Directory.GetFiles(abFullPath, $"{assetName}{GetAssetSuffix(typeName)}", SearchOption.AllDirectories);
                if (fileFullPaths.Length == 0)
                {
                    if (m_AssetComponent.StrictCheck)
                    {
                        Debug.LogError($"目录 {abFullPath} 及其子目录中没有遍历到文件 {assetName}{GetAssetSuffix(typeName)} ！");
                    }
                    return null;
                }
                else if (fileFullPaths.Length != 1)
                {
                    if (m_AssetComponent.StrictCheck)
                    {
                        Debug.LogError($"目录 {abFullPath} 及其子目录中遍历到多个文件 {assetName}{GetAssetSuffix(typeName)} ！");
                    }
                    return null;
                }
                return fileFullPaths[0].Replace('\\', '/');
            }
        }

        /// <summary>
        /// 获取资源Assets开头的路径
        /// EditorMode下使用
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public string GetAssetRelativeFullPath(string typeName, string abPath, string assetName)
        {
            string absoluteFullPath = GetAssetAbsoluteFullPath(typeName, abPath, assetName);
            string relativeFullPath = absoluteFullPath.Substring(Application.dataPath.Length - s_AssetsStringLength, absoluteFullPath.Length - (Application.dataPath.Length - s_AssetsStringLength));
            return relativeFullPath;
        }

        /// <summary>
        /// 获取Asset唯一标识路径
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public string GetAssetPath(string typeName, string abPath, string assetName)
        {
            return $"{abPath}/{assetName}{GetAssetSuffix(typeName)}";
        }

        /// <summary>
        /// 从列表中获取加载中的AssetObject封装对象
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public AssetObject GetLoadingAssetObjectFromList(string typeName, string abPath, string assetName)
        {
            string assetPath = GetAssetPath(typeName, abPath, assetName);
            if (!string.IsNullOrEmpty(assetPath) && m_LoadingList.ContainsKey(assetPath))
            {
                return m_LoadingList[assetPath];
            }
            return null;
        }

        /// <summary>
        /// 从列表中获取已加载的AssetObject封装对象
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public AssetObject GetLoadedAssetObjectFromList(string typeName, string abPath, string assetName)
        {
            string assetPath = GetAssetPath(typeName, abPath, assetName);
            if (!string.IsNullOrEmpty(assetPath) && m_LoadedList.ContainsKey(assetPath))
            {
                return m_LoadedList[assetPath];
            }
            return null;
        }

        /// <summary>
        /// 从列表中获取卸载中的AssetObject封装对象
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public AssetObject GetUnLoadAssetObjectFromList(string typeName, string abPath, string assetName)
        {
            string assetPath = GetAssetPath(typeName, abPath, assetName);
            if (!string.IsNullOrEmpty(assetPath) && m_UnloadList.ContainsKey(assetPath))
            {
                return m_UnloadList[assetPath];
            }
            return null;
        }

    }
}


