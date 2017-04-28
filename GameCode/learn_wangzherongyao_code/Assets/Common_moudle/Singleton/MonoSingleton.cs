using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool applicationIsQuitting = false;
    private static T _instance;
    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance " + typeof(T) +
                    " already destroyed on application quit." +
                    "Won't create again - returning null.");
                return null;
            }
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();
                        DontDestroyOnLoad(singleton);
                        Debug.Log("[Singleton] An instance of " + typeof(T) +
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " +
                            _instance.gameObject.name);
                    }
                }
                return _instance;
            }
        }
    }

    ///////////////////////////////////////////

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        Tick();
    }
    private void OnDestroy()
    {
        applicationIsQuitting = true;
        UnInit();
    }

    ///////////////////////////////////////////

    protected virtual void Init()
    {


    }

    protected virtual void UnInit()
    {


    }

    protected virtual void Tick()
    {


    }

}