using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 层级管理
/// </summary>
public class LayoutMng :BaseClass<LayoutMng>
{
    /// <summary>
    /// 层级
    /// </summary>
    private Dictionary<LayoutUI, Canvas> layDic = new Dictionary<LayoutUI, Canvas>();

    /// <summary>
    /// 初始化层级面板
    /// </summary>
    public void LoadCanvas()
    {
        GameObject go = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UI/CanvasMain"));
        Canvas can = go.GetComponent<Canvas>();
        can.pixelPerfect = true;
        can.worldCamera = Camera.main;
        can.name = "Canvas_Main";

        for (int i = 0; i < (int)LayoutUI.Max; i++)
        {
            Canvas vas = null;
            if (layDic.TryGetValue((LayoutUI)i, out vas)) break;

            vas = new GameObject().AddComponent<Canvas>();
            vas.transform.SetParent(go.transform);
            vas.localPosition(Vector3.zero).localScale(1);
            vas.name = "Layout_" + ((LayoutUI)i).ToString();
            vas.overrideSorting = true;
            vas.sortingOrder = i * 10;
            vas.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            layDic.Add((LayoutUI)i, vas);
        }
    }

    /// <summary>
    /// 获取指定层级
    /// </summary>
    public Canvas GetLayout(LayoutUI lay)
    {
        Canvas vas;
        if (layDic.TryGetValue(lay, out vas))
            return vas;
        else
            return null;
    }

}
