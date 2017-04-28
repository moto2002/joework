using UnityEngine;
using System.Collections;

namespace LuaFramework
{
    public class Main : MonoBehaviour
    {
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);//防止销毁自己
            AppFacade.Instance.StartUp(this.gameObject);//启动游戏
        }

        void Update()
        { 
        
        }

        void OnApplicationQuit()
        { 
        
        }

        void OnDestroy()
        {
            AppFacade.Instance.Destroy();
        }
    }
}