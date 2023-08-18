using UnityEngine;
using Newtonsoft.Json;

namespace SY_FrameWork
{
    public class SysConfig  {
        		/// <summary>		/// 配置文件 		/// <summary>		public int id { get; set; }		/// <summary>		/// 应用名 		/// <summary>		public string appName { get; set; }		/// <summary>		/// 测试后台id 		/// <summary>		public int debugID { get; set; }		/// <summary>		/// 正式后台id 		/// <summary>		public int releaseID { get; set; }		/// <summary>		/// 测试后台密钥 		/// <summary>		public string debugKey { get; set; }		/// <summary>		/// 正式后台密钥 		/// <summary>		public string releaseKey { get; set; }		/// <summary>		/// 包名 		/// <summary>		public string packageName { get; set; }		/// <summary>		/// 隐私协议 		/// <summary>		public string ysxy { get; set; }		/// <summary>		/// 服务协议 		/// <summary>		public string fwxy { get; set; } 
    
        public static string configName = "SysConfig";
        
        public static string Version { get { return Config.version; } }
        
        public static SysConfig [] Datas { get { return Config.datas; } }
        
        public static SysConfig Config {get { if (config == null) Init(); return config;}}
        
        private static SysConfig config;
                                                                                                                                                    
        public string version { get; set; }
        
        public SysConfig [] datas { get; set; }
    
        private static void Init()
        {
            TextAsset jsonStr = Resources.Load(configName) as TextAsset;
            config = JsonConvert.DeserializeObject<SysConfig>(jsonStr.text);
        }
        
        public static SysConfig Get(int id)
        {
            foreach (var item in Config.datas)
            {
                if (item.id == id)
                { 
                    return item;
                }
            }
            return null;
        }
    }
}
