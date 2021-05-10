using UnityEngine;


/// <summary>
/// http服务器
/// </summary>
public enum HttpServerAddress
{
    /// <summary>
    /// 正式
    /// </summary>
    Release,
    /// <summary>
    /// 内网
    /// </summary>
    Intranet,
    /// <summary>
    /// 测试
    /// </summary>
    Debug,
}

/// <summary>
/// 配置
/// </summary>
public enum SysType 
{
    /// <summary>
    /// 默认
    /// </summary>
    Default=1
}


public class AppData : SingletonGetMono<AppData>
{
    
    public HttpServerAddress serverType =HttpServerAddress.Debug;


    public SysType sysType =SysType.Default;


}
