using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ClientCore;
using UnityEngine.SceneManagement;


/// <summary>
/// 项目主控
/// </summary>
public class APP : SingletonObject<APP>
{
    /// <summary>
    /// 游戏入口
    /// </summary>
    private Main main;
    public Main Main { get { return main; } }

    /// <summary>
    /// 加载
    /// </summary>
    public GameObject loadingGO;


    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {

    }

    public void Init(Main main) 
    {
        this.main = main;
        ViewMng.Ins.Init();
        Application.targetFrameRate = 60;
    }

    /// <summary>
    /// 退出
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }



    public void Update()
    {
      
    }

    public void Restart()
    {
      
    }
}