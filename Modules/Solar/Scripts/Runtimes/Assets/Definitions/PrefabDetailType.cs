/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PrefabDetailType.cs
 * author:    taoye
 * created:   2021/6/28
 * descrip:   预制体详细类型定义
 ***************************************************************/
using System;

namespace Solar.Runtime
{
    /// <summary>
    /// 预制体本体类型
    /// </summary>
    public enum PrefabDetailType : byte
    {
        /// <summary>
        /// 无效
        /// </summary>
        None = 0,

        /// <summary>
        /// UI对象
        /// </summary>
        UI,

        /// <summary>
        /// 游戏对象
        /// </summary>
        GameObject,
    }
}


