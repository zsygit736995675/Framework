using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 角色状态
/// </summary>
public enum RoleStatus
{
    none,
    admission,  // 入场
    idle,       // 空闲
    patrol,     // 巡逻
    attack,     // 攻击
    death,      // 死亡
    transitions,// 过场
}


/// <summary>
/// 状态基类
/// </summary>
public abstract class StateBase 
{
    /// <summary>
    /// 归属
    /// </summary>
    public abstract StateMng role { get; set; }

    /// <summary>
    /// 进入
    /// </summary>
    public abstract void Enter(RoleStatus before, StateModel mode);

    /// <summary>
    /// 更新
    /// </summary>
    public abstract void Update();

    /// <summary>
    /// 退出
    /// </summary>
    public abstract void Exit();
}
