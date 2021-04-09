using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 空闲状态
/// </summary>
public class IdleState : StateBase
{
    public override StateMng role { get; set; }

    public IdleState(StateMng role)
    {
        this.role = role;
    }

    public override void Enter(RoleStatus before,StateModel param)
    {
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
    }

}
