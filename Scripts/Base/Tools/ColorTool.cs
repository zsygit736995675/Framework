using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 颜色工具
/// </summary>
public class ColorTool 
{

    public const string red1 = "#F7696F";
    public const string red2 = "#B84F54";
    public const string red3 = "#FE4B53";

    public const string pink1 = "#F67BCC";
    public const string pink2 = "#B74791";
    public const string pink3 = "#F169FD";

    public const string blue1 = "#51BEED";
    public const string blue2 = "#056F9F";
    public const string blue3 = "#7699FE";
    public const string blue4 = "#76CEFE";

    public const string green1 = "#84EF79";
    public const string green2 = "#4C9F43";
    public const string green3 = "#8CC584";
    public const string green4 = "#5C8B55";
    public const string green5 = "#A7C631";
    public const string green6 = "#7D971C";
    public const string green7 = "#00FF4E";//好友在线
    public const string green8 = "#05FB0B";//任务完成

    public const string yellow1 = "#FDCE04";
    public const string yellow2 = "#B2810C";
    public const string yellow3 = "#FDE145";//结算UI胜利

    public const string gray1 = "#B9B9B9";
    public const string gray2 = "#989898";
    
    public const string brown1= "#CA795F";

    public const string white= "#FFFFFF";


    //16位颜色转换为UnityColor 颜色
    public static  Color GetColor(string color)
    {
        if (color.Length == 0)
        {
            return Color.black;//设为黑色
        }
        else
        {
            //#ff8c3 除掉#
            color = color.Substring(1);
            int v = int.Parse(color, System.Globalization.NumberStyles.HexNumber);
            //转换颜色
            return new Color(
            //int>>移位 去低位
            //&按位与 去高位
            ((float)(((v >> 16) & 255))) / 255,
            ((float)((v >> 8) & 255)) / 255,
            ((float)((v >> 0) & 255)) / 255
            );
        }
    }





}
