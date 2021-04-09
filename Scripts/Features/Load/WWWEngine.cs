using System.Collections;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// 基于www封装，通讯下载
/// </summary>
public class WWWEngine : SingletonObject<WWWEngine>
{
    /// <summary>
    /// 下载带泛型
    /// </summary>
    public void Load<T>(string url, Action<T> endAction = null, Action<float> proAction = null) where T : UnityEngine.Object
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
                path = PathTools.GetLoadPath(url);
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
    private IEnumerator DownLoadByUnityWebRequest<T>(string url, Action<T> callback = null, Action<float> proAction = null) where T : UnityEngine.Object
    {
        WWW www = new WWW(url);

        while (true)
        {
            if (www.isDone || www.progress >= 1)
            {
                proAction?.Invoke(1);
                break;
            }
            else
                proAction?.Invoke(www.progress);
        }

        yield return www;

        if (string .IsNullOrEmpty( www.error))
        {
            if (typeof(T) == typeof(AudioClip))
            {
                AudioClip clip = www.GetAudioClip();
                callback?.Invoke((T)(object)clip);
            }

            if (typeof(T) == typeof(Sprite))
            {
                Texture2D texture = www.texture;
                Sprite m_sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                callback?.Invoke((T)(object)m_sprite);
            }

            if (typeof(T) == typeof(Texture2D))
            {
                Texture2D texture = www.texture;
                callback?.Invoke((T)(object)texture);
            }

            //保存到本地
            if (url.Substring(0, 4) == "http")
            {
                File.WriteAllBytes(PathTools.GetSavePath(url), www.bytes);
            }
        }
        else
        {
            Debug.LogError("DownLoadByUnityWebRequest error:" + www.error + "  url:" + url);
        }
    }


    /// <summary>
    /// 下载文件
    /// </summary>
    public void LoadFile(string url, Action<string> endAction = null, Action<float> proAction = null)
    {
        if (!string.IsNullOrEmpty(url))
        {
            string path = PathTools.GetSavePath(url);
            if (!File.Exists(path))
            {
                StartCoroutine(DownLoadFileByUnityWebRequest(url, endAction, proAction));
            }
            else//已在本地缓存  
            {
                path = PathTools.GetLoadPath(url);
                StartCoroutine(DownLoadFileByUnityWebRequest(path, endAction, proAction));
            }
        }
        else
        {
            Debug.LogError("WebUtils error: url is null!");
        }
    }

    private IEnumerator DownLoadFileByUnityWebRequest(string url, Action<string> callback = null, Action<float> proAction = null)
    {
        WWW www = new WWW(url);

        while (true)
        {
            if (www.isDone || www.progress >= 1)
            {
                proAction?.Invoke(1);
                break;
            }
            else
                proAction?.Invoke(www.progress);
        }

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            callback?.Invoke(www.text);

            //保存到本地
            if (url.Substring(0, 4) == "http")
            {
                File.WriteAllBytes(PathTools.GetSavePath(url), www.bytes);
            }
        }
        else
        {
            Debug.LogError("DownLoadByUnityWebRequest error:" + www.error + "  url:" + url);
        }
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="bytes"></param>
    public void CreatFile(string savePath, byte[] bytes )
    {
        FileStream fs = new FileStream(savePath, FileMode.Append);
        BinaryWriter bw = new BinaryWriter(fs);
        fs.Write(bytes, 0, bytes.Length);
        fs.Flush();     //流会缓冲，此行代码指示流不要缓冲数据，立即写入到文件。
        fs.Close();     //关闭流并释放所有资源，同时将缓冲区的没有写入的数据，写入然后再关闭。
        fs.Dispose();   //释放流
    }

}
