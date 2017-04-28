using UnityEngine;
using System.Collections;

public class TestTimer : MonoBehaviour
{
    Timer timer;
    void Start()
    {
        //主循环得跑起来
        // timer = new Timer(-1, 0, () => {
        //    Debug.Log("tick");
        //});

        timer = new Timer(2, 3, () =>
        {
            Debug.Log("finish");
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(100, 100, 100, 50), "stop"))
    //    {
    //        timer.Stop();
    //    }
    //}
}
