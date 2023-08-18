/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PrefabLoadManager.Methods.cs
 * author:    taoye
 * created:   2020/12/24
 * descrip:   Prefab加载管理器-私有方法
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Solar.Runtime
{
    public sealed partial class PrefabLoadManager
    {
        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="prefabObj">Prefab封装对象</param>
        /// <param name="parent">指定的父节点</param>
        /// <returns>实例</returns>
        private GameObject InstanceGO(PrefabObject prefabObj, Transform parent)
        {
            GameObject go = GameObject.Instantiate(prefabObj.Asset, parent, false) as GameObject;
            go.name = go.name.Replace("(Clone)", "");
            PrefabInstanceGOBehaviour goBehaviour = go.AddComponent<PrefabInstanceGOBehaviour>();

            if (!go.activeSelf)
            {
                // 保证GameObject active一次，ObjInfo才能触发Awake，未Awake的脚本不能触发OnDestroy，不触发Awake和OnDestroy的情况下引用计数会出错
                go.SetActive(true);
                go.SetActive(false);
            }

            // 重新处理因上述特殊情况下导致父对象变更的情况
            if (parent != null)
            {
                go.transform.SetParent(parent, false);
            }

            int instanceID = go.GetInstanceID();
            if (goBehaviour != null)
            {
                goBehaviour.InstanceID = instanceID;
                goBehaviour.ABPath = prefabObj.ABPath;
                goBehaviour.AssetName = prefabObj.AssetName;
            }

            prefabObj.GOInstanceIDs.Add(instanceID);
            m_GOInstanceIDList.Add(instanceID, prefabObj);

            return go;
        }

        /// <summary>
        /// 实例化对象并回调
        /// </summary>
        private void InstanceGOWithCallback(PrefabObject prefabObj)
        {
            if (prefabObj.PrefabLoadOverCallbackList.Count == 0) return;

            // 先将回调提取保存出来缓存（保证回调中可能出现的加载和销毁不出错）
            int count = prefabObj.LockCallbackCount;
            var callbackList = prefabObj.PrefabLoadOverCallbackList.GetRange(0, count);
            var callParentList = prefabObj.PrefabInstancingGOParentList.GetRange(0, count);

            prefabObj.LockCallbackCount = 0;
            prefabObj.PrefabLoadOverCallbackList.RemoveRange(0, count);
            prefabObj.PrefabInstancingGOParentList.RemoveRange(0, count);

            for (int i = 0; i < count; i++)
            {
                if (callbackList[i] != null)
                {
                    GameObject go = InstanceGO(prefabObj, callParentList[i]);

                    try
                    {
                        callbackList[i](prefabObj, go);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }

        /// <summary>
        /// “加载完成异步回调列表”心跳管理
        /// </summary>
        private void UpdateLoadedAsync()
        {
            // 处理：当调用异步加载时因之前已经同步加载过了，所以为了遵循异步加载的异步回调规则而临时记录过的Prefab封装对象需要在此处进行异步回调

            if (m_LoadedAsyncTmpAgentList.Count == 0) return;

            int count = m_LoadedAsyncTmpAgentList.Count;
            for (int i = 0; i < count; i++)
            {
                // 异步回调前需要最大程度的收集接下来需要进行回调的数量
                m_LoadedAsyncTmpAgentList[i].LockCallbackCount = m_LoadedAsyncTmpAgentList[i].PrefabLoadOverCallbackList.Count;
            }

            for (int i = 0; i < count; i++)
            {
                InstanceGOWithCallback(m_LoadedAsyncTmpAgentList[i]);
            }
            m_LoadedAsyncTmpAgentList.RemoveRange(0, count);

        }

    }
}


