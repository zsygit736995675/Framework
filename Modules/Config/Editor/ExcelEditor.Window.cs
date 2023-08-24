using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using System;
using ExcelDataReader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;

namespace SY_Editor
{
    /// <summary>
    /// 表格编辑器
    /// </summary>
    public partial class ExcelEditor : EditorWindow
    {
        /// <summary>
        /// 根目录（会刷新）
        /// </summary>
        private static  string _rootDirectory ;

        private string RootDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_rootDirectory))
                {
                    GetRootPath(nameof(ExcelEditor));
                }

                return _rootDirectory;
            }
        }

        /// <summary>
        /// 脚本存放位置
        /// </summary>
        private string scriptsPath { get { return RootDirectory + "Scripts/"; } }

        /// <summary>
        /// json文件存放位置
        /// </summary>
        private string jsonPath { get { return RootDirectory + "Resources/"; } }

        /// <summary>
        /// 表格存放位置
        /// </summary>
        private string tablePath { get { return RootDirectory + "Table/"; } }

        /// <summary>
        /// 模板存放位置
        /// </summary>
        private string modelPath { get { return RootDirectory + "Model/"; } }

        /// <summary>
        ///  版本号
        /// </summary>
        private static int versionNum = 1;

        /// <summary>
        /// 时间
        /// </summary>
        private static string timeStr = "";

        /// <summary>
        /// 完整的版本号
        /// </summary>
        private static string version;

        /// <summary>
        /// 是否压缩
        /// </summary>
        private static bool isCompression = false;
        
        
        [MenuItem("SY_Tools/打开表格管理窗口", false, 1)]
        public static void ShowWindow()
        {
            Rect tect = new Rect(Screen.width / 3, Screen.height / 3, 600, 600);
            EditorWindow window = GetWindowWithRect(typeof(ExcelEditor), tect , true, "配置表格管理窗口");
            window.Show();
        }
        
        private void OnGUI()
        {
            DrawVersionAndDate();
            DrawButton();
        }

        private void OnEnable()
        {
            GetRootPath(nameof(ExcelEditor));
        }
        
        
    }
    
    


}