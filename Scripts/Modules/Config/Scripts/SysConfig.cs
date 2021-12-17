using UnityEngine;
using Newtonsoft.Json;

namespace SY_FrameWork
{
    public class SysConfig  {
        	/// <summary>	///配置文件	/// <summary>	public int id { get; set; }	/// <summary>	///应用名	/// <summary>	public string appName { get; set; }	/// <summary>	///测试后台id	/// <summary>	public int debugID { get; set; }	/// <summary>	///正式后台id	/// <summary>	public int releaseID { get; set; }	/// <summary>	///测试后台密钥	/// <summary>	public string debugKey { get; set; }	/// <summary>	///正式后台密钥	/// <summary>	public string releaseKey { get; set; }	/// <summary>	///包名	/// <summary>	public string packageName { get; set; }	/// <summary>	///隐私协议	/// <summary>	public string ysxy { get; set; }	/// <summary>	///服务协议	/// <summary>	public string fwxy { get; set; }	/// <summary>	///官方链接	/// <summary>	public string link { get; set; }	/// <summary>	///反馈邮箱	/// <summary>	public string email { get; set; }	/// <summary>	///AFKey	/// <summary>	public string af { get; set; }	/// <summary>	///TGAkey	/// <summary>	public string tgaKey { get; set; }	/// <summary>	///TGA上报地址	/// <summary>	public string tgaUrl { get; set; }	/// <summary>	///Topon APPKEY	/// <summary>	public string ToponAPPKEY { get; set; }	/// <summary>	///Topon APPID	/// <summary>	public string ToponAPPID { get; set; }	/// <summary>	///Topon激励视频广告位id	/// <summary>	public string rvID { get; set; }	/// <summary>	///Topon全屏视频广告位id	/// <summary>	public string qpID { get; set; }	/// <summary>	///Topon开屏告位id	/// <summary>	public string openID { get; set; }	/// <summary>	///Topon banner id	/// <summary>	public string bannerID { get; set; }	/// <summary>	///Topon 原生 id	/// <summary>	public string ysID { get; set; }	/// <summary>	///Topon 插屏图片 id	/// <summary>	public string ctID { get; set; }	/// <summary>	///微信开放平台APPID	/// <summary>	public string wxAPPID { get; set; }	/// <summary>	///微信开放平台AppSecret	/// <summary>	public string wxAPPSecret { get; set; }	/// <summary>	///提现配置的key	/// <summary>	public string wd_config { get; set; }	/// <summary>	///友盟key	/// <summary>	public string um_key { get; set; }	/// <summary>	///穿山甲 APPID	/// <summary>	public string csj_id { get; set; }	/// <summary>	///穿山甲 代码位	/// <summary>	public string csj_slot_id { get; set; }	/// <summary>	///穿山甲 默认广告id	/// <summary>	public string source_id { get; set; }	/// <summary>	///阿里黑名单key	/// <summary>	public string aliyun_key { get; set; } 
    
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
