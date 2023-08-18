using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(LanguageTest))]
public class LanguageTestInspector : Editor
{
    private SerializedProperty m_CurrentCountry = null;

    private List<LocalizationText> m_AllLocalizationText;

    private List<CountryAndCode> m_LangeCountryAndCode;


    private void OnEnable()
    {
        if (EditorApplication.isPlaying)
        {
            return;
        }
        
        m_CurrentCountry = serializedObject.FindProperty("currentCountry");
        m_AllLocalizationText = ((LanguageTest)target).GetComponentsInChildren<LocalizationText>().ToList();

        m_LangeCountryAndCode = new List<CountryAndCode>();
        var languageManager = GameObject.Find("LanguageManager")?.GetComponent<LanguageManager>();
        if (languageManager != null)
        {
            m_LangeCountryAndCode = languageManager.countrys.ToList();
        }

        RefreshText();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUI.changed && !EditorApplication.isPlaying)
        {
            RefreshText();
        }
    }

    void RefreshText()
    {
        serializedObject.Update();

        var langeIndex = m_CurrentCountry.enumValueIndex;
        if (langeIndex >= 0 && langeIndex <= m_LangeCountryAndCode.Count - 1)
        {
            CountryAndCode country = m_LangeCountryAndCode[langeIndex];
            Font font = null;

            // 加载字体资源文件
            if (country.FontAssetName.Equals("Arial"))
            {
                font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            }
            else
            {
                font = (Font)AssetDatabase.LoadAssetAtPath($"Assets/StaticRes/Fonts/{country.FontAssetName}.TTF", typeof(Font));
            }

            m_AllLocalizationText.ForEach(item =>
            {
                item.EditorOnLocalize(font, country);
                item.enabled = true;
            });
        }

        serializedObject.ApplyModifiedProperties();
        Repaint();
    }
}