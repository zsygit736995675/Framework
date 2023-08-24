using UnityEngine;
using Newtonsoft.Json;

namespace SY_FrameWork
{
		public partial class StrConfig  { 
           
              		/// <summary>		/// 字符表 		/// <summary>		public int Id { get; set; }		/// <summary>		/// 中文简体zh 		/// <summary>		public string CHINESE { get; set; }		/// <summary>		/// 印地语hi 		/// <summary>		public string HINDI { get; set; }		/// <summary>		/// 英语en 		/// <summary>		public string ENGLISH { get; set; }		/// <summary>		/// 葡萄牙语po 		/// <summary>		public string Portuguese { get; set; }		/// <summary>		/// 阿拉伯语 		/// <summary>		public string Arabic { get; set; }		/// <summary>		/// 法语 		/// <summary>		public string French { get; set; }		/// <summary>		/// 印尼语 		/// <summary>		public string Indonesian { get; set; }		/// <summary>		/// 土耳其语 		/// <summary>		public string Turkic { get; set; }		/// <summary>		/// 德语 		/// <summary>		public string German { get; set; }		/// <summary>		/// 西语 		/// <summary>		public string Spanish { get; set; }		/// <summary>		/// 日语 		/// <summary>		public string Japanese { get; set; }		/// <summary>		/// 韩语 		/// <summary>		public string Korean { get; set; }		/// <summary>		/// 繁体中文 		/// <summary>		public string TC { get; set; }		/// <summary>		/// 俄罗斯ru 		/// <summary>		public string RUSSIAN { get; set; }		/// <summary>		/// 意大利it 		/// <summary>		public string ITALIAN { get; set; }		/// <summary>		/// 越南 		/// <summary>		public string VIETNAMESE { get; set; } 
           
              public const string ConfigName = "StrConfig" ; 
   
               public string Version { get; set; } 
           
               public StrConfig [] Datas { private get; set; } 
           
               public static StrConfig Config { private get; set; } 
           
               public static void Init() 
               { 
                   if (Config == null)
                   {
                       Config = ConfigUitls.LoadConfigTextAsset<StrConfig>(ConfigName); 
                    }
               } 
           
               public static void Init(string json) 
               { 
                   Config = ConfigUitls.Deserialize<StrConfig>(json); 
               } 
           
               public static void Init(StrConfig con) 
               { 
                   Config = con; 
               } 
           
               public static StrConfig Get(int id) 
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
           
               public static StrConfig [] GetConfigs() 
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
                   StrConfig [] configs = GetConfigs();
                   if (Config == null) 
                   { 
                       return 0; 
                   }
   
                   return configs.Length; 
               } 
          };
}
