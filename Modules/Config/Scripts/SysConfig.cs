using UnityEngine;
using Newtonsoft.Json;

namespace SY_FrameWork
{
    public class SysConfig  {
        
    
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