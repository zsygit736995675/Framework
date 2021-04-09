using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 动作枚举
/// </summary>
public enum RoleAction
{
    none,
    idle,
    walk,
    // run,
    attack,
    JumpAttack,
    //击倒
    crouch,
    hit,
    //击飞
    hitfly,
    jingangzhou,
    skill1,
    //眩晕
    vertigo,
    turn,
    hold1,
    hold2,
    hold3,
}

public class SpineBase : MonoBehaviour
{
    private SkeletonAnimation sgp;

    public RoleAction currentAni = RoleAction.none;

    Dictionary<int, Action> events = null;

    private void Awake()
    {
        sgp = GetComponentInChildren<SkeletonAnimation>();
    }

    public float AniSpeed
    {
        get
        {
            return sgp.AnimationState.TimeScale;
        }
        set
        {
            sgp.AnimationState.TimeScale = value;
        }
    }

    public void SetLayer()
    {

    }

    /// <summary>
    /// true是翻转 false正常
    /// </summary>
    bool flipx;
    public bool FlipX
    {
        set
        {
            flipx = value;
            if (value)
                sgp.transform.localScale = new Vector3(-Mathf.Abs(sgp.transform.localScale.x), sgp.transform.localScale.y, sgp.transform.localScale.z);
            else
                sgp.transform.localScale = new Vector3(Mathf.Abs(sgp.transform.localScale.x), sgp.transform.localScale.y, sgp.transform.localScale.z);
        }
        get
        {
            return flipx;
        }
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    protected void PlayAnim(string animName, bool isloop, System.Action callback = null, Dictionary<int, Action> events = null)
    {
        TrackEntry track = sgp.AnimationState.SetAnimation(0, animName, isloop);

        frame = 0;
        this.events = events;

        if (callback != null)
        {
            track.Complete += (tra) =>
            {
                this.events = null;
                callback?.Invoke();
            };
        }
    }


    int frame;
    /// <summary>
    /// 设置帧起点和终点的播放
    /// </summary>
    protected void PlayAnim(string ani, bool loop, float startFrame, float endFrame, Dictionary<int, Action> events, System.Action callback)
    {
        frame = 0;
        this.events = events;

        TrackEntry track = sgp.AnimationState.SetAnimation(0, ani, loop);
        sgp.timeScale = 0;
        track.AnimationStart = startFrame / 30f;
        track.AnimationEnd = endFrame / 30f;
        sgp.timeScale = 1;

        if (callback != null)
        {
            track.Complete += (tra) =>
            {
                this.events = null;
                callback?.Invoke();
            };
        }
    }

    private void FixedUpdate()
    {
        if (events != null && events.Count > 0)
        {
            frame++;
            Action action;
            if (events.TryGetValue(frame, out action))
            {
                action?.Invoke();
            }
        }
    }

    protected virtual void UpdateMe()
    {
    }

    private void Update()
    {
        UpdateMe();
    }


}
