/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PrefabLoadManager.Visitors.cs
 * author:    taoye
 * created:   2020/12/24
 * descrip:   Prefab加载管理器-访问器
 ***************************************************************/

using System.Collections.Generic;

namespace Solar.Runtime
{
    public sealed partial class PrefabLoadManager
    {
        /// <summary>
        /// 加载完成列表
        /// </summary>
        private readonly Dictionary<string, PrefabObject> m_LoadedList;
        public Dictionary<string, PrefabObject> LoadedList
        {
            get
            {
                return m_LoadedList;
            }
        }

        /// <summary>
        /// 异步加载临时中转列表
        /// 用于延迟回调
        /// 当调用异步加载时由于之前已经同步加载过了，所以为了遵循异步加载的异步回调规则，这里临时记录，方便接下来的异步回调
        /// </summary>
        private readonly List<PrefabObject> m_LoadedAsyncTmpAgentList;

        /// <summary>
        /// 创建的所有实例的实例ID所对应的Prefab封装对象
        /// </summary>
        private readonly Dictionary<int, PrefabObject> m_GOInstanceIDList;
        public Dictionary<int, PrefabObject> GOInstanceIDList
        {
            get
            {
                return m_GOInstanceIDList;
            }
        }

        /// <summary>
        /// Asset加载管理器
        /// </summary>
        private readonly AssetLoadManager m_AssetLoadManager = null;
        public AssetLoadManager AssetLoadManager
        {
            get
            {
                return m_AssetLoadManager;
            }
        }

        /// <summary>
        /// Asset组件
        /// </summary>
        private readonly AssetComponent m_AssetComponent = null;

    }
}


