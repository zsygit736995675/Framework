/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PrefabType.cs
 * author:    taoye
 * created:   2021/6/28
 * descrip:   预制体类型定义
 ***************************************************************/
using System;

namespace Solar.Runtime
{
    /// <summary>
    /// 预制体本体类型
    /// </summary>
    [Flags]
    public enum PrefabType : byte
    {
        /// <summary>
        /// UI类型
        /// </summary>
        UI = 0,

        /// <summary>
        /// 实体类型
        /// </summary>
        Entity,

    }
}


