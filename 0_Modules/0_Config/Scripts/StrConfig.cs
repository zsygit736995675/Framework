using UnityEngine;
using Newtonsoft.Json;

namespace SY_FrameWork
{
    public class StrConfig  {
        	/// <summary>	///字符表	/// <summary>	public int id { get; set; }	/// <summary>	///中文	/// <summary>	public string _zh { get; set; }	/// <summary>	///英文	/// <summary>	public string _en { get; set; }	/// <summary>	///印地语	/// <summary>	public string _hi { get; set; }	/// <summary>	///日文	/// <summary>	public string _jp { get; set; }	/// <summary>	///韩文	/// <summary>	public string _kr { get; set; }	/// <summary>	///西语	/// <summary>	public string _sp { get; set; }	/// <summary>	///德语	/// <summary>	public string _ge { get; set; } 
    
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
