/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetComponent.Methods.cs
 * author:    taoye
 * created:   2020/12/16
 * descrip:   Asset组件-私有方法
 ***************************************************************/
using UnityEngine;

namespace Solar.Runtime
{
    public delegate void PrefabLoadOverCallback(PrefabLoadManager.PrefabObject prefabObject, GameObject gameObject);
    public delegate void AssetLoadOverCallback(AssetLoadManager.AssetObject assetObject, Object asset);
    public delegate void AssetUnloadOverCallback(AssetLoadManager.AssetObject assetObject);
    public delegate void AssetBundleLoadOverCallBack(ABLoadManager.ABObject abObject, AssetBundle ab);

    public sealed partial class AssetComponent : MonoBehaviour
    {

    }

}


