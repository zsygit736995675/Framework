/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ABLoadManager.ABObject.cs
 * author:    taoye
 * created:   2020/12/18
 * descrip:   AB加载管理器-AB包封装对象
 ***************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace Solar.Runtime
{
    public sealed partial class ABLoadManager
    {
        /// <summary>
        /// AB包封装对象
        /// </summary>
        public class ABObject
        {
            /// <summary>
            /// AB标准名称格式
            /// </summary>
            public string FormatPath;

            /// <summary>
            /// 引用计数
            /// </summary>
            public int RefCount;

            /// <summary>
            /// 依赖资源总数
            /// </summary>
            public int DependLoadingCount;

            /// <summary>
            /// 资源位置来源
            /// </summary>
            public OriginType Origin;

            /// <summary>
            /// AB异步加载请求
            /// </summary>
            public AssetBundleCreateRequest Request;

            /// <summary>
            /// AssetBundle
            /// </summary>
            public AssetBundle AB;

            /// <summary>
            /// 依赖AB集合
            /// </summary>
            public readonly List<ABObject> Depends = new List<ABObject>();

            /// <summary>
            /// AB异步加载完成回调列表
            /// </summary>
            public readonly List<AssetBundleLoadOverCallBack> ABLoadOverCallbacksList = new List<AssetBundleLoadOverCallBack>();
        }

    }
}


