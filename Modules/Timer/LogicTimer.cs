
public delegate void _OnTimerEnd();

public delegate void _OnTimerOnceEnd(int bouts);

public class LogicTimer
{
    /// <summary>
    /// 标识
    /// </summary>
    public TimerDef EventID{set;get;}

    /// <summary>
    /// 间隔时间
    /// </summary>
    public long Elapse { set; get; }

    /// <summary>
    /// 开启时间
    /// </summary>
    public long BeginTime { set; get; }

    /// <summary>
    /// 要执行的次数
    /// </summary>
    public int RequiredTimes { set; get; }

    /// <summary>
    /// 执行的次数
    /// </summary>
    public int bouts { get; set; }

    /// <summary>
    /// 结束回调
    /// </summary>
    public _OnTimerEnd EndHandler { set; get; }

    /// <summary>
    /// 单次结束回调
    /// </summary>
    public _OnTimerOnceEnd OnceHander { set; get; }

    /// <summary>
    /// 销毁
    /// </summary>
    public void KillSelf(bool isExecute = false) 
    {
        TimerManager.Ins.KillTimer(EventID,isExecute);
    }
}


