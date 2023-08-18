/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetComponentInspector.cs
 * author:    taoye
 * created:   2020/12/22
 * descrip:   Asset组件编辑器面板定制
 ***************************************************************/
using ExcelDataReader;
using Solar.Runtime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Solar.Editor
{
    [CustomEditor(typeof(AssetComponent))]
    internal sealed class AssetComponentInspector : SolarComponentInspector
    {
        private const int UnloadAssetDelayFrameNumMax = 36000;
        private const int LoadedMaxNumToCleanMemeryMax = 1000;

        private SerializedProperty EditorResourceMode = null;
        private SerializedProperty m_UnloadAssetDelayFrameNum = null;
        private SerializedProperty m_LoadedMaxNumToCleanMemery = null;

        private readonly HashSet<string> m_OpenedItems = new HashSet<string>();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
            {
                EditorResourceMode.boolValue = EditorGUILayout.Toggle("编辑器资源加载模式", EditorResourceMode.boolValue);
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.BeginHorizontal("box");
            {
                if (GUILayout.Button("打开AB配置表Excel"))
                {
                    string filePath = Runtime.Path.AB.GetExcelFileFullPath();
                    OpenExcel(filePath);
                    GUIUtility.ExitGUI();
                }

                if (GUILayout.Button("导出AB配置表Excel到Json&刷新AB名称"))
                {
                    string filePath = Runtime.Path.AB.GetExcelFileFullPath();
                    ExportExcelToJsonFromABConfig(System.IO.Path.GetFileNameWithoutExtension(filePath));
                    AssetBundleNamePostprocessor.RefreshAllAsssetBundleNames();
                    GUIUtility.ExitGUI();
                }

                if (GUILayout.Button("打开AB配置表Excel所在文件夹"))
                {
                    OpenDirectory(Runtime.Path.AB.GetExcelRootDirectoryFullPath());
                    GUIUtility.ExitGUI();
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            if (GUILayout.Button("生成 [ Android & Streaming ] AB & 版本列表文件"))
            {
                LineCommandBuild.BuildAssetBundles(BuildTarget.Android, 0, true);
                OpenDirectory(Runtime.Path.AB.Streaming.GetRootDirectoryFullPath(BuildTarget.Android.ToString()));
                GUIUtility.ExitGUI();
            }

            if (GUILayout.Button("生成 [ iOS & Streaming ] AB & 版本列表文件"))
            {
                LineCommandBuild.BuildAssetBundles(BuildTarget.iOS, 0, true);
                OpenDirectory(Runtime.Path.AB.Streaming.GetRootDirectoryFullPath(BuildTarget.iOS.ToString()));
                GUIUtility.ExitGUI();
            }

            EditorGUILayout.Separator();

            AssetComponent t = (AssetComponent)target;

            int unloadAssetDelayFrameNum = (int)EditorGUILayout.Slider("Asset最小过期帧数", m_UnloadAssetDelayFrameNum.intValue, 0, UnloadAssetDelayFrameNumMax);
            if (unloadAssetDelayFrameNum != m_UnloadAssetDelayFrameNum.intValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.UnloadAssetDelayFrameNum = unloadAssetDelayFrameNum;
                }
                else
                {
                    m_UnloadAssetDelayFrameNum.intValue = unloadAssetDelayFrameNum;
                }
            }

            int loadedMaxNumToCleanMemery = (int)EditorGUILayout.Slider("每轮GC所需异步加载完成资源的累计数量", m_LoadedMaxNumToCleanMemery.intValue, 0, LoadedMaxNumToCleanMemeryMax);
            if (loadedMaxNumToCleanMemery != m_LoadedMaxNumToCleanMemery.intValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.LoadedMaxNumToCleanMemery = loadedMaxNumToCleanMemery;
                }
                else
                {
                    m_LoadedMaxNumToCleanMemery.intValue = loadedMaxNumToCleanMemery;
                }
            }

            DrawPrefabList("Prefab加载完成列表");

            DrawAssetList("Asset预加载列表");
            DrawAssetList("Asset加载中列表");
            DrawAssetList("Asset加载完成列表");
            DrawAssetList("Asset准备卸载列表");

            DrawABList("AB准备列表");
            DrawABList("AB加载中列表");
            DrawABList("AB加载完成列表");
            DrawABList("AB待卸载列表");

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

            RefreshTypeNames();
        }

        private void OnEnable()
        {
            EditorResourceMode = serializedObject.FindProperty("EditorResourceMode");
            m_UnloadAssetDelayFrameNum = serializedObject.FindProperty("m_UnloadAssetDelayFrameNum");
            m_LoadedMaxNumToCleanMemery = serializedObject.FindProperty("m_LoadedMaxNumToCleanMemery");

            RefreshTypeNames();
        }

        private void DrawPrefabList(string listName)
        {
            if (!EditorApplication.isPlaying)
            {
                return;
            }

            AssetComponent t = (AssetComponent)target;

            Dictionary<string, PrefabLoadManager.PrefabObject> prefabList = null;

            switch (listName)
            {
                case "Prefab加载完成列表": prefabList = t.LoadedPrefabList; break;
            }

            bool lastState = m_OpenedItems.Contains(listName);
            bool currentState = EditorGUILayout.Foldout(lastState, $"{listName}({prefabList.Count})");
            if (currentState != lastState)
            {
                if (currentState)
                {
                    m_OpenedItems.Add(listName);
                }
                else
                {
                    m_OpenedItems.Remove(listName);
                }
            }

            if (currentState)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    if (prefabList != null && prefabList.Count > 0)
                    {
                        EditorGUILayout.LabelField("Prefab名称", "引用\t其他信息");
                        foreach (var itr in prefabList)
                        {
                            EditorGUILayout.LabelField(itr.Value.AssetName, $"{itr.Value.RefCount}\t [AB路径]{itr.Value.ABPath}");
                        }

                        if (GUILayout.Button("导出 CSV 数据"))
                        {
                            int dataTotalNum = prefabList.Count + 1;
                            string exportFileName = EditorUtility.SaveFilePanel("导出 CSV 数据", string.Empty, $"{listName} {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss")}.csv", string.Empty);
                            if (!string.IsNullOrEmpty(exportFileName))
                            {
                                try
                                {
                                    int index = 0;
                                    string[] data = new string[prefabList.Count + 1];
                                    data[index++] = "Prefab名称,引用,AB路径";
                                    foreach (var itr in prefabList)
                                    {
                                        data[index++] = $"{itr.Value.AssetName},{itr.Value.RefCount},{itr.Value.ABPath}";
                                    }

                                    File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                    UnityEngine.Debug.Log($"导出 CSV 数据到 '{exportFileName}' 成功。");
                                }
                                catch (Exception exception)
                                {
                                    UnityEngine.Debug.LogError($"导出 CSV 数据到 '{exportFileName}' 失败, 异常信息： '{exception.ToString()}'.");
                                }
                            }
                            GUIUtility.ExitGUI();
                        }
                    }
                    else
                    {
                        GUILayout.Label("列表为空 ...");
                    }
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.Separator();
            }

        }

        private void DrawAssetList(string listName)
        {
            if (!EditorApplication.isPlaying)
            {
                return;
            }

            AssetComponent t = (AssetComponent)target;

            Dictionary<string, AssetLoadManager.AssetObject> assetList = null;
            Queue<AssetLoadManager.PreloadAssetObject> preloadedAssetList = null;
            switch (listName)
            {
                case "Asset加载中列表": assetList = t.LoadingAssetList; break;
                case "Asset加载完成列表": assetList = t.LoadedAssetList; break;
                case "Asset准备卸载列表": assetList = t.UnloadAssetList; break;
                case "Asset预加载列表": preloadedAssetList = t.PreloadedAssetList; break;
            }

            bool lastState = m_OpenedItems.Contains(listName);
            bool currentState = EditorGUILayout.Foldout(lastState, $"{listName}({(preloadedAssetList == null ? assetList.Count : preloadedAssetList.Count)})");
            if (currentState != lastState)
            {
                if (currentState)
                {
                    m_OpenedItems.Add(listName);
                }
                else
                {
                    m_OpenedItems.Remove(listName);
                }
            }

            if (currentState)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    if (assetList != null && assetList.Count > 0)
                    {
                        EditorGUILayout.LabelField("Asset名称", "引用\t常驻\t过期\t来源\t其他信息");
                        foreach (var itr in assetList)
                        {
                            string origin = string.Empty;
                            switch(itr.Value.Origin)
                            {
                                case OriginType.None: origin = "N"; break;
                                case OriginType.Editor: origin = "E"; break;
                                case OriginType.Persistent: origin = "P"; break;
                                case OriginType.Streaming: origin = "S"; break;
                            }
                            string typeName = FillGap(itr.Value.TypeName);
                            EditorGUILayout.LabelField(itr.Value.AssetName, $"{itr.Value.RefCount}\t{(!itr.Value.IsWeak).ToString()}\t{itr.Value.UnloadTickNum}\t{origin}\t[类型]{typeName} \t\t[AB路径]{itr.Value.ABPath}");
                        }

                        if (GUILayout.Button("导出 CSV 数据"))
                        {
                            int dataTotalNum = assetList.Count + 1;
                            string exportFileName = EditorUtility.SaveFilePanel("导出 CSV 数据", string.Empty, $"{listName} {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.csv", string.Empty);
                            if (!string.IsNullOrEmpty(exportFileName))
                            {
                                try
                                {
                                    int index = 0;
                                    string[] data = new string[assetList.Count + 1];
                                    data[index++] = "Asset名称,引用,常驻,过期帧数,来源,类型,AB路径";
                                    foreach (var itr in assetList)
                                    {
                                        string origin = string.Empty;
                                        switch (itr.Value.Origin)
                                        {
                                            case OriginType.None: origin = "N"; break;
                                            case OriginType.Editor: origin = "E"; break;
                                            case OriginType.Persistent: origin = "P"; break;
                                            case OriginType.Streaming: origin = "S"; break;
                                        }
                                        string typeName = FillGap(itr.Value.TypeName);
                                        data[index++] = $"{itr.Value.AssetName},{itr.Value.RefCount},{(!itr.Value.IsWeak).ToString()},{itr.Value.UnloadTickNum},{origin},{typeName},{itr.Value.ABPath}";
                                    }

                                    File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                    UnityEngine.Debug.Log($"导出 CSV 数据到 '{exportFileName}' 成功。");
                                }
                                catch (Exception exception)
                                {
                                    UnityEngine.Debug.LogError($"导出 CSV 数据到 '{exportFileName}' 失败, 异常信息： '{exception.ToString()}'.");
                                }
                            }
                            GUIUtility.ExitGUI();
                        }
                    }
                    else if (preloadedAssetList != null && preloadedAssetList.Count > 0)
                    {
                        EditorGUILayout.LabelField("Asset名称", "常驻\tAB路径\t类型");
                        foreach (var preloadAssetObj in preloadedAssetList)
                        {
                            string typeName = FillGap(preloadAssetObj.TypeName);
                            EditorGUILayout.LabelField(preloadAssetObj.AssetName, $"{(!preloadAssetObj.IsWeak).ToString()}\t{preloadAssetObj.ABPath}\t{typeName}");
                        }

                        if (GUILayout.Button("导出 CSV 数据"))
                        {
                            int dataTotalNum = assetList.Count + 1;
                            string exportFileName = EditorUtility.SaveFilePanel("导出 CSV 数据", string.Empty, $"{listName} {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.csv", string.Empty);
                            if (!string.IsNullOrEmpty(exportFileName))
                            {
                                try
                                {
                                    int index = 0;
                                    string[] data = new string[assetList.Count + 1];
                                    data[index++] = "Asset名称,常驻,AB路径,类型";
                                    foreach (var itr in assetList)
                                    {
                                        string typeName = FillGap(itr.Value.TypeName);
                                        data[index++] = $"{itr.Value.AssetName},{(!itr.Value.IsWeak).ToString()},{itr.Value.ABPath},{typeName}";
                                    }

                                    File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                    UnityEngine.Debug.Log($"导出 CSV 数据到 '{exportFileName}' 成功。");
                                }
                                catch (Exception exception)
                                {
                                    UnityEngine.Debug.LogError($"导出 CSV 数据到 '{exportFileName}' 失败, 异常信息： '{exception.ToString()}'.");
                                }
                            }
                            GUIUtility.ExitGUI();
                        }
                    }
                    else
                    {
                        GUILayout.Label("列表为空 ...");
                    }
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.Separator();
            }

        }

        private void DrawABList(string listName)
        {
            if (!EditorApplication.isPlaying)
            {
                return;
            }

            AssetComponent t = (AssetComponent)target;

            Dictionary<string, ABLoadManager.ABObject> abList = null;
            switch (listName)
            {
                case "AB准备列表": abList = t.ReadyABList; break;
                case "AB加载中列表": abList = t.LoadingABList; break;
                case "AB加载完成列表": abList = t.LoadedABList; break;
                case "AB待卸载列表": abList = t.UnloadABList; break;
            }

            bool lastState = m_OpenedItems.Contains(listName);
            bool currentState = EditorGUILayout.Foldout(lastState, $"{listName}({abList.Count})");
            if (currentState != lastState)
            {
                if (currentState)
                {
                    m_OpenedItems.Add(listName);
                }
                else
                {
                    m_OpenedItems.Remove(listName);
                }
            }

            if (currentState)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    if (abList != null && abList.Count > 0)
                    {
                        EditorGUILayout.LabelField("AB格式化路径", "引用\t来源\t依赖");
                        foreach (var itr in abList)
                        {
                            string origin = string.Empty;
                            switch (itr.Value.Origin)
                            {
                                case OriginType.None: origin = "N"; break;
                                case OriginType.Editor: origin = "E"; break;
                                case OriginType.Persistent: origin = "P"; break;
                                case OriginType.Streaming: origin = "S"; break;
                            }
                            EditorGUILayout.LabelField(itr.Value.FormatPath, $"{itr.Value.RefCount}\t{origin}\t{itr.Value.DependLoadingCount}");
                        }

                        if (GUILayout.Button("导出 CSV 数据"))
                        {
                            int dataTotalNum = abList.Count + 1;
                            string exportFileName = EditorUtility.SaveFilePanel("导出 CSV 数据", string.Empty, $"{listName} {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.csv", string.Empty);
                            if (!string.IsNullOrEmpty(exportFileName))
                            {
                                try
                                {
                                    int index = 0;
                                    string[] data = new string[abList.Count + 1];
                                    data[index++] = "AB格式化路径,引用,来源,依赖";
                                    foreach (var itr in abList)
                                    {
                                        string origin = string.Empty;
                                        switch (itr.Value.Origin)
                                        {
                                            case OriginType.None: origin = "N"; break;
                                            case OriginType.Editor: origin = "E"; break;
                                            case OriginType.Persistent: origin = "P"; break;
                                            case OriginType.Streaming: origin = "S"; break;
                                        }
                                        data[index++] = $"{itr.Value.FormatPath},{itr.Value.RefCount},{origin},{itr.Value.DependLoadingCount}";
                                    }

                                    File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                    UnityEngine.Debug.Log($"导出 CSV 数据到 '{exportFileName}' 成功。");
                                }
                                catch (Exception exception)
                                {
                                    UnityEngine.Debug.LogError($"导出 CSV 数据到 '{exportFileName}' 失败, 异常信息： '{exception.ToString()}'.");
                                }
                            }
                            GUIUtility.ExitGUI();
                        }
                    }
                    else
                    {
                        GUILayout.Label("列表为空 ...");
                    }
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.Separator();
            }

        }

        private void RefreshTypeNames()
        {
            serializedObject.ApplyModifiedProperties();
        }

        private string FillGap(string word, int length = 15)
        {
            while(word.Length < length)
            {
                word += " ";
            }
            word = word.Substring(0, length);
            return word;
        }

        /// <summary>
        /// 打开指定的excel表格
        /// </summary>
        /// <param name="excelPath">要打开的Excel文件绝对路径</param>
        /// <exception cref="Exception"></exception>
        public static void OpenExcel(string excelPath)
        {
            string strExtense = System.IO.Path.GetExtension(excelPath);
            if (strExtense == ".xlsm")
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
            else
            {
                UnityEngine.Debug.LogError("指定的文件不是 xlsm文件");
            }
        }

        /// <summary>
        /// 打开指定的文件夹
        /// </summary>
        /// <param name="directoryPath">需要打开的文件夹绝对路径</param>
        public static void OpenDirectory(string directoryPath)
        {
            if (System.IO.Directory.Exists(directoryPath))
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        Process.Start("Explorer.exe", directoryPath.Replace('/', '\\'));
                        break;

                    case RuntimePlatform.OSXEditor:
                        Process.Start("open", directoryPath);
                        break;

                    default:
                        throw new Exception($"Not support open folder on '{Application.platform.ToString()}' platform.");
                }
            }
            else
            {
                UnityEngine.Debug.LogError($"文件夹 {directoryPath} 不存在，无法打开。");
            }
        }

        /// <summary>
        /// 从ABConfig Excel中导出json文件，默认导出第一个sheet
        /// </summary>
        /// <param name="openExcelNamePre"></param>
        /// <returns></returns>
        private bool ExportExcelToJsonFromABConfig(string openExcelNamePre)
        {
            // 要写入的json文件路径
            string strSubJsonDirectoryPath = Runtime.Path.Project.GetProjectSettingsDirPath();
            string strFilePathList = $"{strSubJsonDirectoryPath}/{openExcelNamePre}.json";
            if (!Directory.Exists(strSubJsonDirectoryPath))
            {
                Directory.CreateDirectory(strSubJsonDirectoryPath);
            }

            string exclePath = Runtime.Path.AB.GetExcelFileFullPath();
            string jsonPath = strFilePathList;

            if (string.IsNullOrEmpty(jsonPath))
            {
                UnityEngine.Debug.LogError("Excel, 导出路径为空，导出json文件失败");
                return false;
            }
            string jsonExtension = System.IO.Path.GetExtension(jsonPath);
            if (jsonExtension != ".json")
            {
                UnityEngine.Debug.LogError("Excel, 导出json文件需要扩展名为.json");
                return false;
            }

            // 要打开的excel根路径
            DataSet result = GetExcelData(exclePath);

            // 开始添加文件头
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("{");

            int columns = result.Tables[0].Columns.Count;
            int rows = result.Tables[0].Rows.Count;
            string[] cellKey = new string[columns];
            string[] typeList = new string[columns];
            for (int keyi = 0; keyi < columns; keyi++)
            {
                cellKey[keyi] = result.Tables[0].Rows[1][keyi].ToString();
                typeList[keyi] = result.Tables[0].Rows[2][keyi].ToString();
                if (cellKey[keyi] != string.Empty & typeList[keyi] == string.Empty)
                {
                    UnityEngine.Debug.LogError(exclePath + "表格错误, " + cellKey[keyi] + " 没有类型");
                    return false;
                }
            }
            for (int i = 4; i < rows; i++)
            {
                string Key = result.Tables[0].Rows[i][1].ToString();
                string thisRow = "    \"" + Key + "\" : {";
                for (int j = 1; j < columns; j++)
                {
                    if (cellKey[j] != string.Empty)
                    {
                        string cellValue = result.Tables[0].Rows[i][j].ToString();
                        if (typeList[j] == "number")
                        {
                            thisRow = thisRow + "\"" + cellKey[j] + "\":" + cellValue;
                        }
                        else if (typeList[j] == "string" || typeList[j] == "table")
                        {
                            thisRow = thisRow + "\"" + cellKey[j] + "\":" + "\"" + cellValue + "\"";
                        }
                        else if (typeList[j] == "boolean")
                        {
                            thisRow = thisRow + "\"" + cellKey[j] + "\":" + cellValue;
                        }
                        if (j < columns - 1)
                        {
                            thisRow = thisRow + ",";
                        }
                    }
                }
                thisRow = thisRow + "}";
                if (i < rows - 1)
                {
                    thisRow = thisRow + ",";
                }
                stringBuilder.AppendLine(thisRow);
            }
            stringBuilder.AppendLine("}");
            // 开始写入数据
            if (File.Exists(jsonPath))
            {
                File.Delete(jsonPath);
            }
            File.WriteAllText(jsonPath, stringBuilder.ToString(), new System.Text.UTF8Encoding(false));
            UnityEngine.Debug.Log($"{exclePath} 导出 {jsonPath}完成");

            AssetDatabase.Refresh();

            return true;
        }

        /// <summary>
        /// 获取Excel表格内容，目前支持.xlsm和.csv文件
        /// </summary>
        /// <param name="ExcleAbsolutePath">Excel文件的绝对路径</param>
        /// <param name="CsvFile">是否是.csv文件</param>
        /// <returns></returns>
        public static DataSet GetExcelData(string ExcleAbsolutePath)
        {
            if (string.IsNullOrEmpty(ExcleAbsolutePath))
            {
                UnityEngine.Debug.LogError("Excel, 打开路径为null");
                return null;
            }

            string strExtension = System.IO.Path.GetExtension(ExcleAbsolutePath);
            bool csvFile = false;
            if (strExtension == ".xlsm")
            {
                csvFile = false;
            }
            else if (strExtension == ".csv")
            {
                csvFile = true;
            }
            else
            {
                UnityEngine.Debug.LogError("Excel, 需要打开的文件类型不是xlsm或csv");
                return null;
            }

            FileStream stream = null;
            try
            {
                stream = File.Open(ExcleAbsolutePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("表格" + ExcleAbsolutePath + " 打开失败，请检查表格是否存在");
                throw;
            }

            try
            {
                IExcelDataReader excelReader = null;
                if (csvFile)
                {
                    ExcelReaderConfiguration csvConfig = new ExcelReaderConfiguration();
                    csvConfig.FallbackEncoding = Encoding.GetEncoding("GB2312");
                    excelReader = ExcelReaderFactory.CreateCsvReader(stream, csvConfig);
                }
                else
                {
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                // 转换数据
                DataSet excelData = (excelReader != null) ? excelReader.AsDataSet() : null;
                stream.Close();
                excelReader.Close();
                return excelData;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError($"Excel {ExcleAbsolutePath}存在无法解析的数据。");
                throw;
            }
        }

    }
}
