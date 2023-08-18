/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ABLoadManager.Visitors.cs
 * author:    taoye
 * created:   2020/12/18
 * descrip:   AB加载管理器-访问器
 *            用于管理Asset与AB加载管理器
 ***************************************************************/
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Solar.Runtime
{
    public sealed partial class ABLoadManager
    {
        /// <summary>
        /// 允许同时加载的AB包最大数量
        /// </summary>
        private const int MAX_LOADING_COUNT = 10;

        /// <summary>
        /// 创建临时存储变量
        /// 用于提升性能
        /// </summary>
        private readonly List<ABObject> m_TempLoadeds;

        /// <summary>
        /// WebGL的AB请求
        /// </summary>
        private UnityWebRequest m_WebGLRequest;

        /// <summary>
        /// 全局依赖资源
        /// 记录了Manifest中所有AB的依赖资源关系
        /// </summary>
        private readonly Dictionary<string, string[]> m_DependsDataList;
        public Dictionary<string, string[]> DependsDataList
        {
            get
            {
                return m_DependsDataList;
            }
        }

        /// <summary>
        /// 预备加载的列表
        /// </summary>
        private readonly Dictionary<string, ABObject> m_ReadyABList;
        public Dictionary<string, ABObject> ReadyABList
        {
            get
            {
                return m_ReadyABList;
            }
        }

        /// <summary>
        /// 正在加载的列表
        /// 用于存放异步加载的AB封装对象
        /// </summary>
        private readonly Dictionary<string, ABObject> m_LoadingABList;
        public Dictionary<string, ABObject> LoadingABList
        {
            get
            {
                return m_LoadingABList;
            }
        }

        /// <summary>
        /// 加载完成的列表
        /// </summary>
        private readonly Dictionary<string, ABObject> m_LoadedABList;
        public Dictionary<string, ABObject> LoadedABList
        {
            get
            {
                return m_LoadedABList;
            }
        }

        /// <summary>
        /// 准备卸载的列表
        /// </summary>
        private readonly Dictionary<string, ABObject> m_UnloadABList;
        public Dictionary<string, ABObject> UnloadABList
        {
            get
            {
                return m_UnloadABList;
            }
        }

        /// <summary>
        /// Asset组件
        /// </summary>
        private readonly AssetComponent m_AssetComponent = null;

    }
}


