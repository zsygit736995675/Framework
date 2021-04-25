using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void ReleaseCallback(int totalCount, int overCount);

/// <summary>
/// 计数器
/// </summary>
public class Counter
{

    /// <summary>
    /// 标识
    /// </summary>
    public CounterDef Def { get; set; }

    /// <summary>
    /// 计数
    /// </summary>
    public int RefCount { get; private set; }

    /// <summary>
    /// 计数总数
    /// </summary>
    public int RefCountTotal { get; private set; }

    /// <summary>
    /// 计数器归零时的回调
    /// </summary>
    public Action ZeroCallback { get; set; }

    /// <summary>
    /// 计数器刷新的回调
    /// </summary>
    public ReleaseCallback releaseCallback { get; set; }

    /// <summary>
    /// 重置
    /// </summary>
    public void ReSet()
    {
        RefCount = 0;
        RefCountTotal = 0;
    }

    /// <summary>
    /// 设置数量
    /// </summary>
    public void SetCount(int count)
    {
        RefCount = count;
        RefCountTotal = count;
    }

    /// <summary>
    /// 添加计数
    /// </summary>
    public void Retain(object refOwner = null)
    {
        RefCount++;
        RefCountTotal++;
    }

    /// <summary>
    /// 释放一个计数
    /// </summary>
    public void Release(object refOwner = null)
    {
        RefCount--;
        releaseCallback?.Invoke(RefCountTotal, RefCount);
        OnZeroRef();
    }

    void OnZeroRef()
    {
        if (RefCount == 0)
        {
            ZeroCallback?.Invoke();
            CounterManager.Ins.KillCounter(Def);
        }
    }
}