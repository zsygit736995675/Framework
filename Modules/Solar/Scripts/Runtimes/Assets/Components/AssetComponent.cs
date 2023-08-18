/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetComponent.cs
 * author:    taoye
 * created:   2020/12/16
 * descrip:   Asset组件-对外接口
 ***************************************************************/
using System;
using UnityEngine;

namespace Solar.Runtime
{
    [DisallowMultipleComponent]
    public sealed partial class AssetComponent : MonoBehaviour
    {
        protected void Awake()
        {
            // 实例化Asset加载管理器
            m_AssetLoadManager = new AssetLoadManager(this);
            if (m_AssetLoadManager == null)
            {
                Debug.LogError("AssetLoadManager 无效。");
                return;
            }

            // 实例化Prefab加载管理器
            m_PrefabLoadManager = new PrefabLoadManager(this, m_AssetLoadManager);
            if (m_PrefabLoadManager == null)
            {
                Debug.LogError("PrefabLoadManager 无效。");
                return;
            }

            EditorResourceMode &= Application.isEditor;

        }

        private void Start()
        {
            // 加载Manifest
            LoadManifest();
        }

        private void Update()
        {
            if (m_AssetLoadManager != null)
            {
                m_AssetLoadManager.Update();
            }

            if (m_PrefabLoadManager != null)
            {
                m_PrefabLoadManager.Update();
            }

        }

        private void OnDestroy()
        {

        }

        /// <summary>
        /// 加载Manifest信息
        /// </summary>
        public void LoadManifest()
        {
            if (!EditorResourceMode)
            {
                if (m_AssetLoadManager != null)
                {
                    m_AssetLoadManager.ABLoadManager.LoadManifest();
                }
            }
        }

        /// <summary>
        /// 同步加载Prefab并实例化
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="parent">为实例指定的父对象</param>
        /// <param name="luaParams">自定义lua传入参数</param>
        /// <returns></returns>
        public GameObject LoadPrefabSync(string abPath, string assetName, Transform parent)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                Debug.LogError("AssetComponent.LoadPrefabSync abPath 无效。");
                return null;
            }

            if (!abPath.StartsWith("Assets"))
            {
                Debug.LogError($"AssetComponent.LoadPrefabSync abPath {abPath} 未以Assets开头。");
                return null;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                Debug.LogError("AssetComponent.LoadPrefabSync assetName 无效。");
                return null;
            }

