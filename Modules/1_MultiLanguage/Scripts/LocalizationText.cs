//
// using UnityEngine;
// using UnityEngine.UI;
// /// <summary>
// /// Custom Text Control used for localization text.
// /// </summary>
// [AddComponentMenu("UI/LocalizationText", 10)]
// public class LocalizationText : Text
// {
//
//     /// <summary>
//     /// 文本的key
//     /// </summary>
//     public int Key;
//
//     /// <summary>
//     /// 自定义字体，方便后期替换
//     /// </summary>
//    // public UIFont CustomFont;
//
//     /// <summary>
//     /// 是否开启自身的本地化
//     /// </summary>
//     public bool IsOpenLocalize = true;
//
//     /// <summary>
//     /// 参数
//     /// </summary>
//     object[] param;
//
//
//     protected override void Awake()
//     {
//         base.Awake();
//         if( CustomFont != null )
//         {
//             font = CustomFont.UseFont;
//         }
//         OnLocalize();
//     }
//
//     protected override void OnEnable()
//     {
//         base.OnEnable();
//         if( IsOpenLocalize )
//         {
//             LanguageManager.OnLocalize += OnLocalize;
//         }
//     }
//
//     protected override void OnDisable()
//     {
//         base.OnDisable();
//         if( IsOpenLocalize )
//         {
//             if(LanguageManager.OnLocalize != null )
//             {
//                 LanguageManager.OnLocalize -= OnLocalize;
//             }
//         }
//     }
//
//     /// <summary>
//     /// 重新本地化，用于游戏内切换语言时调用
//     /// </summary>
//     public void OnLocalize()
//     {
//         if( IsOpenLocalize )
//         {
//             if (param == null || param.Length == 0) 
//             {
//                 text = LanguageManager.GetStr(Key);
//                 return;
//             }
//             text = LanguageManager.GetStr(Key,param);
//         }
//     }
//
//     public void SetText(int key,params object [] param) 
//     {
//         this.Key = key;
//         this.param = param;
//         OnLocalize();
//     }
//
//     public void SetText(object [] param)
//     {
//         this.param = param;
//         OnLocalize();
//     }
//
// }