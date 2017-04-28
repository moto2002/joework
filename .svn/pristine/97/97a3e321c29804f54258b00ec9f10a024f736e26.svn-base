using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SetMaxFPS))]
public class ShowFPS : MonoBehaviour
{
    private static int count = 0;//用于控制帧率的显示速度的count
    private static float milliSecond = 0;//毫秒数
    private static float fps = 0;//帧率值
    private static float deltaTime = 0.0f;//用于显示帧率的deltaTime

    void OnGUI()
    {
        //左上方帧数显示
        if (++count > 30)
        {
            count = 0;
            milliSecond = deltaTime * 1000.0f;
            fps = 1.0f / deltaTime;
        }
        //string text = string.Format(" 当前每帧渲染间隔：{0:0.0} ms ({1:0.} 帧每秒)", milliSecond, fps);
        //GUILayout.Label(text);
        string text = string.Format("fps:{0}", (int)fps);
        GUI.Label(new Rect(10, 5, 50, 50), text);
    }

    void Update()
    {
        //帧数显示的计时delataTime
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }
}

