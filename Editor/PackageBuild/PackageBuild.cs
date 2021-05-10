using System;
using System.IO;
using UnityEditor;
using UnityEngine;


public class PackageBuild : EditorWindow
{
    [MenuItem("ZTools / 打包")]  //添加菜单选项
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindowWithRect(typeof(PackageBuild), new Rect(Screen.width / 3, Screen.height / 3, 500, 180), true, "打包界面");
        window.Show();
    }

    BuildModel model = new BuildModel();

    public SysConfig Config { get { return SysConfig.Get((int)model.sysType); } }

    private void OnEnable()
    {
        model.ReSet();
    }

    private void OnDestroy()
    {
      //  Setting();
    }

    private void OnGUI()
    {
        model.version = EditorGUILayout.TextField("版本号", model.version);
        model.versionCode = model.version.Replace(".", "");
        GUILayout.Label("版本Code                                 " + model.versionCode);
        model.serverType = (HttpServerAddress)EditorGUILayout.EnumPopup("服务器选择", model.serverType);
        model.sysType = (SysType)EditorGUILayout.EnumPopup("打包类型", model.sysType);
        model.arc = (AndroidArchitecture)EditorGUILayout.EnumPopup("cpu选项", model.arc);

        if (GUILayout.Button("只设置"))
        {
            Setting();
        }

        if (GUILayout.Button("打包"))
        {
            Setting();
            Package();
        }
    }

    void Package()
    {
        string APKName = Config.appName + "_" + (model.serverType + "_") + DateTime.Now.ToString("MMdd_HHmm") + "_" + model.sysType + "_" + PlayerSettings.bundleVersion + "" + ".apk";
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, APKName, BuildTarget.Android, BuildOptions.CompressWithLz4);
        Application.OpenURL(Application.dataPath.Replace("Assets", ""));
    }

    void Setting()
    {
        //基础设置
        PlayerSettings.bundleVersion = model.version;
        PlayerSettings.Android.bundleVersionCode = int.Parse(model.versionCode);
        PlayerSettings.Android.targetArchitectures = model.arc;
        PlayerSettings.applicationIdentifier = Config.packageName;

        //gradle设置需要模板
        TextAsset gradleStr = Resources.Load("File/gradleStr") as TextAsset;
        string str = gradleStr.text
        .Replace("@toponAppId", Config.ToponAPPID)
        .Replace("@toponAppkey", Config.ToponAPPKEY)
        .Replace("@umkey", Config.um_key)
        .Replace("@splash_id", Config.openID)
        .Replace("@wx_id", Config.wxAPPID)
        .Replace("@csj_app_id", Config.csj_id)
        .Replace("@csj_slot_id", Config.csj_slot_id)
        .Replace("@source_id", Config.source_id)
        .Replace("@package_name", Config.packageName)
        .Replace("@aliyun_key", Config.aliyun_key);
        File.WriteAllText(UnityEngine.Application.dataPath + "/Plugins/Android/mainTemplate.gradle", str);

        //manifest文件替换
        TextAsset manifest = Resources.Load("File/manifest") as TextAsset;
        File.WriteAllText(UnityEngine.Application.dataPath + "/Plugins/Android/AndroidManifest.xml", manifest.text.Replace("@pkg", Config.packageName));

        //设置签名
        PlayerSettings.Android.useCustomKeystore = true;
        string keyPass = "catchpet";
        PlayerSettings.Android.keystoreName = keyPass + ".keystore"; 
        PlayerSettings.Android.keyaliasName =  "cat";
        PlayerSettings.Android.keystorePass = keyPass;
        PlayerSettings.Android.keyaliasPass = keyPass;
    }
}


