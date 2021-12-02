using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;


public class HTTP : BaseClass<HTTP>
{
    /// <summary>
    /// http服务器
    /// </summary>
    public HttpServerAddress httpAddress;

    /// <summary>
    /// 创建一个异步HTTP Post请求
    /// </summary>
    /// <param name="url">URL.</param>
    /// <param name="callback">Callback</param>
    public void HttpAsyncPost(string str, string json, Action<string> callback = null,bool isAdd=false)
    {
            Coroutiner.Start(Post(str,json ,callback,isAdd));       
    }

    IEnumerator Post(string str, string json, Action<string> callback = null, bool isAdd = false)
    {

        string url = GetSeverUrl(str);
        // UnityEngine.Debug.Log("HttpAsyncPost:" + url+json);

        HTTPRequest client = new HTTPRequest(url, "POST", 5000, (HTTPResponse response) =>
        {
            if (null != callback)
            {
                // UnityEngine.Debug.Log("HttpAsyncPost Callback:"+" type:"  + " path:" + str + " json:" + json+ " response:"+ response.GetResponseText());
                callback(response.GetResponseText());
                callback = null;
            }
        });

        string jsonStr = client.SetPostData(json);

        string signature = StrUtils.UserMd5("ck@2019" + jsonStr);
        string ts = Time.realtimeSinceStartup.ToString();
        string token = StrUtils.UserMd5("ck@2019" + ts);

        client.Start(isAdd, signature, ts, token);

        yield return client;
    }


    /// <summary>
    /// 创建一个异步HTTP Get请求
    /// </summary>
    /// <param name="url">URL.</param>
    /// <param name="callback">Callback</param>
    public void HttpAsyncGet( string str, Action<string> callback = null)
    {
        Coroutiner.Start(Get(str,callback)) ;
    }

    IEnumerator Get(string str, Action<string> callback = null)
    {
        string url = GetSeverUrl(str);

        HTTPRequest client = new HTTPRequest(url, "Get", 5000, (HTTPResponse response) =>
        {
            if (null != callback)
            {
                callback(response.GetResponseText());
            }
        });
        client.Start();
        yield  return client;
    }

    /// <summary>
    /// 同步POST
    /// </summary>
    /// <returns></returns>
    public string HttpPost( string str, string json)
    {
        string url = GetSeverUrl(str);
        Debug.Log(url);
        return new WebRequestSugar().HttpPost(url, json);
    }

    /// <summary>
    /// 同步GET
    /// </summary>
    /// <returns></returns>
    public string HttpGet( string str)
    {
        string url = GetSeverUrl( str);
        return new WebRequestSugar().HttpGet(url);
    }

    public static string getFileHash(string filePath)
    {
        try
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            int len = (int)fs.Length;
            byte[] data = new byte[len];
            fs.Read(data, 0, len);
            fs.Close();
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string fileMD5 = "";
            foreach (byte b in result)
            {
                fileMD5 += Convert.ToString(b, 16);
            }
            return fileMD5;
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e.Message);
            return "";
        }
    }

    public string GetSeverUrl(string str)
    {
        StringBuilder sb = new StringBuilder();

        if (httpAddress == HttpServerAddress.Release)
        {
            sb.Append("http://brother_cn.munimob.com/api");
        }

        if (httpAddress == HttpServerAddress.Intranet)
        {
            sb.Append("http://brothers.gogameclub.com/api");
        }

        if (httpAddress == HttpServerAddress.Debug)
        {
            sb.Append("http://127.0.0.1:2341");
        }
        sb.Append(str);
        Debug.Log("GetSeverUrl:" + sb);
        return sb.ToString();
    }


}
