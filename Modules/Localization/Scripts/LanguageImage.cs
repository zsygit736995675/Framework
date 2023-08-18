using UnityEngine;
using UnityEngine.UI;

public class LanguageImage : MonoBehaviour
{
    /// <summary>
    /// 图片key
    /// </summary>
    public string Key;

    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        OnLocalize();
    }

    protected void OnEnable()
    {
        LanguageManager.Instance.OnLocalize += OnLocalize;
    }

    protected  void OnDisable()
    {
        LanguageManager.Instance.OnLocalize -= OnLocalize;
    }

    public void ChangeKey(string key) 
    {
        this.Key = key;
        OnLocalize();
    }

    /// <summary>
    /// 设置图片
    /// </summary>
    public void OnLocalize()
    {
        //  Sprite sprite = LanguageManager.Instance.GetSprite(Key);
        // if (sprite != null && image != null) 
        // {
        //     image.sprite = sprite;
        //     image.SetNativeSize();
        // }
        // else
        // {
        // }
    }
}
