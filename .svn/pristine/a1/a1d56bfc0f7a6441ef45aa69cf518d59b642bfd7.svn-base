using UnityEngine;
using System.Collections;

public class CTimerTest : MonoBehaviour
{
    void Start()
    {
        Singleton<CTimerManager>.GetInstance().AddTimer(1000, 1, new CTimer.OnTimeUpHandler(this.OnTimiPlayComplete));//-1 loop
    }

    private void OnTimiPlayComplete(int timerSequence)
    {
        Debug.Log("TODO");
    }
}
