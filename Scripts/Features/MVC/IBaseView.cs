using System;
using UnityEngine;

/// <summary>
/// 显示方式动画
/// </summary>
public enum AppearanceStyle 
{
    /// <summary>
    /// 没有动画
    /// </summary>
    None,



}


/// <summary>
/// View基类
/// </summary>
public abstract class IBaseView
{
    /// <summary>
    /// 皮肤
    /// </summary>
    public Transform Skin { get; set; }

    /// <summary>
    /// 父物体
    /// </summary>
    public Transform myParent{ get; set; }

    /// <summary>
    /// 标识
    /// </summary>
    public ViewDef key { get; set; }

    /// <summary>
    /// 层级
    /// </summary>
    public LayoutUI layout { get; set; }

    /// <summary>
    /// 是否显示中
    /// </summary>
    public abstract bool isShow { get;}

    /// <summary>
    /// 是否已经初始化
    /// </summary>
    public bool isInit { get; set; }

    /// <summary>
    /// 是否已经加载
    /// </summary>
    public bool isLoad { get; set; }

    /// <summary>
    /// 是否预加载
    /// </summary>
    public bool IsPreloading { get; set; }

    /// <summary>
    /// 遮罩透明度
    /// </summary>
    public float Degreeofmasking { get; set; }

    /// <summary>
    /// 面板数据初始化
    /// </summary>
    public abstract void initData();

    /// <summary>
    /// 初始化
    /// </summary>
    public abstract void initUI();

    /// <summary>
    /// 加载
    /// </summary>
    public abstract void loadSkin();

    /// <summary>
    /// 面板开启
    /// </summary>
    public abstract void open(params object[] args);

    /// <summary>
    /// 面板关闭
    /// </summary>
    public abstract void close(params object[] args);

    /// <summary>
    /// 关闭自己
    /// </summary>
    public abstract void onClose();

    /// <summary>
    /// 当前层级置顶
    /// </summary>
    public abstract void setViewTop();

    /// <summary>
    /// 设置层级
    /// </summary>
    public abstract void setLayout();

    /// <summary>
    /// 销毁
    /// </summary>
    public abstract void destroy();

    /// <summary>
    /// 设置是否隐藏
    /// </summary>
    public abstract void setVisible(bool value);

}
