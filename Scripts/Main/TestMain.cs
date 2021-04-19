using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TestMain : MonoBehaviour
{
    [SerializeField]
    [Header("服务器")]
    private HttpServerAddress httpAddress;

    string imgurl = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1604384804733&di=98230c7dd043f0918256b23a6dffad12&imgtype=0&src=http%3A%2F%2Fpic.90sjimg.com%2Fdesign%2F00%2F47%2F65%2F93%2F5933c7566284a.png";
    string apkurl = "http://miner.munimob.com/pages/miner.apk";
    string audurl = "http://s.aigei.com/src/aud/mp3/3d/3d4d6bafeab4417fac302c56f9d4e74b.mp3?download/%E8%BD%BB%E6%9D%BE%E6%AC%A2%E5%BF%AB%E7%A7%AF%E6%9E%81%E5%90%91%E4%B8%8A%E7%9A%84%E8%BD%BB%E9%9F%B3%E4%B9%90-%E4%B8%BA%E5%BF%83%E6%83%85%E5%85%85%E7%94%B5_%E7%88%B1%E7%BB%99%E7%BD%91_aigei_com.mp3&e=1604679900&token=P7S2Xpzfz11vAkASLTkfHN7Fw-oOZBecqeJaxypL:IZgBTQE3Kb7QBzo3FAyDRADoNM4=";
    string txturl = "http://brother_cn.munimob.com/api/res/StrConfig";

    private void Awake()
    {
        Demo();
        Init();
    }


    void Demo() 
    {
        //事件
        GameEvent gam = new GameEvent();
        EventMgrHelper.Ins.PushEvent(EventDef.Callback, strData0: "这是个事件");

        //表格部分
        Debug.Log(StrConfig.Get(1).zh); 
        Debug.Log(UnitConfig.Get(5).directory[0]); ;
        Debug.Log(StrConfig.Datas.Length);
        Debug.Log(StrConfig.Version);

        //下载部分
        LoadUtils.SetImage(imgurl, GetComponentInChildren<Image>(), "Texture");
        LoadUtils.SetAudio("2147463780", GetComponent<AudioSource>(), "Source");
        WebUtils.Ins.LoadFile(apkurl);
        WebUtils.Ins.LoadFile(txturl, ".txt", (str) => { Debug.Log(str); });

        //ui部分
        APP.Ins.Init(this);

        HTTP.Ins.httpAddress = httpAddress;

        HTTP.Ins.HttpAsyncGet(HTTPConst.getversion, (str) => {

            Debug.Log("HttpAsyncGet:" + str);
        });

    }

    void Init()
    {

    }

  
}
