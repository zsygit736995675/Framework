using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/// <summary>
/// 字符串工具类
/// </summary>
public class StrUtils
{

    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        Console.WriteLine(GetMd5Str1("hello"));//BC4B2A76B9719D91 
        Console.WriteLine(UserMd5("hello"));
    }

    /**//// <summary>
    /// MD5 16位加密 加密后密码为大写
    /// </summary>
    /// <param name="ConvertString"></param>
    /// <returns></returns>
    public static string GetMd5Str1(string ConvertString)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(ConvertString)), 4, 8);
        t2 = t2.Replace("-", "");
        return t2;
    }

    /**//// <summary>
    /// MD5  32位加密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string UserMd5(string str)
    {
        string cl = str;
        StringBuilder pwd = new StringBuilder();
        MD5 md5 = MD5.Create();//实例化一个md5对像
                               // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
        // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
        for (int i = 0; i < s.Length; i++)
        {
            // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
            pwd.Append(s[i].ToString("X2"));
            //pwd = pwd + s[i].ToString("X");

        }
        return pwd.ToString();
    }


    /// <summary>
    /// Base64解密
    /// </summary>
    /// <param name="result">待解密的密文</param>
    /// <returns>解密后的字符串</returns>
    public static string Base64Decode(string result)
    {
        string decode = string.Empty;
        byte[] bytes = System.Convert.FromBase64String(result);
        try
        {
            decode = Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            decode = result;
        }
        return decode;
    }


    /// <summary>
    /// Base64加密
    /// </summary>
    /// <param name="result">明文</param>
    /// <returns>密文</returns>
    public static string Base64Encode(string result)
    {
        string decode = string.Empty;
        byte[] bytes = System.Text.Encoding.Default.GetBytes(result);
        decode = System.Convert.ToBase64String(bytes);
        try
        {
            decode = get_UTF8(decode);
        }
        catch
        {
            decode = result;
        }
        return decode;
    }

    /// <summary>
    /// 字符串转UTF-8   
    /// </summary>
    /// <param name="unicodeString"></param>
    /// <returns></returns>
    public static string get_UTF8(string unicodeString)
    {
        UTF8Encoding utf8 = new UTF8Encoding();
        System.Byte[] encodedBytes = utf8.GetBytes(unicodeString);
        System.String decodedString = utf8.GetString(encodedBytes);
        return decodedString;
    }


    /// <summary>
    /// 通过id获取字符串
    /// </summary>
    public static string GetStr(int id)
    {
        StrConfig con = StrConfig.Get(id);
        if (con != null)
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    return con.en;
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                case SystemLanguage.ChineseTraditional:
                    return con.zh;
                case SystemLanguage.Spanish:
                    return con.sp;
                case SystemLanguage.German:
                    return con.ge;
            }
            return con.zh;
        }
        else
        {
            Debug.LogError("GetStr error:" + "ID：" + id);
            return "";
        }
    }

    /// <summary>
    /// 字符串解析数组
    /// </summary>
    public static int[] GetIntArr(string str)
    {
        str = str.Replace('[', ' ');
        str = str.Replace(']', ' ');

        str = str.Trim();

        string[] strs = str.Split(',');
        if (strs != null && strs.Length > 0)
        {

            int[] tArr = new int[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                tArr[i] = int.Parse(strs[i]);
            }
            return tArr;
        }
        return null;
    }

    /// <summary>
    /// 字符串解析数组
    /// </summary>
    public static float[] GetFloatArr(string str)
    {
        str = str.Replace('[', ' ');
        str = str.Replace(']', ' ');

        str = str.Trim();

        string[] strs = str.Split(',');
        if (strs != null && strs.Length > 0)
        {

            float[] tArr = new float[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                tArr[i] = float.Parse(strs[i]);
            }
            return tArr;
        }
        return null;
    }


    /// <summary>
    /// int转数组型字符串
    /// </summary>
    public static string IntToArr(int p1, int p2, int p3)
    {
        return "[" + p1 + "," + p2 + "," + p3 + "]";
    }

    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <returns></returns>
    public static string GetTimeStamp()
    {
        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds).ToString();
    }

    /// <summary>
    /// 获取当前日期时间yyyyMMddHHmmss
    /// </summary>
    /// <returns></returns>
    public static string GetDtStr()
    {
        DateTime dt = DateTime.Now;
        return string.Format("{0:yyyyMMddHHmmss}", dt);
    }

    /// <summary>
    /// 毫秒值转时分秒
    /// </summary>
    /// <param name="mss"></param>
    /// <returns></returns>
    public static String formatDuring(long mss)
    {
        //long days = mss / (1000 * 60 * 60 * 24);
        string hours = (mss % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60) + "";
        string minutes = (mss % (1000 * 60 * 60)) / (1000 * 60) + "";
        string seconds = (mss % (1000 * 60)) / 1000 + "";

        if (hours.Length < 2)
        {
            hours = 0 + hours;
        }
        if (minutes.Length < 2)
        {
            minutes = 0 + minutes;
        }
        if (seconds.Length < 2)
        {
            seconds = 0 + seconds;
        }
        return hours + ":" + minutes + ":" + seconds;
    }

    /// <summary>
    /// 秒转时分秒
    /// </summary>
    /// <param name="mss"></param>
    /// <returns></returns>
    public static String formatDuring(int mss)
    {
        //long days = mss / (1000 * 60 * 60 * 24);
        string hours = (mss % (1 * 60 * 60 * 24)) / (1 * 60 * 60) + "";
        string minutes = (mss % (1 * 60 * 60)) / (1 * 60) + "";
        string seconds = (mss % (1 * 60)) / 1 + "";

        if (hours.Length < 2)
        {
            hours = 0 + hours;
        }
        if (minutes.Length < 2)
        {
            minutes = 0 + minutes;
        }
        if (seconds.Length < 2)
        {
            seconds = 0 + seconds;
        }
        return hours + ":" + minutes + ":" + seconds;
    }
  
    /// <summary>
    /// 去掉url中的转义字符\
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static String GetUrl(string url)
    {
        string res = null;
        string[] array = url.Split('\\');
        for(int i = 0; i < array.Length; i++)
        {
            res += array[i];
        }
        return res;
    }

    /// <summary>
    /// 获取text的文本长度
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int GetFontlen(string str)
    {
        int len = 0;
        Font font;
        font = Font.CreateDynamicFontFromOSFont("Arial", 25);
        font.RequestCharactersInTexture(str);
        for (int i = 0; i < str.Length; i++)
        {
            CharacterInfo ch;
            font.GetCharacterInfo(str[i], out ch);
            len += ch.advance;
        }
        return len;
    }
}
