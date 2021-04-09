using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 实体类型
/// </summary>
public enum EntityType
{
    player,//玩家
    obstacle ,//板子
    prop,//道具
    Skill,//技能
    death,//死亡齿轮
}

public class EntityBase : MonoBehaviour
{
    /// <summary>
    /// 类型
    /// </summary>
    public EntityType type;

    /// <summary>
    /// 是否激活
    /// </summary>
    public bool isAction = false;

    /// <summary>
    /// 当前动作
    /// </summary>
    public string currentActionName;

    /// <summary>
    /// 当前动作循环
    /// </summary>
    public int currentLoop;

    /// <summary>
    /// 动画已播放时间
    /// </summary>
    public float playTime = 0;

    /// <summary>
    /// 播放速率
    /// </summary>
    public float timeScale = 1;

    /// <summary>
    /// 龙骨动画
    /// </summary>
    private DragonBones.UnityArmatureComponent animator;
    public DragonBones.UnityArmatureComponent Animator { get { if (animator == null) animator = GetComponentInChildren<DragonBones.UnityArmatureComponent>(); return animator; } }

    /// <summary>
    /// 动作回调
    /// </summary>
    private Dictionary<string, Action> callbackDic = new Dictionary<string, Action>();

    /// <summary>
    /// 动画数量
    /// </summary>
    private int animatorCount;
    public int AnimatorCount { get { if (Animator != null) return Animator.animation.animationNames.Count; else return 0; } }

    /// <summary>
    /// 翻转X
    /// </summary>
    public bool FlipX { set { Animator.armature.flipX = value; } get { return animator.armature.flipX; } }

    /// <summary>
    /// 反转Y
    /// </summary>
    public bool FlipY { set { Animator.armature.flipY = value; } get { return animator.armature.flipY; } }


    /// <summary>
    /// 动作
    /// </summary>
    public virtual void PlayAni(string aniName, int loop = -1, Action action = null)
    {
        if (Animator == null)
        {
            Debug.LogError("PlayAni error: gameobj:" + gameObject.name + "   actionName:" + name);
            return;
        }

        playTime = 0;

        currentActionName = aniName;
        Animator.animation.Play(aniName, loop);

        Action call;
        if (callbackDic.TryGetValue(aniName, out call))
        {
            callbackDic.Remove(aniName);
        }

        if (loop > 0 && action != null)
        {
            callbackDic.Add(aniName, action);
        }

        timeScale = 1;

        Animator.animation.timeScale = timeScale;
    }

    /// <summary>
    /// 动作
    /// </summary>
    public void PlayAniGoToTime(string aniName, float timer, int loop = -1, Action action = null)
    {
        if (Animator == null)
        {
            Debug.LogError("PlayAni error: gameobj:" + gameObject.name + "   actionName:" + name);
            return;
        }

        playTime = 0;

        currentActionName = aniName;
        Animator.animation.GotoAndPlayByTime(aniName, timer, loop);

        Action call;
        if (callbackDic.TryGetValue(aniName, out call))
        {
            callbackDic.Remove(aniName);
        }

        if (loop > 0 && action != null)
        {
            callbackDic.Add(aniName, action);
        }
    }

    /// <summary>
    /// 正向
    /// </summary>
    public void Positive()
    {
        if (Animator != null)
        {
            Animator.animation.timeScale = timeScale;
        }
    }

    /// <summary>
    /// 反向
    /// </summary>
    public void Reverse()
    {
        if (Animator != null)
        {
            Animator.animation.timeScale = -timeScale;
        }
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public void Stop()
    {
        if (Animator != null)
        {
            Animator.animation.Stop();
        }
    }

   
    /// <summary>
    /// 层级设置
    /// </summary>
    public void SetSortLayerName(string layerName)
    {
        if (Animator != null)
        {
            Animator.sortingLayerName = layerName;
        }
    }

    /// <summary>
    /// 通过下标获取动作名称
    /// </summary>
    public string GetAniNameByIndex(int index)
    {
        if (Animator != null)
        {
            if (index < Animator.animation.animationNames.Count)
            {
                return Animator.animation.animationNames[index];
            }
            else
            {
                Debug.LogError("GetAniNameByIndex error index overflow");
                return "";
            }
        }
        else
        {
            Debug.LogError("GetAniNameByIndex error animator null neme: "+gameObject.name +" index: "+index);
            return "";
        }
    }

    /// <summary>
    /// 检测动作
    /// </summary>
    public bool DetectAction(string actionName)
    {
        if (Animator == null)
        {
            return false;
        }
        return Animator.animation.animationNames.Contains(actionName);
    }

    private void Update()
    {
        if (Animator != null && Animator.animation.isPlaying&&!Animator.animation.isCompleted)
        {
            playTime += Time.deltaTime;
        }

        if (Animator != null&&callbackDic!=null&&callbackDic.Count>0)
        {
            if (Animator.animation.isCompleted)
            {
                Action call;
                if (callbackDic.TryGetValue(Animator.animation.lastAnimationName, out call))
                {
                    callbackDic.Remove(Animator.animation.lastAnimationName);
                    if (call != null)
                    {
                        call.Invoke();
                    }
                }
            }
        }

    }

    /// <summary>
    /// 随机获取动作
    /// </summary>
    public string RandomGetAction()
    {
        return Animator.animation.animationNames[UnityEngine. Random.Range(0,AnimatorCount)];
    }





  
}
