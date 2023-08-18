using UnityEngine;
using System.Collections;

namespace Solar.Runtime
{
    public class FPS : MonoBehaviour
    {
        public float updateInterval = 0.5F;

        private float accum = 0; // FPS accumulated over the interval
        private int frames = 0; // Frames drawn over the interval
        private float timeleft; // Left time for current interval

        private UnityEngine.UI.Text uitext;

        void Start()
        {
            uitext = GetComponent<UnityEngine.UI.Text>();
            timeleft = updateInterval;
        }

        void Update()
        {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;

            if (timeleft <= 0.0)
            {
                float fps = accum / frames;
                string format = System.String.Format("{0:F2} FPS", fps);
                uitext.text = format;

                if (fps < 10)
                {
                    uitext.color = Color.red;
                }
                else if (fps < 30)
                {
                    uitext.color = Color.yellow;
                }
                else
                {
                    uitext.color = Color.green;
                }

                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
            }
        }
    }
}