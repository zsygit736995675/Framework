using System;
using UnityEngine;

    public class Clock : SingletonObject<Clock>
    {
        private bool UseUnityTime = true;
        private long beginTime = 0;
        private long frameTime = 0;
        private long lastFrameTime = 0;

        public void Init()
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

        public void Shutdown() { }

        public long FrameTime
        {
            get { return frameTime; }
        }

        public float FrameTimeSec
        {
            get
            {
                return frameTime / 1000.0f;
            }
        }

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
                    return (long)(Time.deltaTime * 1000.0f);
                }
            }
        }

        public long UnityDeltaTime
        {
            get
            {
                return (long)(Time.deltaTime * 1000.0f);
            }
        }

        public float DeltaTimeSec
        {
            get
            {
                if (UseUnityTime)
                {
                    return Time.deltaTime;
                }
                return this.DeltaTime / 1000.0f;
            }
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


        public long TickToMilliSec(long tick)
        {
            return tick / (10 * 1000);
        }
    }
