using UnityEngine;
using Newtonsoft.Json;

namespace SY_FrameWork
{
		public partial class SysConfig  { 
           
              		/// <summary>		/// 配置文件 		/// <summary>		public int Id { get; set; }		/// <summary>		/// 应用名 		/// <summary>		public string appName { get; set; }		/// <summary>		/// 测试后台id 		/// <summary>		public int debugID { get; set; }		/// <summary>		/// 正式后台id 		/// <summary>		public int releaseID { get; set; }		/// <summary>		/// 测试后台密钥 		/// <summary>		public string debugKey { get; set; }		/// <summary>		/// 正式后台密钥 		/// <summary>		public string releaseKey { get; set; }		/// <summary>		/// 包名 		/// <summary>		public string packageName { get; set; }		/// <summary>		/// 隐私协议 		/// <summary>		public string ysxy { get; set; }		/// <summary>		/// 服务协议 		/// <summary>		public string fwxy { get; set; } 
           
              public const string ConfigName = "SysConfig" ; 
   
               public string Version { get; set; } 
           
               public SysConfig [] Datas { private get; set; } 
           
               public static SysConfig Config { private get; set; } 
           
               public static void Init() 
               { 
                   if (Config == null)
                   {
                       Config = ConfigUitls.LoadConfigTextAsset<SysConfig>(ConfigName); 
                    }
               } 
           
               public static void Init(string json) 
               { 
                   Config = ConfigUitls.Deserialize<SysConfig>(json); 
               } 
           
               public static void Init(SysConfig con) 
               { 
                   Config = con; 
               } 
           
               public static SysConfig Get(int id) 
               { 
                   if (Config == null) 
                   { 
                       Init(); 
                   }
           
                   if (Config == null)
                   {
                       return null;
                   }
           
                   foreach (var item in Config.Datas) 
                   { 
                       if (item.Id == id) 
                       { 
                           return item; 
                       } 
                   } 
           
                   return null; 
               } 
           
               public static SysConfig [] GetConfigs() 
               { 
                   if (Config == null) 
                   { 
                       Init(); 
                   }
           
                   if (Config == null)
                   {
                       return null;
                   }
           
                   return Config.Datas; 
               } 
           
               public static int GetLength() 
               { 
                   SysConfig [] configs = GetConfigs();
                   if (Config == null) 
                   { 
                       return 0; 
                   }
   
                   return configs.Length; 
               } 
          };
}
