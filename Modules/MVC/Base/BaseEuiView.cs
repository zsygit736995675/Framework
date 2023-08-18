using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;


public class BaseEuiView : IBaseView
{
    public override bool isShow { get { if (Skin == null) return false;  return Skin.gameObject.activeSelf; } }

    public override void initData()
    {
        this.isInit = false;
        this.isLoad = false;
    }

    public override void initUI()
    {
        isInit = true;
    }

    public override void open(params object[] args)
    {
        if (Skin == null)
            loadSkin();

        setVisible(true);
    }

    public override void close(params object[] args)
    {
        setVisible(false);
    }

    public override void setViewTop()
    {
        Transform parent = this.Skin.parent;
        if (parent != null)
        {
            this.Skin.SetAsLastSibling();
        }
    }

    public override void destroy()
    {
        if(Skin!=null)
            GameObject.Destroy(this.Skin);
    }

    public override void setVisible(bool value)
    {
        if (Skin == null) return;

        if (Skin.gameObject.activeSelf != value)
            Skin.gameObject.SetActive(value);
    }

    public override void onClose()
    {
        ViewMng.Ins.close(key);
    }

    public override void loadSkin()
    {
        GameObject gameObject = Resources.Load<GameObject>("UI/" + key.ToString());
        if (gameObject == null)
        {
            Debug.LogError("LoadUIByLocal error: id:" + key + " res:" + key.ToString());
            return;
        }
        GameObject obj = GameObject.Instantiate(gameObject);
        this.Skin = obj.transform;
        this.isLoad = true;
        this.setVisible(false);

        setLayout();
        initUI();
    }

    public override void setLayout()
    {
        if (Skin == null) return;

        myParent = LayoutMng.Ins.GetLayout(layout).transform;
        Skin.transform.SetParent(myParent.transform);
        Skin.GetComponent<RectTransform>().offsetMax = Vector3.zero;
        Skin.GetComponent<RectTransform>().offsetMin = Vector3.zero;
        Skin.transform.localScale = Vector3.one;
        Skin.transform.localPosition = Vector3.zero;
    }

}
