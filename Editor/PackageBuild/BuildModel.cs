using UnityEditor;
using UnityEngine;

public struct BuildModel
{
    public string version;
    public string versionCode;
    public HttpServerAddress serverType;
    public SysType sysType ;
    public AndroidArchitecture arc;

    public void ReSet()
    {
        AppData main = GameObject.Find("Main").GetComponent<AppData>();
        version = PlayerSettings.bundleVersion;
        versionCode = PlayerSettings.Android.bundleVersionCode.ToString();
        arc = PlayerSettings.Android.targetArchitectures;
        serverType = main.serverType;
        sysType = main.sysType;
    }


}