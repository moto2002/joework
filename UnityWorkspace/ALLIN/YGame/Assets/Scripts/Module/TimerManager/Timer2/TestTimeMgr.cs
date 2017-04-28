using UnityEngine;
using System.Collections;

public class TestTimeMgr : MonoBehaviour
{
    void Start()
    {
        //GameObject go1 = new GameObject("go1");
        //TimerHandler  tm1 = go1.AddComponent<TimerHandler >();
        //tm1.StartTimer(5, (x) => { Debug.Log("tick"); }, () =>
        //{
        //    Debug.Log("finish");
        //});

        //GameObject go2 = new GameObject("go2");
        //TimerHandler  tm2 = go2.AddComponent<TimerHandler >();
        //tm2.StartTimer(3, (x) => { Debug.Log("tick1"); }, () =>
        //{
        //    Debug.Log("finish1");
        //});

        TimerManager.Instance.StartTimer(5, (x) => { Debug.Log("time1:"+ x); }, () =>
        {
            Debug.Log("finish1");
        });

        TimerManager.Instance.StartTimer(3, (x) => { Debug.Log("timer2:" + x); }, () =>
          {
              Debug.Log("finish2");
          });
    }

    void Update()
    {

    }
}
