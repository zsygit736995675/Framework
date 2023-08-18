/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PrefabInstanceGOBehaviour.cs
 * author:    taoye
 * created:   2020/12/23
 * descrip:   Prefab实例GO通用组件（用来关联Prefab加载管理器）
 ***************************************************************/

using UnityEngine;

namespace Solar.Runtime
{
    public class PrefabInstanceGOBehaviour : MonoBehaviour
    {
        public int InstanceID = -1;
        public string ABPath = string.Empty;
        public string AssetName = string.Empty;
        public bool RightNowDestroyOnAsset = false;

        void Awake()
        {
            if (string.IsNullOrEmpty(ABPath))
            {
                return;
            }

            if (string.IsNullOrEmpty(AssetName))
            {
                return;
            }

            // 当ABPath与AssetName为非空时，则说明是通过【克隆】方式进行的实例化，并非是通过【动态加载】方式进行的实例化，所以这里需要特殊处理：添加引用计数
            // 注：【克隆】方式是指通过 GameObject.Instantiate(go); 进行的实例化行为！
            //     当go是一个不被各类Manager管控的对象时，可以使用【克隆】方式，其他情况禁止使用【克隆】方式，因为克隆得到的对象将不受各类Manager的管控！
            //     比如：UI对象，只能通过UIManager进行实例化，【克隆】方式的实例化对象并不在UIManager的管理容器内！
            InstanceID = gameObject.GetInstanceID();
            Root.Asset.PrefabLoadManager.AddAssetRef(ABPath, AssetName, gameObject);
        }

        void OnDestroy()
        {
            // 被动销毁，保证引用计数正确
            Root.Asset.PrefabLoadManager.Destroy(gameObject, RightNowDestroyOnAsset);
        }
    }

}


