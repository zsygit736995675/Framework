using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class ScollToDraw : MonoBehaviour
{
    
    /// <summary>
    /// 图片集合
    /// </summary>
    private Image[] ArardImgArr;

    /// <summary>
    /// 抽奖进度
    /// </summary>
    private float[] progress;

    /// <summary>
    /// 位置
    /// </summary>
    private Vector3[] AniPosV3;

    /// <summary>
    /// 转盘速度
    /// </summary>
    private float AniMoveSpeed = 8f;

    /// <summary>
    /// 图片间隔
    /// </summary>
    private int intervalDis = 120;

    /// <summary>
    /// 中间位置
    /// </summary>
    private int mid;

    /// <summary>
    /// 停止标识,等待摇奖结束
    /// </summary>
    private bool isAutoStop = true;

    /// <summary>
    /// 摇奖结束
    /// </summary>
    private bool isStopUpdatePos = true;


    /// <summary>
    /// 初始化位置
    /// </summary>
    private void Reset()
    {
        ArardImgArr = transform.GetComponentsInChildren<Image>();

        mid = ArardImgArr.Length / 2;

        progress = new float[ArardImgArr.Length + 1];
        AniPosV3 = new Vector3[ArardImgArr.Length + 1];

        for (int i = 0; i < ArardImgArr.Length; i++)
        {
            int index = mid - i;
            float y = index * intervalDis;
            //初始化图片位置
            ArardImgArr[i].transform.localPosition = new Vector3(0, index * intervalDis, 0);
            progress[i] = i;
            AniPosV3[i] = new Vector3(0, y);
        }
        //图片向下移动，需要多出来一个缓冲位置
        progress[progress.Length - 1] = progress.Length;
        AniPosV3[AniPosV3.Length - 1] = new Vector3(0, (mid - ArardImgArr.Length) * intervalDis);

        //ResetImage();
    }

    /// <summary>
    /// 想要中奖的位置
    /// </summary>
    int redIndex;
    /// <summary>
    /// 排除的位置
    /// </summary>
    int bigIndex;

    float deltaTime;
    void ResetImage()
    {
        //因为deltaTime会不稳定，所以保存一个稳定的帧率
        deltaTime = Time.deltaTime;
        List<Sprite> sprites = new List<Sprite>(Resources.LoadAll<Sprite>("Texture/Ui/Lottery"));
        for (int i = 0; i < ArardImgArr.Length; i++)
        {
            int index = UnityEngine.Random.Range(0, sprites.Count);
            
            if (sprites[index].name == "6")
                redIndex = i;
            if (sprites[index].name == "5")
                bigIndex = i;

            ArardImgArr[i].sprite = sprites[index];
            ArardImgArr[i].SetNativeSize();
            sprites.RemoveAt(index);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            DrawFun();
        }

        if (isStopUpdatePos) return;

        float t = deltaTime * AniMoveSpeed;
        for (int i = 0; i < ArardImgArr.Length; i++)
        {
            progress[i] += t;
            ArardImgArr[i].transform.localPosition = MovePosition(i);
        }
    }

    // 获取下一个移动到的位置
    Vector3 MovePosition(int i)
    {
        int index = Mathf.FloorToInt(progress[i]);

        //置底
        if (index > progress.Length - 2)
        {
            CheckEnd2(i, index);

            //该格子置顶
            return new Vector3(0, mid * intervalDis);
        }
        else
        {
            //移动向下一个
            return Vector3.Lerp(AniPosV3[index], AniPosV3[index + 1], progress[i] - index);
        }
    }

    void CheckEnd2(int i, int index) 
    {
        //保留其小数部分,该位置进度归零
        progress[i] -= index;

        //中间的格子置底，说明第一个格子在中间
        bool isFirst = ArardImgArr.Length % 2 == 0 ? i == mid : i == mid + 1;
        if (isFirst && isAutoStop)
        {
            isStopUpdatePos = true;



        }
    }

    void CheckEnd1(int i,int index) 
    {
        int target = ArardImgArr.Length % 2 == 0 ? redIndex + mid : redIndex + mid + 1;
        target = target > ArardImgArr.Length - 1 ? target - ArardImgArr.Length : target;
        bool isFirst = target == i;
       

        //保留其小数部分,该位置进度归零
        progress[i] -= index;

        if (isFirst && isAutoStop)
        {
            isStopUpdatePos = true;

        }
    }

    /// <summary>
    /// 点击抽奖
    /// </summary>
    void DrawFun()
    {
        Reset();
        isAutoStop = false;
        isStopUpdatePos = false;
        StartCoroutine(SetMoveSpeed());
    }

    // 抽奖动画速度控制
    IEnumerator SetMoveSpeed()
    {
        AniMoveSpeed = 12;
        yield return new WaitForSeconds(2);

        AniMoveSpeed = 7;
        yield return new WaitForSeconds(1);
        isAutoStop = true;
    }
}
