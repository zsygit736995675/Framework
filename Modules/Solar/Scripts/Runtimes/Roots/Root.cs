/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Root.cs
 * author:    taoye
 * created:   2020/12/16
 * descrip:   Root组件-入口（代码执行顺序最先）
 ***************************************************************/
using System;
using System.Globalization;
using System.Threading;
using UnityEngine;

namespace Solar.Runtime
{
    public partial class Root : MonoBehaviour
    {
        /// <summary>
        /// Asset组件
        /// </summary>
        public static AssetComponent Asset
        {
            get;
            private set;
        }

        private void Awake()
        {
            // 设置全局文化信息参数（避免区域性国家语种字符串数字等信息非标准化转换等问题）
            SetCultureInfo(CultureInfo.InvariantCulture);
        }

        private void Start()
        {
            Asset = GetComponentInChildren<AssetComponent>();
        }

        /// <summary>
        /// 设置全局文化信息参数
        /// </summary>
        /// <param name="cultureInfo">文化信息</param>
        private void SetCultureInfo(CultureInfo cultureInfo)
        {
            try
            {
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
        }

    }

}


