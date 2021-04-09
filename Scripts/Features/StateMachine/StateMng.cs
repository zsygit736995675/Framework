using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMng : MonoBehaviour
{
    RoleStatus status;

    StateBase currentState;

    Dictionary<RoleStatus, StateBase> stateDic = new Dictionary<RoleStatus, StateBase>();

    bool isEnter = false;

    public  virtual void RegistetAll() 
    {
        RegistStatus(RoleStatus.admission, new IdleState(this));
        RegistStatus(RoleStatus.patrol, new PatrolState(this));
    }

    /// <summary>
    /// 状态切换
    /// </summary>
    /// <param name="status"></param>
    public virtual void ChangeStatus(RoleStatus status, StateModel param)
    {
        RoleStatus before = this.status;
        this.status = status;
        currentState?.Exit();
        isEnter = false;

        if (stateDic.TryGetValue(status, out currentState))
        {
            currentState?.Enter(before, param);
            isEnter = false;
        }
    }

    /// <summary>
    /// 注册状态
    /// </summary>
    public void RegistStatus(RoleStatus status, StateBase stateC)
    {
        StateBase temp = null;
        if (stateDic.TryGetValue(status, out temp))
        {
            stateDic[status] = stateC;
        }
        else
        {
            stateDic.Add(status, stateC);
        }
    }

    void Update()
    {
        if (isEnter)
            currentState?.Update();
    }
}
