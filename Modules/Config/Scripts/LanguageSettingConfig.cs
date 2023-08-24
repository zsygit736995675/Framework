using UnityEngine;
using Newtonsoft.Json;

namespace SY_FrameWork
{
		public partial class LanguageSettingConfig  { 
           
              		/// <summary>		/// 字符表 		/// <summary>		public int Id { get; set; }		/// <summary>		/// 对应语言中全称 		/// <summary>		public string Country { get; set; }		/// <summary>		/// 缩写 		/// <summary>		public string ShortName { get; set; }		/// <summary>		/// 中文翻译 		/// <summary>		public string Annotate { get; set; }		/// <summary>		/// 字体资源名称 		/// <summary>		public string FontAssetName { get; set; }		/// <summary>		/// 在UI中的排序 		/// <summary>		public int Sort { get; set; }		/// <summary>		/// 在多语言表中的属性名称 		/// <summary>		public string StrKey { get; set; }		/// <summary>		/// 是否开启 		/// <summary>		public bool IsUse { get; set; } 
           
              public const string ConfigName = "LanguageSettingConfig" ; 
   
               public string Version { get; set; } 
           
               public LanguageSettingConfig [] Datas { private get; set; } 
           
               public static LanguageSettingConfig Config { private get; set; } 
           
               public static void Init() 
               { 
                   if (Config == null)
                   {
                       Config = ConfigUitls.LoadConfigTextAsset<LanguageSettingConfig>(ConfigName); 
                    }
               } 
           
               public static void Init(string json) 
               { 
                   Config = ConfigUitls.Deserialize<LanguageSettingConfig>(json); 
               } 
           
               public static void Init(LanguageSettingConfig con) 
               { 
                   Config = con; 
               } 
           
               public static LanguageSettingConfig Get(int id) 
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
           
               public static LanguageSettingConfig [] GetConfigs() 
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
                   LanguageSettingConfig [] configs = GetConfigs();
                   if (Config == null) 
                   { 
                       return 0; 
                   }
   
                   return configs.Length; 
               } 
          };
}