            return m_PrefabLoadManager.LoadSync(abPath, assetName, parent);
        }

        /// <summary>
        /// 异步加载Prefab并实例化
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="parent">为实例指定的父节点</param>
        /// <param name="luaParams">自定义lua传入参数</param>
        /// <param name="overCallback">prefab加载完成并实例化结束的异步回调</param>
        public void LoadPrefabAsync(string abPath, string assetName, Transform parent, PrefabLoadOverCallback overCallback)
        {
            if (overCallback == null)
            {
                Debug.LogError("AssetComponent.LoadPrefabAsync overCallback 无效。");
                return;
            }

            if (string.IsNullOrEmpty(abPath))
            {
                Debug.LogError("AssetComponent.LoadPrefabAsync abPath 无效。");
                return;
            }

            if (!abPath.StartsWith("Assets"))
            {
                Debug.LogError($"AssetComponent.LoadPrefabAsync abPath: {abPath} 未以Assets开头。");
                return;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                Debug.LogError("AssetComponent.LoadPrefabAsync assetName 无效。");
                return;
            }

            m_PrefabLoadManager.LoadAsync(abPath, assetName, parent, overCallback);

        }

        /// <summary>
        /// 同步加载Asset资源
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAssetSync(string typeName, string abPath, string assetName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                Debug.LogError("AssetComponent.LoadAssetSync typeName 无效。");
                return null;
            }

            if (string.IsNullOrEmpty(abPath))
            {
                Debug.LogError("AssetComponent.LoadAssetSync abPath 无效。");
                return null;
            }

            if (!abPath.StartsWith("Assets"))
            {
                Debug.LogError($"AssetComponent.LoadAssetSync abPath {abPath} 未以Assets开头。");
                return null;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                Debug.LogError("AssetComponent.LoadAssetSync assetName 无效。");
                return null;
            }

            return m_AssetLoadManager.LoadSync(typeName, abPath, assetName);

        }

        /// <summary>
        /// 异步加载Asset资源
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="overCallback">回调函数</param>
        public void LoadAssetAsync(string typeName, string abPath, string assetName, AssetLoadOverCallback overCallback)
        {
            if (overCallback == null)
            {
                Debug.LogError("AssetComponent.LoadAssetAsync overCallback 无效。");
                return;
            }

            if (string.IsNullOrEmpty(typeName))
            {
                Debug.LogError("AssetComponent.LoadAssetAsync typeName 无效。");
                return;
            }

            if (string.IsNullOrEmpty(abPath))
            {
                Debug.LogError("AssetComponent.LoadAssetAsync abPath 无效。");
                return;
            }

            if (!abPath.StartsWith("Assets"))
            {
                Debug.LogError($"AssetComponent.LoadAssetAsync abPath: {abPath} 未以Assets开头。");
                return;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                Debug.LogError("AssetComponent.LoadAssetAsync assetName 无效。");
                return;
            }

            m_AssetLoadManager.LoadAsync(typeName, abPath, assetName, overCallback);

        }

        /// <summary>
        /// 异步预加载Asset资源
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="overCallback">回调函数</param>
        /// <param name="isWeak">弱引用(当为true时表示使用过后会销毁，为false时将不会销毁，常驻内存，慎用)</param>
        public void PreLoadAssetAsync(string typeName, string abPath, string assetName, AssetLoadOverCallback overCallback, bool isWeak = true)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                Debug.LogError("AssetComponent.PreLoadAssetAsync typeName 无效。");
                return;
            }

            if (string.IsNullOrEmpty(abPath))
            {
                Debug.LogError("AssetComponent.PreLoadAssetAsync abPath 无效。");
                return;
            }

            if (!abPath.StartsWith("Assets"))
            {
                Debug.LogError($"AssetComponent.PreLoadAssetAsync abPath: {abPath} 未以Assets开头。");
                return;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                Debug.LogError("AssetComponent.PreLoadAssetAsync assetName 无效。");
                return;
            }

            m_AssetLoadManager.PreLoadAsync(typeName, abPath, assetName, overCallback, isWeak);

        }

        /// <summary>
        /// 卸载Asset
        /// </summary>
        /// <param name="asset">asset原始资源</param>
        /// <param name="rightNow">是否立刻卸载</param>
        public void UnloadAsset(UnityEngine.Object asset, AssetUnloadOverCallback overCallback = null, bool rightNow = false)
        {
            if (asset == null)
            {
                Debug.LogError("AssetComponent.UnloadAsset asset 无效。");
                return;
            }
            m_AssetLoadManager.Unload(asset, overCallback, rightNow);
        }

        /// <summary>
        /// 强制卸载未使用的Asset资源（异步）
        /// 注意以下4点：（非常重要）
        /// 1.使用时需要格外注意调用时机，若为手动调用，请确保在调用接口时待卸载资源已处于【Asset准备卸载列表】中正处于过期延迟等待倒计时的状态。
        /// 2.Resources.UnloadUnusedAssets是异步接口，由于游戏场景越复杂、资源越多，该接口的开销也就相对越大，一般在0.3s~2s范围内。
        /// 3.避免在overCallback回调前对资源进行加载和释放操作，这样便可规避资源生命周期的混乱迭代（异步释放过程中资源被重复加载后又被重复释放导致资源获取为空的问题），强烈建议在overcallback中对结束状态进行监控并进行结束后的额外拓展。
        /// 4.为了游戏正常运行，在调用该接口前请确保游戏业务逻辑中资源的引用计数正确迭代（即Load与Unload配对使用），否则如若出现Load调用次数多余unload调用次数时，框架对资源的引用标记将失去实际的作用（调用该接口时资源引用计数>0，框架层维护的资源不认为会被释放，但是真实的资源却已被Unity释放掉了，最终将会导致游戏运行异常。）
        /// 建议使用的案例场景：
        /// Procedure之间进行切换时可调用该接口。流程A切换到流程B，在流程A中游戏内容全部销毁后（当流程A中游戏内容较多时，可能需要多帧迭代后才能完成真正的销毁操作，该间隔由游戏内容复杂度决定），调用该接口并在overcallback中初始化流程B的所有游戏内容。
        /// </summary>
        /// <param name="overCallback">结束回调</param>
        public void ForceUnloadUnusedAssets(Action overCallback = null)
        {
            m_AssetLoadManager.ForceUnloadUnusedAssets(overCallback);
        }

        /// <summary>
        /// 异步加载Scene
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <param name="sceneName">scene名称</param>
        public void LoadSceneSync(string abPath, string sceneName)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                Debug.LogError("AssetComponent.LoadSceneSync abPath 无效。");
                return;
            }

            if (!abPath.StartsWith("Assets"))
            {
                Debug.LogError($"AssetComponent.LoadSceneSync abPath: {abPath} 未以Assets开头。");
                return;
            }

            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("AssetComponent.LoadSceneSync sceneName 无效。");
                return;
            }

            m_AssetLoadManager.LoadSync("Scene", abPath, sceneName);

        }

        /// <summary>
        /// 异步加载Scene
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <param name="sceneName">scene名称</param>
        /// <param name="overCallback">回调函数</param>
        public void LoadSceneAsync(string abPath, string sceneName, AssetLoadOverCallback overCallback)
        {
            if (overCallback == null)
            {
                Debug.LogError("AssetComponent.LoadSceneAsync overCallback 无效。");
                return;
            }

            if (string.IsNullOrEmpty(abPath))
            {
                Debug.LogError("AssetComponent.LoadSceneAsync abPath 无效。");
                return;
            }

            if (!abPath.StartsWith("Assets"))
            {
                Debug.LogError($"AssetComponent.LoadSceneAsync abPath: {abPath} 未以Assets开头。");
                return;
            }

            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("AssetComponent.LoadSceneAsync sceneName 无效。");
                return;
            }

            m_AssetLoadManager.LoadAsync("Scene", abPath, sceneName, overCallback);

        }

        /// <summary>
        /// 异步预加载Scene
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <param name="sceneName">scene名称</param>
        public void PreLoadSceneAsync(string abPath, string sceneName, AssetLoadOverCallback overCallback, bool isWeak = true)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                Debug.LogError("AssetComponent.PreLoadSceneAsync abPath 无效。");
                return;
            }

            if (!abPath.StartsWith("Assets"))
            {
                Debug.LogError($"AssetComponent.PreLoadSceneAsync abPath: {abPath} 未以Assets开头。");
                return;
            }

            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("AssetComponent.PreLoadSceneAsync sceneName 无效。");
                return;
            }

            m_AssetLoadManager.PreLoadAsync("Scene", abPath, sceneName, overCallback, isWeak);
        }

        /// <summary>
        /// 卸载Scene
        /// </summary>
        /// <param name="asset">asset原始资源</param>
        public void UnloadScene(string sceneName, AssetUnloadOverCallback overCallback = null)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("AssetComponent.UnloadScene sceneName 无效。");
                return;
            }

            UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);

            if (scene == null)
            {
                Debug.LogError("AssetComponent.UnloadScene scene 无效。");
                return;
            }

            m_AssetLoadManager.Unload(scene, overCallback);
        }

        /// <summary>
        /// 判断Asset资源文件是否存在
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public bool IsAssetExist(string typeName, string abPath, string assetName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return false;
            }
            if (string.IsNullOrEmpty(abPath))
            {
                return false;
            }
            if (string.IsNullOrEmpty(assetName))
            {
                return false;
            }
            return m_AssetLoadManager.IsFileExist(typeName, abPath, assetName);
        }

    }

}
