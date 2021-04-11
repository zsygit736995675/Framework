using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// ui层级
/// </summary>
public enum LayoutUI
{
    Bg=0,
    Main,
    Popup,
    Message,
    Tips,
    Max
}
         
public class ViewMng : BaseClass<ViewMng> 
{
    /// <summary>
    /// 注册的ui
    /// </summary>
    private Dictionary<ViewDef, IBaseView> _views = new Dictionary<ViewDef, IBaseView>();

    /// <summary>
    /// 开启中的ui
    /// </summary>
    private List<ViewDef> _opens=new List<ViewDef>();

    /// <summary>
    /// 当前ui打开数量
    /// </summary>
    public int currOpenNum{ get { return this._opens.Count; }}


    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        this._views.Clear();
        this._opens.Clear();
        LayoutMng.Ins.LoadCanvas();
    }

    /// <summary>
    /// 面板注册
    /// </summary>
    public void register(ViewDef key, IBaseView view)
    {
        if (view == null || _views.ContainsKey(key))
            return;

        view.initData();
        this._views.Add(key, view);
    }

    /// <summary>
    /// 面板解除注册
    /// </summary>
    public void unregister(ViewDef key)
    {
        if (!this._views.ContainsKey(key))
            return;

        this._views.Remove(key);
    }

    /// <summary>
    /// 清空处理
    /// </summary>
    public void clear()
    {
        this.closeAll();
        this._views.Clear();
    }

    /// <summary>
    /// 销毁一个面板
    /// </summary>
    public void destroy(ViewDef key, IBaseView newView = null)
    {
        IBaseView oldView = this.getView(key);
        if (oldView != null)
        {
            this.unregister(key);
            oldView.destroy();
            oldView = null;
        }
        this.register(key, newView);
    }

    /// <summary>
    /// 开启面板
    /// </summary>
    public IBaseView open(ViewDef key, params object[] param)
    {
        IBaseView view = this.getView(key);
        if (view == null)
            return null;

        if (view.isShow)
        {
            view.open(view, param);
            view.setViewTop();
            return view;
        }

        if (view.isInit)
        {
            view.open(view, param);
        }
        else
        {
            this.setViewOpen(view, param, key);
        }

        this._opens.Add(key);
        view.setViewTop();
        return view;
    }

    private void setViewOpen(IBaseView view, params object[] param)
    {
        if (!view.isInit)
            view.initUI();
        view.initData();
        view.setVisible(true);
        view.open(param);
    }



    /**
        * 置顶界面（界面在打开中才可以置顶）
    */
    public void topView(ViewDef key)
    {
        IBaseView view = this.getView(key);
        if (view == null)
        {
            Debug.Log("UI_" + key + "不存在");
            return;
        }

        if (!view.isShow)
        {
            Debug.Log("请先打开" + "UI_" + key);
            return;
        }

        bool boolean = view is BaseEuiView;
        if (boolean)
        {
            //int maxDepth = this._viewParent.transform.childCount;
            //this._viewParent.setChildIndex(view as BaseEuiView, maxDepth);
        }
        else
        {
            Debug.Log("view type error");
        }
    }

    /// <summary>
    ///  关闭面板
    /// </summary>
    public void close(ViewDef key, params object[] param)
    {
        if (!this.isShow(key)) return;

        IBaseView view = this.getView(key);
        if (view == null) return;

        int viewIndex = this._opens.IndexOf(key);
        this._opens.RemoveRange(viewIndex, 1);

        view.close(param);
    }

    /// <summary>
    ///  获取指定UI对象
    /// </summary>
    public IBaseView getView(ViewDef key)
    {
        IBaseView view = null;
        this._views.TryGetValue(key, out view);
        return view;
    }

    /// <summary>
    /// 关闭所有界面
    /// </summary>
    public void closeAll()
    {
        while (this._opens.Count > 0)
        {
            this.close(this._opens[0]);
        }
    }

   
    /// <summary>
    /// 检测一个UI是否开启中
    /// </summary>
    public bool isShow(ViewDef key)
    {
        return this._opens.IndexOf(key) != -1;
    }

    /**
        * 加载登陆预加载
    */
    public void onLoadingPreloadUI(Action callback)
    {
        IBaseView view = this.getView(ViewDef.Loading);
        if (view == null)
        {
         
            return;
        }
        else
        {
          
        }
    }

} 
