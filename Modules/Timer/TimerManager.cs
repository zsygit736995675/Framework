using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 计时器管理类
/// </summary>
public class TimerManager : SingletonObject<TimerManager>
{
    /// <summary>
    /// 列表
    /// </summary>
    private List<LogicTimer> mListTimer = new List<LogicTimer>();

    /// <summary>
    /// 不设置id时自增,起点为TIMER_MAX
    /// </summary>
    private TimerDef mTimerID = TimerDef.TIMER_MAX;

    /// <summary>
    /// 计时器
    /// </summary>
    private Clock clock;

    /// <summary>
    /// 事件数量
    /// </summary>
    public int Count { get { return mListTimer.Count; } }

    /// <summary>
    /// 对象池
    /// </summary>
    private MemoryPool<LogicTimer> memoryPool = new MemoryPool<LogicTimer>();

    /// <summary>
    /// 初始化
    /// </summary>
    protected override void Spawn()
    {
        base.Spawn();

        clock = gameObject.AddComponent<Clock>();
    }

    public void Update()
    {
        if (mListTimer.Count < 1) return;

        long nCurTime = clock.FrameTime;

        for (int i = 0; i < Count; ++i)
        {
            LogicTimer timer = mListTimer[i];
            long nElapse = nCurTime - timer.BeginTime;
            if (nElapse >= timer.Elapse)
            {
                timer.BeginTime = nCurTime;
                timer.bouts++;

                try
                {
                    if (timer.RequiredTimes == -1 || timer.bouts < timer.RequiredTimes)
                    {
                        timer.OnceHander?.Invoke(timer.bouts);
                    }
                    else
                    {
                        timer.EndHandler?.Invoke();
                        KillTimer(timer.EventID);
                    }
                }
                catch (System.Exception se)
                {
                    Debug.LogError(se);
                }
            }
        }
    }

    /// <summary>
    /// 设置单次计时器
    /// </summary>
    /// <param name="nEvent">标识</param>
    /// <param name="nElapse">间隔</param>
    /// <param name="handler">结束回调</param>
    /// <returns></returns>
    public LogicTimer SetOnceTimer(TimerDef nEvent, int nElapse, _OnTimerEnd handler)
    {
        LogicTimer _timer = this.GetTimer(nEvent);
        if (_timer != null)
        {
            Debug.LogError("SetTimer==>>" + nEvent.ToString() + " is exist");
            return null;
        }

        LogicTimer timer = CreateTimer();
        timer.EventID = nEvent;
        timer.Elapse = nElapse;
        timer.BeginTime = clock.FrameTime;
        timer.bouts = 0;
        timer.RequiredTimes = 1;
        timer.OnceHander = null;
        timer.EndHandler = handler;

        mListTimer.Add(timer);
        return timer;
    }

    /// <summary>
    /// 单次计时,id自动生成
    /// </summary>
    public LogicTimer SetOnceTimer(int nElapse, _OnTimerEnd handler)
    {
        ++mTimerID;
        return SetOnceTimer(mTimerID, nElapse, handler);
    }

    /// <summary>
    /// 设置一个重复计时器
    /// </summary>
    /// <param name="nEvent">标识</param>
    /// <param name="nElapse">间隔时间</param>
    /// <param name="repeat">重复次数，如果为-1就一直循环</param>
    /// <param name="handler">循环全部结束的回调</param>
    /// <param name="onceHandler">单次循环回调</param>
    /// <returns></returns>
    public LogicTimer SetRepeatTimer(TimerDef nEvent, int nElapse, int repeat, _OnTimerEnd handler,_OnTimerOnceEnd onceHandler=null)
    {
        LogicTimer timer = CreateTimer();
        timer.EventID = nEvent;
        timer.Elapse = nElapse;
        timer.BeginTime = clock.FrameTime;
        timer.bouts = 0;
        timer.RequiredTimes = repeat;
        timer.EndHandler = handler;
        timer.OnceHander = onceHandler;

        mListTimer.Add(timer);
        return timer;
    }

    /// <summary>
    /// 重复计时器，id自动生成
    /// </summary>
    public LogicTimer SetRepeatTimer( int nElapse, int repeat, _OnTimerEnd handler, _OnTimerOnceEnd onceHandler=null)
    {
        ++mTimerID;
        return SetRepeatTimer(mTimerID, nElapse, repeat, handler, onceHandler);
    }

    /// <summary>
    /// 取消指定事件
    /// </summary>
    public void KillTimer(TimerDef nEvent, bool isExecute = false)
    {
        LogicTimer timer = GetTimer(nEvent);
        if (timer != null)
        {
            if (isExecute)
            {
                timer.EndHandler?.Invoke();
            }
            mListTimer.Remove(timer);
            RecycleTimer(timer);
        }
        else
        {
            Debug.LogError("KillTimer==>>" + nEvent.ToString() + " is not exist");
        }
    }

    /// <summary>
    /// 销毁所有
    /// </summary>
    public void KillALl()
    {
        foreach (var item in mListTimer)
        {
            RecycleTimer(item);
        }
        mListTimer.Clear();
    }

    /// <summary>
    /// 获取指定事件
    /// </summary>
    public LogicTimer GetTimer(TimerDef nEvent)
    {
        for (int i = 0; i < mListTimer.Count; ++i)
        {
            LogicTimer timer = mListTimer[i];
            if (timer.EventID == nEvent)
            {
                return timer;
            }
        }
        return null;
    }

    /// <summary>
    /// 修改指定事件
    /// </summary>
    private LogicTimer ModifyTimerElapse(TimerDef nEvent, int elapse)
    {
        for (int i = 0; i < mListTimer.Count; ++i)
        {
            LogicTimer timer = mListTimer[i];
            if (timer.EventID == nEvent)
            {
                timer.Elapse = elapse;
                return timer;
            }
        }
        return null;
    }

    /// <summary>
    /// 创建
    /// </summary>
    LogicTimer CreateTimer()
    {
        LogicTimer lt = memoryPool.Alloc();
        if (lt != null)
        {
            return lt;
        }
        return new LogicTimer();
    }

    /// <summary>
    /// 回收
    /// </summary>
    void RecycleTimer(LogicTimer lt)
    {
        memoryPool.Free(lt);
    }

}
