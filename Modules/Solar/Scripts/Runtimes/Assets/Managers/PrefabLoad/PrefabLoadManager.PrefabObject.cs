/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PrefabLoadManager.PrefabObject.cs
 * author:    taoye
 * created:   2020/12/24
 * descrip:   Prefab加载管理器-Prefab封装对象
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Solar.Runtime
{
    public sealed partial class PrefabLoadManager
    {
        /// <summary>
        /// Prefab封装对象
        /// </summary>
        public class PrefabObject
        {
            /// <summary>
            /// AB包路径
            /// 以Assets开头
            /// </summary>
            public string ABPath;

            /// <summary>
            /// AB中Asset名称
            /// </summary>
            public string AssetName;

            /// <summary>
            /// 经ABPath与AssetName结合后Asset路径
            /// </summary>
            public string AssetPath;

            /// <summary>
            /// 本轮（帧）已经确定下来的需要在下一帧进行的回调数量
            /// 保证异步是下一帧回调
            /// </summary>
            public int LockCallbackCount;

            /// <summary>
            /// Prefab加载完成回调函数集合
            /// 按帧规律抛出
            /// </summary>
            public List<PrefabLoadOverCallback> PrefabLoadOverCallbackList = new List<PrefabLoadOverCallback>();

            /// <summary>
            /// Prefab实例化时指定的父对象集合
            /// 按帧规律缓存并设置后移除
            /// </summary>
            public List<Transform> PrefabInstancingGOParentList = new List<Transform>();

            /// <summary>
            /// Asset资源
            /// </summary>
            public UnityEngine.Object Asset;

            /// <summary>
            /// 引用计数
            /// </summary>
            public int RefCount;

            /// <summary>
            /// 通过该Prefab实例化的对象的实例ID集合
            /// 实时维护
            /// </summary>
            public HashSet<int> GOInstanceIDs = new HashSet<int>();

        }

    }
}


