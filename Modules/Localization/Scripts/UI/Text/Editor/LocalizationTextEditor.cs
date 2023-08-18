/*
 * ==============================================================================
 * File Name: LocalizationTextEditor.cs
 * Description: 用来扩展Text，增加显示属性
 * 
 * Version 1.0
 * Create Time: 30/08/2018 12:00
 * 
 * Author: taihe
 * Company: DefaultCompany
 * ==============================================================================
*/

using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(LocalizationText))]
public class LocalizationTextEditor : UnityEditor.UI.TextEditor
{
    public override void OnInspectorGUI()
    {
        LocalizationText component = (LocalizationText) target;
        base.OnInspectorGUI();
        component.Key = int.Parse( EditorGUILayout.TextField("Key String", component.Key.ToString()));
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Is Open Localize", GUILayout.Width(140f));
        component.IsOpenLocalize = EditorGUILayout.Toggle(component.IsOpenLocalize);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Is Open Font Localize", GUILayout.Width(140f));
        component.IsFontOpenLocalize = EditorGUILayout.Toggle(component.IsFontOpenLocalize);
        EditorGUILayout.EndHorizontal();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            EditorUtility.ClearDirty(target);
        }
    }
}