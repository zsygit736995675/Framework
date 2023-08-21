using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LanguageManager))]
public class LanguageManagerInspector : Editor
{
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!EditorApplication.isPlaying)
        {
            if(GUILayout.Button("刷新多语言设置"))
            {
                Reset();
            }
        }
        
        if(GUILayout.Button("打开多语言配置表"))
        {
            OpenExcelByName( "LanguageSettingConfig");
        }
        
        if(GUILayout.Button("打开多语言文本表"))
        {
            OpenExcelByName( "StrConfig");
        }
    }

    void OpenExcelByName(string excelName)
    {
        OpenExcel( $"{Application.dataPath}/Framework/Config/Table/{excelName}.xlsx");
    }

    public void OpenExcel(string excelPath)
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
                Process.Start(excelPath);
                break;
            case RuntimePlatform.OSXEditor:
                Process.Start("open", excelPath);
                break;
            default:
                throw new Exception($"Not support open file on '{Application.platform.ToString()}' platform.");
        }
    }
    
    /// <summary>
    /// 通过配置文件，重置多语言设置
    /// </summary>
    void Reset()
    {
        LanguageSettingConfig[] configs = LanguageSettingConfig.GetConfigs();
        if (configs == null)
        {
            return;
        }
        
        List<CountryAndCode> list = new List<CountryAndCode>();
        foreach (var setting in configs)
        {
            if (!setting.IsUse)
            {
                continue;
            }

            CountryAndCode code = new CountryAndCode();
            code.Country = setting.Country;
            code.ShortName = setting.ShortName;
            code.Annotate = setting.Annotate;
            code.Sort = setting.Sort;
            code.StrKey = setting.StrKey;
            code.FontAssetName = setting.FontAssetName;
            list.Add(code);
        }

        list.Sort((a, b) =>
        {
            return a.Sort - b.Sort;
        });
        
        LanguageManager languageManager = ((LanguageManager)target);
        languageManager.countrys = list.ToArray();
        
        EditorUtility.SetDirty(target);
        EditorUtility.ClearDirty(target);
    }
    
}
