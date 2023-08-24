using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace SY_Editor
{
    public partial class ExcelEditor : EditorWindow
    {
    
        
        /// <summary>
        /// 根据当前脚本路径获取根路径
        /// </summary>
        /// <param name="_scriptName"></param>
        /// <returns></returns>
        static void  GetRootPath(string _scriptName)
        {
            string[] path = AssetDatabase.FindAssets(_scriptName);
            if (path.Length > 1)
            {
                Debug.LogError(_scriptName + "有同名文件!");
            }

            string root = Application.dataPath.Replace("Assets", "");
            _rootDirectory = root + AssetDatabase.GUIDToAssetPath(path[0]).Replace((@"Editor/" + _scriptName + ".cs"), "");
            Debug.Log("Root Directory:" + _rootDirectory);
        }

      

        /// <summary>
        /// 绘制按钮
        /// </summary>
        void DrawButton()
        {
            isCompression = GUILayout.Toggle(isCompression,"Json是否进行压缩");
                
            GUILayout.Space(10);
            if (GUILayout.Button("BuildSelected"))
            {
                SelectionRead();
                AssetDatabase.Refresh();
            }

            GUILayout.Space(10);
            if (GUILayout.Button("BuildAll"))
            {
                ReadExcel();
                AssetDatabase.Refresh();
            }
        }

        void SelectionRead()
        {
            string[] fils = Selection.assetGUIDs;
            if (fils.Length > 0)
            {
                foreach (var file in fils)
                {
                    var path = Path.GetFullPath(AssetDatabase.GUIDToAssetPath(file));
                    string fileName = Path.GetFileName(path);
                    if (!fileName.EndsWith("xlsx"))
                    {
                        Debug.LogError($"所选文件不符合:{fileName}");
                        continue;
                    }

                    LoadData(path, fileName);
                    Debug.Log($"已完成:{fileName}");
                }
            }
            else
            {
                Debug.LogError("没有选中任何文件！");
            }
        }

        /// <summary>
        /// 绘制版本和日期
        /// </summary>
        void DrawVersionAndDate()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(10);
           
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("版本号:", GUILayout.Width(100));
            string dateStr = DateTime.Now.ToString("yyyyMMdd");
            version = (long.Parse(dateStr) * 1000 + versionNum).ToString();
            EditorGUILayout.LabelField(version, GUILayout.Width(150));
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                versionNum--;
            }

            versionNum = EditorGUILayout.IntField(versionNum, GUILayout.Width(100));

            if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                versionNum++;
            }

            versionNum = versionNum > 999 ? 999 : versionNum < 1 ? 1 : versionNum;
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            timeStr = DateTime.Now.ToString();
            EditorGUILayout.LabelField("日期    :", GUILayout.Width(100));
            EditorGUILayout.LabelField(timeStr);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.EndVertical();
        }

        /// <summary>
        /// 遍历文件夹，读取所有表格
        /// </summary>
        public void ReadExcel()
        {
            if (CheckDirectory())
            {
                //获取指定目录下所有的文件
                DirectoryInfo direction = new DirectoryInfo(tablePath);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                Debug.Log("FileCount:" + files.Length / 2);

                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".meta") || !files[i].Name.EndsWith(".xlsx"))
                        continue;

                    Debug.Log($"已完成==>>{files[i].FullName}");
                    LoadData(files[i].FullName, files[i].Name);
                }
            }
        }

        /// <summary>
        /// 检测所需目录
        /// </summary>
        /// <returns></returns>
        bool CheckDirectory()
        {
            if (Directory.Exists(tablePath))
            {
                if (!Directory.Exists(scriptsPath))
                    Directory.CreateDirectory(scriptsPath);

                if (!Directory.Exists(jsonPath))
                    Directory.CreateDirectory(jsonPath);

                return true;
            }
            else
            {
                Debug.LogError("ReadExcel configPath not Exists!" + tablePath);
                return false;
            }
        }

        /// <summary>
        /// 读取表格并保存脚本及json
        /// </summary>
        void LoadData(string filePath, string fileName)
        {
            //获取文件流
            try
            {
                FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
                //生成表格的读取
                IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                // 表格数据全部读取到result里(引入：DataSet（using System.Data;）
                DataSet result = excelDataReader.AsDataSet();

                CreateJson(result, fileName);
                CreateTemplate(result, fileName);

                fileStream.Close();
                result.Clear();
            }
            catch (Exception e)
            {
                Debug.LogError($"表格读取失败==>> FileName:{fileName}  error:{e}");
            }
        }

        /// <summary>
        /// 生成json文件
        /// </summary>
        void CreateJson(DataSet result, string fileName)
        {
            // 获取表格有多少列 
            int columns = result.Tables[0].Columns.Count;
            // 获取表格有多少行  
            int rows = result.Tables[0].Rows.Count;
            // 表格数据列表
            List<TableData> dataList = new List<TableData>();

            JArray array = new JArray();

            //第一行为表头，第二行为类型 ，第三行为字段名 不读取
            for (int i = 3; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    // 获取表格中指定行指定列的数据 
                    string value = result.Tables[0].Rows[i][j].ToString();

                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }

                    TableData tempData = new TableData();
                    tempData.type = result.Tables[0].Rows[1][j].ToString();
                    tempData.fieldName = result.Tables[0].Rows[2][j].ToString();
                    tempData.value = value;
                    dataList.Add(tempData);
                }

                if (dataList != null && dataList.Count > 0)
                {
                    JObject tempo = new JObject();
                    foreach (var item in dataList)
                    {
                        switch (item.type)
                        {
                            case "string":
                            case "String":
                                tempo[item.fieldName] = GetValue<string>(item.value);
                                break;
                            case "int":
                            case "Int":
                                tempo[item.fieldName] = GetValue<int>(item.value);
                                break;
                            case "Float":
                            case "float":
                                tempo[item.fieldName] = GetValue<float>(item.value);
                                break;
                            case "Bool":
                            case "bool":
                                tempo[item.fieldName] = GetValue<bool>(item.value);
                                break;
                            case "string[]":
                            case "String[]":
                                tempo[item.fieldName] = new JArray(GetList<string>(item.value));
                                break;
                            case "int[]":
                            case "Int[]":
                                tempo[item.fieldName] = new JArray(GetList<int>(item.value));
                                break;
                            case "float[]":
                            case "Float[]":
                                tempo[item.fieldName] = new JArray(GetList<float>(item.value));
                                break;
                            case "bool[]":
                            case "Bool[]":
                                tempo[item.fieldName] = new JArray(GetList<bool>(item.value));
                                break;
                            case "Vector3":
                            case "vector3":
                            case "v3":
                                tempo[item.fieldName] = GetVector3(item.value);
                                break;
                        }
                    }

                    if (tempo != null)
                        array.Add(tempo);
                    dataList.Clear();
                }
            }

            JObject o = new JObject();
            o["datas"] = array;
            o["version"] = version;

            fileName = fileName.Replace(".xlsx", ".json");
            string saveJaon = JsonConvert.SerializeObject(o,isCompression? Formatting.None: Formatting.Indented);
            File.WriteAllText(jsonPath + fileName, saveJaon);
        }

        /// <summary>
        /// 字符串拆分列表
        /// </summary>
        static List<T> GetList<T>(string str)
        {
            str = str.Replace("[", "");
            str = str.Replace("]", "");
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            string[] ss = str.Split(',');
            int length = ss.Length;
            List<T> arry = new List<T>(ss.Length);
            for (int i = 0; i < length; i++)
            {
                arry.Add(GetValue<T>(ss[i]));
            }

            return arry;
        }

        static T GetValue<T>(object value)
        {
            T t = default;
            try
            {
                t = (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception e)
            {
                Debug.LogError("GetValue  value:" + value.ToString() + " error:" + e.ToString());
            }

            return t;
        }

        /// <summary>
        /// 解析Vector3
        /// </summary>
        static string GetVector3(string str)
        {
            //只有数组和Vector需要去除括号
            str = str.Replace("[", "");
            str = str.Replace("]", "");
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            string[] ss = str.Split(',');
            float x = 0, y = 0, z = 0;
            int length = ss.Length;
            if (length > 0)
            {
                x = length >= 1 ? float.Parse(ss[0]) : 0;
                y = length >= 2 ? float.Parse(ss[1]) : 0;
                z = length >= 3 ? float.Parse(ss[2]) : 0;
            }

            string json = "-{" + string.Format("-\"x-\" : {0}, -\"y-\" : {1}, -\"z-\" : {2}", x, y, z) + "}-";
            return json;
        }

        /// <summary>
        /// 生成实体类模板
        /// </summary>
        void CreateTemplate(DataSet result, string fileName)
        {
            string field = "";
            for (int i = 0; i < result.Tables[0].Columns.Count; i++)
            {
                string typeStr = result.Tables[0].Rows[1][i].ToString();
                typeStr = typeStr.ToLower();

                if (!typeStr.Equals("Vector3") && !typeStr.Equals("vector3") && !typeStr.Equals("v3"))
                {
                    typeStr = typeStr.ToLower();
                }

                if (typeStr.Equals("vector3") || typeStr.Equals("v3"))
                {
                    typeStr = "Vector3";
                }

                string nameStr = result.Tables[0].Rows[2][i].ToString();
                if (string.IsNullOrEmpty(typeStr) || string.IsNullOrEmpty(nameStr))
                {
                    continue;
                }

                string note = result.Tables[0].Rows[0][i].ToString();
                note = note.Replace("\n", "");
                note = note.Replace("\r", "");
                field += "\r\t\t/// <summary>" + 
                         $"\r\t\t/// { note } "+ 
                         "\r\t\t/// <summary>" +
                         $"\r\t\tpublic {typeStr} {nameStr}"+ " { get; set; }\r";
            }

            fileName = fileName.Replace(".xlsx", "");
            string tempStr = System.IO.File.ReadAllText(modelPath + "TableModel.txt");
            tempStr = tempStr.Replace("@Name", fileName);
            tempStr = tempStr.Replace("@File1", field);
            tempStr = tempStr.Replace("@json", "");
            File.WriteAllText(scriptsPath + fileName + ".cs", tempStr);
        }
        
        public struct TableData
        {
            public string fieldName;
            public string type;
            public string value;

            public override string ToString()
            {
                return string.Format("fieldName:{0} type:{1} value:{2}", fieldName, type, value);
            }
        }
        
        
    }
}

