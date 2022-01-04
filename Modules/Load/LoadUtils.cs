using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 加载工具
/// </summary>
public class LoadUtils
{

    /// <summary>
    ///  通过图集设置图片
    /// </summary>
    public static void SetChildImage(Image image, string url,string name)
    {
        Sprite[] sprs = Resources.LoadAll<Sprite>(url);

        if (sprs != null && sprs.Length > 0)
        {
            foreach (var item in sprs)
            {
                if (item.name == name)
                {
                    image.sprite = item;
                    return;
                }
            }
            Debug.LogError("SetChildImage url error:" + url);
        }
        else
        {
            Debug.LogError("SetChildImage url error:"+url);
        }
    }

    /// <summary>
    /// 设置图片
    /// </summary>
    /// <param name="url"> 地址</param>
    /// <param name="image"> 图片 </param>
    /// <param name="befor"> Resource下路径  填空直接再Resource中加载 </param>
    public static void SetImage(string url,Image image, string befor ="")
    {
        if (string.IsNullOrEmpty(url) || image == null)
        {
            return;
        }
        //网图
        if (url.Substring(0, 4) == "http")
        {
            WebUtils.Ins.Load<Sprite>(url, (sp) =>
            {
                image.sprite = sp;
            });
        }
        else //Resources加载
        {
            string[] strs = url.Split('.');
            if (strs.Length > 1) 
            {
                url = strs[0];
            }

            url = befor +"/"+ url;

            Sprite spri = ResourceLoader.Ins.Load<Sprite>(url);

            if (spri != null)
            {
                image.sprite = spri;
            }
            else
            {
                Debug.LogError("LoadUtils SetImage error: " + url);
            }
        }
    }

    /// <summary>
    /// 设置音频
    /// </summary>
    public static void SetAudio(string url, AudioSource audio, string befor = "")
    {
        if (string.IsNullOrEmpty(url) || audio == null)
        {
            return;
        }

        if (url.Substring(0, 4) == "http")
        {
            WebUtils.Ins.Load<AudioClip>(url, (clip) =>
            {
                audio.clip = clip;
                audio.Play();
            });
        }
        else
        {
            string[] strs = url.Split('.');
            if (strs.Length > 1) 
            {
                url = strs[0];
            }

            url = befor +"/"+ url;
            AudioClip clip = ResourceLoader.Ins.Load<AudioClip>(url);
            audio.clip = clip;
            audio.Play();
        }
    }



}
