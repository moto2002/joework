using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Framework
{
    public class GameFramework : MonoSingleton<GameFramework>
    {
        protected override void Init()
        {
            CTimerManager.GetInstance();
        }

        protected override void UnInit()
        {
            CTimerManager.DestroyInstance();
        }

        protected override void Tick()
        {
            CTimerManager.GetInstance().Update();
        }
    }
}

