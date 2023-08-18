/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetLoadManager.Methods.cs
 * author:    taoye
 * created:   2020/12/18
 * descrip:   Asset加载管理器-私有方法
 ***************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Solar.Runtime
{
    public sealed partial class AssetLoadManager
    {
        /// <summary>
        /// 根据回调解绑回调
        /// </summary>
        /// <param name="overCallback">asset资源加载完成的异步回调</param>
        private void RemoveCallBackByCallBack(AssetLoadOverCallback overCallback)
        {
            foreach (var assetObj in m_LoadingList.Values)
            {
                if (assetObj.AssetLoadOverCallbackList.Count == 0) continue;
                int index = assetObj.AssetLoadOverCallbackList.IndexOf(overCallback);
                if (index >= 0)
                {
                    assetObj.AssetLoadOverCallbackList.RemoveAt(index);
                }
            }

            foreach (var assetObj in m_LoadedList.Values)
            {
                if (assetObj.AssetLoadOverCallbackList.Count == 0) continue;
                int index = assetObj.AssetLoadOverCallbackList.IndexOf(overCallback);
                if (index >= 0)
                {
                    assetObj.AssetLoadOverCallbackList.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// 内部执行Asset资源加载完成的回调
        /// </summary>
        /// <param name="assetObj">asset封装对象</param>
        private void DoAssetCallback(AssetObject assetObj)
        {
            if (assetObj.AssetLoadOverCallbackList.Count == 0)
            {
                return;
            }

            // 先提取count，保证回调中有加载需求不加载
            int count = assetObj.LockCallbackCount;
            for (int i = 0; i < count; i++)
            {
                if (assetObj.AssetLoadOverCallbackList[i] != null)
                {
                    // 每次回调，引用计数+1
                    assetObj.RefCount++;

                    try
                    {
                        assetObj.AssetLoadOverCallbackList[i](assetObj, assetObj.Asset);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
            assetObj.AssetLoadOverCallbackList.RemoveRange(0, count);
        }

        /// <summary>
        /// 内部执行Asset资源的卸载
        /// </summary>
        /// <param name="assetObj">asset封装对象</param>
        private void DoUnload(AssetObject assetObj)
        {
            if (assetObj.IsScene)
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(assetObj.AssetName);
            }

            //// 注意：Resources.UnloadAsset仅能释放非GameObject和Component的资源，比如Texture、Mesh等真正的资源。对于由Prefab加载出来的Object或Component，则不能通过该函数来进行释放。
            //if (assetObj.Asset.GetType() != typeof(GameObject) && assetObj.Asset.GetType() != typeof(Component))
            //{
            //    Resources.UnloadAsset(assetObj.Asset);
            //}

            if (!m_AssetComponent.EditorResourceMode)
            {
                m_ABLoadManager.Unload(assetObj.ABPath);
            }

            assetObj.Asset = null;

            if (assetObj.IsScene)
            {
                m_Scenes.Remove(assetObj);
            }
            else
            {
                if (m_AssetInstanceIDList.ContainsKey(assetObj.InstanceID))
                {
                    m_AssetInstanceIDList.Remove(assetObj.InstanceID);
                }
            }

            // unload完毕回调
            if (assetObj.AssetUnloadOverCallbackList != null)
            {
                foreach (AssetUnloadOverCallback overCallback in assetObj.AssetUnloadOverCallbackList)
                {
                    if (overCallback != null)
                    {
                        overCallback(assetObj);
                    }
                }
            }

        }

        /// <summary>
        /// “预加载列表”心跳管理
        /// </summary>
        private void UpdatePreload()
        {
            if (m_LoadingList.Count > 0 || m_PreloadedAsyncList.Count == 0)
            {
                return;
            }

            // 从队列取出一个Asset封装对象进行异步加载
            PreloadAssetObject plAssetObj = null;
            while (m_PreloadedAsyncList.Count > 0 && plAssetObj == null)
            {
                plAssetObj = m_PreloadedAsyncList.Dequeue();

                if (m_LoadingList.ContainsKey(plAssetObj.AssetPath))
                {
                    m_LoadingList[plAssetObj.AssetPath].IsWeak = plAssetObj.IsWeak;
                }
                else if (m_LoadedList.ContainsKey(plAssetObj.AssetPath))
                {
                    m_LoadedList[plAssetObj.AssetPath].IsWeak = plAssetObj.IsWeak;
                    plAssetObj = null;
                }
                else
                {
                    LoadAsync(plAssetObj.TypeName, plAssetObj.ABPath, plAssetObj.AssetName, plAssetObj.AssetLoadOverCallback);
                    if (m_LoadingList.ContainsKey(plAssetObj.AssetPath))
                    {
                        m_LoadingList[plAssetObj.AssetPath].IsWeak = plAssetObj.IsWeak;
                    }
                    else if (m_LoadedList.ContainsKey(plAssetObj.AssetPath))
                    {
                        m_LoadedList[plAssetObj.AssetPath].IsWeak = plAssetObj.IsWeak;
                    }
                }
            }
        }

        /// <summary>
        /// “加载完成列表”心跳管理
        /// </summary>
        private void UpdateLoadedAsync()
        {
            if (m_LoadedAsyncTmpAgentList.Count == 0) return;

            int count = m_LoadedAsyncTmpAgentList.Count;
            for (int i = 0; i < count; i++)
            {
                // 先锁定回调数量，保证异步成立
                m_LoadedAsyncTmpAgentList[i].LockCallbackCount = m_LoadedAsyncTmpAgentList[i].AssetLoadOverCallbackList.Count;
            }
            for (int i = 0; i < count; i++)
            {
                DoAssetCallback(m_LoadedAsyncTmpAgentList[i]);
            }
            m_LoadedAsyncTmpAgentList.RemoveRange(0, count);

            if (m_LoadingList.Count == 0 && m_LoadingIntervalCount > m_AssetComponent.LoadedMaxNumToCleanMemery)
            {
                // 在连续的大量加载后，强制调用一次gc
                m_LoadingIntervalCount = 0;
                System.GC.Collect();
            }
        }

        /// <summary>
        /// “加载中列表”心跳管理
        /// </summary>
        private void UpdateLoading()
        {
            if (m_LoadingList.Count == 0) return;

            // 检测加载完的
            m_TempLoadeds.Clear();
            foreach (var assetObj in m_LoadingList.Values)
            {
                if (m_AssetComponent.EditorResourceMode)
                {
#if UNITY_EDITOR
                    if (assetObj.IsScene)
                    {
                        if (assetObj.Request != null && assetObj.Request.isDone)
                        {
                            m_Scenes.Add(assetObj);
                            assetObj.Request = null;
                            m_TempLoadeds.Add(assetObj);
                        }
                    }
                    else
                    {
                        //if (UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetObj.ABPath.ToLower(), assetObj.AssetName).Length > 0)
                        {
                            string assetRelativeFullPath = GetAssetRelativeFullPath(assetObj.TypeName, assetObj.ABPath, assetObj.AssetName);
                            Type assetType = Assembly.GetType($"UnityEngine.{assetObj.TypeName}");
                            if (assetType != null)
                            {
                                assetObj.Asset = UnityEditor.AssetDatabase.LoadAssetAtPath(assetRelativeFullPath, assetType);
                            }
                            else
                            {
                                assetObj.Asset = UnityEditor.AssetDatabase.LoadAssetAtPath(assetRelativeFullPath, typeof(UnityEngine.Object));
                            }
                        }

                        if (assetObj.Asset == null)
                        {
                            // 提取的资源失败，从加载列表删除
                            m_LoadingList.Remove(assetObj.AssetPath);
                            Debug.LogError($"AssetLoadManager assetObj.Asset Null : {assetObj.AssetPath}");
                            break;
                        }

                        assetObj.InstanceID = assetObj.Asset.GetInstanceID();
                        m_AssetInstanceIDList.Add(assetObj.InstanceID, assetObj);
                        assetObj.Request = null;
                        m_TempLoadeds.Add(assetObj);
                    }
#endif
                }
                else
                {
                    if (assetObj.Request != null && assetObj.Request.isDone)
                    {
                        if (assetObj.IsScene)
                        {
                            m_Scenes.Add(assetObj);
                        }
                        else
                        {
                            // 加载完进行数据清理
                            if (assetObj.Request is AssetBundleRequest)
                            {
                                assetObj.Asset = (assetObj.Request as AssetBundleRequest).asset;
                            }
                            if (assetObj.Asset == null)
                            {
                                // 提取的资源失败，从加载列表删除
                                m_LoadingList.Remove(assetObj.AssetPath);
                                Debug.LogError($"AssetLoadManager assetObj.Asset Null : {assetObj.AssetPath}");
                                break;
                            }
                            assetObj.InstanceID = assetObj.Asset.GetInstanceID();
                            m_AssetInstanceIDList.Add(assetObj.InstanceID, assetObj);
                        }

                        assetObj.Request = null;
                        m_TempLoadeds.Add(assetObj);
                    }
                }
            }

            // 回调中有可能对m_LoadingList进行操作，先移动
            foreach (var assetObj in m_TempLoadeds)
            {
                m_LoadingList.Remove(assetObj.AssetPath);
                m_LoadedList.Add(assetObj.AssetPath, assetObj);

                // 统计本轮加载的数量
                m_LoadingIntervalCount++;

                // 先锁定回调数量，保证异步成立
                assetObj.LockCallbackCount = assetObj.AssetLoadOverCallbackList.Count;
            }

            // 统一进行加载完成的回调派发
            foreach (var assetObj in m_TempLoadeds)
            {
                DoAssetCallback(assetObj);
            }
        }

        /// <summary>
        /// “卸载列表”心跳管理
        /// </summary>
        private void UpdateUnload()
        {
            if (m_UnloadList.Count == 0) return;

            m_TempLoadeds.Clear();
            foreach (var assetObj in m_UnloadList.Values)
            {
                if (assetObj.IsWeak && assetObj.RefCount == 0 && assetObj.AssetLoadOverCallbackList.Count == 0)
                {
                    // 引用计数为0，延迟卸载时间到，且没有需要回调的函数，销毁
                    if (assetObj.UnloadTickNum < 0)
                    {
                        m_LoadedList.Remove(assetObj.AssetPath);
                        DoUnload(assetObj);
                        m_TempLoadeds.Add(assetObj);
                    }
                    else
                    {
                        assetObj.UnloadTickNum--;
                    }
                }

                // 正常在unload列表中对象的引用计数应该为0，如果此时发现外界对该对象又进行了重新load从而导致引用计数>0，这时需要从unload列表中马上移除来取消后面即将触发的释放操作。
                if (assetObj.RefCount > 0 || !assetObj.IsWeak)
                {
                    // 延迟卸载帧数清0
                    assetObj.UnloadTickNum = 0;
                    // 引用计数增加（销毁期间有加载）
                    m_TempLoadeds.Add(assetObj);
                }
            }

            foreach (var assetObj in m_TempLoadeds)
            {
                m_UnloadList.Remove(assetObj.AssetPath);
            }

        }

        /// <summary>
        /// 根据场景对象从当前已经激活的场景集合中获取对应的Asset封装对象
        /// </summary>
        /// <param name="scene">场景对象</param>
        /// <returns></returns>
        private AssetObject GetSceneAssetObjectByScene(UnityEngine.SceneManagement.Scene scene)
        {
            AssetObject result = m_Scenes.Find((assetObject) =>
            {
                if (assetObject.AssetName == scene.name)
                {
                    return true;
                }
                return false;
            });
            return result;
        }

    }
}
