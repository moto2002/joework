using UnityEngine;
using System.Collections;
using ShiHuanJue.Debuger;
using ShiHuanJue.Event;

public class GameManager
{
    float m_logicCurrTime = 0;
    int m_logicTimeFrame = 10;

    float m_viewCurrTime = 0;
    int m_viewTimeFrame = 60;

    private static GameManager m_instance;
    public static GameManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new GameManager();
            return m_instance;
        }
    }

    public GameManager()
    {
        ResourcesManager m_AssetBundleLoadManager = ResourcesManager.Instance;
        InputManager m_InputManager = InputManager.Instance;
        TimerManager m_TimerManager = TimerManager.Instance;
    }

    public void OnGameStart()
    {
        TTUIPageMgr.Instance.ShowPage<UILoading>(() =>
        {
            TimerManager.Instance.StartTimer(2, null, () =>
            {
                //VersionManager.Instance.Excute(this.VersionMangerCallBack);
                this.VersionMangerCallBack();
            });
        });
    }

    public void OnGameUpdate()
    {
        //模拟一下逻辑帧，到时候是由服务器驱动
        if (m_logicCurrTime == 0 || Time.realtimeSinceStartup - m_logicCurrTime >= ( 1.0f / (float)m_logicTimeFrame))
        {
            EventDispatcher.TriggerEvent(EventsConst.FrameWork.LogicUpdate);
            m_logicCurrTime = Time.realtimeSinceStartup;
        }

        //本地渲染帧
        if (m_viewCurrTime == 0 || Time.realtimeSinceStartup - m_viewCurrTime >= (1.0f / (float)m_viewTimeFrame))
        {
            EventDispatcher.TriggerEvent(EventsConst.FrameWork.Update);
            m_viewCurrTime = Time.realtimeSinceStartup;
        }
    }

    public void OnGameDestroy()
    {

    }

    public void OnGameQuit()
    {

    }

    public void VersionMangerCallBack()
    {
        TTUIPageMgr.Instance.HidePage<UILoading>();
        SceneLoadManager.Instance.GoToLoginScene();
    }

}
