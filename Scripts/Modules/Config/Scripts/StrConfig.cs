using UnityEngine;
using Newtonsoft.Json;

namespace SY_FrameWork
{
    public class StrConfig  {
        
    
        public static string configName = "StrConfig";
        
        public static string Version { get { return Config.version; } }
        
        public static StrConfig [] Datas { get { return Config.datas; } }
        
        public static StrConfig Config {get { if (config == null) Init(); return config;}}
        
        private static StrConfig config;
        
        public string version { get; set; }
        
        public StrConfig [] datas { get; set; }
    
        private static void Init()
        {
            TextAsset jsonStr = Resources.Load(configName) as TextAsset;
            config = JsonConvert.DeserializeObject<StrConfig>(jsonStr.text);
        }
        
        public static StrConfig Get(int id)
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