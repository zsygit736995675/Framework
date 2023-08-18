/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  OriginType.cs
 * author:    taoye
 * created:   2022/3/15
 * descrip:   资源位置来源类型定义
 ***************************************************************/
using System;

namespace Solar.Runtime
{
    /// <summary>
    /// 资源位置来源类型
    /// </summary>
    [Flags]
    public enum OriginType : byte
    {
        /// <summary>
        /// 无效
        /// </summary>
        None = 0,

        /// <summary>
        /// 编辑器区
        /// </summary>
        Editor,

        /// <summary>
        /// 读写区
        /// </summary>
        Persistent,

        /// <summary>
        /// 只读区
        /// </summary>
        Streaming,

    }
}


