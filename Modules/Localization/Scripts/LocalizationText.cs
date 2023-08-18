using System.Collections.Generic;
using ArabicTool;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Custom Text Control used for localization text.
/// </summary>
[AddComponentMenu("UI/LocalizationText", 10)]
public class LocalizationText : Text
{
    /// <summary>
    /// 文本的key
    /// </summary>
    public int Key;

    /// <summary>
    /// 自定义字体，方便后期替换
    /// </summary>
    public UIFont CustomFont;

    /// <summary>
    /// 是否开启自身的本地化
    /// </summary>
    public bool IsOpenLocalize = true;
    
    /// <summary>
    /// 字体是否开启本地化
    /// </summary>
    public bool IsFontOpenLocalize = true;

    /// <summary>
    /// 参数
    /// </summary>
    object[] param;


    protected override void Awake()
    {
        base.Awake();
        if (CustomFont != null)
        {
            font = CustomFont.UseFont;
        }

        OnLocalize();

        if (Application.isPlaying && IsOpenLocalize)
        {
            LanguageManager.Instance.OnLocalize += OnLocalize;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (Application.isPlaying && IsOpenLocalize && LanguageManager.Instance.OnLocalize != null)
        {
            LanguageManager.Instance.OnLocalize -= OnLocalize;
        }
    }

    /// <summary>
    /// 重新本地化，用于游戏内切换语言时调用
    /// </summary>
    public void OnLocalize()
    {
        if (IsOpenLocalize)
        {
            if (LanguageManager.Instance != null)
            {
                font = LanguageManager.Instance.CurrentFont;
                string str = LanguageManager.GetStr(Key, param);
                if (ArabicTextTool.HasArabicChar(str))
                {
                    DoSetArabText(str);
                }
                else
                {
                    if (this.horizontalOverflow == HorizontalWrapMode.Overflow)
                    {
                        this.horizontalOverflow = HorizontalWrapMode.Wrap;
                    }
                    text = str;
                }
            }
        }
    }
    
    private void DoSetArabText(string orgStr, float width = 0.0f, float height = 0.0f)
    {
        string str = ArabicTextTool.Convert(orgStr);
        this.horizontalOverflow = HorizontalWrapMode.Wrap;
        VerticalWrapMode verticalOverflow = this.verticalOverflow;
        this.verticalOverflow = VerticalWrapMode.Overflow;
        TextGenerator textGenerator = new TextGenerator(str.Length);
        Vector2 extents = (double) width <= 0.0 || (double) height <= 0.0 ? this.GetComponent<RectTransform>().rect.size : new Vector2(width, height);
        if ((double) extents.x == 0 && (double) extents.y == 0)
        {
            Debug.LogError("[ArabicTool]Text rect is zero.");
        }
      
        if ((double) extents.x == 0)
        {
            extents = new Vector2(Screen.width, extents.y);
        }
        textGenerator.Populate(str, this.GetGenerationSettings(extents));
        UILineInfo[] linesArray = textGenerator.GetLinesArray();
        List<int> lineinfo = new List<int>();
        for (int index = 0; index < linesArray.Length; ++index)
        {
            int startCharIdx = linesArray[index].startCharIdx;
            int num = -1;
            if (index + 1 < linesArray.Length)
                num = linesArray[index + 1].startCharIdx - startCharIdx;
            if (num < 0)
                num = str.Length - startCharIdx;
            lineinfo.Add(num);
        }
        this.horizontalOverflow = HorizontalWrapMode.Overflow;
        this.verticalOverflow = verticalOverflow;
        text = ArabicTextTool.Reverse(str, ref lineinfo, true);
    }


    /// <summary>
    /// 编辑器模式修改
    /// </summary>
    public void EditorOnLocalize(Font loadFont, CountryAndCode language)
    {
        if (IsOpenLocalize)
        {
            font = loadFont;
            string str = LanguageManager.GetStr(Key,language, param);
            if (ArabicTextTool.HasArabicChar(str))
            {
                DoSetArabText(str);
            }
            else
            {
                if (this.horizontalOverflow == HorizontalWrapMode.Overflow)
                {
                    this.horizontalOverflow = HorizontalWrapMode.Wrap;
                }
                text = str;
            }
        }
    }

    public void SetText(int key, params object[] param)
    {
        this.Key = key;
        this.param = param;
        OnLocalize();
    }

    public void SetText(params object[] param)
    {
        this.param = param;
        OnLocalize();
    }

    public void SetText(string str)
    {
        text = str;
    }
}