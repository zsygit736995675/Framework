using UnityEngine;


/// <summary>
/// http服务器
/// </summary>
public enum HttpServerAddress
{
    [InspectorName("测试服")]
    Debug,
    [InspectorName("正式服")]
    Release,
    [InspectorName("内网")]
    Intranet,
}

/// <summary>
/// 配置
/// </summary>
public enum SysType 
{
    [InspectorName("默认")]
    Default =1
}


public class AppData : SingletonGetMono<AppData>
{
    [Header("服务器")]
    public HttpServerAddress serverType =HttpServerAddress.Debug;


    public SysType sysType =SysType.Default;


}
