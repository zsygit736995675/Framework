/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetComponent.Visitors.cs
 * author:    taoye
 * created:   2020/12/16
 * descrip:   Asset组件-访问器
 ***************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace Solar.Runtime
{
    public sealed partial class AssetComponent : MonoBehaviour
    {
        /// <summary>
        /// 是否为编辑器资源模式
        /// </summary>
        /// <returns></returns>
        [SerializeField]
        public bool EditorResourceMode;

        /// <summary>
        /// Asset最小过期帧数
        /// Asset资源过期帧数，60帧*60s：相当于1分钟
        /// </summary>
        [SerializeField]
        private int m_UnloadAssetDelayFrameNum = 60 * 60;
        public int UnloadAssetDelayFrameNum
        {
            get
            {
                return m_UnloadAssetDelayFrameNum;
            }
            set
            {
                m_UnloadAssetDelayFrameNum = value;
            }
        }

        /// <summary>
        /// 每轮资源清理所需要加载完成资源的累计个数
        /// </summary>
        [SerializeField]
        private int m_LoadedMaxNumToCleanMemery = 50;
        public int LoadedMaxNumToCleanMemery
        {
            get
            {
                return m_LoadedMaxNumToCleanMemery;
            }
            set
            {
                m_LoadedMaxNumToCleanMemery = value;
            }
        }

        /// <summary>
        /// Asset加载管理器
        /// </summary>
        private AssetLoadManager m_AssetLoadManager = null;
        public AssetLoadManager AssetLoadManager
        {
            get
            {
                return m_AssetLoadManager;
            }
        }

        /// <summary>
        /// Prefab加载管理器
        /// </summary>
        private PrefabLoadManager m_PrefabLoadManager = null;
        public PrefabLoadManager PrefabLoadManager
        {
            get
            {
                return m_PrefabLoadManager;
            }
        }

        /// <summary>
        /// 全局依赖资源
        /// 记录了Manifest中所有AB的依赖资源关系
        /// </summary>
        public Dictionary<string, string[]> DependsDataList
        {
            get
            {
                return m_AssetLoadManager.ABLoadManager.DependsDataList;
            }
        }

        /// <summary>
        /// 获取Prefab加载完成列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, PrefabLoadManager.PrefabObject> LoadedPrefabList
        {
            get
            {
                return m_PrefabLoadManager.LoadedList;
            }
        }

        /// <summary>
        /// 获取正在加载的Asset列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, AssetLoadManager.AssetObject> LoadingAssetList
        {
            get
            {
                return m_AssetLoadManager.LoadingList;
            }
        }

        /// <summary>
        /// 获取加载完成的Asset列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, AssetLoadManager.AssetObject> LoadedAssetList
        {
            get
            {
                return m_AssetLoadManager.LoadedList;
            }
        }

        /// <summary>
        /// 获取准备卸载的Asset列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, AssetLoadManager.AssetObject> UnloadAssetList
        {
            get
            {
                return m_AssetLoadManager.UnloadList;
            }
        }

        /// <summary>
        /// 获取预加载的Asset列表
        /// </summary>
        /// <returns></returns>
        public Queue<AssetLoadManager.PreloadAssetObject> PreloadedAssetList
        {
            get
            {
                return m_AssetLoadManager.PreloadedAsyncList;
            }
        }

        /// <summary>
        /// 获取所有当前AB准备列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ABLoadManager.ABObject> ReadyABList
        {
            get
            {
                return m_AssetLoadManager.ABLoadManager.ReadyABList;
            }
        }

        /// <summary>
        /// 获取所有当前加载中的AB列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ABLoadManager.ABObject> LoadingABList
        {
            get
            {
                return m_AssetLoadManager.ABLoadManager.LoadingABList;
            }
        }

        /// <summary>
        /// 获取所有当前已加载的AB包
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ABLoadManager.ABObject> LoadedABList
        {
            get
            {
                return m_AssetLoadManager.ABLoadManager.LoadedABList;
            }
        }

        /// <summary>
        /// 获取所有当前待卸载AB列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ABLoadManager.ABObject> UnloadABList
        {
            get
            {
                return m_AssetLoadManager.ABLoadManager.UnloadABList;
            }
        }

        /// <summary>
        /// 获取当前所有激活的场景集合
        /// </summary>
        public List<AssetLoadManager.AssetObject> Scenes
        {
            get
            {
                return m_AssetLoadManager.Scenes;
            }
        }

        /// <summary>
        /// 是否严格检测
        /// </summary>
        private bool m_StrictCheck = true;
        public bool StrictCheck
        {
            set
            {
                m_StrictCheck = value;
            }
            get
            {
                return m_StrictCheck;
            }
        }

    }

}
