using UnityEngine;
using System.Collections;

public class GameMain : MonoBehaviour
{
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

    void Start()
    {
        GameManager.Instance.OnGameStart();
	}

    void Update()
    {
        GameManager.Instance.OnGameUpdate();
    }

	void OnDestroy()
	{
        GameManager.Instance.OnGameDestroy();
	}

	void OnApplicationQuit()
	{
        GameManager.Instance.OnGameQuit();
	}
}
