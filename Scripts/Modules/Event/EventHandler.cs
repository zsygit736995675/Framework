/// <summary>
/// 事件基类
/// </summary>
public class EventHandler
{
      
    public delegate void _HandleEvent(EventDef ev, LogicEvent le);

    private _HandleEvent mhandler = null;

    public void RegisterEvent(EventDef ev)
    {
        EventMgrHelper.Ins.RegisterEventHandler(ev, this);
    }

    public void UnRegisterEvent(EventDef ev)
    {
        EventMgrHelper.Ins.UnRegisterEventHandler(ev, this);
    }

    public virtual void HandleEvent(EventDef ev, LogicEvent le)
    {
        mhandler?.Invoke(ev, le);
    }

    public _HandleEvent Handler
    {
        set { mhandler = value; }
    }
}
