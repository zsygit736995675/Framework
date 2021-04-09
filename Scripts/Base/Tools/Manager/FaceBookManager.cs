﻿//using System;
//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;
//using Facebook.Unity;
//using Facebook.MiniJSON;

//public class FaceBookManager
//{
//    // public delegate void OnFBLoginSucced(Facebook.Unity.AccessToken token);
//    public delegate void OnFBLoginFaild(bool isCancel, string errorInfo);

//    public delegate void OnFBShareLinkSucced(string postId);
//    public delegate void OnFBShareLinkFaild(bool isCancel, string errorInfo);

//    public delegate void OnGotFBFriendInGame(string resultJsonStr);

//    public delegate void OnGotFBMyInfo(string resultJsonStr);
//    public delegate void OnGetFBInfoFaild();

//    internal static void GetMyInfo(Action<string> p, object v)
//    {
//        throw new NotImplementedException();
//    }

//    public delegate void OnFBInvitedSucceed(string resultJsonStr);

//    private static string appLinkUrl;

//    /// <summary>
//    /// 初始化
//    /// </summary>
//    public static void Init(Action action)
//    {
//        if (!FB.IsInitialized)
//        {
//            FB.Init(() =>
//            {

//                if (FB.IsInitialized)
//                {
//                    // Signal an app activation App Event
//                    FB.ActivateApp();
//                    // Continue with Facebook SDK
//                    // ...
//                    //FBGetAPPLinkUrl();

//                    action?.Invoke();
//                }
//                else
//                {
//                    Debug.Log("Failed to Initialize the Facebook SDK");
//                }
//            }, OnHideUnity);
//        }
//        else
//        {
//            FB.ActivateApp();

//            if (action != null)
//            {
//                action();
//            }
//        }
//    }

//    private static void OnHideUnity(bool isGameShown)
//    {
//        if (!isGameShown)
//        {
//            // Pause the game - we will need to hide
//            Time.timeScale = 0;
//        }
//        else
//        {
//            // Resume the game - we're getting focus again
//            Time.timeScale = 1;
//        }
//    }

//    /// <summary>
//    /// 登录
//    /// </summary>
//    public static void LoginResult(Action action)
//    {
//        List<string> perms = new List<string>() { "public_profile", "email" };
//        FB.LogInWithReadPermissions(perms, (result) =>
//        {
//            if (FB.IsLoggedIn)
//            {
//                // AccessToken class will have session details
//                var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;

//                if (action != null)
//                {
//                    action();
//                }
//            }
//            else
//            {

//                Debug.Log("User cancelled login");
//            }

//        });
//    }

//    public static void EventLogin( )
//    {
//        Dictionary<string, object> pairs = new Dictionary<string, object>();

//        //pairs[Facebook.Unity.AppEventParameterName.RegistrationMethod] = PlayerManager.Ins.Player.Login_Type;
//        pairs[Facebook.Unity.AppEventParameterName.Currency] = "HKD";

//        if (!FB.IsInitialized)
//        {
//            FB.Init(() =>
//            {
//                Debug.Log("EventLog: Login  facebook Init");
//                FB.LogAppEvent(Facebook.Unity.AppEventName.CompletedRegistration, PlayerPrefsManager.LoginCount ,pairs);
//            });
//        }
//        else
//        {
//            Debug.Log("EventLog: Login  facebook");
//            FB.LogAppEvent(Facebook.Unity.AppEventName.CompletedRegistration, PlayerPrefsManager.LoginCount, pairs);
//        }
//    }

//    public static void EventLevel(int level)
//    {
//        if (!FB.IsInitialized)
//        {
//            FB.Init(() =>
//            {
//                Debug.Log("EventLog: Level  facebook Init");
//                FB.LogAppEvent(Facebook.Unity.AppEventName.AchievedLevel,level);
//            });
//        }
//        else
//        {
//            Debug.Log("EventLog: Level  facebook ");
//            FB.LogAppEvent(Facebook.Unity.AppEventName.AchievedLevel,level);
//        }
//    }


//    public static void Share()
//    {
//       // UpdateManager.Ins.verData.data.iosId = "1459965214";

