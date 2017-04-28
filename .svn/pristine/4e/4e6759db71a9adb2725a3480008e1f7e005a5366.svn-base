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
        GameManager.Inst.GameStart();
	}

    void Update()
    {
        GameManager.Inst.GameUpdate();
    }

	void OnDestroy()
	{
        GameManager.Inst.GameDestroy();
	}

	void OnApplicationQuit()
	{
        GameManager.Inst.GameQuit();
	}
}
