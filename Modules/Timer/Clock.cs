using System;
using UnityEngine;


/// <summary>
/// 计时器，初始化后开始计时
/// </summary>
public class Clock : MonoBehaviour
{
    private bool UseUnityTime = true;
    private long beginTime = 0;
    private long frameTime = 0;
    private long lastFrameTime = 0;


    private void Start()
    {
        if (!UseUnityTime)
        {
            beginTime = TickToMilliSec(System.DateTime.Now.Ticks);
            frameTime = TickToMilliSec(System.DateTime.Now.Ticks) - beginTime;
        }
        else
        {
            beginTime = (long)(Time.time * 1000.0f);
            frameTime = (long)(Time.time * 1000.0f) - beginTime;
        }
        lastFrameTime = frameTime - 30;
    }

    public void Update()
    {
        lastFrameTime = frameTime;
        if (!UseUnityTime)
        {
            frameTime = TickToMilliSec(System.DateTime.Now.Ticks) - beginTime;
        }
        else
        {
            frameTime = (long)(Time.time * 1000.0f) - beginTime;
        }
    }

    /// <summary>
    /// 计时（毫秒）
    /// </summary>
    public long FrameTime { get { return frameTime; } }

    /// <summary>
    /// 计时（秒）
    /// </summary>
    public float FrameTimeSec { get { return frameTime / 1000.0f; } }

    /// <summary>
    /// 帧间隔（毫秒）
    /// </summary>
    public long DeltaTime
    {
        get
        {
            if (!UseUnityTime)
            {
                return Math.Max(frameTime - lastFrameTime, 0);
            }
            else
            {
                return UnityDeltaTime;
            }
        }
    }

    /// <summary>
    /// unity帧间隔(毫秒)
    /// </summary>
    public long UnityDeltaTime { get { return (long)(Time.deltaTime * 1000.0f); } }

    /// <summary>
    /// 帧间隔（秒）
    /// </summary>
    public float DeltaTimeSec { get { return this.DeltaTime / 1000.0f; } }

    /// <summary>
    /// tick转毫秒
    /// </summary>
    public long TickToMilliSec(long tick) { return tick / (10 * 1000); }
}
