using UnityEngine;
using UnityEngine.UI;

namespace SY_FrameWork
{
    /// <summary>
    /// Custom Text Control used for localization text.
    /// </summary>
    [AddComponentMenu("UI/LanguageText", 10)]
    public class LanguageText : MonoBehaviour
    {
        /// <summary>
        /// 文本的key
        /// </summary>
        public int Key;

        /// <summary>
        /// 是否开启自身的本地化
        /// </summary>
        public bool IsOpenLocalize = true;

        /// <summary>
        /// 参数
        /// </summary>
        object[] param;

        private Text text;

        
        protected  void Awake()
        {
            text = GetComponent<Text>();
            OnLocalize();
        }

        protected void OnEnable()
        {
            if (IsOpenLocalize)
            {
                LanguageManager.OnLocalize += OnLocalize;
            }
        }

        protected  void OnDisable()
        {
            if (IsOpenLocalize)
            {
                if (LanguageManager.OnLocalize != null)
                {
                    LanguageManager.OnLocalize -= OnLocalize;
                }
            }
        }

        /// <summary>
        /// 重新本地化，用于游戏内切换语言时调用
        /// </summary>
        public void OnLocalize()
        {
            if (text == null) return;
            
            if (IsOpenLocalize)
            {
                if (param == null || param.Length == 0)
                {
                    text.text = LanguageManager.GetStr(Key);
                    return;
                }

                text.text = LanguageManager.GetStr(Key, param);
            }
        }

        public void SetText(int key, params object[] param)
        {
            this.Key = key;
            this.param = param;
            OnLocalize();
        }

        public void SetText(object[] param)
        {
            this.param = param;
            OnLocalize();
        }
    }
}