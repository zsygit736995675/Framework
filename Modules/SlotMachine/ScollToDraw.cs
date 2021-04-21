using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class ScollToDraw : MonoBehaviour
{
    // 抽奖按钮
    public Button DrowBtn;

    // 奖励图片
    private Image[] ArardImgArr;

    // 转盘速度
    private float AniMoveSpeed = 8f;

    /// <summary>
    /// 图片间隔
    /// </summary>
    public int intervalDis = 120;

    /// <summary>
    /// 进度
    /// </summary>
    private float[] progress;

    /// <summary>
    /// 位置
    /// </summary>
    private Vector3[] AniPosV3;

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


    void Start()
    {
        DrowBtn.onClick.AddListener(DrawFun);

    }

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
            ArardImgArr[i].transform.localPosition = new Vector3(0, index * intervalDis, 0);
            progress[i] = i;
            AniPosV3[i] = new Vector3(0, y);
        }
        progress[progress.Length - 1] = progress.Length;
        AniPosV3[AniPosV3.Length - 1] = new Vector3(0, (mid - ArardImgArr.Length) * intervalDis);
    }

    void Update()
    {
        if (isStopUpdatePos) return;

        float t = Time.deltaTime * AniMoveSpeed;
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
            //保留其小数部分,该位置进度归零
            progress[i] -= index;

            //中间的格子置底，说明第一个格子在中间
            bool isFirst = ArardImgArr.Length % 2 == 0 ? i == mid : i == mid + 1;
            if (isFirst && isAutoStop)
            {
                isStopUpdatePos = true;



            }
            //该格子置顶
            return new Vector3(0, mid * intervalDis);
        }
        else
        {
            //移动向下一个
            return Vector3.Lerp(AniPosV3[index], AniPosV3[index + 1], progress[i] - index);
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
