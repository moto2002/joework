using UnityEngine;
using System.Collections;

public class ShowFPS : MonoBehaviour
{
    /// <summary>
    /// 每次刷新计算的时间      帧/秒
    /// </summary>
    public float updateInterval = 0.5f;
    /// <summary>
    /// 最后间隔结束时间
    /// </summary>
    private double lastInterval;
    private int frames = 0;
    private float currFPS;


    // Use this for initialization
    void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }

    // Update is called once per frame
    void Update()
    {

        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval)
        {
            currFPS = (float)(frames / (timeNow - lastInterval));
            frames = 0;
            lastInterval = timeNow;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("FPS:" + currFPS.ToString("f2"));
    }

}