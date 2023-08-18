using UnityEngine;

/// <summary>
/// 一般为提前放置好的预制体使用
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonGetMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if( null == instance )
                instance = Transform.FindObjectOfType<T>();

            if( null == instance )
            {
                GameObject obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent<T>();
            }
            
            return instance;
        }
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<T>();
        }
        
        OnAwake();
    }
    
    private void Start()
    {
        OnStart();
    }

    public virtual void OnAwake() { }

    public virtual void OnStart() { }
}