////        if (PlayerManager.Ins.IsFaceBook)
////        {
////#if UNITY_ANDROID && !UNITY_EDITOR
////		   FB.ShareLink(new Uri("https://play.google.com/store/apps/details?id=" + UpdateManager.Ins.verData.data.googleId), callback: ShareCallback);
////#elif UNITY_IOS && !UNITY_EDITOR
////            FB.ShareLink(new Uri("https://apps.apple.com/us/app/face-meme-emoji-gif-maker/id" + UpdateManager.Ins.verData.data.iosId), callback: ShareCallback);
////#endif
////        }
////        else
////        {
////            PlayerManager.Ins.FackBookInit(() =>
////            {
////                PlayerManager.Ins.Player.Inquire();
////#if UNITY_ANDROID && !UNITY_EDITOR
////		   FB.ShareLink(new Uri("https://play.google.com/store/apps/details?id=" + UpdateManager.Ins.verData.data.googleId), callback: ShareCallback);
////#elif UNITY_IOS && !UNITY_EDITOR
////           FB.ShareLink(new Uri("https://apps.apple.com/us/app/face-meme-emoji-gif-maker/id" + UpdateManager.Ins.verData.data.iosId), callback: ShareCallback);
////#endif
////            });
////        }
//    }


//    private static void ShareCallback(IShareResult result)
//    {
//        if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
//        {
//            Debug.Log("ShareLink Error: " + result.Error);
//        }
//        else if (!String.IsNullOrEmpty(result.PostId))
//        {
//            // Print post identifier of the shared content
//            Debug.Log(result.PostId);
//        }
//        else
//        {
//            // Share succeeded without postID
//            Debug.Log("ShareLink success!");
//        }
//    }

//    /// <summary>   
//    /// 邀请
//    /// </summary>
//    public static void Invite()
//    {
//        //string message, IEnumerable<string> to = null, IEnumerable<object> filters = null, IEnumerable<string> excludeIds = null, int? maxRecipients = default(int?), string data = "", string title = "", FacebookDelegate<IAppRequestResult> callback = null
//        // FB.AppRequest(APP.facebookID,null/*to*/,null/**/,null);

//        //FB.AppRequest("Come play this great game!", null, null, null, null, null, null, 
//        //        delegate (IAppRequestResult result) {
//        //            Debug.Log(result.RawResult); Debug.Log(result.RawResult);
//        //});
//    }

//    /// <summary>
//    /// 获取自己的信息
//    /// </summary>
//    /// <param name="onGotFBMyInfo"></param>
//    public static void GetMyInfo(OnGotFBMyInfo onGotFBMyInfo = null, OnGetFBInfoFaild onGetFaild = null)
//    {
//        //Logger.LogUI("GetMyInfo");
//        //没有授权
//        if (FB.IsLoggedIn == false)
//        {
//            if (onGetFaild != null)
//            {
//                onGetFaild();
//            }
//            return;
//        }

//        //Logger.LogUI("API");
//        FB.API("me?fields=id,name,picture", HttpMethod.GET, (result) =>
//        {
//            //Logger.LogUI(result.RawResult);
//            if (onGotFBMyInfo != null)
//            {
//                onGotFBMyInfo(result.RawResult);
//            }
//        });
//    }

//    /// <summary>
//    ///  获取APPLink, 获取失败，TODO
//    /// </summary>
//    public static void FBGetAPPLinkUrl()
//    {
//        FB.GetAppLink((result) =>
//        {
//            Debug.Log(result.RawResult);
//            Debug.Log("Ref: " + result.Ref);
//            Debug.Log("TargetUrl: " + result.TargetUrl);
//            Debug.Log("Url: " + result.Url);
//            appLinkUrl = result.Url;
//            //Logger.LogUI(appLinkUrl);
//        });
//    }

//}


///// <summary>
///// 图片信息
///// </summary>
//public class PictureData
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public int height { get; set; }
//    /// <summary>
//    /// 
//    /// </summary>
//    public bool is_silhouette { get; set; }
//    /// <summary>
//    /// 
//    /// </summary>
//    public string url { get; set; }
//    /// <summary>
//    /// 
//    /// </summary>
//    public int width { get; set; }
//}

//public class Picture
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public PictureData data { get; set; }
//}

///// <summary>
///// facebook用过户信息
///// </summary>
//public class FBUserInfo
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public string id { get; set; }
//    /// <summary>
//    /// 
//    /// </summary>
//    public string name { get; set; }
//    /// <summary>
//    /// 
//    /// </summary>
//    public Picture picture { get; set; }
//}