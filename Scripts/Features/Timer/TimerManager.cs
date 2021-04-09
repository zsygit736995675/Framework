using System;
using System.Collections.Generic;

/// <summary>
/// 计时器管理类
/// </summary>
public class TimerManager : SingletonObject<TimerManager>
{
    private List<LogicTimer> mListTimer = new List<LogicTimer>();

    private LogicTimer mCurTimer = null;

    private int mTimerID = (int)TimerID.TIMER_MAX;

    private LogicTimer CurTimer()
    {
        return mCurTimer;
    }

    public void Init()
    {

    }

    public int GetCount()
    {
        return mListTimer.Count;
    }

    public void Update()
    {
        if (mListTimer.Count < 1)
        {
            return;
        }
        long nCurTime = Clock.Ins.FrameTime;
        for (int i = mListTimer.Count - 1; i >= 0; --i)
        {
            LogicTimer timer = mListTimer[i];
            if (!timer.Valid)
            {
                mListTimer.RemoveAt(i);
            }
        }

        int nCount = mListTimer.Count;
        for (int i = 0; i < nCount; ++i)
        {
            LogicTimer timer = mListTimer[i];
            if (!timer.Valid)
            {
                continue;
            }

            long nElapse = nCurTime - timer.BeginTime;
            if (nElapse > timer.Elapse)
            {
                //timer.IsEndElapse = true;

                timer.BeginTime = nCurTime;

                mCurTimer = timer;

                try
                {
                    if (timer.Handler != null)
                    {
                        timer.Handler();
                    }
                }
                catch (System.Exception se)
                {
                    //FireEngine.Logger.GetFile(FireEngine.LogFile.Global).LogException(se);
                }

                mCurTimer = null;
            }
        }
    }

    // use TimerMgrHelper instead
    public void SetTimer(TimerID nEvent, int nElapse, _OnTimer handler)
    {
        LogicTimer _timer = this.GetTimer((int)nEvent);
        if (_timer != null)
        {
            //FireEngine.Logger.GetFile ( FireEngine.LogFile.Global ).LogError ( nEvent.ToString () + " is exist . " );
            return;
        }


        LogicTimer timer = new LogicTimer();
        timer.EventID = (int)nEvent;
        timer.Elapse = nElapse;
        timer.Handler = handler;
        timer.BeginTime = Clock.Ins.FrameTime;

        mListTimer.Add(timer);
    }

    // use TimerMgrHelper instead
    public LogicTimer SetTimer(int nElapse, _OnTimer handler)
    {
        ++mTimerID;
        LogicTimer timer = new LogicTimer();
        timer.EventID = mTimerID;
        timer.Elapse = nElapse;
        timer.Handler = handler;
        timer.BeginTime = Clock.Ins.FrameTime;

        mListTimer.Add(timer);

        return timer;
    }

/// <summary>
/// 只执行一次
/// </summary>
    public LogicTimer SetOnceTimer(int nElapse, Action callback)
    {
        ++mTimerID;
        LogicTimer timer = new LogicTimer();
        timer.EventID = mTimerID;
        timer.Elapse = nElapse;
        timer.Handler = ()=>{
            this.KillCurTimer();
            if (callback != null) callback();
            };
        timer.BeginTime = Clock.Ins.FrameTime;

        mListTimer.Add(timer);

        return timer;
    }

    // use TimerMgrHelper instead
    public void KillCurTimer()
    {
        if (mCurTimer == null)
        {
            return;
        }

        this.KillTimer((TimerID)mCurTimer.EventID);
    }

    // use TimerMgrHelper instead
    public void KillTimer(TimerID nEvent)
    {
        for (int i = 0; i < mListTimer.Count; ++i)
        {
            if (mListTimer[i].EventID == (int)nEvent)
            {
                mListTimer[i].Valid = false;
                return;
            }
        }
    }

    // use TimerMgrHelper instead
    public LogicTimer GetTimer(int nEvent)
    {
        for (int i = 0; i < mListTimer.Count; ++i)
        {
            LogicTimer timer = mListTimer[i];
            if (timer.EventID == nEvent && timer.Valid)
            {
                return timer;
            }
        }

        return null;
    }
    // use TimerMgrHelper instead
    private LogicTimer ModifyTimerElapse(int nEvent, int elapse)
    {
        for (int i = 0; i < mListTimer.Count; ++i)
        {
            LogicTimer timer = mListTimer[i];
            if (timer.EventID == nEvent && timer.Valid)
            {
                timer.Elapse = elapse;

                return timer;
            }
        }

        return null;
    }

    // use TimerMgrHelper instead
    private LogicTimer ResetBeginTime(int nEvent)
    {
        for (int i = 0; i < mListTimer.Count; ++i)
        {
            LogicTimer timer = mListTimer[i];
            if (timer.EventID == nEvent && timer.Valid)
            {
                timer.BeginTime = Clock.Ins.FrameTime;

                return timer;
            }
        }

        return null;
    }

    public void Reset()
    {
        mListTimer.Clear();
    }

}
