/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetLoadManager.PreloadAssetObject.cs
 * author:    taoye
 * created:   2020/12/18
 * descrip:   Asset加载管理器-预加载Asset封装对象
 ***************************************************************/

namespace Solar.Runtime
{
    public sealed partial class AssetLoadManager
    {
        /// <summary>
        /// 预加载Asset封装对象
        /// </summary>
        public class PreloadAssetObject
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
            /// 是否为Scene
            /// </summary>
            public bool IsScene;

            /// <summary>
            /// 是否是弱引用
            /// 用于预加载和释放
            /// 为true时，表示这个资源可以在没有引用时卸载，否则常驻内存。
            /// 常驻内存是指引用计数为0也不卸载。
            /// </summary>
            public bool IsWeak = true;

            /// <summary>
            /// Asset预加载完成回调函数
            /// </summary>
            public AssetLoadOverCallback AssetLoadOverCallback = null;

        }

    }
}


