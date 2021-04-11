using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.Networking;


public class WebUtils : SingletonObject<WebUtils>
{

    /// <summary>
    /// 下载带泛型
    /// </summary>
    public  void Load<T>(string url , Action<T> endAction=null, Action<float> proAction=null) where T: UnityEngine.Object
    {
        if (!string.IsNullOrEmpty(url))
        {
            string path = PathTools.GetSavePath(url);
            if (!File.Exists(path))
            {
                StartCoroutine(DownLoadByUnityWebRequest<T>(url, endAction, proAction));
            }
            else//已在本地缓存  
            {
                StartCoroutine(DownLoadByUnityWebRequest<T>(path, endAction, proAction));
            }
        }
        else 
        {
            Debug.LogError("WebUtils error: url is null!");
        }
    }

    /// <summary>
    /// UnityWebRequest下载
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url">地址</param>
    /// <param name="type">下载类型</param>
    /// <param name="callback">结束回调</param>
    /// <param name="proAction">进度回调</param>
    /// <returns></returns>
    private IEnumerator DownLoadByUnityWebRequest<T>(string url, Action<T> callback = null,Action<float> proAction=null) where T : UnityEngine.Object
    {
        UnityWebRequest uwr = new UnityWebRequest(url);

        if (typeof(T) == typeof(AudioClip)) 
        {
            string _suf = PathTools.GetSuffix(url);
            AudioType at = AudioType.MPEG;
            if (_suf.Contains("ogg"))
                at = AudioType.OGGVORBIS;
            DownloadHandlerAudioClip downloadAudioClip = new DownloadHandlerAudioClip(url, at);
            uwr.downloadHandler = downloadAudioClip;
        }

        if (typeof(T) == typeof(Sprite)|| typeof(T) == typeof(Texture2D))
        {
            DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
            uwr.downloadHandler = downloadTexture;
        }

        uwr.SendWebRequest();
        while (true)
        {
            if (uwr.isNetworkError || uwr.isHttpError)
                break;

            if (uwr.isDone || uwr.downloadProgress >= 1) 
            {
                proAction?.Invoke(1);
                break;
            }
            else
                proAction?.Invoke(uwr.downloadProgress);
        }

        yield return uwr;

        if (!uwr.isNetworkError && !uwr.isHttpError)
        {
            if (typeof(T) == typeof(AudioClip))
            {
                AudioClip audioClip = ((DownloadHandlerAudioClip)uwr.downloadHandler).audioClip;
                callback.Invoke((T)(UnityEngine.Object)audioClip);
            }

            if (typeof(T) == typeof(Sprite))
            {
                Texture2D texture = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
                Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                callback?.Invoke((T)(UnityEngine.Object)sp);
            }

            if (typeof(T) == typeof(Texture2D))
            {
                Texture2D texture1 = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
                callback?.Invoke((T)(UnityEngine.Object)texture1);
            }

            //保存到本地
            if (url.Substring(0, 4) == "http")
            {
                File.WriteAllBytes(PathTools.GetSavePath(url), uwr.downloadHandler.data);
            }
        }
        else 
        {
            Debug.LogError("DownLoadByUnityWebRequest error:"+ uwr.error +"  url:"+url);
        }
    }


    /// <summary>
    /// 下载文件
    /// </summary>
    public void LoadFile(string url,string _suffix="", Action<string> endAction = null, Action<float> proAction = null,bool isweb=false) 
    {
        string path = PathTools.GetSavePath(url+_suffix);
        if (!File.Exists(path)|| isweb)
        {
            StartCoroutine(DownLoadFileByUnityWebRequest(url, _suffix, endAction, proAction));
        }
        else//已在本地缓存  
        {
            StartCoroutine(DownLoadFileByUnityWebRequest(path, _suffix, endAction, proAction));
        }
    }

    private IEnumerator DownLoadFileByUnityWebRequest(string url, string _suffix = "", Action<string> callback = null, Action<float> proAction = null) 
    {
       UnityWebRequest uwr =  UnityWebRequest.Get(url);
        uwr.SendWebRequest();

        while (true)
        {
            if (uwr.isNetworkError || uwr.isHttpError)
                break;

            if (uwr.isDone || uwr.downloadProgress >= 1)
            {
                proAction?.Invoke(1);
                break;
            }
            else
                proAction?.Invoke(uwr.downloadProgress);
        }

        yield return uwr;

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.LogError("DownLoadByUnityWebRequest error:" + uwr.error + "  url:" + url);
        }
        else 
        {
            callback?.Invoke(uwr.downloadHandler.text.ToString());
            //保存到本地
            if (url.Substring(0, 4) == "http")
            {
                File.WriteAllBytes(PathTools.GetSavePath(url + _suffix), uwr.downloadHandler.data);
            }
        }
    }

}
