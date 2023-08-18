using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 巡逻
/// </summary>
public class PatrolState : StateBase
{
    public override StateMng role { get; set; }

    public PatrolState(StateMng role)
    {
        this.role = role;
    }

    public override void Enter(RoleStatus before, StateModel param)
    {

    }

    public override void Exit()
    {
    }


    public override void Update()
    {
       
    }

    /// <summary>
    /// 寻敌
    /// </summary>
    void Blame() 
    {
        
    }

   


}
