/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetLoadManager.AssetObject.cs
 * author:    taoye
 * created:   2020/12/18
 * descrip:   Asset加载管理器-Asset封装对象
 ***************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace Solar.Runtime
{
    public sealed partial class AssetLoadManager
    {
        /// <summary>
        /// Asset封装对象
        /// </summary>
        public class AssetObject
        {
            /// <summary>
            /// Asset类型名称
            /// </summary>
            public string TypeName;

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
            /// 资源位置来源
            /// </summary>
            public OriginType Origin;

            /// <summary>
            /// 是否为Scene
            /// </summary>
            public bool IsScene;

            /// <summary>
            /// 本轮（帧）已经确定下来的需要在下一帧进行的回调数量
            /// 保证异步是下一帧回调
            /// </summary>
            public int LockCallbackCount;

            /// <summary>
            /// Asset加载完成回调函数集合
            /// </summary>
            public List<AssetLoadOverCallback> AssetLoadOverCallbackList = new List<AssetLoadOverCallback>();

            /// <summary>
            /// Asset卸载完成回调函数集合
            /// </summary>
            public List<AssetUnloadOverCallback> AssetUnloadOverCallbackList = new List<AssetUnloadOverCallback>();

            /// <summary>
            /// Asset实例ID
            /// </summary>
            public int InstanceID;

            /// <summary>
            /// 异步请求
            /// </summary>
            public AsyncOperation Request;

            /// <summary>
            /// Asset资源
            /// 当加载资源为Scene时该Asset为null
            /// </summary>
            public UnityEngine.Object Asset;

            /// <summary>
            /// 是否是弱引用
            /// 用于预加载和释放
            /// 为true时，表示这个资源可以在没有引用时卸载，否则常驻内存。
            /// 常驻内存是指引用计数为0也不卸载。
            /// </summary>
            public bool IsWeak = true;

            /// <summary>
            /// 引用计数
            /// </summary>
            public int RefCount;

            /// <summary>
            /// 延迟卸载的帧数
            /// UNLOAD_DELAY_FIXED_FRAME_NUM + m_UnloadList.Count
            /// 用上面的方式赋值来保证在加入unload释放队列中的时候一定是后加入的Asset比前一个加入的Asset晚一帧释放
            /// 这样可以保证在某个时刻大量卸载的时候，资源卸载的压力平摊到后面一段时间上，兼顾效率和内存
            /// </summary>
            public int UnloadTickNum;
        }

    }
}
