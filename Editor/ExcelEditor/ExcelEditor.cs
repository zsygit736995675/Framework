using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using Excel;
using Newtonsoft.Json.Linq;
using System;
using UnityEditor;

/// <summary>
/// 生成配置文件
/// </summary>
public class ExcelEditor : EditorWindow
{

    /// <summary>
    /// 模板存放位置
    /// </summary>
    static string scriptsPath = "/Framework/Scripts/Config/";

    /// <summary>
    /// json文件存放位置
    /// </summary>
    static string jsonPath = "/Framework/Resources/Config/";

    /// <summary>
    /// 表格存放位置
    /// </summary>
    static string configPath = "/Framework/Config/";

    /// <summary>
    /// 表格数据列表
    /// </summary>
    static List<TableData> dataList = new List<TableData>();


    /// <summary>
    ///  版本号
    /// </summary>
    string version = "20200331";


    [MenuItem("Tools /Build表格")]  //添加菜单选项
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindowWithRect(typeof(ExcelEditor), new Rect(Screen.width / 3, Screen.height / 3, 500, 100), true, "配置文件生成窗口");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        version = EditorGUILayout.TextField("Version", version);

        GUILayout.Space(10);
        if (GUILayout.Button("Build"))
        {
            ReadExcel();
        }
    }

    /// <summary>
    /// 遍历文件夹，读取所有表格
    /// </summary>
    public void ReadExcel()
    {
        if (!Directory.Exists(UnityEngine.Application.dataPath + scriptsPath))
            Directory.CreateDirectory(UnityEngine.Application.dataPath + scriptsPath);

        if (!Directory.Exists(UnityEngine.Application.dataPath + jsonPath))
            Directory.CreateDirectory(UnityEngine.Application.dataPath + jsonPath);

        string Path = UnityEngine.Application.dataPath + configPath;
        if (Directory.Exists(Path))
        {
            //获取指定目录下所有的文件
            DirectoryInfo direction = new DirectoryInfo(Path);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            Debug.Log("fileCount:" + files.Length);

            //CreateMgr(files);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta") || !files[i].Name.EndsWith(".xlsx"))
                {
                    continue;
                }
                Debug.Log("FullName:" + files[i].FullName);
                LoadData(files[i].FullName, files[i].Name);
            }
        }
        else
        {
            Debug.LogError("ReadExcel configPath not Exists!" + Path);
        }
    }

    /// <summary>
    /// 生成manager
    /// </summary>
    void CreateMgr(FileInfo[] files)
    {
        string field = "";
        foreach (var item in files)
        {
            if (item.Name.EndsWith(".meta") || !item.Name.EndsWith(".xlsx"))
                continue;

            string name = item.Name.Replace(".xlsx", "");
            field += name + ".Init();\r\t\t";
        }
        string txt = Mgr_str;
        field = Mgr_str.Replace("@field", field);
        File.WriteAllText(UnityEngine.Application.dataPath + scriptsPath + "ConfigMgr.cs", field);
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

            fileStream.Close();
            result.Clear();
        }
        catch (Exception e)
        {
            Debug.LogError("表格可能打开了！" + e);
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

        TableData tempData;
        string value;
        JArray array = new JArray();

        //第一行为表头，第二行为类型 ，第三行为字段名 不读取
        for (int i = 3; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // 获取表格中指定行指定列的数据 
                value = result.Tables[0].Rows[i][j].ToString();

                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }
                tempData = new TableData();
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
                            tempo[item.fieldName] = GetValue<string>(item.value);
                            break;
                        case "int":
                            tempo[item.fieldName] = GetValue<int>(item.value);
                            break;
                        case "float":
                            tempo[item.fieldName] = GetValue<float>(item.value);
                            break;
                        case "bool":
                            tempo[item.fieldName] = GetValue<bool>(item.value);
                            break;
                        case "string[]":
                            tempo[item.fieldName] = new JArray(GetList<string>(item.value, ','));
                            break;
                        case "int[]":
                            tempo[item.fieldName] = new JArray(GetList<int>(item.value, ','));
                            break;
                        case "float[]":
                            tempo[item.fieldName] = new JArray(GetList<float>(item.value, ','));
                            break;
                        case "bool[]":
                            tempo[item.fieldName] = new JArray(GetList<bool>(item.value, ','));
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

        CreateTemplate(result, fileName, o.ToString());

        fileName = fileName.Replace(".xlsx", ".json");
        File.WriteAllText(UnityEngine.Application.dataPath + jsonPath + fileName, o.ToString());
    }


    /// <summary>
    /// 字符串拆分列表
    /// </summary>
    static List<T> GetList<T>(string str, char spliteChar)
    {
        string[] ss = str.Split(spliteChar);
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
    /// 生成实体类模板
    /// </summary>
    void CreateTemplate(DataSet result, string fileName, string json)
    {
        if (!Directory.Exists(UnityEngine.Application.dataPath + scriptsPath))
        {
            Directory.CreateDirectory(UnityEngine.Application.dataPath + scriptsPath);
        }

        string field = "";
        for (int i = 0; i < result.Tables[0].Columns.Count; i++)
        {
            string typeStr = result.Tables[0].Rows[1][i].ToString();
            typeStr = typeStr.ToLower();

            string nameStr = result.Tables[0].Rows[2][i].ToString();
            if (string.IsNullOrEmpty(typeStr) || string.IsNullOrEmpty(nameStr))
            {
                continue;
            }

            string note = result.Tables[0].Rows[0][i].ToString();
            note = note.Replace("\n", "");
            note = note.Replace("\r", "");
            field += "/// <summary>\r\t\t///" + note + "\r\t\t/// <summary>\r\t\tpublic " + typeStr + " " + nameStr + " { get; set; }\r\t\t";
        }

        fileName = fileName.Replace(".xlsx", "");
        string tempStr = Eg_str;
        tempStr = tempStr.Replace("@Name", fileName);
        tempStr = tempStr.Replace("@File1", field);
        tempStr = tempStr.Replace("@json", "");
        File.WriteAllText(UnityEngine.Application.dataPath + scriptsPath + fileName + ".cs", tempStr);
    }

    /// <summary>
    /// manager模板
    /// </summary>
    static string Mgr_str =

        "using System;\r" +
        "using UnityEngine;\r\r" +
        "public class ConfigMgr : BaseClass<ConfigMgr>\r" +
        "{\r\t" +
        "public void Init()\r\t" +
        "{\r\t\t" +
        "@field\r\t" +
        "}\r" +
        "}";


    /// <summary>
    /// 实体类模板
    /// </summary>
    static string Eg_str =

        "using UnityEngine;\r" +
        "using Newtonsoft.Json;\r\r" +
        "public class @Name  {\r\r\t\t" +
        "@File1 \r\t\t" +
        "public static string configName = \"@Name\";\r\t\t" +
        "public static string Version { get { return Config.version; } }\r\t\t" +
        "public static @Name [] Datas { get { return Config.datas; } }\r\t\t" +
        "public static @Name Config {get { if (config == null) Init(); return config;}}\r\r\t\t" +
        "private static @Name config;\r\t\t" +
        "public string version { get; set; }\r\t\t" +
        "public @Name [] datas { get; set; }\r\r\t\t" +
        "private static void Init()\r\t\t{\r\t\t\tTextAsset jsonStr = Resources.Load(\"Config/\"+configName) as TextAsset;\r\t\t\t" +
        "config = JsonConvert.DeserializeObject<@Name>(jsonStr.text);\r\t\t}\r\r\t\t" +
        "public static @Name Get(int id)\r\t\t{\r\t\t\tforeach (var item in Config.datas)\r\t\t\t{\r\t\t\t\tif (item.id == id)\r\t\t\t\t{ \r\t\t\t\t\treturn item;\r\t\t\t\t}\r\t\t\t}\r\t\t\treturn null;\r\t\t}\r\r\t\t" +
        "\r}";
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
