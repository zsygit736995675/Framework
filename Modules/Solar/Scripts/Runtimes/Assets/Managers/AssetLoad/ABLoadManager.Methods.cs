/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ABLoadManager.Methods.cs
 * author:    taoye
 * created:   2020/12/18
 * descrip:   AB加载管理器-私有方法
 *            用于管理Asset与AB加载管理器
 ***************************************************************/
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Solar.Runtime
{
    public sealed partial class ABLoadManager
    {
        /// <summary>
        /// 内部同步加载AB
        /// </summary>
        /// <param name="abFormatPath">ab格式化路径</param>
        /// <returns></returns>
        private ABObject Internal_LoadAssetBundleSync(string abFormatPath)
        {
            ABObject abObj = null;
            // 如果处在【加载完成列表】中则直接返回（自身与其递归到的所有依赖AB项引用计数全部+1）
            if (m_LoadedABList.ContainsKey(abFormatPath))
            {
                abObj = m_LoadedABList[abFormatPath];
                abObj.RefCount++;

                foreach (var dpObj in abObj.Depends)
                {
                    Internal_LoadAssetBundleSync(dpObj.FormatPath);
                }

                return abObj;
            }
            // 如果处在【加载中列表（异步方式）】中则立即将异步改成同步加载得到结果（自身与其递归到的所有依赖AB项引用计数全部+1）
            else if (m_LoadingABList.ContainsKey(abFormatPath))
            {
                abObj = m_LoadingABList[abFormatPath];
                abObj.RefCount++;

                foreach (var dpObj in abObj.Depends)
                {
                    Internal_LoadAssetBundleSync(dpObj.FormatPath);
                }

                // 强制加载完成并回调
                NormalOrForceLoadOverAndCallBack(abObj);

                return abObj;
            }
            // 如果处在【准备加载列表】中则立即进行同步加载得到结果（自身与其递归到的所有依赖AB项引用计数全部+1）
            else if (m_ReadyABList.ContainsKey(abFormatPath))
            {
                abObj = m_ReadyABList[abFormatPath];
                abObj.RefCount++;

                foreach (var dpObj in abObj.Depends)
                {
                    Internal_LoadAssetBundleSync(dpObj.FormatPath);
                }

                string path1;
                OriginType origin1;
                GetABLoadPathOnDisk(abFormatPath, out path1, out origin1);

#if UNITY_WEBGL
                abObj.AB = LoadAssetBundleFromWebGL(abFormatPath);
#else
                abObj.AB = AssetBundle.LoadFromFile(path1);
#endif
                abObj.Origin = origin1;

                m_ReadyABList.Remove(abObj.FormatPath);
                m_LoadedABList.Add(abObj.FormatPath, abObj);

                // 强制加载完成并回调
                NormalOrForceLoadOverAndCallBack(abObj);

                return abObj;
            }

            // 如果三种列表中都不存在则直接创建一个新的同步加载
            abObj = new ABObject();
            abObj.FormatPath = abFormatPath;
            abObj.RefCount = 1;

            string path;
            OriginType origin;
            GetABLoadPathOnDisk(abFormatPath, out path, out origin);

#if UNITY_WEBGL
            abObj.AB = LoadAssetBundleFromWebGL(abFormatPath);
#else
            abObj.AB = AssetBundle.LoadFromFile(path);
#endif
            abObj.Origin = origin;

            // 用同步的方式加载依赖项
            string[] dependsData = null;
            if (m_DependsDataList.ContainsKey(abFormatPath))
            {
                dependsData = m_DependsDataList[abFormatPath];
            }

            if (dependsData != null && dependsData.Length > 0)
            {
                // 同步加载后将使加载中的依赖资源数量清零
                abObj.DependLoadingCount = 0;

                // 对依赖资源进行同步加载并记录依赖资源
                foreach (var dpFormatName in dependsData)
                {
                    var dpObj = Internal_LoadAssetBundleSync(dpFormatName);
                    abObj.Depends.Add(dpObj);
                }
            }

            // 将新创建的同步加载得到的资源加入到【已加载完成列表】中
            m_LoadedABList.Add(abObj.FormatPath, abObj);

            return abObj;
        }

        /// <summary>
        /// 内部异步加载AB
        /// </summary>
        /// <param name="abFormatPath">ab格式化路径</param>
        /// <param name="abLoadOverCallback">加载完成回调函数</param>
        /// <returns></returns>
        private ABObject Internal_LoadAssetBundleAsync(string abFormatPath, AssetBundleLoadOverCallBack abLoadOverCallback)
        {
            ABObject abObj = null;

            // 如果处在【加载完成列表】中则直接返回（自身与其所有的依赖AB项引用计数全部+1）
            if (m_LoadedABList.ContainsKey(abFormatPath))
            {
                abObj = m_LoadedABList[abFormatPath];
                AddSelfAndDependsRef(abObj);
                abLoadOverCallback(abObj, abObj.AB);
                return abObj;
            }
            // 如果处在【加载中列表（异步方式）】中则立即将新传入的回调函数加入ab封装对象中，等待异步结束后回调（自身与其所有的依赖AB项引用计数全部+1）
            else if (m_LoadingABList.ContainsKey(abFormatPath))
            {
                abObj = m_LoadingABList[abFormatPath];
                AddSelfAndDependsRef(abObj);
                abObj.ABLoadOverCallbacksList.Add(abLoadOverCallback);
                return abObj;
            }
            // 在准备加载中
            // 如果处在【准备加载列表】中则立即将新传入的回调函数加入ab封装对象中，等待异步开始到结束（自身与其所有的依赖AB项引用计数全部+1）
            else if (m_ReadyABList.ContainsKey(abFormatPath))
            {
                abObj = m_ReadyABList[abFormatPath];
                AddSelfAndDependsRef(abObj);
                abObj.ABLoadOverCallbacksList.Add(abLoadOverCallback);
                return abObj;
            }

            // 如果三种列表中都不存在则直接创建一个新的异步加载
            abObj = new ABObject();
            abObj.FormatPath = abFormatPath;

            abObj.RefCount = 1;
            abObj.ABLoadOverCallbacksList.Add(abLoadOverCallback);

            // 加载依赖项
            string[] dependsData = null;
            if (m_DependsDataList.ContainsKey(abFormatPath))
            {
                dependsData = m_DependsDataList[abFormatPath];
            }

            if (dependsData != null && dependsData.Length > 0)
            {
                // 将即将开始异步加载的依赖资源的总数量赋给当前新创建的ab资源中，等待后面异步加载依赖资源时对该数量的刷新
                abObj.DependLoadingCount = dependsData.Length;
                foreach (var dpFormatName in dependsData)
                {
                    var dpObj = Internal_LoadAssetBundleAsync(dpFormatName, (ABObject abObject, AssetBundle _ab) =>
                         {
                             if (abObj.DependLoadingCount <= 0)
                             {
                                 Debug.LogError($"加载依赖AB错误，ab名称:{abFormatPath}");
                                 return;
                             }

                             // 完成1个依赖资源的加载后，数量-1
                             abObj.DependLoadingCount--;

                             // 当所有依赖全部加载完毕后，触发正常加载完成的逻辑并触发回调
                             if (abObj.DependLoadingCount == 0)
                             {
#if UNITY_WEBGL
                                 NormalOrForceLoadOverAndCallBack(abObj);
#else
                                 if(abObj.Request != null && abObj.Request.isDone)
                                 {
                                     NormalOrForceLoadOverAndCallBack(abObj);
                                 }
#endif
                             }
                         }
                    );
                    // 将依赖资源记录到当前新创建的ab资源中
                    abObj.Depends.Add(dpObj);
                }

            }

            // 正在加载的数量不能超过上限
            if (m_LoadingABList.Count < MAX_LOADING_COUNT)
            {
                // 立即开始异步加载，并加入到【加载中列表】中
                DoLoadAsync(abObj);
                m_LoadingABList.Add(abFormatPath, abObj);
            }
            else
            {
                // 如果超过了上限，则暂时放入准备列表中
                m_ReadyABList.Add(abFormatPath, abObj);
            }

            return abObj;
        }

        /// <summary>
        /// 内部异步卸载AB
        /// </summary>
        /// <param name="abFormatPath">ab格式化路径</param>
        private void Internal_UnloadAssetBundleAsync(string abFormatPath)
        {
            ABObject abObj = null;

            // 获得可能存在于三种列表中的AB封装资源对象
            if (m_LoadedABList.ContainsKey(abFormatPath))
            {
                abObj = m_LoadedABList[abFormatPath];
            }
            else if (m_LoadingABList.ContainsKey(abFormatPath))
            {
                abObj = m_LoadingABList[abFormatPath];
            }
            else if (m_ReadyABList.ContainsKey(abFormatPath))
            {
                abObj = m_ReadyABList[abFormatPath];
            }

            if (abObj == null)
            {
                Debug.LogError($"卸载AB包错误，ab名称:{abFormatPath}");
                return;
            }

            if (abObj.RefCount == 0)
            {
                Debug.LogError($"卸载AB包时引用计数错误！ab名称:{abFormatPath}");
                return;
            }

            // 自身与其递归得到所有依赖AB资源的引用计数全部-1
            abObj.RefCount--;
            foreach (var dpObj in abObj.Depends)
            {
                Internal_UnloadAssetBundleAsync(dpObj.FormatPath);
            }

            // 引用计数-1后如果为0则加入到【卸载列表】中
            if (abObj.RefCount == 0)
            {
                if (!m_UnloadABList.ContainsKey(abObj.FormatPath))
                {
                    m_UnloadABList.Add(abObj.FormatPath, abObj);
                }
            }
        }

        /// <summary>
        /// ab自身与所有依赖项AB引用计数+1
        /// 递归调用
        /// </summary>
        /// <param name="abObj"></param>
        private void AddSelfAndDependsRef(ABObject abObj)
        {
            abObj.RefCount++;

            if (abObj.Depends.Count == 0) return;
            foreach (var dpObj in abObj.Depends)
            {
                // 递归依赖项，加载完
                AddSelfAndDependsRef(dpObj);
            }
        }

        /// <summary>
        /// 异步加载AB资源
        /// </summary>
        /// <param name="abObj">AB封装对象</param>
        private void DoLoadAsync(ABObject abObj)
        {
            string path;
            OriginType origin;
            GetABLoadPathOnDisk(abObj.FormatPath, out path, out origin);

#if UNITY_WEBGL
            abObj.AB = LoadAssetBundleFromWebGL(abObj.FormatPath);
#else
            abObj.Request = AssetBundle.LoadFromFileAsync(path);
            if (abObj.Request == null)
            {
                Debug.LogError($"加载AB包时的路径错误！ab名称:{abObj.FormatPath}");
            }
#endif
            abObj.Origin = origin;
        }

        /// <summary>
        /// 卸载AB资源
        /// </summary>
        /// <param name="abObj">AB封装对象</param>
        private void DoUnload(ABObject abObj)
        {
            // 这里用true，卸载Asset内存，实现指定卸载
            if (abObj.AB == null)
            {
                Debug.LogError($"卸载AB包时错误！ab名称:{abObj.FormatPath}");
                return;
            }

            abObj.AB.Unload(true);
            abObj.AB = null;
        }

        /// <summary>
        /// 正常或强制加载完成并触发回调
        /// </summary>
        /// <param name="abObj">AB封装对象</param>
        private void NormalOrForceLoadOverAndCallBack(ABObject abObj)
        {
            // 从异步中提取ab
            if (abObj.Request != null)
            {
                // 如果没加载完，通过API立刻拿到资源（变异步为同步）
                abObj.AB = abObj.Request.assetBundle;
                abObj.Request = null;
                m_LoadingABList.Remove(abObj.FormatPath);
                m_LoadedABList.Add(abObj.FormatPath, abObj);
            }

            if (abObj.AB == null)
            {
                string path;
                OriginType origin;
                GetABLoadPathOnDisk(abObj.FormatPath, out path, out origin);
#if UNITY_WEBGL
                abObj.AB = LoadAssetBundleFromWebGL(abObj.FormatPath);
#else
                abObj.AB = AssetBundle.LoadFromFile(path);
#endif
                abObj.Origin = origin;
            }

            // 运行回调
            foreach (var callback in abObj.ABLoadOverCallbacksList)
            {
                callback(abObj, abObj.AB);
            }
            abObj.ABLoadOverCallbacksList.Clear();
        }

        /// <summary>
        /// 获取ab包在磁盘上的加载路径
        /// </summary>
        /// <param name="formatPath">ab格式化路径</param>
        /// <param name="path">ab资源路径</param>
        /// <param name="origin">ab资源来源</param>
        private void GetABLoadPathOnDisk(string formatPath, out string path, out OriginType origin)
        {
            // 优先检查读写区域的资源是否存在，如果存在则加载读写区域的资源，否则加载只读区域
            string filePersistentPath = Path.AB.Persistent.GetFileFullPath(formatPath);
            if (File.Exists(filePersistentPath))
            {
                path = filePersistentPath;
                origin = OriginType.Persistent;
            }
            else
            {
                path = Path.AB.Streaming.GetFileFullPath(formatPath);
                origin = OriginType.Streaming;
            }
        }

        /// <summary>
        /// “加载中列表”心跳管理
        /// </summary>
        private void UpdateLoadingList()
        {
            if (m_LoadingABList.Count == 0) return;

            // 检测加载完的AB
            m_TempLoadeds.Clear();
            foreach (var abObj in m_LoadingABList.Values)
            {
                if (abObj.DependLoadingCount == 0)
                {
#if UNITY_WEBGL
                    m_TempLoadeds.Add(abObj);
#else
                    if(abObj.Request != null && abObj.Request.isDone)
                    {
                        m_TempLoadeds.Add(abObj);              
                    }
#endif
                }
            }
            // 回调中有可能对m_LoadingABList进行操作，提取后回调
            foreach (var abObj in m_TempLoadeds)
            {
                // 加载完进行回调
                NormalOrForceLoadOverAndCallBack(abObj);
            }

        }

        /// <summary>
        /// “卸载列表”心跳管理
        /// </summary>
        private void UpdateUnLoadList()
        {
            if (m_UnloadABList.Count == 0) return;

            m_TempLoadeds.Clear();
            foreach (var abObj in m_UnloadABList.Values)
            {
                if (abObj.RefCount == 0 && abObj.AB != null)
                {
                    // 引用计数为0并且已经加载完，没加载完等加载完销毁
                    DoUnload(abObj);

                    m_LoadedABList.Remove(abObj.FormatPath);
                    m_TempLoadeds.Add(abObj);
                }

                if (abObj.RefCount > 0)
                {
                    // 引用计数加回来（销毁又瞬间重新加载，不销毁，从销毁列表移除）
                    m_TempLoadeds.Add(abObj);
                }
            }

            foreach (var abObj in m_TempLoadeds)
            {
                m_UnloadABList.Remove(abObj.FormatPath);
            }
        }

        /// <summary>
        /// “准备列表”心跳管理
        /// </summary>
        private void UpdateReadyList()
        {
            if (m_ReadyABList.Count == 0) return;
            if (m_LoadingABList.Count >= MAX_LOADING_COUNT) return;

            m_TempLoadeds.Clear();
            foreach (var abObj in m_ReadyABList.Values)
            {
                DoLoadAsync(abObj);

                m_TempLoadeds.Add(abObj);
                m_LoadingABList.Add(abObj.FormatPath, abObj);

                if (m_LoadingABList.Count >= MAX_LOADING_COUNT)
                {
                    break;
                }
            }

            foreach (var abObj in m_TempLoadeds)
            {
                m_ReadyABList.Remove(abObj.FormatPath);
            }
        }

    }
}


