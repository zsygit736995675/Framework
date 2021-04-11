using UnityEngine;

/// <summary>
/// 事件接收示例，公用的事件接收
/// </summary>
public class GameEvent : EventHandler {

    public GameEvent()
    {
        //注册事件
        RegisterEvent(EventDef.Callback);
    }

    public override void HandleEvent(EventDef ev, LogicEvent le)
    {
        base.HandleEvent(ev, le);
        if (ev == EventDef.Callback)
        {
            Debug.Log("callback+：" + le.StrParam0.ToString());
        }
    }
}
